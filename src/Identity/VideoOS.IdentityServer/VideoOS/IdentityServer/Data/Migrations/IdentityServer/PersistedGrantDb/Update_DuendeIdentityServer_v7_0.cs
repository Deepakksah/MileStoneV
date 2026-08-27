using System;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace VideoOS.IdentityServer.Data.Migrations.IdentityServer.PersistedGrantDb;

[DbContext(typeof(PersistedGrantDbContext))]
[Migration("20240425053215_Update_DuendeIdentityServer_v7_0")]
public class Update_DuendeIdentityServer_v7_0 : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropPrimaryKey("PK_ServerSideSessions", "ServerSideSessions");
		Type typeFromHandle = typeof(int);
		migrationBuilder.AlterColumn<long>("Id", "ServerSideSessions", "bigint", null, null, rowVersion: false, "dbo", nullable: false, null, null, null, typeFromHandle, "int").Annotation("SqlServer:Identity", "1, 1").OldAnnotation("SqlServer:Identity", "1, 1");
		migrationBuilder.AddPrimaryKey("PK_ServerSideSessions", "ServerSideSessions", "Id");
		migrationBuilder.CreateTable("PushedAuthorizationRequests", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id = table.Column<long>("bigint").Annotation("SqlServer:Identity", "1, 1");
			int? maxLength = 64;
			return new
			{
				Id = id,
				ReferenceValueHash = table.Column<string>("nvarchar(64)", null, maxLength),
				ExpiresAtUtc = table.Column<DateTime>("datetime2"),
				Parameters = table.Column<string>("nvarchar(max)")
			};
		}, "dbo", table =>
		{
			table.PrimaryKey("PK_PushedAuthorizationRequests", x => x.Id);
		});
		migrationBuilder.CreateIndex("IX_PushedAuthorizationRequests_ExpiresAtUtc", "PushedAuthorizationRequests", "ExpiresAtUtc", "dbo");
		migrationBuilder.CreateIndex("IX_PushedAuthorizationRequests_ReferenceValueHash", "PushedAuthorizationRequests", "ReferenceValueHash", "dbo", unique: true);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable("PushedAuthorizationRequests", "dbo");
		Type typeFromHandle = typeof(long);
		migrationBuilder.AlterColumn<int>("Id", "ServerSideSessions", "int", null, null, rowVersion: false, "dbo", nullable: false, null, null, null, typeFromHandle, "bigint").Annotation("SqlServer:Identity", "1, 1").OldAnnotation("SqlServer:Identity", "1, 1");
	}

	protected override void BuildTargetModel(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("dbo").HasAnnotation("ProductVersion", "8.0.4").HasAnnotation("Relational:MaxIdentifierLength", 128);
		modelBuilder.UseIdentityColumns(1L);
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.DeviceFlowCodes", delegate(EntityTypeBuilder b)
		{
			b.Property<string>("UserCode").HasMaxLength(200).HasColumnType("nvarchar(200)");
			b.Property<string>("ClientId").IsRequired().HasMaxLength(200)
				.HasColumnType("nvarchar(200)");
			b.Property<DateTime>("CreationTime").HasColumnType("datetime2");
			b.Property<string>("Data").IsRequired().HasMaxLength(50000)
				.HasColumnType("nvarchar(max)");
			b.Property<string>("Description").HasMaxLength(200).HasColumnType("nvarchar(200)");
			b.Property<string>("DeviceCode").IsRequired().HasMaxLength(200)
				.HasColumnType("nvarchar(200)");
			b.Property<DateTime?>("Expiration").IsRequired().HasColumnType("datetime2");
			b.Property<string>("SessionId").HasMaxLength(100).HasColumnType("nvarchar(100)");
			b.Property<string>("SubjectId").HasMaxLength(200).HasColumnType("nvarchar(200)");
			b.HasKey("UserCode");
			b.HasIndex("DeviceCode").IsUnique();
			b.HasIndex("Expiration");
			b.ToTable("DeviceCodes", "dbo");
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.Key", delegate(EntityTypeBuilder b)
		{
			b.Property<string>("Id").HasColumnType("nvarchar(450)");
			b.Property<string>("Algorithm").IsRequired().HasMaxLength(100)
				.HasColumnType("nvarchar(100)");
			b.Property<DateTime>("Created").HasColumnType("datetime2");
			b.Property<string>("Data").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<bool>("DataProtected").HasColumnType("bit");
			b.Property<bool>("IsX509Certificate").HasColumnType("bit");
			b.Property<string>("Use").HasColumnType("nvarchar(450)");
			b.Property<int>("Version").HasColumnType("int");
			b.HasKey("Id");
			b.HasIndex("Use");
			b.ToTable("Keys", "dbo");
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.PersistedGrant", delegate(EntityTypeBuilder b)
		{
			b.Property<long>("Id").ValueGeneratedOnAdd().HasColumnType("bigint");
			b.Property<long>("Id").UseIdentityColumn(1L);
			b.Property<string>("ClientId").IsRequired().HasMaxLength(200)
				.HasColumnType("nvarchar(200)");
			b.Property<DateTime?>("ConsumedTime").HasColumnType("datetime2");
			b.Property<DateTime>("CreationTime").HasColumnType("datetime2");
			b.Property<string>("Data").IsRequired().HasMaxLength(50000)
				.HasColumnType("nvarchar(max)");
			b.Property<string>("Description").HasMaxLength(200).HasColumnType("nvarchar(200)");
			b.Property<DateTime?>("Expiration").HasColumnType("datetime2");
			b.Property<string>("Key").HasMaxLength(200).HasColumnType("nvarchar(200)");
			b.Property<string>("SessionId").HasMaxLength(100).HasColumnType("nvarchar(100)");
			b.Property<string>("SubjectId").HasMaxLength(200).HasColumnType("nvarchar(200)");
			b.Property<string>("Type").IsRequired().HasMaxLength(50)
				.HasColumnType("nvarchar(50)");
			b.HasKey("Id");
			b.HasIndex("ConsumedTime");
			b.HasIndex("Expiration");
			b.HasIndex("Key").IsUnique().HasFilter("[Key] IS NOT NULL");
			b.HasIndex("SubjectId", "ClientId", "Type");
			b.HasIndex("SubjectId", "SessionId", "Type");
			b.ToTable("PersistedGrants", "dbo");
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.PushedAuthorizationRequest", delegate(EntityTypeBuilder b)
		{
			b.Property<long>("Id").ValueGeneratedOnAdd().HasColumnType("bigint");
			b.Property<long>("Id").UseIdentityColumn(1L);
			b.Property<DateTime>("ExpiresAtUtc").HasColumnType("datetime2");
			b.Property<string>("Parameters").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<string>("ReferenceValueHash").IsRequired().HasMaxLength(64)
				.HasColumnType("nvarchar(64)");
			b.HasKey("Id");
			b.HasIndex("ExpiresAtUtc");
			b.HasIndex("ReferenceValueHash").IsUnique();
			b.ToTable("PushedAuthorizationRequests", "dbo");
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ServerSideSession", delegate(EntityTypeBuilder b)
		{
			b.Property<long>("Id").ValueGeneratedOnAdd().HasColumnType("bigint");
			b.Property<long>("Id").UseIdentityColumn(1L);
			b.Property<DateTime>("Created").HasColumnType("datetime2");
			b.Property<string>("Data").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<string>("DisplayName").HasMaxLength(100).HasColumnType("nvarchar(100)");
			b.Property<DateTime?>("Expires").HasColumnType("datetime2");
			b.Property<string>("Key").IsRequired().HasMaxLength(100)
				.HasColumnType("nvarchar(100)");
			b.Property<DateTime>("Renewed").HasColumnType("datetime2");
			b.Property<string>("Scheme").IsRequired().HasMaxLength(100)
				.HasColumnType("nvarchar(100)");
			b.Property<string>("SessionId").HasMaxLength(100).HasColumnType("nvarchar(100)");
			b.Property<string>("SubjectId").IsRequired().HasMaxLength(100)
				.HasColumnType("nvarchar(100)");
			b.HasKey("Id");
			b.HasIndex("DisplayName");
			b.HasIndex("Expires");
			b.HasIndex("Key").IsUnique();
			b.HasIndex("SessionId");
			b.HasIndex("SubjectId");
			b.ToTable("ServerSideSessions", "dbo");
		});
	}
}
