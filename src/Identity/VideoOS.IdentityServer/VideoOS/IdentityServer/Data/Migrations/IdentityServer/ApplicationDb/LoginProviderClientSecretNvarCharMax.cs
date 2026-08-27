using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VideoOS.IdentityServer.Data.Migrations.IdentityServer.ApplicationDb;

[DbContext(typeof(ApplicationDbContext))]
[Migration("20220201100532_LoginProviderClientSecretNvarCharMax")]
public class LoginProviderClientSecretNvarCharMax : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		Type typeFromHandle = typeof(string);
		int? oldMaxLength = 450;
		migrationBuilder.AlterColumn<string>("ClientSecret", "LoginProviders", null, null, null, rowVersion: false, "dbo", nullable: false, null, null, null, typeFromHandle, "nvarchar(450)", null, oldMaxLength);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		int? maxLength = 450;
		Type typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("ClientSecret", "LoginProviders", "nvarchar(450)", null, maxLength, rowVersion: false, "dbo", nullable: false, null, null, null, typeFromHandle);
	}

	protected override void BuildTargetModel(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("dbo").HasAnnotation("ProductVersion", "3.1.5").HasAnnotation("Relational:MaxIdentifierLength", 128)
			.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
		modelBuilder.Entity("VideoOS.IdentityServer.Entities.AdminIdentity", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int")
				.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<string>("Identity").IsRequired().HasColumnType("nvarchar(450)")
				.HasMaxLength(450);
			b.HasKey("Id");
			b.HasIndex("Identity").IsUnique();
			b.ToTable("AdminIdentities");
		});
		modelBuilder.Entity("VideoOS.IdentityServer.Entities.EventLog", delegate(EntityTypeBuilder b)
		{
			b.Property<long>("Id").ValueGeneratedOnAdd().HasColumnType("bigint")
				.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<string>("Event").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<string>("ObjectId").HasColumnType("nvarchar(max)");
			b.Property<string>("ObjectName").HasColumnType("nvarchar(max)");
			b.Property<string>("RemoteIpAddress").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<DateTimeOffset>("TimeStamp").HasColumnType("datetimeoffset");
			b.Property<string>("UserName").IsRequired().HasColumnType("nvarchar(max)");
			b.HasKey("Id");
			b.ToTable("EventLogs");
		});
		modelBuilder.Entity("VideoOS.IdentityServer.Entities.LoginProvider", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
			b.Property<string>("Authority").IsRequired().HasColumnType("nvarchar(450)")
				.HasMaxLength(450);
			b.Property<string>("CallBackPath").IsRequired().HasColumnType("nvarchar(450)")
				.HasMaxLength(450);
			b.Property<string>("ClientId").IsRequired().HasColumnType("nvarchar(100)")
				.HasMaxLength(100);
			b.Property<string>("ClientSecret").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<string>("DisplayName").IsRequired().HasColumnType("nvarchar(100)")
				.HasMaxLength(100);
			b.Property<bool>("Enabled").HasColumnType("bit");
			b.Property<bool>("PromptForLogin").HasColumnType("bit");
			b.Property<string>("Scopes").HasColumnType("nvarchar(max)");
			b.Property<string>("UserNameClaimType").IsRequired().HasColumnType("nvarchar(100)")
				.HasMaxLength(100);
			b.HasKey("Id");
			b.HasIndex("Authority").IsUnique();
			b.HasIndex("DisplayName").IsUnique();
			b.ToTable("LoginProviders");
		});
	}
}
