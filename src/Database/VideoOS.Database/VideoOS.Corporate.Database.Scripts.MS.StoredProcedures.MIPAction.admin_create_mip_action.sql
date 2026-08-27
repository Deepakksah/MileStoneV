-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_create_mip_action]') AND type = 'P')
DROP PROCEDURE [admin_create_mip_action]
GO

CREATE PROCEDURE [admin_create_mip_action]
	@IDMIPAction [UNIQUEIDENTIFIER],
	@Name [NameType],
	@DefiningPluginId [UNIQUEIDENTIFIER],
	@DefiningPluginName [NameType],
	@SelectionText [NameType],
	@DescriptionText [NameType],
	@PickerText [NameType],
	@MIPKinds [XML],
	@PickerText2 [NameType],
	@MIPKinds2 [XML]
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
	
		INSERT [MIPAction] (
			[IDMIPAction],
			[LastModified],
			[CreatedDate],
			[Name],
			[DefiningPluginId],
			[DefiningPluginName],
			[SelectionText],
			[DescriptionText],
			[PickerText],
			[MIPKinds],
			[PickerText2],
			[MIPKinds2]
		) 
		VALUES (
			@IDMIPAction,
			@CurrentUtcTime,
			@CurrentUtcTime,
			@Name,
			@DefiningPluginId,
			@DefiningPluginName,
			@SelectionText,
			@DescriptionText,
			@PickerText,
			@MIPKinds,
			@PickerText2,
			@MIPKinds2
		)

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