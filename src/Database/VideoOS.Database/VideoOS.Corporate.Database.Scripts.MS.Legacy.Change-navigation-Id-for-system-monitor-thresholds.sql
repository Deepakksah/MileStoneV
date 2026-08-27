-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @navigation XML
SET @navigation = (SELECT CAST(Setting as XML) FROM SystemSettings WHERE Name = 'NavigationXml')

DECLARE @sysDashboardNode XML
SET @sysDashboardNode = @navigation.query('/main/entry[@ID="Root"]/entry[@ID="SystemDashboard"]/entry[@ID="Thresholds"]')

IF (LEN(CONVERT(NVARCHAR(MAX), @sysDashboardNode)) > 0)
BEGIN

SET NOCOUNT ON
	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE 
	BEGIN TRY
		BEGIN TRANSACTION UPDATETRANSACTIONID
			SET @navigation.modify('replace value of (/main/entry[@ID="Root"]/entry[@ID="SystemDashboard"]/entry[@ID="Thresholds"]/@ID)[1] with "SystemMonitorThreshold"')
		UPDATE SystemSettings
		SET Setting = CONVERT(NVARCHAR(MAX), @navigation)
		WHERE Name = 'NavigationXml'
		COMMIT TRANSACTION UPDATETRANSACTIONID
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION UPDATETRANSACTIONID
    SELECT
        ERROR_NUMBER() AS ErrorNumber
        ,ERROR_SEVERITY() AS ErrorSeverity
        ,ERROR_STATE() AS ErrorState
        ,ERROR_PROCEDURE() AS ErrorProcedure
        ,ERROR_LINE() AS ErrorLine
        ,ERROR_MESSAGE() AS ErrorMessage;
	END CATCH

END
GO