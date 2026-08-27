using System;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VideoOS.IdentityServer.Data.Migrations.IdentityServer.PersistedGrantDb;

[DbContext(typeof(PersistedGrantDbContext))]
[Migration("20210406100125_UpdateTo_IdentityServer_4.1.0_PersistedGrantDbMigration")]
public class UpdateTo_IdentityServer_410_PersistedGrantDbMigration : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.EnsureSchema("dbo");
		migrationBuilder.RenameTable("PersistedGrants", null, "PersistedGrants", "dbo");
		migrationBuilder.RenameTable("DeviceCodes", null, "DeviceCodes", "dbo");
		migrationBuilder.AddColumn<DateTime>("ConsumedTime", "PersistedGrants", null, null, null, rowVersion: false, "dbo", nullable: true);
		int? maxLength = 200;
		migrationBuilder.AddColumn<string>("Description", "PersistedGrants", null, null, maxLength, rowVersion: false, "dbo", nullable: true);
		maxLength = 100;
		migrationBuilder.AddColumn<string>("SessionId", "PersistedGrants", null, null, maxLength, rowVersion: false, "dbo", nullable: true);
		maxLength = 200;
		migrationBuilder.AddColumn<string>("Description", "DeviceCodes", null, null, maxLength, rowVersion: false, "dbo", nullable: true);
		maxLength = 100;
		migrationBuilder.AddColumn<string>("SessionId", "DeviceCodes", null, null, maxLength, rowVersion: false, "dbo", nullable: true);
		migrationBuilder.CreateIndex("IX_PersistedGrants_SubjectId_SessionId_Type", "PersistedGrants", new string[3] { "SubjectId", "SessionId", "Type" }, "dbo");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropIndex("IX_PersistedGrants_SubjectId_SessionId_Type", "PersistedGrants", "dbo");
		migrationBuilder.DropColumn("ConsumedTime", "PersistedGrants", "dbo");
		migrationBuilder.DropColumn("Description", "PersistedGrants", "dbo");
		migrationBuilder.DropColumn("SessionId", "PersistedGrants", "dbo");
		migrationBuilder.DropColumn("Description", "DeviceCodes", "dbo");
		migrationBuilder.DropColumn("SessionId", "DeviceCodes", "dbo");
		migrationBuilder.RenameTable("PersistedGrants", "dbo", "PersistedGrants");
		migrationBuilder.RenameTable("DeviceCodes", "dbo", "DeviceCodes");
	}

	protected override void BuildTargetModel(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("dbo").HasAnnotation("ProductVersion", "3.1.5").HasAnnotation("Relational:MaxIdentifierLength", 128)
			.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.DeviceFlowCodes", delegate(EntityTypeBuilder b)
		{
			b.Property<string>("UserCode").HasColumnType("nvarchar(200)").HasMaxLength(200);
			b.Property<string>("ClientId").IsRequired().HasColumnType("nvarchar(200)")
				.HasMaxLength(200);
			b.Property<DateTime>("CreationTime").HasColumnType("datetime2");
			b.Property<string>("Data").IsRequired().HasColumnType("nvarchar(max)")
				.HasMaxLength(50000);
			b.Property<string>("Description").HasColumnType("nvarchar(200)").HasMaxLength(200);
			b.Property<string>("DeviceCode").IsRequired().HasColumnType("nvarchar(200)")
				.HasMaxLength(200);
			b.Property<DateTime?>("Expiration").IsRequired().HasColumnType("datetime2");
			b.Property<string>("SessionId").HasColumnType("nvarchar(100)").HasMaxLength(100);
			b.Property<string>("SubjectId").HasColumnType("nvarchar(200)").HasMaxLength(200);
			b.HasKey("UserCode");
			b.HasIndex("DeviceCode").IsUnique();
			b.HasIndex("Expiration");
			b.ToTable("DeviceCodes");
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.PersistedGrant", delegate(EntityTypeBuilder b)
		{
			b.Property<string>("Key").HasColumnType("nvarchar(200)").HasMaxLength(200);
			b.Property<string>("ClientId").IsRequired().HasColumnType("nvarchar(200)")
				.HasMaxLength(200);
			b.Property<DateTime?>("ConsumedTime").HasColumnType("datetime2");
			b.Property<DateTime>("CreationTime").HasColumnType("datetime2");
			b.Property<string>("Data").IsRequired().HasColumnType("nvarchar(max)")
				.HasMaxLength(50000);
			b.Property<string>("Description").HasColumnType("nvarchar(200)").HasMaxLength(200);
			b.Property<DateTime?>("Expiration").HasColumnType("datetime2");
			b.Property<string>("SessionId").HasColumnType("nvarchar(100)").HasMaxLength(100);
			b.Property<string>("SubjectId").HasColumnType("nvarchar(200)").HasMaxLength(200);
			b.Property<string>("Type").IsRequired().HasColumnType("nvarchar(50)")
				.HasMaxLength(50);
			b.HasKey("Key");
			b.HasIndex("Expiration");
			b.HasIndex("SubjectId", "ClientId", "Type");
			b.HasIndex("SubjectId", "SessionId", "Type");
			b.ToTable("PersistedGrants");
		});
	}
}
