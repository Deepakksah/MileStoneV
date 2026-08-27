using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace VideoOS.IdentityServer.Data.Migrations.IdentityServer.ConfigurationDb;

[DbContext(typeof(SeededConfigurationDbContext))]
[Migration("20191018052143_Update_3.0.1_IdentityServerConfigurationDbMigration")]
public class Update_301_IdentityServerConfigurationDbMigration : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		object defaultValue = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified);
		migrationBuilder.AddColumn<DateTime>("Created", "IdentityResources", null, null, null, rowVersion: false, null, nullable: false, defaultValue);
		migrationBuilder.AddColumn<bool>("NonEditable", "IdentityResources", null, null, null, rowVersion: false, null, nullable: false, false);
		migrationBuilder.AddColumn<DateTime>("Updated", "IdentityResources", null, null, null, rowVersion: false, null, nullable: true);
		int? maxLength = 4000;
		Type typeFromHandle = typeof(string);
		int? oldMaxLength = 2000;
		migrationBuilder.AlterColumn<string>("Value", "ClientSecrets", null, null, maxLength, rowVersion: false, null, nullable: false, null, null, null, typeFromHandle, null, null, oldMaxLength);
		migrationBuilder.Sql("\r\n                    UPDATE [ClientSecrets] \r\n                    SET Type = 'SharedSecret'\r\n                    WHERE Type \r\n                    IS NULL\r\n                ");
		oldMaxLength = 250;
		typeFromHandle = typeof(string);
		maxLength = 250;
		migrationBuilder.AlterColumn<string>("Type", "ClientSecrets", null, null, oldMaxLength, rowVersion: false, null, nullable: false, null, null, null, typeFromHandle, null, null, maxLength, oldRowVersion: false, oldNullable: true);
		defaultValue = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified);
		migrationBuilder.AddColumn<DateTime>("Created", "ClientSecrets", null, null, null, rowVersion: false, null, nullable: false, defaultValue);
		defaultValue = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified);
		migrationBuilder.AddColumn<DateTime>("Created", "Clients", null, null, null, rowVersion: false, null, nullable: false, defaultValue);
		migrationBuilder.AddColumn<int>("DeviceCodeLifetime", "Clients", null, null, null, rowVersion: false, null, nullable: false, 0);
		migrationBuilder.AddColumn<DateTime>("LastAccessed", "Clients", null, null, null, rowVersion: false, null, nullable: true);
		migrationBuilder.AddColumn<bool>("NonEditable", "Clients", null, null, null, rowVersion: false, null, nullable: false, false);
		migrationBuilder.AddColumn<DateTime>("Updated", "Clients", null, null, null, rowVersion: false, null, nullable: true);
		maxLength = 100;
		migrationBuilder.AddColumn<string>("UserCodeType", "Clients", null, null, maxLength, rowVersion: false, null, nullable: true);
		migrationBuilder.AddColumn<int>("UserSsoLifetime", "Clients", null, null, null, rowVersion: false, null, nullable: true);
		maxLength = 4000;
		typeFromHandle = typeof(string);
		oldMaxLength = 2000;
		migrationBuilder.AlterColumn<string>("Value", "ApiSecrets", null, null, maxLength, rowVersion: false, null, nullable: false, null, null, null, typeFromHandle, null, null, oldMaxLength, oldRowVersion: false, oldNullable: true);
		migrationBuilder.Sql("\r\n                    UPDATE [ApiSecrets] \r\n                    SET Type = 'SharedSecret'\r\n                    WHERE Type \r\n                    IS NULL\r\n                ");
		oldMaxLength = 250;
		typeFromHandle = typeof(string);
		maxLength = 250;
		migrationBuilder.AlterColumn<string>("Type", "ApiSecrets", null, null, oldMaxLength, rowVersion: false, null, nullable: false, null, null, null, typeFromHandle, null, null, maxLength, oldRowVersion: false, oldNullable: true);
		defaultValue = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified);
		migrationBuilder.AddColumn<DateTime>("Created", "ApiSecrets", null, null, null, rowVersion: false, null, nullable: false, defaultValue);
		defaultValue = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified);
		migrationBuilder.AddColumn<DateTime>("Created", "ApiResources", null, null, null, rowVersion: false, null, nullable: false, defaultValue);
		migrationBuilder.AddColumn<DateTime>("LastAccessed", "ApiResources", null, null, null, rowVersion: false, null, nullable: true);
		migrationBuilder.AddColumn<bool>("NonEditable", "ApiResources", null, null, null, rowVersion: false, null, nullable: false, false);
		migrationBuilder.AddColumn<DateTime>("Updated", "ApiResources", null, null, null, rowVersion: false, null, nullable: true);
		migrationBuilder.CreateTable("ApiProperties", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id = table.Column<int>().Annotation("SqlServer:Identity", "1, 1");
			int? maxLength2 = 250;
			OperationBuilder<AddColumnOperation> key = table.Column<string>(null, null, maxLength2);
			maxLength2 = 2000;
			return new
			{
				Id = id,
				Key = key,
				Value = table.Column<string>(null, null, maxLength2),
				ApiResourceId = table.Column<int>()
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_ApiProperties", x => x.Id);
			table.ForeignKey("FK_ApiProperties_ApiResources_ApiResourceId", x => x.ApiResourceId, "ApiResources", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("IdentityProperties", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id = table.Column<int>().Annotation("SqlServer:Identity", "1, 1");
			int? maxLength2 = 250;
			OperationBuilder<AddColumnOperation> key = table.Column<string>(null, null, maxLength2);
			maxLength2 = 2000;
			return new
			{
				Id = id,
				Key = key,
				Value = table.Column<string>(null, null, maxLength2),
				IdentityResourceId = table.Column<int>()
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_IdentityProperties", x => x.Id);
			table.ForeignKey("FK_IdentityProperties_IdentityResources_IdentityResourceId", x => x.IdentityResourceId, "IdentityResources", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateIndex("IX_ApiProperties_ApiResourceId", "ApiProperties", "ApiResourceId");
		migrationBuilder.CreateIndex("IX_IdentityProperties_IdentityResourceId", "IdentityProperties", "IdentityResourceId");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable("ApiProperties");
		migrationBuilder.DropTable("IdentityProperties");
		migrationBuilder.DropColumn("Created", "IdentityResources");
		migrationBuilder.DropColumn("NonEditable", "IdentityResources");
		migrationBuilder.DropColumn("Updated", "IdentityResources");
		migrationBuilder.DropColumn("Created", "ClientSecrets");
		migrationBuilder.DropColumn("Created", "Clients");
		migrationBuilder.DropColumn("DeviceCodeLifetime", "Clients");
		migrationBuilder.DropColumn("LastAccessed", "Clients");
		migrationBuilder.DropColumn("NonEditable", "Clients");
		migrationBuilder.DropColumn("Updated", "Clients");
		migrationBuilder.DropColumn("UserCodeType", "Clients");
		migrationBuilder.DropColumn("UserSsoLifetime", "Clients");
		migrationBuilder.DropColumn("Created", "ApiSecrets");
		migrationBuilder.DropColumn("Created", "ApiResources");
		migrationBuilder.DropColumn("LastAccessed", "ApiResources");
		migrationBuilder.DropColumn("NonEditable", "ApiResources");
		migrationBuilder.DropColumn("Updated", "ApiResources");
		int? maxLength = 2000;
		Type typeFromHandle = typeof(string);
		int? oldMaxLength = 4000;
		migrationBuilder.AlterColumn<string>("Value", "ClientSecrets", null, null, maxLength, rowVersion: false, null, nullable: false, null, null, null, typeFromHandle, null, null, oldMaxLength);
		oldMaxLength = 250;
		typeFromHandle = typeof(string);
		maxLength = 250;
		migrationBuilder.AlterColumn<string>("Type", "ClientSecrets", null, null, oldMaxLength, rowVersion: false, null, nullable: true, null, null, null, typeFromHandle, null, null, maxLength);
		maxLength = 2000;
		typeFromHandle = typeof(string);
		oldMaxLength = 4000;
		migrationBuilder.AlterColumn<string>("Value", "ApiSecrets", null, null, maxLength, rowVersion: false, null, nullable: true, null, null, null, typeFromHandle, null, null, oldMaxLength);
		oldMaxLength = 250;
		typeFromHandle = typeof(string);
		maxLength = 250;
		migrationBuilder.AlterColumn<string>("Type", "ApiSecrets", null, null, oldMaxLength, rowVersion: false, null, nullable: true, null, null, null, typeFromHandle, null, null, maxLength);
	}

	protected override void BuildTargetModel(ModelBuilder modelBuilder)
	{
		modelBuilder.HasAnnotation("ProductVersion", "3.0.0").HasAnnotation("Relational:MaxIdentifierLength", 128).HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ApiResource", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int")
				.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<DateTime>("Created").HasColumnType("datetime2");
			b.Property<string>("Description").HasColumnType("nvarchar(1000)").HasMaxLength(1000);
			b.Property<string>("DisplayName").HasColumnType("nvarchar(200)").HasMaxLength(200);
			b.Property<bool>("Enabled").HasColumnType("bit");
			b.Property<DateTime?>("LastAccessed").HasColumnType("datetime2");
			b.Property<string>("Name").IsRequired().HasColumnType("nvarchar(200)")
				.HasMaxLength(200);
			b.Property<bool>("NonEditable").HasColumnType("bit");
			b.Property<DateTime?>("Updated").HasColumnType("datetime2");
			b.HasKey("Id");
			b.HasIndex("Name").IsUnique();
			b.ToTable("ApiResources");
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ApiResourceClaim", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int")
				.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<int>("ApiResourceId").HasColumnType("int");
			b.Property<string>("Type").IsRequired().HasColumnType("nvarchar(200)")
				.HasMaxLength(200);
			b.HasKey("Id");
			b.HasIndex("ApiResourceId");
			b.ToTable("ApiClaims");
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ApiResourceProperty", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int")
				.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<int>("ApiResourceId").HasColumnType("int");
			b.Property<string>("Key").IsRequired().HasColumnType("nvarchar(250)")
				.HasMaxLength(250);
			b.Property<string>("Value").IsRequired().HasColumnType("nvarchar(2000)")
				.HasMaxLength(2000);
			b.HasKey("Id");
			b.HasIndex("ApiResourceId");
			b.ToTable("ApiProperties");
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ApiScope", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int")
				.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<int>("ApiResourceId").HasColumnType("int");
			b.Property<string>("Description").HasColumnType("nvarchar(1000)").HasMaxLength(1000);
			b.Property<string>("DisplayName").HasColumnType("nvarchar(200)").HasMaxLength(200);
			b.Property<bool>("Emphasize").HasColumnType("bit");
			b.Property<string>("Name").IsRequired().HasColumnType("nvarchar(200)")
				.HasMaxLength(200);
			b.Property<bool>("Required").HasColumnType("bit");
			b.Property<bool>("ShowInDiscoveryDocument").HasColumnType("bit");
			b.HasKey("Id");
			b.HasIndex("ApiResourceId");
			b.HasIndex("Name").IsUnique();
			b.ToTable("ApiScopes");
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ApiScopeClaim", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int")
				.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<int>("ApiScopeId").HasColumnType("int");
			b.Property<string>("Type").IsRequired().HasColumnType("nvarchar(200)")
				.HasMaxLength(200);
			b.HasKey("Id");
			b.HasIndex("ApiScopeId");
			b.ToTable("ApiScopeClaims");
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ApiSecret", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int")
				.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<int>("ApiResourceId").HasColumnType("int");
			b.Property<DateTime>("Created").HasColumnType("datetime2");
			b.Property<string>("Description").HasColumnType("nvarchar(1000)").HasMaxLength(1000);
			b.Property<DateTime?>("Expiration").HasColumnType("datetime2");
			b.Property<string>("Type").IsRequired().HasColumnType("nvarchar(250)")
				.HasMaxLength(250);
			b.Property<string>("Value").IsRequired().HasColumnType("nvarchar(4000)")
				.HasMaxLength(4000);
			b.HasKey("Id");
			b.HasIndex("ApiResourceId");
			b.ToTable("ApiSecrets");
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.Client", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int")
				.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<int>("AbsoluteRefreshTokenLifetime").HasColumnType("int");
			b.Property<int>("AccessTokenLifetime").HasColumnType("int");
			b.Property<int>("AccessTokenType").HasColumnType("int");
			b.Property<bool>("AllowAccessTokensViaBrowser").HasColumnType("bit");
			b.Property<bool>("AllowOfflineAccess").HasColumnType("bit");
			b.Property<bool>("AllowPlainTextPkce").HasColumnType("bit");
			b.Property<bool>("AllowRememberConsent").HasColumnType("bit");
			b.Property<bool>("AlwaysIncludeUserClaimsInIdToken").HasColumnType("bit");
			b.Property<bool>("AlwaysSendClientClaims").HasColumnType("bit");
			b.Property<int>("AuthorizationCodeLifetime").HasColumnType("int");
			b.Property<bool>("BackChannelLogoutSessionRequired").HasColumnType("bit");
			b.Property<string>("BackChannelLogoutUri").HasColumnType("nvarchar(2000)").HasMaxLength(2000);
			b.Property<string>("ClientClaimsPrefix").HasColumnType("nvarchar(200)").HasMaxLength(200);
			b.Property<string>("ClientId").IsRequired().HasColumnType("nvarchar(200)")
				.HasMaxLength(200);
			b.Property<string>("ClientName").HasColumnType("nvarchar(200)").HasMaxLength(200);
			b.Property<string>("ClientUri").HasColumnType("nvarchar(2000)").HasMaxLength(2000);
			b.Property<int?>("ConsentLifetime").HasColumnType("int");
			b.Property<DateTime>("Created").HasColumnType("datetime2");
			b.Property<string>("Description").HasColumnType("nvarchar(1000)").HasMaxLength(1000);
			b.Property<int>("DeviceCodeLifetime").HasColumnType("int");
			b.Property<bool>("EnableLocalLogin").HasColumnType("bit");
			b.Property<bool>("Enabled").HasColumnType("bit");
			b.Property<bool>("FrontChannelLogoutSessionRequired").HasColumnType("bit");
			b.Property<string>("FrontChannelLogoutUri").HasColumnType("nvarchar(2000)").HasMaxLength(2000);
			b.Property<int>("IdentityTokenLifetime").HasColumnType("int");
			b.Property<bool>("IncludeJwtId").HasColumnType("bit");
			b.Property<DateTime?>("LastAccessed").HasColumnType("datetime2");
			b.Property<string>("LogoUri").HasColumnType("nvarchar(2000)").HasMaxLength(2000);
			b.Property<bool>("NonEditable").HasColumnType("bit");
			b.Property<string>("PairWiseSubjectSalt").HasColumnType("nvarchar(200)").HasMaxLength(200);
			b.Property<string>("ProtocolType").IsRequired().HasColumnType("nvarchar(200)")
				.HasMaxLength(200);
			b.Property<int>("RefreshTokenExpiration").HasColumnType("int");
			b.Property<int>("RefreshTokenUsage").HasColumnType("int");
			b.Property<bool>("RequireClientSecret").HasColumnType("bit");
			b.Property<bool>("RequireConsent").HasColumnType("bit");
			b.Property<bool>("RequirePkce").HasColumnType("bit");
			b.Property<int>("SlidingRefreshTokenLifetime").HasColumnType("int");
			b.Property<bool>("UpdateAccessTokenClaimsOnRefresh").HasColumnType("bit");
			b.Property<DateTime?>("Updated").HasColumnType("datetime2");
			b.Property<string>("UserCodeType").HasColumnType("nvarchar(100)").HasMaxLength(100);
			b.Property<int?>("UserSsoLifetime").HasColumnType("int");
			b.HasKey("Id");
			b.HasIndex("ClientId").IsUnique();
			b.ToTable("Clients");
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientClaim", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int")
				.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<int>("ClientId").HasColumnType("int");
			b.Property<string>("Type").IsRequired().HasColumnType("nvarchar(250)")
				.HasMaxLength(250);
			b.Property<string>("Value").IsRequired().HasColumnType("nvarchar(250)")
				.HasMaxLength(250);
			b.HasKey("Id");
			b.HasIndex("ClientId");
			b.ToTable("ClientClaims");
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientCorsOrigin", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int")
				.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<int>("ClientId").HasColumnType("int");
			b.Property<string>("Origin").IsRequired().HasColumnType("nvarchar(150)")
				.HasMaxLength(150);
			b.HasKey("Id");
			b.HasIndex("ClientId");
			b.ToTable("ClientCorsOrigins");
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientGrantType", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int")
				.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<int>("ClientId").HasColumnType("int");
			b.Property<string>("GrantType").IsRequired().HasColumnType("nvarchar(250)")
				.HasMaxLength(250);
			b.HasKey("Id");
			b.HasIndex("ClientId");
			b.ToTable("ClientGrantTypes");
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientIdPRestriction", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int")
				.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<int>("ClientId").HasColumnType("int");
			b.Property<string>("Provider").IsRequired().HasColumnType("nvarchar(200)")
				.HasMaxLength(200);
			b.HasKey("Id");
			b.HasIndex("ClientId");
			b.ToTable("ClientIdPRestrictions");
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientPostLogoutRedirectUri", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int")
				.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<int>("ClientId").HasColumnType("int");
			b.Property<string>("PostLogoutRedirectUri").IsRequired().HasColumnType("nvarchar(2000)")
				.HasMaxLength(2000);
			b.HasKey("Id");
			b.HasIndex("ClientId");
			b.ToTable("ClientPostLogoutRedirectUris");
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientProperty", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int")
				.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<int>("ClientId").HasColumnType("int");
			b.Property<string>("Key").IsRequired().HasColumnType("nvarchar(250)")
				.HasMaxLength(250);
			b.Property<string>("Value").IsRequired().HasColumnType("nvarchar(2000)")
				.HasMaxLength(2000);
			b.HasKey("Id");
			b.HasIndex("ClientId");
			b.ToTable("ClientProperties");
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientRedirectUri", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int")
				.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<int>("ClientId").HasColumnType("int");
			b.Property<string>("RedirectUri").IsRequired().HasColumnType("nvarchar(2000)")
				.HasMaxLength(2000);
			b.HasKey("Id");
			b.HasIndex("ClientId");
			b.ToTable("ClientRedirectUris");
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientScope", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int")
				.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<int>("ClientId").HasColumnType("int");
			b.Property<string>("Scope").IsRequired().HasColumnType("nvarchar(200)")
				.HasMaxLength(200);
			b.HasKey("Id");
			b.HasIndex("ClientId");
			b.ToTable("ClientScopes");
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientSecret", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int")
				.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<int>("ClientId").HasColumnType("int");
			b.Property<DateTime>("Created").HasColumnType("datetime2");
			b.Property<string>("Description").HasColumnType("nvarchar(2000)").HasMaxLength(2000);
			b.Property<DateTime?>("Expiration").HasColumnType("datetime2");
			b.Property<string>("Type").IsRequired().HasColumnType("nvarchar(250)")
				.HasMaxLength(250);
			b.Property<string>("Value").IsRequired().HasColumnType("nvarchar(4000)")
				.HasMaxLength(4000);
			b.HasKey("Id");
			b.HasIndex("ClientId");
			b.ToTable("ClientSecrets");
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.IdentityClaim", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int")
				.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<int>("IdentityResourceId").HasColumnType("int");
			b.Property<string>("Type").IsRequired().HasColumnType("nvarchar(200)")
				.HasMaxLength(200);
			b.HasKey("Id");
			b.HasIndex("IdentityResourceId");
			b.ToTable("IdentityClaims");
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.IdentityResource", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int")
				.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<DateTime>("Created").HasColumnType("datetime2");
			b.Property<string>("Description").HasColumnType("nvarchar(1000)").HasMaxLength(1000);
			b.Property<string>("DisplayName").HasColumnType("nvarchar(200)").HasMaxLength(200);
			b.Property<bool>("Emphasize").HasColumnType("bit");
			b.Property<bool>("Enabled").HasColumnType("bit");
			b.Property<string>("Name").IsRequired().HasColumnType("nvarchar(200)")
				.HasMaxLength(200);
			b.Property<bool>("NonEditable").HasColumnType("bit");
			b.Property<bool>("Required").HasColumnType("bit");
			b.Property<bool>("ShowInDiscoveryDocument").HasColumnType("bit");
			b.Property<DateTime?>("Updated").HasColumnType("datetime2");
			b.HasKey("Id");
			b.HasIndex("Name").IsUnique();
			b.ToTable("IdentityResources");
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.IdentityResourceProperty", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int")
				.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<int>("IdentityResourceId").HasColumnType("int");
			b.Property<string>("Key").IsRequired().HasColumnType("nvarchar(250)")
				.HasMaxLength(250);
			b.Property<string>("Value").IsRequired().HasColumnType("nvarchar(2000)")
				.HasMaxLength(2000);
			b.HasKey("Id");
			b.HasIndex("IdentityResourceId");
			b.ToTable("IdentityProperties");
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ApiResourceClaim", delegate(EntityTypeBuilder b)
		{
			b.HasOne("IdentityServer4.EntityFramework.Entities.ApiResource", "ApiResource").WithMany("UserClaims").HasForeignKey("ApiResourceId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ApiResourceProperty", delegate(EntityTypeBuilder b)
		{
			b.HasOne("IdentityServer4.EntityFramework.Entities.ApiResource", "ApiResource").WithMany("Properties").HasForeignKey("ApiResourceId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ApiScope", delegate(EntityTypeBuilder b)
		{
			b.HasOne("IdentityServer4.EntityFramework.Entities.ApiResource", "ApiResource").WithMany("Scopes").HasForeignKey("ApiResourceId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ApiScopeClaim", delegate(EntityTypeBuilder b)
		{
			b.HasOne("IdentityServer4.EntityFramework.Entities.ApiScope", "ApiScope").WithMany("UserClaims").HasForeignKey("ApiScopeId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ApiSecret", delegate(EntityTypeBuilder b)
		{
			b.HasOne("IdentityServer4.EntityFramework.Entities.ApiResource", "ApiResource").WithMany("Secrets").HasForeignKey("ApiResourceId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientClaim", delegate(EntityTypeBuilder b)
		{
			b.HasOne("IdentityServer4.EntityFramework.Entities.Client", "Client").WithMany("Claims").HasForeignKey("ClientId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientCorsOrigin", delegate(EntityTypeBuilder b)
		{
			b.HasOne("IdentityServer4.EntityFramework.Entities.Client", "Client").WithMany("AllowedCorsOrigins").HasForeignKey("ClientId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientGrantType", delegate(EntityTypeBuilder b)
		{
			b.HasOne("IdentityServer4.EntityFramework.Entities.Client", "Client").WithMany("AllowedGrantTypes").HasForeignKey("ClientId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientIdPRestriction", delegate(EntityTypeBuilder b)
		{
			b.HasOne("IdentityServer4.EntityFramework.Entities.Client", "Client").WithMany("IdentityProviderRestrictions").HasForeignKey("ClientId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientPostLogoutRedirectUri", delegate(EntityTypeBuilder b)
		{
			b.HasOne("IdentityServer4.EntityFramework.Entities.Client", "Client").WithMany("PostLogoutRedirectUris").HasForeignKey("ClientId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientProperty", delegate(EntityTypeBuilder b)
		{
			b.HasOne("IdentityServer4.EntityFramework.Entities.Client", "Client").WithMany("Properties").HasForeignKey("ClientId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientRedirectUri", delegate(EntityTypeBuilder b)
		{
			b.HasOne("IdentityServer4.EntityFramework.Entities.Client", "Client").WithMany("RedirectUris").HasForeignKey("ClientId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientScope", delegate(EntityTypeBuilder b)
		{
			b.HasOne("IdentityServer4.EntityFramework.Entities.Client", "Client").WithMany("AllowedScopes").HasForeignKey("ClientId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientSecret", delegate(EntityTypeBuilder b)
		{
			b.HasOne("IdentityServer4.EntityFramework.Entities.Client", "Client").WithMany("ClientSecrets").HasForeignKey("ClientId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.IdentityClaim", delegate(EntityTypeBuilder b)
		{
			b.HasOne("IdentityServer4.EntityFramework.Entities.IdentityResource", "IdentityResource").WithMany("UserClaims").HasForeignKey("IdentityResourceId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.IdentityResourceProperty", delegate(EntityTypeBuilder b)
		{
			b.HasOne("IdentityServer4.EntityFramework.Entities.IdentityResource", "IdentityResource").WithMany("Properties").HasForeignKey("IdentityResourceId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
	}
}
