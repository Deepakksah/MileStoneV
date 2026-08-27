-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[restricted_media_create]') AND [type] IN (N'P', N'PC'))
DROP PROCEDURE [restricted_media_create]
GO

CREATE PROCEDURE [restricted_media_create]
	@IDRestrictedMedia  UNIQUEIDENTIFIER,
	@IDDevices          [tvp_StringTableGuidType] READONLY,
	@Header             [LongNameType],
	@Description        [DescriptionType2],
	@UserName           [NameType],
	@StartTime          DATETIME2,
	@EndTime            DATETIME2,
	@LastModified       DATETIME2,
	@session_id         [GuidType]
AS	
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRAN

		EXEC restricted_media_create_transaction @IDRestrictedMedia, @IDDevices, @Header, @Description, @UserName, @StartTime, @EndTime, @LastModified

		COMMIT TRAN

		DECLARE @AuditMediaRestrictionCreated INT = 7120 -- Log: Restricted media '{Header}' was created.
		EXEC create_pending_log @session_id, @AuditMediaRestrictionCreated, @IDRestrictedMedia, @Header
	END TRY
	BEGIN CATCH 
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION
		END;
		IF(ERROR_NUMBER() < 60000 AND ERROR_NUMBER() <> 1205 AND ERROR_NUMBER() <> 8628 AND ERROR_NUMBER() <> 8645 AND ERROR_NUMBER() <> 2801)
		BEGIN
			SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
			SET @ErrorArgs = dbo.[FormatErrorMessage](ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE())
			;THROW 60000, @ErrorArgs, 1;
		END
		;THROW
	END CATCH
END
GO