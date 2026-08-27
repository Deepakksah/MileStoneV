-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT 1 FROM [SystemSettings] WHERE [Name] = 'DashboardHistoryEnabled')
BEGIN
    UPDATE [SystemSettings]
    SET [Setting] = 'False'
    WHERE [Name] = 'DashboardHistoryEnabled'
END

IF EXISTS(SELECT 1 FROM [SystemSettings] WHERE [Name] = 'DashboardHistorySamplingIntervalSeconds')
BEGIN
    UPDATE [SystemSettings]
    SET [Setting] = '1800'
    WHERE [Name] = 'DashboardHistorySamplingIntervalSeconds'
END
GO