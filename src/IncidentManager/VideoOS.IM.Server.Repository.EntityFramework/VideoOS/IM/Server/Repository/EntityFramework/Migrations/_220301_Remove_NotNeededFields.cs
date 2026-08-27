using System.CodeDom.Compiler;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Builders;
using System.Data.Entity.Migrations.Infrastructure;
using System.Resources;

namespace VideoOS.IM.Server.Repository.EntityFramework.Migrations;

[GeneratedCode("EntityFramework.Migrations", "6.2.0-61023")]
public sealed class _220301_Remove_NotNeededFields : DbMigration, IMigrationMetadata
{
	private readonly ResourceManager Resources = new ResourceManager(typeof(_220301_Remove_NotNeededFields));

	string IMigrationMetadata.Id => "202205241145302_22.03.01_Remove_NotNeededFields";

	string IMigrationMetadata.Source => null;

	string IMigrationMetadata.Target => Resources.GetString("Target");

	public override void Up()
	{
		DropForeignKey("dbo.RepoIncidentCall", "ActionTypeId", "dbo.RepoDynamicalDataTypesAndOptions");
		DropIndex("dbo.RepoIncidentCall", new string[1] { "ActionTypeId" });
		DropColumn("dbo.RepoDraftSequence", "DraftSequenceIdentificationInt");
		DropColumn("dbo.RepoDraftSequence", "DraftSequenceIdentificationString");
		DropColumn("dbo.RepoDraftSequence", "DraftSequenceIdentificationApp");
		DropColumn("dbo.RepoDraftSequence", "IsVideoDraftSequence");
		DropColumn("dbo.RepoDraftSequence", "IsImageDraftSequence");
		DropColumn("dbo.RepoDraftSequence", "SoftDeleted");
		DropColumn("dbo.RepoIncidentSequence", "CameraAdditionalData");
		DropColumn("dbo.RepoIncidentSequence", "UserIdStr");
		DropColumn("dbo.RepoIncidentSequence", "UserName");
		DropColumn("dbo.RepoIncidentSequence", "Comment");
		DropColumn("dbo.RepoIncidentSequence", "ExportMessage");
		DropColumn("dbo.RepoIncidentSequence", "ExportCumulativeMessages");
		DropColumn("dbo.RepoIncidentSequence", "ExportContainErrors");
		DropColumn("dbo.RepoIncidentSequence", "ExportErrors");
		DropColumn("dbo.RepoIncidentSequence", "SoftDeleted");
		DropColumn("dbo.RepoIncident", "ActionTaken");
		DropColumn("dbo.RepoIncident", "LockerStatusStr");
		DropColumn("dbo.RepoIncident", "ErrorMessages");
		DropColumn("dbo.RepoIncident", "IsActive");
		DropColumn("dbo.RepoIncident", "ActiveMsUserId");
		DropColumn("dbo.RepoIncident", "ActiveSCId");
		DropColumn("dbo.RepoIncident", "TimeOfActive");
		DropColumn("dbo.RepoIncident", "SoftDeleted");
		DropColumn("dbo.RepoIncidentActivityLog", "UserTitle");
		DropColumn("dbo.RepoIncidentActivityLog", "MsUserId");
		DropColumn("dbo.RepoIncidentActivityLog", "AdditionalData");
		DropColumn("dbo.RepoIncidentActivityLog", "ActionType");
		DropColumn("dbo.RepoIncidentActivityLog", "SoftDeleted");
		DropColumn("dbo.RepoDynamicalDataTypesAndOptions", "SoftDeleted");
		DropColumn("dbo.RepoIncidentCall", "ActionTypeId");
		DropColumn("dbo.RepoIncidentCall", "AdditionalInfo");
		DropColumn("dbo.RepoIncidentCall", "AdditionalInfoData");
		DropColumn("dbo.RepoIncidentCall", "SoftDeleted");
		DropColumn("dbo.RepoIncidentComment", "UserIdStr");
		DropColumn("dbo.RepoIncidentComment", "TimeStamp");
		DropColumn("dbo.RepoIncidentComment", "SoftDeleted");
		DropColumn("dbo.RepoGlobalSettings", "GlobalSettingsIndentificatorLong");
		DropColumn("dbo.RepoGlobalSettings", "UserScopeId");
		DropColumn("dbo.RepoGlobalSettings", "MashineScopeId");
		DropColumn("dbo.RepoGlobalSettings", "SCAppScopeId");
		DropColumn("dbo.RepoGlobalSettings", "MCAppScopeId");
		DropColumn("dbo.RepoGlobalSettings", "CustomScopeId");
		DropColumn("dbo.RepoGlobalSettings", "ValueType");
		DropColumn("dbo.RepoGlobalSettings", "ValueBool");
		DropColumn("dbo.RepoGlobalSettings", "ValueLong");
		DropColumn("dbo.RepoGlobalSettings", "ValueString");
		DropColumn("dbo.RepoGlobalSettings", "ValueDecimal");
		DropColumn("dbo.RepoGlobalSettings", "ValueDateTime");
		DropColumn("dbo.RepoGlobalSettings", "SoftDeleted");
	}

