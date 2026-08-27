using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VideoOS.IdentityServer.Data.Migrations.IdentityServer.SeededConfigurationDb;

[DbContext(typeof(SeededConfigurationDbContext))]
[Migration("20250311134255_AddClientIdAndNameToEvetLogs")]
public class AddClientIdAndNameToEvetLogs : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.Sql("\r\n                    UPDATE [dbo].[EventLogs] \r\n                    SET [dbo].[EventLogs].ClientName = client.ClientName,  [dbo].[EventLogs].ClientId = client.ClientId, [dbo].[EventLogs].UserName = ''\r\n                    FROM [dbo].[Clients] as client\r\n\t                    INNER JOIN\r\n\t\t                    (SELECT Id, LTRIM(RTRIM(RIGHT(UserName, LEN(UserName)-CHARINDEX(':', UserName)))) as ClientId \r\n\t\t                    FROM [dbo].[EventLogs] \r\n\t\t                    WHERE UserName LIKE 'client_id:%') as log\r\n\t                    ON (client.ClientId = log.clientId)\r\n                    WHERE [dbo].[EventLogs].Id = log.Id\r\n            ");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
	}

	protected override void BuildTargetModel(ModelBuilder modelBuilder)
	{
		modelBuilder.HasAnnotation("ProductVersion", "8.0.4").HasAnnotation("Relational:MaxIdentifierLength", 128);
		modelBuilder.UseIdentityColumns(1L);
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiResource", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int");
			b.Property<int>("Id").UseIdentityColumn(1L);
			b.Property<string>("AllowedAccessTokenSigningAlgorithms").HasMaxLength(100).HasColumnType("nvarchar(100)");
			b.Property<DateTime>("Created").HasColumnType("datetime2");
			b.Property<string>("Description").HasMaxLength(1000).HasColumnType("nvarchar(1000)");
			b.Property<string>("DisplayName").HasMaxLength(200).HasColumnType("nvarchar(200)");
			b.Property<bool>("Enabled").HasColumnType("bit");
			b.Property<DateTime?>("LastAccessed").HasColumnType("datetime2");
			b.Property<string>("Name").IsRequired().HasMaxLength(200)
				.HasColumnType("nvarchar(200)");
			b.Property<bool>("NonEditable").HasColumnType("bit");
			b.Property<bool>("RequireResourceIndicator").HasColumnType("bit");
			b.Property<bool>("ShowInDiscoveryDocument").HasColumnType("bit");
			b.Property<DateTime?>("Updated").HasColumnType("datetime2");
			b.HasKey("Id");
			b.HasIndex("Name").IsUnique();
			b.ToTable("ApiResources", (string?)null);
			b.HasData(new
			{
				Id = -1,
				Created = new DateTime(2022, 11, 1, 23, 0, 0, 0, DateTimeKind.Utc),
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
				Created = new DateTime(2022, 11, 1, 23, 0, 0, 0, DateTimeKind.Utc),
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
				Created = new DateTime(2022, 11, 1, 23, 0, 0, 0, DateTimeKind.Utc),
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
				Created = new DateTime(2022, 11, 1, 23, 0, 0, 0, DateTimeKind.Utc),
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
				Created = new DateTime(2022, 11, 1, 23, 0, 0, 0, DateTimeKind.Utc),
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
				Created = new DateTime(2022, 11, 1, 23, 0, 0, 0, DateTimeKind.Utc),
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
				Created = new DateTime(2022, 11, 1, 23, 0, 0, 0, DateTimeKind.Utc),
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
				Created = new DateTime(2022, 11, 1, 23, 0, 0, 0, DateTimeKind.Utc),
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
				Created = new DateTime(2022, 11, 1, 23, 0, 0, 0, DateTimeKind.Utc),
				Description = "Api for IDP logs",
				DisplayName = "IDP Logs",
				Enabled = true,
				Name = "idp_log_event",
				NonEditable = false,
				RequireResourceIndicator = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -10,
				Created = new DateTime(2022, 11, 1, 23, 0, 0, 0, DateTimeKind.Utc),
				Description = "Api for video processing server",
				DisplayName = "Video Processing Server",
				Enabled = true,
				Name = "vps",
				NonEditable = false,
				RequireResourceIndicator = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -11,
				Created = new DateTime(2022, 11, 1, 23, 0, 0, 0, DateTimeKind.Utc),
				Description = "Api for the Incident Manager server",
				DisplayName = "Incident Manager Server",
				Enabled = true,
				Name = "incidentmanagerserver",
				NonEditable = false,
				RequireResourceIndicator = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -12,
				Created = new DateTime(2022, 11, 1, 23, 0, 0, 0, DateTimeKind.Utc),
				Description = "Api for failover server",
				DisplayName = "Failover Server",
				Enabled = true,
				Name = "failover",
				NonEditable = false,
				RequireResourceIndicator = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -13,
				Created = new DateTime(2022, 11, 1, 23, 0, 0, 0, DateTimeKind.Utc),
				Description = "Scim Api",
				DisplayName = "Scim",
				Enabled = true,
				Name = "scim",
				NonEditable = false,
				RequireResourceIndicator = false,
				ShowInDiscoveryDocument = true
			});
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiResourceClaim", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int");
			b.Property<int>("Id").UseIdentityColumn(1L);
			b.Property<int>("ApiResourceId").HasColumnType("int");
			b.Property<string>("Type").IsRequired().HasMaxLength(200)
				.HasColumnType("nvarchar(200)");
			b.HasKey("Id");
			b.HasIndex("ApiResourceId", "Type").IsUnique();
			b.ToTable("ApiResourceClaims", (string?)null);
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
				Type = "updated_at"
			}, new
			{
				Id = -4,
				ApiResourceId = -6,
				Type = "name"
			});
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiResourceProperty", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int");
			b.Property<int>("Id").UseIdentityColumn(1L);
			b.Property<int>("ApiResourceId").HasColumnType("int");
			b.Property<string>("Key").IsRequired().HasMaxLength(250)
				.HasColumnType("nvarchar(250)");
			b.Property<string>("Value").IsRequired().HasMaxLength(2000)
				.HasColumnType("nvarchar(2000)");
			b.HasKey("Id");
			b.HasIndex("ApiResourceId", "Key").IsUnique();
			b.ToTable("ApiResourceProperties", (string?)null);
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiResourceScope", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int");
			b.Property<int>("Id").UseIdentityColumn(1L);
			b.Property<int>("ApiResourceId").HasColumnType("int");
			b.Property<string>("Scope").IsRequired().HasMaxLength(200)
				.HasColumnType("nvarchar(200)");
			b.HasKey("Id");
			b.HasIndex("ApiResourceId", "Scope").IsUnique();
			b.ToTable("ApiResourceScopes", (string?)null);
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
			}, new
			{
				Id = -10,
				ApiResourceId = -10,
				Scope = "vps"
			}, new
			{
				Id = -11,
				ApiResourceId = -11,
				Scope = "incidentmanagerserver"
			}, new
			{
				Id = -12,
				ApiResourceId = -12,
				Scope = "failover"
			}, new
			{
				Id = -13,
				ApiResourceId = -13,
				Scope = "scim"
			});
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiResourceSecret", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int");
			b.Property<int>("Id").UseIdentityColumn(1L);
			b.Property<int>("ApiResourceId").HasColumnType("int");
			b.Property<DateTime>("Created").HasColumnType("datetime2");
			b.Property<string>("Description").HasMaxLength(1000).HasColumnType("nvarchar(1000)");
			b.Property<DateTime?>("Expiration").HasColumnType("datetime2");
			b.Property<string>("Type").IsRequired().HasMaxLength(250)
				.HasColumnType("nvarchar(250)");
			b.Property<string>("Value").IsRequired().HasMaxLength(4000)
				.HasColumnType("nvarchar(4000)");
			b.HasKey("Id");
			b.HasIndex("ApiResourceId");
			b.ToTable("ApiResourceSecrets", (string?)null);
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiScope", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int");
			b.Property<int>("Id").UseIdentityColumn(1L);
			b.Property<DateTime>("Created").HasColumnType("datetime2");
			b.Property<string>("Description").HasMaxLength(1000).HasColumnType("nvarchar(1000)");
			b.Property<string>("DisplayName").HasMaxLength(200).HasColumnType("nvarchar(200)");
			b.Property<bool>("Emphasize").HasColumnType("bit");
			b.Property<bool>("Enabled").HasColumnType("bit");
			b.Property<DateTime?>("LastAccessed").HasColumnType("datetime2");
			b.Property<string>("Name").IsRequired().HasMaxLength(200)
				.HasColumnType("nvarchar(200)");
			b.Property<bool>("NonEditable").HasColumnType("bit");
			b.Property<bool>("Required").HasColumnType("bit");
			b.Property<bool>("ShowInDiscoveryDocument").HasColumnType("bit");
			b.Property<DateTime?>("Updated").HasColumnType("datetime2");
			b.HasKey("Id");
			b.HasIndex("Name").IsUnique();
			b.ToTable("ApiScopes", (string?)null);
			b.HasData(new
			{
				Id = -1,
				Created = new DateTime(2022, 11, 1, 23, 0, 0, 0, DateTimeKind.Utc),
				DisplayName = "Client registration scope",
				Emphasize = false,
				Enabled = true,
				Name = "write:client",
				NonEditable = false,
				Required = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -2,
				Created = new DateTime(2022, 11, 1, 23, 0, 0, 0, DateTimeKind.Utc),
				DisplayName = "Management scope",
				Emphasize = false,
				Enabled = true,
				Name = "management",
				NonEditable = false,
				Required = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -3,
				Created = new DateTime(2022, 11, 1, 23, 0, 0, 0, DateTimeKind.Utc),
				DisplayName = "Recorder scope",
				Emphasize = false,
				Enabled = true,
				Name = "recorder",
				NonEditable = false,
				Required = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -4,
				Created = new DateTime(2022, 11, 1, 23, 0, 0, 0, DateTimeKind.Utc),
				DisplayName = "IDP Management Scope",
				Emphasize = false,
				Enabled = true,
				Name = "idp_management",
				NonEditable = false,
				Required = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -5,
				Created = new DateTime(2022, 11, 1, 23, 0, 0, 0, DateTimeKind.Utc),
				DisplayName = "Datacollector Scope",
				Emphasize = false,
				Enabled = true,
				Name = "datacollector",
				NonEditable = false,
				Required = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -6,
				Created = new DateTime(2022, 11, 1, 23, 0, 0, 0, DateTimeKind.Utc),
				DisplayName = "Management server Scope",
				Emphasize = false,
				Enabled = true,
				Name = "managementserver",
				NonEditable = false,
				Required = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -7,
				Created = new DateTime(2022, 11, 1, 23, 0, 0, 0, DateTimeKind.Utc),
				DisplayName = "Failover response server Scope",
				Emphasize = false,
				Enabled = true,
				Name = "failoverresponseserver",
				NonEditable = false,
				Required = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -8,
				Created = new DateTime(2022, 11, 1, 23, 0, 0, 0, DateTimeKind.Utc),
				DisplayName = "Media Db Scope",
				Emphasize = false,
				Enabled = true,
				Name = "mediadb",
				NonEditable = false,
				Required = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -9,
				Created = new DateTime(2022, 11, 1, 23, 0, 0, 0, DateTimeKind.Utc),
				DisplayName = "Log Event Scope",
				Emphasize = false,
				Enabled = true,
				Name = "idp_log_event",
				NonEditable = false,
				Required = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -10,
				Created = new DateTime(2022, 11, 1, 23, 0, 0, 0, DateTimeKind.Utc),
				DisplayName = "Video Processing Server Scope",
				Emphasize = false,
				Enabled = true,
				Name = "vps",
				NonEditable = false,
				Required = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -11,
				Created = new DateTime(2022, 11, 1, 23, 0, 0, 0, DateTimeKind.Utc),
				DisplayName = "Incident Manager server scope",
				Emphasize = false,
				Enabled = true,
				Name = "incidentmanagerserver",
				NonEditable = false,
				Required = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -12,
				Created = new DateTime(2022, 11, 1, 23, 0, 0, 0, DateTimeKind.Utc),
				DisplayName = "Failover server Scope",
				Emphasize = false,
				Enabled = true,
				Name = "failover",
				NonEditable = false,
				Required = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -13,
				Created = new DateTime(2022, 11, 1, 23, 0, 0, 0, DateTimeKind.Utc),
				DisplayName = "Scim Scope",
				Emphasize = false,
				Enabled = true,
				Name = "scim",
				NonEditable = false,
				Required = false,
				ShowInDiscoveryDocument = true
			});
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiScopeClaim", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int");
			b.Property<int>("Id").UseIdentityColumn(1L);
			b.Property<int>("ScopeId").HasColumnType("int");
			b.Property<string>("Type").IsRequired().HasMaxLength(200)
				.HasColumnType("nvarchar(200)");
			b.HasKey("Id");
			b.HasIndex("ScopeId", "Type").IsUnique();
			b.ToTable("ApiScopeClaims", (string?)null);
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiScopeProperty", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int");
			b.Property<int>("Id").UseIdentityColumn(1L);
			b.Property<string>("Key").IsRequired().HasMaxLength(250)
				.HasColumnType("nvarchar(250)");
			b.Property<int>("ScopeId").HasColumnType("int");
			b.Property<string>("Value").IsRequired().HasMaxLength(2000)
				.HasColumnType("nvarchar(2000)");
			b.HasKey("Id");
			b.HasIndex("ScopeId", "Key").IsUnique();
			b.ToTable("ApiScopeProperties", (string?)null);
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.Client", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int");
			b.Property<int>("Id").UseIdentityColumn(1L);
			b.Property<int>("AbsoluteRefreshTokenLifetime").HasColumnType("int");
			b.Property<int>("AccessTokenLifetime").HasColumnType("int");
			b.Property<int>("AccessTokenType").HasColumnType("int");
			b.Property<bool>("AllowAccessTokensViaBrowser").HasColumnType("bit");
			b.Property<bool>("AllowOfflineAccess").HasColumnType("bit");
			b.Property<bool>("AllowPlainTextPkce").HasColumnType("bit");
			b.Property<bool>("AllowRememberConsent").HasColumnType("bit");
			b.Property<string>("AllowedIdentityTokenSigningAlgorithms").HasMaxLength(100).HasColumnType("nvarchar(100)");
			b.Property<bool>("AlwaysIncludeUserClaimsInIdToken").HasColumnType("bit");
			b.Property<bool>("AlwaysSendClientClaims").HasColumnType("bit");
			b.Property<int>("AuthorizationCodeLifetime").HasColumnType("int");
			b.Property<bool>("BackChannelLogoutSessionRequired").HasColumnType("bit");
			b.Property<string>("BackChannelLogoutUri").HasMaxLength(2000).HasColumnType("nvarchar(2000)");
			b.Property<int?>("CibaLifetime").HasColumnType("int");
			b.Property<string>("ClientClaimsPrefix").HasMaxLength(200).HasColumnType("nvarchar(200)");
			b.Property<string>("ClientId").IsRequired().HasMaxLength(200)
				.HasColumnType("nvarchar(200)");
			b.Property<string>("ClientName").HasMaxLength(200).HasColumnType("nvarchar(200)");
			b.Property<string>("ClientUri").HasMaxLength(2000).HasColumnType("nvarchar(2000)");
			b.Property<int?>("ConsentLifetime").HasColumnType("int");
			b.Property<bool?>("CoordinateLifetimeWithUserSession").HasColumnType("bit");
			b.Property<DateTime>("Created").HasColumnType("datetime2");
			b.Property<TimeSpan>("DPoPClockSkew").HasColumnType("time");
			b.Property<int>("DPoPValidationMode").HasColumnType("int");
			b.Property<string>("Description").HasMaxLength(1000).HasColumnType("nvarchar(1000)");
			b.Property<int>("DeviceCodeLifetime").HasColumnType("int");
			b.Property<bool>("EnableLocalLogin").HasColumnType("bit");
			b.Property<bool>("Enabled").HasColumnType("bit");
			b.Property<bool>("FrontChannelLogoutSessionRequired").HasColumnType("bit");
			b.Property<string>("FrontChannelLogoutUri").HasMaxLength(2000).HasColumnType("nvarchar(2000)");
			b.Property<int>("IdentityTokenLifetime").HasColumnType("int");
			b.Property<bool>("IncludeJwtId").HasColumnType("bit");
			b.Property<string>("InitiateLoginUri").HasMaxLength(2000).HasColumnType("nvarchar(2000)");
			b.Property<DateTime?>("LastAccessed").HasColumnType("datetime2");
			b.Property<string>("LogoUri").HasMaxLength(2000).HasColumnType("nvarchar(2000)");
			b.Property<bool>("NonEditable").HasColumnType("bit");
			b.Property<string>("PairWiseSubjectSalt").HasMaxLength(200).HasColumnType("nvarchar(200)");
			b.Property<int?>("PollingInterval").HasColumnType("int");
			b.Property<string>("ProtocolType").IsRequired().HasMaxLength(200)
				.HasColumnType("nvarchar(200)");
			b.Property<int?>("PushedAuthorizationLifetime").HasColumnType("int");
			b.Property<int>("RefreshTokenExpiration").HasColumnType("int");
			b.Property<int>("RefreshTokenUsage").HasColumnType("int");
			b.Property<bool>("RequireClientSecret").HasColumnType("bit");
			b.Property<bool>("RequireConsent").HasColumnType("bit");
			b.Property<bool>("RequireDPoP").HasColumnType("bit");
			b.Property<bool>("RequirePkce").HasColumnType("bit");
			b.Property<bool>("RequirePushedAuthorization").HasColumnType("bit");
			b.Property<bool>("RequireRequestObject").HasColumnType("bit");
			b.Property<int>("SlidingRefreshTokenLifetime").HasColumnType("int");
			b.Property<bool>("UpdateAccessTokenClaimsOnRefresh").HasColumnType("bit");
			b.Property<DateTime?>("Updated").HasColumnType("datetime2");
			b.Property<string>("UserCodeType").HasMaxLength(100).HasColumnType("nvarchar(100)");
			b.Property<int?>("UserSsoLifetime").HasColumnType("int");
			b.HasKey("Id");
			b.HasIndex("ClientId").IsUnique();
			b.ToTable("Clients", (string?)null);
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
				Created = new DateTime(2022, 11, 1, 23, 0, 0, 0, DateTimeKind.Utc),
				DPoPClockSkew = new TimeSpan(0, 0, 5, 0, 0),
				DPoPValidationMode = 0,
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
				RequireDPoP = false,
				RequirePkce = true,
				RequirePushedAuthorization = false,
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
				Created = new DateTime(2022, 11, 1, 23, 0, 0, 0, DateTimeKind.Utc),
				DPoPClockSkew = new TimeSpan(0, 0, 5, 0, 0),
				DPoPValidationMode = 0,
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
				RequireDPoP = false,
				RequirePkce = true,
				RequirePushedAuthorization = false,
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
				AlwaysIncludeUserClaimsInIdToken = true,
				AlwaysSendClientClaims = false,
				AuthorizationCodeLifetime = 300,
				BackChannelLogoutSessionRequired = true,
				ClientClaimsPrefix = "client_",
				ClientId = "VmsAdminClient",
				ClientName = "VmsAdminClient",
				Created = new DateTime(2022, 11, 1, 23, 0, 0, 0, DateTimeKind.Utc),
				DPoPClockSkew = new TimeSpan(0, 0, 5, 0, 0),
				DPoPValidationMode = 0,
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
				RequireDPoP = false,
				RequirePkce = true,
				RequirePushedAuthorization = false,
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
				AlwaysIncludeUserClaimsInIdToken = true,
				AlwaysSendClientClaims = false,
				AuthorizationCodeLifetime = 300,
				BackChannelLogoutSessionRequired = true,
				ClientClaimsPrefix = "client_",
				ClientId = "VmsClient",
				ClientName = "VmsClient",
				Created = new DateTime(2022, 11, 1, 23, 0, 0, 0, DateTimeKind.Utc),
				DPoPClockSkew = new TimeSpan(0, 0, 5, 0, 0),
				DPoPValidationMode = 0,
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
				RequireDPoP = false,
				RequirePkce = true,
				RequirePushedAuthorization = false,
				RequireRequestObject = false,
				SlidingRefreshTokenLifetime = 1296000,
				UpdateAccessTokenClaimsOnRefresh = true
			}, new
			{
				Id = -5,
				AbsoluteRefreshTokenLifetime = 0,
				AccessTokenLifetime = 3600,
				AccessTokenType = 0,
				AllowAccessTokensViaBrowser = false,
				AllowOfflineAccess = true,
				AllowPlainTextPkce = false,
				AllowRememberConsent = true,
				AlwaysIncludeUserClaimsInIdToken = true,
				AlwaysSendClientClaims = false,
				AuthorizationCodeLifetime = 300,
				BackChannelLogoutSessionRequired = true,
				ClientClaimsPrefix = "client_",
				ClientId = "VmsMobileClient",
				ClientName = "VmsMobileClient",
				Created = new DateTime(2022, 11, 1, 23, 0, 0, 0, DateTimeKind.Utc),
				DPoPClockSkew = new TimeSpan(0, 0, 5, 0, 0),
				DPoPValidationMode = 0,
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
				RequireDPoP = false,
				RequirePkce = true,
				RequirePushedAuthorization = false,
				RequireRequestObject = false,
				SlidingRefreshTokenLifetime = 10800,
				UpdateAccessTokenClaimsOnRefresh = true
			});
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientClaim", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int");
			b.Property<int>("Id").UseIdentityColumn(1L);
			b.Property<int>("ClientId").HasColumnType("int");
			b.Property<string>("Type").IsRequired().HasMaxLength(250)
				.HasColumnType("nvarchar(250)");
			b.Property<string>("Value").IsRequired().HasMaxLength(250)
				.HasColumnType("nvarchar(250)");
			b.HasKey("Id");
			b.HasIndex("ClientId", "Type", "Value").IsUnique();
			b.ToTable("ClientClaims", (string?)null);
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientCorsOrigin", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int");
			b.Property<int>("Id").UseIdentityColumn(1L);
			b.Property<int>("ClientId").HasColumnType("int");
			b.Property<string>("Origin").IsRequired().HasMaxLength(150)
				.HasColumnType("nvarchar(150)");
			b.HasKey("Id");
			b.HasIndex("ClientId", "Origin").IsUnique();
			b.ToTable("ClientCorsOrigins", (string?)null);
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientGrantType", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int");
			b.Property<int>("Id").UseIdentityColumn(1L);
			b.Property<int>("ClientId").HasColumnType("int");
			b.Property<string>("GrantType").IsRequired().HasMaxLength(250)
				.HasColumnType("nvarchar(250)");
			b.HasKey("Id");
			b.HasIndex("ClientId", "GrantType").IsUnique();
			b.ToTable("ClientGrantTypes", (string?)null);
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
				Id = -16,
				ClientId = -3,
				GrantType = "urn:ietf:params:oauth:grant-type:token-exchange"
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
			}, new
			{
				Id = -17,
				ClientId = -4,
				GrantType = "urn:ietf:params:oauth:grant-type:token-exchange"
			}, new
			{
				Id = -12,
				ClientId = -5,
				GrantType = "authorization_code"
			}, new
			{
				Id = -13,
				ClientId = -5,
				GrantType = "refresh_token"
			}, new
			{
				Id = -14,
				ClientId = -5,
				GrantType = "password"
			}, new
			{
				Id = -15,
				ClientId = -5,
				GrantType = "windows_credentials"
			});
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientIdPRestriction", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int");
			b.Property<int>("Id").UseIdentityColumn(1L);
			b.Property<int>("ClientId").HasColumnType("int");
			b.Property<string>("Provider").IsRequired().HasMaxLength(200)
				.HasColumnType("nvarchar(200)");
			b.HasKey("Id");
			b.HasIndex("ClientId", "Provider").IsUnique();
			b.ToTable("ClientIdPRestrictions", (string?)null);
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientPostLogoutRedirectUri", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int");
			b.Property<int>("Id").UseIdentityColumn(1L);
			b.Property<int>("ClientId").HasColumnType("int");
			b.Property<string>("PostLogoutRedirectUri").IsRequired().HasMaxLength(400)
				.HasColumnType("nvarchar(400)");
			b.HasKey("Id");
			b.HasIndex("ClientId", "PostLogoutRedirectUri").IsUnique();
			b.ToTable("ClientPostLogoutRedirectUris", (string?)null);
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientProperty", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int");
			b.Property<int>("Id").UseIdentityColumn(1L);
			b.Property<int>("ClientId").HasColumnType("int");
			b.Property<string>("Key").IsRequired().HasMaxLength(250)
				.HasColumnType("nvarchar(250)");
			b.Property<string>("Value").IsRequired().HasMaxLength(2000)
				.HasColumnType("nvarchar(2000)");
			b.HasKey("Id");
			b.HasIndex("ClientId", "Key").IsUnique();
			b.ToTable("ClientProperties", (string?)null);
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientRedirectUri", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int");
			b.Property<int>("Id").UseIdentityColumn(1L);
			b.Property<int>("ClientId").HasColumnType("int");
			b.Property<string>("RedirectUri").IsRequired().HasMaxLength(400)
				.HasColumnType("nvarchar(400)");
			b.HasKey("Id");
			b.HasIndex("ClientId", "RedirectUri").IsUnique();
			b.ToTable("ClientRedirectUris", (string?)null);
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
			}, new
			{
				Id = -3,
				ClientId = -5,
				RedirectUri = "com.milestonesys.xprotect://login.external"
			});
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientScope", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int");
			b.Property<int>("Id").UseIdentityColumn(1L);
			b.Property<int>("ClientId").HasColumnType("int");
			b.Property<string>("Scope").IsRequired().HasMaxLength(200)
				.HasColumnType("nvarchar(200)");
			b.HasKey("Id");
			b.HasIndex("ClientId", "Scope").IsUnique();
			b.ToTable("ClientScopes", (string?)null);
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
				Id = -13,
				ClientId = -3,
				Scope = "incidentmanagerserver"
			}, new
			{
				Id = -20,
				ClientId = -3,
				Scope = "provider"
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
			}, new
			{
				Id = -14,
				ClientId = -4,
				Scope = "incidentmanagerserver"
			}, new
			{
				Id = -21,
				ClientId = -4,
				Scope = "provider"
			}, new
			{
				Id = -15,
				ClientId = -5,
				Scope = "managementserver"
			}, new
			{
				Id = -16,
				ClientId = -5,
				Scope = "openid"
			}, new
			{
				Id = -17,
				ClientId = -5,
				Scope = "profile"
			}, new
			{
				Id = -18,
				ClientId = -5,
				Scope = "email"
			}, new
			{
				Id = -19,
				ClientId = -5,
				Scope = "offline_access"
			});
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientSecret", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int");
			b.Property<int>("Id").UseIdentityColumn(1L);
			b.Property<int>("ClientId").HasColumnType("int");
			b.Property<DateTime>("Created").HasColumnType("datetime2");
			b.Property<string>("Description").HasMaxLength(2000).HasColumnType("nvarchar(2000)");
			b.Property<DateTime?>("Expiration").HasColumnType("datetime2");
			b.Property<string>("Type").IsRequired().HasMaxLength(250)
				.HasColumnType("nvarchar(250)");
			b.Property<string>("Value").IsRequired().HasMaxLength(4000)
				.HasColumnType("nvarchar(4000)");
			b.HasKey("Id");
			b.HasIndex("ClientId");
			b.ToTable("ClientSecrets", (string?)null);
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.IdentityProvider", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int");
			b.Property<int>("Id").UseIdentityColumn(1L);
			b.Property<DateTime>("Created").HasColumnType("datetime2");
			b.Property<string>("DisplayName").HasMaxLength(200).HasColumnType("nvarchar(200)");
			b.Property<bool>("Enabled").HasColumnType("bit");
			b.Property<DateTime?>("LastAccessed").HasColumnType("datetime2");
			b.Property<bool>("NonEditable").HasColumnType("bit");
			b.Property<string>("Properties").HasColumnType("nvarchar(max)");
			b.Property<string>("Scheme").IsRequired().HasMaxLength(200)
				.HasColumnType("nvarchar(200)");
			b.Property<string>("Type").IsRequired().HasMaxLength(20)
				.HasColumnType("nvarchar(20)");
			b.Property<DateTime?>("Updated").HasColumnType("datetime2");
			b.HasKey("Id");
			b.HasIndex("Scheme").IsUnique();
			b.ToTable("IdentityProviders", (string?)null);
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.IdentityResource", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int");
			b.Property<int>("Id").UseIdentityColumn(1L);
			b.Property<DateTime>("Created").HasColumnType("datetime2");
			b.Property<string>("Description").HasMaxLength(1000).HasColumnType("nvarchar(1000)");
			b.Property<string>("DisplayName").HasMaxLength(200).HasColumnType("nvarchar(200)");
			b.Property<bool>("Emphasize").HasColumnType("bit");
			b.Property<bool>("Enabled").HasColumnType("bit");
			b.Property<string>("Name").IsRequired().HasMaxLength(200)
				.HasColumnType("nvarchar(200)");
			b.Property<bool>("NonEditable").HasColumnType("bit");
			b.Property<bool>("Required").HasColumnType("bit");
			b.Property<bool>("ShowInDiscoveryDocument").HasColumnType("bit");
			b.Property<DateTime?>("Updated").HasColumnType("datetime2");
			b.HasKey("Id");
			b.HasIndex("Name").IsUnique();
			b.ToTable("IdentityResources", (string?)null);
			b.HasData(new
			{
				Id = -1,
				Created = new DateTime(2022, 11, 1, 23, 0, 0, 0, DateTimeKind.Utc),
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
				Created = new DateTime(2022, 11, 1, 23, 0, 0, 0, DateTimeKind.Utc),
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
				Created = new DateTime(2022, 11, 1, 23, 0, 0, 0, DateTimeKind.Utc),
				DisplayName = "Your email address",
				Emphasize = true,
				Enabled = true,
				Name = "email",
				NonEditable = false,
				Required = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -4,
				Created = new DateTime(2022, 11, 1, 23, 0, 0, 0, DateTimeKind.Utc),
				Description = "Your users profile information belonging to the current login provider",
				DisplayName = "User provider profile",
				Emphasize = true,
				Enabled = true,
				Name = "provider",
				NonEditable = false,
				Required = false,
				ShowInDiscoveryDocument = true
			});
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.IdentityResourceClaim", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int");
			b.Property<int>("Id").UseIdentityColumn(1L);
			b.Property<int>("IdentityResourceId").HasColumnType("int");
			b.Property<string>("Type").IsRequired().HasMaxLength(200)
				.HasColumnType("nvarchar(200)");
			b.HasKey("Id");
			b.HasIndex("IdentityResourceId", "Type").IsUnique();
			b.ToTable("IdentityResourceClaims", (string?)null);
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
			b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int");
			b.Property<int>("Id").UseIdentityColumn(1L);
			b.Property<int>("IdentityResourceId").HasColumnType("int");
			b.Property<string>("Key").IsRequired().HasMaxLength(250)
				.HasColumnType("nvarchar(250)");
			b.Property<string>("Value").IsRequired().HasMaxLength(2000)
				.HasColumnType("nvarchar(2000)");
			b.HasKey("Id");
			b.HasIndex("IdentityResourceId", "Key").IsUnique();
			b.ToTable("IdentityResourceProperties", (string?)null);
		});
		modelBuilder.Entity("VideoOS.IdentityServer.Entities.EventLog", delegate(EntityTypeBuilder b)
		{
			b.Property<long>("Id").ValueGeneratedOnAdd().HasColumnType("bigint");
			b.Property<long>("Id").UseIdentityColumn(1L);
			b.Property<string>("ClientId").HasColumnType("nvarchar(max)");
			b.Property<string>("ClientName").HasColumnType("nvarchar(max)");
			b.Property<string>("Event").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<string>("ObjectId").HasColumnType("nvarchar(max)");
			b.Property<string>("ObjectName").HasColumnType("nvarchar(max)");
			b.Property<string>("RemoteIpAddress").IsRequired().HasColumnType("nvarchar(max)");
			b.Property<DateTimeOffset>("TimeStamp").HasColumnType("datetimeoffset");
			b.Property<string>("UserName").IsRequired().HasColumnType("nvarchar(max)");
			b.HasKey("Id");
			b.ToTable("EventLogs", null, delegate(TableBuilder t)
			{
				t.ExcludeFromMigrations();
			});
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiResourceClaim", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Duende.IdentityServer.EntityFramework.Entities.ApiResource", "ApiResource").WithMany("UserClaims").HasForeignKey("ApiResourceId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.Navigation("ApiResource");
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiResourceProperty", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Duende.IdentityServer.EntityFramework.Entities.ApiResource", "ApiResource").WithMany("Properties").HasForeignKey("ApiResourceId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.Navigation("ApiResource");
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiResourceScope", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Duende.IdentityServer.EntityFramework.Entities.ApiResource", "ApiResource").WithMany("Scopes").HasForeignKey("ApiResourceId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.Navigation("ApiResource");
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiResourceSecret", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Duende.IdentityServer.EntityFramework.Entities.ApiResource", "ApiResource").WithMany("Secrets").HasForeignKey("ApiResourceId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.Navigation("ApiResource");
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiScopeClaim", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Duende.IdentityServer.EntityFramework.Entities.ApiScope", "Scope").WithMany("UserClaims").HasForeignKey("ScopeId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.Navigation("Scope");
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiScopeProperty", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Duende.IdentityServer.EntityFramework.Entities.ApiScope", "Scope").WithMany("Properties").HasForeignKey("ScopeId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.Navigation("Scope");
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientClaim", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Duende.IdentityServer.EntityFramework.Entities.Client", "Client").WithMany("Claims").HasForeignKey("ClientId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.Navigation("Client");
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientCorsOrigin", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Duende.IdentityServer.EntityFramework.Entities.Client", "Client").WithMany("AllowedCorsOrigins").HasForeignKey("ClientId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.Navigation("Client");
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientGrantType", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Duende.IdentityServer.EntityFramework.Entities.Client", "Client").WithMany("AllowedGrantTypes").HasForeignKey("ClientId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.Navigation("Client");
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientIdPRestriction", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Duende.IdentityServer.EntityFramework.Entities.Client", "Client").WithMany("IdentityProviderRestrictions").HasForeignKey("ClientId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.Navigation("Client");
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientPostLogoutRedirectUri", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Duende.IdentityServer.EntityFramework.Entities.Client", "Client").WithMany("PostLogoutRedirectUris").HasForeignKey("ClientId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.Navigation("Client");
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientProperty", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Duende.IdentityServer.EntityFramework.Entities.Client", "Client").WithMany("Properties").HasForeignKey("ClientId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.Navigation("Client");
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientRedirectUri", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Duende.IdentityServer.EntityFramework.Entities.Client", "Client").WithMany("RedirectUris").HasForeignKey("ClientId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.Navigation("Client");
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientScope", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Duende.IdentityServer.EntityFramework.Entities.Client", "Client").WithMany("AllowedScopes").HasForeignKey("ClientId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.Navigation("Client");
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientSecret", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Duende.IdentityServer.EntityFramework.Entities.Client", "Client").WithMany("ClientSecrets").HasForeignKey("ClientId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.Navigation("Client");
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.IdentityResourceClaim", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Duende.IdentityServer.EntityFramework.Entities.IdentityResource", "IdentityResource").WithMany("UserClaims").HasForeignKey("IdentityResourceId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.Navigation("IdentityResource");
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.IdentityResourceProperty", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Duende.IdentityServer.EntityFramework.Entities.IdentityResource", "IdentityResource").WithMany("Properties").HasForeignKey("IdentityResourceId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
			b.Navigation("IdentityResource");
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiResource", delegate(EntityTypeBuilder b)
		{
			b.Navigation("Properties");
			b.Navigation("Scopes");
			b.Navigation("Secrets");
			b.Navigation("UserClaims");
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiScope", delegate(EntityTypeBuilder b)
		{
			b.Navigation("Properties");
			b.Navigation("UserClaims");
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.Client", delegate(EntityTypeBuilder b)
		{
			b.Navigation("AllowedCorsOrigins");
			b.Navigation("AllowedGrantTypes");
			b.Navigation("AllowedScopes");
			b.Navigation("Claims");
			b.Navigation("ClientSecrets");
			b.Navigation("IdentityProviderRestrictions");
			b.Navigation("PostLogoutRedirectUris");
			b.Navigation("Properties");
			b.Navigation("RedirectUris");
		});
		modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.IdentityResource", delegate(EntityTypeBuilder b)
		{
			b.Navigation("Properties");
			b.Navigation("UserClaims");
		});
	}
}
