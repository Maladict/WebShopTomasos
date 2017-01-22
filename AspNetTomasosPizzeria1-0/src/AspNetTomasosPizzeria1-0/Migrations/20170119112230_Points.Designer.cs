using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using AspNetTomasosPizzeria1_0.Models;

namespace AspNetTomasosPizzeria1_0.Migrations
{
    [DbContext(typeof(TomasosContext))]
    [Migration("20170119112230_Points")]
    partial class Points
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AspNetTomasosPizzeria1_0.Models.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<int>("KundId");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("KundId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("AspNetTomasosPizzeria1_0.Models.Bestallning", b =>
                {
                    b.Property<int>("BestallningId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("BestallningID");

                    b.Property<DateTime>("BestallningDatum")
                        .HasColumnType("datetime");

                    b.Property<int>("KundId")
                        .HasColumnName("KundID");

                    b.Property<bool>("Levererad");

                    b.Property<int>("Totalbelopp");

                    b.HasKey("BestallningId");

                    b.HasIndex("KundId");

                    b.ToTable("Bestallning");
                });

            modelBuilder.Entity("AspNetTomasosPizzeria1_0.Models.BestallningMatratt", b =>
                {
                    b.Property<int>("MatrattId")
                        .HasColumnName("MatrattID");

                    b.Property<int>("BestallningId")
                        .HasColumnName("BestallningID");

                    b.Property<int>("Antal")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("1");

                    b.HasKey("MatrattId", "BestallningId")
                        .HasName("PK_BestallningMatratt");

                    b.HasIndex("BestallningId");

                    b.ToTable("BestallningMatratt");
                });

            modelBuilder.Entity("AspNetTomasosPizzeria1_0.Models.Kund", b =>
                {
                    b.Property<int>("KundId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("KundID");

                    b.Property<string>("AnvandarNamn")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Gatuadress")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Losenord")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("Namn")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(100);

                    b.Property<int>("Points");

                    b.Property<string>("Postnr")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("Postort")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Role");

                    b.Property<string>("Telefon")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("KundId");

                    b.ToTable("Kund");
                });

            modelBuilder.Entity("AspNetTomasosPizzeria1_0.Models.Matratt", b =>
                {
                    b.Property<int>("MatrattId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("MatrattID");

                    b.Property<string>("Beskrivning")
                        .HasColumnType("varchar(200)");

                    b.Property<string>("MatrattNamn")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<int>("MatrattTyp");

                    b.Property<int>("Pris");

                    b.HasKey("MatrattId");

                    b.HasIndex("MatrattTyp");

                    b.ToTable("Matratt");
                });

            modelBuilder.Entity("AspNetTomasosPizzeria1_0.Models.MatrattProdukt", b =>
                {
                    b.Property<int>("MatrattId")
                        .HasColumnName("MatrattID");

                    b.Property<int>("ProduktId")
                        .HasColumnName("ProduktID");

                    b.HasKey("MatrattId", "ProduktId")
                        .HasName("PK_MatrattProdukt");

                    b.HasIndex("ProduktId");

                    b.ToTable("MatrattProdukt");
                });

            modelBuilder.Entity("AspNetTomasosPizzeria1_0.Models.MatrattTyp", b =>
                {
                    b.Property<int>("MatrattTyp1")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("MatrattTyp");

                    b.Property<string>("Beskrivning")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("MatrattTyp1")
                        .HasName("PK_MatrattTyp");

                    b.ToTable("MatrattTyp");
                });

            modelBuilder.Entity("AspNetTomasosPizzeria1_0.Models.Produkt", b =>
                {
                    b.Property<int>("ProduktId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ProduktID");

                    b.Property<string>("ProduktNamn")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("ProduktId");

                    b.ToTable("Produkt");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("AspNetTomasosPizzeria1_0.Models.AppUser", b =>
                {
                    b.HasOne("AspNetTomasosPizzeria1_0.Models.Kund", "Kund")
                        .WithMany()
                        .HasForeignKey("KundId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AspNetTomasosPizzeria1_0.Models.Bestallning", b =>
                {
                    b.HasOne("AspNetTomasosPizzeria1_0.Models.Kund", "Kund")
                        .WithMany("Bestallning")
                        .HasForeignKey("KundId")
                        .HasConstraintName("FK_Bestallning_Kund");
                });

            modelBuilder.Entity("AspNetTomasosPizzeria1_0.Models.BestallningMatratt", b =>
                {
                    b.HasOne("AspNetTomasosPizzeria1_0.Models.Bestallning", "Bestallning")
                        .WithMany("BestallningMatratt")
                        .HasForeignKey("BestallningId")
                        .HasConstraintName("FK_BestallningMatratt_Bestallning");

                    b.HasOne("AspNetTomasosPizzeria1_0.Models.Matratt", "Matratt")
                        .WithMany("BestallningMatratt")
                        .HasForeignKey("MatrattId")
                        .HasConstraintName("FK_BestallningMatratt_Matratt");
                });

            modelBuilder.Entity("AspNetTomasosPizzeria1_0.Models.Matratt", b =>
                {
                    b.HasOne("AspNetTomasosPizzeria1_0.Models.MatrattTyp", "MatrattTypNavigation")
                        .WithMany("Matratt")
                        .HasForeignKey("MatrattTyp");
                });

            modelBuilder.Entity("AspNetTomasosPizzeria1_0.Models.MatrattProdukt", b =>
                {
                    b.HasOne("AspNetTomasosPizzeria1_0.Models.Matratt", "Matratt")
                        .WithMany("MatrattProdukt")
                        .HasForeignKey("MatrattId")
                        .HasConstraintName("FK_MatrattProdukt_Matratt");

                    b.HasOne("AspNetTomasosPizzeria1_0.Models.Produkt", "Produkt")
                        .WithMany("MatrattProdukt")
                        .HasForeignKey("ProduktId")
                        .HasConstraintName("FK_MatrattProdukt_Produkt");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("AspNetTomasosPizzeria1_0.Models.AppUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("AspNetTomasosPizzeria1_0.Models.AppUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AspNetTomasosPizzeria1_0.Models.AppUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
