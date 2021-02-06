using System;
using System.Configuration;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using OREC.DAL.Models;

namespace OREC.DAL
{
    public partial class ORECContext : DbContext
    {
        private IConfiguration Configuration => new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json")
           .Build();
        public ORECContext(DbContextOptions<ORECContext> options): base(options){}
        public ORECContext(){}
        public virtual DbSet<JobSkillMap> JobSkillMap { get; set; }
        public virtual DbSet<Jobs> Jobs { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Skills> Skills { get; set; }
        public virtual DbSet<UserJobMap> UserJobMap { get; set; }
        public virtual DbSet<UserSkillMap> UserSkillMap { get; set; }
        public virtual DbSet<UserDetails> UserDetails { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("ORECDBConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<JobSkillMap>(entity =>
            {
                entity.Property(e => e.JobSkillMapId).HasColumnName("JobSkillMapID");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.SkillId).HasColumnName("SkillID");

                entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedBy).HasColumnName("ModifiedBy");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.IsDeleted).HasColumnName("IsDeleted");

            });

            modelBuilder.Entity<Jobs>(entity =>
            {
                entity.HasKey(e => e.JobId);

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.JobTitle)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.JobDesc)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.JobType)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RequiredExperience).HasColumnName("RequiredExperience");

                entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedBy).HasColumnName("ModifiedBy");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.IsDeleted).HasColumnName("IsDeleted");

            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.Property(e => e.RoleId)
                    .HasColumnName("RoleID")
                    .ValueGeneratedNever();

                entity.Property(e => e.RoleDesc)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedBy).HasColumnName("ModifiedBy");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.IsDeleted).HasColumnName("IsDeleted");
            });

            modelBuilder.Entity<Skills>(entity =>
            {
                entity.HasKey(e => e.SkillId);

                entity.Property(e => e.SkillId).HasColumnName("SkillID");

                entity.Property(e => e.SkillTitle)
                    .HasMaxLength(20)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.SkillDescription)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedBy).HasColumnName("ModifiedBy");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.IsDeleted).HasColumnName("IsDeleted");

            });

            modelBuilder.Entity<UserJobMap>(entity =>
            {
                entity.Property(e => e.UserJobMapId).HasColumnName("UserJobMapID");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedBy).HasColumnName("ModifiedBy");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.IsDeleted).HasColumnName("IsDeleted");
            });

            modelBuilder.Entity<UserSkillMap>(entity =>
            {
                entity.Property(e => e.UserSkillMapId).HasColumnName("UserSkillMapID");

                entity.Property(e => e.SkillId).HasColumnName("SkillID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedBy).HasColumnName("ModifiedBy");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.IsDeleted).HasColumnName("IsDeleted");
            });

            modelBuilder.Entity<UserDetails>(entity =>
            {
                entity.HasKey(e => e.UserDetailsID);

                entity.Property(e => e.UserDetailsID).HasColumnName("UserDetailsID");

                entity.Property(e => e.UserID).HasColumnName("UserID");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                   .IsRequired()
                   .HasMaxLength(50)
                   .IsUnicode(false);

                entity.Property(e => e.MobileNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.HighestQualification)
                   .HasMaxLength(20)
                   .IsUnicode(false);

                entity.Property(e => e.Experience).HasColumnName("Experience");

                entity.Property(e => e.InternalEmployeeId).HasColumnName("InternalEmployeeID");

                entity.Property(e => e.CurrentOrganization)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.CurrentPosition)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.IsDeleted).HasColumnName("IsDeleted");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserID);

                entity.Property(e => e.UserID).HasColumnName("UserID");

                entity.Property(e => e.RoleID).HasColumnName("RoleID");

                entity.Property(e => e.EmailID)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.IsDeleted).HasColumnName("IsDeleted");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
