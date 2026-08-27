using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VideoOS.IdentityServer.Data.Migrations.IdentityServer.SeededConfigurationDb;

[DbContext(typeof(SeededConfigurationDbContext))]
[Migration("20210122090727_SeedData")]
public class SeedData : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.InsertData("ApiResources", new string[9] { "Id", "Created", "Description", "DisplayName", "Enabled", "LastAccessed", "Name", "NonEditable", "Updated" }, new object[7, 9]
		{
			{
				-1,
				new DateTime(2021, 1, 22, 9, 7, 27, 403, DateTimeKind.Utc).AddTicks(4912L),
				"Api for client registration",
				"Client registration",
				true,
				null,
				"registration",
				false,
				null
			},
			{
				-2,
				new DateTime(2021, 1, 22, 9, 7, 27, 406, DateTimeKind.Utc).AddTicks(9417L),
				"Services for management server",
				"Management server",
				true,
				null,
				"management",
				false,
				null
			},
			{
				-3,
				new DateTime(2021, 1, 22, 9, 7, 27, 406, DateTimeKind.Utc).AddTicks(9450L),
				"Services for recorder server",
				"Recorder server",
				true,
				null,
				"recorder",
				false,
				null
			},
			{
				-4,
				new DateTime(2021, 1, 22, 9, 7, 27, 406, DateTimeKind.Utc).AddTicks(9460L),
				"Management of IDP",
				"IDP Management",
				true,
				null,
				"idp_management",
				false,
				null
			},
			{
				-5,
				new DateTime(2021, 1, 22, 9, 7, 27, 406, DateTimeKind.Utc).AddTicks(9473L),
				"Api for datacollector",
				"Datacollector",
				true,
				null,
				"datacollector",
				false,
				null
			},
			{
				-6,
				new DateTime(2021, 1, 22, 9, 7, 27, 406, DateTimeKind.Utc).AddTicks(9518L),
				"Api for management server",
				"Management Server",
				true,
				null,
				"managementserver",
				false,
				null
			},
			{
				-7,
				new DateTime(2021, 1, 22, 9, 7, 27, 406, DateTimeKind.Utc).AddTicks(9531L),
				"Api for failover response server",
				"Failover Response Server",
				true,
				null,
				"failoverresponseserver",
				false,
				null
			}
		});
		migrationBuilder.InsertData("Clients", new string[42]
		{
			"Id", "AbsoluteRefreshTokenLifetime", "AccessTokenLifetime", "AccessTokenType", "AllowAccessTokensViaBrowser", "AllowOfflineAccess", "AllowPlainTextPkce", "AllowRememberConsent", "AlwaysIncludeUserClaimsInIdToken", "AlwaysSendClientClaims",
			"AuthorizationCodeLifetime", "BackChannelLogoutSessionRequired", "BackChannelLogoutUri", "ClientClaimsPrefix", "ClientId", "ClientName", "ClientUri", "ConsentLifetime", "Created", "Description",
			"DeviceCodeLifetime", "EnableLocalLogin", "Enabled", "FrontChannelLogoutSessionRequired", "FrontChannelLogoutUri", "IdentityTokenLifetime", "IncludeJwtId", "LastAccessed", "LogoUri", "NonEditable",
			"PairWiseSubjectSalt", "ProtocolType", "RefreshTokenExpiration", "RefreshTokenUsage", "RequireClientSecret", "RequireConsent", "RequirePkce", "SlidingRefreshTokenLifetime", "UpdateAccessTokenClaimsOnRefresh", "Updated",
			"UserCodeType", "UserSsoLifetime"
		}, new object[2, 42]
		{
			{
				-1,
				2592000,
				3600,
				0,
				false,
				false,
				false,
				true,
				false,
				false,
				300,
				true,
				null,
				"client_",
				"winauthclient",
				"Registration client",
				null,
				null,
				new DateTime(2021, 1, 22, 9, 7, 27, 369, DateTimeKind.Utc).AddTicks(2487L),
				null,
				300,
				true,
				true,
				true,
				null,
				300,
				false,
				null,
				null,
				false,
				null,
				"oidc",
				1,
				1,
				false,
				true,
				false,
				1296000,
				false,
				null,
				null,
				null
			},
			{
				-2,
				2592000,
				3600,
				0,
				false,
				false,
				false,
				true,
				false,
				false,
				300,
				true,
				null,
				"client_",
				"GrantValidatorClient",
				"GrantValidatorClient",
				null,
				null,
				new DateTime(2021, 1, 22, 9, 7, 27, 372, DateTimeKind.Utc).AddTicks(7465L),
				null,
				300,
				true,
				true,
				true,
				null,
				300,
				false,
				null,
				null,
				false,
				null,
				"oidc",
				1,
				1,
				false,
				false,
				false,
				1296000,
				false,
				null,
				null,
				null
			}
		});
		migrationBuilder.InsertData("IdentityResources", new string[11]
		{
			"Id", "Created", "Description", "DisplayName", "Emphasize", "Enabled", "Name", "NonEditable", "Required", "ShowInDiscoveryDocument",
			"Updated"
		}, new object[3, 11]
		{
			{
				-1,
				new DateTime(2021, 1, 22, 9, 7, 27, 426, DateTimeKind.Utc).AddTicks(1499L),
				null,
				"Your user identifier",
				false,
				true,
				"openid",
				false,
				true,
				true,
				null
			},
			{
				-2,
				new DateTime(2021, 1, 22, 9, 7, 27, 430, DateTimeKind.Utc).AddTicks(9885L),
				"Your user profile information (first name, last name, etc.)",
				"User profile",
				true,
				true,
				"profile",
				false,
				false,
				true,
				null
			},
			{
				-3,
				new DateTime(2021, 1, 22, 9, 7, 27, 435, DateTimeKind.Utc).AddTicks(7309L),
				null,
				"Your email address",
				true,
				true,
				"email",
				false,
				false,
				true,
				null
			}
		});
		migrationBuilder.InsertData("ApiClaims", new string[3] { "Id", "ApiResourceId", "Type" }, new object[3, 3]
		{
			{ -1, -1, "name" },
			{ -2, -4, "name" },
			{ -3, -6, "name" }
		});
		migrationBuilder.InsertData("ApiScopes", new string[8] { "Id", "ApiResourceId", "Description", "DisplayName", "Emphasize", "Name", "Required", "ShowInDiscoveryDocument" }, new object[7, 8]
		{
			{ -1, -1, "Client registration scope", "Client registration scope", false, "write:client", false, true },
			{ -2, -2, "Server wide scope for management services", "Management scope", false, "management", false, true },
			{ -3, -3, "Server wide scope for recorder services", "Recorder scope", false, "recorder", false, true },
			{ -4, -4, "Services for datacollector server", "IDP Management Scope", false, "idp_management", false, true },
			{ -5, -5, "Server wide scope for the datacollector service", "Datacollector Scope", false, "datacollector", false, true },
			{ -6, -6, "Server wide scope for the management server in IIS", "Management server Scope", false, "managementserver", false, true },
			{ -7, -7, "Server wide scope for failover response server in recording server", "Failover response server Scope", false, "failoverresponseserver", false, true }
		});
		migrationBuilder.InsertData("ClientGrantTypes", new string[3] { "Id", "ClientId", "GrantType" }, new object[3, 3]
		{
			{ -3, -2, "password" },
			{ -2, -2, "windows_credentials" },
			{ -1, -1, "windows_auth" }
		});
		migrationBuilder.InsertData("ClientScopes", new string[3] { "Id", "ClientId", "Scope" }, new object[2, 3]
		{
			{ -2, -2, "managementserver" },
			{ -1, -1, "write:client" }
		});
		migrationBuilder.InsertData("IdentityClaims", new string[3] { "Id", "IdentityResourceId", "Type" }, new object[17, 3]
		{
			{ -10, -2, "website" },
			{ -15, -2, "updated_at" },
			{ -14, -2, "locale" },
			{ -13, -2, "zoneinfo" },
			{ -12, -2, "birthdate" },
			{ -11, -2, "gender" },
			{ -9, -2, "picture" },
			{ -1, -1, "sub" },
			{ -7, -2, "preferred_username" },
			{ -6, -2, "nickname" },
			{ -5, -2, "middle_name" },
			{ -4, -2, "given_name" },
			{ -3, -2, "family_name" },
			{ -2, -2, "name" },
			{ -16, -3, "email" },
			{ -8, -2, "profile" },
			{ -17, -3, "email_verified" }
		});
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DeleteData("ApiClaims", "Id", -3);
		migrationBuilder.DeleteData("ApiClaims", "Id", -2);
		migrationBuilder.DeleteData("ApiClaims", "Id", -1);
		migrationBuilder.DeleteData("ApiScopes", "Id", -7);
		migrationBuilder.DeleteData("ApiScopes", "Id", -6);
		migrationBuilder.DeleteData("ApiScopes", "Id", -5);
		migrationBuilder.DeleteData("ApiScopes", "Id", -4);
		migrationBuilder.DeleteData("ApiScopes", "Id", -3);
		migrationBuilder.DeleteData("ApiScopes", "Id", -2);
		migrationBuilder.DeleteData("ApiScopes", "Id", -1);
		migrationBuilder.DeleteData("ClientGrantTypes", "Id", -3);
		migrationBuilder.DeleteData("ClientGrantTypes", "Id", -2);
		migrationBuilder.DeleteData("ClientGrantTypes", "Id", -1);
		migrationBuilder.DeleteData("ClientScopes", "Id", -2);
		migrationBuilder.DeleteData("ClientScopes", "Id", -1);
		migrationBuilder.DeleteData("IdentityClaims", "Id", -17);
		migrationBuilder.DeleteData("IdentityClaims", "Id", -16);
		migrationBuilder.DeleteData("IdentityClaims", "Id", -15);
		migrationBuilder.DeleteData("IdentityClaims", "Id", -14);
		migrationBuilder.DeleteData("IdentityClaims", "Id", -13);
		migrationBuilder.DeleteData("IdentityClaims", "Id", -12);
		migrationBuilder.DeleteData("IdentityClaims", "Id", -11);
		migrationBuilder.DeleteData("IdentityClaims", "Id", -10);
		migrationBuilder.DeleteData("IdentityClaims", "Id", -9);
		migrationBuilder.DeleteData("IdentityClaims", "Id", -8);
		migrationBuilder.DeleteData("IdentityClaims", "Id", -7);
		migrationBuilder.DeleteData("IdentityClaims", "Id", -6);
		migrationBuilder.DeleteData("IdentityClaims", "Id", -5);
		migrationBuilder.DeleteData("IdentityClaims", "Id", -4);
		migrationBuilder.DeleteData("IdentityClaims", "Id", -3);
		migrationBuilder.DeleteData("IdentityClaims", "Id", -2);
		migrationBuilder.DeleteData("IdentityClaims", "Id", -1);
		migrationBuilder.DeleteData("ApiResources", "Id", -7);
		migrationBuilder.DeleteData("ApiResources", "Id", -6);
		migrationBuilder.DeleteData("ApiResources", "Id", -5);
		migrationBuilder.DeleteData("ApiResources", "Id", -4);
		migrationBuilder.DeleteData("ApiResources", "Id", -3);
		migrationBuilder.DeleteData("ApiResources", "Id", -2);
		migrationBuilder.DeleteData("ApiResources", "Id", -1);
		migrationBuilder.DeleteData("Clients", "Id", -2);
		migrationBuilder.DeleteData("Clients", "Id", -1);
		migrationBuilder.DeleteData("IdentityResources", "Id", -3);
		migrationBuilder.DeleteData("IdentityResources", "Id", -2);
		migrationBuilder.DeleteData("IdentityResources", "Id", -1);
	}

	protected override void BuildTargetModel(ModelBuilder modelBuilder)
	{
		modelBuilder.HasAnnotation("ProductVersion", "3.1.5").HasAnnotation("Relational:MaxIdentifierLength", 128).HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
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
			b.HasData(new
			{
				Id = -1,
				Created = new DateTime(2021, 1, 22, 9, 7, 27, 403, DateTimeKind.Utc).AddTicks(4912L),
				Description = "Api for client registration",
				DisplayName = "Client registration",
				Enabled = true,
				Name = "registration",
				NonEditable = false
			}, new
			{
				Id = -2,
				Created = new DateTime(2021, 1, 22, 9, 7, 27, 406, DateTimeKind.Utc).AddTicks(9417L),
				Description = "Services for management server",
				DisplayName = "Management server",
				Enabled = true,
				Name = "management",
				NonEditable = false
			}, new
			{
				Id = -3,
				Created = new DateTime(2021, 1, 22, 9, 7, 27, 406, DateTimeKind.Utc).AddTicks(9450L),
				Description = "Services for recorder server",
				DisplayName = "Recorder server",
				Enabled = true,
				Name = "recorder",
				NonEditable = false
			}, new
			{
				Id = -4,
				Created = new DateTime(2021, 1, 22, 9, 7, 27, 406, DateTimeKind.Utc).AddTicks(9460L),
				Description = "Management of IDP",
				DisplayName = "IDP Management",
				Enabled = true,
				Name = "idp_management",
				NonEditable = false
			}, new
			{
				Id = -5,
				Created = new DateTime(2021, 1, 22, 9, 7, 27, 406, DateTimeKind.Utc).AddTicks(9473L),
				Description = "Api for datacollector",
				DisplayName = "Datacollector",
				Enabled = true,
				Name = "datacollector",
				NonEditable = false
			}, new
			{
				Id = -6,
				Created = new DateTime(2021, 1, 22, 9, 7, 27, 406, DateTimeKind.Utc).AddTicks(9518L),
				Description = "Api for management server",
				DisplayName = "Management Server",
				Enabled = true,
				Name = "managementserver",
				NonEditable = false
			}, new
			{
				Id = -7,
				Created = new DateTime(2021, 1, 22, 9, 7, 27, 406, DateTimeKind.Utc).AddTicks(9531L),
				Description = "Api for failover response server",
				DisplayName = "Failover Response Server",
				Enabled = true,
				Name = "failoverresponseserver",
				NonEditable = false
			});
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
			b.HasData(new
			{
				Id = -1,
				ApiResourceId = -1,
				Description = "Client registration scope",
				DisplayName = "Client registration scope",
				Emphasize = false,
				Name = "write:client",
				Required = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -2,
				ApiResourceId = -2,
				Description = "Server wide scope for management services",
				DisplayName = "Management scope",
				Emphasize = false,
				Name = "management",
				Required = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -3,
				ApiResourceId = -3,
				Description = "Server wide scope for recorder services",
				DisplayName = "Recorder scope",
				Emphasize = false,
				Name = "recorder",
				Required = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -4,
				ApiResourceId = -4,
				Description = "Services for datacollector server",
				DisplayName = "IDP Management Scope",
				Emphasize = false,
				Name = "idp_management",
				Required = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -5,
				ApiResourceId = -5,
				Description = "Server wide scope for the datacollector service",
				DisplayName = "Datacollector Scope",
				Emphasize = false,
				Name = "datacollector",
				Required = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -6,
				ApiResourceId = -6,
				Description = "Server wide scope for the management server in IIS",
				DisplayName = "Management server Scope",
				Emphasize = false,
				Name = "managementserver",
				Required = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -7,
				ApiResourceId = -7,
				Description = "Server wide scope for failover response server in recording server",
				DisplayName = "Failover response server Scope",
				Emphasize = false,
				Name = "failoverresponseserver",
				Required = false,
				ShowInDiscoveryDocument = true
			});
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
				Created = new DateTime(2021, 1, 22, 9, 7, 27, 369, DateTimeKind.Utc).AddTicks(2487L),
				DeviceCodeLifetime = 300,
				EnableLocalLogin = true,
				Enabled = true,
				FrontChannelLogoutSessionRequired = true,
				IdentityTokenLifetime = 300,
				IncludeJwtId = false,
				NonEditable = false,
				ProtocolType = "oidc",
				RefreshTokenExpiration = 1,
				RefreshTokenUsage = 1,
				RequireClientSecret = false,
				RequireConsent = true,
				RequirePkce = false,
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
				Created = new DateTime(2021, 1, 22, 9, 7, 27, 372, DateTimeKind.Utc).AddTicks(7465L),
				DeviceCodeLifetime = 300,
				EnableLocalLogin = true,
				Enabled = true,
				FrontChannelLogoutSessionRequired = true,
				IdentityTokenLifetime = 300,
				IncludeJwtId = false,
				NonEditable = false,
				ProtocolType = "oidc",
				RefreshTokenExpiration = 1,
				RefreshTokenUsage = 1,
				RequireClientSecret = false,
				RequireConsent = false,
				RequirePkce = false,
				SlidingRefreshTokenLifetime = 1296000,
				UpdateAccessTokenClaimsOnRefresh = false
			});
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
			b.HasData(new
			{
				Id = -1,
				Created = new DateTime(2021, 1, 22, 9, 7, 27, 426, DateTimeKind.Utc).AddTicks(1499L),
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
				Created = new DateTime(2021, 1, 22, 9, 7, 27, 430, DateTimeKind.Utc).AddTicks(9885L),
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
				Created = new DateTime(2021, 1, 22, 9, 7, 27, 435, DateTimeKind.Utc).AddTicks(7309L),
				DisplayName = "Your email address",
				Emphasize = true,
				Enabled = true,
				Name = "email",
				NonEditable = false,
				Required = false,
				ShowInDiscoveryDocument = true
			});
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
