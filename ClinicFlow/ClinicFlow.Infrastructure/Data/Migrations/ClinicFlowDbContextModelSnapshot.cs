using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ClinicFlow.Infrastructure.Data;

#nullable disable

namespace ClinicFlow.Infrastructure.Data.Migrations
{
    [DbContext(typeof(ClinicFlowDbContext))]
    partial class ClinicFlowDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ClinicFlow.Domain.Entities.Appointment", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasAnnotation("SqlServer:Identity", "1, 1");

                b.Property<int>("DoctorId")
                    .HasColumnType("int");

                b.Property<int>("PatientId")
                    .HasColumnType("int");

                b.Property<DateTime>("ScheduledAt")
                    .HasColumnType("datetime2");

                b.Property<int>("UserId")
                    .HasColumnType("int");

                b.HasKey("Id");

                b.HasIndex("DoctorId");

                b.HasIndex("PatientId");

                b.HasIndex("UserId");

                b.HasIndex("ScheduledAt")
                    .IsClustered(false);

                b.ToTable("Appointments");
            });

            modelBuilder.Entity("ClinicFlow.Domain.Entities.Doctor", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasAnnotation("SqlServer:Identity", "1, 1");

                b.Property<string>("FirstName")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("LastName")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Specialty")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.HasKey("Id");

                b.ToTable("Doctors");
            });

            modelBuilder.Entity("ClinicFlow.Domain.Entities.Patient", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasAnnotation("SqlServer:Identity", "1, 1");

                b.Property<DateTime>("DateOfBirth")
                    .HasColumnType("datetime2");

                b.Property<string>("FirstName")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("LastName")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.HasKey("Id");

                b.ToTable("Patients");
            });

            modelBuilder.Entity("ClinicFlow.Domain.Entities.User", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasAnnotation("SqlServer:Identity", "1, 1");

                b.Property<string>("PasswordHash")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Username")
                    .IsRequired()
                    .HasColumnType("nvarchar(450)");

                b.HasKey("Id");

                b.HasIndex("Username")
                    .IsUnique();

                b.ToTable("Users");
            });

            modelBuilder.Entity("ClinicFlow.Domain.Entities.Appointment", b =>
            {
                b.HasOne("ClinicFlow.Domain.Entities.Doctor", "Doctor")
                    .WithMany("Appointments")
                    .HasForeignKey("DoctorId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("ClinicFlow.Domain.Entities.Patient", "Patient")
                    .WithMany("Appointments")
                    .HasForeignKey("PatientId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("ClinicFlow.Domain.Entities.User", "User")
                    .WithMany("Appointments")
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Doctor");

                b.Navigation("Patient");

                b.Navigation("User");
            });
#pragma warning restore 612, 618
        }
    }
}
