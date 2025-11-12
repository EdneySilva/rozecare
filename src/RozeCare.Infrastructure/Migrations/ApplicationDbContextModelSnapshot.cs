using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RozeCare.Infrastructure.Persistence;

#nullable disable

namespace RozeCare.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            Npgsql.EntityFrameworkCore.PostgreSQL.Metadata.NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<Guid>", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<string>("ConcurrencyStamp");

                b.Property<string>("Name")
                    .HasMaxLength(256);

                b.Property<string>("NormalizedName")
                    .HasMaxLength(256);

                b.HasKey("Id");

                b.HasIndex("NormalizedName")
                    .IsUnique()
                    .HasDatabaseName("RoleNameIndex");

                b.ToTable("AspNetRoles", (string)null);
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<Guid>", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                Npgsql.EntityFrameworkCore.PostgreSQL.Metadata.NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                b.Property<string>("ClaimType");

                b.Property<string>("ClaimValue");

                b.Property<Guid>("RoleId");

                b.HasKey("Id");

                b.HasIndex("RoleId");

                b.ToTable("AspNetRoleClaims", (string)null);
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<Guid>", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                Npgsql.EntityFrameworkCore.PostgreSQL.Metadata.NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                b.Property<string>("ClaimType");

                b.Property<string>("ClaimValue");

                b.Property<Guid>("UserId");

                b.HasKey("Id");

                b.HasIndex("UserId");

                b.ToTable("AspNetUserClaims", (string)null);
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<Guid>", b =>
            {
                b.Property<string>("LoginProvider")
                    .HasMaxLength(128);

                b.Property<string>("ProviderKey")
                    .HasMaxLength(128);

                b.Property<string>("ProviderDisplayName");

                b.Property<Guid>("UserId");

                b.HasKey("LoginProvider", "ProviderKey");

                b.HasIndex("UserId");

                b.ToTable("AspNetUserLogins", (string)null);
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<Guid>", b =>
            {
                b.Property<Guid>("UserId");

                b.Property<Guid>("RoleId");

                b.HasKey("UserId", "RoleId");

                b.HasIndex("RoleId");

                b.ToTable("AspNetUserRoles", (string)null);
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<Guid>", b =>
            {
                b.Property<Guid>("UserId");

                b.Property<string>("LoginProvider")
                    .HasMaxLength(128);

                b.Property<string>("Name")
                    .HasMaxLength(128);

                b.Property<string>("Value");

                b.HasKey("UserId", "LoginProvider", "Name");

                b.ToTable("AspNetUserTokens", (string)null);
            });

            modelBuilder.Entity("RozeCare.Domain.Entities.Allergy", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<Guid?>("CreatedBy");

                b.Property<DateTime>("CreatedAtUtc");

                b.Property<Guid?>("UpdatedBy");

                b.Property<DateTime?>("UpdatedAtUtc");

                b.Property<Guid>("PatientId");

                b.Property<string>("Reaction")
                    .IsRequired();

                b.Property<string>("Severity")
                    .IsRequired();

                b.Property<string>("Substance")
                    .IsRequired();

                b.HasKey("Id");

                b.HasIndex("PatientId");

                b.HasIndex("ProviderId");

                b.ToTable("Allergies");
            });

            modelBuilder.Entity("RozeCare.Domain.Entities.ApplicationUser", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<int>("AccessFailedCount");

                b.Property<DateOnly?>("BirthDate");

                b.Property<string>("ConcurrencyStamp");

                b.Property<string>("Country")
                    .HasMaxLength(3);

                b.Property<Guid?>("CreatedBy");

                b.Property<DateTime>("CreatedAtUtc");

                b.Property<string>("Email")
                    .HasMaxLength(256);

                b.Property<bool>("EmailConfirmed");

                b.Property<DateTime?>("LgpdConsentAtUtc");

                b.Property<bool>("LockoutEnabled");

                b.Property<DateTimeOffset?>("LockoutEnd");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValue(string.Empty);

                b.Property<string>("NormalizedEmail")
                    .HasMaxLength(256);

                b.Property<string>("NormalizedUserName")
                    .HasMaxLength(256);

                b.Property<string>("PasswordHash");

                b.Property<string>("PhoneNumber")
                    .HasMaxLength(30);

                b.Property<bool>("PhoneNumberConfirmed");

                b.Property<string>("Role")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValue("Patient");

                b.Property<string>("SecurityStamp");

                b.Property<bool>("TwoFactorEnabled");

                b.Property<Guid?>("UpdatedBy");

                b.Property<DateTime?>("UpdatedAtUtc");

                b.Property<string>("UserName")
                    .HasMaxLength(256);

                b.HasKey("Id");

                b.HasIndex("NormalizedEmail")
                    .HasDatabaseName("EmailIndex");

                b.HasIndex("NormalizedUserName")
                    .IsUnique()
                    .HasDatabaseName("UserNameIndex");

                b.ToTable("AspNetUsers", (string)null);
            });

            modelBuilder.Entity("RozeCare.Domain.Entities.AuditLog", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<Guid?>("ActorUserId");

                b.Property<string>("Action")
                    .IsRequired();

                b.Property<Guid?>("CreatedBy");

                b.Property<DateTime>("CreatedAtUtc");

                b.Property<string>("Details")
                    .IsRequired();

                b.Property<Guid?>("ResourceId");

                b.Property<string>("ResourceType")
                    .IsRequired();

                b.Property<Guid?>("UpdatedBy");

                b.Property<DateTime?>("UpdatedAtUtc");

                b.Property<DateTime>("WhenUtc");

                b.HasKey("Id");

                b.ToTable("AuditLogs");
            });

            modelBuilder.Entity("RozeCare.Domain.Entities.Consent", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<Guid?>("CreatedBy");

                b.Property<DateTime>("CreatedAtUtc");

                b.Property<Guid>("GranteeId");

                b.Property<string>("GranteeType")
                    .IsRequired()
                    .HasMaxLength(50);

                b.Property<Guid>("PatientId");

                b.Property<string>("Scopes")
                    .IsRequired();

                b.Property<string>("Status")
                    .IsRequired()
                    .HasMaxLength(50);

                b.Property<Guid?>("UpdatedBy");

                b.Property<DateTime?>("UpdatedAtUtc");

                b.Property<DateTime>("ExpiresAtUtc");

                b.HasKey("Id");

                b.HasIndex("PatientId", "Status");

                b.ToTable("Consents");
            });

            modelBuilder.Entity("RozeCare.Domain.Entities.Document", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<Guid?>("CreatedBy");

                b.Property<DateTime>("CreatedAtUtc");

                b.Property<string>("Description");

                b.Property<string>("BlobUrl")
                    .IsRequired();

                b.Property<string>("ContentType")
                    .IsRequired();

                b.Property<string>("FileName")
                    .IsRequired();

                b.Property<string>("Hash")
                    .IsRequired();

                b.Property<Guid>("PatientId");

                b.Property<long>("Size");

                b.Property<string>("Tags")
                    .IsRequired();

                b.Property<Guid?>("UpdatedBy");

                b.Property<DateTime?>("UpdatedAtUtc");

                b.Property<DateTime>("UploadedAt");

                b.HasKey("Id");

                b.HasIndex("PatientId");

                b.ToTable("Documents");
            });

            modelBuilder.Entity("RozeCare.Domain.Entities.Encounter", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<Guid?>("CreatedBy");

                b.Property<DateTime>("CreatedAtUtc");

                b.Property<DateTime>("Date");

                b.Property<string>("Diagnoses")
                    .IsRequired();

                b.Property<Guid>("PatientId");

                b.Property<Guid>("ProviderId");

                b.Property<string>("Notes")
                    .IsRequired();

                b.Property<string>("Prescriptions")
                    .IsRequired();

                b.Property<Guid?>("UpdatedBy");

                b.Property<DateTime?>("UpdatedAtUtc");

                b.Property<string>("Type")
                    .IsRequired();

                b.HasKey("Id");

                b.HasIndex("PatientId");

                b.ToTable("Encounters");
            });

            modelBuilder.Entity("RozeCare.Domain.Entities.Medication", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<Guid?>("CreatedBy");

                b.Property<DateTime>("CreatedAtUtc");

                b.Property<string>("Dosage")
                    .IsRequired();

                b.Property<DateTime?>("EndDate");

                b.Property<Guid>("PatientId");

                b.Property<string>("PrescribedBy");

                b.Property<DateTime>("StartDate");

                b.Property<Guid?>("UpdatedBy");

                b.Property<DateTime?>("UpdatedAtUtc");

                b.Property<string>("Name")
                    .IsRequired();

                b.HasKey("Id");

                b.HasIndex("PatientId");

                b.ToTable("Medications");
            });

            modelBuilder.Entity("RozeCare.Domain.Entities.Observation", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<Guid?>("CreatedBy");

                b.Property<DateTime>("CreatedAtUtc");

                b.Property<string>("Code")
                    .IsRequired();

                b.Property<string>("Display")
                    .IsRequired();

                b.Property<DateTime>("EffectiveDate");

                b.Property<Guid>("PatientId");

                b.Property<Guid?>("UpdatedBy");

                b.Property<DateTime?>("UpdatedAtUtc");

                b.Property<string>("Unit");

                b.Property<decimal?>("ValueQuantity");

                b.Property<string>("ValueString");

                b.Property<string>("ValueCodeable");

                b.HasKey("Id");

                b.HasIndex("PatientId", "Code");

                b.ToTable("Observations");
            });

            modelBuilder.Entity("RozeCare.Domain.Entities.PatientProfile", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<string>("Allergies")
                    .IsRequired();

                b.Property<string>("BloodType");

                b.Property<string>("Conditions")
                    .IsRequired();

                b.Property<Guid?>("CreatedBy");

                b.Property<DateTime>("CreatedAtUtc");

                b.Property<string>("EmergencyContacts")
                    .IsRequired();

                b.Property<string>("PreferredProviders")
                    .IsRequired();

                b.Property<Guid?>("UpdatedBy");

                b.Property<DateTime?>("UpdatedAtUtc");

                b.Property<Guid>("UserId");

                b.HasKey("Id");

                b.HasIndex("UserId");

                b.ToTable("PatientProfiles");
            });

            modelBuilder.Entity("RozeCare.Domain.Entities.Provider", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<string>("Accreditation");

                b.Property<string>("Address")
                    .IsRequired();

                b.Property<Guid?>("CreatedBy");

                b.Property<DateTime>("CreatedAtUtc");

                b.Property<string>("Contact")
                    .IsRequired();

                b.Property<string>("Name")
                    .IsRequired();

                b.Property<Guid?>("UpdatedBy");

                b.Property<DateTime?>("UpdatedAtUtc");

                b.Property<string>("Type")
                    .IsRequired();

                b.HasKey("Id");

                b.ToTable("Providers");
            });

            modelBuilder.Entity("RozeCare.Domain.Entities.Provider", b =>
            {
                b.Navigation("Encounters");
            });

            modelBuilder.Entity("RozeCare.Domain.Entities.UserRefreshToken", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<Guid?>("CreatedBy");

                b.Property<DateTime>("CreatedAtUtc");

                b.Property<bool>("IsRevoked");

                b.Property<Guid>("UserId");

                b.Property<string>("Token")
                    .IsRequired()
                    .HasMaxLength(500);

                b.Property<Guid?>("UpdatedBy");

                b.Property<DateTime?>("UpdatedAtUtc");

                b.Property<DateTime>("ExpiresAtUtc");

                b.HasKey("Id");

                b.HasIndex("UserId", "Token")
                    .IsUnique();

                b.ToTable("UserRefreshTokens");
            });

            modelBuilder.Entity("RozeCare.Domain.Entities.Allergy", b =>
            {
                b.HasOne("RozeCare.Domain.Entities.ApplicationUser", "Patient")
                    .WithMany()
                    .HasForeignKey("PatientId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Patient");
            });

            modelBuilder.Entity("RozeCare.Domain.Entities.Consent", b =>
            {
                b.HasOne("RozeCare.Domain.Entities.ApplicationUser", "Patient")
                    .WithMany("Consents")
                    .HasForeignKey("PatientId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Patient");
            });

            modelBuilder.Entity("RozeCare.Domain.Entities.Document", b =>
            {
                b.HasOne("RozeCare.Domain.Entities.ApplicationUser", "Patient")
                    .WithMany()
                    .HasForeignKey("PatientId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Patient");
            });

            modelBuilder.Entity("RozeCare.Domain.Entities.Encounter", b =>
            {
                b.HasOne("RozeCare.Domain.Entities.ApplicationUser", "Patient")
                    .WithMany()
                    .HasForeignKey("PatientId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("RozeCare.Domain.Entities.Provider", "Provider")
                    .WithMany("Encounters")
                    .HasForeignKey("ProviderId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Patient");

                b.Navigation("Provider");
            });

            modelBuilder.Entity("RozeCare.Domain.Entities.Medication", b =>
            {
                b.HasOne("RozeCare.Domain.Entities.ApplicationUser", "Patient")
                    .WithMany()
                    .HasForeignKey("PatientId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Patient");
            });

            modelBuilder.Entity("RozeCare.Domain.Entities.Observation", b =>
            {
                b.HasOne("RozeCare.Domain.Entities.ApplicationUser", "Patient")
                    .WithMany()
                    .HasForeignKey("PatientId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Patient");
            });

            modelBuilder.Entity("RozeCare.Domain.Entities.PatientProfile", b =>
            {
                b.HasOne("RozeCare.Domain.Entities.ApplicationUser", "User")
                    .WithMany("PatientProfiles")
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("User");
            });

            modelBuilder.Entity("RozeCare.Domain.Entities.UserRefreshToken", b =>
            {
                b.HasOne("RozeCare.Domain.Entities.ApplicationUser", "User")
                    .WithMany("RefreshTokens")
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("User");
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<Guid>", b =>
            {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<Guid>", null)
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<Guid>", b =>
            {
                b.HasOne("RozeCare.Domain.Entities.ApplicationUser", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<Guid>", b =>
            {
                b.HasOne("RozeCare.Domain.Entities.ApplicationUser", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<Guid>", b =>
            {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<Guid>", null)
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("RozeCare.Domain.Entities.ApplicationUser", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<Guid>", b =>
            {
                b.HasOne("RozeCare.Domain.Entities.ApplicationUser", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("RozeCare.Domain.Entities.ApplicationUser", b =>
            {
                b.Navigation("Consents");

                b.Navigation("PatientProfiles");

                b.Navigation("RefreshTokens");
            });
#pragma warning restore 612, 618
        }
    }
}
