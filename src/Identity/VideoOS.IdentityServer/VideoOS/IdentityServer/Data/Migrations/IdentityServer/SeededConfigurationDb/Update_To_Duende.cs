using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VideoOS.IdentityServer.Data.Migrations.IdentityServer.SeededConfigurationDb;

[DbContext(typeof(SeededConfigurationDbContext))]
[Migration("20220214094600_Update_To_Duende")]
public class Update_To_Duende : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropIndex("IX_IdentityResourceProperties_IdentityResourceId", "IdentityResourceProperties");
		migrationBuilder.DropIndex("IX_IdentityResourceClaims_IdentityResourceId", "IdentityResourceClaims");
		migrationBuilder.DropIndex("IX_ClientScopes_ClientId", "ClientScopes");
		migrationBuilder.DropIndex("IX_ClientRedirectUris_ClientId", "ClientRedirectUris");
		migrationBuilder.DropIndex("IX_ClientProperties_ClientId", "ClientProperties");
		migrationBuilder.DropIndex("IX_ClientPostLogoutRedirectUris_ClientId", "ClientPostLogoutRedirectUris");
		migrationBuilder.DropIndex("IX_ClientIdPRestrictions_ClientId", "ClientIdPRestrictions");
		migrationBuilder.DropIndex("IX_ClientGrantTypes_ClientId", "ClientGrantTypes");
		migrationBuilder.DropIndex("IX_ClientCorsOrigins_ClientId", "ClientCorsOrigins");
		migrationBuilder.DropIndex("IX_ClientClaims_ClientId", "ClientClaims");
		migrationBuilder.DropIndex("IX_ApiScopeProperties_ScopeId", "ApiScopeProperties");
		migrationBuilder.DropIndex("IX_ApiScopeClaims_ScopeId", "ApiScopeClaims");
		migrationBuilder.DropIndex("IX_ApiResourceScopes_ApiResourceId", "ApiResourceScopes");
		migrationBuilder.DropIndex("IX_ApiResourceProperties_ApiResourceId", "ApiResourceProperties");
		migrationBuilder.DropIndex("IX_ApiResourceClaims_ApiResourceId", "ApiResourceClaims");
		object defaultValue = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified);
		migrationBuilder.AddColumn<DateTime>("Created", "IdentityProviders", "datetime2", null, null, rowVersion: false, null, nullable: false, defaultValue);
		migrationBuilder.AddColumn<DateTime>("LastAccessed", "IdentityProviders", "datetime2", null, null, rowVersion: false, null, nullable: true);
		migrationBuilder.AddColumn<bool>("NonEditable", "IdentityProviders", "bit", null, null, rowVersion: false, null, nullable: false, false);
		migrationBuilder.AddColumn<DateTime>("Updated", "IdentityProviders", "datetime2", null, null, rowVersion: false, null, nullable: true);
		migrationBuilder.AddColumn<int>("CibaLifetime", "Clients", "int", null, null, rowVersion: false, null, nullable: true);
		migrationBuilder.AddColumn<int>("PollingInterval", "Clients", "int", null, null, rowVersion: false, null, nullable: true);
		int? maxLength = 400;
		Type typeFromHandle = typeof(string);
		int? oldMaxLength = 2000;
		migrationBuilder.AlterColumn<string>("RedirectUri", "ClientRedirectUris", "nvarchar(400)", null, maxLength, rowVersion: false, null, nullable: false, null, null, null, typeFromHandle, "nvarchar(2000)", null, oldMaxLength);
		oldMaxLength = 400;
		typeFromHandle = typeof(string);
		maxLength = 2000;
		migrationBuilder.AlterColumn<string>("PostLogoutRedirectUri", "ClientPostLogoutRedirectUris", "nvarchar(400)", null, oldMaxLength, rowVersion: false, null, nullable: false, null, null, null, typeFromHandle, "nvarchar(2000)", null, maxLength);
		defaultValue = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified);
		migrationBuilder.AddColumn<DateTime>("Created", "ApiScopes", "datetime2", null, null, rowVersion: false, null, nullable: false, defaultValue);
		migrationBuilder.AddColumn<DateTime>("LastAccessed", "ApiScopes", "datetime2", null, null, rowVersion: false, null, nullable: true);
		migrationBuilder.AddColumn<bool>("NonEditable", "ApiScopes", "bit", null, null, rowVersion: false, null, nullable: false, false);
		migrationBuilder.AddColumn<DateTime>("Updated", "ApiScopes", "datetime2", null, null, rowVersion: false, null, nullable: true);
		migrationBuilder.UpdateData("ApiResources", "Id", -10, "Created", new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(3415L));
		migrationBuilder.UpdateData("ApiResources", "Id", -9, "Created", new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(3409L));
		migrationBuilder.UpdateData("ApiResources", "Id", -8, "Created", new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(3378L));
		migrationBuilder.UpdateData("ApiResources", "Id", -7, "Created", new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(3373L));
		migrationBuilder.UpdateData("ApiResources", "Id", -6, "Created", new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(3366L));
		migrationBuilder.UpdateData("ApiResources", "Id", -5, "Created", new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(3361L));
		migrationBuilder.UpdateData("ApiResources", "Id", -4, "Created", new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(3354L));
		migrationBuilder.UpdateData("ApiResources", "Id", -3, "Created", new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(3349L));
		migrationBuilder.UpdateData("ApiResources", "Id", -2, "Created", new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(3343L));
		migrationBuilder.UpdateData("ApiResources", "Id", -1, "Created", new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(3322L));
		migrationBuilder.UpdateData("ApiScopes", "Id", -10, "Created", new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(4268L));
		migrationBuilder.UpdateData("ApiScopes", "Id", -9, "Created", new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(4236L));
		migrationBuilder.UpdateData("ApiScopes", "Id", -8, "Created", new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(4233L));
		migrationBuilder.UpdateData("ApiScopes", "Id", -7, "Created", new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(4230L));
		migrationBuilder.UpdateData("ApiScopes", "Id", -6, "Created", new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(4226L));
		migrationBuilder.UpdateData("ApiScopes", "Id", -5, "Created", new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(4223L));
		migrationBuilder.UpdateData("ApiScopes", "Id", -4, "Created", new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(4220L));
		migrationBuilder.UpdateData("ApiScopes", "Id", -3, "Created", new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(4216L));
		migrationBuilder.UpdateData("ApiScopes", "Id", -2, "Created", new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(4213L));
		migrationBuilder.UpdateData("ApiScopes", "Id", -1, "Created", new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(4205L));
		migrationBuilder.UpdateData("Clients", "Id", -4, "Created", new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(2556L));
		migrationBuilder.UpdateData("Clients", "Id", -3, "Created", new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(2504L));
		migrationBuilder.UpdateData("Clients", "Id", -2, "Created", new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(2460L));
		migrationBuilder.UpdateData("Clients", "Id", -1, "Created", new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(2400L));
		migrationBuilder.UpdateData("IdentityResources", "Id", -3, "Created", new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(3914L));
		migrationBuilder.UpdateData("IdentityResources", "Id", -2, "Created", new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(3899L));
		migrationBuilder.UpdateData("IdentityResources", "Id", -1, "Created", new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(3885L));
		migrationBuilder.CreateIndex("IX_IdentityResourceProperties_IdentityResourceId_Key", "IdentityResourceProperties", new string[2] { "IdentityResourceId", "Key" }, null, unique: true);
		migrationBuilder.CreateIndex("IX_IdentityResourceClaims_IdentityResourceId_Type", "IdentityResourceClaims", new string[2] { "IdentityResourceId", "Type" }, null, unique: true);
		migrationBuilder.CreateIndex("IX_IdentityProviders_Scheme", "IdentityProviders", "Scheme", null, unique: true);
		migrationBuilder.CreateIndex("IX_ClientScopes_ClientId_Scope", "ClientScopes", new string[2] { "ClientId", "Scope" }, null, unique: true);
		migrationBuilder.CreateIndex("IX_ClientRedirectUris_ClientId_RedirectUri", "ClientRedirectUris", new string[2] { "ClientId", "RedirectUri" }, null, unique: true);
		migrationBuilder.CreateIndex("IX_ClientProperties_ClientId_Key", "ClientProperties", new string[2] { "ClientId", "Key" }, null, unique: true);
		migrationBuilder.CreateIndex("IX_ClientPostLogoutRedirectUris_ClientId_PostLogoutRedirectUri", "ClientPostLogoutRedirectUris", new string[2] { "ClientId", "PostLogoutRedirectUri" }, null, unique: true);
		migrationBuilder.CreateIndex("IX_ClientIdPRestrictions_ClientId_Provider", "ClientIdPRestrictions", new string[2] { "ClientId", "Provider" }, null, unique: true);
		migrationBuilder.CreateIndex("IX_ClientGrantTypes_ClientId_GrantType", "ClientGrantTypes", new string[2] { "ClientId", "GrantType" }, null, unique: true);
		migrationBuilder.CreateIndex("IX_ClientCorsOrigins_ClientId_Origin", "ClientCorsOrigins", new string[2] { "ClientId", "Origin" }, null, unique: true);
		migrationBuilder.CreateIndex("IX_ClientClaims_ClientId_Type_Value", "ClientClaims", new string[3] { "ClientId", "Type", "Value" }, null, unique: true);
		migrationBuilder.CreateIndex("IX_ApiScopeProperties_ScopeId_Key", "ApiScopeProperties", new string[2] { "ScopeId", "Key" }, null, unique: true);
		migrationBuilder.CreateIndex("IX_ApiScopeClaims_ScopeId_Type", "ApiScopeClaims", new string[2] { "ScopeId", "Type" }, null, unique: true);
		migrationBuilder.CreateIndex("IX_ApiResourceScopes_ApiResourceId_Scope", "ApiResourceScopes", new string[2] { "ApiResourceId", "Scope" }, null, unique: true);
		migrationBuilder.CreateIndex("IX_ApiResourceProperties_ApiResourceId_Key", "ApiResourceProperties", new string[2] { "ApiResourceId", "Key" }, null, unique: true);
		migrationBuilder.CreateIndex("IX_ApiResourceClaims_ApiResourceId_Type", "ApiResourceClaims", new string[2] { "ApiResourceId", "Type" }, null, unique: true);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropIndex("IX_IdentityResourceProperties_IdentityResourceId_Key", "IdentityResourceProperties");
		migrationBuilder.DropIndex("IX_IdentityResourceClaims_IdentityResourceId_Type", "IdentityResourceClaims");
		migrationBuilder.DropIndex("IX_IdentityProviders_Scheme", "IdentityProviders");
		migrationBuilder.DropIndex("IX_ClientScopes_ClientId_Scope", "ClientScopes");
		migrationBuilder.DropIndex("IX_ClientRedirectUris_ClientId_RedirectUri", "ClientRedirectUris");
		migrationBuilder.DropIndex("IX_ClientProperties_ClientId_Key", "ClientProperties");
		migrationBuilder.DropIndex("IX_ClientPostLogoutRedirectUris_ClientId_PostLogoutRedirectUri", "ClientPostLogoutRedirectUris");
		migrationBuilder.DropIndex("IX_ClientIdPRestrictions_ClientId_Provider", "ClientIdPRestrictions");
		migrationBuilder.DropIndex("IX_ClientGrantTypes_ClientId_GrantType", "ClientGrantTypes");
		migrationBuilder.DropIndex("IX_ClientCorsOrigins_ClientId_Origin", "ClientCorsOrigins");
		migrationBuilder.DropIndex("IX_ClientClaims_ClientId_Type_Value", "ClientClaims");
		migrationBuilder.DropIndex("IX_ApiScopeProperties_ScopeId_Key", "ApiScopeProperties");
		migrationBuilder.DropIndex("IX_ApiScopeClaims_ScopeId_Type", "ApiScopeClaims");
		migrationBuilder.DropIndex("IX_ApiResourceScopes_ApiResourceId_Scope", "ApiResourceScopes");
		migrationBuilder.DropIndex("IX_ApiResourceProperties_ApiResourceId_Key", "ApiResourceProperties");
		migrationBuilder.DropIndex("IX_ApiResourceClaims_ApiResourceId_Type", "ApiResourceClaims");
		migrationBuilder.DropColumn("Created", "IdentityProviders");
		migrationBuilder.DropColumn("LastAccessed", "IdentityProviders");
		migrationBuilder.DropColumn("NonEditable", "IdentityProviders");
		migrationBuilder.DropColumn("Updated", "IdentityProviders");
		migrationBuilder.DropColumn("CibaLifetime", "Clients");
		migrationBuilder.DropColumn("PollingInterval", "Clients");
		migrationBuilder.DropColumn("Created", "ApiScopes");
		migrationBuilder.DropColumn("LastAccessed", "ApiScopes");
		migrationBuilder.DropColumn("NonEditable", "ApiScopes");
		migrationBuilder.DropColumn("Updated", "ApiScopes");
		int? maxLength = 2000;
		Type typeFromHandle = typeof(string);
		int? oldMaxLength = 400;
		migrationBuilder.AlterColumn<string>("RedirectUri", "ClientRedirectUris", "nvarchar(2000)", null, maxLength, rowVersion: false, null, nullable: false, null, null, null, typeFromHandle, "nvarchar(400)", null, oldMaxLength);
		oldMaxLength = 2000;
		typeFromHandle = typeof(string);
		maxLength = 400;
		migrationBuilder.AlterColumn<string>("PostLogoutRedirectUri", "ClientPostLogoutRedirectUris", "nvarchar(2000)", null, oldMaxLength, rowVersion: false, null, nullable: false, null, null, null, typeFromHandle, "nvarchar(400)", null, maxLength);
		migrationBuilder.UpdateData("ApiResources", "Id", -10, "Created", new DateTime(2022, 1, 11, 14, 11, 55, 91, DateTimeKind.Utc).AddTicks(9887L));
		migrationBuilder.UpdateData("ApiResources", "Id", -9, "Created", new DateTime(2022, 1, 11, 14, 11, 55, 91, DateTimeKind.Utc).AddTicks(9875L));
		migrationBuilder.UpdateData("ApiResources", "Id", -8, "Created", new DateTime(2022, 1, 11, 14, 11, 55, 91, DateTimeKind.Utc).AddTicks(9866L));
		migrationBuilder.UpdateData("ApiResources", "Id", -7, "Created", new DateTime(2022, 1, 11, 14, 11, 55, 91, DateTimeKind.Utc).AddTicks(9857L));
		migrationBuilder.UpdateData("ApiResources", "Id", -6, "Created", new DateTime(2022, 1, 11, 14, 11, 55, 91, DateTimeKind.Utc).AddTicks(9847L));
		migrationBuilder.UpdateData("ApiResources", "Id", -5, "Created", new DateTime(2022, 1, 11, 14, 11, 55, 91, DateTimeKind.Utc).AddTicks(9836L));
		migrationBuilder.UpdateData("ApiResources", "Id", -4, "Created", new DateTime(2022, 1, 11, 14, 11, 55, 91, DateTimeKind.Utc).AddTicks(9828L));
		migrationBuilder.UpdateData("ApiResources", "Id", -3, "Created", new DateTime(2022, 1, 11, 14, 11, 55, 91, DateTimeKind.Utc).AddTicks(9820L));
		migrationBuilder.UpdateData("ApiResources", "Id", -2, "Created", new DateTime(2022, 1, 11, 14, 11, 55, 91, DateTimeKind.Utc).AddTicks(9802L));
		migrationBuilder.UpdateData("ApiResources", "Id", -1, "Created", new DateTime(2022, 1, 11, 14, 11, 55, 91, DateTimeKind.Utc).AddTicks(8990L));
		migrationBuilder.UpdateData("Clients", "Id", -4, "Created", new DateTime(2022, 1, 11, 14, 11, 55, 64, DateTimeKind.Utc).AddTicks(1135L));
		migrationBuilder.UpdateData("Clients", "Id", -3, "Created", new DateTime(2022, 1, 11, 14, 11, 55, 64, DateTimeKind.Utc).AddTicks(721L));
		migrationBuilder.UpdateData("Clients", "Id", -2, "Created", new DateTime(2022, 1, 11, 14, 11, 55, 64, DateTimeKind.Utc).AddTicks(458L));
		migrationBuilder.UpdateData("Clients", "Id", -1, "Created", new DateTime(2022, 1, 11, 14, 11, 55, 62, DateTimeKind.Utc).AddTicks(5380L));
		migrationBuilder.UpdateData("IdentityResources", "Id", -3, "Created", new DateTime(2022, 1, 11, 14, 11, 55, 115, DateTimeKind.Utc).AddTicks(2619L));
		migrationBuilder.UpdateData("IdentityResources", "Id", -2, "Created", new DateTime(2022, 1, 11, 14, 11, 55, 111, DateTimeKind.Utc).AddTicks(3091L));
		migrationBuilder.UpdateData("IdentityResources", "Id", -1, "Created", new DateTime(2022, 1, 11, 14, 11, 55, 107, DateTimeKind.Utc).AddTicks(3580L));
		migrationBuilder.CreateIndex("IX_IdentityResourceProperties_IdentityResourceId", "IdentityResourceProperties", "IdentityResourceId");
		migrationBuilder.CreateIndex("IX_IdentityResourceClaims_IdentityResourceId", "IdentityResourceClaims", "IdentityResourceId");
		migrationBuilder.CreateIndex("IX_ClientScopes_ClientId", "ClientScopes", "ClientId");
		migrationBuilder.CreateIndex("IX_ClientRedirectUris_ClientId", "ClientRedirectUris", "ClientId");
		migrationBuilder.CreateIndex("IX_ClientProperties_ClientId", "ClientProperties", "ClientId");
		migrationBuilder.CreateIndex("IX_ClientPostLogoutRedirectUris_ClientId", "ClientPostLogoutRedirectUris", "ClientId");
		migrationBuilder.CreateIndex("IX_ClientIdPRestrictions_ClientId", "ClientIdPRestrictions", "ClientId");
		migrationBuilder.CreateIndex("IX_ClientGrantTypes_ClientId", "ClientGrantTypes", "ClientId");
		migrationBuilder.CreateIndex("IX_ClientCorsOrigins_ClientId", "ClientCorsOrigins", "ClientId");
		migrationBuilder.CreateIndex("IX_ClientClaims_ClientId", "ClientClaims", "ClientId");
		migrationBuilder.CreateIndex("IX_ApiScopeProperties_ScopeId", "ApiScopeProperties", "ScopeId");
		migrationBuilder.CreateIndex("IX_ApiScopeClaims_ScopeId", "ApiScopeClaims", "ScopeId");
		migrationBuilder.CreateIndex("IX_ApiResourceScopes_ApiResourceId", "ApiResourceScopes", "ApiResourceId");
		migrationBuilder.CreateIndex("IX_ApiResourceProperties_ApiResourceId", "ApiResourceProperties", "ApiResourceId");
		migrationBuilder.CreateIndex("IX_ApiResourceClaims_ApiResourceId", "ApiResourceClaims", "ApiResourceId");
	}

	protected override void BuildTargetModel(ModelBuilder modelBuilder)
	{
		modelBuilder.HasAnnotation("ProductVersion", "6.0.1").HasAnnotation("Relational:MaxIdentifierLength", 128);
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
				Created = new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(3322L),
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
				Created = new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(3343L),
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
				Created = new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(3349L),
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
				Created = new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(3354L),
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
				Created = new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(3361L),
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
				Created = new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(3366L),
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
				Created = new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(3373L),
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
				Created = new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(3378L),
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
				Created = new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(3409L),
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
				Created = new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(3415L),
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
				Created = new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(4205L),
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
				Created = new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(4213L),
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
				Created = new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(4216L),
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
				Created = new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(4220L),
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
				Created = new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(4223L),
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
				Created = new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(4226L),
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
				Created = new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(4230L),
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
				Created = new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(4233L),
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
				Created = new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(4236L),
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
				Created = new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(4268L),
				DisplayName = "Video Processing Server Scope",
				Emphasize = false,
				Enabled = true,
				Name = "vps",
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
			b.Property<DateTime>("Created").HasColumnType("datetime2");
			b.Property<string>("Description").HasMaxLength(1000).HasColumnType("nvarchar(1000)");
			b.Property<int>("DeviceCodeLifetime").HasColumnType("int");
			b.Property<bool>("EnableLocalLogin").HasColumnType("bit");
			b.Property<bool>("Enabled").HasColumnType("bit");
			b.Property<bool>("FrontChannelLogoutSessionRequired").HasColumnType("bit");
			b.Property<string>("FrontChannelLogoutUri").HasMaxLength(2000).HasColumnType("nvarchar(2000)");
			b.Property<int>("IdentityTokenLifetime").HasColumnType("int");
			b.Property<bool>("IncludeJwtId").HasColumnType("bit");
			b.Property<DateTime?>("LastAccessed").HasColumnType("datetime2");
			b.Property<string>("LogoUri").HasMaxLength(2000).HasColumnType("nvarchar(2000)");
			b.Property<bool>("NonEditable").HasColumnType("bit");
			b.Property<string>("PairWiseSubjectSalt").HasMaxLength(200).HasColumnType("nvarchar(200)");
			b.Property<int?>("PollingInterval").HasColumnType("int");
			b.Property<string>("ProtocolType").IsRequired().HasMaxLength(200)
				.HasColumnType("nvarchar(200)");
			b.Property<int>("RefreshTokenExpiration").HasColumnType("int");
			b.Property<int>("RefreshTokenUsage").HasColumnType("int");
			b.Property<bool>("RequireClientSecret").HasColumnType("bit");
			b.Property<bool>("RequireConsent").HasColumnType("bit");
			b.Property<bool>("RequirePkce").HasColumnType("bit");
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
				Created = new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(2400L),
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
				Created = new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(2460L),
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
				AlwaysIncludeUserClaimsInIdToken = true,
				AlwaysSendClientClaims = false,
				AuthorizationCodeLifetime = 300,
				BackChannelLogoutSessionRequired = true,
				ClientClaimsPrefix = "client_",
				ClientId = "VmsAdminClient",
				ClientName = "VmsAdminClient",
				Created = new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(2504L),
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
				AlwaysIncludeUserClaimsInIdToken = true,
				AlwaysSendClientClaims = false,
				AuthorizationCodeLifetime = 300,
				BackChannelLogoutSessionRequired = true,
				ClientClaimsPrefix = "client_",
				ClientId = "VmsClient",
				ClientName = "VmsClient",
				Created = new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(2556L),
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
				Created = new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(3885L),
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
				Created = new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(3899L),
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
				Created = new DateTime(2022, 2, 14, 9, 45, 59, 733, DateTimeKind.Utc).AddTicks(3914L),
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
