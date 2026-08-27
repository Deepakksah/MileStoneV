using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace VideoOS.IdentityServer.Data.Migrations.Identity.IdentityDb;

[DbContext(typeof(IdentityDbContextWrapper))]
[Migration("20191021085101_InitialIdentityDbMigration")]
public class InitialIdentityDbMigration : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable("AspNetRoles", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id = table.Column<string>();
			int? maxLength = 256;
			OperationBuilder<AddColumnOperation> name = table.Column<string>(null, null, maxLength, rowVersion: false, null, nullable: true);
			maxLength = 256;
			return new
			{
				Id = id,
				Name = name,
				NormalizedName = table.Column<string>(null, null, maxLength, rowVersion: false, null, nullable: true),
				ConcurrencyStamp = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true)
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_AspNetRoles", x => x.Id);
		});
		migrationBuilder.CreateTable("AspNetUsers", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id = table.Column<string>();
			int? maxLength = 256;
			OperationBuilder<AddColumnOperation> userName = table.Column<string>(null, null, maxLength, rowVersion: false, null, nullable: true);
			maxLength = 256;
			OperationBuilder<AddColumnOperation> normalizedUserName = table.Column<string>(null, null, maxLength, rowVersion: false, null, nullable: true);
			maxLength = 256;
			OperationBuilder<AddColumnOperation> email = table.Column<string>(null, null, maxLength, rowVersion: false, null, nullable: true);
			maxLength = 256;
			return new
			{
				Id = id,
				UserName = userName,
				NormalizedUserName = normalizedUserName,
				Email = email,
				NormalizedEmail = table.Column<string>(null, null, maxLength, rowVersion: false, null, nullable: true),
				EmailConfirmed = table.Column<bool>(),
				PasswordHash = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
				SecurityStamp = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
				ConcurrencyStamp = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
				PhoneNumber = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
				PhoneNumberConfirmed = table.Column<bool>(),
				TwoFactorEnabled = table.Column<bool>(),
				LockoutEnd = table.Column<DateTimeOffset>(null, null, null, rowVersion: false, null, nullable: true),
				LockoutEnabled = table.Column<bool>(),
				AccessFailedCount = table.Column<int>()
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_AspNetUsers", x => x.Id);
		});
		migrationBuilder.CreateTable("AspNetRoleClaims", (ColumnsBuilder table) => new
		{
			Id = table.Column<int>().Annotation("SqlServer:Identity", "1, 1"),
			RoleId = table.Column<string>(),
			ClaimType = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			ClaimValue = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true)
		}, null, table =>
		{
			table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
			table.ForeignKey("FK_AspNetRoleClaims_AspNetRoles_RoleId", x => x.RoleId, "AspNetRoles", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("AspNetUserClaims", (ColumnsBuilder table) => new
		{
			Id = table.Column<int>().Annotation("SqlServer:Identity", "1, 1"),
			UserId = table.Column<string>(),
			ClaimType = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			ClaimValue = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true)
		}, null, table =>
		{
			table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
			table.ForeignKey("FK_AspNetUserClaims_AspNetUsers_UserId", x => x.UserId, "AspNetUsers", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("AspNetUserLogins", (ColumnsBuilder table) => new
		{
			LoginProvider = table.Column<string>(),
			ProviderKey = table.Column<string>(),
			ProviderDisplayName = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true),
			UserId = table.Column<string>()
		}, null, table =>
		{
			table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
			table.ForeignKey("FK_AspNetUserLogins_AspNetUsers_UserId", x => x.UserId, "AspNetUsers", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("AspNetUserRoles", (ColumnsBuilder table) => new
		{
			UserId = table.Column<string>(),
			RoleId = table.Column<string>()
		}, null, table =>
		{
			table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
			table.ForeignKey("FK_AspNetUserRoles_AspNetRoles_RoleId", x => x.RoleId, "AspNetRoles", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
			table.ForeignKey("FK_AspNetUserRoles_AspNetUsers_UserId", x => x.UserId, "AspNetUsers", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("AspNetUserTokens", (ColumnsBuilder table) => new
		{
			UserId = table.Column<string>(),
			LoginProvider = table.Column<string>(),
			Name = table.Column<string>(),
			Value = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true)
		}, null, table =>
		{
			table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
			table.ForeignKey("FK_AspNetUserTokens_AspNetUsers_UserId", x => x.UserId, "AspNetUsers", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateIndex("IX_AspNetRoleClaims_RoleId", "AspNetRoleClaims", "RoleId");
		migrationBuilder.CreateIndex("RoleNameIndex", "AspNetRoles", "NormalizedName", null, unique: true, "[NormalizedName] IS NOT NULL");
		migrationBuilder.CreateIndex("IX_AspNetUserClaims_UserId", "AspNetUserClaims", "UserId");
		migrationBuilder.CreateIndex("IX_AspNetUserLogins_UserId", "AspNetUserLogins", "UserId");
		migrationBuilder.CreateIndex("IX_AspNetUserRoles_RoleId", "AspNetUserRoles", "RoleId");
		migrationBuilder.CreateIndex("EmailIndex", "AspNetUsers", "NormalizedEmail");
		migrationBuilder.CreateIndex("UserNameIndex", "AspNetUsers", "NormalizedUserName", null, unique: true, "[NormalizedUserName] IS NOT NULL");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable("AspNetRoleClaims");
		migrationBuilder.DropTable("AspNetUserClaims");
		migrationBuilder.DropTable("AspNetUserLogins");
		migrationBuilder.DropTable("AspNetUserRoles");
		migrationBuilder.DropTable("AspNetUserTokens");
		migrationBuilder.DropTable("AspNetRoles");
		migrationBuilder.DropTable("AspNetUsers");
	}

	protected override void BuildTargetModel(ModelBuilder modelBuilder)
	{
		modelBuilder.HasAnnotation("ProductVersion", "3.0.0").HasAnnotation("Relational:MaxIdentifierLength", 128).HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
		modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", delegate(EntityTypeBuilder b)
		{
			b.Property<string>("Id").HasColumnType("nvarchar(450)");
			b.Property<string>("ConcurrencyStamp").IsConcurrencyToken().HasColumnType("nvarchar(max)");
			b.Property<string>("Name").HasColumnType("nvarchar(256)").HasMaxLength(256);
			b.Property<string>("NormalizedName").HasColumnType("nvarchar(256)").HasMaxLength(256);
			b.HasKey("Id");
			b.HasIndex("NormalizedName").IsUnique().HasName("RoleNameIndex")
				.HasFilter("[NormalizedName] IS NOT NULL");
			b.ToTable("AspNetRoles");
		});
		modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int")
				.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<string>("ClaimType").HasColumnType("nvarchar(max)");
			b.Property<string>("ClaimValue").HasColumnType("nvarchar(max)");
			b.Property<string>("RoleId").IsRequired().HasColumnType("nvarchar(450)");
			b.HasKey("Id");
			b.HasIndex("RoleId");
			b.ToTable("AspNetRoleClaims");
		});
		modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", delegate(EntityTypeBuilder b)
		{
			b.Property<string>("Id").HasColumnType("nvarchar(450)");
			b.Property<int>("AccessFailedCount").HasColumnType("int");
			b.Property<string>("ConcurrencyStamp").IsConcurrencyToken().HasColumnType("nvarchar(max)");
			b.Property<string>("Email").HasColumnType("nvarchar(256)").HasMaxLength(256);
			b.Property<bool>("EmailConfirmed").HasColumnType("bit");
			b.Property<bool>("LockoutEnabled").HasColumnType("bit");
			b.Property<DateTimeOffset?>("LockoutEnd").HasColumnType("datetimeoffset");
			b.Property<string>("NormalizedEmail").HasColumnType("nvarchar(256)").HasMaxLength(256);
			b.Property<string>("NormalizedUserName").HasColumnType("nvarchar(256)").HasMaxLength(256);
			b.Property<string>("PasswordHash").HasColumnType("nvarchar(max)");
			b.Property<string>("PhoneNumber").HasColumnType("nvarchar(max)");
			b.Property<bool>("PhoneNumberConfirmed").HasColumnType("bit");
			b.Property<string>("SecurityStamp").HasColumnType("nvarchar(max)");
			b.Property<bool>("TwoFactorEnabled").HasColumnType("bit");
			b.Property<string>("UserName").HasColumnType("nvarchar(256)").HasMaxLength(256);
			b.HasKey("Id");
			b.HasIndex("NormalizedEmail").HasName("EmailIndex");
			b.HasIndex("NormalizedUserName").IsUnique().HasName("UserNameIndex")
				.HasFilter("[NormalizedUserName] IS NOT NULL");
			b.ToTable("AspNetUsers");
		});
		modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int")
				.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<string>("ClaimType").HasColumnType("nvarchar(max)");
			b.Property<string>("ClaimValue").HasColumnType("nvarchar(max)");
			b.Property<string>("UserId").IsRequired().HasColumnType("nvarchar(450)");
			b.HasKey("Id");
			b.HasIndex("UserId");
			b.ToTable("AspNetUserClaims");
		});
		modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", delegate(EntityTypeBuilder b)
		{
			b.Property<string>("LoginProvider").HasColumnType("nvarchar(450)");
			b.Property<string>("ProviderKey").HasColumnType("nvarchar(450)");
			b.Property<string>("ProviderDisplayName").HasColumnType("nvarchar(max)");
			b.Property<string>("UserId").IsRequired().HasColumnType("nvarchar(450)");
			b.HasKey("LoginProvider", "ProviderKey");
			b.HasIndex("UserId");
			b.ToTable("AspNetUserLogins");
		});
		modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", delegate(EntityTypeBuilder b)
		{
			b.Property<string>("UserId").HasColumnType("nvarchar(450)");
			b.Property<string>("RoleId").HasColumnType("nvarchar(450)");
			b.HasKey("UserId", "RoleId");
			b.HasIndex("RoleId");
			b.ToTable("AspNetUserRoles");
		});
		modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", delegate(EntityTypeBuilder b)
		{
			b.Property<string>("UserId").HasColumnType("nvarchar(450)");
			b.Property<string>("LoginProvider").HasColumnType("nvarchar(450)");
			b.Property<string>("Name").HasColumnType("nvarchar(450)");
			b.Property<string>("Value").HasColumnType("nvarchar(max)");
			b.HasKey("UserId", "LoginProvider", "Name");
			b.ToTable("AspNetUserTokens");
		});
		modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null).WithMany().HasForeignKey("RoleId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null).WithMany().HasForeignKey("UserId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null).WithMany().HasForeignKey("UserId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null).WithMany().HasForeignKey("RoleId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null).WithMany().HasForeignKey("UserId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null).WithMany().HasForeignKey("UserId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
	}
}
