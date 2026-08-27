using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VideoOS.IdentityServer.Data.Migrations.IdentityServer.SeededConfigurationDb;

[DbContext(typeof(SeededConfigurationDbContext))]
[Migration("20211117122253_AddVmsClientId")]
public class AddVmsClientId : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.UpdateData("ApiResources", "Id", -9, "Created", new DateTime(2021, 11, 17, 12, 22, 52, 775, DateTimeKind.Utc).AddTicks(5710L));
		migrationBuilder.UpdateData("ApiResources", "Id", -8, "Created", new DateTime(2021, 11, 17, 12, 22, 52, 775, DateTimeKind.Utc).AddTicks(5703L));
		migrationBuilder.UpdateData("ApiResources", "Id", -7, "Created", new DateTime(2021, 11, 17, 12, 22, 52, 775, DateTimeKind.Utc).AddTicks(5696L));
		migrationBuilder.UpdateData("ApiResources", "Id", -6, "Created", new DateTime(2021, 11, 17, 12, 22, 52, 775, DateTimeKind.Utc).AddTicks(5689L));
		migrationBuilder.UpdateData("ApiResources", "Id", -5, "Created", new DateTime(2021, 11, 17, 12, 22, 52, 775, DateTimeKind.Utc).AddTicks(5682L));
		migrationBuilder.UpdateData("ApiResources", "Id", -4, "Created", new DateTime(2021, 11, 17, 12, 22, 52, 775, DateTimeKind.Utc).AddTicks(5674L));
		migrationBuilder.UpdateData("ApiResources", "Id", -3, "Created", new DateTime(2021, 11, 17, 12, 22, 52, 775, DateTimeKind.Utc).AddTicks(5667L));
		migrationBuilder.UpdateData("ApiResources", "Id", -2, "Created", new DateTime(2021, 11, 17, 12, 22, 52, 775, DateTimeKind.Utc).AddTicks(5650L));
		migrationBuilder.UpdateData("ApiResources", "Id", -1, "Created", new DateTime(2021, 11, 17, 12, 22, 52, 775, DateTimeKind.Utc).AddTicks(5212L));
		migrationBuilder.UpdateData("Clients", "Id", -3, "Created", new DateTime(2021, 11, 17, 12, 22, 52, 741, DateTimeKind.Utc).AddTicks(2006L));
		migrationBuilder.UpdateData("Clients", "Id", -2, "Created", new DateTime(2021, 11, 17, 12, 22, 52, 741, DateTimeKind.Utc).AddTicks(1791L));
		migrationBuilder.UpdateData("Clients", "Id", -1, "Created", new DateTime(2021, 11, 17, 12, 22, 52, 739, DateTimeKind.Utc).AddTicks(6553L));
		migrationBuilder.InsertData("Clients", new string[44]
		{
			"Id", "AbsoluteRefreshTokenLifetime", "AccessTokenLifetime", "AccessTokenType", "AllowAccessTokensViaBrowser", "AllowOfflineAccess", "AllowPlainTextPkce", "AllowRememberConsent", "AllowedIdentityTokenSigningAlgorithms", "AlwaysIncludeUserClaimsInIdToken",
			"AlwaysSendClientClaims", "AuthorizationCodeLifetime", "BackChannelLogoutSessionRequired", "BackChannelLogoutUri", "ClientClaimsPrefix", "ClientId", "ClientName", "ClientUri", "ConsentLifetime", "Created",
			"Description", "DeviceCodeLifetime", "EnableLocalLogin", "Enabled", "FrontChannelLogoutSessionRequired", "FrontChannelLogoutUri", "IdentityTokenLifetime", "IncludeJwtId", "LastAccessed", "LogoUri",
			"NonEditable", "PairWiseSubjectSalt", "ProtocolType", "RefreshTokenExpiration", "RefreshTokenUsage", "RequireClientSecret", "RequireConsent", "RequirePkce", "RequireRequestObject", "SlidingRefreshTokenLifetime",
			"UpdateAccessTokenClaimsOnRefresh", "Updated", "UserCodeType", "UserSsoLifetime"
		}, new object[44]
		{
			-4,
			0,
			3600,
			0,
			false,
			true,
			false,
			true,
			null,
			false,
			false,
			300,
			true,
			null,
			"client_",
			"VmsClient",
			"VmsClient",
			null,
			null,
			new DateTime(2021, 11, 17, 12, 22, 52, 741, DateTimeKind.Utc).AddTicks(2408L),
			null,
			300,
			true,
			true,
			true,
			null,
			300,
			true,
			null,
			null,
			false,
			null,
			"oidc",
			0,
			0,
			false,
			false,
			true,
			false,
			1296000,
			true,
			null,
			null,
			null
		});
		migrationBuilder.UpdateData("IdentityResources", "Id", -3, "Created", new DateTime(2021, 11, 17, 12, 22, 52, 799, DateTimeKind.Utc).AddTicks(4142L));
		migrationBuilder.UpdateData("IdentityResources", "Id", -2, "Created", new DateTime(2021, 11, 17, 12, 22, 52, 795, DateTimeKind.Utc).AddTicks(4890L));
		migrationBuilder.UpdateData("IdentityResources", "Id", -1, "Created", new DateTime(2021, 11, 17, 12, 22, 52, 791, DateTimeKind.Utc).AddTicks(4782L));
		migrationBuilder.UpdateData("ClientGrantTypes", "Id", -8, new string[2] { "ClientId", "GrantType" }, new object[2] { -4, "authorization_code" });
		migrationBuilder.InsertData("ClientGrantTypes", new string[3] { "Id", "ClientId", "GrantType" }, new object[3, 3]
		{
			{ -9, -4, "refresh_token" },
			{ -10, -4, "password" },
			{ -11, -4, "windows_credentials" }
		});
		migrationBuilder.InsertData("ClientRedirectUris", new string[3] { "Id", "ClientId", "RedirectUri" }, new object[3] { -2, -4, "http://127.0.0.1" });
		migrationBuilder.InsertData("ClientScopes", new string[3] { "Id", "ClientId", "Scope" }, new object[5, 3]
		{
			{ -8, -4, "managementserver" },
			{ -9, -4, "openid" },
			{ -10, -4, "profile" },
			{ -11, -4, "email" },
			{ -12, -4, "offline_access" }
		});
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DeleteData("ClientGrantTypes", "Id", -11);
		migrationBuilder.DeleteData("ClientGrantTypes", "Id", -10);
		migrationBuilder.DeleteData("ClientGrantTypes", "Id", -9);
		migrationBuilder.DeleteData("ClientGrantTypes", "Id", -8);
		migrationBuilder.DeleteData("ClientRedirectUris", "Id", -2);
		migrationBuilder.DeleteData("ClientScopes", "Id", -12);
		migrationBuilder.DeleteData("ClientScopes", "Id", -11);
		migrationBuilder.DeleteData("ClientScopes", "Id", -10);
		migrationBuilder.DeleteData("ClientScopes", "Id", -9);
		migrationBuilder.DeleteData("ClientScopes", "Id", -8);
		migrationBuilder.DeleteData("Clients", "Id", -4);
		migrationBuilder.UpdateData("ApiResources", "Id", -9, "Created", new DateTime(2021, 10, 26, 8, 57, 16, 326, DateTimeKind.Utc).AddTicks(8750L));
		migrationBuilder.UpdateData("ApiResources", "Id", -8, "Created", new DateTime(2021, 10, 26, 8, 57, 16, 326, DateTimeKind.Utc).AddTicks(8742L));
		migrationBuilder.UpdateData("ApiResources", "Id", -7, "Created", new DateTime(2021, 10, 26, 8, 57, 16, 326, DateTimeKind.Utc).AddTicks(8701L));
		migrationBuilder.UpdateData("ApiResources", "Id", -6, "Created", new DateTime(2021, 10, 26, 8, 57, 16, 326, DateTimeKind.Utc).AddTicks(8692L));
		migrationBuilder.UpdateData("ApiResources", "Id", -5, "Created", new DateTime(2021, 10, 26, 8, 57, 16, 326, DateTimeKind.Utc).AddTicks(8685L));
		migrationBuilder.UpdateData("ApiResources", "Id", -4, "Created", new DateTime(2021, 10, 26, 8, 57, 16, 326, DateTimeKind.Utc).AddTicks(8676L));
		migrationBuilder.UpdateData("ApiResources", "Id", -3, "Created", new DateTime(2021, 10, 26, 8, 57, 16, 326, DateTimeKind.Utc).AddTicks(8668L));
		migrationBuilder.UpdateData("ApiResources", "Id", -2, "Created", new DateTime(2021, 10, 26, 8, 57, 16, 326, DateTimeKind.Utc).AddTicks(8650L));
		migrationBuilder.UpdateData("ApiResources", "Id", -1, "Created", new DateTime(2021, 10, 26, 8, 57, 16, 326, DateTimeKind.Utc).AddTicks(8252L));
		migrationBuilder.InsertData("ClientGrantTypes", new string[3] { "Id", "ClientId", "GrantType" }, new object[3] { -8, -3, "client_credentials" });
		migrationBuilder.UpdateData("Clients", "Id", -3, "Created", new DateTime(2021, 10, 26, 8, 57, 16, 297, DateTimeKind.Utc).AddTicks(7344L));
		migrationBuilder.UpdateData("Clients", "Id", -2, "Created", new DateTime(2021, 10, 26, 8, 57, 16, 297, DateTimeKind.Utc).AddTicks(7128L));
		migrationBuilder.UpdateData("Clients", "Id", -1, "Created", new DateTime(2021, 10, 26, 8, 57, 16, 296, DateTimeKind.Utc).AddTicks(1379L));
		migrationBuilder.UpdateData("IdentityResources", "Id", -3, "Created", new DateTime(2021, 10, 26, 8, 57, 16, 351, DateTimeKind.Utc).AddTicks(3550L));
		migrationBuilder.UpdateData("IdentityResources", "Id", -2, "Created", new DateTime(2021, 10, 26, 8, 57, 16, 347, DateTimeKind.Utc).AddTicks(2579L));
		migrationBuilder.UpdateData("IdentityResources", "Id", -1, "Created", new DateTime(2021, 10, 26, 8, 57, 16, 343, DateTimeKind.Utc).AddTicks(2328L));
	}

	protected override void BuildTargetModel(ModelBuilder modelBuilder)
	{
		modelBuilder.HasAnnotation("ProductVersion", "3.1.5").HasAnnotation("Relational:MaxIdentifierLength", 128).HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiResource", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int")
				.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<string>("AllowedAccessTokenSigningAlgorithms").HasColumnType("nvarchar(100)").HasMaxLength(100);
			b.Property<DateTime>("Created").HasColumnType("datetime2");
			b.Property<string>("Description").HasColumnType("nvarchar(1000)").HasMaxLength(1000);
			b.Property<string>("DisplayName").HasColumnType("nvarchar(200)").HasMaxLength(200);
			b.Property<bool>("Enabled").HasColumnType("bit");
			b.Property<DateTime?>("LastAccessed").HasColumnType("datetime2");
			b.Property<string>("Name").IsRequired().HasColumnType("nvarchar(200)")
				.HasMaxLength(200);
			b.Property<bool>("NonEditable").HasColumnType("bit");
			b.Property<bool>("RequireResourceIndicator").HasColumnType("bit");
			b.Property<bool>("ShowInDiscoveryDocument").HasColumnType("bit");
			b.Property<DateTime?>("Updated").HasColumnType("datetime2");
			b.HasKey("Id");
			b.HasIndex("Name").IsUnique();
			b.ToTable("ApiResources");
			b.HasData(new
			{
				Id = -1,
				Created = new DateTime(2021, 11, 17, 12, 22, 52, 775, DateTimeKind.Utc).AddTicks(5212L),
				Description = "Api for client registration",
				DisplayName = "Client registration",
				Enabled = true,
				Name = "registration",
				NonEditable = false,
				RequireResourceIndicator = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -2,
				Created = new DateTime(2021, 11, 17, 12, 22, 52, 775, DateTimeKind.Utc).AddTicks(5650L),
				Description = "Services for management server",
				DisplayName = "Management server",
				Enabled = true,
				Name = "management",
				NonEditable = false,
				RequireResourceIndicator = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -3,
				Created = new DateTime(2021, 11, 17, 12, 22, 52, 775, DateTimeKind.Utc).AddTicks(5667L),
				Description = "Services for recorder server",
				DisplayName = "Recorder server",
				Enabled = true,
				Name = "recorder",
				NonEditable = false,
				RequireResourceIndicator = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -4,
				Created = new DateTime(2021, 11, 17, 12, 22, 52, 775, DateTimeKind.Utc).AddTicks(5674L),
				Description = "Management of IDP",
				DisplayName = "IDP Management",
				Enabled = true,
				Name = "idp_management",
				NonEditable = false,
				RequireResourceIndicator = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -5,
				Created = new DateTime(2021, 11, 17, 12, 22, 52, 775, DateTimeKind.Utc).AddTicks(5682L),
				Description = "Api for datacollector",
				DisplayName = "Datacollector",
				Enabled = true,
				Name = "datacollector",
				NonEditable = false,
				RequireResourceIndicator = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -6,
				Created = new DateTime(2021, 11, 17, 12, 22, 52, 775, DateTimeKind.Utc).AddTicks(5689L),
				Description = "Api for management server",
				DisplayName = "Management Server",
				Enabled = true,
				Name = "managementserver",
				NonEditable = false,
				RequireResourceIndicator = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -7,
				Created = new DateTime(2021, 11, 17, 12, 22, 52, 775, DateTimeKind.Utc).AddTicks(5696L),
				Description = "Api for failover response server",
				DisplayName = "Failover Response Server",
				Enabled = true,
				Name = "failoverresponseserver",
				NonEditable = false,
				RequireResourceIndicator = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -8,
				Created = new DateTime(2021, 11, 17, 12, 22, 52, 775, DateTimeKind.Utc).AddTicks(5703L),
				Description = "Api for mediadb",
				DisplayName = "MediaDb",
				Enabled = true,
				Name = "mediadb",
				NonEditable = false,
				RequireResourceIndicator = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -9,
				Created = new DateTime(2021, 11, 17, 12, 22, 52, 775, DateTimeKind.Utc).AddTicks(5710L),
				Description = "Api for IDP logs",
				DisplayName = "IDP Logs",
				Enabled = true,
				Name = "idp_log_event",
				NonEditable = false,
				RequireResourceIndicator = false,
				ShowInDiscoveryDocument = true
			});
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiResourceClaim", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int")
				.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<int>("ApiResourceId").HasColumnType("int");
			b.Property<string>("Type").IsRequired().HasColumnType("nvarchar(200)")
				.HasMaxLength(200);
			b.HasKey("Id");
			b.HasIndex("ApiResourceId");
			b.ToTable("ApiResourceClaims");
			b.HasData(new
			{
				Id = -1,
				ApiResourceId = -1,
				Type = "name"
			}, new
			{
				Id = -2,
				ApiResourceId = -4,
				Type = "name"
			}, new
			{
				Id = -3,
				ApiResourceId = -6,
				Type = "name"
			});
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiResourceProperty", delegate(EntityTypeBuilder b)
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
			b.ToTable("ApiResourceProperties");
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiResourceScope", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int")
				.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<int>("ApiResourceId").HasColumnType("int");
			b.Property<string>("Scope").IsRequired().HasColumnType("nvarchar(200)")
				.HasMaxLength(200);
			b.HasKey("Id");
			b.HasIndex("ApiResourceId");
			b.ToTable("ApiResourceScopes");
			b.HasData(new
			{
				Id = -1,
				ApiResourceId = -1,
				Scope = "write:client"
			}, new
			{
				Id = -2,
				ApiResourceId = -2,
				Scope = "management"
			}, new
			{
				Id = -3,
				ApiResourceId = -3,
				Scope = "recorder"
			}, new
			{
				Id = -4,
				ApiResourceId = -4,
				Scope = "idp_management"
			}, new
			{
				Id = -5,
				ApiResourceId = -5,
				Scope = "datacollector"
			}, new
			{
				Id = -6,
				ApiResourceId = -6,
				Scope = "managementserver"
			}, new
			{
				Id = -7,
				ApiResourceId = -7,
				Scope = "failoverresponseserver"
			}, new
			{
				Id = -8,
				ApiResourceId = -8,
				Scope = "mediadb"
			}, new
			{
				Id = -9,
				ApiResourceId = -9,
				Scope = "idp_log_event"
			});
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiResourceSecret", delegate(EntityTypeBuilder b)
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
			b.ToTable("ApiResourceSecrets");
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiScope", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int")
				.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<string>("Description").HasColumnType("nvarchar(1000)").HasMaxLength(1000);
			b.Property<string>("DisplayName").HasColumnType("nvarchar(200)").HasMaxLength(200);
			b.Property<bool>("Emphasize").HasColumnType("bit");
			b.Property<bool>("Enabled").HasColumnType("bit");
			b.Property<string>("Name").IsRequired().HasColumnType("nvarchar(200)")
				.HasMaxLength(200);
			b.Property<bool>("Required").HasColumnType("bit");
			b.Property<bool>("ShowInDiscoveryDocument").HasColumnType("bit");
			b.HasKey("Id");
			b.HasIndex("Name").IsUnique();
			b.ToTable("ApiScopes");
			b.HasData(new
			{
				Id = -1,
				DisplayName = "Client registration scope",
				Emphasize = false,
				Enabled = true,
				Name = "write:client",
				Required = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -2,
				DisplayName = "Management scope",
				Emphasize = false,
				Enabled = true,
				Name = "management",
				Required = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -3,
				DisplayName = "Recorder scope",
				Emphasize = false,
				Enabled = true,
				Name = "recorder",
				Required = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -4,
				DisplayName = "IDP Management Scope",
				Emphasize = false,
				Enabled = true,
				Name = "idp_management",
				Required = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -5,
				DisplayName = "Datacollector Scope",
				Emphasize = false,
				Enabled = true,
				Name = "datacollector",
				Required = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -6,
				DisplayName = "Management server Scope",
				Emphasize = false,
				Enabled = true,
				Name = "managementserver",
				Required = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -7,
				DisplayName = "Failover response server Scope",
				Emphasize = false,
				Enabled = true,
				Name = "failoverresponseserver",
				Required = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -8,
				DisplayName = "Media Db Scope",
				Emphasize = false,
				Enabled = true,
				Name = "mediadb",
				Required = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -9,
				DisplayName = "Log Event Scope",
				Emphasize = false,
				Enabled = true,
				Name = "idp_log_event",
				Required = false,
				ShowInDiscoveryDocument = true
			});
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiScopeClaim", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int")
				.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<int>("ScopeId").HasColumnType("int");
			b.Property<string>("Type").IsRequired().HasColumnType("nvarchar(200)")
				.HasMaxLength(200);
			b.HasKey("Id");
			b.HasIndex("ScopeId");
			b.ToTable("ApiScopeClaims");
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiScopeProperty", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int")
				.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<string>("Key").IsRequired().HasColumnType("nvarchar(250)")
				.HasMaxLength(250);
			b.Property<int>("ScopeId").HasColumnType("int");
			b.Property<string>("Value").IsRequired().HasColumnType("nvarchar(2000)")
				.HasMaxLength(2000);
			b.HasKey("Id");
			b.HasIndex("ScopeId");
			b.ToTable("ApiScopeProperties");
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.Client", delegate(EntityTypeBuilder b)
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
			b.Property<string>("AllowedIdentityTokenSigningAlgorithms").HasColumnType("nvarchar(100)").HasMaxLength(100);
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
			b.Property<bool>("RequireRequestObject").HasColumnType("bit");
			b.Property<int>("SlidingRefreshTokenLifetime").HasColumnType("int");
			b.Property<bool>("UpdateAccessTokenClaimsOnRefresh").HasColumnType("bit");
			b.Property<DateTime?>("Updated").HasColumnType("datetime2");
			b.Property<string>("UserCodeType").HasColumnType("nvarchar(100)").HasMaxLength(100);
			b.Property<int?>("UserSsoLifetime").HasColumnType("int");
			b.HasKey("Id");
			b.HasIndex("ClientId").IsUnique();
			b.ToTable("Clients");
			b.HasData(new
			{
				Id = -1,
				AbsoluteRefreshTokenLifetime = 2592000,
				AccessTokenLifetime = 3600,
				AccessTokenType = 0,
				AllowAccessTokensViaBrowser = false,
				AllowOfflineAccess = false,
				AllowPlainTextPkce = false,
				AllowRememberConsent = true,
				AlwaysIncludeUserClaimsInIdToken = false,
				AlwaysSendClientClaims = false,
				AuthorizationCodeLifetime = 300,
				BackChannelLogoutSessionRequired = true,
				ClientClaimsPrefix = "client_",
				ClientId = "winauthclient",
				ClientName = "Registration client",
				Created = new DateTime(2021, 11, 17, 12, 22, 52, 739, DateTimeKind.Utc).AddTicks(6553L),
				DeviceCodeLifetime = 300,
				EnableLocalLogin = true,
				Enabled = true,
				FrontChannelLogoutSessionRequired = true,
				IdentityTokenLifetime = 300,
				IncludeJwtId = true,
				NonEditable = false,
				ProtocolType = "oidc",
				RefreshTokenExpiration = 1,
				RefreshTokenUsage = 1,
				RequireClientSecret = false,
				RequireConsent = false,
				RequirePkce = true,
				RequireRequestObject = false,
				SlidingRefreshTokenLifetime = 1296000,
				UpdateAccessTokenClaimsOnRefresh = false
			}, new
			{
				Id = -2,
				AbsoluteRefreshTokenLifetime = 2592000,
				AccessTokenLifetime = 3600,
				AccessTokenType = 0,
				AllowAccessTokensViaBrowser = false,
				AllowOfflineAccess = false,
				AllowPlainTextPkce = false,
				AllowRememberConsent = true,
				AlwaysIncludeUserClaimsInIdToken = false,
				AlwaysSendClientClaims = false,
				AuthorizationCodeLifetime = 300,
				BackChannelLogoutSessionRequired = true,
				ClientClaimsPrefix = "client_",
				ClientId = "GrantValidatorClient",
				ClientName = "GrantValidatorClient",
				Created = new DateTime(2021, 11, 17, 12, 22, 52, 741, DateTimeKind.Utc).AddTicks(1791L),
				DeviceCodeLifetime = 300,
				EnableLocalLogin = true,
				Enabled = true,
				FrontChannelLogoutSessionRequired = true,
				IdentityTokenLifetime = 300,
				IncludeJwtId = true,
				NonEditable = false,
				ProtocolType = "oidc",
				RefreshTokenExpiration = 1,
				RefreshTokenUsage = 1,
				RequireClientSecret = false,
				RequireConsent = false,
				RequirePkce = true,
				RequireRequestObject = false,
				SlidingRefreshTokenLifetime = 1296000,
				UpdateAccessTokenClaimsOnRefresh = false
			}, new
			{
				Id = -3,
				AbsoluteRefreshTokenLifetime = 2592000,
				AccessTokenLifetime = 3600,
				AccessTokenType = 0,
				AllowAccessTokensViaBrowser = false,
				AllowOfflineAccess = true,
				AllowPlainTextPkce = false,
				AllowRememberConsent = true,
				AlwaysIncludeUserClaimsInIdToken = false,
				AlwaysSendClientClaims = false,
				AuthorizationCodeLifetime = 300,
				BackChannelLogoutSessionRequired = true,
				ClientClaimsPrefix = "client_",
				ClientId = "ManagementClient",
				ClientName = "ManagementClient",
				Created = new DateTime(2021, 11, 17, 12, 22, 52, 741, DateTimeKind.Utc).AddTicks(2006L),
				DeviceCodeLifetime = 300,
				EnableLocalLogin = true,
				Enabled = true,
				FrontChannelLogoutSessionRequired = true,
				IdentityTokenLifetime = 300,
				IncludeJwtId = true,
				NonEditable = false,
				ProtocolType = "oidc",
				RefreshTokenExpiration = 0,
				RefreshTokenUsage = 1,
				RequireClientSecret = false,
				RequireConsent = false,
				RequirePkce = true,
				RequireRequestObject = false,
				SlidingRefreshTokenLifetime = 1296000,
				UpdateAccessTokenClaimsOnRefresh = true
			}, new
			{
				Id = -4,
				AbsoluteRefreshTokenLifetime = 0,
				AccessTokenLifetime = 3600,
				AccessTokenType = 0,
				AllowAccessTokensViaBrowser = false,
				AllowOfflineAccess = true,
				AllowPlainTextPkce = false,
				AllowRememberConsent = true,
				AlwaysIncludeUserClaimsInIdToken = false,
				AlwaysSendClientClaims = false,
				AuthorizationCodeLifetime = 300,
				BackChannelLogoutSessionRequired = true,
				ClientClaimsPrefix = "client_",
				ClientId = "VmsClient",
				ClientName = "VmsClient",
				Created = new DateTime(2021, 11, 17, 12, 22, 52, 741, DateTimeKind.Utc).AddTicks(2408L),
				DeviceCodeLifetime = 300,
				EnableLocalLogin = true,
				Enabled = true,
				FrontChannelLogoutSessionRequired = true,
				IdentityTokenLifetime = 300,
				IncludeJwtId = true,
				NonEditable = false,
				ProtocolType = "oidc",
				RefreshTokenExpiration = 0,
				RefreshTokenUsage = 0,
				RequireClientSecret = false,
				RequireConsent = false,
				RequirePkce = true,
				RequireRequestObject = false,
				SlidingRefreshTokenLifetime = 1296000,
				UpdateAccessTokenClaimsOnRefresh = true
			});
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientClaim", delegate(EntityTypeBuilder b)
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
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientCorsOrigin", delegate(EntityTypeBuilder b)
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
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientGrantType", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int")
				.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<int>("ClientId").HasColumnType("int");
			b.Property<string>("GrantType").IsRequired().HasColumnType("nvarchar(250)")
				.HasMaxLength(250);
			b.HasKey("Id");
			b.HasIndex("ClientId");
			b.ToTable("ClientGrantTypes");
			b.HasData(new
			{
				Id = -1,
				ClientId = -1,
				GrantType = "windows_auth"
			}, new
			{
				Id = -2,
				ClientId = -2,
				GrantType = "windows_credentials"
			}, new
			{
				Id = -3,
				ClientId = -2,
				GrantType = "password"
			}, new
			{
				Id = -4,
				ClientId = -3,
				GrantType = "authorization_code"
			}, new
			{
				Id = -5,
				ClientId = -3,
				GrantType = "refresh_token"
			}, new
			{
				Id = -6,
				ClientId = -3,
				GrantType = "password"
			}, new
			{
				Id = -7,
				ClientId = -3,
				GrantType = "windows_credentials"
			}, new
			{
				Id = -8,
				ClientId = -4,
				GrantType = "authorization_code"
			}, new
			{
				Id = -9,
				ClientId = -4,
				GrantType = "refresh_token"
			}, new
			{
				Id = -10,
				ClientId = -4,
				GrantType = "password"
			}, new
			{
				Id = -11,
				ClientId = -4,
				GrantType = "windows_credentials"
			});
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientIdPRestriction", delegate(EntityTypeBuilder b)
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
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientPostLogoutRedirectUri", delegate(EntityTypeBuilder b)
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
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientProperty", delegate(EntityTypeBuilder b)
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
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientRedirectUri", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int")
				.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<int>("ClientId").HasColumnType("int");
			b.Property<string>("RedirectUri").IsRequired().HasColumnType("nvarchar(2000)")
				.HasMaxLength(2000);
			b.HasKey("Id");
			b.HasIndex("ClientId");
			b.ToTable("ClientRedirectUris");
			b.HasData(new
			{
				Id = -1,
				ClientId = -3,
				RedirectUri = "http://127.0.0.1"
			}, new
			{
				Id = -2,
				ClientId = -4,
				RedirectUri = "http://127.0.0.1"
			});
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientScope", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int")
				.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<int>("ClientId").HasColumnType("int");
			b.Property<string>("Scope").IsRequired().HasColumnType("nvarchar(200)")
				.HasMaxLength(200);
			b.HasKey("Id");
			b.HasIndex("ClientId");
			b.ToTable("ClientScopes");
			b.HasData(new
			{
				Id = -1,
				ClientId = -1,
				Scope = "write:client"
			}, new
			{
				Id = -2,
				ClientId = -2,
				Scope = "managementserver"
			}, new
			{
				Id = -3,
				ClientId = -3,
				Scope = "managementserver"
			}, new
			{
				Id = -4,
				ClientId = -3,
				Scope = "openid"
			}, new
			{
				Id = -5,
				ClientId = -3,
				Scope = "profile"
			}, new
			{
				Id = -6,
				ClientId = -3,
				Scope = "email"
			}, new
			{
				Id = -7,
				ClientId = -3,
				Scope = "offline_access"
			}, new
			{
				Id = -8,
				ClientId = -4,
				Scope = "managementserver"
			}, new
			{
				Id = -9,
				ClientId = -4,
				Scope = "openid"
			}, new
			{
				Id = -10,
				ClientId = -4,
				Scope = "profile"
			}, new
			{
				Id = -11,
				ClientId = -4,
				Scope = "email"
			}, new
			{
				Id = -12,
				ClientId = -4,
				Scope = "offline_access"
			});
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientSecret", delegate(EntityTypeBuilder b)
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
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.IdentityProvider", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int")
				.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<string>("DisplayName").HasColumnType("nvarchar(200)").HasMaxLength(200);
			b.Property<bool>("Enabled").HasColumnType("bit");
			b.Property<string>("Properties").HasColumnType("nvarchar(max)");
			b.Property<string>("Scheme").IsRequired().HasColumnType("nvarchar(200)")
				.HasMaxLength(200);
			b.Property<string>("Type").IsRequired().HasColumnType("nvarchar(20)")
				.HasMaxLength(20);
			b.HasKey("Id");
			b.ToTable("IdentityProviders");
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.IdentityResource", delegate(EntityTypeBuilder b)
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
			b.HasData(new
			{
				Id = -1,
				Created = new DateTime(2021, 11, 17, 12, 22, 52, 791, DateTimeKind.Utc).AddTicks(4782L),
				DisplayName = "Your user identifier",
				Emphasize = false,
				Enabled = true,
				Name = "openid",
				NonEditable = false,
				Required = true,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -2,
				Created = new DateTime(2021, 11, 17, 12, 22, 52, 795, DateTimeKind.Utc).AddTicks(4890L),
				Description = "Your user profile information (first name, last name, etc.)",
				DisplayName = "User profile",
				Emphasize = true,
				Enabled = true,
				Name = "profile",
				NonEditable = false,
				Required = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -3,
				Created = new DateTime(2021, 11, 17, 12, 22, 52, 799, DateTimeKind.Utc).AddTicks(4142L),
				DisplayName = "Your email address",
				Emphasize = true,
				Enabled = true,
				Name = "email",
				NonEditable = false,
				Required = false,
				ShowInDiscoveryDocument = true
			});
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.IdentityResourceClaim", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int")
				.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<int>("IdentityResourceId").HasColumnType("int");
			b.Property<string>("Type").IsRequired().HasColumnType("nvarchar(200)")
				.HasMaxLength(200);
			b.HasKey("Id");
			b.HasIndex("IdentityResourceId");
			b.ToTable("IdentityResourceClaims");
			b.HasData(new
			{
				Id = -1,
				IdentityResourceId = -1,
				Type = "sub"
			}, new
			{
				Id = -2,
				IdentityResourceId = -2,
				Type = "name"
			}, new
			{
				Id = -3,
				IdentityResourceId = -2,
				Type = "family_name"
			}, new
			{
				Id = -4,
				IdentityResourceId = -2,
				Type = "given_name"
			}, new
			{
				Id = -5,
				IdentityResourceId = -2,
				Type = "middle_name"
			}, new
			{
				Id = -6,
				IdentityResourceId = -2,
				Type = "nickname"
			}, new
			{
				Id = -7,
				IdentityResourceId = -2,
				Type = "preferred_username"
			}, new
			{
				Id = -8,
				IdentityResourceId = -2,
				Type = "profile"
			}, new
			{
				Id = -9,
				IdentityResourceId = -2,
				Type = "picture"
			}, new
			{
				Id = -10,
				IdentityResourceId = -2,
				Type = "website"
			}, new
			{
				Id = -11,
				IdentityResourceId = -2,
				Type = "gender"
			}, new
			{
				Id = -12,
				IdentityResourceId = -2,
				Type = "birthdate"
			}, new
			{
				Id = -13,
				IdentityResourceId = -2,
				Type = "zoneinfo"
			}, new
			{
				Id = -14,
				IdentityResourceId = -2,
				Type = "locale"
			}, new
			{
				Id = -15,
				IdentityResourceId = -2,
				Type = "updated_at"
			}, new
			{
				Id = -16,
				IdentityResourceId = -3,
				Type = "email"
			}, new
			{
				Id = -17,
				IdentityResourceId = -3,
				Type = "email_verified"
			});
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.IdentityResourceProperty", delegate(EntityTypeBuilder b)
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
			b.ToTable("IdentityResourceProperties");
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiResourceClaim", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Duende.IdentityServer.EntityFramework.Entities.ApiResource", "ApiResource").WithMany("UserClaims").HasForeignKey("ApiResourceId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiResourceProperty", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Duende.IdentityServer.EntityFramework.Entities.ApiResource", "ApiResource").WithMany("Properties").HasForeignKey("ApiResourceId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiResourceScope", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Duende.IdentityServer.EntityFramework.Entities.ApiResource", "ApiResource").WithMany("Scopes").HasForeignKey("ApiResourceId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiResourceSecret", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Duende.IdentityServer.EntityFramework.Entities.ApiResource", "ApiResource").WithMany("Secrets").HasForeignKey("ApiResourceId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiScopeClaim", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Duende.IdentityServer.EntityFramework.Entities.ApiScope", "Scope").WithMany("UserClaims").HasForeignKey("ScopeId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiScopeProperty", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Duende.IdentityServer.EntityFramework.Entities.ApiScope", "Scope").WithMany("Properties").HasForeignKey("ScopeId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientClaim", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Duende.IdentityServer.EntityFramework.Entities.Client", "Client").WithMany("Claims").HasForeignKey("ClientId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientCorsOrigin", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Duende.IdentityServer.EntityFramework.Entities.Client", "Client").WithMany("AllowedCorsOrigins").HasForeignKey("ClientId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientGrantType", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Duende.IdentityServer.EntityFramework.Entities.Client", "Client").WithMany("AllowedGrantTypes").HasForeignKey("ClientId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientIdPRestriction", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Duende.IdentityServer.EntityFramework.Entities.Client", "Client").WithMany("IdentityProviderRestrictions").HasForeignKey("ClientId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientPostLogoutRedirectUri", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Duende.IdentityServer.EntityFramework.Entities.Client", "Client").WithMany("PostLogoutRedirectUris").HasForeignKey("ClientId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientProperty", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Duende.IdentityServer.EntityFramework.Entities.Client", "Client").WithMany("Properties").HasForeignKey("ClientId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientRedirectUri", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Duende.IdentityServer.EntityFramework.Entities.Client", "Client").WithMany("RedirectUris").HasForeignKey("ClientId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientScope", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Duende.IdentityServer.EntityFramework.Entities.Client", "Client").WithMany("AllowedScopes").HasForeignKey("ClientId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientSecret", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Duende.IdentityServer.EntityFramework.Entities.Client", "Client").WithMany("ClientSecrets").HasForeignKey("ClientId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.IdentityResourceClaim", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Duende.IdentityServer.EntityFramework.Entities.IdentityResource", "IdentityResource").WithMany("UserClaims").HasForeignKey("IdentityResourceId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.IdentityResourceProperty", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Duende.IdentityServer.EntityFramework.Entities.IdentityResource", "IdentityResource").WithMany("Properties").HasForeignKey("IdentityResourceId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
	}
}
