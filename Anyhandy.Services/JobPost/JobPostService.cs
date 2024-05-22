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
using System.Linq.Expressions;
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

        public JobPostVM submitJobPost(MainServiceDTO jobDTO, int userID)
        {
            JobPostVM jobPostVM = new JobPostVM();
            int addressId = 0;
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
                            EndRate = jobDTO.JobDetails.EndRate,
                            StartRate = jobDTO.JobDetails.StartRate,
                            Amount = jobDTO.JobDetails.FixedPriceAmount == null ? null : int.Parse(jobDTO.JobDetails.FixedPriceAmount),                     
                            Status = 1,
                            JobTitle = "No details",
                            JobDetails = jobDTO.MainServiceDescription,
                            UserId = userID,
                            DueDate = jobDTO.JobDetails.EndDate,
                            JobStart = jobDTO.JobDetails.StartDate,
                            JobExpectedEnd = jobDTO.JobDetails.EndDate
                        };

                        jobContext.Save(jobEntity);
                        var recentJobId = jobEntity.JobId;
                        jobPostVM.JobId = recentJobId;
                        jobPostVM.MainServiceName = jobDTO.MainServiceName;
                        if (jobDTO.Location.AddressType != null || jobDTO.Location.Details != null)
                        {
                            var userAddressContext = new AnyHandyDBContext<UserAddress>();
                            var countryContext = new AnyHandyDBContext<Country>();
                            var cityContext = new AnyHandyDBContext<City>();
                            // Create a new Job entity instance
                            var userAddressEntity = new UserAddress
                            {
                                AddressType = jobDTO.Location.AddressType == null ? string.Empty : jobDTO.Location.AddressType,
                                City = cityContext.Cities.FirstOrDefault(x => x.Name == jobDTO.Location.City).CityId,
                                Country = countryContext.Countries.FirstOrDefault(x => x.Name == jobDTO.Location.Country).CountryId,
                                Details = jobDTO.Location.Details == null ? string.Empty : jobDTO.Location.Details,
                                UserId = userID,
                            };
                            userAddressContext.Save(userAddressEntity);
                            addressId = userAddressEntity.AddressId;
                        }
                        jobPostVM.Location.AddressId = addressId;

                        foreach (var subServiceDTO in jobDTO.SubServices)
                        {
                            // Get sub-service details
                            int subServiceId = mainServiceContext.SubServices
                                .FirstOrDefault(p => p.ServiceNameEn.ToLower().Contains(subServiceDTO.Name.ToLower())).SubServicesId;


                            jobPostVM.SubServices.Add(new SubServiceVM { Name = subServiceDTO.Name, id = subServiceId });
                            Dictionary<string, Type> serviceTableMap = new Dictionary<string, Type>()
                            {
                                { "Home Cleaning", typeof(TblHomeCleaning) },
                                { "Electrical Service", typeof(TblElectricalService) },
                                { "Painting", typeof(TblPaintingService) },
                                { "Landscaping And Lawn Care", typeof(TblLandscapingService) },
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
                                    if(subServiceDTO.LocationType != null)
                                    locationTypeID = mainServiceContext.LocationTypes
                                                     .FirstOrDefault(p => p.LocationTypeName.Equals(subServiceDTO.LocationType)).LocationTypeId;


                                    if(subServiceDTO.RoomType != null)
                                    roomTypeID = mainServiceContext.RoomTypes
                                                     .FirstOrDefault(p => p.RoomTypeName.Equals(subServiceDTO.RoomType)).RoomTypeId;

                                }
                                if (jobDTO.MainServiceName == "Home Cleaning")
                                {
                                    if (subServiceDTO.LocationType != null)
                                        locationTypeID = mainServiceContext.LocationTypes
                                                     .FirstOrDefault(p => p.LocationTypeName.Equals(subServiceDTO.LocationType)).LocationTypeId;

                                    if (subServiceDTO.areaType != null)
                                    {

                                        AreaTypeId = mainServiceContext.AreaTypes
                                                        .FirstOrDefault(p => p.AreaTypeName.Equals(subServiceDTO.areaType)).AreaTypeId;
                                    }
                                    if (subServiceDTO.TypeFurniture != null)
                                    {
                                        
                                        typeFurnitureId = mainServiceContext.TypeFurnitures
                                                    .FirstOrDefault(p => p.TypeFurnitureName.Equals(subServiceDTO.TypeFurniture)).TypeFurnitureId;
                                    }

                                }
                                var cccc = 19;
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
                                    { "SpecialRequest", subServiceDTO.SpecialRequest },
                                    { "LocationSizeId", subServiceDTO.SizeArea},// pest control
                                    { "NumberRooms", subServiceDTO.NumberItems },
                                    { "RoomTypeId", roomTypeID == 0 ? null : roomTypeID},
                                    { "LocationTypeId", locationTypeID == 0 ? null : locationTypeID},
                                    { "SubServiceId", subServiceId },
                                    { "Description", subServiceDTO.SpecialRequest },
                                    { "NumberOfItems", subServiceDTO.NumberItems }, // carpentry service
                                    { "TypeAppliance", subServiceDTO.TypeAppliance }, // appliance Service,
                                    {  "AreaSize", subServiceDTO.SizeArea},
                                    {  "SubServicesId", subServiceId},     //landscap
                                    {  "ProvideSupplies", subServiceDTO.ProvideSupplies},         // Home cleaning
                                    {  "NumberCleaner", subServiceDTO.NumberCleaner},         //Home cleaning
                                    {  "NumberHours", subServiceDTO.NumberHours},         //Home cleaning
                                    {  "AreaTypeId", AreaTypeId == 0 ? null : AreaTypeId},         //Home cleaning
                                    {  "TypeFurnitureId", typeFurnitureId == 0 ? null : typeFurnitureId}

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
                                    case "Landscaping And Lawn Care":
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

                                        var plumbingControlServiceInstance = (TblPlumbingService)instance;
                                        int plumbingId = plumbingControlServiceInstance.PlumbingServiceId; // Assuming property exists
                                        


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

                                        var generalServiceInstance = (TblGeneralService)instance;
                                        int generalServiceId = generalServiceInstance.GeneralServiceId;
                                        if (subServiceDTO.Images.Count > 0)
                                        {
                                            List<TblGeneralServiceAttachment> tblGeneralServiceAttachmentList = new List<TblGeneralServiceAttachment>();
                                            var tblGeneralServiceAttachmentContext = new AnyHandyDBContext<TblGeneralServiceAttachment>();
                                            foreach (var image in subServiceDTO.Images)
                                            {
                                               var key = Guid.NewGuid().ToString();
                                                _storageService.UploadFileAsync(key, image.Content, image.ContentType).Wait(); // Wait for upload to finish

                                                var entity = new TblGeneralServiceAttachment
                                                {
                                                    GeneralServiceServiceId = generalServiceId,
                                                    FilePath = key
                                                };
                                                tblGeneralServiceAttachmentList.Add(entity);
                                            }
                                            tblGeneralServiceAttachmentContext.BulkSave(tblGeneralServiceAttachmentList);
                                        }
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
                        return jobPostVM;
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions here
                        Console.WriteLine("Error adding job: " + ex.Message);
                        return jobPostVM;
                    }
                }
            }
        }

        public void editJobPost(MainServiceDTO jobDTO, int userID, int jobId)
        {
            using (var context = new AnyHandyDBContext<Job>())
            {
                var job = GetJobPostById(context, jobId);
                if (job != null)
                {
                    updateJobDetails(context, job, jobDTO);
                    context.SaveChanges();

                    updateUserLocation(jobDTO);

                    int existingJob = job.JobId;
                    using (var mainServiceContext = new AnyHandyDBContext<MainService1>())
                    {
                        List<string> NonDeletionSubservice = new List<string>();
                        var modifiedSubservices = jobDTO.SubServices.Select(x => x.Name.ToLower()).ToList();
                        int i = 0;
                        foreach (var subServiceDTO in jobDTO.SubServices)
                        {
                            int subServiceId = mainServiceContext.SubServices
                                .FirstOrDefault(p => p.ServiceNameEn.ToLower().Contains(subServiceDTO.Name.ToLower())).SubServicesId;
                            

                            switch (jobDTO.MainServiceName)
                            {
                                case "Painting":
                                    var paintingContext = new AnyHandyDBContext<TblPaintingService>();
               
                                    int existingPaintingId = 0;
                                    // add new if painting
                                    if (subServiceDTO.IsNew)
                                    {
                                        var newPaintingSubservice = new TblPaintingService
                                        {
                                            ProvidePaint = subServiceDTO.ProvidePaint,
                                            PaintColor = subServiceDTO.PaintColor,
                                            NumberofCoats = subServiceDTO.NumberOfCoats.HasValue && (bool)subServiceDTO.NumberOfCoats ? 2 : 0,
                                            MoreDetailsDescription = subServiceDTO.SpecialRequest,
                                            NumberItems = Convert.ToInt32(subServiceDTO.NumberItems?.TrimEnd('+')),
                                            SizeArea = Convert.ToDecimal(subServiceDTO.SizeArea),
                                            SpecialRequest = subServiceDTO.SpecialRequest,
                                            SubCategoryId = subServiceId,
                                            JobId = existingJob
                                        };
                                        paintingContext.Save(newPaintingSubservice);
                                        existingPaintingId = newPaintingSubservice.PaintingServiceId;
                                    }
                                    else
                                    {
                                        var existingPainting = paintingContext.TblPaintingServices.FirstOrDefault(p => p.SubCategoryId == subServiceId
                                   && p.JobId == existingJob);

                                        if(existingPainting != null)
                                        {

                                            // Update properties from DTO to model
                                            existingPainting.ProvidePaint = subServiceDTO.ProvidePaint ?? existingPainting.ProvidePaint;
                                            existingPainting.PaintColor = subServiceDTO.PaintColor ?? existingPainting.PaintColor;
                                            existingPainting.NumberofCoats = subServiceDTO.NumberOfCoats.HasValue && (bool)subServiceDTO.NumberOfCoats ? 2 : 0;
                                            existingPainting.MoreDetailsDescription = subServiceDTO.SpecialRequest ?? existingPainting.SpecialRequest;
                                            existingPainting.NumberItems = Convert.ToInt32(subServiceDTO.NumberItems?.TrimEnd('+')); //?? existingPainting.NumberItems;
                                            existingPainting.SizeArea = Convert.ToDecimal(subServiceDTO.SizeArea); //?? existingPainting.SizeArea;
                                            existingPainting.SpecialRequest = subServiceDTO.SpecialRequest ?? existingPainting.SpecialRequest;
                                            paintingContext.SaveChanges();

                                            existingPaintingId = existingPainting.PaintingServiceId;
                                        }
                                    }
                                    // add new images if attached
                                    if (subServiceDTO.Images.Count > 0 && existingPaintingId > 0)
                                    {
                                        var tblPaintingServicesAttachmentContext = new AnyHandyDBContext<TblPaintingServicesAttachment>();
                                        List<TblPaintingServicesAttachment> tblPaintingServicesAttachmentList = new List<TblPaintingServicesAttachment>();
                                        // first I remove all the already and upload new one's
                                        // if pic is not already added than add new pic into S3 and db
                                        var alreadyUploadedPic = tblPaintingServicesAttachmentContext.TblPaintingServicesAttachments.Where(p =>
                                                                                  p.PaintingServiceId == existingPaintingId).ToList();
                                        if(alreadyUploadedPic.Any())
                                        tblPaintingServicesAttachmentContext.BulkRemove(alreadyUploadedPic);
                                        foreach (var image in subServiceDTO.Images)
                                        {
                                                var key = Guid.NewGuid().ToString();
                                                _storageService.UploadFileAsync(key, image.Content, image.ContentType).Wait(); // Wait for upload to finish

                                                var tblPaintingServicesAttachmentEntity = new TblPaintingServicesAttachment
                                                {
                                                    PaintingServiceId = existingPaintingId,
                                                    FilePath = key
                                                };
                                                tblPaintingServicesAttachmentList.Add(tblPaintingServicesAttachmentEntity);
                                        }
                                        if(tblPaintingServicesAttachmentList.Any())
                                        tblPaintingServicesAttachmentContext.BulkSave(tblPaintingServicesAttachmentList);
                                    }

                                    if (i == jobDTO.SubServices.Count - 1)
                                    {
                                        // Fetch painting services and include related subcategory
                                        var list = paintingContext.TblPaintingServices.Include(x => x.SubCategory).Where(x => x.JobId == existingJob).ToList();

                                        // Identify services to delete
                                        var deletions = list.Where(x => !modifiedSubservices.Contains(x.SubCategory.ServiceNameEn.ToLower())).ToList();
                                        if(deletions.Any())
                                        {
                                            // Prepare attachment deletion list
                                            var attachmentContext = new AnyHandyDBContext<TblPaintingServicesAttachment>();
                                            var paintingIdsToDelete = deletions.Select(d => d.PaintingServiceId).ToList();
                                            var attachmentsToDelete = attachmentContext.TblPaintingServicesAttachments.
                                                Where(a => paintingIdsToDelete.Contains((int)a.PaintingServiceId)).ToList();

                                            // Bulk remove attachments and painting services
                                            attachmentContext.BulkRemove(attachmentsToDelete);
                                            paintingContext.BulkRemove(deletions);
                                        }
                                    }
                                    break;
                                case "Home Cleaning":
                                    var homeCleaningContext = new AnyHandyDBContext<TblHomeCleaning>();
                                    int existingHomeCleaningId = 0;

                                    // Add new if home cleaning service
                                    if (i == jobDTO.SubServices.Count - 1)
                                    {
                                        // Fetch home cleaning services and include related subcategory
                                        var list = homeCleaningContext.TblHomeCleanings.Include(x => x.SubService).Where(x => x.JobId == existingJob).ToList();

                                        // Identify services to delete
                                        var deletions = list.Where(x => !modifiedSubservices.Contains(x.SubService.ServiceNameEn.ToLower())).ToList();
                                        if(deletions.Any())
                                        {
                                            // Prepare attachment deletion list
                                            var attachmentContext = new AnyHandyDBContext<TblHomeCleaningAttachment>();
                                            var homeCleaningIdsToDelete = deletions.Select(d => d.HomeCleaningServiceId).ToList();
                                            var attachmentsToDelete = attachmentContext.TblHomeCleaningAttachments
                                                .Where(a => homeCleaningIdsToDelete.Contains((int)a.HomeCleaningServiceId)).ToList();

                                            // Bulk remove attachments and home cleaning services
                                            attachmentContext.BulkRemove(attachmentsToDelete);
                                            homeCleaningContext.BulkRemove(deletions);
                                        }
                                    }
                                    if (subServiceDTO.IsNew)
                                    {

                                        int areaTypeId = subServiceDTO.areaType != null ?
                                                         mainServiceContext.AreaTypes.FirstOrDefault(p => p.AreaTypeName.Equals(subServiceDTO.areaType))?.AreaTypeId ?? 0 : 0;

                                        int typeFurnitureId = subServiceDTO.TypeFurniture != null ?
                                                              mainServiceContext.TypeFurnitures.FirstOrDefault(p => p.TypeFurnitureName.Equals(subServiceDTO.TypeFurniture))?.TypeFurnitureId ?? 0 : 0;

                                        var newHomeCleaningService = new TblHomeCleaning
                                        {
                                            ProvideSupplies = subServiceDTO.ProvideSupplies,
                                            LocationTypeId = subServiceDTO.LocationType == null ? null : mainServiceContext.LocationTypes
                                            .FirstOrDefault(p => p.LocationTypeName.Equals(subServiceDTO.LocationType)).LocationTypeId,
                                            NumberCleaner = string.IsNullOrEmpty(subServiceDTO.NumberCleaner) ? (int?)null : Convert.ToInt32(subServiceDTO.NumberCleaner.TrimEnd('+')),
                                            NumberHours = string.IsNullOrEmpty(subServiceDTO.NumberHours) ? (int?)null : Convert.ToInt32(subServiceDTO.NumberHours.TrimEnd('+')),
                                            MoreDetailsDescription = subServiceDTO.SpecialRequest,
                                            AreaTypeId = areaTypeId == 0 ? (int?)null : areaTypeId,
                                            TypeFurnitureId = typeFurnitureId == 0 ? (int?)null : typeFurnitureId,
                                            NumberItems = string.IsNullOrEmpty(subServiceDTO.NumberItems) ? (int?)null : Convert.ToInt32(subServiceDTO.NumberItems.TrimEnd('+')),
                                            SubServiceId = subServiceId,
                                            JobId = existingJob
                                        };
                                        homeCleaningContext.Save(newHomeCleaningService);
                                        existingHomeCleaningId = newHomeCleaningService.HomeCleaningServiceId;
                                    }
                                    else
                                    {
                                        var existingHomeCleaning = homeCleaningContext.TblHomeCleanings.FirstOrDefault(h => h.SubServiceId == subServiceId && h.JobId == existingJob);

                                        if (existingHomeCleaning != null)
                                        {
                                            // Get the IDs for LocationType, AreaType, and TypeFurniture if they exist
                                            int locationTypeID = subServiceDTO.LocationType != null ?
                                                                 mainServiceContext.LocationTypes.FirstOrDefault(p => p.LocationTypeName.Equals(subServiceDTO.LocationType))?.LocationTypeId ?? 0 : 0;

                                            int areaTypeId = subServiceDTO.areaType != null ?
                                                             mainServiceContext.AreaTypes.FirstOrDefault(p => p.AreaTypeName.Equals(subServiceDTO.areaType))?.AreaTypeId ?? 0 : 0;

                                            int typeFurnitureId = subServiceDTO.TypeFurniture != null ?
                                                                  mainServiceContext.TypeFurnitures.FirstOrDefault(p => p.TypeFurnitureName.Equals(subServiceDTO.TypeFurniture))?.TypeFurnitureId ?? 0 : 0;

                                            // Update properties from DTO to model
                                            existingHomeCleaning.ProvideSupplies = subServiceDTO.ProvideSupplies ?? existingHomeCleaning.ProvideSupplies;
                                            existingHomeCleaning.LocationTypeId = locationTypeID == 0 ? existingHomeCleaning.LocationTypeId : locationTypeID;
                                            existingHomeCleaning.NumberCleaner = string.IsNullOrEmpty(subServiceDTO.NumberCleaner) ? existingHomeCleaning.NumberCleaner : Convert.ToInt32(subServiceDTO.NumberCleaner.Trim('+'));
                                            existingHomeCleaning.NumberHours = string.IsNullOrEmpty(subServiceDTO.NumberHours) ? existingHomeCleaning.NumberHours : Convert.ToInt32(subServiceDTO.NumberHours.Trim('+'));
                                            existingHomeCleaning.MoreDetailsDescription = subServiceDTO.SpecialRequest ?? existingHomeCleaning.MoreDetailsDescription;
                                            existingHomeCleaning.AreaTypeId = areaTypeId == 0 ? existingHomeCleaning.AreaTypeId : areaTypeId;
                                            existingHomeCleaning.TypeFurnitureId = typeFurnitureId == 0 ? existingHomeCleaning.TypeFurnitureId : typeFurnitureId;
                                            existingHomeCleaning.NumberItems = string.IsNullOrEmpty(subServiceDTO.NumberItems) ? existingHomeCleaning.NumberItems : Convert.ToInt32(subServiceDTO.NumberItems.TrimEnd('+'));

                                            homeCleaningContext.SaveChanges();
                                            existingHomeCleaningId = existingHomeCleaning.HomeCleaningServiceId;
                                        }
                                    }

                                    // Add new images if attached
                                    if (subServiceDTO.Images.Count > 0 && existingHomeCleaningId > 0)
                                    {
                                        var tblHomeCleaningAttachmentsContext = new AnyHandyDBContext<TblHomeCleaningAttachment>();
                                        List<TblHomeCleaningAttachment> tblHomeCleaningAttachmentList = new List<TblHomeCleaningAttachment>();

                                        // Remove all the already uploaded images
                                        var alreadyUploadedPics = tblHomeCleaningAttachmentsContext.TblHomeCleaningAttachments.Where(h => h.HomeCleaningServiceId == existingHomeCleaningId).ToList();
                                        if (alreadyUploadedPics.Any())
                                            tblHomeCleaningAttachmentsContext.BulkRemove(alreadyUploadedPics);

                                        // Add new images
                                        foreach (var image in subServiceDTO.Images)
                                        {
                                            var key = Guid.NewGuid().ToString();
                                            _storageService.UploadFileAsync(key, image.Content, image.ContentType).Wait(); // Wait for upload to finish

                                            var tblHomeCleaningAttachmentEntity = new TblHomeCleaningAttachment
                                            {
                                                HomeCleaningServiceId = existingHomeCleaningId,
                                                FilePath = key
                                            };
                                            tblHomeCleaningAttachmentList.Add(tblHomeCleaningAttachmentEntity);
                                        }

                                        if (tblHomeCleaningAttachmentList.Any())
                                            tblHomeCleaningAttachmentsContext.BulkSave(tblHomeCleaningAttachmentList);
                                    }
                                    break;


                                    break;

                                case "Electrical Service":
                                    var electricalServiceContext = new AnyHandyDBContext<TblElectricalService>();
                                    int existingElectricalServiceId = 0;

                                    // Add new if electrical service
                                    if (i == jobDTO.SubServices.Count - 1)
                                    {
                                        // Fetch electrical services and include related subcategory
                                        var list = electricalServiceContext.TblElectricalServices.Include(x => x.SubCategory).Where(x => x.JobId == existingJob).ToList();

                                        // Identify services to delete
                                        var deletions = list.Where(x => !modifiedSubservices.Contains(x.SubCategory.ServiceNameEn.ToLower())).ToList();
                                        if(deletions.Any())
                                        {
                                            // Prepare attachment deletion list
                                            var attachmentContext = new AnyHandyDBContext<TblElectricalServicesAttachment>();
                                            var electricalServiceIdsToDelete = deletions.Select(d => d.ElectricalServiceId).ToList();
                                            var attachmentsToDelete = attachmentContext.TblElectricalServicesAttachments
                                                .Where(a => electricalServiceIdsToDelete.Contains((int)a.ElectricalServiceId)).ToList();

                                            // Bulk remove attachments and electrical services
                                            attachmentContext.BulkRemove(attachmentsToDelete);
                                            electricalServiceContext.BulkRemove(deletions);
                                        }
                                    }
                                    if (subServiceDTO.IsNew)
                                    {
                                        var newElectricalService = new TblElectricalService
                                        {
                                            Description = subServiceDTO.SpecialRequest,
                                            NumberOfItems = string.IsNullOrEmpty(subServiceDTO.NumberItems) ? (int?)null : Convert.ToInt32(subServiceDTO.NumberItems.TrimEnd('+')),
                                            SubCategoryId = subServiceId,
                                            JobId = existingJob
                                        };
                                        electricalServiceContext.Save(newElectricalService);
                                        existingElectricalServiceId = newElectricalService.ElectricalServiceId;
                                    }
                                    else
                                    {
                                        var existingElectricalService = electricalServiceContext.TblElectricalServices.FirstOrDefault(e => e.SubCategoryId == subServiceId && e.JobId == existingJob);

                                        // Update properties from DTO to model
                                        if (existingElectricalService != null)
                                        {
                                            existingElectricalService.Description = subServiceDTO.SpecialRequest ?? existingElectricalService.Description;
                                            existingElectricalService.NumberOfItems = string.IsNullOrEmpty(subServiceDTO.NumberItems) ? existingElectricalService.NumberOfItems : Convert.ToInt32(subServiceDTO.NumberItems.TrimEnd('+'));

                                            electricalServiceContext.SaveChanges();
                                            existingElectricalServiceId = existingElectricalService.ElectricalServiceId;
                                        }
                                    }

                                    // Add new images if attached
                                    if (subServiceDTO.Images.Count > 0 && existingElectricalServiceId > 0)
                                    {
                                        var tblElectricalServicesAttachmentContext = new AnyHandyDBContext<TblElectricalServicesAttachment>();
                                        List<TblElectricalServicesAttachment> tblElectricalServicesAttachmentList = new List<TblElectricalServicesAttachment>();

                                        // Remove all the already uploaded images
                                        var alreadyUploadedPics = tblElectricalServicesAttachmentContext.TblElectricalServicesAttachments.Where(e => e.ElectricalServiceId == existingElectricalServiceId).ToList();
                                        if (alreadyUploadedPics.Any())
                                            tblElectricalServicesAttachmentContext.BulkRemove(alreadyUploadedPics);

                                        // Add new images
                                        foreach (var image in subServiceDTO.Images)
                                        {
                                            var key = Guid.NewGuid().ToString();
                                            _storageService.UploadFileAsync(key, image.Content, image.ContentType).Wait(); // Wait for upload to finish

                                            var tblElectricalServicesAttachmentEntity = new TblElectricalServicesAttachment
                                            {
                                                ElectricalServiceId = existingElectricalServiceId,
                                                FilePath = key
                                            };
                                            tblElectricalServicesAttachmentList.Add(tblElectricalServicesAttachmentEntity);
                                        }

                                        if (tblElectricalServicesAttachmentList.Any())
                                            tblElectricalServicesAttachmentContext.BulkSave(tblElectricalServicesAttachmentList);
                                    }
                                    break;

                                case "Landscaping And Lawn Care":
                                    var landscapingServiceContext = new AnyHandyDBContext<TblLandscapingService>();
                                    int existingLandscapingServiceId = 0;


                                    if (i == jobDTO.SubServices.Count - 1)
                                    {
                                        // Fetch landscaping services and include related subcategory
                                        var list = landscapingServiceContext.TblLandscapingServices.Include(x => x.SubServices).Where(x => x.JobId == existingJob).ToList();

                                        // Identify services to delete
                                        var deletions = list.Where(x => !modifiedSubservices.Contains(x.SubServices.ServiceNameEn.ToLower())).ToList();

                                        // Prepare attachment deletion list
                                        if(deletions.Any())
                                        {

                                            var attachmentContext = new AnyHandyDBContext<TblLandscapingServiceAttachment>();
                                            var landscapingIdsToDelete = deletions.Select(d => d.LandscapingServiceId).ToList();
                                            var attachmentsToDelete = attachmentContext.TblLandscapingServiceAttachments
                                                .Where(a => landscapingIdsToDelete.Contains((int)a.LandscapingServiceId)).ToList();

                                            // Bulk remove attachments and landscaping services
                                            attachmentContext.BulkRemove(attachmentsToDelete);
                                            landscapingServiceContext.BulkRemove(deletions);
                                        }
                                    }
                                    if (subServiceDTO.IsNew)
                                    {
                                        var newLandscapingService = new TblLandscapingService
                                        {
                                            MoreDetailsDescription = subServiceDTO.SpecialRequest,
                                            AreaSize = subServiceDTO.SizeArea == null ? (decimal?)null : Convert.ToDecimal(subServiceDTO.SizeArea),
                                            SubServicesId = subServiceId,
                                            JobId = existingJob
                                        };
                                        landscapingServiceContext.Save(newLandscapingService);
                                        existingLandscapingServiceId = newLandscapingService.LandscapingServiceId;
                                    }
                                    else
                                    {
                                        var existingLandscapingService = landscapingServiceContext.TblLandscapingServices.FirstOrDefault(l => l.SubServicesId == subServiceId && l.JobId == existingJob);

                                        if (existingLandscapingService != null)
                                        {
                                            // Update properties from DTO to model
                                            existingLandscapingService.MoreDetailsDescription = subServiceDTO.SpecialRequest ?? existingLandscapingService.MoreDetailsDescription;
                                            existingLandscapingService.AreaSize = subServiceDTO.SizeArea == null ? existingLandscapingService.AreaSize : Convert.ToDecimal(subServiceDTO.SizeArea);

                                            landscapingServiceContext.SaveChanges();
                                            existingLandscapingServiceId = existingLandscapingService.LandscapingServiceId;
                                        }
                                    }

                                    // Add new images if attached
                                    if (subServiceDTO.Images.Count > 0 && existingLandscapingServiceId > 0)
                                    {
                                        var tblLandscapingServicesAttachmentContext = new AnyHandyDBContext<TblLandscapingServiceAttachment>();
                                        List<TblLandscapingServiceAttachment> tblLandscapingServiceAttachmentList = new List<TblLandscapingServiceAttachment>();

                                        // Remove all the already uploaded images
                                        var alreadyUploadedPics = tblLandscapingServicesAttachmentContext.TblLandscapingServiceAttachments.Where(l => l.LandscapingServiceId == existingLandscapingServiceId).ToList();
                                        if (alreadyUploadedPics.Any())
                                            tblLandscapingServicesAttachmentContext.BulkRemove(alreadyUploadedPics);

                                        // Add new images
                                        foreach (var image in subServiceDTO.Images)
                                        {
                                            var key = Guid.NewGuid().ToString();
                                            _storageService.UploadFileAsync(key, image.Content, image.ContentType).Wait(); // Wait for upload to finish

                                            var tblLandscapingServiceAttachmentEntity = new TblLandscapingServiceAttachment
                                            {
                                                LandscapingServiceId = existingLandscapingServiceId,
                                                AttachmentPath = key
                                            };
                                            tblLandscapingServiceAttachmentList.Add(tblLandscapingServiceAttachmentEntity);
                                        }

                                        if (tblLandscapingServiceAttachmentList.Any())
                                            tblLandscapingServicesAttachmentContext.BulkSave(tblLandscapingServiceAttachmentList);
                                    }
                                    break;

                                case "Plumbing":
                                    using (var plumbingContext = new AnyHandyDBContext<TblPlumbingService>())
                                    {
                                        int existingPlumbingId = 0;


                                        if (i == jobDTO.SubServices.Count - 1)
                                        {
                                            // Fetch plumbing services and include related subcategory
                                            var list = plumbingContext.TblPlumbingServices.Include(x => x.SubCategory).Where(x => x.JobId == existingJob).ToList();

                                            // Identify services to delete
                                            var deletions = list.Where(x => !modifiedSubservices.Contains(x.SubCategory.ServiceNameEn.ToLower())).ToList();
                                            if (deletions.Any())
                                            {
                                                // Prepare attachment deletion list
                                                var attachmentContext = new AnyHandyDBContext<TblPlumbingServicesAttachment>();
                                                var plumbingIdsToDelete = deletions.Select(d => d.PlumbingServiceId).ToList();
                                                var attachmentsToDelete = attachmentContext.TblPlumbingServicesAttachments
                                                    .Where(a => plumbingIdsToDelete.Contains((int)a.PlumbingServiceId)).ToList();

                                                // Bulk remove attachments and plumbing services
                                                attachmentContext.BulkRemove(attachmentsToDelete);
                                                plumbingContext.BulkRemove(deletions);
                                            }
                                        }
                                        // Add new plumbing service if not editing
                                        if (subServiceDTO.IsNew)
                                        {
                                            var newPlumbingService = new TblPlumbingService
                                            {
                                                Description = subServiceDTO.SpecialRequest,
                                                NumberOfItems = Convert.ToInt32(subServiceDTO.NumberItems.TrimEnd('+')),
                                                SubCategoryId = subServiceId,
                                                JobId = existingJob

                                            };
                                            plumbingContext.Save(newPlumbingService);
                                            existingPlumbingId = newPlumbingService.PlumbingServiceId;
                                        }
                                        else
                                        {
                                            var existingPlumbing = plumbingContext.TblPlumbingServices.FirstOrDefault(p => p.SubCategoryId == subServiceDTO.subserviceID && p.JobId == existingJob);
                                            if (existingPlumbing != null)
                                            {
                                                // Update properties from DTO to model
                                                existingPlumbing.Description = subServiceDTO.SpecialRequest ?? existingPlumbing.Description;
                                                existingPlumbing.NumberOfItems = Convert.ToInt32(subServiceDTO.NumberItems.TrimEnd('+'));

                                                plumbingContext.SaveChanges();

                                                existingPlumbingId = existingPlumbing.PlumbingServiceId;
                                            }
                                        }

                                        // Add new images if attached
                                        if (subServiceDTO.Images.Any() && existingPlumbingId > 0)
                                        {
                                            using (var attachmentContext = new AnyHandyDBContext<TblPlumbingServicesAttachment>())
                                            {
                                                var attachmentsToAdd = new List<TblPlumbingServicesAttachment>();

                                                foreach (var image in subServiceDTO.Images)
                                                {
                                                    // Check if the picture is already added
                                                    var alreadyUploadedPic = attachmentContext.TblPlumbingServicesAttachments
                                                        .FirstOrDefault(p => p.FilePath == subServiceDTO.uuid && p.PlumbingServiceId == existingPlumbingId);

                                                    if (alreadyUploadedPic == null)
                                                    {
                                                        var key = subServiceDTO.uuid;
                                                        _storageService.UploadFileAsync(key, image.Content, image.ContentType).Wait(); // Wait for upload to finish

                                                        var attachmentEntity = new TblPlumbingServicesAttachment
                                                        {
                                                            PlumbingServiceId = existingPlumbingId,
                                                            FilePath = key
                                                        };
                                                        attachmentsToAdd.Add(attachmentEntity);
                                                    }
                                                }

                                                if (attachmentsToAdd.Any())
                                                {
                                                    attachmentContext.BulkSave(attachmentsToAdd);
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case "General Handyman Services":
                                    var generalServiceContext = new AnyHandyDBContext<TblGeneralService>();
                                    //if (i == jobDTO.SubServices.Count - 1)
                                    //{
                                    //    // Fetch general handyman services and include related subcategory
                                    //    var list = generalServiceContext.TblGeneralServices.Include(x => x.Job).Where(x => x.JobId == existingJob).ToList();

                                    //    // Identify services to delete
                                    //    var deletions = list.Where(x => !modifiedSubservices.Contains(x.Job)).ToList();

                                    //    // Prepare attachment deletion list
                                    //    var attachmentContext = new AnyHandyDBContext<TblGeneralServiceAttachment>();
                                    //    var generalServiceIdsToDelete = deletions.Select(d => d.GeneralServiceId).ToList();
                                    //    var attachmentsToDelete = attachmentContext.TblGeneralServiceAttachments
                                    //        .Where(a => generalServiceIdsToDelete.Contains((int)a.GeneralServiceServiceId)).ToList();

                                    //    // Bulk remove attachments and general services
                                    //    attachmentContext.BulkRemove(attachmentsToDelete);
                                    //    generalServiceContext.BulkRemove(deletions);

                                    //}
                                    int existingGeneralServiceId = 0;

                                    // Add new general service if subServiceDTO.IsNew is true
                                    if (subServiceDTO.IsNew)
                                    {
                                        var newGeneralService = new TblGeneralService
                                        {
                                            MoreDetailsDescription = subServiceDTO.SpecialRequest,
                                            JobId = existingJob

                                        };
                                        generalServiceContext.Save(newGeneralService);
                                        existingGeneralServiceId = newGeneralService.GeneralServiceId;
                                    }
                                    else
                                    {
                                        var existingGeneralService = generalServiceContext.TblGeneralServices.FirstOrDefault(g => g.JobId == existingJob);
                                        if (existingGeneralService != null)
                                        {
                                            // Update properties from DTO to model
                                            existingGeneralService.MoreDetailsDescription = subServiceDTO.SpecialRequest ?? existingGeneralService.MoreDetailsDescription;
                                            generalServiceContext.SaveChanges();
                                            existingGeneralServiceId = existingGeneralService.GeneralServiceId;
                                        }
                                    }

                                    // Add new images if attached
                                    if (subServiceDTO.Images.Count > 0 && existingGeneralServiceId > 0)
                                    {
                                        var tblGeneralServiceAttachmentContext = new AnyHandyDBContext<TblGeneralServiceAttachment>();
                                        List<TblGeneralServiceAttachment> tblGeneralServiceAttachmentList = new List<TblGeneralServiceAttachment>();

                                        // Remove all the already uploaded images
                                        var alreadyUploadedPics = tblGeneralServiceAttachmentContext.TblGeneralServiceAttachments.Where(p => p.GeneralServiceServiceId == existingGeneralServiceId).ToList();
                                        if (alreadyUploadedPics.Any())
                                            tblGeneralServiceAttachmentContext.BulkRemove(alreadyUploadedPics);

                                        // Add new images
                                        foreach (var image in subServiceDTO.Images)
                                        {
                                            var key = Guid.NewGuid().ToString();
                                            _storageService.UploadFileAsync(key, image.Content, image.ContentType).Wait(); // Wait for upload to finish

                                            var tblGeneralServiceAttachmentEntity = new TblGeneralServiceAttachment
                                            {
                                                GeneralServiceServiceId = existingGeneralServiceId,
                                                FilePath = key
                                            };
                                            tblGeneralServiceAttachmentList.Add(tblGeneralServiceAttachmentEntity);
                                        }

                                        if (tblGeneralServiceAttachmentList.Any())
                                            tblGeneralServiceAttachmentContext.BulkSave(tblGeneralServiceAttachmentList);
                                    }
                                    break;
                                case "Pest Control":
                                    var pestControlContext = new AnyHandyDBContext<TblPestControlService>();
                                    if (i == jobDTO.SubServices.Count - 1)
                                    {
                                        // Fetch pest control services and include related subcategory
                                        var list = pestControlContext.TblPestControlServices.Include(x => x.SubService).Where(x => x.JobId == existingJob).ToList();

                                        // Identify services to delete
                                        var deletions = list.Where(x => !modifiedSubservices.Contains(x.SubService.ServiceNameAr.ToLower())).ToList();
                                        if(deletions.Any())
                                        {
                                            // Bulk remove pest control services
                                            pestControlContext.BulkRemove(deletions);
                                        }

                                    }
                                    int existingPestControlId = 0;

                                    // Add new pest control service if subServiceDTO.IsNew is true
                                    if (subServiceDTO.IsNew)
                                    {
                                        var newPestControlService = new TblPestControlService
                                        {
                                            MoreDetailsDescription = subServiceDTO.SpecialRequest,
                                            NumberRooms = string.IsNullOrEmpty(subServiceDTO.NumberItems) ? (int?)null : Convert.ToInt32(subServiceDTO.NumberItems.Trim('+')),
                                            LocationTypeId = subServiceDTO.LocationType == null ? (int?)null : mainServiceContext.LocationTypes
                                            .FirstOrDefault(p => p.LocationTypeName.Equals(subServiceDTO.LocationType))?.LocationTypeId,

                                            LocationSizeId = subServiceDTO.SizeArea,
                                            RoomTypeId = subServiceDTO.RoomType == null ? (int?)null : mainServiceContext.RoomTypes
                                                .FirstOrDefault(p => p.RoomTypeName.Equals(subServiceDTO.RoomType))?.RoomTypeId,
                                            SubServiceId = subServiceId,
                                            JobId = existingJob
                                        };
                                        pestControlContext.Save(newPestControlService);
                                        existingPestControlId = newPestControlService.PestControlServiceId;
                                    }
                                    else
                                    {
                                        var existingPestControlService = pestControlContext.TblPestControlServices.FirstOrDefault(p => p.SubServiceId == subServiceId && p.JobId == existingJob);
                                        if (existingPestControlService != null)
                                        {
                                            // Update properties from DTO to model
                                            existingPestControlService.MoreDetailsDescription = subServiceDTO.SpecialRequest ?? existingPestControlService.MoreDetailsDescription;
                                            existingPestControlService.NumberRooms = string.IsNullOrEmpty(subServiceDTO.NumberItems) ? existingPestControlService.NumberRooms : Convert.ToInt32(subServiceDTO.NumberItems.Trim('+'));
                                            if (subServiceDTO.LocationType != null)
                                            {
                                                existingPestControlService.LocationTypeId = mainServiceContext.LocationTypes
                                                    .FirstOrDefault(p => p.LocationTypeName.Equals(subServiceDTO.LocationType))?.LocationTypeId ?? existingPestControlService.LocationTypeId;
                                            }
                                            if (subServiceDTO.SizeArea.HasValue)
                                            {
                                                existingPestControlService.LocationSizeId = subServiceDTO.SizeArea.Value;
                                            }
                                            if (subServiceDTO.RoomType != null)
                                            {
                                                existingPestControlService.RoomTypeId = mainServiceContext.RoomTypes
                                                    .FirstOrDefault(p => p.RoomTypeName.Equals(subServiceDTO.RoomType))?.RoomTypeId ?? existingPestControlService.RoomTypeId;
                                            }

                                            pestControlContext.SaveChanges();
                                            existingPestControlId = existingPestControlService.PestControlServiceId;
                                        }
                                    }
                                        break;
                                    case "HVAC":
                                    var hvacContext = new AnyHandyDBContext<TblHvacService>();
                                    if (i == jobDTO.SubServices.Count - 1)
                                    {
                                        // Fetch HVAC services and include related subcategory
                                        var list = hvacContext.TblHvacServices.Include(x => x.SubService).Where(x => x.JobId == existingJob).ToList();

                                        // Identify services to delete
                                        var deletions = list.Where(x => !modifiedSubservices.Contains(x.SubService.ServiceNameEn.ToLower())).ToList();
                                        if(deletions.Any())
                                        {

                                            // Prepare attachment deletion list
                                            var attachmentContext = new AnyHandyDBContext<TblHvacservicesAttachment>();
                                            var hvacIdsToDelete = deletions.Select(d => d.HvacServiceId).ToList();
                                            var attachmentsToDelete = attachmentContext.TblHvacservicesAttachments
                                                .Where(a => hvacIdsToDelete.Contains((int)a.HvacServiceId)).ToList();

                                            // Bulk remove attachments and HVAC services
                                            attachmentContext.BulkRemove(attachmentsToDelete);
                                            hvacContext.BulkRemove(deletions);
                                        }
                                    }
                                    int existingHvacId = 0;

                                    // Add new HVAC service if subServiceDTO.IsNew is true
                                    if (subServiceDTO.IsNew)
                                    {
                                        var newHvacService = new TblHvacService
                                        {
                                            MoreDetailsDescription = subServiceDTO.SpecialRequest,
                                            NumberItems = string.IsNullOrEmpty(subServiceDTO.NumberItems) ? (int?)null : Convert.ToInt32(subServiceDTO.NumberItems.TrimEnd('+')),
                                            SubServiceId = subServiceId,
                                            JobId = existingJob
                                        };
                                        hvacContext.Save(newHvacService);
                                        existingHvacId = newHvacService.HvacServiceId;
                                    }
                                    else
                                    {
                                        var existingHvacService = hvacContext.TblHvacServices.FirstOrDefault(h => h.SubServiceId == subServiceId && h.JobId == existingJob);
                                        if (existingHvacService != null)
                                        {
                                            // Update properties from DTO to model
                                            existingHvacService.MoreDetailsDescription = subServiceDTO.SpecialRequest ?? existingHvacService.MoreDetailsDescription;
                                            existingHvacService.NumberItems = string.IsNullOrEmpty(subServiceDTO.NumberItems) ? existingHvacService.NumberItems : Convert.ToInt32(subServiceDTO.NumberItems.TrimEnd('+'));

                                            hvacContext.SaveChanges();
                                            existingHvacId = existingHvacService.HvacServiceId;
                                        }
                                    }

                                    // Add new images if attached
                                    if (subServiceDTO.Images.Count > 0 && existingHvacId > 0)
                                    {
                                        var hvacAttachmentContext = new AnyHandyDBContext<TblHvacservicesAttachment>();
                                        List<TblHvacservicesAttachment> hvacAttachmentList = new List<TblHvacservicesAttachment>();

                                        // Remove existing attachments
                                        var alreadyUploadedPic = hvacAttachmentContext.TblHvacservicesAttachments.Where(p => p.HvacServiceId == existingHvacId).ToList();
                                        if (alreadyUploadedPic.Any())
                                        {
                                            hvacAttachmentContext.BulkRemove(alreadyUploadedPic);
                                        }

                                        // Upload new images and save their paths
                                        foreach (var image in subServiceDTO.Images)
                                        {
                                            var key = Guid.NewGuid().ToString();
                                            _storageService.UploadFileAsync(key, image.Content, image.ContentType).Wait(); // Wait for upload to finish

                                            var hvacAttachmentEntity = new TblHvacservicesAttachment
                                            {
                                                HvacServiceId = existingHvacId,
                                                FilePath = key
                                            };
                                            hvacAttachmentList.Add(hvacAttachmentEntity);
                                        }
                                        if (hvacAttachmentList.Any())
                                        {
                                            hvacAttachmentContext.BulkSave(hvacAttachmentList);
                                        }
                                    }
                                    break;
                                case "Carpentry Service":
                                    var carpentryContext = new AnyHandyDBContext<TblCarpentryService>();
                                    if (i == jobDTO.SubServices.Count - 1)
                                    {
                                        // Fetch carpentry services and include related subcategory
                                        var list = carpentryContext.TblCarpentryServices.Include(x => x.SubCategory).Where(x => x.JobId == existingJob).ToList();

                                        // Identify services to delete
                                        var deletions = list.Where(x => !modifiedSubservices.Contains(x.SubCategory.ServiceNameEn.ToLower())).ToList();
                                        if (deletions.Any())
                                        {
                                            // Prepare attachment deletion list
                                            var attachmentContext = new AnyHandyDBContext<TblCarpentryServicesAttachment>();
                                            var carpentryIdsToDelete = deletions.Select(d => d.CarpentryServiceId).ToList();
                                            var attachmentsToDelete = attachmentContext.TblCarpentryServicesAttachments
                                                .Where(a => carpentryIdsToDelete.Contains((int)a.CarpentryServiceId)).ToList();

                                            // Bulk remove attachments and carpentry services
                                            attachmentContext.BulkRemove(attachmentsToDelete);
                                            carpentryContext.BulkRemove(deletions);
                                        }
                                    }
                                    int existingCarpentryId = 0;

                                    // Add new carpentry service if subServiceDTO.IsNew is true
                                    if (subServiceDTO.IsNew)
                                    {
                                        var newCarpentryService = new TblCarpentryService
                                        {
                                            Description = subServiceDTO.SpecialRequest,
                                            NumberOfItems = string.IsNullOrEmpty(subServiceDTO.NumberItems) ? (int?)null : Convert.ToInt32(subServiceDTO.NumberItems.TrimEnd('+')),
                                            SubCategoryId = subServiceId,
                                            JobId = existingJob
                                        };
                                        carpentryContext.Save(newCarpentryService);
                                        existingCarpentryId = newCarpentryService.CarpentryServiceId;
                                    }
                                    else
                                    {
                                        var existingCarpentryService = carpentryContext.TblCarpentryServices.FirstOrDefault(c => c.SubCategoryId == subServiceId && c.JobId == existingJob);
                                        if (existingCarpentryService != null)
                                        {
                                            // Update properties from DTO to model
                                            existingCarpentryService.Description = subServiceDTO.SpecialRequest ?? existingCarpentryService.Description;
                                            existingCarpentryService.NumberOfItems = string.IsNullOrEmpty(subServiceDTO.NumberItems) ? existingCarpentryService.NumberOfItems : Convert.ToInt32(subServiceDTO.NumberItems.TrimEnd('+'));

                                            carpentryContext.SaveChanges();
                                            existingCarpentryId = existingCarpentryService.CarpentryServiceId;
                                        }
                                    }

                                    // Add new images if attached
                                    if (subServiceDTO.Images.Count > 0 && existingCarpentryId > 0)
                                    {
                                        var carpentryAttachmentContext = new AnyHandyDBContext<TblCarpentryServicesAttachment>();
                                        List<TblCarpentryServicesAttachment> carpentryAttachmentList = new List<TblCarpentryServicesAttachment>();

                                        // Remove existing attachments
                                        var alreadyUploadedPic = carpentryAttachmentContext.TblCarpentryServicesAttachments.Where(p => p.CarpentryServiceId == existingCarpentryId).ToList();
                                        if (alreadyUploadedPic.Any())
                                        {
                                            carpentryAttachmentContext.BulkRemove(alreadyUploadedPic);
                                        }

                                        // Upload new images and save their paths
                                        foreach (var image in subServiceDTO.Images)
                                        {
                                            var key = Guid.NewGuid().ToString();
                                            _storageService.UploadFileAsync(key, image.Content, image.ContentType).Wait(); // Wait for upload to finish

                                            var carpentryAttachmentEntity = new TblCarpentryServicesAttachment
                                            {
                                                CarpentryServiceId = existingCarpentryId,
                                                FilePath = key
                                            };
                                            carpentryAttachmentList.Add(carpentryAttachmentEntity);
                                        }
                                        if (carpentryAttachmentList.Any())
                                        {
                                            carpentryAttachmentContext.BulkSave(carpentryAttachmentList);
                                        }
                                    }
                                    break;
                                case "Appliance Repairs":
                                    var applianceRepairContext = new AnyHandyDBContext<TblApplianceRepairService>();

                                    if (i == jobDTO.SubServices.Count - 1)
                                    {
                                        // Fetch appliance repair services and include related subcategory
                                        var list = applianceRepairContext.TblApplianceRepairServices.Include(x => x.SubService).Where(x => x.JobId == existingJob).ToList();

                                        // Identify services to delete
                                        
                                        var deletions = list.Where(x => !modifiedSubservices.Contains(x.SubService.ServiceNameEn.ToLower())).ToList();
                                        if(deletions.Any())
                                        {
                                            // Bulk remove appliance repair services
                                            applianceRepairContext.BulkRemove(deletions);
                                        }
                                    }

                                    int existingApplianceRepairId = 0;

                                    // Add new appliance repair service if subServiceDTO.IsNew is true
                                    if (subServiceDTO.IsNew)
                                    {
                                        var newApplianceRepairService = new TblApplianceRepairService
                                        {
                                            MoreDetailsDescription = subServiceDTO.SpecialRequest,
                                            TypeAppliance = subServiceDTO.TypeAppliance,
                                            NumberItems = string.IsNullOrEmpty(subServiceDTO.NumberItems) ? (int?)null : Convert.ToInt32(subServiceDTO.NumberItems.TrimEnd('+')),
                                            SubServiceId = subServiceId,
                                            JobId = existingJob
                                        };
                                        applianceRepairContext.Save(newApplianceRepairService);
                                        existingApplianceRepairId = newApplianceRepairService.ApplianceRepairServiceId;
                                    }
                                    else
                                    {
                                        var existingApplianceRepairService = applianceRepairContext.TblApplianceRepairServices.FirstOrDefault(a => a.SubServiceId == subServiceId && a.JobId == existingJob);
                                        if (existingApplianceRepairService != null)
                                        {
                                            // Update properties from DTO to model
                                            existingApplianceRepairService.MoreDetailsDescription = subServiceDTO.SpecialRequest ?? existingApplianceRepairService.MoreDetailsDescription;
                                            existingApplianceRepairService.TypeAppliance = subServiceDTO.TypeAppliance ?? existingApplianceRepairService.TypeAppliance;
                                            existingApplianceRepairService.NumberItems = string.IsNullOrEmpty(subServiceDTO.NumberItems) ? existingApplianceRepairService.NumberItems : Convert.ToInt32(subServiceDTO.NumberItems.TrimEnd('+'));

                                            applianceRepairContext.SaveChanges();
                                            existingApplianceRepairId = existingApplianceRepairService.ApplianceRepairServiceId;
                                        }
                                    }
                                    break;
                                // Add cases for other service types as needed
                                default:
                                    // Handle default case
                                    break;
                            }
                            i++;
                        }
                    }
                }
            }

        }
        void HandlePaintingChanges(AnyHandyDBContext<TblPaintingService> context,
                            List<TblPaintingService> existingPaintings,
                            List<string> modifiedSubservices,
                            Anyhandy.Models.DTOs.SubService subServiceDTO, int subServiceId, int existingJob)
        {
            // Identify services to delete
            var deletions = existingPaintings.Where(x => !modifiedSubservices.Contains(x.SubCategory.ServiceNameEn.ToLower())).ToList();

            // Prepare attachment deletion list
            var paintingIdsToDelete = deletions.Select(d => d.PaintingServiceId).ToList();
            DeletePaintingAttachments(paintingIdsToDelete);

            // Bulk remove painting services
            context.BulkRemove(deletions);

            // Handle new or existing painting
            if (subServiceDTO.IsNew)
            {
                CreateNewPainting(context, subServiceDTO, subServiceId, existingJob);
            }
            else
            {
                UpdateExistingPainting(context, existingPaintings, subServiceDTO, subServiceId, existingJob);
            }
        }

        void DeletePaintingAttachments(List<int> paintingIds)
        {
            using (var attachmentContext = new AnyHandyDBContext<TblPaintingServicesAttachment>())
            {
                var attachmentsToDelete = attachmentContext.TblPaintingServicesAttachments
                  .Where(a => paintingIds.Contains((int)a.PaintingServiceId))
                  .ToList();
                attachmentContext.BulkRemove(attachmentsToDelete);
            }
        }
        void CreateNewPainting(AnyHandyDBContext<TblPaintingService> context,
                       Anyhandy.Models.DTOs.SubService subServiceDTO, int subServiceId, int existingJob)
        {
            var newPaintingSubservice = new TblPaintingService
            {
                ProvidePaint = subServiceDTO.ProvidePaint,
                PaintColor = subServiceDTO.PaintColor,
                NumberofCoats = subServiceDTO.NumberOfCoats.HasValue && (bool)subServiceDTO.NumberOfCoats ? 2 : 0,
                MoreDetailsDescription = subServiceDTO.SpecialRequest,
                NumberItems = Convert.ToInt32(subServiceDTO.NumberItems?.TrimEnd('+')),
                SizeArea = Convert.ToDecimal(subServiceDTO.SizeArea),
                SpecialRequest = subServiceDTO.SpecialRequest,
                SubCategoryId = subServiceId,
                JobId = existingJob
            };
            context.Save(newPaintingSubservice);
        }
        void UpdateExistingPainting(AnyHandyDBContext<TblPaintingService> context,
                           List<TblPaintingService> existingPaintings,
                           Anyhandy.Models.DTOs.SubService subServiceDTO, int subServiceId, int existingJob)
        {
            var existingPainting = existingPaintings.FirstOrDefault(p => p.SubCategoryId == subServiceId && p.JobId == existingJob);
            if (existingPainting != null)
            {
                existingPainting.ProvidePaint = subServiceDTO.ProvidePaint ?? existingPainting.ProvidePaint;
                existingPainting.PaintColor = subServiceDTO.PaintColor ?? existingPainting.PaintColor;
                existingPainting.NumberofCoats = subServiceDTO.NumberOfCoats.HasValue && (bool)subServiceDTO.NumberOfCoats ? 2 : 0;
                existingPainting.MoreDetailsDescription = subServiceDTO.SpecialRequest ?? existingPainting.SpecialRequest;
                existingPainting.NumberItems = Convert.ToInt32(subServiceDTO.NumberItems?.TrimEnd('+'));
                existingPainting.SizeArea = Convert.ToDecimal(subServiceDTO.SizeArea);
                existingPainting.SpecialRequest = subServiceDTO.SpecialRequest ?? existingPainting.SpecialRequest;
                context.SaveChanges();
            }
        }
        public void updateUserLocation(MainServiceDTO jobDTO)
        {
            if (jobDTO.Location.AddressType != null || jobDTO.Location.Details != null)
            {
                using (var userAddressContext = new AnyHandyDBContext<UserAddress>())
                using (var countryContext = new AnyHandyDBContext<Country>())
                using (var cityContext = new AnyHandyDBContext<City>())
                {
                    // Retrieve the existing UserAddress based on the userID
                    var userAddressEntity = userAddressContext.UserAddresses.FirstOrDefault(ua => ua.AddressId == jobDTO.Location.AddressId);

                    if (userAddressEntity != null)
                    {
                        // Update the existing UserAddress entity instance
                        userAddressEntity.AddressType = jobDTO.Location.AddressType ?? userAddressEntity.AddressType;
                        userAddressEntity.City = cityContext.Cities.FirstOrDefault(x => x.Name == jobDTO.Location.City)?.CityId ?? userAddressEntity.City;
                        userAddressEntity.Country = countryContext.Countries.FirstOrDefault(x => x.Name == jobDTO.Location.Country)?.CountryId ?? userAddressEntity.Country;
                        userAddressEntity.Details = jobDTO.Location.Details ?? userAddressEntity.Details;

                        // Save changes to the database
                        userAddressContext.SaveChanges();
                    }
                    else
                    {
                        // Handle the case where the user address is not found (e.g., create a new address or throw an exception)
                        throw new Exception("User address not found");
                    }
                }
            }

        }



        public void updateJobDetails(AnyHandyDBContext<Job> context, Job existingJobPost, MainServiceDTO jobDTO)
        {
            // Update relevant properties of existingJobPost based on form data
            existingJobPost.JobDetails = jobDTO.MainServiceDescription;
            if(jobDTO.JobDetails.StartImmediatly)
            {
               
                existingJobPost.DueDate = null;
                existingJobPost.JobStart = null;
                existingJobPost.JobExpectedEnd = null;
            }
            else
            {
                existingJobPost.DueDate = jobDTO.JobDetails.EndDate;
                existingJobPost.JobStart = jobDTO.JobDetails.StartDate;
                existingJobPost.JobExpectedEnd = jobDTO.JobDetails.EndDate;
            }
            existingJobPost.EndRate = jobDTO.JobDetails.EndRate;
            existingJobPost.StartRate = jobDTO.JobDetails.StartRate;
            existingJobPost.Amount = jobDTO.JobDetails.FixedPriceAmount == null ? null : int.Parse(jobDTO.JobDetails.FixedPriceAmount);
           
           

            // Since the context is passed, changes are tracked and context.SaveChanges() should be called outside this method
        }

        public Job GetJobPostById(AnyHandyDBContext<Job> context, int id)
        {
            return context.Jobs.FirstOrDefault(p => p.JobId == id);
        }

        public bool IsJobPostExistsById(int id)
        {
            using (var jobContext = new AnyHandyDBContext<Job>())
            {
                var job = jobContext.Jobs
               .FirstOrDefault(p => p.JobId == id);
                return job == null ? false : true;
            }
        }
        private static DbSet<DbContext> GetDbSet(DbContext context, Type type)
        {
            // Use reflection to get the generic Set method from DbContext
            var method = typeof(DbContext).GetMethod("Set", new Type[0]);

            // Make the generic method with the specific type
            var genericMethod = method.MakeGenericMethod(type);

            // Invoke the method on the context instance to get the DbSet
            return (DbSet<DbContext>)genericMethod.Invoke(context, null);
        }

    }
}
