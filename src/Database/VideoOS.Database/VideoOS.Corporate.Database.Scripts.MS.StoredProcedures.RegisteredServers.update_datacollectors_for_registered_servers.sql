-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_datacollectors_for_registered_servers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_datacollectors_for_registered_servers]
GO

CREATE PROCEDURE [update_datacollectors_for_registered_servers]
	@ServerId [UNIQUEIDENTIFIER],
	@DataColletorId INT

AS	
BEGIN
	SET NOCOUNT ON
 
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @rowCount INT

		BEGIN TRAN

		UPDATE [RegisteredServers]
		SET [DataCollectorId] = @DataColletorId
		WHERE [IDServer] = @ServerId

		SET @rowCount = @@ROWCOUNT
		IF(@rowCount <> 1)
		BEGIN
			;THROW 60000, @procedureName, 1;
		END

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