using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace VideoOS.IdentityServer.Data.Migrations.IdentityServer.SeededConfigurationDb;

[DbContext(typeof(SeededConfigurationDbContext))]
[Migration("20210406100155_UpdateTo_IdentityServer_4.1.0_ConfigurationDbMigration")]
public class UpdateTo_IdentityServer_410_ConfigurationDbMigration : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropForeignKey("FK_ApiClaims_ApiResources_ApiResourceId", "ApiClaims");
		migrationBuilder.DropForeignKey("FK_ApiProperties_ApiResources_ApiResourceId", "ApiProperties");
		migrationBuilder.DropForeignKey("FK_ApiScopeClaims_ApiScopes_ApiScopeId", "ApiScopeClaims");
		migrationBuilder.DropForeignKey("FK_ApiScopes_ApiResources_ApiResourceId", "ApiScopes");
		migrationBuilder.DropForeignKey("FK_IdentityProperties_IdentityResources_IdentityResourceId", "IdentityProperties");
		migrationBuilder.DropTable("ApiSecrets");
		migrationBuilder.DropTable("IdentityClaims");
		migrationBuilder.DropIndex("IX_ApiScopes_ApiResourceId", "ApiScopes");
		migrationBuilder.DropIndex("IX_ApiScopeClaims_ApiScopeId", "ApiScopeClaims");
		migrationBuilder.DropPrimaryKey("PK_IdentityProperties", "IdentityProperties");
		migrationBuilder.DropPrimaryKey("PK_ApiProperties", "ApiProperties");
		migrationBuilder.DropPrimaryKey("PK_ApiClaims", "ApiClaims");
		migrationBuilder.DropColumn("ApiResourceId", "ApiScopes");
		migrationBuilder.DropColumn("ApiScopeId", "ApiScopeClaims");
		migrationBuilder.RenameTable("IdentityProperties", null, "IdentityResourceProperties");
		migrationBuilder.RenameTable("ApiProperties", null, "ApiResourceProperties");
		migrationBuilder.RenameTable("ApiClaims", null, "ApiResourceClaims");
		migrationBuilder.RenameIndex("IX_IdentityProperties_IdentityResourceId", "IX_IdentityResourceProperties_IdentityResourceId", "IdentityResourceProperties");
		migrationBuilder.RenameIndex("IX_ApiProperties_ApiResourceId", "IX_ApiResourceProperties_ApiResourceId", "ApiResourceProperties");
		migrationBuilder.RenameIndex("IX_ApiClaims_ApiResourceId", "IX_ApiResourceClaims_ApiResourceId", "ApiResourceClaims");
		int? maxLength = 100;
		migrationBuilder.AddColumn<string>("AllowedIdentityTokenSigningAlgorithms", "Clients", null, null, maxLength, rowVersion: false, null, nullable: true);
		migrationBuilder.AddColumn<bool>("RequireRequestObject", "Clients", null, null, null, rowVersion: false, null, nullable: false, false);
		migrationBuilder.AddColumn<bool>("Enabled", "ApiScopes", null, null, null, rowVersion: false, null, nullable: false, false);
		migrationBuilder.AddColumn<int>("ScopeId", "ApiScopeClaims", null, null, null, rowVersion: false, null, nullable: false, 0);
		maxLength = 100;
		migrationBuilder.AddColumn<string>("AllowedAccessTokenSigningAlgorithms", "ApiResources", null, null, maxLength, rowVersion: false, null, nullable: true);
		migrationBuilder.AddColumn<bool>("ShowInDiscoveryDocument", "ApiResources", null, null, null, rowVersion: false, null, nullable: false, false);
		migrationBuilder.AddPrimaryKey("PK_IdentityResourceProperties", "IdentityResourceProperties", "Id");
		migrationBuilder.AddPrimaryKey("PK_ApiResourceProperties", "ApiResourceProperties", "Id");
		migrationBuilder.AddPrimaryKey("PK_ApiResourceClaims", "ApiResourceClaims", "Id");
		migrationBuilder.CreateTable("ApiResourceScopes", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id = table.Column<int>().Annotation("SqlServer:Identity", "1, 1");
			int? maxLength2 = 200;
			return new
			{
				Id = id,
				Scope = table.Column<string>(null, null, maxLength2),
				ApiResourceId = table.Column<int>()
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_ApiResourceScopes", x => x.Id);
			table.ForeignKey("FK_ApiResourceScopes_ApiResources_ApiResourceId", x => x.ApiResourceId, "ApiResources", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("ApiResourceSecrets", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id = table.Column<int>().Annotation("SqlServer:Identity", "1, 1");
			int? maxLength2 = 1000;
			OperationBuilder<AddColumnOperation> description = table.Column<string>(null, null, maxLength2, rowVersion: false, null, nullable: true);
			maxLength2 = 4000;
			OperationBuilder<AddColumnOperation> value = table.Column<string>(null, null, maxLength2);
			OperationBuilder<AddColumnOperation> expiration = table.Column<DateTime>(null, null, null, rowVersion: false, null, nullable: true);
			maxLength2 = 250;
			return new
			{
				Id = id,
				Description = description,
				Value = value,
				Expiration = expiration,
				Type = table.Column<string>(null, null, maxLength2),
				Created = table.Column<DateTime>(),
				ApiResourceId = table.Column<int>()
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_ApiResourceSecrets", x => x.Id);
			table.ForeignKey("FK_ApiResourceSecrets_ApiResources_ApiResourceId", x => x.ApiResourceId, "ApiResources", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("ApiScopeProperties", delegate(ColumnsBuilder table)
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
				ScopeId = table.Column<int>()
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_ApiScopeProperties", x => x.Id);
			table.ForeignKey("FK_ApiScopeProperties_ApiScopes_ScopeId", x => x.ScopeId, "ApiScopes", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("IdentityResourceClaims", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id = table.Column<int>().Annotation("SqlServer:Identity", "1, 1");
			int? maxLength2 = 200;
			return new
			{
				Id = id,
				Type = table.Column<string>(null, null, maxLength2),
				IdentityResourceId = table.Column<int>()
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_IdentityResourceClaims", x => x.Id);
			table.ForeignKey("FK_IdentityResourceClaims_IdentityResources_IdentityResourceId", x => x.IdentityResourceId, "IdentityResources", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.InsertData("ApiResourceScopes", new string[3] { "Id", "ApiResourceId", "Scope" }, new object[8, 3]
		{
			{ -1, -1, "write:client" },
			{ -2, -2, "management" },
			{ -3, -3, "recorder" },
			{ -4, -4, "idp_management" },
			{ -5, -5, "datacollector" },
			{ -6, -6, "managementserver" },
			{ -7, -7, "failoverresponseserver" },
			{ -8, -8, "mediadb" }
		});
		migrationBuilder.UpdateData("ApiResources", "Id", -8, new string[2] { "Created", "ShowInDiscoveryDocument" }, new object[2]
		{
			new DateTime(2021, 4, 6, 10, 1, 54, 535, DateTimeKind.Utc).AddTicks(1219L),
			true
		});
		migrationBuilder.UpdateData("ApiResources", "Id", -7, new string[2] { "Created", "ShowInDiscoveryDocument" }, new object[2]
		{
			new DateTime(2021, 4, 6, 10, 1, 54, 535, DateTimeKind.Utc).AddTicks(1219L),
			true
		});
		migrationBuilder.UpdateData("ApiResources", "Id", -6, new string[2] { "Created", "ShowInDiscoveryDocument" }, new object[2]
		{
			new DateTime(2021, 4, 6, 10, 1, 54, 535, DateTimeKind.Utc).AddTicks(1210L),
			true
		});
		migrationBuilder.UpdateData("ApiResources", "Id", -5, new string[2] { "Created", "ShowInDiscoveryDocument" }, new object[2]
		{
			new DateTime(2021, 4, 6, 10, 1, 54, 535, DateTimeKind.Utc).AddTicks(1202L),
			true
		});
		migrationBuilder.UpdateData("ApiResources", "Id", -4, new string[2] { "Created", "ShowInDiscoveryDocument" }, new object[2]
		{
			new DateTime(2021, 4, 6, 10, 1, 54, 535, DateTimeKind.Utc).AddTicks(1191L),
			true
		});
		migrationBuilder.UpdateData("ApiResources", "Id", -3, new string[2] { "Created", "ShowInDiscoveryDocument" }, new object[2]
		{
			new DateTime(2021, 4, 6, 10, 1, 54, 535, DateTimeKind.Utc).AddTicks(1184L),
			true
		});
		migrationBuilder.UpdateData("ApiResources", "Id", -2, new string[2] { "Created", "ShowInDiscoveryDocument" }, new object[2]
		{
			new DateTime(2021, 4, 6, 10, 1, 54, 535, DateTimeKind.Utc).AddTicks(1163L),
			true
		});
		migrationBuilder.UpdateData("ApiResources", "Id", -1, new string[2] { "Created", "ShowInDiscoveryDocument" }, new object[2]
		{
			new DateTime(2021, 4, 6, 10, 1, 54, 535, DateTimeKind.Utc).AddTicks(483L),
			true
		});
		migrationBuilder.UpdateData("ApiScopes", "Id", -8, new string[2] { "Description", "Enabled" }, new object[2] { null, true });
		migrationBuilder.UpdateData("ApiScopes", "Id", -7, new string[2] { "Description", "Enabled" }, new object[2] { null, true });
		migrationBuilder.UpdateData("ApiScopes", "Id", -6, new string[2] { "Description", "Enabled" }, new object[2] { null, true });
		migrationBuilder.UpdateData("ApiScopes", "Id", -5, new string[2] { "Description", "Enabled" }, new object[2] { null, true });
		migrationBuilder.UpdateData("ApiScopes", "Id", -4, new string[2] { "Description", "Enabled" }, new object[2] { null, true });
		migrationBuilder.UpdateData("ApiScopes", "Id", -3, new string[2] { "Description", "Enabled" }, new object[2] { null, true });
		migrationBuilder.UpdateData("ApiScopes", "Id", -2, new string[2] { "Description", "Enabled" }, new object[2] { null, true });
		migrationBuilder.UpdateData("ApiScopes", "Id", -1, new string[2] { "Description", "Enabled" }, new object[2] { null, true });
		migrationBuilder.UpdateData("Clients", "Id", -2, new string[3] { "Created", "IncludeJwtId", "RequirePkce" }, new object[3]
		{
			new DateTime(2021, 4, 6, 10, 1, 54, 503, DateTimeKind.Utc).AddTicks(3266L),
			true,
			true
		});
		migrationBuilder.UpdateData("Clients", "Id", -1, new string[4] { "Created", "IncludeJwtId", "RequireConsent", "RequirePkce" }, new object[4]
		{
			new DateTime(2021, 4, 6, 10, 1, 54, 501, DateTimeKind.Utc).AddTicks(9029L),
			true,
			false,
			true
		});
		migrationBuilder.InsertData("IdentityResourceClaims", new string[3] { "Id", "IdentityResourceId", "Type" }, new object[17, 3]
		{
			{ -2, -2, "name" },
			{ -1, -1, "sub" },
			{ -15, -2, "updated_at" },
			{ -9, -2, "picture" },
			{ -17, -3, "email_verified" },
			{ -16, -3, "email" },
			{ -14, -2, "locale" },
			{ -13, -2, "zoneinfo" },
			{ -12, -2, "birthdate" },
			{ -11, -2, "gender" },
			{ -10, -2, "website" },
			{ -3, -2, "family_name" },
			{ -8, -2, "profile" },
			{ -7, -2, "preferred_username" },
			{ -6, -2, "nickname" },
			{ -5, -2, "middle_name" },
			{ -4, -2, "given_name" }
		});
		migrationBuilder.UpdateData("IdentityResources", "Id", -3, "Created", new DateTime(2021, 4, 6, 10, 1, 54, 558, DateTimeKind.Utc).AddTicks(3054L));
		migrationBuilder.UpdateData("IdentityResources", "Id", -2, "Created", new DateTime(2021, 4, 6, 10, 1, 54, 554, DateTimeKind.Utc).AddTicks(1655L));
		migrationBuilder.UpdateData("IdentityResources", "Id", -1, "Created", new DateTime(2021, 4, 6, 10, 1, 54, 550, DateTimeKind.Utc).AddTicks(2011L));
		migrationBuilder.CreateIndex("IX_ApiScopeClaims_ScopeId", "ApiScopeClaims", "ScopeId");
		migrationBuilder.CreateIndex("IX_ApiResourceScopes_ApiResourceId", "ApiResourceScopes", "ApiResourceId");
		migrationBuilder.CreateIndex("IX_ApiResourceSecrets_ApiResourceId", "ApiResourceSecrets", "ApiResourceId");
		migrationBuilder.CreateIndex("IX_ApiScopeProperties_ScopeId", "ApiScopeProperties", "ScopeId");
		migrationBuilder.CreateIndex("IX_IdentityResourceClaims_IdentityResourceId", "IdentityResourceClaims", "IdentityResourceId");
		migrationBuilder.AddForeignKey("FK_ApiResourceClaims_ApiResources_ApiResourceId", "ApiResourceClaims", "ApiResourceId", "ApiResources", null, null, "Id", ReferentialAction.NoAction, ReferentialAction.Cascade);
		migrationBuilder.AddForeignKey("FK_ApiResourceProperties_ApiResources_ApiResourceId", "ApiResourceProperties", "ApiResourceId", "ApiResources", null, null, "Id", ReferentialAction.NoAction, ReferentialAction.Cascade);
		migrationBuilder.AddForeignKey("FK_ApiScopeClaims_ApiScopes_ScopeId", "ApiScopeClaims", "ScopeId", "ApiScopes", null, null, "Id", ReferentialAction.NoAction, ReferentialAction.Cascade);
		migrationBuilder.AddForeignKey("FK_IdentityResourceProperties_IdentityResources_IdentityResourceId", "IdentityResourceProperties", "IdentityResourceId", "IdentityResources", null, null, "Id", ReferentialAction.NoAction, ReferentialAction.Cascade);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropForeignKey("FK_ApiResourceClaims_ApiResources_ApiResourceId", "ApiResourceClaims");
		migrationBuilder.DropForeignKey("FK_ApiResourceProperties_ApiResources_ApiResourceId", "ApiResourceProperties");
		migrationBuilder.DropForeignKey("FK_ApiScopeClaims_ApiScopes_ScopeId", "ApiScopeClaims");
		migrationBuilder.DropForeignKey("FK_IdentityResourceProperties_IdentityResources_IdentityResourceId", "IdentityResourceProperties");
		migrationBuilder.DropTable("ApiResourceScopes");
		migrationBuilder.DropTable("ApiResourceSecrets");
		migrationBuilder.DropTable("ApiScopeProperties");
		migrationBuilder.DropTable("IdentityResourceClaims");
		migrationBuilder.DropIndex("IX_ApiScopeClaims_ScopeId", "ApiScopeClaims");
		migrationBuilder.DropPrimaryKey("PK_IdentityResourceProperties", "IdentityResourceProperties");
		migrationBuilder.DropPrimaryKey("PK_ApiResourceProperties", "ApiResourceProperties");
		migrationBuilder.DropPrimaryKey("PK_ApiResourceClaims", "ApiResourceClaims");
		migrationBuilder.DropColumn("AllowedIdentityTokenSigningAlgorithms", "Clients");
		migrationBuilder.DropColumn("RequireRequestObject", "Clients");
		migrationBuilder.DropColumn("Enabled", "ApiScopes");
		migrationBuilder.DropColumn("ScopeId", "ApiScopeClaims");
		migrationBuilder.DropColumn("AllowedAccessTokenSigningAlgorithms", "ApiResources");
		migrationBuilder.DropColumn("ShowInDiscoveryDocument", "ApiResources");
		migrationBuilder.RenameTable("IdentityResourceProperties", null, "IdentityProperties");
		migrationBuilder.RenameTable("ApiResourceProperties", null, "ApiProperties");
		migrationBuilder.RenameTable("ApiResourceClaims", null, "ApiClaims");
		migrationBuilder.RenameIndex("IX_IdentityResourceProperties_IdentityResourceId", "IX_IdentityProperties_IdentityResourceId", "IdentityProperties");
		migrationBuilder.RenameIndex("IX_ApiResourceProperties_ApiResourceId", "IX_ApiProperties_ApiResourceId", "ApiProperties");
		migrationBuilder.RenameIndex("IX_ApiResourceClaims_ApiResourceId", "IX_ApiClaims_ApiResourceId", "ApiClaims");
		migrationBuilder.AddColumn<int>("ApiResourceId", "ApiScopes", "int", null, null, rowVersion: false, null, nullable: false, 0);
		migrationBuilder.AddColumn<int>("ApiScopeId", "ApiScopeClaims", "int", null, null, rowVersion: false, null, nullable: false, 0);
		migrationBuilder.AddPrimaryKey("PK_IdentityProperties", "IdentityProperties", "Id");
		migrationBuilder.AddPrimaryKey("PK_ApiProperties", "ApiProperties", "Id");
		migrationBuilder.AddPrimaryKey("PK_ApiClaims", "ApiClaims", "Id");
		migrationBuilder.CreateTable("ApiSecrets", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id = table.Column<int>("int").Annotation("SqlServer:Identity", "1, 1");
			OperationBuilder<AddColumnOperation> apiResourceId = table.Column<int>("int");
			OperationBuilder<AddColumnOperation> created = table.Column<DateTime>("datetime2");
			int? maxLength = 1000;
			OperationBuilder<AddColumnOperation> description = table.Column<string>("nvarchar(1000)", null, maxLength, rowVersion: false, null, nullable: true);
			OperationBuilder<AddColumnOperation> expiration = table.Column<DateTime>("datetime2", null, null, rowVersion: false, null, nullable: true);
			maxLength = 250;
			OperationBuilder<AddColumnOperation> type = table.Column<string>("nvarchar(250)", null, maxLength);
			maxLength = 4000;
			return new
			{
				Id = id,
				ApiResourceId = apiResourceId,
				Created = created,
				Description = description,
				Expiration = expiration,
				Type = type,
				Value = table.Column<string>("nvarchar(4000)", null, maxLength)
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_ApiSecrets", x => x.Id);
			table.ForeignKey("FK_ApiSecrets_ApiResources_ApiResourceId", x => x.ApiResourceId, "ApiResources", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.CreateTable("IdentityClaims", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id = table.Column<int>("int").Annotation("SqlServer:Identity", "1, 1");
			OperationBuilder<AddColumnOperation> identityResourceId = table.Column<int>("int");
			int? maxLength = 200;
			return new
			{
				Id = id,
				IdentityResourceId = identityResourceId,
				Type = table.Column<string>("nvarchar(200)", null, maxLength)
			};
		}, null, table =>
		{
			table.PrimaryKey("PK_IdentityClaims", x => x.Id);
			table.ForeignKey("FK_IdentityClaims_IdentityResources_IdentityResourceId", x => x.IdentityResourceId, "IdentityResources", "Id", null, ReferentialAction.NoAction, ReferentialAction.Cascade);
		});
		migrationBuilder.UpdateData("ApiResources", "Id", -8, "Created", new DateTime(2021, 3, 12, 10, 49, 11, 75, DateTimeKind.Utc).AddTicks(3160L));
		migrationBuilder.UpdateData("ApiResources", "Id", -7, "Created", new DateTime(2021, 3, 12, 10, 49, 11, 75, DateTimeKind.Utc).AddTicks(3160L));
		migrationBuilder.UpdateData("ApiResources", "Id", -6, "Created", new DateTime(2021, 3, 12, 10, 49, 11, 75, DateTimeKind.Utc).AddTicks(3148L));
		migrationBuilder.UpdateData("ApiResources", "Id", -5, "Created", new DateTime(2021, 3, 12, 10, 49, 11, 75, DateTimeKind.Utc).AddTicks(3108L));
		migrationBuilder.UpdateData("ApiResources", "Id", -4, "Created", new DateTime(2021, 3, 12, 10, 49, 11, 75, DateTimeKind.Utc).AddTicks(3097L));
		migrationBuilder.UpdateData("ApiResources", "Id", -3, "Created", new DateTime(2021, 3, 12, 10, 49, 11, 75, DateTimeKind.Utc).AddTicks(3087L));
		migrationBuilder.UpdateData("ApiResources", "Id", -2, "Created", new DateTime(2021, 3, 12, 10, 49, 11, 75, DateTimeKind.Utc).AddTicks(3054L));
		migrationBuilder.UpdateData("ApiResources", "Id", -1, "Created", new DateTime(2021, 3, 12, 10, 49, 11, 72, DateTimeKind.Utc).AddTicks(901L));
		migrationBuilder.UpdateData("ApiScopes", "Id", -7, new string[2] { "ApiResourceId", "Description" }, new object[2] { -7, "Server wide scope for failover response server in recording server" });
		migrationBuilder.UpdateData("ApiScopes", "Id", -6, new string[2] { "ApiResourceId", "Description" }, new object[2] { -6, "Server wide scope for the management server in IIS" });
		migrationBuilder.UpdateData("ApiScopes", "Id", -5, new string[2] { "ApiResourceId", "Description" }, new object[2] { -5, "Server wide scope for the datacollector service" });
		migrationBuilder.UpdateData("ApiScopes", "Id", -4, new string[2] { "ApiResourceId", "Description" }, new object[2] { -4, "Services for datacollector server" });
		migrationBuilder.UpdateData("ApiScopes", "Id", -3, new string[2] { "ApiResourceId", "Description" }, new object[2] { -3, "Server wide scope for recorder services" });
		migrationBuilder.UpdateData("ApiScopes", "Id", -2, new string[2] { "ApiResourceId", "Description" }, new object[2] { -2, "Server wide scope for management services" });
		migrationBuilder.UpdateData("ApiScopes", "Id", -1, new string[2] { "ApiResourceId", "Description" }, new object[2] { -1, "Client registration scope" });
		migrationBuilder.UpdateData("Clients", "Id", -2, new string[3] { "Created", "IncludeJwtId", "RequirePkce" }, new object[3]
		{
			new DateTime(2021, 3, 12, 10, 49, 11, 45, DateTimeKind.Utc).AddTicks(5549L),
			false,
			false
		});
		migrationBuilder.UpdateData("Clients", "Id", -1, new string[4] { "Created", "IncludeJwtId", "RequireConsent", "RequirePkce" }, new object[4]
		{
			new DateTime(2021, 3, 12, 10, 49, 11, 42, DateTimeKind.Utc).AddTicks(7481L),
			false,
			true,
			false
		});
		migrationBuilder.InsertData("IdentityClaims", new string[3] { "Id", "IdentityResourceId", "Type" }, new object[17, 3]
		{
			{ -1, -1, "sub" },
			{ -2, -2, "name" },
			{ -3, -2, "family_name" },
			{ -4, -2, "given_name" },
			{ -5, -2, "middle_name" },
			{ -8, -2, "profile" },
			{ -6, -2, "nickname" },
			{ -17, -3, "email_verified" },
			{ -16, -3, "email" },
			{ -15, -2, "updated_at" },
			{ -14, -2, "locale" },
			{ -13, -2, "zoneinfo" },
			{ -12, -2, "birthdate" },
			{ -11, -2, "gender" },
			{ -10, -2, "website" },
			{ -9, -2, "picture" },
			{ -7, -2, "preferred_username" }
		});
		migrationBuilder.UpdateData("IdentityResources", "Id", -3, "Created", new DateTime(2021, 3, 12, 10, 49, 11, 99, DateTimeKind.Utc).AddTicks(9722L));
		migrationBuilder.UpdateData("IdentityResources", "Id", -2, "Created", new DateTime(2021, 3, 12, 10, 49, 11, 95, DateTimeKind.Utc).AddTicks(4262L));
		migrationBuilder.UpdateData("IdentityResources", "Id", -1, "Created", new DateTime(2021, 3, 12, 10, 49, 11, 91, DateTimeKind.Utc).AddTicks(579L));
		migrationBuilder.CreateIndex("IX_ApiScopes_ApiResourceId", "ApiScopes", "ApiResourceId");
		migrationBuilder.CreateIndex("IX_ApiScopeClaims_ApiScopeId", "ApiScopeClaims", "ApiScopeId");
		migrationBuilder.CreateIndex("IX_ApiSecrets_ApiResourceId", "ApiSecrets", "ApiResourceId");
		migrationBuilder.CreateIndex("IX_IdentityClaims_IdentityResourceId", "IdentityClaims", "IdentityResourceId");
		migrationBuilder.AddForeignKey("FK_ApiClaims_ApiResources_ApiResourceId", "ApiClaims", "ApiResourceId", "ApiResources", null, null, "Id", ReferentialAction.NoAction, ReferentialAction.Cascade);
		migrationBuilder.AddForeignKey("FK_ApiProperties_ApiResources_ApiResourceId", "ApiProperties", "ApiResourceId", "ApiResources", null, null, "Id", ReferentialAction.NoAction, ReferentialAction.Cascade);
		migrationBuilder.AddForeignKey("FK_ApiScopeClaims_ApiScopes_ApiScopeId", "ApiScopeClaims", "ApiScopeId", "ApiScopes", null, null, "Id", ReferentialAction.NoAction, ReferentialAction.Cascade);
		migrationBuilder.AddForeignKey("FK_ApiScopes_ApiResources_ApiResourceId", "ApiScopes", "ApiResourceId", "ApiResources", null, null, "Id", ReferentialAction.NoAction, ReferentialAction.Cascade);
		migrationBuilder.AddForeignKey("FK_IdentityProperties_IdentityResources_IdentityResourceId", "IdentityProperties", "IdentityResourceId", "IdentityResources", null, null, "Id", ReferentialAction.NoAction, ReferentialAction.Cascade);
	}

	protected override void BuildTargetModel(ModelBuilder modelBuilder)
	{
		modelBuilder.HasAnnotation("ProductVersion", "3.1.5").HasAnnotation("Relational:MaxIdentifierLength", 128).HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ApiResource", delegate(EntityTypeBuilder b)
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
			b.Property<bool>("ShowInDiscoveryDocument").HasColumnType("bit");
			b.Property<DateTime?>("Updated").HasColumnType("datetime2");
			b.HasKey("Id");
			b.HasIndex("Name").IsUnique();
			b.ToTable("ApiResources");
			b.HasData(new
			{
				Id = -1,
				Created = new DateTime(2021, 4, 6, 10, 1, 54, 535, DateTimeKind.Utc).AddTicks(483L),
				Description = "Api for client registration",
				DisplayName = "Client registration",
				Enabled = true,
				Name = "registration",
				NonEditable = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -2,
				Created = new DateTime(2021, 4, 6, 10, 1, 54, 535, DateTimeKind.Utc).AddTicks(1163L),
				Description = "Services for management server",
				DisplayName = "Management server",
				Enabled = true,
				Name = "management",
				NonEditable = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -3,
				Created = new DateTime(2021, 4, 6, 10, 1, 54, 535, DateTimeKind.Utc).AddTicks(1184L),
				Description = "Services for recorder server",
				DisplayName = "Recorder server",
				Enabled = true,
				Name = "recorder",
				NonEditable = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -4,
				Created = new DateTime(2021, 4, 6, 10, 1, 54, 535, DateTimeKind.Utc).AddTicks(1191L),
				Description = "Management of IDP",
				DisplayName = "IDP Management",
				Enabled = true,
				Name = "idp_management",
				NonEditable = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -5,
				Created = new DateTime(2021, 4, 6, 10, 1, 54, 535, DateTimeKind.Utc).AddTicks(1202L),
				Description = "Api for datacollector",
				DisplayName = "Datacollector",
				Enabled = true,
				Name = "datacollector",
				NonEditable = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -6,
				Created = new DateTime(2021, 4, 6, 10, 1, 54, 535, DateTimeKind.Utc).AddTicks(1210L),
				Description = "Api for management server",
				DisplayName = "Management Server",
				Enabled = true,
				Name = "managementserver",
				NonEditable = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -7,
				Created = new DateTime(2021, 4, 6, 10, 1, 54, 535, DateTimeKind.Utc).AddTicks(1219L),
				Description = "Api for failover response server",
				DisplayName = "Failover Response Server",
				Enabled = true,
				Name = "failoverresponseserver",
				NonEditable = false,
				ShowInDiscoveryDocument = true
			}, new
			{
				Id = -8,
				Created = new DateTime(2021, 4, 6, 10, 1, 54, 535, DateTimeKind.Utc).AddTicks(1219L),
				Description = "Api for media db",
				DisplayName = "Media Db",
				Enabled = true,
				Name = "mediadb",
				NonEditable = false,
				ShowInDiscoveryDocument = true
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
			b.ToTable("ApiResourceProperties");
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ApiResourceScope", delegate(EntityTypeBuilder b)
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
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ApiResourceSecret", delegate(EntityTypeBuilder b)
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
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ApiScope", delegate(EntityTypeBuilder b)
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
				DisplayName = "Media db Scope",
				Emphasize = false,
				Enabled = true,
				Name = "mediadb",
				Required = false,
				ShowInDiscoveryDocument = true
			});
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ApiScopeClaim", delegate(EntityTypeBuilder b)
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
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ApiScopeProperty", delegate(EntityTypeBuilder b)
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
				Created = new DateTime(2021, 4, 6, 10, 1, 54, 501, DateTimeKind.Utc).AddTicks(9029L),
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
				Created = new DateTime(2021, 4, 6, 10, 1, 54, 503, DateTimeKind.Utc).AddTicks(3266L),
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
				Created = new DateTime(2021, 4, 6, 10, 1, 54, 550, DateTimeKind.Utc).AddTicks(2011L),
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
				Created = new DateTime(2021, 4, 6, 10, 1, 54, 554, DateTimeKind.Utc).AddTicks(1655L),
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
				Created = new DateTime(2021, 4, 6, 10, 1, 54, 558, DateTimeKind.Utc).AddTicks(3054L),
				DisplayName = "Your email address",
				Emphasize = true,
				Enabled = true,
				Name = "email",
				NonEditable = false,
				Required = false,
				ShowInDiscoveryDocument = true
			});
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.IdentityResourceClaim", delegate(EntityTypeBuilder b)
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
			b.ToTable("IdentityResourceProperties");
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
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ApiResourceScope", delegate(EntityTypeBuilder b)
		{
			b.HasOne("IdentityServer4.EntityFramework.Entities.ApiResource", "ApiResource").WithMany("Scopes").HasForeignKey("ApiResourceId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ApiResourceSecret", delegate(EntityTypeBuilder b)
		{
			b.HasOne("IdentityServer4.EntityFramework.Entities.ApiResource", "ApiResource").WithMany("Secrets").HasForeignKey("ApiResourceId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ApiScopeClaim", delegate(EntityTypeBuilder b)
		{
			b.HasOne("IdentityServer4.EntityFramework.Entities.ApiScope", "Scope").WithMany("UserClaims").HasForeignKey("ScopeId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		});
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.ApiScopeProperty", delegate(EntityTypeBuilder b)
		{
			b.HasOne("IdentityServer4.EntityFramework.Entities.ApiScope", "Scope").WithMany("Properties").HasForeignKey("ScopeId")
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
		modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.IdentityResourceClaim", delegate(EntityTypeBuilder b)
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