	public override void Down()
	{
		AddColumn("dbo.RepoGlobalSettings", "SoftDeleted", (ColumnBuilder c) => c.Boolean(false));
		AddColumn("dbo.RepoGlobalSettings", "ValueDateTime", (ColumnBuilder c) => c.DateTime());
		AddColumn("dbo.RepoGlobalSettings", "ValueDecimal", (ColumnBuilder c) => c.Decimal(false, (byte)18, (byte)2));
		AddColumn("dbo.RepoGlobalSettings", "ValueString", (ColumnBuilder c) => c.String());
		AddColumn("dbo.RepoGlobalSettings", "ValueLong", (ColumnBuilder c) => c.Long(false));
		AddColumn("dbo.RepoGlobalSettings", "ValueBool", (ColumnBuilder c) => c.Boolean(false));
		AddColumn("dbo.RepoGlobalSettings", "ValueType", (ColumnBuilder c) => c.String());
		AddColumn("dbo.RepoGlobalSettings", "CustomScopeId", (ColumnBuilder c) => c.String());
		AddColumn("dbo.RepoGlobalSettings", "MCAppScopeId", (ColumnBuilder c) => c.String());
		AddColumn("dbo.RepoGlobalSettings", "SCAppScopeId", (ColumnBuilder c) => c.String());
		AddColumn("dbo.RepoGlobalSettings", "MashineScopeId", (ColumnBuilder c) => c.String());
		AddColumn("dbo.RepoGlobalSettings", "UserScopeId", (ColumnBuilder c) => c.String());
		AddColumn("dbo.RepoGlobalSettings", "GlobalSettingsIndentificatorLong", (ColumnBuilder c) => c.Long(false));
		AddColumn("dbo.RepoIncidentComment", "SoftDeleted", (ColumnBuilder c) => c.Boolean(false));
		AddColumn("dbo.RepoIncidentComment", "TimeStamp", delegate(ColumnBuilder c)
		{
			byte? precision = (byte)7;
			return c.Time(null, precision);
		});
		AddColumn("dbo.RepoIncidentComment", "UserIdStr", (ColumnBuilder c) => c.String());
		AddColumn("dbo.RepoIncidentCall", "SoftDeleted", (ColumnBuilder c) => c.Boolean(false));
		AddColumn("dbo.RepoIncidentCall", "AdditionalInfoData", (ColumnBuilder c) => c.String());
		AddColumn("dbo.RepoIncidentCall", "AdditionalInfo", (ColumnBuilder c) => c.String());
		AddColumn("dbo.RepoIncidentCall", "ActionTypeId", (ColumnBuilder c) => c.Long());
		AddColumn("dbo.RepoDynamicalDataTypesAndOptions", "SoftDeleted", (ColumnBuilder c) => c.Boolean(false));
		AddColumn("dbo.RepoIncidentActivityLog", "SoftDeleted", (ColumnBuilder c) => c.Boolean(false));
		AddColumn("dbo.RepoIncidentActivityLog", "ActionType", (ColumnBuilder c) => c.String());
		AddColumn("dbo.RepoIncidentActivityLog", "AdditionalData", (ColumnBuilder c) => c.String());
		AddColumn("dbo.RepoIncidentActivityLog", "MsUserId", (ColumnBuilder c) => c.String());
		AddColumn("dbo.RepoIncidentActivityLog", "UserTitle", (ColumnBuilder c) => c.String());
		AddColumn("dbo.RepoIncident", "SoftDeleted", (ColumnBuilder c) => c.Boolean(false));
		AddColumn("dbo.RepoIncident", "TimeOfActive", (ColumnBuilder c) => c.DateTime());
		AddColumn("dbo.RepoIncident", "ActiveSCId", (ColumnBuilder c) => c.String());
		AddColumn("dbo.RepoIncident", "ActiveMsUserId", (ColumnBuilder c) => c.String());
		AddColumn("dbo.RepoIncident", "IsActive", (ColumnBuilder c) => c.Boolean(false));
		AddColumn("dbo.RepoIncident", "ErrorMessages", (ColumnBuilder c) => c.String());
		AddColumn("dbo.RepoIncident", "LockerStatusStr", (ColumnBuilder c) => c.String());
		AddColumn("dbo.RepoIncident", "ActionTaken", (ColumnBuilder c) => c.String());
		AddColumn("dbo.RepoIncidentSequence", "SoftDeleted", (ColumnBuilder c) => c.Boolean(false));
		AddColumn("dbo.RepoIncidentSequence", "ExportErrors", (ColumnBuilder c) => c.String());
		AddColumn("dbo.RepoIncidentSequence", "ExportContainErrors", (ColumnBuilder c) => c.Boolean(false));
		AddColumn("dbo.RepoIncidentSequence", "ExportCumulativeMessages", (ColumnBuilder c) => c.String());
		AddColumn("dbo.RepoIncidentSequence", "ExportMessage", (ColumnBuilder c) => c.String());
		AddColumn("dbo.RepoIncidentSequence", "Comment", (ColumnBuilder c) => c.String());
		AddColumn("dbo.RepoIncidentSequence", "UserName", (ColumnBuilder c) => c.String());
		AddColumn("dbo.RepoIncidentSequence", "UserIdStr", (ColumnBuilder c) => c.String());
		AddColumn("dbo.RepoIncidentSequence", "CameraAdditionalData", (ColumnBuilder c) => c.String());
		AddColumn("dbo.RepoDraftSequence", "SoftDeleted", (ColumnBuilder c) => c.Boolean(false));
		AddColumn("dbo.RepoDraftSequence", "IsImageDraftSequence", (ColumnBuilder c) => c.Boolean(false));
		AddColumn("dbo.RepoDraftSequence", "IsVideoDraftSequence", (ColumnBuilder c) => c.Boolean(false));
		AddColumn("dbo.RepoDraftSequence", "DraftSequenceIdentificationApp", (ColumnBuilder c) => c.String());
		AddColumn("dbo.RepoDraftSequence", "DraftSequenceIdentificationString", (ColumnBuilder c) => c.String());
		AddColumn("dbo.RepoDraftSequence", "DraftSequenceIdentificationInt", (ColumnBuilder c) => c.Int(false));
		CreateIndex("dbo.RepoIncidentCall", "ActionTypeId");
		AddForeignKey("dbo.RepoIncidentCall", "ActionTypeId", "dbo.RepoDynamicalDataTypesAndOptions", "Id");
	}
}
