using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace VideoOS.IdentityServer.Data.Migrations.IdentityServer.ApplicationDb;

[DbContext(typeof(ApplicationDbContext))]
[Migration("20231114123308_AddTrustedIssuer")]
public class AddTrustedIssuer : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable("TrustedIssuers", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id = table.Column<int>("int").Annotation("SqlServer:Identity", "1, 1");
			int? maxLength = 450;
			OperationBuilder<AddColumnOperation> address = table.Column<string>("varchar(450)", null, maxLength);
			maxLength = 450;
			return new
			{
				Id = id,
				Address = address,
				Issuer = table.Column<string>("varchar(450)", null, maxLength)
			};
		}, "dbo", table =>
		{
			table.PrimaryKey("PK_TrustedIssuers", x => x.Id);
		});
		migrationBuilder.CreateIndex("IX_TrustedIssuers_Address", "TrustedIssuers", "Address", "dbo", unique: true);
		migrationBuilder.CreateIndex("IX_TrustedIssuers_Issuer", "TrustedIssuers", "Issuer", "dbo", unique: true);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable("TrustedIssuers", "dbo");
	}

	protected override void BuildTargetModel(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("dbo").HasAnnotation("ProductVersion", "6.0.1").HasAnnotation("Relational:MaxIdentifierLength", 128);
		modelBuilder.UseIdentityColumns(1L);
		modelBuilder.Entity("VideoOS.IdentityServer.Entities.AdminIdentity", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int");
			b.Property<int>("Id").UseIdentityColumn(1L);
			b.Property<string>("Identity").IsRequired().HasMaxLength(450)
				.HasColumnType("nvarchar(450)");
			b.HasKey("Id");
			b.HasIndex("Identity").IsUnique();
			b.ToTable("AdminIdentities", "dbo");
		});
		modelBuilder.Entity("VideoOS.IdentityServer.Entities.EventLog", delegate(EntityTypeBuilder b)
		{
			b.Property<long>("Id").ValueGeneratedOnAdd().HasColumnType("bigint");
			b.Property<long>("Id").UseIdentityColumn(1L);
			b.Property<string>("Event").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<string>("ObjectId").HasColumnType("nvarchar(max)");
			b.Property<string>("ObjectName").HasColumnType("nvarchar(max)");
			b.Property<string>("RemoteIpAddress").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<DateTimeOffset>("TimeStamp").HasColumnType("datetimeoffset");
			b.Property<string>("UserName").IsRequired().HasColumnType("nvarchar(max)");
			b.HasKey("Id");
			b.ToTable("EventLogs", "dbo");
		});
		modelBuilder.Entity("VideoOS.IdentityServer.Entities.LoginProvider", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("Authority").IsRequired().HasMaxLength(450)
				.HasColumnType("nvarchar(450)");
			b.Property<string>("CallBackPath").IsRequired().HasMaxLength(450)
				.HasColumnType("nvarchar(450)");
			b.Property<string>("ClientId").IsRequired().HasMaxLength(100)
				.HasColumnType("nvarchar(100)");
			b.Property<string>("ClientSecret").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<string>("DisplayName").IsRequired().HasMaxLength(100)
				.HasColumnType("nvarchar(100)");
			b.Property<bool>("Enabled").HasColumnType("bit");
			b.Property<bool>("PromptForLogin").HasColumnType("bit");
			b.Property<string>("Scopes").HasColumnType("nvarchar(max)");
			b.Property<string>("UserNameClaimType").IsRequired().HasMaxLength(100)
				.HasColumnType("nvarchar(100)");
			b.HasKey("Id");
			b.HasIndex("Authority").IsUnique();
			b.HasIndex("DisplayName").IsUnique();
			b.ToTable("LoginProviders", "dbo");
		});
		modelBuilder.Entity("VideoOS.IdentityServer.Entities.TrustedIssuer", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int");
			b.Property<int>("Id").UseIdentityColumn(1L);
			b.Property<string>("Address").IsRequired().HasMaxLength(450)
				.HasColumnType("varchar(450)");
			b.Property<string>("Issuer").IsRequired().HasMaxLength(450)
				.HasColumnType("varchar(450)");
			b.HasKey("Id");
			b.HasIndex("Address").IsUnique();
			b.HasIndex("Issuer").IsUnique();
			b.ToTable("TrustedIssuers", "dbo");
		});
	}
}
