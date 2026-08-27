using System.CodeDom.Compiler;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Builders;
using System.Data.Entity.Migrations.Infrastructure;
using System.Data.Entity.Migrations.Model;
using System.Resources;

namespace VideoOS.IM.Server.Repository.EntityFramework.Migrations;

[GeneratedCode("EntityFramework.Migrations", "6.2.0-61023")]
public sealed class Initial_IM_DB : DbMigration, IMigrationMetadata
{
	private readonly ResourceManager Resources = new ResourceManager(typeof(Initial_IM_DB));

	string IMigrationMetadata.Id => "202204131243570_Initial_IM_DB";

	string IMigrationMetadata.Source => null;

	string IMigrationMetadata.Target => Resources.GetString("Target");

	public override void Up()
	{
		CreateTable("dbo.RepoDraftSequence", (ColumnBuilder c) => new
		{
			Id = c.Long(false, identity: true),
			Title = c.String(),
			TimeStamp = c.DateTime(),
			Description = c.String(),
			DraftSequenceIdentificationInt = c.Int(false),
			DraftSequenceIdentificationString = c.String(),
			DraftSequenceIdentificationMsUSerId = c.String(),
			DraftSequenceIdentificationApp = c.String(),
			IsVideoDraftSequence = c.Boolean(false),
			IsImageDraftSequence = c.Boolean(false),
			TimeCreated = c.DateTime(),
			CreatedUserId = c.String(),
			CreatedUserName = c.String(),
			TimeModified = c.DateTime(),
			ModifiedUserId = c.String(),
			ModifiedUserName = c.String(),
			SoftDeleted = c.Boolean(false),
			SystemString = c.String()
		}).PrimaryKey(t => t.Id);
		CreateTable("dbo.RepoIncidentSequence", (ColumnBuilder c) => new
		{
			Id = c.Long(false, identity: true),
			CameraId = c.Guid(false),
			CameraTitle = c.String(),
			CameraAdditionalData = c.String(),
			StartTime = c.DateTime(),
			EndTime = c.DateTime(),
			UserIdStr = c.String(),
			UserName = c.String(),
			Comment = c.String(),
			OrderNumber = c.Int(false),
			ExportMessage = c.String(),
			ExportCumulativeMessages = c.String(),
			ExportContainErrors = c.Boolean(false),
			ExportErrors = c.String(),
			IncidentId = c.Long(),
			DraftSequenceId = c.Long(),
			TimeCreated = c.DateTime(),
			CreatedUserId = c.String(),
			CreatedUserName = c.String(),
			TimeModified = c.DateTime(),
			ModifiedUserId = c.String(),
			ModifiedUserName = c.String(),
			SoftDeleted = c.Boolean(false),
			SystemString = c.String()
		}).PrimaryKey(t => t.Id).ForeignKey("dbo.RepoIncident", t => t.IncidentId, cascadeDelete: true).ForeignKey("dbo.RepoDraftSequence", t => t.DraftSequenceId)
			.Index(t => t.IncidentId)
			.Index(t => t.DraftSequenceId);
		CreateTable("dbo.RepoIncident", delegate(ColumnBuilder c)
		{
			ColumnModel id = c.Long(false, identity: true);
			ColumnModel reference = c.Long(false);
			ColumnModel referenceStr = c.String();
			ColumnModel typeStr = c.String();
			ColumnModel typeId = c.Long();
			ColumnModel statusStr = c.String();
			ColumnModel statusId = c.Long();
			ColumnModel start = c.DateTime();
			ColumnModel startStr = c.String();
			ColumnModel end = c.DateTime();
			byte? precision = (byte)7;
			return new
			{
				Id = id,
				Reference = reference,
				ReferenceStr = referenceStr,
				TypeStr = typeStr,
				TypeId = typeId,
				StatusStr = statusStr,
				StatusId = statusId,
				Start = start,
				StartStr = startStr,
				End = end,
				Time = c.Time(null, precision),
				Date = c.DateTime(),
				ActionTaken = c.String(),
				LockerStatusStr = c.String(),
				Description = c.String(),
				ErrorMessages = c.String(),
				IsActive = c.Boolean(false),
				ActiveMsUserId = c.String(),
				ActiveSCId = c.String(),
				TimeOfActive = c.DateTime(),
				ExtPropLong1 = c.Long(false),
				ExtPropLong1_Title = c.String(),
				ExtPropLong1_Desc = c.String(),
				ExtPropLong2 = c.Long(false),
				ExtPropLong2_Title = c.String(),
				ExtPropLong2_Desc = c.String(),
				ExtPropLong3 = c.Long(false),
				ExtPropLong3_Title = c.String(),
				ExtPropLong3_Desc = c.String(),
				ExtPropString1 = c.String(),
				ExtPropString1_Title = c.String(),
				ExtPropString1_Desc = c.String(),
				ExtPropString2 = c.String(),
				ExtPropString2_Title = c.String(),
				ExtPropString2_Desc = c.String(),
				ExtPropString3 = c.String(),
				ExtPropString3_Title = c.String(),
				ExtPropString3_Desc = c.String(),
				ExtPropBool1 = c.Boolean(false),
				ExtPropBool1_Title = c.String(),
				ExtPropBool1_Desc = c.String(),
				ExtPropBool2 = c.Boolean(false),
				ExtPropBool2_Title = c.String(),
				ExtPropBool2_Desc = c.String(),
				ExtPropBool3 = c.Boolean(false),
				ExtPropBool3_Title = c.String(),
				ExtPropBool3_Desc = c.String(),
				ExtPropDecimal1 = c.Decimal(false, (byte)18, (byte)2),
				ExtPropDecimal1_Title = c.String(),
				ExtPropDecimal1_Desc = c.String(),
				ExtPropDecimal2 = c.Decimal(false, (byte)18, (byte)2),
				ExtPropDecimal2_Title = c.String(),
				ExtPropDecimal2_Desc = c.String(),
				ExtPropDecimal3 = c.Decimal(false, (byte)18, (byte)2),
				ExtPropDecimal3_Title = c.String(),
				ExtPropDecimal3_Desc = c.String(),
				ExtPropDateTime1 = c.DateTime(),
				ExtPropDateTime1_Title = c.String(),
				ExtPropDateTime1_Desc = c.String(),
				ExtPropDateTime1_Configuration = c.String(),
				ExtPropDateTime2 = c.DateTime(),
				ExtPropDateTime2_Title = c.String(),
				ExtPropDateTime2_Desc = c.String(),
				ExtPropDateTime2_Configuration = c.String(),
				ExtPropDateTime3 = c.DateTime(),
				ExtPropDateTime3_Title = c.String(),
				ExtPropDateTime3_Desc = c.String(),
				ExtPropDateTime3_Configuration = c.String(),
				ExtPropJSon1 = c.String(),
				ExtPropJSon1_Title = c.String(),
				ExtPropJSon1_Desc = c.String(),
				ExtPropJSon1_Configuration = c.String(),
				ExtPropJSon2 = c.String(),
				ExtPropJSon2_Title = c.String(),
				ExtPropJSon2_Desc = c.String(),
				ExtPropJSon2_Configuration = c.String(),
				ExtPropJSon3 = c.String(),
				ExtPropJSon3_Title = c.String(),
				ExtPropJSon3_Desc = c.String(),
				ExtPropJSon3_Configuration = c.String(),
				ExtendedPropsGroup1_JSonConfiguration = c.String(),
				ExtendedPropsGroup2_JSonConfiguration = c.String(),
				Category1Id = c.Long(),
				Category2Id = c.Long(),
				Category3Id = c.Long(),
				Category4Id = c.Long(),
				Category5Id = c.Long(),
				TimeCreated = c.DateTime(),
				CreatedUserId = c.String(),
				CreatedUserName = c.String(),
				TimeModified = c.DateTime(),
				ModifiedUserId = c.String(),
				ModifiedUserName = c.String(),
				SoftDeleted = c.Boolean(false),
				SystemString = c.String()
			};
		}).PrimaryKey(t => t.Id).ForeignKey("dbo.RepoDynamicalDataTypesAndOptions", t => t.Category1Id).ForeignKey("dbo.RepoDynamicalDataTypesAndOptions", t => t.Category2Id)
			.ForeignKey("dbo.RepoDynamicalDataTypesAndOptions", t => t.Category3Id)
			.ForeignKey("dbo.RepoDynamicalDataTypesAndOptions", t => t.Category4Id)
			.ForeignKey("dbo.RepoDynamicalDataTypesAndOptions", t => t.Category5Id)
			.ForeignKey("dbo.RepoDynamicalDataTypesAndOptions", t => t.StatusId)
			.ForeignKey("dbo.RepoDynamicalDataTypesAndOptions", t => t.TypeId)
			.Index(t => t.TypeId)
			.Index(t => t.StatusId)
			.Index(t => t.Category1Id)
			.Index(t => t.Category2Id)
			.Index(t => t.Category3Id)
			.Index(t => t.Category4Id)
			.Index(t => t.Category5Id);
		CreateTable("dbo.RepoIncidentActivityLog", (ColumnBuilder c) => new
		{
			Id = c.Long(false, identity: true),
			UserTitle = c.String(),
			MsUserId = c.String(),
			TimeOfActivityLog = c.DateTime(),
			Description = c.String(),
			AdditionalData = c.String(),
			EntityType = c.String(),
			EntityId = c.String(),
			ActionType = c.String(),
			JsonOldValues = c.String(),
			JsonNewValues = c.String(),
			IncidentId = c.Long(false),
			TimeCreated = c.DateTime(),
			CreatedUserId = c.String(),
			CreatedUserName = c.String(),
			TimeModified = c.DateTime(),
			ModifiedUserId = c.String(),
			ModifiedUserName = c.String(),
			SoftDeleted = c.Boolean(false),
			SystemString = c.String()
		}).PrimaryKey(t => t.Id).ForeignKey("dbo.RepoIncident", t => t.IncidentId, cascadeDelete: true).Index(t => t.IncidentId);
		CreateTable("dbo.RepoDynamicalDataTypesAndOptions", (ColumnBuilder c) => new
		{
			Id = c.Long(false, identity: true),
			TypeOfItemStr = c.String(),
			EntityScopeStr = c.String(),
			PropertyScopeStr = c.String(),
			ValueInt = c.Int(false),
			ValueStr = c.String(),
			ValueDecimal = c.Decimal(false, (byte)18, (byte)2),
			ValueBool = c.Boolean(false),
			Name = c.String(),
			Title = c.String(),
			Description = c.String(),
			ClassOfData = c.String(),
			ClassOfDataint = c.Int(false),
			CustomAction = c.String(),
			TimeCreated = c.DateTime(),
			CreatedUserId = c.String(),
			CreatedUserName = c.String(),
			TimeModified = c.DateTime(),
			ModifiedUserId = c.String(),
			ModifiedUserName = c.String(),
			SoftDeleted = c.Boolean(false),
			SystemString = c.String()
		}).PrimaryKey(t => t.Id);
		CreateTable("dbo.RepoIncidentCall", (ColumnBuilder c) => new
		{
			Id = c.Long(false, identity: true),
			Called = c.DateTime(),
			Arrived = c.DateTime(),
			Departed = c.DateTime(),
			Notes = c.String(),
			ActionTypeId = c.Long(),
			AdditionalInfo = c.String(),
			AdditionalInfoData = c.String(),
			IncidentId = c.Long(false),
			TimeCreated = c.DateTime(),
			CreatedUserId = c.String(),
			CreatedUserName = c.String(),
			TimeModified = c.DateTime(),
			ModifiedUserId = c.String(),
			ModifiedUserName = c.String(),
			SoftDeleted = c.Boolean(false),
			SystemString = c.String()
		}).PrimaryKey(t => t.Id).ForeignKey("dbo.RepoDynamicalDataTypesAndOptions", t => t.ActionTypeId).ForeignKey("dbo.RepoIncident", t => t.IncidentId, cascadeDelete: true)
			.Index(t => t.ActionTypeId)
			.Index(t => t.IncidentId);
		CreateTable("dbo.RepoIncidentComment", delegate(ColumnBuilder c)
		{
			ColumnModel id = c.Long(false, identity: true);
			ColumnModel userIdStr = c.String();
			ColumnModel user = c.String();
			byte? precision = (byte)7;
			return new
			{
				Id = id,
				UserIdStr = userIdStr,
				User = user,
				TimeStamp = c.Time(null, precision),
				Comment = c.String(),
				IncidentId = c.Long(false),
				TimeCreated = c.DateTime(),
				CreatedUserId = c.String(),
				CreatedUserName = c.String(),
				TimeModified = c.DateTime(),
				ModifiedUserId = c.String(),
				ModifiedUserName = c.String(),
				SoftDeleted = c.Boolean(false),
				SystemString = c.String()
			};
		}).PrimaryKey(t => t.Id).ForeignKey("dbo.RepoIncident", t => t.IncidentId, cascadeDelete: true).Index(t => t.IncidentId);
		CreateTable("dbo.RepoGlobalSettings", (ColumnBuilder c) => new
		{
			Id = c.Long(false, identity: true),
			GlobalSettingsIndentificator = c.String(),
			GlobalSettingsIndentificatorLong = c.Long(false),
			UserScopeId = c.String(),
			MashineScopeId = c.String(),
			SCAppScopeId = c.String(),
			MCAppScopeId = c.String(),
			CustomScopeId = c.String(),
			ValueType = c.String(),
			ValueBool = c.Boolean(false),
			ValueLong = c.Long(false),
			ValueString = c.String(),
			ValueDecimal = c.Decimal(false, (byte)18, (byte)2),
			ValueDateTime = c.DateTime(),
			ValueJSon = c.String(),
			TimeCreated = c.DateTime(),
			CreatedUserId = c.String(),
			CreatedUserName = c.String(),
			TimeModified = c.DateTime(),
			ModifiedUserId = c.String(),
			ModifiedUserName = c.String(),
			SoftDeleted = c.Boolean(false),
			SystemString = c.String()
		}).PrimaryKey(t => t.Id);
	}

