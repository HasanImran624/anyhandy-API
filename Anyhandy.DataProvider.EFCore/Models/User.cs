using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class User
    {
        public User()
        {
            HandymanServicesLocations = new HashSet<HandymanServicesLocation>();
            JobContracts = new HashSet<JobContract>();
            JobProposals = new HashSet<JobProposal>();
            Jobs = new HashSet<Job>();
            MessageReceivers = new HashSet<Message>();
            MessageSenders = new HashSet<Message>();
            UserAddresses = new HashSet<UserAddress>();
            UserPackagePurchaseRequests = new HashSet<UserPackagePurchaseRequest>();
            UserPackages = new HashSet<UserPackage>();
            UserProfileServices = new HashSet<UserProfileService>();
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

        public virtual ICollection<HandymanServicesLocation> HandymanServicesLocations { get; set; }
        public virtual ICollection<JobContract> JobContracts { get; set; }
        public virtual ICollection<JobProposal> JobProposals { get; set; }
        public virtual ICollection<Job> Jobs { get; set; }
        public virtual ICollection<Message> MessageReceivers { get; set; }
        public virtual ICollection<Message> MessageSenders { get; set; }
        public virtual ICollection<UserAddress> UserAddresses { get; set; }
        public virtual ICollection<UserPackagePurchaseRequest> UserPackagePurchaseRequests { get; set; }
        public virtual ICollection<UserPackage> UserPackages { get; set; }
        public virtual ICollection<UserProfileService> UserProfileServices { get; set; }
    }
}
