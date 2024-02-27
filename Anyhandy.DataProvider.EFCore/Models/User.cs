using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class User
    {
        public User()
        {
            Handymanserviceslocations = new HashSet<Handymanserviceslocation>();
            Jobcontracts = new HashSet<Jobcontract>();
            Jobproposals = new HashSet<Jobproposal>();
            Jobs = new HashSet<Job>();
            MessageReceivers = new HashSet<Message>();
            MessageSenders = new HashSet<Message>();
            Useraddresses = new HashSet<Useraddress>();
            Userpackagepurchaserequests = new HashSet<Userpackagepurchaserequest>();
            Userpackages = new HashSet<Userpackage>();
            Userprofileservices = new HashSet<Userprofileservice>();
        }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Picture { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string MobileNumber { get; set; }
        public string Paswword { get; set; }
        public bool? IsHandyman { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Handymanserviceslocation> Handymanserviceslocations { get; set; }
        public virtual ICollection<Jobcontract> Jobcontracts { get; set; }
        public virtual ICollection<Jobproposal> Jobproposals { get; set; }
        public virtual ICollection<Job> Jobs { get; set; }
        public virtual ICollection<Message> MessageReceivers { get; set; }
        public virtual ICollection<Message> MessageSenders { get; set; }
        public virtual ICollection<Useraddress> Useraddresses { get; set; }
        public virtual ICollection<Userpackagepurchaserequest> Userpackagepurchaserequests { get; set; }
        public virtual ICollection<Userpackage> Userpackages { get; set; }
        public virtual ICollection<Userprofileservice> Userprofileservices { get; set; }
    }
}
