﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SERP.Filenet.DB;

namespace CME.DB.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CME.Entities.Department", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ApplicationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("CreatedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreatedOnDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("LastModifiedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("LastModifiedOnDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("OrganizationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.ToTable("cat_Departments");
                });

            modelBuilder.Entity("CME.Entities.Organization", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ApplicationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("CreatedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreatedOnDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("LastModifiedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("LastModifiedOnDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("cat_Organizations");

                    b.HasData(
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000001"),
                            Address = "12 Chu Văn An, Điện Bàn, Ba Đình, Hà Nội",
                            Code = "BVXP",
                            CreatedOnDate = new DateTime(2021, 3, 12, 16, 14, 22, 290, DateTimeKind.Local).AddTicks(4606),
                            LastModifiedOnDate = new DateTime(2021, 3, 12, 16, 14, 22, 291, DateTimeKind.Local).AddTicks(2232),
                            Name = "BV Xanh Pôn"
                        });
                });

            modelBuilder.Entity("CME.Entities.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ApplicationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("CreatedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreatedOnDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("LastModifiedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("LastModifiedOnDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("auth_Roles");

                    b.HasData(
                        new
                        {
                            Id = new Guid("00000000-2222-0000-0000-000000000001"),
                            Code = "admin",
                            CreatedOnDate = new DateTime(2021, 3, 12, 16, 14, 22, 292, DateTimeKind.Local).AddTicks(8163),
                            LastModifiedOnDate = new DateTime(2021, 3, 12, 16, 14, 22, 292, DateTimeKind.Local).AddTicks(8168),
                            Name = "Admin"
                        },
                        new
                        {
                            Id = new Guid("00000000-2222-0000-0000-000000000002"),
                            Code = "normal-user",
                            CreatedOnDate = new DateTime(2021, 3, 12, 16, 14, 22, 292, DateTimeKind.Local).AddTicks(8311),
                            LastModifiedOnDate = new DateTime(2021, 3, 12, 16, 14, 22, 292, DateTimeKind.Local).AddTicks(8312),
                            Name = "Người dùng"
                        });
                });

            modelBuilder.Entity("CME.Entities.Title", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ApplicationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("CreatedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreatedOnDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("LastModifiedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("LastModifiedOnDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("cat_Titles");
                });

            modelBuilder.Entity("CME.Entities.TrainingForm", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ApplicationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("CreatedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreatedOnDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("LastModifiedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("LastModifiedOnDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("cat_TrainingForms");

                    b.HasData(
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-200000000001"),
                            Code = "HTHN",
                            CreatedOnDate = new DateTime(2021, 3, 12, 16, 14, 22, 292, DateTimeKind.Local).AddTicks(4202),
                            LastModifiedOnDate = new DateTime(2021, 3, 12, 16, 14, 22, 292, DateTimeKind.Local).AddTicks(4214),
                            Name = "Hội thảo, hội nghị"
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-200000000002"),
                            Code = "NCKH",
                            CreatedOnDate = new DateTime(2021, 3, 12, 16, 14, 22, 292, DateTimeKind.Local).AddTicks(6616),
                            LastModifiedOnDate = new DateTime(2021, 3, 12, 16, 14, 22, 292, DateTimeKind.Local).AddTicks(6618),
                            Name = "Nghiên cứu khoa học"
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-200000000003"),
                            Code = "DTDH",
                            CreatedOnDate = new DateTime(2021, 3, 12, 16, 14, 22, 292, DateTimeKind.Local).AddTicks(6644),
                            LastModifiedOnDate = new DateTime(2021, 3, 12, 16, 14, 22, 292, DateTimeKind.Local).AddTicks(6646),
                            Name = "Đào tạo dài hạn"
                        });
                });

            modelBuilder.Entity("CME.Entities.TrainingProgram", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ApplicationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("CreatedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreatedOnDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FromDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("LastModifiedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("LastModifiedOnDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("MetaData")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("OrganizationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ToDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("TrainingFormId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.HasIndex("TrainingFormId");

                    b.ToTable("TrainingPrograms");
                });

            modelBuilder.Entity("CME.Entities.TrainingProgram_User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<Guid?>("ApplicationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreatedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreatedOnDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("LastModifiedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("LastModifiedOnDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TrainingProgramId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("TrainingSubjectName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TrainingProgramId");

                    b.HasIndex("UserId");

                    b.ToTable("TrainingProgram_User");
                });

            modelBuilder.Entity("CME.Entities.TrainingSubject", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<Guid?>("ApplicationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("CreatedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreatedOnDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("LastModifiedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("LastModifiedOnDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TrainingFormId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TrainingFormId");

                    b.ToTable("cat_TrainingSubjects");

                    b.HasData(
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-300000000001"),
                            Amount = 4.0,
                            CreatedOnDate = new DateTime(2021, 3, 12, 16, 14, 22, 292, DateTimeKind.Local).AddTicks(6315),
                            LastModifiedOnDate = new DateTime(2021, 3, 12, 16, 14, 22, 292, DateTimeKind.Local).AddTicks(6321),
                            Name = "Người tham dự",
                            Order = 0,
                            TrainingFormId = new Guid("00000000-0000-0000-0000-200000000001")
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-300000000002"),
                            Amount = 8.0,
                            CreatedOnDate = new DateTime(2021, 3, 12, 16, 14, 22, 292, DateTimeKind.Local).AddTicks(6483),
                            LastModifiedOnDate = new DateTime(2021, 3, 12, 16, 14, 22, 292, DateTimeKind.Local).AddTicks(6485),
                            Name = "Người thuyết trình",
                            Order = 0,
                            TrainingFormId = new Guid("00000000-0000-0000-0000-200000000001")
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-330000000001"),
                            Amount = 24.0,
                            CreatedOnDate = new DateTime(2021, 3, 12, 16, 14, 22, 292, DateTimeKind.Local).AddTicks(6667),
                            LastModifiedOnDate = new DateTime(2021, 3, 12, 16, 14, 22, 292, DateTimeKind.Local).AddTicks(6668),
                            Name = "NV nội vụ",
                            Order = 0,
                            TrainingFormId = new Guid("00000000-0000-0000-0000-200000000003")
                        });
                });

            modelBuilder.Entity("CME.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ApplicationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AvatarUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CertificateNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("CreatedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreatedOnDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("DepartmentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Fullname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("IdentificationNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("IssueDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("LastModifiedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("LastModifiedOnDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Lastname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("OrganizationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("TitleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("OrganizationId");

                    b.HasIndex("TitleId");

                    b.ToTable("auth_Users");
                });

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.Property<Guid>("RolesId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UsersId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("RolesId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("RoleUser");
                });

            modelBuilder.Entity("CME.Entities.Department", b =>
                {
                    b.HasOne("CME.Entities.Organization", "Organization")
                        .WithMany()
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("CME.Entities.TrainingProgram", b =>
                {
                    b.HasOne("CME.Entities.Organization", "Organization")
                        .WithMany()
                        .HasForeignKey("OrganizationId");

                    b.HasOne("CME.Entities.TrainingForm", "TrainingForm")
                        .WithMany()
                        .HasForeignKey("TrainingFormId");

                    b.Navigation("Organization");

                    b.Navigation("TrainingForm");
                });

            modelBuilder.Entity("CME.Entities.TrainingProgram_User", b =>
                {
                    b.HasOne("CME.Entities.TrainingProgram", "TrainingProgram")
                        .WithMany("TrainingProgram_Users")
                        .HasForeignKey("TrainingProgramId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CME.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TrainingProgram");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CME.Entities.TrainingSubject", b =>
                {
                    b.HasOne("CME.Entities.TrainingForm", "TrainingForm")
                        .WithMany("TrainingSubjects")
                        .HasForeignKey("TrainingFormId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TrainingForm");
                });

            modelBuilder.Entity("CME.Entities.User", b =>
                {
                    b.HasOne("CME.Entities.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId");

                    b.HasOne("CME.Entities.Organization", "Organization")
                        .WithMany()
                        .HasForeignKey("OrganizationId");

                    b.HasOne("CME.Entities.Title", "Title")
                        .WithMany()
                        .HasForeignKey("TitleId");

                    b.Navigation("Department");

                    b.Navigation("Organization");

                    b.Navigation("Title");
                });

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.HasOne("CME.Entities.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CME.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CME.Entities.TrainingForm", b =>
                {
                    b.Navigation("TrainingSubjects");
                });

            modelBuilder.Entity("CME.Entities.TrainingProgram", b =>
                {
                    b.Navigation("TrainingProgram_Users");
                });
#pragma warning restore 612, 618
        }
    }
}