-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_create_mip_object]') AND type = 'P')
DROP PROCEDURE [admin_create_mip_object]
GO

CREATE PROCEDURE [admin_create_mip_object]
	@pkid [UNIQUEIDENTIFIER],
	@parent_id [GuidType],
	@Name [NameType],
	@Kind [UNIQUEIDENTIFIER],
	@SubKind [UNIQUEIDENTIFIER],
	@PluginId [UNIQUEIDENTIFIER],
	@CustomData NVARCHAR(MAX)
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
	
		DECLARE @tmpId [GuidType]
	
		-- Verify that the server group exists
		SELECT @tmpId = [IDServerGroup]
		FROM [ServerGroup] WITH(UPDLOCK)
		WHERE [IDServerGroup] = @parent_id

		IF(@tmpId IS NULL)
		BEGIN
			;THROW 61012, @procedureName, 1;
		END

		INSERT [MIPObject] (
			[IDMIPObject],
			[LastModified],
			[CreatedDate],
			[Name],
			[Kind],
			[SubKind],
			[PluginId],
			[CustomData]
		)
		VALUES (
			@pkid,
			GETUTCDATE(),
			GETUTCDATE(),
			@Name,
			@Kind,
			@SubKind,
			@PluginId,
			@CustomData
		)

		-- Register object as a server group member
		EXEC [insert_server_group_member]
			@IDObject = @pkid,
			@IDServerGroup = @parent_id,
			@ObjectType = 'MIPOBJECT'
			
		-- Get DataSet after changes
		DECLARE @dataSetAfter XML
		SET @dataSetAfter = dbo.serverGroupMemberAsDataSet(@parent_id, @pkid)

		-- Create diffgram
		EXEC [create_task_transaction]
			@type = 'diffgram_insert',
			@target = '909749AE-88C4-4780-A381-383D691E98FA',
			@data_a = @dataSetAfter,
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