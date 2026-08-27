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
[Migration("20220531080439_Update_DuendeIdentityServer_v6_1")]
public class Update_DuendeIdentityServer_v6_1 : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropPrimaryKey("PK_PersistedGrants", "PersistedGrants", "dbo");
		int? maxLength = 200;
		Type typeFromHandle = typeof(string);
		int? oldMaxLength = 200;
		migrationBuilder.AlterColumn<string>("Key", "PersistedGrants", "nvarchar(200)", null, maxLength, rowVersion: false, "dbo", nullable: true, null, null, null, typeFromHandle, "nvarchar(200)", null, oldMaxLength);
		migrationBuilder.AddColumn<long>("Id", "PersistedGrants", "bigint", null, null, rowVersion: false, "dbo", nullable: false, 0L).Annotation("SqlServer:Identity", "1, 1");
		migrationBuilder.AddPrimaryKey("PK_PersistedGrants", "PersistedGrants", "Id", "dbo");
		migrationBuilder.CreateTable("ServerSideSessions", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id = table.Column<int>("int").Annotation("SqlServer:Identity", "1, 1");
			int? maxLength2 = 100;
			OperationBuilder<AddColumnOperation> key = table.Column<string>("nvarchar(100)", null, maxLength2);
			maxLength2 = 100;
			OperationBuilder<AddColumnOperation> scheme = table.Column<string>("nvarchar(100)", null, maxLength2);
			maxLength2 = 100;
			OperationBuilder<AddColumnOperation> subjectId = table.Column<string>("nvarchar(100)", null, maxLength2);
			maxLength2 = 100;
			OperationBuilder<AddColumnOperation> sessionId = table.Column<string>("nvarchar(100)", null, maxLength2, rowVersion: false, null, nullable: true);
			maxLength2 = 100;
			return new
			{
				Id = id,
				Key = key,
				Scheme = scheme,
				SubjectId = subjectId,
				SessionId = sessionId,
				DisplayName = table.Column<string>("nvarchar(100)", null, maxLength2, rowVersion: false, null, nullable: true),
				Created = table.Column<DateTime>("datetime2"),
				Renewed = table.Column<DateTime>("datetime2"),
				Expires = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true),
				Data = table.Column<string>("nvarchar(max)")
			};
		}, "dbo", table =>
		{
			table.PrimaryKey("PK_ServerSideSessions", x => x.Id);
		});
		migrationBuilder.CreateIndex("IX_PersistedGrants_Key", "PersistedGrants", "Key", "dbo", unique: true, "[Key] IS NOT NULL");
		migrationBuilder.CreateIndex("IX_ServerSideSessions_DisplayName", "ServerSideSessions", "DisplayName", "dbo");
		migrationBuilder.CreateIndex("IX_ServerSideSessions_Expires", "ServerSideSessions", "Expires", "dbo");
		migrationBuilder.CreateIndex("IX_ServerSideSessions_Key", "ServerSideSessions", "Key", "dbo", unique: true);
		migrationBuilder.CreateIndex("IX_ServerSideSessions_SessionId", "ServerSideSessions", "SessionId", "dbo");
		migrationBuilder.CreateIndex("IX_ServerSideSessions_SubjectId", "ServerSideSessions", "SubjectId", "dbo");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable("ServerSideSessions", "dbo");
		migrationBuilder.DropPrimaryKey("PK_PersistedGrants", "PersistedGrants", "dbo");
		migrationBuilder.DropIndex("IX_PersistedGrants_Key", "PersistedGrants", "dbo");
		migrationBuilder.DropColumn("Id", "PersistedGrants", "dbo");
		int? maxLength = 200;
		Type typeFromHandle = typeof(string);
		int? oldMaxLength = 200;
		migrationBuilder.AlterColumn<string>("Key", "PersistedGrants", "nvarchar(200)", null, maxLength, rowVersion: false, "dbo", nullable: false, "", null, null, typeFromHandle, "nvarchar(200)", null, oldMaxLength, oldRowVersion: false, oldNullable: true);
		migrationBuilder.AddPrimaryKey("PK_PersistedGrants", "PersistedGrants", "Key", "dbo");
	}

	protected override void BuildTargetModel(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("dbo").HasAnnotation("ProductVersion", "6.0.1").HasAnnotation("Relational:MaxIdentifierLength", 128);
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
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ServerSideSession", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int");
			b.Property<int>("Id").UseIdentityColumn(1L);
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
