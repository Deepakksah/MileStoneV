-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_serverkind_group]') AND type in (N'P', N'PC'))
DROP PROCEDURE [delete_serverkind_group]
GO

CREATE PROCEDURE [delete_serverkind_group]
	@IDServerGroup [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRAN

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		DECLARE @Name [NameType]
		DECLARE @IDParentGroup [GuidType]

		SELECT @IDParentGroup = [IDParentGroup],
			@Name = [Name]
		FROM [ServerGroup] SG WITH(UPDLOCK)
		INNER JOIN [ServerGroupKind] SGK on SGK.[IDServerGroup] = SG.[IDServerGroup]
		WHERE SG.[IDServerGroup] = @IDServerGroup

		-- Does the server group exist?
		IF(@Name IS NULL)
		BEGIN
			;THROW 60059, @procedureName, 1;
		END

		-- Is this the primary group
		IF(@IDParentGroup = '')
		BEGIN
			;THROW 60075, @procedureName, 1;
		END
	
		-- Does the server group contain any members
		DECLARE @CountMembers INT;

		SELECT @CountMembers = COUNT(IDObject) FROM [ServerGroupMembership] sgm
		WHERE sgm.[IDServerGroup] = @IDServerGroup

		IF(@CountMembers <> 0)
		BEGIN
			;THROW 60074, @procedureName, 1;
		END;

		SELECT @CountMembers = COUNT(IDServerGroup) FROM [ServerGroup] sg
		WHERE sg.[IDParentGroup] = @IDServerGroup

		IF(@CountMembers <> 0)
		BEGIN
			;THROW 60074, @procedureName, 1;
		END;

		-- Delete the server group kind
		DELETE FROM [ServerGroupKind]
		WHERE [IDServerGroup] = @IDServerGroup

		-- Get data set before changes
		DECLARE @dataSetBefore XML
		SET @dataSetBefore = dbo.serverGroupAsDataSet(@IDServerGroup)

		-- Delete the server group and
		DELETE FROM [ServerGroup]
		WHERE [IDServerGroup] = @IDServerGroup
			
		-- Create diffgram
		EXEC [create_task_transaction]
			@type = 'diffgram_delete',
			@target = '909749AE-88C4-4780-A381-383D691E98FA',
			@data_a = @dataSetBefore,
			@current_time = @CurrentUtcTime

		COMMIT TRAN
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