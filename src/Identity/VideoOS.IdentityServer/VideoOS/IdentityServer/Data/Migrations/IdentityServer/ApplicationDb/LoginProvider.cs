using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace VideoOS.IdentityServer.Data.Migrations.IdentityServer.ApplicationDb;

[DbContext(typeof(ApplicationDbContext))]
[Migration("20211210081727_LoginProvider")]
public class LoginProvider : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable("LoginProviders", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id = table.Column<Guid>();
			int? maxLength = 100;
			OperationBuilder<AddColumnOperation> clientId = table.Column<string>(null, null, maxLength);
			maxLength = 450;
			OperationBuilder<AddColumnOperation> clientSecret = table.Column<string>(null, null, maxLength);
			maxLength = 450;
			OperationBuilder<AddColumnOperation> callBackPath = table.Column<string>(null, null, maxLength);
			maxLength = 450;
			OperationBuilder<AddColumnOperation> authority = table.Column<string>(null, null, maxLength);
			maxLength = 100;
			OperationBuilder<AddColumnOperation> displayName = table.Column<string>(null, null, maxLength);
			OperationBuilder<AddColumnOperation> scopes = table.Column<string>(null, null, null, rowVersion: false, null, nullable: true);
			maxLength = 100;
			return new
			{
				Id = id,
				ClientId = clientId,
				ClientSecret = clientSecret,
				CallBackPath = callBackPath,
				Authority = authority,
				DisplayName = displayName,
				Scopes = scopes,
				UserNameClaimType = table.Column<string>(null, null, maxLength),
				PromptForLogin = table.Column<bool>(),
				Enabled = table.Column<bool>()
			};
		}, "dbo", table =>
		{
			table.PrimaryKey("PK_LoginProviders", x => x.Id);
		});
		migrationBuilder.CreateIndex("IX_LoginProviders_Authority", "LoginProviders", "Authority", "dbo", unique: true);
		migrationBuilder.CreateIndex("IX_LoginProviders_DisplayName", "LoginProviders", "DisplayName", "dbo", unique: true);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable("LoginProviders", "dbo");
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
			b.Property<string>("ClientSecret").IsRequired().HasColumnType("nvarchar(450)")
				.HasMaxLength(450);
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
