using Anyhandy.DataProvider.EFCore.Context;
using Anyhandy.DataProvider.EFCore.Models;
using Anyhandy.Interface;
using Anyhandy.Interface.Packages;
using Anyhandy.Interface.User;
using Anyhandy.Models.DTOs;
using Anyhandy.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Anyhandy.Services.Users
{
    public class JobPostService : IJobPost
    {

        private readonly IFileStorage _storageService;

        public JobPostService(IFileStorage storageService)
        {
            _storageService = storageService;
        }

        public void submitJobPost(MainServiceDTO jobDTO, int userID)
        {
            using (var mainServiceContext = new AnyHandyDBContext<MainService1>())
            {
                int mainServiceId = mainServiceContext.MainServices1
                    .FirstOrDefault(p => p.MainServiceName.Contains(jobDTO.MainServiceName)).MainServiceId;

                using (var jobContext = new AnyHandyDBContext<Job>())
                {
                    try
                    {
                        var jobEntity = new Job
                        {
                            MainServicesId = mainServiceId,
                            PostedDate = DateTime.UtcNow,
                            Amount = jobDTO.JobDetails.FixedPriceAmount == string.Empty ? null : int.Parse(jobDTO.JobDetails.FixedPriceAmount),                     
                            Status = 1,
                            JobTitle = "No details",
                            JobDetails = jobDTO.MainServiceDescription,
                            UserId = userID,
                            DueDate = jobDTO.JobDetails.EndDate,
                            JobDue = jobDTO.JobDetails.EndDate,
                            JobExpectedEnd = jobDTO.JobDetails.EndDate
                        };

                        jobContext.Save(jobEntity);
                        var recentJobId = jobEntity.JobId;
                        if (jobDTO.Location.AddressType != null)
                        {
                            var userAddressContext = new AnyHandyDBContext<UserAddress>();
                            // Create a new Job entity instance
                            var userAddressEntity = new UserAddress
                            {
                                AddressType = jobDTO.Location.AddressType,
                                //City = jobDTO.Location.City,
                                Details = jobDTO.Location.Details,
                                UserId = userID                         // need to userOriginalValue
                            };
                            userAddressContext.Save(userAddressEntity);
                        }


                        foreach (var subServiceDTO in jobDTO.SubServices)
                        {
                            // Get sub-service details
                            int subServiceId = mainServiceContext.SubServices
                                .FirstOrDefault(p => p.ServiceNameEn.Contains(subServiceDTO.Name)).SubServicesId;

                         

                            Dictionary<string, Type> serviceTableMap = new Dictionary<string, Type>()
                            {
                                { "Home Cleaning", typeof(TblHomeCleaning) },
                                { "Electrical Service", typeof(TblElectricalService) },
                                { "Painting", typeof(TblPaintingService) },
                                { "Landscaping and Lawn Care", typeof(TblLandscapingService) },
                                { "Pest Control", typeof(TblPestControlService) },
                                { "Plumbing", typeof(TblPlumbingService) },
                                { "HVAC", typeof(TblHvacService) },
                                { "Carpentry Service", typeof(TblCarpentryService) },
                                { "Appliance Repairs", typeof(TblApplianceRepairService) },
                                { "General Handyman Services", typeof(TblGeneralService) }
                            };


                            string classNameFromUI = jobDTO.MainServiceName; //        jobDTO.MainService; -> can be any mainService

                            // Check if the class name exists in the dictionary
                            if (serviceTableMap.TryGetValue(classNameFromUI, out Type type))
                            {
                                // Create an instance of the type
                                int locationTypeID = 0, roomTypeID = 0, noOfCoats = 0, typeFurnitureId = 0, AreaTypeId = 0;
                                var instance = Activator.CreateInstance(type);

                                // Get properties of the class
                                var properties = type.GetProperties();



                                if (jobDTO.MainServiceName == "Painting")
                                {
                                    noOfCoats = subServiceDTO.NumberOfCoats.HasValue && (bool)subServiceDTO.NumberOfCoats ? 2 : 0;

                                }
                                if (jobDTO.MainServiceName == "Pest Control")
                                {
                                    locationTypeID = mainServiceContext.LocationTypes
                                                     .FirstOrDefault(p => p.LocationTypeName.Equals(subServiceDTO.LocationType)).LocationTypeId;

                                    roomTypeID = mainServiceContext.RoomTypes
                                                     .FirstOrDefault(p => p.RoomTypeName.Equals(subServiceDTO.RoomType)).RoomTypeId;

                                }
                                if (jobDTO.MainServiceName == "Home Cleaning")
                                {
                                    locationTypeID = mainServiceContext.LocationTypes
                                                     .FirstOrDefault(p => p.LocationTypeName.Equals(subServiceDTO.LocationType)).LocationTypeId;

                                    if (subServiceDTO.areaType != null)
                                    {

                                        typeFurnitureId = mainServiceContext.AreaTypes
                                                        .FirstOrDefault(p => p.AreaTypeName.Equals(subServiceDTO.areaType)).AreaTypeId;
                                    }
                                    if (subServiceDTO.TypeFurniture != null)
                                    {

                                        AreaTypeId = mainServiceContext.TypeFurnitures
                                                    .FirstOrDefault(p => p.TypeFurnitureName.Equals(subServiceDTO.TypeFurniture)).TypeFurnitureId;
                                    }

                                }

                                // Sample data received from UI (replace with actual data)
                                Dictionary<string, object> uiData = new Dictionary<string, object>
                                {
                                    { "JobId", recentJobId },
                                    { "SubCategoryId", subServiceId },
                                    { "MoreDetailsDescription", subServiceDTO.SpecialRequest },
                                    { "NumberItems", subServiceDTO.NumberItems },
                                    { "SizeArea", subServiceDTO.SizeArea },
                                    { "ProvidePaint", subServiceDTO.ProvidePaint },
                                    { "PaintColor", subServiceDTO.PaintColor },
                                    { "NumberofCoats", noOfCoats },
                                    { "SpecialRequest", subServiceDTO.SpecialRequest },   // pest control
                                    { "NumberRooms", subServiceDTO.NumberItems },
                                    { "RoomTypeId", roomTypeID },
                                    { "LocationTypeId", locationTypeID },
                                    { "SubServiceId", subServiceId },
                                    { "Description", subServiceDTO.SpecialRequest },
                                    { "NumberOfItems", subServiceDTO.NumberItems }, // carpentry service
                                    { "TypeAppliance", subServiceDTO.TypeAppliance }, // appliance Service,
                                    {  "AreaSize", subServiceDTO.SizeArea},         //landscap
                                    {  "ProvideSupplies", subServiceDTO.ProvideSupplies},         // Home cleaning
                                    {  "NumberCleaner", subServiceDTO.NumberCleaner},         //Home cleaning
                                    {  "NumberHours", subServiceDTO.NumberHours},         //Home cleaning
                                    {  "AreaTypeId", AreaTypeId == 0 ? null : AreaTypeId},         //Home cleaning
                                    {  "TypeFurnitureId", typeFurnitureId == 0 ? null : typeFurnitureId},         //Home cleaning

                                };

                                // Assign values to properties dynamically
                                foreach (var property in properties)
                                {
                                    // Check if the property name exists in the UI data
                                    if (uiData.TryGetValue(property.Name, out object value))
                                    {
                                        try
                                        {


                                            if (property.PropertyType == typeof(bool?))
                                            {
                                                // Attempt to convert to integer using truncation (discard decimal part)
                                                bool bol = Convert.ToBoolean(value);  // This might cause data loss

                                                // Set the value as nullable int
                                                object convertedValue = bol;
                                                property.SetValue(instance, convertedValue);
                                            }
                                            else if (property.PropertyType == typeof(decimal?))
                                            {
                                                // Attempt to convert to integer using truncation (discard decimal part)
                                                decimal intDec = Convert.ToDecimal(value);  // This might cause data loss

                                                // Set the value as nullable int
                                                object convertedValue = intDec;
                                                property.SetValue(instance, convertedValue);
                                            }
                                            else if (property.PropertyType == typeof(int?))
                                            {
                                                int intVal;
                                                string numberString = value?.ToString()?.TrimEnd('+');
                                                if (int.TryParse(numberString, out intVal))
                                                {
                                                    object convertedValue = intVal;
                                                    property.SetValue(instance, convertedValue);
                                                }
                                                else
                                                {
                                                    // Handle cases where conversion to int fails (e.g., non-numeric data)
                                                    // You can set null or a default value here
                                                    property.SetValue(instance, null);
                                                }
                                            }
                                            else
                                            {
                                                // Not a nullable int, use regular conversion
                                                object convertedValue = Convert.ChangeType(value, property.PropertyType);
                                                property.SetValue(instance, convertedValue);
                                            }
                                        }
                                        catch (InvalidCastException e)
                                        {
                                            // Handle the case where conversion fails
                                            // Log the error with details about property and value
                                            Console.WriteLine($"Error converting value for property '{property.Name}': {e.Message}");
                                        }
                                    }
                                }


                                // handle switch cases
                                switch (jobDTO.MainServiceName)
                                {
                                    case "Painting":
                                        // Assuming instance is an instance of TblPaintingService
                                        var paintingContext = new AnyHandyDBContext<TblPaintingService>();
                                        paintingContext.Save((TblPaintingService)instance);
                                        var paintingServiceInstance = (TblPaintingService)instance;
                                        int paintingId = paintingServiceInstance.PaintingServiceId; // Assuming property exists

                                        if (subServiceDTO.Images.Count > 0)
                                        {
                                            List<TblPaintingServicesAttachment> tblPaintingServicesAttachmentList = new List<TblPaintingServicesAttachment>();
                                            var tblPaintingServicesAttachmentContext = new AnyHandyDBContext<TblPaintingServicesAttachment>();
                                            foreach (var image in subServiceDTO.Images)
                                            {
                                                var key = Guid.NewGuid().ToString();
                                                _storageService.UploadFileAsync(key, image.Content, image.ContentType).Wait(); // Wait for upload to finish

                                                var tblPaintingServicesAttachmentEntity = new TblPaintingServicesAttachment
                                                {
                                                    PaintingServiceId = paintingId,
                                                    FilePath = key
                                                };
                                                tblPaintingServicesAttachmentList.Add(tblPaintingServicesAttachmentEntity);
                                            }
                                            tblPaintingServicesAttachmentContext.BulkSave(tblPaintingServicesAttachmentList);
                                        }

                                        break;
                                    case "Home Cleaning":
                                        // Assuming instance is an instance of TblHomeCleaningService
                                        var homeCleaningContext = new AnyHandyDBContext<TblHomeCleaning>();
                                        homeCleaningContext.Save((TblHomeCleaning)instance);

                                        var HomeCleaningInstance = (TblHomeCleaning)instance;
                                        int homeCleaningId = HomeCleaningInstance.HomeCleaningServiceId;
                                        if (subServiceDTO.Images.Count > 0)
                                        {
                                            List<TblHomeCleaningAttachment> tblHomeCleaningServicesAttachmentList = new List<TblHomeCleaningAttachment>();
                                            var tblHomeCleaningServicesAttachmentContext = new AnyHandyDBContext<TblHomeCleaningAttachment>();
                                            foreach (var image in subServiceDTO.Images)
                                            {
                                                var key = Guid.NewGuid().ToString();
                                                _storageService.UploadFileAsync(key, image.Content, image.ContentType).Wait(); // Wait for upload to finish

                                                var tblHomeCleaningServicesAttachmentEntity = new TblHomeCleaningAttachment
                                                {
                                                    HomeCleaningServiceId = homeCleaningId,
                                                    FilePath = key
                                                };
                                                tblHomeCleaningServicesAttachmentList.Add(tblHomeCleaningServicesAttachmentEntity);
                                            }
                                            tblHomeCleaningServicesAttachmentContext.BulkSave(tblHomeCleaningServicesAttachmentList);
                                        }


                                        break;
                                    case "Electrical Service":
                                        // Assuming instance is an instance of TblElectricalService
                                        var electricalContext = new AnyHandyDBContext<TblElectricalService>();
                                        electricalContext.Save((TblElectricalService)instance);

                                        var electricalServiceInstance = (TblElectricalService)instance;
                                        int electricServiceId = electricalServiceInstance.ElectricalServiceId;

                                        if (subServiceDTO.Images.Count > 0)
                                        {
                                            List<TblElectricalServicesAttachment> tblElectricalServicesAttachmentlist = new List<TblElectricalServicesAttachment>();
                                            var tblElectricalServicesAttachmentContext = new AnyHandyDBContext<TblElectricalServicesAttachment>();
                                            foreach (var image in subServiceDTO.Images)
                                            {
                                                var key = Guid.NewGuid().ToString();
                                                _storageService.UploadFileAsync(key, image.Content, image.ContentType).Wait();

                                                var tblElectricalServicesAttachmentEntity = new TblElectricalServicesAttachment
                                                {
                                                    ElectricalServiceId = electricServiceId,
                                                    FilePath = key,
                                                };
                                                tblElectricalServicesAttachmentlist.Add(tblElectricalServicesAttachmentEntity);
                                               
                                            }
                                            tblElectricalServicesAttachmentContext.BulkSave(tblElectricalServicesAttachmentlist);
                                        }


                                        break;
                                    case "Landscaping and Lawn Care":
                                        // Assuming instance is an instance of TblLandscapingService
                                        var landscapingContext = new AnyHandyDBContext<TblLandscapingService>();
                                        landscapingContext.Save((TblLandscapingService)instance);

                                        var landscapingServiceInstance = (TblLandscapingService)instance;
                                        int landscapingServiceId = landscapingServiceInstance.LandscapingServiceId; // Assuming property exists

                                        if (subServiceDTO.Images.Count > 0)
                                        {
                                            List<TblLandscapingServiceAttachment> tblLandscapingServicesAttachmentList = new List<TblLandscapingServiceAttachment>();
                                            var tblLandscapingServicesAttachmentContext = new AnyHandyDBContext<TblLandscapingServiceAttachment>();
                                            foreach (var image in subServiceDTO.Images)
                                            {
                                                var key = Guid.NewGuid().ToString();
                                                _storageService.UploadFileAsync(key, image.Content, image.ContentType).Wait();

                                                var tblLandscapingServicesAttachmentEntity = new TblLandscapingServiceAttachment
                                                {
                                                    LandscapingServiceId = landscapingServiceId,
                                                    AttachmentPath = key
                                                };
                                                tblLandscapingServicesAttachmentList.Add(tblLandscapingServicesAttachmentEntity);
                                            }
                                            tblLandscapingServicesAttachmentContext.BulkSave(tblLandscapingServicesAttachmentList);
                                        }

                                        break;
                                    case "Pest Control":
                                        // Assuming instance is an instance of TblPestControlService
                                        var pestControlContext = new AnyHandyDBContext<TblPestControlService>();
                                        pestControlContext.Save((TblPestControlService)instance);

                                        break;
                                    case "Plumbing":
                                        // Assuming instance is an instance of TblPlumbingService
                                        var plumbingContext = new AnyHandyDBContext<TblPlumbingService>();
                                        plumbingContext.Save((TblPlumbingService)instance);

                                        var plumbingControlServiceInstance = (TblPestControlService)instance;
                                        int plumbingId = plumbingControlServiceInstance.PestControlServiceId; // Assuming property exists
                                        


                                        if (subServiceDTO.Images.Count > 0)
                                        {
                                            List<TblPlumbingServicesAttachment> tblElectricalServicesAttachmentlist = new List<TblPlumbingServicesAttachment>();
                                            var tblElectricalServicesAttachmentContext = new AnyHandyDBContext<TblPlumbingServicesAttachment>();
                                            foreach (var image in subServiceDTO.Images)
                                            {

                                                //string key = "ElectricalService/" + image.FileName;
                                                // Generate a unique key for the file (e.g., using a GUID)
                                                var key = Guid.NewGuid().ToString();
                                                _storageService.UploadFileAsync(key, image.Content,
                                                   image.ContentType);

                                                var tblElectricalServicesAttachmentEntity = new TblPlumbingServicesAttachment
                                                {
                                                    PlumbingServiceId = plumbingId,
                                                    FilePath = key,
                                                };
                                                tblElectricalServicesAttachmentlist.Add(tblElectricalServicesAttachmentEntity);

                                            }
                                            tblElectricalServicesAttachmentContext.BulkSave(tblElectricalServicesAttachmentlist);
                                        }


                                        break;
                                    case "HVAC":
                                        // Assuming instance is an instance of TblHvacService
                                        var hvacContext = new AnyHandyDBContext<TblHvacService>();
                                        hvacContext.Save((TblHvacService)instance);

                                        var hvacServiceInstance = (TblHvacService)instance;
                                        int hvacId = hvacServiceInstance.HvacServiceId; // Assuming property exists

                                        if (subServiceDTO.Images.Count > 0)
                                        {
                                            List<TblHvacservicesAttachment> tblHvacServicesAttachmentList = new List<TblHvacservicesAttachment>();
                                            var tblHvacServicesAttachmentContext = new AnyHandyDBContext<TblHvacservicesAttachment>();
                                            foreach (var image in subServiceDTO.Images)
                                            {
                                                var key = Guid.NewGuid().ToString();
                                                _storageService.UploadFileAsync(key, image.Content, image.ContentType).Wait(); // Wait for upload to finish

                                                var tblHvacServicesAttachmentEntity = new TblHvacservicesAttachment
                                                {
                                                    HvacServiceId = hvacId,
                                                    FilePath = key
                                                };
                                                tblHvacServicesAttachmentList.Add(tblHvacServicesAttachmentEntity);
                                            }
                                            tblHvacServicesAttachmentContext.BulkSave(tblHvacServicesAttachmentList);
                                        }

                                        break;
                                    case "Carpentry Service":
                                        // Assuming instance is an instance of TblCarpentryService
                                        var carpentryContext = new AnyHandyDBContext<TblCarpentryService>();
                                        carpentryContext.Save((TblCarpentryService)instance);

                                        var carpentryServiceInstance = (TblCarpentryService)instance;
                                        int carpentryID = carpentryServiceInstance.CarpentryServiceId; // Assuming property exists

                                        if (subServiceDTO.Images.Count > 0)
                                        {
                                            List<TblCarpentryServicesAttachment> tblCarpentryServicesAttachmentList = new List<TblCarpentryServicesAttachment>();
                                            var tblCarpentryServicesAttachmentContext = new AnyHandyDBContext<TblCarpentryServicesAttachment>();
                                            foreach (var image in subServiceDTO.Images)
                                            {
                                                var key = Guid.NewGuid().ToString();
                                                _storageService.UploadFileAsync(key, image.Content, image.ContentType).Wait(); // Wait for upload to finish

                                                var tblCarpentryServicesAttachmentEntity = new TblCarpentryServicesAttachment
                                                {
                                                    CarpentryServiceId = carpentryID,
                                                    FilePath = key
                                                };
                                                tblCarpentryServicesAttachmentList.Add(tblCarpentryServicesAttachmentEntity);
                                            }
                                            tblCarpentryServicesAttachmentContext.BulkSave(tblCarpentryServicesAttachmentList);
                                        }

                                                break;
                                    case "Appliance Repairs":
                                        // Assuming instance is an instance of TblApplianceRepairService
                                        var applianceRepairContext = new AnyHandyDBContext<TblApplianceRepairService>();
                                        applianceRepairContext.Save((TblApplianceRepairService)instance);

                                        break;
                                    case "General Handyman Services":
                                        // Assuming instance is an instance of TblGeneralServiceAttachment
                                        var generalHandymanContext = new AnyHandyDBContext<TblGeneralService>();
                                        generalHandymanContext.Save((TblGeneralService)instance);
                                        break;
                                    // Add cases for other service types as needed
                                    default:
                                        // Handle default case
                                        break;
                                }
                            }
                            else
                            {
                                // Handle case when the class name is not found in the dictionary
                                // For example, throw an exception or log an error
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions here
                        Console.WriteLine("Error adding job: " + ex.Message);
                    }
                }
            }
        }








    }
}
