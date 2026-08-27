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
[Migration("20191017180223_InitialIdentityServerConfigurationDbMigration")]
public class InitialIdentityServerConfigurationDbMigration : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable("ApiResources", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id = table.Column<int>().Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			OperationBuilder<AddColumnOperation> enabled = table.Column<bool>();
			int? maxLength = 200;
			OperationBuilder<AddColumnOperation> name = table.Column<string>(null, null, maxLength);
			maxLength = 200;
			OperationBuilder<AddColumnOperation> displayName = table.Column<string>(null, null, maxLength, rowVersion: false, null, nullable: true);
			maxLength = 1000;
			return new
			{
				Id = id,
				Enabled = enabled,
				Name = name,
				DisplayName = displayName,
				Description = table.Column<string>(null, null, maxLength, rowVersion: false, null, nullable: true)
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_ApiResources", x => x.Id);
		});
		migrationBuilder.CreateTable("Clients", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id = table.Column<int>().Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			OperationBuilder<AddColumnOperation> enabled = table.Column<bool>();
			int? maxLength = 200;
			OperationBuilder<AddColumnOperation> clientId = table.Column<string>(null, null, maxLength);
			maxLength = 200;
			OperationBuilder<AddColumnOperation> protocolType = table.Column<string>(null, null, maxLength);
			OperationBuilder<AddColumnOperation> requireClientSecret = table.Column<bool>();
			maxLength = 200;
			OperationBuilder<AddColumnOperation> clientName = table.Column<string>(null, null, maxLength, rowVersion: false, null, nullable: true);
			maxLength = 1000;
			OperationBuilder<AddColumnOperation> description = table.Column<string>(null, null, maxLength, rowVersion: false, null, nullable: true);
			maxLength = 2000;
			OperationBuilder<AddColumnOperation> clientUri = table.Column<string>(null, null, maxLength, rowVersion: false, null, nullable: true);
			maxLength = 2000;
			OperationBuilder<AddColumnOperation> logoUri = table.Column<string>(null, null, maxLength, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> requireConsent = table.Column<bool>();
			OperationBuilder<AddColumnOperation> allowRememberConsent = table.Column<bool>();
			OperationBuilder<AddColumnOperation> alwaysIncludeUserClaimsInIdToken = table.Column<bool>();
			OperationBuilder<AddColumnOperation> requirePkce = table.Column<bool>();
			OperationBuilder<AddColumnOperation> allowPlainTextPkce = table.Column<bool>();
			OperationBuilder<AddColumnOperation> allowAccessTokensViaBrowser = table.Column<bool>();
			maxLength = 2000;
			OperationBuilder<AddColumnOperation> frontChannelLogoutUri = table.Column<string>(null, null, maxLength, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> frontChannelLogoutSessionRequired = table.Column<bool>();
			maxLength = 2000;
			OperationBuilder<AddColumnOperation> backChannelLogoutUri = table.Column<string>(null, null, maxLength, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> backChannelLogoutSessionRequired = table.Column<bool>();
			OperationBuilder<AddColumnOperation> allowOfflineAccess = table.Column<bool>();
			OperationBuilder<AddColumnOperation> identityTokenLifetime = table.Column<int>();
			OperationBuilder<AddColumnOperation> accessTokenLifetime = table.Column<int>();
			OperationBuilder<AddColumnOperation> authorizationCodeLifetime = table.Column<int>();
			OperationBuilder<AddColumnOperation> consentLifetime = table.Column<int>(null, null, null, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> absoluteRefreshTokenLifetime = table.Column<int>();
			OperationBuilder<AddColumnOperation> slidingRefreshTokenLifetime = table.Column<int>();
			OperationBuilder<AddColumnOperation> refreshTokenUsage = table.Column<int>();
			OperationBuilder<AddColumnOperation> updateAccessTokenClaimsOnRefresh = table.Column<bool>();
			OperationBuilder<AddColumnOperation> refreshTokenExpiration = table.Column<int>();
			OperationBuilder<AddColumnOperation> accessTokenType = table.Column<int>();
			OperationBuilder<AddColumnOperation> enableLocalLogin = table.Column<bool>();
			OperationBuilder<AddColumnOperation> includeJwtId = table.Column<bool>();
			OperationBuilder<AddColumnOperation> alwaysSendClientClaims = table.Column<bool>();
			maxLength = 200;
			OperationBuilder<AddColumnOperation> clientClaimsPrefix = table.Column<string>(null, null, maxLength, rowVersion: false, null, nullable: true);
			maxLength = 200;
			return new
			{
				Id = id,
				Enabled = enabled,
				ClientId = clientId,
				ProtocolType = protocolType,
				RequireClientSecret = requireClientSecret,
				ClientName = clientName,
				Description = description,
				ClientUri = clientUri,
				LogoUri = logoUri,
				RequireConsent = requireConsent,
				AllowRememberConsent = allowRememberConsent,
				AlwaysIncludeUserClaimsInIdToken = alwaysIncludeUserClaimsInIdToken,
				RequirePkce = requirePkce,
				AllowPlainTextPkce = allowPlainTextPkce,
				AllowAccessTokensViaBrowser = allowAccessTokensViaBrowser,
				FrontChannelLogoutUri = frontChannelLogoutUri,
				FrontChannelLogoutSessionRequired = frontChannelLogoutSessionRequired,
				BackChannelLogoutUri = backChannelLogoutUri,
				BackChannelLogoutSessionRequired = backChannelLogoutSessionRequired,
				AllowOfflineAccess = allowOfflineAccess,
				IdentityTokenLifetime = identityTokenLifetime,
				AccessTokenLifetime = accessTokenLifetime,
				AuthorizationCodeLifetime = authorizationCodeLifetime,
				ConsentLifetime = consentLifetime,
				AbsoluteRefreshTokenLifetime = absoluteRefreshTokenLifetime,
				SlidingRefreshTokenLifetime = slidingRefreshTokenLifetime,
				RefreshTokenUsage = refreshTokenUsage,
				UpdateAccessTokenClaimsOnRefresh = updateAccessTokenClaimsOnRefresh,
				RefreshTokenExpiration = refreshTokenExpiration,
				AccessTokenType = accessTokenType,
				EnableLocalLogin = enableLocalLogin,
				IncludeJwtId = includeJwtId,
				AlwaysSendClientClaims = alwaysSendClientClaims,
				ClientClaimsPrefix = clientClaimsPrefix,
				PairWiseSubjectSalt = table.Column<string>(null, null, maxLength, rowVersion: false, null, nullable: true)
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_Clients", x => x.Id);
		});
		migrationBuilder.CreateTable("IdentityResources", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id = table.Column<int>().Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			OperationBuilder<AddColumnOperation> enabled = table.Column<bool>();
			int? maxLength = 200;
			OperationBuilder<AddColumnOperation> name = table.Column<string>(null, null, maxLength);
			maxLength = 200;
			OperationBuilder<AddColumnOperation> displayName = table.Column<string>(null, null, maxLength, rowVersion: false, null, nullable: true);
			maxLength = 1000;
			return new
			{
				Id = id,
				Enabled = enabled,
				Name = name,
				DisplayName = displayName,
				Description = table.Column<string>(null, null, maxLength, rowVersion: false, null, nullable: true),
				Required = table.Column<bool>(),
				Emphasize = table.Column<bool>(),
				ShowInDiscoveryDocument = table.Column<bool>()
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_IdentityResources", x => x.Id);
		});
		migrationBuilder.CreateTable("ApiClaims", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id = table.Column<int>().Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			int? maxLength = 200;
			return new
			{
				Id = id,
				Type = table.Column<string>(null, null, maxLength),
				ApiResourceId = table.Column<int>()
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_ApiClaims", x => x.Id);
			table.ForeignKey("FK_ApiClaims_ApiResources_ApiResourceId", x => x.ApiResourceId, "ApiResources", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("ApiScopes", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id = table.Column<int>().Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			int? maxLength = 200;
			OperationBuilder<AddColumnOperation> name = table.Column<string>(null, null, maxLength);
			maxLength = 200;
			OperationBuilder<AddColumnOperation> displayName = table.Column<string>(null, null, maxLength, rowVersion: false, null, nullable: true);
			maxLength = 1000;
			return new
			{
				Id = id,
				Name = name,
				DisplayName = displayName,
				Description = table.Column<string>(null, null, maxLength, rowVersion: false, null, nullable: true),
				Required = table.Column<bool>(),
				Emphasize = table.Column<bool>(),
				ShowInDiscoveryDocument = table.Column<bool>(),
				ApiResourceId = table.Column<int>()
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_ApiScopes", x => x.Id);
			table.ForeignKey("FK_ApiScopes_ApiResources_ApiResourceId", x => x.ApiResourceId, "ApiResources", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("ApiSecrets", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id = table.Column<int>().Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			int? maxLength = 1000;
			OperationBuilder<AddColumnOperation> description = table.Column<string>(null, null, maxLength, rowVersion: false, null, nullable: true);
			maxLength = 2000;
			OperationBuilder<AddColumnOperation> value = table.Column<string>(null, null, maxLength, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> expiration = table.Column<DateTime>(null, null, null, rowVersion: false, null, nullable: true);
			maxLength = 250;
			return new
			{
				Id = id,
				Description = description,
				Value = value,
				Expiration = expiration,
				Type = table.Column<string>(null, null, maxLength, rowVersion: false, null, nullable: true),
				ApiResourceId = table.Column<int>()
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_ApiSecrets", x => x.Id);
			table.ForeignKey("FK_ApiSecrets_ApiResources_ApiResourceId", x => x.ApiResourceId, "ApiResources", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("ClientClaims", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id = table.Column<int>().Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			int? maxLength = 250;
			OperationBuilder<AddColumnOperation> type = table.Column<string>(null, null, maxLength);
			maxLength = 250;
			return new
			{
				Id = id,
				Type = type,
				Value = table.Column<string>(null, null, maxLength),
				ClientId = table.Column<int>()
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_ClientClaims", x => x.Id);
			table.ForeignKey("FK_ClientClaims_Clients_ClientId", x => x.ClientId, "Clients", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("ClientCorsOrigins", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id = table.Column<int>().Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			int? maxLength = 150;
			return new
			{
				Id = id,
				Origin = table.Column<string>(null, null, maxLength),
				ClientId = table.Column<int>()
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_ClientCorsOrigins", x => x.Id);
			table.ForeignKey("FK_ClientCorsOrigins_Clients_ClientId", x => x.ClientId, "Clients", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("ClientGrantTypes", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id = table.Column<int>().Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			int? maxLength = 250;
			return new
			{
				Id = id,
				GrantType = table.Column<string>(null, null, maxLength),
				ClientId = table.Column<int>()
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_ClientGrantTypes", x => x.Id);
			table.ForeignKey("FK_ClientGrantTypes_Clients_ClientId", x => x.ClientId, "Clients", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("ClientIdPRestrictions", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id = table.Column<int>().Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			int? maxLength = 200;
			return new
			{
				Id = id,
				Provider = table.Column<string>(null, null, maxLength),
				ClientId = table.Column<int>()
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_ClientIdPRestrictions", x => x.Id);
			table.ForeignKey("FK_ClientIdPRestrictions_Clients_ClientId", x => x.ClientId, "Clients", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("ClientPostLogoutRedirectUris", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id = table.Column<int>().Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			int? maxLength = 2000;
			return new
			{
				Id = id,
				PostLogoutRedirectUri = table.Column<string>(null, null, maxLength),
				ClientId = table.Column<int>()
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_ClientPostLogoutRedirectUris", x => x.Id);
			table.ForeignKey("FK_ClientPostLogoutRedirectUris_Clients_ClientId", x => x.ClientId, "Clients", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("ClientProperties", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id = table.Column<int>().Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			int? maxLength = 250;
			OperationBuilder<AddColumnOperation> key = table.Column<string>(null, null, maxLength);
			maxLength = 2000;
			return new
			{
				Id = id,
				Key = key,
				Value = table.Column<string>(null, null, maxLength),
				ClientId = table.Column<int>()
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_ClientProperties", x => x.Id);
			table.ForeignKey("FK_ClientProperties_Clients_ClientId", x => x.ClientId, "Clients", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("ClientRedirectUris", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id = table.Column<int>().Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			int? maxLength = 2000;
			return new
			{
				Id = id,
				RedirectUri = table.Column<string>(null, null, maxLength),
				ClientId = table.Column<int>()
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_ClientRedirectUris", x => x.Id);
			table.ForeignKey("FK_ClientRedirectUris_Clients_ClientId", x => x.ClientId, "Clients", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("ClientScopes", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id = table.Column<int>().Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			int? maxLength = 200;
			return new
			{
				Id = id,
				Scope = table.Column<string>(null, null, maxLength),
				ClientId = table.Column<int>()
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_ClientScopes", x => x.Id);
			table.ForeignKey("FK_ClientScopes_Clients_ClientId", x => x.ClientId, "Clients", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("ClientSecrets", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id = table.Column<int>().Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			int? maxLength = 2000;
			OperationBuilder<AddColumnOperation> description = table.Column<string>(null, null, maxLength, rowVersion: false, null, nullable: true);
			maxLength = 2000;
			OperationBuilder<AddColumnOperation> value = table.Column<string>(null, null, maxLength);
			OperationBuilder<AddColumnOperation> expiration = table.Column<DateTime>(null, null, null, rowVersion: false, null, nullable: true);
			maxLength = 250;
			return new
			{
				Id = id,
				Description = description,
				Value = value,
				Expiration = expiration,
				Type = table.Column<string>(null, null, maxLength, rowVersion: false, null, nullable: true),
				ClientId = table.Column<int>()
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_ClientSecrets", x => x.Id);
			table.ForeignKey("FK_ClientSecrets_Clients_ClientId", x => x.ClientId, "Clients", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("IdentityClaims", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id = table.Column<int>().Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			int? maxLength = 200;
			return new
			{
				Id = id,
				Type = table.Column<string>(null, null, maxLength),
				IdentityResourceId = table.Column<int>()
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_IdentityClaims", x => x.Id);
			table.ForeignKey("FK_IdentityClaims_IdentityResources_IdentityResourceId", x => x.IdentityResourceId, "IdentityResources", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("ApiScopeClaims", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id = table.Column<int>().Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			int? maxLength = 200;
			return new
			{
				Id = id,
				Type = table.Column<string>(null, null, maxLength),
				ApiScopeId = table.Column<int>()
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_ApiScopeClaims", x => x.Id);
			table.ForeignKey("FK_ApiScopeClaims_ApiScopes_ApiScopeId", x => x.ApiScopeId, "ApiScopes", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateIndex("IX_ApiClaims_ApiResourceId", "ApiClaims", "ApiResourceId");
		migrationBuilder.CreateIndex("IX_ApiResources_Name", "ApiResources", "Name", null, unique: true);
		migrationBuilder.CreateIndex("IX_ApiScopeClaims_ApiScopeId", "ApiScopeClaims", "ApiScopeId");
		migrationBuilder.CreateIndex("IX_ApiScopes_ApiResourceId", "ApiScopes", "ApiResourceId");
		migrationBuilder.CreateIndex("IX_ApiScopes_Name", "ApiScopes", "Name", null, unique: true);
		migrationBuilder.CreateIndex("IX_ApiSecrets_ApiResourceId", "ApiSecrets", "ApiResourceId");
		migrationBuilder.CreateIndex("IX_ClientClaims_ClientId", "ClientClaims", "ClientId");
		migrationBuilder.CreateIndex("IX_ClientCorsOrigins_ClientId", "ClientCorsOrigins", "ClientId");
		migrationBuilder.CreateIndex("IX_ClientGrantTypes_ClientId", "ClientGrantTypes", "ClientId");
		migrationBuilder.CreateIndex("IX_ClientIdPRestrictions_ClientId", "ClientIdPRestrictions", "ClientId");
		migrationBuilder.CreateIndex("IX_ClientPostLogoutRedirectUris_ClientId", "ClientPostLogoutRedirectUris", "ClientId");
		migrationBuilder.CreateIndex("IX_ClientProperties_ClientId", "ClientProperties", "ClientId");
		migrationBuilder.CreateIndex("IX_ClientRedirectUris_ClientId", "ClientRedirectUris", "ClientId");
		migrationBuilder.CreateIndex("IX_Clients_ClientId", "Clients", "ClientId", null, unique: true);
		migrationBuilder.CreateIndex("IX_ClientScopes_ClientId", "ClientScopes", "ClientId");
		migrationBuilder.CreateIndex("IX_ClientSecrets_ClientId", "ClientSecrets", "ClientId");
		migrationBuilder.CreateIndex("IX_IdentityClaims_IdentityResourceId", "IdentityClaims", "IdentityResourceId");
		migrationBuilder.CreateIndex("IX_IdentityResources_Name", "IdentityResources", "Name", null, unique: true);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable("ApiClaims");
		migrationBuilder.DropTable("ApiScopeClaims");
		migrationBuilder.DropTable("ApiSecrets");
		migrationBuilder.DropTable("ClientClaims");
		migrationBuilder.DropTable("ClientCorsOrigins");
		migrationBuilder.DropTable("ClientGrantTypes");
		migrationBuilder.DropTable("ClientIdPRestrictions");
		migrationBuilder.DropTable("ClientPostLogoutRedirectUris");
		migrationBuilder.DropTable("ClientProperties");
		migrationBuilder.DropTable("ClientRedirectUris");
		migrationBuilder.DropTable("ClientScopes");
		migrationBuilder.DropTable("ClientSecrets");
		migrationBuilder.DropTable("IdentityClaims");
		migrationBuilder.DropTable("ApiScopes");
		migrationBuilder.DropTable("Clients");
		migrationBuilder.DropTable("IdentityResources");
		migrationBuilder.DropTable("ApiResources");
	}

	protected override void BuildTargetModel(ModelBuilder modelBuilder)
	{
		modelBuilder.HasAnnotation("ProductVersion", "2.2.3-servicing-35854").HasAnnotation("Relational:MaxIdentifierLength", 128).HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ApiResource", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<string>("Description").HasMaxLength(1000);
			b.Property<string>("DisplayName").HasMaxLength(200);
			b.Property<bool>("Enabled");
			b.Property<string>("Name").IsRequired().HasMaxLength(200);
			b.HasKey("Id");
			b.HasIndex("Name").IsUnique();
			b.ToTable("ApiResources");
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ApiResourceClaim", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<int>("ApiResourceId");
			b.Property<string>("Type").IsRequired().HasMaxLength(200);
			b.HasKey("Id");
			b.HasIndex("ApiResourceId");
			b.ToTable("ApiClaims");
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ApiScope", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<int>("ApiResourceId");
			b.Property<string>("Description").HasMaxLength(1000);
			b.Property<string>("DisplayName").HasMaxLength(200);
			b.Property<bool>("Emphasize");
			b.Property<string>("Name").IsRequired().HasMaxLength(200);
			b.Property<bool>("Required");
			b.Property<bool>("ShowInDiscoveryDocument");
			b.HasKey("Id");
			b.HasIndex("ApiResourceId");
			b.HasIndex("Name").IsUnique();
			b.ToTable("ApiScopes");
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ApiScopeClaim", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<int>("ApiScopeId");
			b.Property<string>("Type").IsRequired().HasMaxLength(200);
			b.HasKey("Id");
			b.HasIndex("ApiScopeId");
			b.ToTable("ApiScopeClaims");
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ApiSecret", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<int>("ApiResourceId");
			b.Property<string>("Description").HasMaxLength(1000);
			b.Property<DateTime?>("Expiration");
			b.Property<string>("Type").HasMaxLength(250);
			b.Property<string>("Value").HasMaxLength(2000);
			b.HasKey("Id");
			b.HasIndex("ApiResourceId");
			b.ToTable("ApiSecrets");
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.Client", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<int>("AbsoluteRefreshTokenLifetime");
			b.Property<int>("AccessTokenLifetime");
			b.Property<int>("AccessTokenType");
			b.Property<bool>("AllowAccessTokensViaBrowser");
			b.Property<bool>("AllowOfflineAccess");
			b.Property<bool>("AllowPlainTextPkce");
			b.Property<bool>("AllowRememberConsent");
			b.Property<bool>("AlwaysIncludeUserClaimsInIdToken");
			b.Property<bool>("AlwaysSendClientClaims");
			b.Property<int>("AuthorizationCodeLifetime");
			b.Property<bool>("BackChannelLogoutSessionRequired");
			b.Property<string>("BackChannelLogoutUri").HasMaxLength(2000);
			b.Property<string>("ClientClaimsPrefix").HasMaxLength(200);
			b.Property<string>("ClientId").IsRequired().HasMaxLength(200);
			b.Property<string>("ClientName").HasMaxLength(200);
			b.Property<string>("ClientUri").HasMaxLength(2000);
			b.Property<int?>("ConsentLifetime");
			b.Property<string>("Description").HasMaxLength(1000);
			b.Property<bool>("EnableLocalLogin");
			b.Property<bool>("Enabled");
			b.Property<bool>("FrontChannelLogoutSessionRequired");
			b.Property<string>("FrontChannelLogoutUri").HasMaxLength(2000);
			b.Property<int>("IdentityTokenLifetime");
			b.Property<bool>("IncludeJwtId");
			b.Property<string>("LogoUri").HasMaxLength(2000);
			b.Property<string>("PairWiseSubjectSalt").HasMaxLength(200);
			b.Property<string>("ProtocolType").IsRequired().HasMaxLength(200);
			b.Property<int>("RefreshTokenExpiration");
			b.Property<int>("RefreshTokenUsage");
			b.Property<bool>("RequireClientSecret");
			b.Property<bool>("RequireConsent");
			b.Property<bool>("RequirePkce");
			b.Property<int>("SlidingRefreshTokenLifetime");
			b.Property<bool>("UpdateAccessTokenClaimsOnRefresh");
			b.HasKey("Id");
			b.HasIndex("ClientId").IsUnique();
			b.ToTable("Clients");
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientClaim", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<int>("ClientId");
			b.Property<string>("Type").IsRequired().HasMaxLength(250);
			b.Property<string>("Value").IsRequired().HasMaxLength(250);
			b.HasKey("Id");
			b.HasIndex("ClientId");
			b.ToTable("ClientClaims");
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientCorsOrigin", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<int>("ClientId");
			b.Property<string>("Origin").IsRequired().HasMaxLength(150);
			b.HasKey("Id");
			b.HasIndex("ClientId");
			b.ToTable("ClientCorsOrigins");
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientGrantType", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<int>("ClientId");
			b.Property<string>("GrantType").IsRequired().HasMaxLength(250);
			b.HasKey("Id");
			b.HasIndex("ClientId");
			b.ToTable("ClientGrantTypes");
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientIdPRestriction", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<int>("ClientId");
			b.Property<string>("Provider").IsRequired().HasMaxLength(200);
			b.HasKey("Id");
			b.HasIndex("ClientId");
			b.ToTable("ClientIdPRestrictions");
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientPostLogoutRedirectUri", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<int>("ClientId");
			b.Property<string>("PostLogoutRedirectUri").IsRequired().HasMaxLength(2000);
			b.HasKey("Id");
			b.HasIndex("ClientId");
			b.ToTable("ClientPostLogoutRedirectUris");
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientProperty", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<int>("ClientId");
			b.Property<string>("Key").IsRequired().HasMaxLength(250);
			b.Property<string>("Value").IsRequired().HasMaxLength(2000);
			b.HasKey("Id");
			b.HasIndex("ClientId");
			b.ToTable("ClientProperties");
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientRedirectUri", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<int>("ClientId");
			b.Property<string>("RedirectUri").IsRequired().HasMaxLength(2000);
			b.HasKey("Id");
			b.HasIndex("ClientId");
			b.ToTable("ClientRedirectUris");
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientScope", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<int>("ClientId");
			b.Property<string>("Scope").IsRequired().HasMaxLength(200);
			b.HasKey("Id");
			b.HasIndex("ClientId");
			b.ToTable("ClientScopes");
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientSecret", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<int>("ClientId");
			b.Property<string>("Description").HasMaxLength(2000);
			b.Property<DateTime?>("Expiration");
			b.Property<string>("Type").HasMaxLength(250);
			b.Property<string>("Value").IsRequired().HasMaxLength(2000);
			b.HasKey("Id");
			b.HasIndex("ClientId");
			b.ToTable("ClientSecrets");
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.IdentityClaim", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<int>("IdentityResourceId");
			b.Property<string>("Type").IsRequired().HasMaxLength(200);
			b.HasKey("Id");
			b.HasIndex("IdentityResourceId");
			b.ToTable("IdentityClaims");
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.IdentityResource", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("Id").ValueGeneratedOnAdd().HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
			b.Property<string>("Description").HasMaxLength(1000);
			b.Property<string>("DisplayName").HasMaxLength(200);
			b.Property<bool>("Emphasize");
			b.Property<bool>("Enabled");
			b.Property<string>("Name").IsRequired().HasMaxLength(200);
			b.Property<bool>("Required");
			b.Property<bool>("ShowInDiscoveryDocument");
			b.HasKey("Id");
			b.HasIndex("Name").IsUnique();
			b.ToTable("IdentityResources");
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ApiResourceClaim", delegate(EntityTypeBuilder b)
		{
			b.HasOne("IdentityServer4.EntityFramework.Entities.ApiResource", "ApiResource").WithMany("UserClaims").HasForeignKey("ApiResourceId")
				.OnDelete(DeleteBehavior.Cascade);
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ApiScope", delegate(EntityTypeBuilder b)
		{
			b.HasOne("IdentityServer4.EntityFramework.Entities.ApiResource", "ApiResource").WithMany("Scopes").HasForeignKey("ApiResourceId")
				.OnDelete(DeleteBehavior.Cascade);
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ApiScopeClaim", delegate(EntityTypeBuilder b)
		{
			b.HasOne("IdentityServer4.EntityFramework.Entities.ApiScope", "ApiScope").WithMany("UserClaims").HasForeignKey("ApiScopeId")
				.OnDelete(DeleteBehavior.Cascade);
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ApiSecret", delegate(EntityTypeBuilder b)
		{
			b.HasOne("IdentityServer4.EntityFramework.Entities.ApiResource", "ApiResource").WithMany("Secrets").HasForeignKey("ApiResourceId")
				.OnDelete(DeleteBehavior.Cascade);
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientClaim", delegate(EntityTypeBuilder b)
		{
			b.HasOne("IdentityServer4.EntityFramework.Entities.Client", "Client").WithMany("Claims").HasForeignKey("ClientId")
				.OnDelete(DeleteBehavior.Cascade);
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientCorsOrigin", delegate(EntityTypeBuilder b)
		{
			b.HasOne("IdentityServer4.EntityFramework.Entities.Client", "Client").WithMany("AllowedCorsOrigins").HasForeignKey("ClientId")
				.OnDelete(DeleteBehavior.Cascade);
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientGrantType", delegate(EntityTypeBuilder b)
		{
			b.HasOne("IdentityServer4.EntityFramework.Entities.Client", "Client").WithMany("AllowedGrantTypes").HasForeignKey("ClientId")
				.OnDelete(DeleteBehavior.Cascade);
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientIdPRestriction", delegate(EntityTypeBuilder b)
		{
			b.HasOne("IdentityServer4.EntityFramework.Entities.Client", "Client").WithMany("IdentityProviderRestrictions").HasForeignKey("ClientId")
				.OnDelete(DeleteBehavior.Cascade);
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientPostLogoutRedirectUri", delegate(EntityTypeBuilder b)
		{
			b.HasOne("IdentityServer4.EntityFramework.Entities.Client", "Client").WithMany("PostLogoutRedirectUris").HasForeignKey("ClientId")
				.OnDelete(DeleteBehavior.Cascade);
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientProperty", delegate(EntityTypeBuilder b)
		{
			b.HasOne("IdentityServer4.EntityFramework.Entities.Client", "Client").WithMany("Properties").HasForeignKey("ClientId")
				.OnDelete(DeleteBehavior.Cascade);
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientRedirectUri", delegate(EntityTypeBuilder b)
		{
			b.HasOne("IdentityServer4.EntityFramework.Entities.Client", "Client").WithMany("RedirectUris").HasForeignKey("ClientId")
				.OnDelete(DeleteBehavior.Cascade);
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientScope", delegate(EntityTypeBuilder b)
		{
			b.HasOne("IdentityServer4.EntityFramework.Entities.Client", "Client").WithMany("AllowedScopes").HasForeignKey("ClientId")
				.OnDelete(DeleteBehavior.Cascade);
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ClientSecret", delegate(EntityTypeBuilder b)
		{
			b.HasOne("IdentityServer4.EntityFramework.Entities.Client", "Client").WithMany("ClientSecrets").HasForeignKey("ClientId")
				.OnDelete(DeleteBehavior.Cascade);
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.IdentityClaim", delegate(EntityTypeBuilder b)
		{
			b.HasOne("IdentityServer4.EntityFramework.Entities.IdentityResource", "IdentityResource").WithMany("UserClaims").HasForeignKey("IdentityResourceId")
				.OnDelete(DeleteBehavior.Cascade);
		});
	}
}