	public override void Down()
	{
		DropForeignKey("dbo.RepoIncidentSequence", "DraftSequenceId", "dbo.RepoDraftSequence");
		DropForeignKey("dbo.RepoIncidentSequence", "IncidentId", "dbo.RepoIncident");
		DropForeignKey("dbo.RepoIncidentComment", "IncidentId", "dbo.RepoIncident");
		DropForeignKey("dbo.RepoIncidentCall", "IncidentId", "dbo.RepoIncident");
		DropForeignKey("dbo.RepoIncidentCall", "ActionTypeId", "dbo.RepoDynamicalDataTypesAndOptions");
		DropForeignKey("dbo.RepoIncident", "TypeId", "dbo.RepoDynamicalDataTypesAndOptions");
		DropForeignKey("dbo.RepoIncident", "StatusId", "dbo.RepoDynamicalDataTypesAndOptions");
		DropForeignKey("dbo.RepoIncident", "Category5Id", "dbo.RepoDynamicalDataTypesAndOptions");
		DropForeignKey("dbo.RepoIncident", "Category4Id", "dbo.RepoDynamicalDataTypesAndOptions");
		DropForeignKey("dbo.RepoIncident", "Category3Id", "dbo.RepoDynamicalDataTypesAndOptions");
		DropForeignKey("dbo.RepoIncident", "Category2Id", "dbo.RepoDynamicalDataTypesAndOptions");
		DropForeignKey("dbo.RepoIncident", "Category1Id", "dbo.RepoDynamicalDataTypesAndOptions");
		DropForeignKey("dbo.RepoIncidentActivityLog", "IncidentId", "dbo.RepoIncident");
		DropIndex("dbo.RepoIncidentComment", new string[1] { "IncidentId" });
		DropIndex("dbo.RepoIncidentCall", new string[1] { "IncidentId" });
		DropIndex("dbo.RepoIncidentCall", new string[1] { "ActionTypeId" });
		DropIndex("dbo.RepoIncidentActivityLog", new string[1] { "IncidentId" });
		DropIndex("dbo.RepoIncident", new string[1] { "Category5Id" });
		DropIndex("dbo.RepoIncident", new string[1] { "Category4Id" });
		DropIndex("dbo.RepoIncident", new string[1] { "Category3Id" });
		DropIndex("dbo.RepoIncident", new string[1] { "Category2Id" });
		DropIndex("dbo.RepoIncident", new string[1] { "Category1Id" });
		DropIndex("dbo.RepoIncident", new string[1] { "StatusId" });
		DropIndex("dbo.RepoIncident", new string[1] { "TypeId" });
		DropIndex("dbo.RepoIncidentSequence", new string[1] { "DraftSequenceId" });
		DropIndex("dbo.RepoIncidentSequence", new string[1] { "IncidentId" });
		DropTable("dbo.RepoGlobalSettings");
		DropTable("dbo.RepoIncidentComment");
		DropTable("dbo.RepoIncidentCall");
		DropTable("dbo.RepoDynamicalDataTypesAndOptions");
		DropTable("dbo.RepoIncidentActivityLog");
		DropTable("dbo.RepoIncident");
		DropTable("dbo.RepoIncidentSequence");
		DropTable("dbo.RepoDraftSequence");
	}
}
