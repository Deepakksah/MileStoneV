-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[install_servergroups]') AND type in (N'P', N'PC'))
DROP PROCEDURE [install_servergroups]
GO

CREATE PROCEDURE [install_servergroups]
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRANSACTION

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		DECLARE @IDViewGroupSecurityObject [GuidType]

		-- Create Failover Servers Root
		IF NOT EXISTS(SELECT * FROM [ServerGroup] WHERE [IDServerGroup] = '6A85FFC3-C134-4338-A072-024817CB5820' AND [StructureType] = 'FAILOVER_GROUP')
		BEGIN
			INSERT [ServerGroup] ([IDServerGroup], [Name], [Description], [IDParentGroup], [StructureType], [OrderNo], [Level], [Path], [CreatedDate], [LastModified])
			VALUES(
				'6A85FFC3-C134-4338-A072-024817CB5820',
				'FAILOVER_GROUP',
				'',
				'',
				'FAILOVER_GROUP',
				1,
				0,
				'\Failover',
				@CurrentUtcTime,
				@CurrentUtcTime
			)

			UPDATE [ServerGroup]
			SET [RelationIds] = dbo.GetServerGroupRelations('6A85FFC3-C134-4338-A072-024817CB5820')
			WHERE [IDServerGroup] = '6A85FFC3-C134-4338-A072-024817CB5820'
		END
	
		-- Create SmartClient Root
		IF NOT EXISTS(SELECT * FROM [ServerGroup] WHERE [IDServerGroup] = '430C2FE2-B233-42DA-B508-1A22EF03383D' AND [StructureType] = 'SMARTCLIENT_GROUP')
		BEGIN
			INSERT [ServerGroup] ([IDServerGroup], [Name], [Description], [IDParentGroup], [StructureType], [OrderNo], [Level], [Path], [CreatedDate], [LastModified])
			VALUES(
				'430C2FE2-B233-42DA-B508-1A22EF03383D',
				'SMARTCLIENT_GROUP',
				'',
				'',
				'SMARTCLIENT_GROUP',
				3,
				0,
				'\SmartClient',
				@CurrentUtcTime,
				@CurrentUtcTime
			)

			UPDATE [ServerGroup]
			SET [RelationIds] = dbo.GetServerGroupRelations('430C2FE2-B233-42DA-B508-1A22EF03383D')
			WHERE [IDServerGroup] = '430C2FE2-B233-42DA-B508-1A22EF03383D'
		END

		-- Create Private ViewGroups Root
		IF NOT EXISTS(SELECT * FROM [ServerGroup] WHERE [IDServerGroup] = '4BF8A8B0-54AB-4E82-994F-98D8E9584176' AND [StructureType] = 'VIEW_GROUP')
		BEGIN
			INSERT [ServerGroup] ([IDServerGroup], [Name], [Description], [IDParentGroup], [StructureType], [OrderNo], [Level], [Path], [CreatedDate], [LastModified])
			VALUES(
				'4BF8A8B0-54AB-4E82-994F-98D8E9584176',
				'4BF8A8B0-54AB-4E82-994F-98D8E9584176',
				'',
				'430C2FE2-B233-42DA-B508-1A22EF03383D',
				'VIEW_GROUP',
				1,
				1,
				'\SmartClient\Private',
				@CurrentUtcTime,
				@CurrentUtcTime
			)

			UPDATE [ServerGroup]
			SET [RelationIds] = dbo.GetServerGroupRelations('4BF8A8B0-54AB-4E82-994F-98D8E9584176')
			WHERE [IDServerGroup] = '4BF8A8B0-54AB-4E82-994F-98D8E9584176'
		END
	
		-- Create MIP Objects Root
		IF NOT EXISTS(SELECT * FROM [ServerGroup] WHERE [IDServerGroup] = 'ABF0C309-E9F6-41AF-AA1A-A48B45AA0683' AND [StructureType] = 'MIPOBJECT_GROUP')
		BEGIN
			INSERT [ServerGroup] ([IDServerGroup], [Name], [Description], [IDParentGroup], [StructureType], [OrderNo], [Level], [Path], [CreatedDate], [LastModified])
			VALUES(
				'ABF0C309-E9F6-41AF-AA1A-A48B45AA0683',
				'MIPOBJECT_GROUP',
				'',
				'',
				'MIPOBJECT_GROUP',
				1,
				0,
				'\MIPObject',
				@CurrentUtcTime,
				@CurrentUtcTime
			)

			UPDATE [ServerGroup]
			SET [RelationIds] = dbo.GetServerGroupRelations('ABF0C309-E9F6-41AF-AA1A-A48B45AA0683')
			WHERE [IDServerGroup] = 'ABF0C309-E9F6-41AF-AA1A-A48B45AA0683'
		END

		IF NOT EXISTS(SELECT * FROM [ServerGroupKind] WHERE [IDServerGroup] = 'ABF0C309-E9F6-41AF-AA1A-A48B45AA0683')
		INSERT [ServerGroupKind] ([IDServerGroup], [Kind], [PluginId])
		VALUES(
			'ABF0C309-E9F6-41AF-AA1A-A48B45AA0683',
			'00000000-0000-0000-0000-000000000000',
			'00000000-0000-0000-0000-000000000000'
		)

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH 
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION
		END;
		DECLARE @shouldReThrowError BIT = dbo.[ShouldReThrowError](ERROR_NUMBER(), ERROR_SEVERITY())
		IF(@shouldReThrowError = 1)
			THROW;
		SET @ErrorArgs = dbo.[FormatErrorMessage](ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE())
		;THROW 60000, @ErrorArgs, 1;
	END CATCH;
END
GO

EXEC [install_servergroups]
GO
