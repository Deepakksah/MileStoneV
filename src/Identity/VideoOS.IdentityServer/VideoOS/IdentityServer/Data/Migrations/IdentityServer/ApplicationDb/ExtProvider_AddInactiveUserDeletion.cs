using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VideoOS.IdentityServer.Data.Migrations.IdentityServer.ApplicationDb;

[DbContext(typeof(ApplicationDbContext))]
[Migration("20241206123212_ExtProvider_AddInactiveUserDeletion")]
public class ExtProvider_AddInactiveUserDeletion : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AddColumn<bool>("EnableInactiveUserDeletion", "LoginProviders", "bit", null, null, rowVersion: false, "dbo", nullable: false, false);
		migrationBuilder.AddColumn<int>("InactiveUserDeletionAfterDays", "LoginProviders", "int", null, null, rowVersion: false, "dbo", nullable: false, 30);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn("EnableInactiveUserDeletion", "LoginProviders", "dbo");
		migrationBuilder.DropColumn("InactiveUserDeletionAfterDays", "LoginProviders", "dbo");
	}

	protected override void BuildTargetModel(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("dbo").HasAnnotation("ProductVersion", "8.0.4").HasAnnotation("Relational:MaxIdentifierLength", 128);
		modelBuilder.UseIdentityColumns(1L);
		modelBuilder.Entity("VideoOS.IdentityServer.DomainEventInfrastructure.OutboxMessage", delegate(EntityTypeBuilder b)
		{
			b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier")
				.HasDefaultValueSql("NEWSEQUENTIALID()");
			b.Property<int>("CompatibilityLevel").HasColumnType("int");
			b.Property<DateTime>("CreatedAtUtc").ValueGeneratedOnAdd().HasColumnType("datetime2")
				.HasDefaultValueSql("SYSUTCDATETIME()");
			b.Property<string>("Error").HasColumnType("nvarchar(max)");
			b.Property<string>("EventType").IsRequired().HasMaxLength(200)
				.HasColumnType("nvarchar(200)");
			b.Property<DateTime?>("LastAttemptUtc").HasColumnType("datetime2");
			b.Property<DateTime?>("LockExpiresAtUtc").HasColumnType("datetime2");
			b.Property<string>("Payload").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<DateTime?>("ProcessedAtUtc").HasColumnType("datetime2");
			b.Property<string>("ProcessingInstanceId").HasMaxLength(36).HasColumnType("nvarchar(36)");
			b.Property<int>("RetryCount").ValueGeneratedOnAdd().HasColumnType("int")
				.HasDefaultValue(0);
			b.Property<int>("Status").HasColumnType("int");
			b.HasKey("Id");
			b.HasIndex("Status", "CompatibilityLevel", "Id").HasDatabaseName("IX_OutboxMessages_New");
			b.HasIndex("Status", "CompatibilityLevel", "Id").IncludeProperties("ProcessingInstanceId", "LockExpiresAtUtc", "CreatedAtUtc");
			b.HasIndex("Status", "CompatibilityLevel", "LastAttemptUtc", "Id").HasDatabaseName("IX_OutboxMessages_Retry");
			b.HasIndex("Status", "CompatibilityLevel", "LastAttemptUtc", "Id").IncludeProperties("ProcessingInstanceId", "LockExpiresAtUtc", "RetryCount", "CreatedAtUtc");
			b.ToTable("OutboxMessages", "dbo");
		});
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
			b.Property<bool>("EnableInactiveUserDeletion").HasColumnType("bit");
			b.Property<bool>("Enabled").HasColumnType("bit");
			b.Property<int>("InactiveUserDeletionAfterDays").HasColumnType("int");
			b.Property<string>("MappingClaimType").IsRequired().HasMaxLength(100)
				.HasColumnType("nvarchar(100)");
			b.Property<bool>("PromptForLogin").HasColumnType("bit");
			b.Property<string>("Scopes").HasColumnType("nvarchar(max)");
			b.Property<string>("UserNameClaimType").IsRequired().HasMaxLength(100)
				.HasColumnType("nvarchar(100)");
			b.HasKey("Id");
			b.HasIndex("Authority").IsUnique();
			b.HasIndex("DisplayName").IsUnique();
			b.ToTable("LoginProviders", "dbo");
		});
		modelBuilder.Entity("VideoOS.IdentityServer.Entities.LoginProviderClient", delegate(EntityTypeBuilder b)
		{
			b.Property<string>("ClientId").HasColumnType("nvarchar(450)");
			b.Property<Guid>("LoginProviderId").HasColumnType("uniqueidentifier");
			b.HasKey("ClientId");
			b.HasIndex("LoginProviderId");
			b.ToTable("LoginProviderClients", "dbo");
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
		modelBuilder.Entity("VideoOS.IdentityServer.Entities.LoginProviderClient", delegate(EntityTypeBuilder b)
		{
			b.HasOne("VideoOS.IdentityServer.Entities.LoginProvider", null).WithMany("ScimClients").HasForeignKey("LoginProviderId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("VideoOS.IdentityServer.Entities.LoginProvider", delegate(EntityTypeBuilder b)
		{
			b.Navigation("ScimClients");
		});
	}
}
