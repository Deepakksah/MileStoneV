-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dataset_hardware_select_by_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dataset_hardware_select_by_ids]
GO

CREATE PROCEDURE [dataset_hardware_select_by_ids]
	@ids XML
AS
BEGIN
	SELECT [LastModified], [IDHardware], [IDHardwareDriver], [Name], [Description], [URI], [LoginID], [EncryptedPassword], [IDRecorder], [Enabled], [C18NDigest], [Settings], [EventSettings], [VendorProductID], [DriverNumber], [DriverProxyName], [HostedHardware_id], [CameraData], [EdgeSettings], [SubType], [CredentialsLastModified], [HardwareDataLastUpdated]
	FROM [Hardware] h
	INNER JOIN XmlToIdTable(@ids) ids ON h.[IDHardware] = ids.[ID]
END
GO