-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================


-- Initialize the ProcedureName for error messages.

SET NOCOUNT ON

DECLARE @procedureName SYSNAME
SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + 'install_detectedeviceconfig'
DECLARE @ErrorArgs NVARCHAR(500)

BEGIN TRY
	BEGIN TRANSACTION

	-- created default Detected Device Configurations	
	DECLARE @settingName nvarchar(256) 
	SET @settingName = 'AutomaticDeviceDetectionActive'

	DECLARE @value nvarchar(256) 
	SET @value = 'False'
	
	IF(NOT EXISTS(SELECT [Name] FROM [SystemSettings] WHERE [Name] = @settingName))
	BEGIN
		INSERT INTO SystemSettings
		VALUES
		(
			GetUtcDate(), @settingName, @value
		)		
	END
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
GO