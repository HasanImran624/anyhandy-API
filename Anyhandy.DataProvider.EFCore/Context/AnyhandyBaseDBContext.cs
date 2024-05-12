using System;
using Anyhandy.Common;
using Anyhandy.DataProvider.EFCore.Models;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Context
{
    public partial class AnyhandyBaseDBContext : DbContext
    {
        public AnyhandyBaseDBContext()
        {
        }

        public AnyhandyBaseDBContext(DbContextOptions<AnyhandyBaseDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<AreaType> AreaTypes { get; set; }
        public virtual DbSet<ContractMilestone> ContractMilestones { get; set; }
        public virtual DbSet<ContractMilestonesPayment> ContractMilestonesPayments { get; set; }
        public virtual DbSet<HandymanServicesLocation> HandymanServicesLocations { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<JobAttachment> JobAttachments { get; set; }
        public virtual DbSet<JobContract> JobContracts { get; set; }
        public virtual DbSet<JobProposal> JobProposals { get; set; }
        public virtual DbSet<JobProposalAttachment> JobProposalAttachments { get; set; }
        public virtual DbSet<JobService> JobServices { get; set; }
        public virtual DbSet<LocationType> LocationTypes { get; set; }
        public virtual DbSet<MainService> MainServices { get; set; }
        public virtual DbSet<MainService1> MainServices1 { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<MessageAttachment> MessageAttachments { get; set; }
        public virtual DbSet<RoomType> RoomTypes { get; set; }
        public virtual DbSet<SubService> SubServices { get; set; }
        public virtual DbSet<SubService1> SubServices1 { get; set; }
        public virtual DbSet<TblApplianceRepairService> TblApplianceRepairServices { get; set; }
        public virtual DbSet<TblCarpentryService> TblCarpentryServices { get; set; }
        public virtual DbSet<TblCarpentryServicesAttachment> TblCarpentryServicesAttachments { get; set; }
        public virtual DbSet<TblElectricalService> TblElectricalServices { get; set; }
        public virtual DbSet<TblElectricalServicesAttachment> TblElectricalServicesAttachments { get; set; }
        public virtual DbSet<TblGeneralService> TblGeneralServices { get; set; }
        public virtual DbSet<TblGeneralServiceAttachment> TblGeneralServiceAttachments { get; set; }
        public virtual DbSet<TblHomeCleaning> TblHomeCleanings { get; set; }
        public virtual DbSet<TblHomeCleaningAttachment> TblHomeCleaningAttachments { get; set; }
        public virtual DbSet<TblHvacService> TblHvacServices { get; set; }
        public virtual DbSet<TblHvacservicesAttachment> TblHvacservicesAttachments { get; set; }
        public virtual DbSet<TblLandscapingService> TblLandscapingServices { get; set; }
        public virtual DbSet<TblLandscapingServiceAttachment> TblLandscapingServiceAttachments { get; set; }
        public virtual DbSet<TblPaintingService> TblPaintingServices { get; set; }
        public virtual DbSet<TblPaintingServicesAttachment> TblPaintingServicesAttachments { get; set; }
        public virtual DbSet<TblPestControlService> TblPestControlServices { get; set; }
        public virtual DbSet<TblPlumbingService> TblPlumbingServices { get; set; }
        public virtual DbSet<TblPlumbingServicesAttachment> TblPlumbingServicesAttachments { get; set; }
        public virtual DbSet<TypeFurniture> TypeFurnitures { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserAddress> UserAddresses { get; set; }
        public virtual DbSet<UserPackage> UserPackages { get; set; }
        public virtual DbSet<UserProfileRating> UserProfileRatings { get; set; }
        public virtual DbSet<UserPackagePurchaseRequest> UserPackagePurchaseRequests { get; set; }
        public virtual DbSet<UserProfileService> UserProfileServices { get; set; }
        public virtual DbSet<UserType> UserTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL(AppConfigrationManager.AppSettings["AnyhandyDatabase"]);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("countries");

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("cities");

                entity.HasIndex(e => e.CountryId, "CountryID");

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Cities)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("cities_ibfk_1");
            });

            modelBuilder.Entity<AreaType>(entity =>
            {
                entity.ToTable("AreaType");

                entity.Property(e => e.AreaTypeId).HasColumnName("AreaTypeID");

                entity.Property(e => e.AreaTypeName).HasMaxLength(50);
            });

            modelBuilder.Entity<ContractMilestone>(entity =>
            {
                entity.HasKey(e => e.MilestoneId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.JobContractId, "JobContractID");

                entity.Property(e => e.MilestoneId).HasColumnName("MilestoneID");

                entity.Property(e => e.Amount).HasColumnType("decimal(10,2)");

                entity.Property(e => e.JobContractId).HasColumnName("JobContractID");

                entity.HasOne(d => d.JobContract)
                    .WithMany(p => p.ContractMilestones)
                    .HasForeignKey(d => d.JobContractId)
                    .HasConstraintName("ContractMilestones_ibfk_1");
            });

            modelBuilder.Entity<ContractMilestonesPayment>(entity =>
            {
                entity.ToTable("ContractMilestonesPayment");

                entity.HasIndex(e => e.MilestoneId, "MilestoneID");

                entity.Property(e => e.ContractMilestonesPaymentId).HasColumnName("ContractMilestonesPaymentID");

                entity.Property(e => e.Amount).HasColumnType("decimal(10,2)");

                entity.Property(e => e.MilestoneId).HasColumnName("MilestoneID");

                entity.Property(e => e.PaymentReference)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Milestone)
                    .WithMany(p => p.ContractMilestonesPayments)
                    .HasForeignKey(d => d.MilestoneId)
                    .HasConstraintName("ContractMilestonesPayment_ibfk_1");
            });

            modelBuilder.Entity<HandymanServicesLocation>(entity =>
            {
                entity.HasKey(e => e.ServiceLocationId)
                    .HasName("PRIMARY");

                entity.ToTable("HandymanServicesLocation");

                entity.HasIndex(e => e.UserId, "UserID");

                entity.Property(e => e.ServiceLocationId).HasColumnName("ServiceLocationID");

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HandymanServicesLocations)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("HandymanServicesLocation_ibfk_1");
            });

            modelBuilder.Entity<Job>(entity =>
            {
                entity.ToTable("Job");

                entity.HasIndex(e => e.JobAddressId, "JobAddressID");

                entity.HasIndex(e => e.MainServicesId, "MainServicesID");

                entity.HasIndex(e => e.UserId, "UserID");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.Amount).HasColumnType("decimal(10,2)");

                entity.Property(e => e.DueDate).HasColumnType("date");

                entity.Property(e => e.JobAddressId).HasColumnName("JobAddressID");

                entity.Property(e => e.JobTitle).HasMaxLength(250);

                entity.Property(e => e.MainServicesId).HasColumnName("MainServicesID");

                entity.Property(e => e.PostedDate).HasColumnType("date");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.JobAddress)
                    .WithMany(p => p.Jobs)
                    .HasForeignKey(d => d.JobAddressId)
                    .HasConstraintName("Job_ibfk_3");

                entity.HasOne(d => d.MainServices)
                    .WithMany(p => p.Jobs)
                    .HasForeignKey(d => d.MainServicesId)
                    .HasConstraintName("Job_ibfk_1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Jobs)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("Job_ibfk_2");
            });

            modelBuilder.Entity<JobAttachment>(entity =>
            {
                entity.HasKey(e => e.AttachmentId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.JobId, "JobID");

                entity.Property(e => e.AttachmentId).HasColumnName("AttachmentID");

                entity.Property(e => e.FilePath)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.FileShortDescription).HasMaxLength(100);

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.JobAttachments)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("JobAttachments_ibfk_1");
            });

            modelBuilder.Entity<JobContract>(entity =>
            {
                entity.ToTable("JobContract");

                entity.HasIndex(e => e.JobId, "JobID");

                entity.HasIndex(e => e.JobProposalId, "JobProposalID");

                entity.HasIndex(e => e.SelectedHandyManId, "SelectedHandyManID");

                entity.Property(e => e.JobContractId).HasColumnName("JobContractID");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.JobProposalId).HasColumnName("JobProposalID");

                entity.Property(e => e.SelectedHandyManId).HasColumnName("SelectedHandyManID");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.JobContracts)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("JobContract_ibfk_1");

                entity.HasOne(d => d.JobProposal)
                    .WithMany(p => p.JobContracts)
                    .HasForeignKey(d => d.JobProposalId)
                    .HasConstraintName("JobContract_ibfk_3");

                entity.HasOne(d => d.SelectedHandyMan)
                    .WithMany(p => p.JobContracts)
                    .HasForeignKey(d => d.SelectedHandyManId)
                    .HasConstraintName("JobContract_ibfk_2");
            });

            modelBuilder.Entity<JobProposal>(entity =>
            {
                entity.HasIndex(e => e.JobId, "JobID");

                entity.HasIndex(e => e.UserId, "UserID");

                entity.Property(e => e.JobProposalId).HasColumnName("JobProposalID");

                entity.Property(e => e.Amount).HasColumnType("decimal(10,2)");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.Status).HasColumnType("enum('Accepted','Rejected','Pending')");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.JobProposals)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("JobProposals_ibfk_1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.JobProposals)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("JobProposals_ibfk_2");
            });

            modelBuilder.Entity<JobProposalAttachment>(entity =>
            {
                entity.HasKey(e => e.AttachmentId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.JobProposalId, "JobProposalID");

                entity.Property(e => e.AttachmentId).HasColumnName("AttachmentID");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.FilePath)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.JobProposalId).HasColumnName("JobProposalID");

                entity.HasOne(d => d.JobProposal)
                    .WithMany(p => p.JobProposalAttachments)
                    .HasForeignKey(d => d.JobProposalId)
                    .HasConstraintName("JobProposalAttachments_ibfk_1");
            });

            modelBuilder.Entity<JobService>(entity =>
            {
                entity.HasIndex(e => e.JobId, "JobID");

                entity.HasIndex(e => e.JobSubServiceId, "JobSubServiceID");

                entity.Property(e => e.JobServiceId).HasColumnName("JobServiceID");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.JobSubServiceId).HasColumnName("JobSubServiceID");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.JobServices)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("JobServices_ibfk_1");

                entity.HasOne(d => d.JobSubService)
                    .WithMany(p => p.JobServices)
                    .HasForeignKey(d => d.JobSubServiceId)
                    .HasConstraintName("JobServices_ibfk_2");
            });

            modelBuilder.Entity<LocationType>(entity =>
            {
                entity.ToTable("LocationType");

                entity.Property(e => e.LocationTypeId).HasColumnName("LocationTypeID");

                entity.Property(e => e.LocationTypeName).HasMaxLength(50);
            });

            modelBuilder.Entity<MainService>(entity =>
            {
                entity.HasKey(e => e.MainServicesId)
                    .HasName("PRIMARY");

                entity.Property(e => e.MainServicesId).HasColumnName("main_servicesID");

                entity.Property(e => e.ServiceNameAr)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("service_nameAr");

                entity.Property(e => e.ServiceNameEn)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("service_nameEn");
            });

            modelBuilder.Entity<MainService1>(entity =>
            {
                entity.HasKey(e => e.MainServiceId)
                    .HasName("PRIMARY");

                entity.ToTable("main_services");

                entity.Property(e => e.MainServiceId).HasColumnName("main_service_id");

                entity.Property(e => e.MainServiceName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("main_service_name");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasIndex(e => e.ReceiverId, "receiver_id");

                entity.HasIndex(e => e.SenderId, "sender_id");

                entity.Property(e => e.MessageId).HasColumnName("message_id");

                entity.Property(e => e.MessageText)
                    .IsRequired()
                    .HasColumnName("message_text");

                entity.Property(e => e.ReceiverId).HasColumnName("receiver_id");

                entity.Property(e => e.SenderId).HasColumnName("sender_id");

                entity.HasOne(d => d.Receiver)
                    .WithMany(p => p.MessageReceivers)
                    .HasForeignKey(d => d.ReceiverId)
                    .HasConstraintName("Messages_ibfk_2");

                entity.HasOne(d => d.Sender)
                    .WithMany(p => p.MessageSenders)
                    .HasForeignKey(d => d.SenderId)
                    .HasConstraintName("Messages_ibfk_1");
            });

            modelBuilder.Entity<MessageAttachment>(entity =>
            {
                entity.HasKey(e => e.AttachmentId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.MessageId, "MessageID");

                entity.Property(e => e.AttachmentId).HasColumnName("AttachmentID");

                entity.Property(e => e.FilePath)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.MessageId).HasColumnName("MessageID");

                entity.HasOne(d => d.Message)
                    .WithMany(p => p.MessageAttachments)
                    .HasForeignKey(d => d.MessageId)
                    .HasConstraintName("MessageAttachments_ibfk_1");
            });

            modelBuilder.Entity<RoomType>(entity =>
            {
                entity.ToTable("RoomType");

                entity.Property(e => e.RoomTypeId).HasColumnName("RoomTypeID");

                entity.Property(e => e.RoomTypeName).HasMaxLength(50);
            });

            modelBuilder.Entity<SubService>(entity =>
            {
                entity.HasKey(e => e.SubServicesId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.MainServicesId, "main_servicesID");

                entity.Property(e => e.SubServicesId).HasColumnName("sub_servicesID");

                entity.Property(e => e.MainServicesId).HasColumnName("main_servicesID");

                entity.Property(e => e.ServiceNameAr)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("service_nameAr");

                entity.Property(e => e.ServiceNameEn)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("service_nameEn");

                entity.HasOne(d => d.MainServices)
                    .WithMany(p => p.SubServices)
                    .HasForeignKey(d => d.MainServicesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SubServices_ibfk_1");
            });

            modelBuilder.Entity<SubService1>(entity =>
            {
                entity.HasKey(e => e.SubServiceId)
                    .HasName("PRIMARY");

                entity.ToTable("sub_services");

                entity.HasIndex(e => e.MainServiceId, "main_service_id");

                entity.Property(e => e.SubServiceId).HasColumnName("sub_service_id");

                entity.Property(e => e.MainServiceId).HasColumnName("main_service_id");

                entity.Property(e => e.SubServiceName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("sub_service_name");

                entity.HasOne(d => d.MainService)
                    .WithMany(p => p.SubService1s)
                    .HasForeignKey(d => d.MainServiceId)
                    .HasConstraintName("sub_services_ibfk_1");
            });

            modelBuilder.Entity<TblApplianceRepairService>(entity =>
            {
                entity.HasKey(e => e.ApplianceRepairServiceId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_ApplianceRepairService");

                entity.HasIndex(e => e.JobId, "JobID");

                entity.HasIndex(e => e.SubServiceId, "SubServiceID");

                entity.Property(e => e.ApplianceRepairServiceId).HasColumnName("ApplianceRepairServiceID");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.SubServiceId).HasColumnName("SubServiceID");

                entity.Property(e => e.TypeAppliance).HasMaxLength(255);

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.TblApplianceRepairServices)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("tbl_ApplianceRepairService_ibfk_1");

                entity.HasOne(d => d.SubService)
                    .WithMany(p => p.TblApplianceRepairServices)
                    .HasForeignKey(d => d.SubServiceId)
                    .HasConstraintName("tbl_ApplianceRepairService_ibfk_2");
            });

            modelBuilder.Entity<TblCarpentryService>(entity =>
            {
                entity.HasKey(e => e.CarpentryServiceId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_CarpentryServices");

                entity.HasIndex(e => e.JobId, "JobID");

                entity.HasIndex(e => e.SubCategoryId, "SubCategoryID");

                entity.Property(e => e.CarpentryServiceId).HasColumnName("CarpentryServiceID");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.SubCategoryId).HasColumnName("SubCategoryID");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.TblCarpentryServices)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("tbl_CarpentryServices_ibfk_1");

                entity.HasOne(d => d.SubCategory)
                    .WithMany(p => p.TblCarpentryServices)
                    .HasForeignKey(d => d.SubCategoryId)
                    .HasConstraintName("tbl_CarpentryServices_ibfk_2");
            });

            modelBuilder.Entity<TblCarpentryServicesAttachment>(entity =>
            {
                entity.HasKey(e => e.CarpentryServicesAttachmentsId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_CarpentryServicesAttachments");

                entity.HasIndex(e => e.CarpentryServiceId, "CarpentryServiceID");

                entity.Property(e => e.CarpentryServicesAttachmentsId).HasColumnName("CarpentryServicesAttachmentsID");

                entity.Property(e => e.CarpentryServiceId).HasColumnName("CarpentryServiceID");

                entity.Property(e => e.FilePath)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.CarpentryService)
                    .WithMany(p => p.TblCarpentryServicesAttachments)
                    .HasForeignKey(d => d.CarpentryServiceId)
                    .HasConstraintName("tbl_CarpentryServicesAttachments_ibfk_1");
            });

            modelBuilder.Entity<TblElectricalService>(entity =>
            {
                entity.HasKey(e => e.ElectricalServiceId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_ElectricalServices");

                entity.HasIndex(e => e.JobId, "JobID");

                entity.HasIndex(e => e.SubCategoryId, "SubCategoryID");

                entity.Property(e => e.ElectricalServiceId).HasColumnName("ElectricalServiceID");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.SubCategoryId).HasColumnName("SubCategoryID");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.TblElectricalServices)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("tbl_ElectricalServices_ibfk_1");

                entity.HasOne(d => d.SubCategory)
                    .WithMany(p => p.TblElectricalServices)
                    .HasForeignKey(d => d.SubCategoryId)
                    .HasConstraintName("tbl_ElectricalServices_ibfk_2");
            });

            modelBuilder.Entity<TblElectricalServicesAttachment>(entity =>
            {
                entity.HasKey(e => e.ElectricalServicesAttachmentsId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_ElectricalServicesAttachments");

                entity.HasIndex(e => e.ElectricalServiceId, "ElectricalServiceID");

                entity.Property(e => e.ElectricalServicesAttachmentsId).HasColumnName("ElectricalServicesAttachmentsID");

                entity.Property(e => e.ElectricalServiceId).HasColumnName("ElectricalServiceID");

                entity.Property(e => e.FilePath)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.ElectricalService)
                    .WithMany(p => p.TblElectricalServicesAttachments)
                    .HasForeignKey(d => d.ElectricalServiceId)
                    .HasConstraintName("tbl_ElectricalServicesAttachments_ibfk_1");
            });

            modelBuilder.Entity<TblGeneralService>(entity =>
            {
                entity.HasKey(e => e.GeneralServiceId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_GeneralService");

                entity.HasIndex(e => e.JobId, "JobID");

                entity.Property(e => e.GeneralServiceId).HasColumnName("GeneralServiceID");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.TblGeneralServices)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("tbl_GeneralService_ibfk_1");
            });

            modelBuilder.Entity<TblGeneralServiceAttachment>(entity =>
            {
                entity.HasKey(e => e.AttachmentId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_GeneralServiceAttachment");

                entity.HasIndex(e => e.GeneralServiceServiceId, "GeneralServiceServiceID");

                entity.Property(e => e.AttachmentId).HasColumnName("AttachmentID");

                entity.Property(e => e.FilePath).HasMaxLength(255);

                entity.Property(e => e.GeneralServiceServiceId).HasColumnName("GeneralServiceServiceID");

                entity.HasOne(d => d.GeneralServiceService)
                    .WithMany(p => p.TblGeneralServiceAttachments)
                    .HasForeignKey(d => d.GeneralServiceServiceId)
                    .HasConstraintName("tbl_GeneralServiceAttachment_ibfk_1");
            });

            modelBuilder.Entity<TblHomeCleaning>(entity =>
            {
                entity.HasKey(e => e.HomeCleaningServiceId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_HomeCleaning");

                entity.HasIndex(e => e.AreaTypeId, "AreaTypeID");

                entity.HasIndex(e => e.JobId, "JobID");

                entity.HasIndex(e => e.LocationTypeId, "LocationTypeID");

                entity.HasIndex(e => e.SubServiceId, "SubServiceID");

                entity.HasIndex(e => e.TypeFurnitureId, "TypeFurnitureID");

                entity.Property(e => e.HomeCleaningServiceId).HasColumnName("HomeCleaningServiceID");

                entity.Property(e => e.AreaTypeId).HasColumnName("AreaTypeID");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.LocationTypeId).HasColumnName("LocationTypeID");

                entity.Property(e => e.SizeItems).HasMaxLength(255);

                entity.Property(e => e.SubServiceId).HasColumnName("SubServiceID");

                entity.Property(e => e.TypeFurnitureId).HasColumnName("TypeFurnitureID");

                entity.HasOne(d => d.AreaType)
                    .WithMany(p => p.TblHomeCleanings)
                    .HasForeignKey(d => d.AreaTypeId)
                    .HasConstraintName("tbl_HomeCleaning_ibfk_4");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.TblHomeCleanings)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("tbl_HomeCleaning_ibfk_1");

                entity.HasOne(d => d.LocationType)
                    .WithMany(p => p.TblHomeCleanings)
                    .HasForeignKey(d => d.LocationTypeId)
                    .HasConstraintName("tbl_HomeCleaning_ibfk_3");

                entity.HasOne(d => d.SubService)
                    .WithMany(p => p.TblHomeCleanings)
                    .HasForeignKey(d => d.SubServiceId)
                    .HasConstraintName("tbl_HomeCleaning_ibfk_2");

                entity.HasOne(d => d.TypeFurniture)
                    .WithMany(p => p.TblHomeCleanings)
                    .HasForeignKey(d => d.TypeFurnitureId)
                    .HasConstraintName("tbl_HomeCleaning_ibfk_5");
            });

            modelBuilder.Entity<TblHomeCleaningAttachment>(entity =>
            {
                entity.HasKey(e => e.HomeCleaningAttachmentId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_HomeCleaningAttachments");

                entity.HasIndex(e => e.HomeCleaningServiceId, "HomeCleaningServiceID");

                entity.Property(e => e.HomeCleaningAttachmentId).HasColumnName("HomeCleaningAttachmentID");

                entity.Property(e => e.FilePath).HasMaxLength(255);

                entity.Property(e => e.HomeCleaningServiceId).HasColumnName("HomeCleaningServiceID");

                entity.HasOne(d => d.HomeCleaningService)
                    .WithMany(p => p.TblHomeCleaningAttachments)
                    .HasForeignKey(d => d.HomeCleaningServiceId)
                    .HasConstraintName("tbl_HomeCleaningAttachments_ibfk_1");
            });

            modelBuilder.Entity<TblHvacService>(entity =>
            {
                entity.HasKey(e => e.HvacServiceId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_HvacServices");

                entity.HasIndex(e => e.JobId, "JobID");

                entity.HasIndex(e => e.SubServiceId, "SubServiceID");

                entity.Property(e => e.HvacServiceId).HasColumnName("HvacServiceID");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.SubServiceId).HasColumnName("SubServiceID");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.TblHvacServices)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("tbl_HvacServices_ibfk_1");

                entity.HasOne(d => d.SubService)
                    .WithMany(p => p.TblHvacServices)
                    .HasForeignKey(d => d.SubServiceId)
                    .HasConstraintName("tbl_HvacServices_ibfk_2");
            });

            modelBuilder.Entity<TblHvacservicesAttachment>(entity =>
            {
                entity.HasKey(e => e.HvacServicesAttachmentsId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_HVACServicesAttachments");

                entity.HasIndex(e => e.HvacServiceId, "HvacServiceID");

                entity.Property(e => e.HvacServicesAttachmentsId).HasColumnName("HvacServicesAttachmentsID");

                entity.Property(e => e.FilePath)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.HvacServiceId).HasColumnName("HvacServiceID");

                entity.HasOne(d => d.HvacService)
                    .WithMany(p => p.TblHvacservicesAttachments)
                    .HasForeignKey(d => d.HvacServiceId)
                    .HasConstraintName("tbl_HVACServicesAttachments_ibfk_1");
            });

            modelBuilder.Entity<TblLandscapingService>(entity =>
            {
                entity.HasKey(e => e.LandscapingServiceId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_LandscapingService");

                entity.HasIndex(e => e.JobId, "JobID");

                entity.HasIndex(e => e.SubServicesId, "SubServicesID");

                entity.Property(e => e.LandscapingServiceId).HasColumnName("LandscapingServiceID");

                entity.Property(e => e.AreaSize).HasColumnType("decimal(10,2)");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.LandscapingService)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.SubServicesId).HasColumnName("SubServicesID");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.TblLandscapingServices)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("tbl_LandscapingService_ibfk_1");

                entity.HasOne(d => d.SubServices)
                    .WithMany(p => p.TblLandscapingServices)
                    .HasForeignKey(d => d.SubServicesId)
                    .HasConstraintName("tbl_LandscapingService_ibfk_2");
            });

            modelBuilder.Entity<TblLandscapingServiceAttachment>(entity =>
            {
                entity.HasKey(e => e.LandscapingServiceAttachmentId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_LandscapingServiceAttachment");

                entity.HasIndex(e => e.LandscapingServiceId, "LandscapingServiceID");

                entity.Property(e => e.LandscapingServiceAttachmentId).HasColumnName("LandscapingServiceAttachmentID");

                entity.Property(e => e.AttachmentPath)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.LandscapingServiceId).HasColumnName("LandscapingServiceID");

                entity.HasOne(d => d.LandscapingService)
                    .WithMany(p => p.TblLandscapingServiceAttachments)
                    .HasForeignKey(d => d.LandscapingServiceId)
                    .HasConstraintName("tbl_LandscapingServiceAttachment_ibfk_1");
            });

            modelBuilder.Entity<TblPaintingService>(entity =>
            {
                entity.HasKey(e => e.PaintingServiceId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_PaintingServices");

                entity.HasIndex(e => e.JobId, "JobID");

                entity.HasIndex(e => e.SubCategoryId, "SubCategoryID");

                entity.Property(e => e.PaintingServiceId).HasColumnName("PaintingServiceID");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.PaintColor).HasMaxLength(255);

                entity.Property(e => e.SizeArea).HasColumnType("decimal(10,2)");

                entity.Property(e => e.SubCategoryId).HasColumnName("SubCategoryID");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.TblPaintingServices)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("tbl_PaintingServices_ibfk_1");

                entity.HasOne(d => d.SubCategory)
                    .WithMany(p => p.TblPaintingServices)
                    .HasForeignKey(d => d.SubCategoryId)
                    .HasConstraintName("tbl_PaintingServices_ibfk_2");
            });

            modelBuilder.Entity<TblPaintingServicesAttachment>(entity =>
            {
                entity.HasKey(e => e.PaintingServicesAttachmentsId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_PaintingServicesAttachments");

                entity.HasIndex(e => e.PaintingServiceId, "PaintingServiceID");

                entity.Property(e => e.PaintingServicesAttachmentsId).HasColumnName("PaintingServicesAttachmentsID");

                entity.Property(e => e.FilePath)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.PaintingServiceId).HasColumnName("PaintingServiceID");

                entity.HasOne(d => d.PaintingService)
                    .WithMany(p => p.TblPaintingServicesAttachments)
                    .HasForeignKey(d => d.PaintingServiceId)
                    .HasConstraintName("tbl_PaintingServicesAttachments_ibfk_1");
            });

            modelBuilder.Entity<TblPestControlService>(entity =>
            {
                entity.HasKey(e => e.PestControlServiceId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_PestControlService");

                entity.HasIndex(e => e.JobId, "JobID");

                entity.HasIndex(e => e.LocationTypeId, "LocationTypeID");

                entity.HasIndex(e => e.RoomTypeId, "RoomTypeID");

                entity.HasIndex(e => e.SubServiceId, "SubServiceID");

                entity.Property(e => e.PestControlServiceId).HasColumnName("PestControlServiceID");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.LocationSizeId)
                    .HasColumnType("decimal(10,2)")
                    .HasColumnName("LocationSizeID");

                entity.Property(e => e.LocationTypeId).HasColumnName("LocationTypeID");

                entity.Property(e => e.RoomTypeId).HasColumnName("RoomTypeID");

                entity.Property(e => e.SubServiceId).HasColumnName("SubServiceID");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.TblPestControlServices)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("tbl_PestControlService_ibfk_1");

                entity.HasOne(d => d.LocationType)
                    .WithMany(p => p.TblPestControlServices)
                    .HasForeignKey(d => d.LocationTypeId)
                    .HasConstraintName("tbl_PestControlService_ibfk_3");

                entity.HasOne(d => d.RoomType)
                    .WithMany(p => p.TblPestControlServices)
                    .HasForeignKey(d => d.RoomTypeId)
                    .HasConstraintName("tbl_PestControlService_ibfk_4");

                entity.HasOne(d => d.SubService)
                    .WithMany(p => p.TblPestControlServices)
                    .HasForeignKey(d => d.SubServiceId)
                    .HasConstraintName("tbl_PestControlService_ibfk_2");
            });

            modelBuilder.Entity<TblPlumbingService>(entity =>
            {
                entity.HasKey(e => e.PlumbingServiceId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_PlumbingServices");

                entity.HasIndex(e => e.JobId, "JobID");

                entity.HasIndex(e => e.SubCategoryId, "SubCategoryID");

                entity.Property(e => e.PlumbingServiceId).HasColumnName("PlumbingServiceID");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.SubCategoryId).HasColumnName("SubCategoryID");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.TblPlumbingServices)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("tbl_PlumbingServices_ibfk_1");

                entity.HasOne(d => d.SubCategory)
                    .WithMany(p => p.TblPlumbingServices)
                    .HasForeignKey(d => d.SubCategoryId)
                    .HasConstraintName("tbl_PlumbingServices_ibfk_2");
            });

            modelBuilder.Entity<TblPlumbingServicesAttachment>(entity =>
            {
                entity.HasKey(e => e.PlumbingServicesAttachmentsId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_PlumbingServicesAttachments");

                entity.HasIndex(e => e.PlumbingServiceId, "PlumbingServiceID");

                entity.Property(e => e.PlumbingServicesAttachmentsId).HasColumnName("PlumbingServicesAttachmentsID");

                entity.Property(e => e.FilePath)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.PlumbingServiceId).HasColumnName("PlumbingServiceID");

                entity.HasOne(d => d.PlumbingService)
                    .WithMany(p => p.TblPlumbingServicesAttachments)
                    .HasForeignKey(d => d.PlumbingServiceId)
                    .HasConstraintName("tbl_PlumbingServicesAttachments_ibfk_1");
            });

            modelBuilder.Entity<TypeFurniture>(entity =>
            {
                entity.ToTable("TypeFurniture");

                entity.Property(e => e.TypeFurnitureId).HasColumnName("TypeFurnitureID");

                entity.Property(e => e.TypeFurnitureName).HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.IsActive)
                    .HasColumnType("bit(1)")
                    .HasColumnName("isActive");

                entity.Property(e => e.IsHandyman)
                    .HasColumnType("bit(1)")
                    .HasColumnName("isHandyman");

                entity.Property(e => e.LastName).HasMaxLength(300);

                entity.Property(e => e.MobileNumber).HasMaxLength(100);

                entity.Property(e => e.Paswword)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Picture).HasMaxLength(300);
            });

            modelBuilder.Entity<UserAddress>(entity =>
            {
                entity.HasKey(e => e.AddressId)
                    .HasName("PRIMARY");

                entity.ToTable("UserAddress");

                entity.HasIndex(e => e.UserId, "UserID");

                entity.Property(e => e.AddressId).HasColumnName("AddressID");

                entity.Property(e => e.AddressType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Details).HasMaxLength(255);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserAddresses)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("UserAddress_ibfk_1");
            });

            modelBuilder.Entity<UserPackage>(entity =>
            {
                entity.HasKey(e => e.PackageId)
                    .HasName("PRIMARY");

                entity.ToTable("UserPackage");

                entity.HasIndex(e => e.HandymanUserId, "HandymanUserID");

                entity.HasIndex(e => e.MainCategoryId, "MainCategoryID");

                entity.HasIndex(e => e.SubCategoryId, "SubCategoryID");

                entity.Property(e => e.PackageId).HasColumnName("PackageID");

                entity.Property(e => e.AutoPurchase).HasColumnType("tinyint");

                entity.Property(e => e.HandymanUserId).HasColumnName("HandymanUserID");

                entity.Property(e => e.MainCategoryId).HasColumnName("MainCategoryID");

                entity.Property(e => e.Price).HasColumnType("decimal(10,2)");

                entity.Property(e => e.SubCategoryId).HasColumnName("SubCategoryID");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.HandymanUser)
                    .WithMany(p => p.UserPackages)
                    .HasForeignKey(d => d.HandymanUserId)
                    .HasConstraintName("UserPackage_ibfk_1");

                entity.HasOne(d => d.MainCategory)
                    .WithMany(p => p.UserPackages)
                    .HasForeignKey(d => d.MainCategoryId)
                    .HasConstraintName("UserPackage_ibfk_2");

                entity.HasOne(d => d.SubCategory)
                    .WithMany(p => p.UserPackages)
                    .HasForeignKey(d => d.SubCategoryId)
                    .HasConstraintName("UserPackage_ibfk_3");
            });


            modelBuilder.Entity<UserProfileRating>(entity =>
            {
                entity.HasKey(e => e.RatingId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.RatedByUserId, "fk_rated_by_user");

                entity.HasIndex(e => e.UserId, "fk_user");

                entity.Property(e => e.RatingId).HasColumnName("rating_id");

                entity.Property(e => e.RatedByUserId).HasColumnName("rated_by_user_id");

                entity.Property(e => e.Rating).HasColumnName("rating");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.RatedByUser)
                    .WithMany(p => p.UserProfileRatingRatedByUsers)
                    .HasForeignKey(d => d.RatedByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_rated_by_user");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserProfileRatingUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_user");
            });





            modelBuilder.Entity<UserPackagePurchaseRequest>(entity =>
            {
                entity.HasKey(e => e.PackageRequestId)
                    .HasName("PRIMARY");

                entity.ToTable("UserPackagePurchaseRequest");

                entity.HasIndex(e => e.PackageId, "PackageID");

                entity.HasIndex(e => e.UserId, "UserID");

                entity.Property(e => e.PackageRequestId).HasColumnName("PackageRequestID");

                entity.Property(e => e.PackageId).HasColumnName("PackageID");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Package)
                    .WithMany(p => p.UserPackagePurchaseRequests)
                    .HasForeignKey(d => d.PackageId)
                    .HasConstraintName("UserPackagePurchaseRequest_ibfk_1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserPackagePurchaseRequests)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("UserPackagePurchaseRequest_ibfk_2");
            });

            modelBuilder.Entity<UserProfileService>(entity =>
            {
                entity.ToTable("UserProfileService");

                entity.HasIndex(e => e.MainServiceId, "MainServiceID");

                entity.HasIndex(e => e.SubCategoryId, "SubCategoryID");

                entity.HasIndex(e => e.UserId, "UserID");

                entity.Property(e => e.UserProfileServiceId).HasColumnName("UserProfileServiceID");

                entity.Property(e => e.MainServiceId).HasColumnName("MainServiceID");

                entity.Property(e => e.SubCategoryId).HasColumnName("SubCategoryID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.MainService)
                    .WithMany(p => p.UserProfileServices)
                    .HasForeignKey(d => d.MainServiceId)
                    .HasConstraintName("UserProfileService_ibfk_2");

                entity.HasOne(d => d.SubCategory)
                    .WithMany(p => p.UserProfileServices)
                    .HasForeignKey(d => d.SubCategoryId)
                    .HasConstraintName("UserProfileService_ibfk_3");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserProfileServices)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("UserProfileService_ibfk_1");
            });

            modelBuilder.Entity<UserType>(entity =>
            {
                entity.ToTable("UserType");

                entity.Property(e => e.UserTypeId).HasColumnName("UserTypeID");

                entity.Property(e => e.UserTypeInfo)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsFixedLength(true);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
