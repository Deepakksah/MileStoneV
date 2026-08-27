using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VideoOS.IdentityServer.Data.Migrations.IdentityServer.SeededConfigurationDb;

[DbContext(typeof(SeededConfigurationDbContext))]
[Migration("20210406131958_UpdateTo_Duende_5.1_ConfigurationDbMigration")]
public class UpdateTo_Duende_51_ConfigurationDbMigration : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.UpdateData("ApiResources", "Id", -8, "Created", new DateTime(2021, 4, 6, 13, 19, 58, 286, DateTimeKind.Utc).AddTicks(7754L));
		migrationBuilder.UpdateData("ApiResources", "Id", -7, "Created", new DateTime(2021, 4, 6, 13, 19, 58, 286, DateTimeKind.Utc).AddTicks(7754L));
		migrationBuilder.UpdateData("ApiResources", "Id", -6, "Created", new DateTime(2021, 4, 6, 13, 19, 58, 286, DateTimeKind.Utc).AddTicks(7745L));
		migrationBuilder.UpdateData("ApiResources", "Id", -5, "Created", new DateTime(2021, 4, 6, 13, 19, 58, 286, DateTimeKind.Utc).AddTicks(7737L));
		migrationBuilder.UpdateData("ApiResources", "Id", -4, "Created", new DateTime(2021, 4, 6, 13, 19, 58, 286, DateTimeKind.Utc).AddTicks(7728L));
		migrationBuilder.UpdateData("ApiResources", "Id", -3, "Created", new DateTime(2021, 4, 6, 13, 19, 58, 286, DateTimeKind.Utc).AddTicks(7720L));
		migrationBuilder.UpdateData("ApiResources", "Id", -2, "Created", new DateTime(2021, 4, 6, 13, 19, 58, 286, DateTimeKind.Utc).AddTicks(7701L));
		migrationBuilder.UpdateData("ApiResources", "Id", -1, "Created", new DateTime(2021, 4, 6, 13, 19, 58, 286, DateTimeKind.Utc).AddTicks(7049L));
		migrationBuilder.UpdateData("Clients", "Id", -2, "Created", new DateTime(2021, 4, 6, 13, 19, 58, 256, DateTimeKind.Utc).AddTicks(1664L));
		migrationBuilder.UpdateData("Clients", "Id", -1, "Created", new DateTime(2021, 4, 6, 13, 19, 58, 254, DateTimeKind.Utc).AddTicks(7129L));
		migrationBuilder.UpdateData("IdentityResources", "Id", -3, "Created", new DateTime(2021, 4, 6, 13, 19, 58, 310, DateTimeKind.Utc).AddTicks(610L));
		migrationBuilder.UpdateData("IdentityResources", "Id", -2, "Created", new DateTime(2021, 4, 6, 13, 19, 58, 306, DateTimeKind.Utc).AddTicks(847L));
		migrationBuilder.UpdateData("IdentityResources", "Id", -1, "Created", new DateTime(2021, 4, 6, 13, 19, 58, 302, DateTimeKind.Utc).AddTicks(461L));
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.UpdateData("ApiResources", "Id", -8, "Created", new DateTime(2021, 4, 6, 11, 18, 21, 715, DateTimeKind.Utc).AddTicks(8901L));
		migrationBuilder.UpdateData("ApiResources", "Id", -7, "Created", new DateTime(2021, 4, 6, 11, 18, 21, 715, DateTimeKind.Utc).AddTicks(8901L));
		migrationBuilder.UpdateData("ApiResources", "Id", -6, "Created", new DateTime(2021, 4, 6, 11, 18, 21, 715, DateTimeKind.Utc).AddTicks(8891L));
		migrationBuilder.UpdateData("ApiResources", "Id", -5, "Created", new DateTime(2021, 4, 6, 11, 18, 21, 715, DateTimeKind.Utc).AddTicks(8884L));
		migrationBuilder.UpdateData("ApiResources", "Id", -4, "Created", new DateTime(2021, 4, 6, 11, 18, 21, 715, DateTimeKind.Utc).AddTicks(8864L));
		migrationBuilder.UpdateData("ApiResources", "Id", -3, "Created", new DateTime(2021, 4, 6, 11, 18, 21, 715, DateTimeKind.Utc).AddTicks(8855L));
		migrationBuilder.UpdateData("ApiResources", "Id", -2, "Created", new DateTime(2021, 4, 6, 11, 18, 21, 715, DateTimeKind.Utc).AddTicks(8809L));
		migrationBuilder.UpdateData("ApiResources", "Id", -1, "Created", new DateTime(2021, 4, 6, 11, 18, 21, 715, DateTimeKind.Utc).AddTicks(7831L));
		migrationBuilder.UpdateData("Clients", "Id", -2, "Created", new DateTime(2021, 4, 6, 11, 18, 21, 683, DateTimeKind.Utc).AddTicks(7674L));
		migrationBuilder.UpdateData("Clients", "Id", -1, "Created", new DateTime(2021, 4, 6, 11, 18, 21, 682, DateTimeKind.Utc).AddTicks(2537L));
		migrationBuilder.UpdateData("IdentityResources", "Id", -3, "Created", new DateTime(2021, 4, 6, 11, 18, 21, 739, DateTimeKind.Utc).AddTicks(7349L));
		migrationBuilder.UpdateData("IdentityResources", "Id", -2, "Created", new DateTime(2021, 4, 6, 11, 18, 21, 735, DateTimeKind.Utc).AddTicks(5525L));
		migrationBuilder.UpdateData("IdentityResources", "Id", -1, "Created", new DateTime(2021, 4, 6, 11, 18, 21, 731, DateTimeKind.Utc).AddTicks(2109L));
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
				Created = new DateTime(2021, 4, 6, 13, 19, 58, 286, DateTimeKind.Utc).AddTicks(7049L),
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
				Created = new DateTime(2021, 4, 6, 13, 19, 58, 286, DateTimeKind.Utc).AddTicks(7701L),
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
				Created = new DateTime(2021, 4, 6, 13, 19, 58, 286, DateTimeKind.Utc).AddTicks(7720L),
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
				Created = new DateTime(2021, 4, 6, 13, 19, 58, 286, DateTimeKind.Utc).AddTicks(7728L),
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
				Created = new DateTime(2021, 4, 6, 13, 19, 58, 286, DateTimeKind.Utc).AddTicks(7737L),
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
				Created = new DateTime(2021, 4, 6, 13, 19, 58, 286, DateTimeKind.Utc).AddTicks(7745L),
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
				Created = new DateTime(2021, 4, 6, 13, 19, 58, 286, DateTimeKind.Utc).AddTicks(7754L),
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
				Created = new DateTime(2021, 4, 6, 13, 19, 58, 286, DateTimeKind.Utc).AddTicks(7754L),
				Description = "Api for media db",
				DisplayName = "Media Db",
				Enabled = true,
				Name = "mediadb",
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
				Created = new DateTime(2021, 4, 6, 13, 19, 58, 254, DateTimeKind.Utc).AddTicks(7129L),
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
				Created = new DateTime(2021, 4, 6, 13, 19, 58, 256, DateTimeKind.Utc).AddTicks(1664L),
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
				Created = new DateTime(2021, 4, 6, 13, 19, 58, 302, DateTimeKind.Utc).AddTicks(461L),
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
				Created = new DateTime(2021, 4, 6, 13, 19, 58, 306, DateTimeKind.Utc).AddTicks(847L),
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
				Created = new DateTime(2021, 4, 6, 13, 19, 58, 310, DateTimeKind.Utc).AddTicks(610L),
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
