-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_device_links_from_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_device_links_from_id]
GO

CREATE PROCEDURE [query_device_links_from_id]
	@IDDevice [GuidType]
AS
BEGIN	
	SELECT [IDDevice]
		,[IDDeviceLinked]
		,[Priority]
		,[LastModified]
	FROM [DeviceLinks] WHERE [IDDevice] = @IDDevice
END
GO