using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anyhandy.Models.DTOs
{
    public class SubServiceImage
    {
        public string FileName { get; set; }
        public string ContentType { get; set; } // Optional: Store content type (e.g., image/jpeg)
        public byte[] Content { get; set; } // Store image data as byte array
    }


    public class MainServiceDTO
    {
        public string MainServiceDescription { get; set; }
        public string SelectedMainServiceCode { get; set; }
        public List<SubService> SubServices { get; set; }
        public Location Location { get; set; }
        public JobDetails JobDetails { get; set; }
        public string MainServiceName { get; set; }
    }

    public class SubService
    {
        public List<SubServiceImage> Images { get; set; } = new List<SubServiceImage> { };
        public string? NumberItems { get; set; }
        public int? SizeArea { get; set; }
        public string? PaintColor { get; set; }
        public bool? ProvidePaint { get; set; }
        public bool? NumberOfCoats { get; set; }
        public string? SpecialRequest { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string uuid { get; set; }

        // pest control
        public string? RoomType { get; set; }
        public string? LocationType { get; set; }
        public bool? ForOfficeType { get; set; }
        // appliance rep
        public string? TypeAppliance { get; set; }
        // home cleaning
        public bool? ProvideSupplies { get; set; }
        public string? NumberCleaner { get; set; }
        public string? NumberHours { get; set; }
        public string? TypeFurniture { get; set; }

        public string? areaType { get; set; }

    }

    public class Location
    {
        public string? City { get; set; }
        public string? AddressType { get; set; }
        public string? NumberAndBuildingName { get; set; }
        public string? Details { get; set; }
        public string? Area { get; set; }
    }

    public class JobDetails
    {
        public bool? IsHourlyRate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsFixRate { get; set; }
        public string? StartRate { get; set; }
        public string? EndRate { get; set; }
        public bool StartImmediatly { get; set; }
        public string? FixedPriceAmount { get; set; }

    }

}
