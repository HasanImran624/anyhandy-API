
using System;
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

        public virtual DbSet<Areatype> Areatypes { get; set; }
        public virtual DbSet<Contractmilestone> Contractmilestones { get; set; }
        public virtual DbSet<Contractmilestonespayment> Contractmilestonespayments { get; set; }
        public virtual DbSet<Handymanserviceslocation> Handymanserviceslocations { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<Jobattachment> Jobattachments { get; set; }
        public virtual DbSet<Jobcontract> Jobcontracts { get; set; }
        public virtual DbSet<Jobproposal> Jobproposals { get; set; }
        public virtual DbSet<Jobproposalattachment> Jobproposalattachments { get; set; }
        public virtual DbSet<Jobservice> Jobservices { get; set; }
        public virtual DbSet<Locationtype> Locationtypes { get; set; }
        public virtual DbSet<MainService> MainServices { get; set; }
        public virtual DbSet<Mainservice1> Mainservices1 { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Messageattachment> Messageattachments { get; set; }
        public virtual DbSet<Roomtype> Roomtypes { get; set; }
        public virtual DbSet<SubService> SubServices { get; set; }
        public virtual DbSet<Subservice1> Subservices1 { get; set; }
        public virtual DbSet<TblAppliancerepairservice> TblAppliancerepairservices { get; set; }
        public virtual DbSet<TblCarpentryservice> TblCarpentryservices { get; set; }
        public virtual DbSet<TblCarpentryservicesattachment> TblCarpentryservicesattachments { get; set; }
        public virtual DbSet<TblElectricalservice> TblElectricalservices { get; set; }
        public virtual DbSet<TblElectricalservicesattachment> TblElectricalservicesattachments { get; set; }
        public virtual DbSet<TblGeneralservice> TblGeneralservices { get; set; }
        public virtual DbSet<TblGeneralserviceattachment> TblGeneralserviceattachments { get; set; }
        public virtual DbSet<TblHomecleaning> TblHomecleanings { get; set; }
        public virtual DbSet<TblHomecleaningattachment> TblHomecleaningattachments { get; set; }
        public virtual DbSet<TblHvacservice> TblHvacservices { get; set; }
        public virtual DbSet<TblHvacservicesattachment> TblHvacservicesattachments { get; set; }
        public virtual DbSet<TblLandscapingservice> TblLandscapingservices { get; set; }
        public virtual DbSet<TblLandscapingserviceattachment> TblLandscapingserviceattachments { get; set; }
        public virtual DbSet<TblPaintingservice> TblPaintingservices { get; set; }
        public virtual DbSet<TblPaintingservicesattachment> TblPaintingservicesattachments { get; set; }
        public virtual DbSet<TblPestcontrolservice> TblPestcontrolservices { get; set; }
        public virtual DbSet<TblPlumbingservice> TblPlumbingservices { get; set; }
        public virtual DbSet<TblPlumbingservicesattachment> TblPlumbingservicesattachments { get; set; }
        public virtual DbSet<Typefurniture> Typefurnitures { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Useraddress> Useraddresses { get; set; }
        public virtual DbSet<Userpackage> Userpackages { get; set; }
        public virtual DbSet<Userpackagepurchaserequest> Userpackagepurchaserequests { get; set; }
        public virtual DbSet<Userprofileservice> Userprofileservices { get; set; }
        public virtual DbSet<Usertype> Usertypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySQL("server=127.0.0.1;user id=root;password=1234567;database=anyhandy;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Areatype>(entity =>
            {
                entity.ToTable("areatype");

                entity.Property(e => e.AreaTypeId).HasColumnName("AreaTypeID");

                entity.Property(e => e.AreaTypeName).HasMaxLength(50);
            });

            modelBuilder.Entity<Contractmilestone>(entity =>
            {
                entity.HasKey(e => e.MilestoneId)
                    .HasName("PRIMARY");

                entity.ToTable("contractmilestones");

                entity.HasIndex(e => e.JobContractId, "JobContractID");

                entity.Property(e => e.MilestoneId).HasColumnName("MilestoneID");

                entity.Property(e => e.Amount).HasColumnType("decimal(10,2)");

                entity.Property(e => e.JobContractId).HasColumnName("JobContractID");

                entity.HasOne(d => d.JobContract)
                    .WithMany(p => p.Contractmilestones)
                    .HasForeignKey(d => d.JobContractId)
                    .HasConstraintName("ContractMilestones_ibfk_1");
            });

            modelBuilder.Entity<Contractmilestonespayment>(entity =>
            {
                entity.ToTable("contractmilestonespayment");

                entity.HasIndex(e => e.MilestoneId, "MilestoneID");

                entity.Property(e => e.ContractMilestonesPaymentId).HasColumnName("ContractMilestonesPaymentID");

                entity.Property(e => e.Amount).HasColumnType("decimal(10,2)");

                entity.Property(e => e.MilestoneId).HasColumnName("MilestoneID");

                entity.Property(e => e.PaymentReference)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Milestone)
                    .WithMany(p => p.Contractmilestonespayments)
                    .HasForeignKey(d => d.MilestoneId)
                    .HasConstraintName("ContractMilestonesPayment_ibfk_1");
            });

            modelBuilder.Entity<Handymanserviceslocation>(entity =>
            {
                entity.HasKey(e => e.ServiceLocationId)
                    .HasName("PRIMARY");

                entity.ToTable("handymanserviceslocation");

                entity.HasIndex(e => e.UserId, "UserID");

                entity.Property(e => e.ServiceLocationId).HasColumnName("ServiceLocationID");

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Handymanserviceslocations)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("HandymanServicesLocation_ibfk_1");
            });

            modelBuilder.Entity<Job>(entity =>
            {
                entity.ToTable("job");

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

            modelBuilder.Entity<Jobattachment>(entity =>
            {
                entity.HasKey(e => e.AttachmentId)
                    .HasName("PRIMARY");

                entity.ToTable("jobattachments");

                entity.HasIndex(e => e.JobId, "JobID");

                entity.Property(e => e.AttachmentId).HasColumnName("AttachmentID");

                entity.Property(e => e.FilePath)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.FileShortDescription).HasMaxLength(100);

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.Jobattachments)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("JobAttachments_ibfk_1");
            });

            modelBuilder.Entity<Jobcontract>(entity =>
            {
                entity.ToTable("jobcontract");

                entity.HasIndex(e => e.JobId, "JobID");

                entity.HasIndex(e => e.JobProposalId, "JobProposalID");

                entity.HasIndex(e => e.SelectedHandyManId, "SelectedHandyManID");

                entity.Property(e => e.JobContractId).HasColumnName("JobContractID");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.JobProposalId).HasColumnName("JobProposalID");

                entity.Property(e => e.SelectedHandyManId).HasColumnName("SelectedHandyManID");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.Jobcontracts)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("JobContract_ibfk_1");

                entity.HasOne(d => d.JobProposal)
                    .WithMany(p => p.Jobcontracts)
                    .HasForeignKey(d => d.JobProposalId)
                    .HasConstraintName("JobContract_ibfk_3");

                entity.HasOne(d => d.SelectedHandyMan)
                    .WithMany(p => p.Jobcontracts)
                    .HasForeignKey(d => d.SelectedHandyManId)
                    .HasConstraintName("JobContract_ibfk_2");
            });

            modelBuilder.Entity<Jobproposal>(entity =>
            {
                entity.ToTable("jobproposals");

                entity.HasIndex(e => e.JobId, "JobID");

                entity.HasIndex(e => e.UserId, "UserID");

                entity.Property(e => e.JobProposalId).HasColumnName("JobProposalID");

                entity.Property(e => e.Amount).HasColumnType("decimal(10,2)");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.Status).HasColumnType("enum('Accepted','Rejected','Pending')");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.Jobproposals)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("JobProposals_ibfk_1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Jobproposals)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("JobProposals_ibfk_2");
            });

            modelBuilder.Entity<Jobproposalattachment>(entity =>
            {
                entity.HasKey(e => e.AttachmentId)
                    .HasName("PRIMARY");

                entity.ToTable("jobproposalattachments");

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
                    .WithMany(p => p.Jobproposalattachments)
                    .HasForeignKey(d => d.JobProposalId)
                    .HasConstraintName("JobProposalAttachments_ibfk_1");
            });

            modelBuilder.Entity<Jobservice>(entity =>
            {
                entity.ToTable("jobservices");

                entity.HasIndex(e => e.JobId, "JobID");

                entity.HasIndex(e => e.JobSubServiceId, "JobSubServiceID");

                entity.Property(e => e.JobServiceId).HasColumnName("JobServiceID");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.JobSubServiceId).HasColumnName("JobSubServiceID");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.Jobservices)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("JobServices_ibfk_1");

                entity.HasOne(d => d.JobSubService)
                    .WithMany(p => p.Jobservices)
                    .HasForeignKey(d => d.JobSubServiceId)
                    .HasConstraintName("JobServices_ibfk_2");
            });

            modelBuilder.Entity<Locationtype>(entity =>
            {
                entity.ToTable("locationtype");

                entity.Property(e => e.LocationTypeId).HasColumnName("LocationTypeID");

                entity.Property(e => e.LocationTypeName).HasMaxLength(50);
            });

            modelBuilder.Entity<MainService>(entity =>
            {
                entity.ToTable("main_services");

                entity.Property(e => e.MainServiceId).HasColumnName("main_service_id");

                entity.Property(e => e.MainServiceName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("main_service_name");
            });

            modelBuilder.Entity<Mainservice1>(entity =>
            {
                entity.HasKey(e => e.MainServicesId)
                    .HasName("PRIMARY");

                entity.ToTable("mainservices");

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

            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("messages");

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

            modelBuilder.Entity<Messageattachment>(entity =>
            {
                entity.HasKey(e => e.AttachmentId)
                    .HasName("PRIMARY");

                entity.ToTable("messageattachments");

                entity.HasIndex(e => e.MessageId, "MessageID");

                entity.Property(e => e.AttachmentId).HasColumnName("AttachmentID");

                entity.Property(e => e.FilePath)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.MessageId).HasColumnName("MessageID");

                entity.HasOne(d => d.Message)
                    .WithMany(p => p.Messageattachments)
                    .HasForeignKey(d => d.MessageId)
                    .HasConstraintName("MessageAttachments_ibfk_1");
            });

            modelBuilder.Entity<Roomtype>(entity =>
            {
                entity.ToTable("roomtype");

                entity.Property(e => e.RoomTypeId).HasColumnName("RoomTypeID");

                entity.Property(e => e.RoomTypeName).HasMaxLength(50);
            });

            modelBuilder.Entity<SubService>(entity =>
            {
                entity.ToTable("sub_services");

                entity.HasIndex(e => e.MainServiceId, "main_service_id");

                entity.Property(e => e.SubServiceId).HasColumnName("sub_service_id");

                entity.Property(e => e.MainServiceId).HasColumnName("main_service_id");

                entity.Property(e => e.SubServiceName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("sub_service_name");

                entity.HasOne(d => d.MainService)
                    .WithMany(p => p.SubServices)
                    .HasForeignKey(d => d.MainServiceId)
                    .HasConstraintName("sub_services_ibfk_1");
            });

            modelBuilder.Entity<Subservice1>(entity =>
            {
                entity.HasKey(e => e.SubServicesId)
                    .HasName("PRIMARY");

                entity.ToTable("subservices");

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
                    .WithMany(p => p.Subservice1s)
                    .HasForeignKey(d => d.MainServicesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SubServices_ibfk_1");
            });

            modelBuilder.Entity<TblAppliancerepairservice>(entity =>
            {
                entity.HasKey(e => e.ApplianceRepairServiceId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_appliancerepairservice");

                entity.HasIndex(e => e.JobId, "JobID");

                entity.HasIndex(e => e.SubServiceId, "SubServiceID");

                entity.Property(e => e.ApplianceRepairServiceId).HasColumnName("ApplianceRepairServiceID");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.SubServiceId).HasColumnName("SubServiceID");

                entity.Property(e => e.TypeAppliance).HasMaxLength(255);

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.TblAppliancerepairservices)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("tbl_ApplianceRepairService_ibfk_1");

                entity.HasOne(d => d.SubService)
                    .WithMany(p => p.TblAppliancerepairservices)
                    .HasForeignKey(d => d.SubServiceId)
                    .HasConstraintName("tbl_ApplianceRepairService_ibfk_2");
            });

            modelBuilder.Entity<TblCarpentryservice>(entity =>
            {
                entity.HasKey(e => e.CarpentryServiceId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_carpentryservices");

                entity.HasIndex(e => e.JobId, "JobID");

                entity.HasIndex(e => e.SubCategoryId, "SubCategoryID");

                entity.Property(e => e.CarpentryServiceId).HasColumnName("CarpentryServiceID");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.SubCategoryId).HasColumnName("SubCategoryID");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.TblCarpentryservices)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("tbl_CarpentryServices_ibfk_1");

                entity.HasOne(d => d.SubCategory)
                    .WithMany(p => p.TblCarpentryservices)
                    .HasForeignKey(d => d.SubCategoryId)
                    .HasConstraintName("tbl_CarpentryServices_ibfk_2");
            });

            modelBuilder.Entity<TblCarpentryservicesattachment>(entity =>
            {
                entity.HasKey(e => e.CarpentryServicesAttachmentsId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_carpentryservicesattachments");

                entity.HasIndex(e => e.CarpentryServiceId, "CarpentryServiceID");

                entity.Property(e => e.CarpentryServicesAttachmentsId).HasColumnName("CarpentryServicesAttachmentsID");

                entity.Property(e => e.CarpentryServiceId).HasColumnName("CarpentryServiceID");

                entity.Property(e => e.FilePath)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.CarpentryService)
                    .WithMany(p => p.TblCarpentryservicesattachments)
                    .HasForeignKey(d => d.CarpentryServiceId)
                    .HasConstraintName("tbl_CarpentryServicesAttachments_ibfk_1");
            });

            modelBuilder.Entity<TblElectricalservice>(entity =>
            {
                entity.HasKey(e => e.ElectricalServiceId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_electricalservices");

                entity.HasIndex(e => e.JobId, "JobID");

                entity.HasIndex(e => e.SubCategoryId, "SubCategoryID");

                entity.Property(e => e.ElectricalServiceId).HasColumnName("ElectricalServiceID");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.SubCategoryId).HasColumnName("SubCategoryID");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.TblElectricalservices)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("tbl_ElectricalServices_ibfk_1");

                entity.HasOne(d => d.SubCategory)
                    .WithMany(p => p.TblElectricalservices)
                    .HasForeignKey(d => d.SubCategoryId)
                    .HasConstraintName("tbl_ElectricalServices_ibfk_2");
            });

            modelBuilder.Entity<TblElectricalservicesattachment>(entity =>
            {
                entity.HasKey(e => e.ElectricalServicesAttachmentsId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_electricalservicesattachments");

                entity.HasIndex(e => e.ElectricalServiceId, "ElectricalServiceID");

                entity.Property(e => e.ElectricalServicesAttachmentsId).HasColumnName("ElectricalServicesAttachmentsID");

                entity.Property(e => e.ElectricalServiceId).HasColumnName("ElectricalServiceID");

                entity.Property(e => e.FilePath)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.ElectricalService)
                    .WithMany(p => p.TblElectricalservicesattachments)
                    .HasForeignKey(d => d.ElectricalServiceId)
                    .HasConstraintName("tbl_ElectricalServicesAttachments_ibfk_1");
            });

            modelBuilder.Entity<TblGeneralservice>(entity =>
            {
                entity.HasKey(e => e.GeneralServiceId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_generalservice");

                entity.HasIndex(e => e.JobId, "JobID");

                entity.Property(e => e.GeneralServiceId).HasColumnName("GeneralServiceID");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.TblGeneralservices)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("tbl_GeneralService_ibfk_1");
            });

            modelBuilder.Entity<TblGeneralserviceattachment>(entity =>
            {
                entity.HasKey(e => e.AttachmentId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_generalserviceattachment");

                entity.HasIndex(e => e.GeneralServiceServiceId, "GeneralServiceServiceID");

                entity.Property(e => e.AttachmentId).HasColumnName("AttachmentID");

                entity.Property(e => e.FilePath).HasMaxLength(255);

                entity.Property(e => e.GeneralServiceServiceId).HasColumnName("GeneralServiceServiceID");

                entity.HasOne(d => d.GeneralServiceService)
                    .WithMany(p => p.TblGeneralserviceattachments)
                    .HasForeignKey(d => d.GeneralServiceServiceId)
                    .HasConstraintName("tbl_GeneralServiceAttachment_ibfk_1");
            });

            modelBuilder.Entity<TblHomecleaning>(entity =>
            {
                entity.HasKey(e => e.HomeCleaningServiceId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_homecleaning");

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
                    .WithMany(p => p.TblHomecleanings)
                    .HasForeignKey(d => d.AreaTypeId)
                    .HasConstraintName("tbl_HomeCleaning_ibfk_4");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.TblHomecleanings)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("tbl_HomeCleaning_ibfk_1");

                entity.HasOne(d => d.LocationType)
                    .WithMany(p => p.TblHomecleanings)
                    .HasForeignKey(d => d.LocationTypeId)
                    .HasConstraintName("tbl_HomeCleaning_ibfk_3");

                entity.HasOne(d => d.SubService)
                    .WithMany(p => p.TblHomecleanings)
                    .HasForeignKey(d => d.SubServiceId)
                    .HasConstraintName("tbl_HomeCleaning_ibfk_2");

                entity.HasOne(d => d.TypeFurniture)
                    .WithMany(p => p.TblHomecleanings)
                    .HasForeignKey(d => d.TypeFurnitureId)
                    .HasConstraintName("tbl_HomeCleaning_ibfk_5");
            });

            modelBuilder.Entity<TblHomecleaningattachment>(entity =>
            {
                entity.HasKey(e => e.HomeCleaningAttachmentId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_homecleaningattachments");

                entity.HasIndex(e => e.HomeCleaningServiceId, "HomeCleaningServiceID");

                entity.Property(e => e.HomeCleaningAttachmentId).HasColumnName("HomeCleaningAttachmentID");

                entity.Property(e => e.FilePath).HasMaxLength(255);

                entity.Property(e => e.HomeCleaningServiceId).HasColumnName("HomeCleaningServiceID");

                entity.HasOne(d => d.HomeCleaningService)
                    .WithMany(p => p.TblHomecleaningattachments)
                    .HasForeignKey(d => d.HomeCleaningServiceId)
                    .HasConstraintName("tbl_HomeCleaningAttachments_ibfk_1");
            });

            modelBuilder.Entity<TblHvacservice>(entity =>
            {
                entity.HasKey(e => e.HvacServiceId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_hvacservices");

                entity.HasIndex(e => e.JobId, "JobID");

                entity.HasIndex(e => e.SubServiceId, "SubServiceID");

                entity.Property(e => e.HvacServiceId).HasColumnName("HvacServiceID");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.SubServiceId).HasColumnName("SubServiceID");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.TblHvacservices)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("tbl_HvacServices_ibfk_1");

                entity.HasOne(d => d.SubService)
                    .WithMany(p => p.TblHvacservices)
                    .HasForeignKey(d => d.SubServiceId)
                    .HasConstraintName("tbl_HvacServices_ibfk_2");
            });

            modelBuilder.Entity<TblHvacservicesattachment>(entity =>
            {
                entity.HasKey(e => e.HvacServicesAttachmentsId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_hvacservicesattachments");

                entity.HasIndex(e => e.HvacServiceId, "HvacServiceID");

                entity.Property(e => e.HvacServicesAttachmentsId).HasColumnName("HvacServicesAttachmentsID");

                entity.Property(e => e.FilePath)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.HvacServiceId).HasColumnName("HvacServiceID");

                entity.HasOne(d => d.HvacService)
                    .WithMany(p => p.TblHvacservicesattachments)
                    .HasForeignKey(d => d.HvacServiceId)
                    .HasConstraintName("tbl_HVACServicesAttachments_ibfk_1");
            });

            modelBuilder.Entity<TblLandscapingservice>(entity =>
            {
                entity.HasKey(e => e.LandscapingServiceId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_landscapingservice");

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
                    .WithMany(p => p.TblLandscapingservices)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("tbl_LandscapingService_ibfk_1");

                entity.HasOne(d => d.SubServices)
                    .WithMany(p => p.TblLandscapingservices)
                    .HasForeignKey(d => d.SubServicesId)
                    .HasConstraintName("tbl_LandscapingService_ibfk_2");
            });

            modelBuilder.Entity<TblLandscapingserviceattachment>(entity =>
            {
                entity.HasKey(e => e.LandscapingServiceAttachmentId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_landscapingserviceattachment");

                entity.HasIndex(e => e.LandscapingServiceId, "LandscapingServiceID");

                entity.Property(e => e.LandscapingServiceAttachmentId).HasColumnName("LandscapingServiceAttachmentID");

                entity.Property(e => e.AttachmentPath)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.LandscapingServiceId).HasColumnName("LandscapingServiceID");

                entity.HasOne(d => d.LandscapingService)
                    .WithMany(p => p.TblLandscapingserviceattachments)
                    .HasForeignKey(d => d.LandscapingServiceId)
                    .HasConstraintName("tbl_LandscapingServiceAttachment_ibfk_1");
            });

            modelBuilder.Entity<TblPaintingservice>(entity =>
            {
                entity.HasKey(e => e.PaintingServiceId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_paintingservices");

                entity.HasIndex(e => e.JobId, "JobID");

                entity.HasIndex(e => e.SubCategoryId, "SubCategoryID");

                entity.Property(e => e.PaintingServiceId).HasColumnName("PaintingServiceID");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.PaintColor).HasMaxLength(255);

                entity.Property(e => e.SizeArea).HasColumnType("decimal(10,2)");

                entity.Property(e => e.SubCategoryId).HasColumnName("SubCategoryID");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.TblPaintingservices)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("tbl_PaintingServices_ibfk_1");

                entity.HasOne(d => d.SubCategory)
                    .WithMany(p => p.TblPaintingservices)
                    .HasForeignKey(d => d.SubCategoryId)
                    .HasConstraintName("tbl_PaintingServices_ibfk_2");
            });

            modelBuilder.Entity<TblPaintingservicesattachment>(entity =>
            {
                entity.HasKey(e => e.PaintingServicesAttachmentsId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_paintingservicesattachments");

                entity.HasIndex(e => e.PaintingServiceId, "PaintingServiceID");

                entity.Property(e => e.PaintingServicesAttachmentsId).HasColumnName("PaintingServicesAttachmentsID");

                entity.Property(e => e.FilePath)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.PaintingServiceId).HasColumnName("PaintingServiceID");

                entity.HasOne(d => d.PaintingService)
                    .WithMany(p => p.TblPaintingservicesattachments)
                    .HasForeignKey(d => d.PaintingServiceId)
                    .HasConstraintName("tbl_PaintingServicesAttachments_ibfk_1");
            });

            modelBuilder.Entity<TblPestcontrolservice>(entity =>
            {
                entity.HasKey(e => e.PestControlServiceId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_pestcontrolservice");

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
                    .WithMany(p => p.TblPestcontrolservices)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("tbl_PestControlService_ibfk_1");

                entity.HasOne(d => d.LocationType)
                    .WithMany(p => p.TblPestcontrolservices)
                    .HasForeignKey(d => d.LocationTypeId)
                    .HasConstraintName("tbl_PestControlService_ibfk_3");

                entity.HasOne(d => d.RoomType)
                    .WithMany(p => p.TblPestcontrolservices)
                    .HasForeignKey(d => d.RoomTypeId)
                    .HasConstraintName("tbl_PestControlService_ibfk_4");

                entity.HasOne(d => d.SubService)
                    .WithMany(p => p.TblPestcontrolservices)
                    .HasForeignKey(d => d.SubServiceId)
                    .HasConstraintName("tbl_PestControlService_ibfk_2");
            });

            modelBuilder.Entity<TblPlumbingservice>(entity =>
            {
                entity.HasKey(e => e.PlumbingServiceId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_plumbingservices");

                entity.HasIndex(e => e.JobId, "JobID");

                entity.HasIndex(e => e.SubCategoryId, "SubCategoryID");

                entity.Property(e => e.PlumbingServiceId).HasColumnName("PlumbingServiceID");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.SubCategoryId).HasColumnName("SubCategoryID");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.TblPlumbingservices)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("tbl_PlumbingServices_ibfk_1");

                entity.HasOne(d => d.SubCategory)
                    .WithMany(p => p.TblPlumbingservices)
                    .HasForeignKey(d => d.SubCategoryId)
                    .HasConstraintName("tbl_PlumbingServices_ibfk_2");
            });

            modelBuilder.Entity<TblPlumbingservicesattachment>(entity =>
            {
                entity.HasKey(e => e.PlumbingServicesAttachmentsId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_plumbingservicesattachments");

                entity.HasIndex(e => e.PlumbingServiceId, "PlumbingServiceID");

                entity.Property(e => e.PlumbingServicesAttachmentsId).HasColumnName("PlumbingServicesAttachmentsID");

                entity.Property(e => e.FilePath)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.PlumbingServiceId).HasColumnName("PlumbingServiceID");

                entity.HasOne(d => d.PlumbingService)
                    .WithMany(p => p.TblPlumbingservicesattachments)
                    .HasForeignKey(d => d.PlumbingServiceId)
                    .HasConstraintName("tbl_PlumbingServicesAttachments_ibfk_1");
            });

            modelBuilder.Entity<Typefurniture>(entity =>
            {
                entity.ToTable("typefurniture");

                entity.Property(e => e.TypeFurnitureId).HasColumnName("TypeFurnitureID");

                entity.Property(e => e.TypeFurnitureName).HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

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

            modelBuilder.Entity<Useraddress>(entity =>
            {
                entity.HasKey(e => e.AddressId)
                    .HasName("PRIMARY");

                entity.ToTable("useraddress");

                entity.HasIndex(e => e.UserId, "UserID");

                entity.Property(e => e.AddressId).HasColumnName("AddressID");

                entity.Property(e => e.AddressType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Details).HasMaxLength(255);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Useraddresses)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("UserAddress_ibfk_1");
            });

            modelBuilder.Entity<Userpackage>(entity =>
            {
                entity.HasKey(e => e.PackageId)
                    .HasName("PRIMARY");

                entity.ToTable("userpackage");

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
                    .WithMany(p => p.Userpackages)
                    .HasForeignKey(d => d.HandymanUserId)
                    .HasConstraintName("UserPackage_ibfk_1");

                entity.HasOne(d => d.MainCategory)
                    .WithMany(p => p.Userpackages)
                    .HasForeignKey(d => d.MainCategoryId)
                    .HasConstraintName("UserPackage_ibfk_2");

                entity.HasOne(d => d.SubCategory)
                    .WithMany(p => p.Userpackages)
                    .HasForeignKey(d => d.SubCategoryId)
                    .HasConstraintName("UserPackage_ibfk_3");
            });

            modelBuilder.Entity<Userpackagepurchaserequest>(entity =>
            {
                entity.HasKey(e => e.PackageRequestId)
                    .HasName("PRIMARY");

                entity.ToTable("userpackagepurchaserequest");

                entity.HasIndex(e => e.PackageId, "PackageID");

                entity.HasIndex(e => e.UserId, "UserID");

                entity.Property(e => e.PackageRequestId).HasColumnName("PackageRequestID");

                entity.Property(e => e.PackageId).HasColumnName("PackageID");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Package)
                    .WithMany(p => p.Userpackagepurchaserequests)
                    .HasForeignKey(d => d.PackageId)
                    .HasConstraintName("UserPackagePurchaseRequest_ibfk_1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Userpackagepurchaserequests)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("UserPackagePurchaseRequest_ibfk_2");
            });

            modelBuilder.Entity<Userprofileservice>(entity =>
            {
                entity.ToTable("userprofileservice");

                entity.HasIndex(e => e.MainServiceId, "MainServiceID");

                entity.HasIndex(e => e.SubCategoryId, "SubCategoryID");

                entity.HasIndex(e => e.UserId, "UserID");

                entity.Property(e => e.UserProfileServiceId).HasColumnName("UserProfileServiceID");

                entity.Property(e => e.MainServiceId).HasColumnName("MainServiceID");

                entity.Property(e => e.SubCategoryId).HasColumnName("SubCategoryID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.MainService)
                    .WithMany(p => p.Userprofileservices)
                    .HasForeignKey(d => d.MainServiceId)
                    .HasConstraintName("UserProfileService_ibfk_2");

                entity.HasOne(d => d.SubCategory)
                    .WithMany(p => p.Userprofileservices)
                    .HasForeignKey(d => d.SubCategoryId)
                    .HasConstraintName("UserProfileService_ibfk_3");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Userprofileservices)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("UserProfileService_ibfk_1");
            });

            modelBuilder.Entity<Usertype>(entity =>
            {
                entity.ToTable("usertype");

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

