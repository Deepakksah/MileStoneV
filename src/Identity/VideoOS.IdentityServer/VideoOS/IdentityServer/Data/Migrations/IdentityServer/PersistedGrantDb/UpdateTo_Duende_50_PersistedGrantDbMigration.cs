using System;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace VideoOS.IdentityServer.Data.Migrations.IdentityServer.PersistedGrantDb;

[DbContext(typeof(PersistedGrantDbContext))]
[Migration("20210406111853_UpdateTo_Duende_5.0_PersistedGrantDbMigration")]
public class UpdateTo_Duende_50_PersistedGrantDbMigration : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable("Keys", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id = table.Column<string>();
			OperationBuilder<AddColumnOperation> version = table.Column<int>();
			OperationBuilder<AddColumnOperation> created = table.Column<DateTime>();
			OperationBuilder<AddColumnOperation> use = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true);
			int? maxLength = 100;
			return new
			{
				Id = id,
				Version = version,
				Created = created,
				Use = use,
				Algorithm = table.Column<string>(null, null, maxLength),
				IsX509Certificate = table.Column<bool>(),
				DataProtected = table.Column<bool>(),
				Data = table.Column<string>()
			};
		}, "dbo", table =>
		{
			table.PrimaryKey("PK_Keys", x => x.Id);
		});
		migrationBuilder.CreateIndex("IX_Keys_Use", "Keys", "Use", "dbo");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable("Keys", "dbo");
	}

	protected override void BuildTargetModel(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("dbo").HasAnnotation("ProductVersion", "3.1.5").HasAnnotation("Relational:MaxIdentifierLength", 128)
			.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.DeviceFlowCodes", delegate(EntityTypeBuilder b)
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
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.Key", delegate(EntityTypeBuilder b)
		{
			b.Property<string>("Id").HasColumnType("nvarchar(450)");
			b.Property<string>("Algorithm").IsRequired().HasColumnType("nvarchar(100)")
				.HasMaxLength(100);
			b.Property<DateTime>("Created").HasColumnType("datetime2");
			b.Property<string>("Data").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<bool>("DataProtected").HasColumnType("bit");
			b.Property<bool>("IsX509Certificate").HasColumnType("bit");
			b.Property<string>("Use").HasColumnType("nvarchar(450)");
			b.Property<int>("Version").HasColumnType("int");
			b.HasKey("Id");
			b.HasIndex("Use");
			b.ToTable("Keys");
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.PersistedGrant", delegate(EntityTypeBuilder b)
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
