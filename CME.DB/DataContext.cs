using CME.Entities;
using CME.Entities.Constants;
using Microsoft.EntityFrameworkCore;
using System;

namespace SERP.Filenet.DB
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<TrainingForm> TrainingForms { get; set; }
        public DbSet<TrainingSubject> TrainingSubjects { get; set; }
        public DbSet<TrainingProgram> TrainingPrograms { get; set; }
        public DbSet<TrainingProgram_User> TrainingProgram_Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Organization>().HasData(
                new Organization
                {
                    Id = Guid.Parse(Default.OrganizationId),
                    Name = Default.OrganizationName,
                    Code = Default.OrganizationCode,
                    Address = Default.OrganizationAddress,
                    CreatedOnDate = DateTime.Now,
                    LastModifiedOnDate = DateTime.Now
                }
            );

            #region HTHN
            modelBuilder.Entity<TrainingForm>().HasData(
                new TrainingForm
                {
                    Id = Guid.Parse(Default.TrainingFormId_HTHN),
                    Name = Default.TrainingFormName_HTHN,
                    Code = Default.TrainingFormCode_HTHN,
                    CreatedOnDate = DateTime.Now,
                    LastModifiedOnDate = DateTime.Now
                }
            );

            modelBuilder.Entity<TrainingSubject>().HasData(
                new TrainingSubject
                {
                    Id = Guid.Parse(Default.TrainingSubjectId_Participant_HTHN),
                    Name = Default.TrainingSubjectName_Participant_HTHN,
                    Amount = Default.TrainingSubjectAmount_Participant_HTHN,
                    TrainingFormId = Guid.Parse(Default.TrainingFormId_HTHN),
                    CreatedOnDate = DateTime.Now,
                    LastModifiedOnDate = DateTime.Now
                }
            );

            modelBuilder.Entity<TrainingSubject>().HasData(
                new TrainingSubject
                {
                    Id = Guid.Parse(Default.TrainingSubjectId_Owner_HTHN),
                    Name = Default.TrainingSubjectName_Owner_HTHN,
                    Amount = Default.TrainingSubjectAmount_Owner_HTHN,
                    TrainingFormId = Guid.Parse(Default.TrainingFormId_HTHN),
                    CreatedOnDate = DateTime.Now,
                    LastModifiedOnDate = DateTime.Now
                }
            );
            #endregion

            #region NCKH
            modelBuilder.Entity<TrainingForm>().HasData(
                new TrainingForm
                {
                    Id = Guid.Parse(Default.TrainingFormId_NCKH),
                    Name = Default.TrainingFormName_NCKH,
                    Code = Default.TrainingFormCode_NCKH,
                    CreatedOnDate = DateTime.Now,
                    LastModifiedOnDate = DateTime.Now
                }
            );
            #endregion

            #region DTDH
            modelBuilder.Entity<TrainingForm>().HasData(
                new TrainingForm
                {
                    Id = Guid.Parse(Default.TrainingFormId_DTDH),
                    Name = Default.TrainingFormName_DTDH,
                    Code = Default.TrainingFormCode_DTDH,
                    CreatedOnDate = DateTime.Now,
                    LastModifiedOnDate = DateTime.Now
                }
            );

            modelBuilder.Entity<TrainingSubject>().HasData(
                new TrainingSubject
                {
                    Id = Guid.Parse(Default.TrainingSubjectId_Participant_DTDH),
                    Name = Default.TrainingSubjectName_Participant_DTDH,
                    Amount = Default.TrainingSubjectAmount_Participant_DTDH,
                    TrainingFormId = Guid.Parse(Default.TrainingFormId_DTDH),
                    CreatedOnDate = DateTime.Now,
                    LastModifiedOnDate = DateTime.Now
                }
            );

            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = Guid.Parse(Default.RoleId_Admin),
                    Name = Default.RoleName_Admin,
                    Code = Default.RoleCode_Admin,
                    CreatedOnDate = DateTime.Now,
                    LastModifiedOnDate = DateTime.Now
                }
            );

            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = Guid.Parse(Default.RoleId_User),
                    Name = Default.RoleName_User,
                    Code = Default.RoleCode_User,
                    CreatedOnDate = DateTime.Now,
                    LastModifiedOnDate = DateTime.Now
                }
            );
            #endregion

        }
    }
}
