using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VideoOS.IdentityServer.Data.Migrations.IdentityServer.ApplicationDb;

[DbContext(typeof(ApplicationDbContext))]
[Migration("20240920070759_ExtProvider_AddDefaultScopes_ExistingProvidersPre24R2")]
public class ExtProvider_AddDefaultScopes_ExistingProvidersPre24R2 : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		string value = "\"profile\"";
		string value2 = "\"email\"";
		migrationBuilder.Sql($"\r\n                DECLARE @scopes NVARCHAR(MAX);\r\n                SELECT TOP(1) @scopes = Scopes FROM [dbo].[LoginProviders];\r\n                    IF (@scopes = '[]')\r\n\t                BEGIN\r\n\t\t                UPDATE [dbo].[LoginProviders] SET Scopes = '[{value},{value2}]';\r\n\t\t                RETURN;\r\n\t                END\r\n                DECLARE @i_email INT;\r\n                DECLARE @i_profile INT;\r\n                SELECT @i_email = PATINDEX('%{value2}%', @scopes);\r\n                SELECT @i_profile = PATINDEX('%{value}%', @scopes);\r\n                IF @i_email > 0 AND @i_profile > 0\r\n\t                RETURN;\r\n                DECLARE @add_Scopes NVARCHAR(MAX);\r\n                IF @i_profile = 0 AND @i_email > 0\r\n\t                SELECT @add_Scopes = '{value}';\r\n                ELSE IF @i_profile > 0 AND @i_email = 0\r\n\t                SELECT @add_Scopes = '{value2}';\r\n                ELSE IF @i_profile = 0 AND @i_email = 0\r\n\t                SELECT @add_Scopes = '{value},{value2}';\r\n                DECLARE @trimmed NVARCHAR(MAX);\r\n                SELECT @trimmed = REPLACE(@scopes, '[', '');\r\n                SELECT @scopes = '[' + @add_Scopes + ',' + @trimmed;\r\n                UPDATE [dbo].[LoginProviders] SET Scopes = @scopes;\r\n                RETURN;\r\n            ");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
	}

	protected override void BuildTargetModel(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("dbo").HasAnnotation("ProductVersion", "8.0.4").HasAnnotation("Relational:MaxIdentifierLength", 128);
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
