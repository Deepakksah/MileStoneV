using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VideoOS.IdentityServer.Data.Migrations.IdentityServer.SeededConfigurationDb;

[DbContext(typeof(SeededConfigurationDbContext))]
[Migration("20220104141038_AddVPSScope")]
public class AddVPSScope : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.UpdateData("ApiResources", "Id", -9, "Created", new DateTime(2022, 1, 4, 14, 10, 37, 704, DateTimeKind.Utc).AddTicks(89L));
		migrationBuilder.UpdateData("ApiResources", "Id", -8, "Created", new DateTime(2022, 1, 4, 14, 10, 37, 704, DateTimeKind.Utc).AddTicks(80L));
		migrationBuilder.UpdateData("ApiResources", "Id", -7, "Created", new DateTime(2022, 1, 4, 14, 10, 37, 703, DateTimeKind.Utc).AddTicks(9987L));
		migrationBuilder.UpdateData("ApiResources", "Id", -6, "Created", new DateTime(2022, 1, 4, 14, 10, 37, 703, DateTimeKind.Utc).AddTicks(9979L));
		migrationBuilder.UpdateData("ApiResources", "Id", -5, "Created", new DateTime(2022, 1, 4, 14, 10, 37, 703, DateTimeKind.Utc).AddTicks(9971L));
		migrationBuilder.UpdateData("ApiResources", "Id", -4, "Created", new DateTime(2022, 1, 4, 14, 10, 37, 703, DateTimeKind.Utc).AddTicks(9962L));
		migrationBuilder.UpdateData("ApiResources", "Id", -3, "Created", new DateTime(2022, 1, 4, 14, 10, 37, 703, DateTimeKind.Utc).AddTicks(9953L));
		migrationBuilder.UpdateData("ApiResources", "Id", -2, "Created", new DateTime(2022, 1, 4, 14, 10, 37, 703, DateTimeKind.Utc).AddTicks(9934L));
		migrationBuilder.UpdateData("ApiResources", "Id", -1, "Created", new DateTime(2022, 1, 4, 14, 10, 37, 703, DateTimeKind.Utc).AddTicks(9335L));
		migrationBuilder.InsertData("ApiResources", new string[12]
		{
			"Id", "AllowedAccessTokenSigningAlgorithms", "Created", "Description", "DisplayName", "Enabled", "LastAccessed", "Name", "NonEditable", "RequireResourceIndicator",
			"ShowInDiscoveryDocument", "Updated"
		}, new object[12]
		{
			-10,
			null,
			new DateTime(2022, 1, 4, 14, 10, 37, 704, DateTimeKind.Utc).AddTicks(96L),
			"Api for video processing server",
			"Video Processing Server",
			true,
			null,
			"vps",
			false,
			false,
			true,
			null
		});
		migrationBuilder.InsertData("ApiScopes", new string[8] { "Id", "Description", "DisplayName", "Emphasize", "Enabled", "Name", "Required", "ShowInDiscoveryDocument" }, new object[8] { -10, null, "Video Processing Server Scope", false, true, "vps", false, true });
		migrationBuilder.UpdateData("Clients", "Id", -4, "Created", new DateTime(2022, 1, 4, 14, 10, 37, 671, DateTimeKind.Utc).AddTicks(5763L));
		migrationBuilder.UpdateData("Clients", "Id", -3, "Created", new DateTime(2022, 1, 4, 14, 10, 37, 671, DateTimeKind.Utc).AddTicks(5326L));
		migrationBuilder.UpdateData("Clients", "Id", -2, "Created", new DateTime(2022, 1, 4, 14, 10, 37, 671, DateTimeKind.Utc).AddTicks(5084L));
		migrationBuilder.UpdateData("Clients", "Id", -1, "Created", new DateTime(2022, 1, 4, 14, 10, 37, 669, DateTimeKind.Utc).AddTicks(8549L));
		migrationBuilder.UpdateData("IdentityResources", "Id", -3, "Created", new DateTime(2022, 1, 4, 14, 10, 37, 730, DateTimeKind.Utc).AddTicks(5634L));
		migrationBuilder.UpdateData("IdentityResources", "Id", -2, "Created", new DateTime(2022, 1, 4, 14, 10, 37, 725, DateTimeKind.Utc).AddTicks(9827L));
		migrationBuilder.UpdateData("IdentityResources", "Id", -1, "Created", new DateTime(2022, 1, 4, 14, 10, 37, 721, DateTimeKind.Utc).AddTicks(8868L));
		migrationBuilder.InsertData("ApiResourceScopes", new string[3] { "Id", "ApiResourceId", "Scope" }, new object[3] { -10, -10, "vps" });
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DeleteData("ApiResourceScopes", "Id", -10);
		migrationBuilder.DeleteData("ApiScopes", "Id", -10);
		migrationBuilder.DeleteData("ApiResources", "Id", -10);
		migrationBuilder.UpdateData("ApiResources", "Id", -9, "Created", new DateTime(2022, 1, 4, 14, 7, 25, 191, DateTimeKind.Utc).AddTicks(9474L));
		migrationBuilder.UpdateData("ApiResources", "Id", -8, "Created", new DateTime(2022, 1, 4, 14, 7, 25, 191, DateTimeKind.Utc).AddTicks(9465L));
		migrationBuilder.UpdateData("ApiResources", "Id", -7, "Created", new DateTime(2022, 1, 4, 14, 7, 25, 191, DateTimeKind.Utc).AddTicks(9457L));
		migrationBuilder.UpdateData("ApiResources", "Id", -6, "Created", new DateTime(2022, 1, 4, 14, 7, 25, 191, DateTimeKind.Utc).AddTicks(9449L));
		migrationBuilder.UpdateData("ApiResources", "Id", -5, "Created", new DateTime(2022, 1, 4, 14, 7, 25, 191, DateTimeKind.Utc).AddTicks(9439L));
		migrationBuilder.UpdateData("ApiResources", "Id", -4, "Created", new DateTime(2022, 1, 4, 14, 7, 25, 191, DateTimeKind.Utc).AddTicks(9369L));
		migrationBuilder.UpdateData("ApiResources", "Id", -3, "Created", new DateTime(2022, 1, 4, 14, 7, 25, 191, DateTimeKind.Utc).AddTicks(9361L));
		migrationBuilder.UpdateData("ApiResources", "Id", -2, "Created", new DateTime(2022, 1, 4, 14, 7, 25, 191, DateTimeKind.Utc).AddTicks(9340L));
		migrationBuilder.UpdateData("ApiResources", "Id", -1, "Created", new DateTime(2022, 1, 4, 14, 7, 25, 191, DateTimeKind.Utc).AddTicks(8659L));
		migrationBuilder.UpdateData("Clients", "Id", -4, "Created", new DateTime(2022, 1, 4, 14, 7, 25, 160, DateTimeKind.Utc).AddTicks(723L));
		migrationBuilder.UpdateData("Clients", "Id", -3, "Created", new DateTime(2022, 1, 4, 14, 7, 25, 160, DateTimeKind.Utc).AddTicks(269L));
		migrationBuilder.UpdateData("Clients", "Id", -2, "Created", new DateTime(2022, 1, 4, 14, 7, 25, 159, DateTimeKind.Utc).AddTicks(9992L));
		migrationBuilder.UpdateData("Clients", "Id", -1, "Created", new DateTime(2022, 1, 4, 14, 7, 25, 158, DateTimeKind.Utc).AddTicks(1064L));
		migrationBuilder.UpdateData("IdentityResources", "Id", -3, "Created", new DateTime(2022, 1, 4, 14, 7, 25, 223, DateTimeKind.Utc).AddTicks(3522L));
		migrationBuilder.UpdateData("IdentityResources", "Id", -2, "Created", new DateTime(2022, 1, 4, 14, 7, 25, 218, DateTimeKind.Utc).AddTicks(1618L));
		migrationBuilder.UpdateData("IdentityResources", "Id", -1, "Created", new DateTime(2022, 1, 4, 14, 7, 25, 211, DateTimeKind.Utc).AddTicks(2170L));
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
				Created = new DateTime(2022, 1, 4, 14, 10, 37, 703, DateTimeKind.Utc).AddTicks(9335L),
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
				Created = new DateTime(2022, 1, 4, 14, 10, 37, 703, DateTimeKind.Utc).AddTicks(9934L),
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
				Created = new DateTime(2022, 1, 4, 14, 10, 37, 703, DateTimeKind.Utc).AddTicks(9953L),
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
				Created = new DateTime(2022, 1, 4, 14, 10, 37, 703, DateTimeKind.Utc).AddTicks(9962L),
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
				Created = new DateTime(2022, 1, 4, 14, 10, 37, 703, DateTimeKind.Utc).AddTicks(9971L),
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
				Created = new DateTime(2022, 1, 4, 14, 10, 37, 703, DateTimeKind.Utc).AddTicks(9979L),
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
				Created = new DateTime(2022, 1, 4, 14, 10, 37, 703, DateTimeKind.Utc).AddTicks(9987L),
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
				Created = new DateTime(2022, 1, 4, 14, 10, 37, 704, DateTimeKind.Utc).AddTicks(80L),
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
				Created = new DateTime(2022, 1, 4, 14, 10, 37, 704, DateTimeKind.Utc).AddTicks(89L),
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
				Created = new DateTime(2022, 1, 4, 14, 10, 37, 704, DateTimeKind.Utc).AddTicks(96L),
				Description = "Api for video processing server",
				DisplayName = "Video Processing Server",
				Enabled = true,
				Name = "vps",
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
			}, new
			{
				Id = -10,
				ApiResourceId = -10,
				Scope = "vps"
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
			}, new
			{
				Id = -10,
				DisplayName = "Video Processing Server Scope",
				Emphasize = false,
				Enabled = true,
				Name = "vps",
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
				Created = new DateTime(2022, 1, 4, 14, 10, 37, 669, DateTimeKind.Utc).AddTicks(8549L),
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
				Created = new DateTime(2022, 1, 4, 14, 10, 37, 671, DateTimeKind.Utc).AddTicks(5084L),
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
				ClientId = "VmsAdminClient",
				ClientName = "VmsAdminClient",
				Created = new DateTime(2022, 1, 4, 14, 10, 37, 671, DateTimeKind.Utc).AddTicks(5326L),
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
				Created = new DateTime(2022, 1, 4, 14, 10, 37, 671, DateTimeKind.Utc).AddTicks(5763L),
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
				Created = new DateTime(2022, 1, 4, 14, 10, 37, 721, DateTimeKind.Utc).AddTicks(8868L),
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
				Created = new DateTime(2022, 1, 4, 14, 10, 37, 725, DateTimeKind.Utc).AddTicks(9827L),
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
				Created = new DateTime(2022, 1, 4, 14, 10, 37, 730, DateTimeKind.Utc).AddTicks(5634L),
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
