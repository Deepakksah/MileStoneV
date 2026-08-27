-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Script takes tables containing inserts, updates and deletes for each entity and
-- performs the required CUD, cleans up orphaned license records from deleted items

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF (EXISTS (SELECT 1 FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[dbo].[admin_update_license_values]') AND [type] IN (N'P', N'PC')))
BEGIN
	DROP PROC [dbo].[admin_update_license_values];
END
GO

CREATE PROCEDURE [dbo].[admin_update_license_values] 
	@RecorderInserts AS tvp_LicenseUpdateTable READONLY,
	@RecorderUpdates AS tvp_LicenseUpdateTable READONLY,
	@RecorderDeletes AS tvp_LicenseUpdateTable READONLY,
	@HardwareInserts AS tvp_LicenseUpdateTable READONLY,
	@HardwareUpdates AS tvp_LicenseUpdateTable READONLY,
	@HardwareDeletes AS tvp_LicenseUpdateTable READONLY,
	@DeviceInserts AS tvp_LicenseUpdateTable READONLY,
	@DeviceUpdates AS tvp_LicenseUpdateTable READONLY,
	@DeviceDeletes AS tvp_LicenseUpdateTable READONLY
AS
BEGIN
	SET NOCOUNT ON

	-- delete recorder devices which are selected for deletion
	DELETE lr
	FROM [LicenseRecorder] lr
		 INNER JOIN @RecorderDeletes ru ON lr.IDRecorder = ru.Id

	-- delete hardware licenses that are selected for deletion
	DELETE lhw
	FROM [LicenseHardware] lhw
		 INNER JOIN @HardwareDeletes hu ON lhw.IDHardware = hu.Id

	-- delete device licenses that are selected for deletion
	DELETE ld
	FROM [LicenseDevice] ld
		 INNER JOIN @DeviceDeletes du ON ld.IDDevice = du.Id

	-- delete duplicate hardware records that have same id but different unique (generally hw that is replaced)
	DELETE lhw
	FROM [LicenseHardware] lhw
		 INNER JOIN @HardwareInserts hu ON lhw.IDHardware = hu.Id AND hu.UniqueId <> lhw.UniqueId
	DELETE lhw
	FROM [LicenseHardware] lhw
		 INNER JOIN @HardwareUpdates hu ON lhw.IDHardware = hu.Id AND hu.UniqueId <> lhw.UniqueId

	-- delete duplicate device records that have same id but different unique (generally devices that have been replaced)
	DELETE ld
	FROM [LicenseDevice] ld
		INNER JOIN @DeviceInserts du ON ld.IDDevice = du.Id AND du.UniqueId <> ld.UniqueId
	DELETE ld
	FROM [LicenseDevice] ld
		INNER JOIN @DeviceUpdates du ON ld.IDDevice = du.Id AND du.UniqueId <> ld.UniqueId

	-- insert new recorder licenses
	INSERT INTO [LicenseRecorder] ([LastModified], [IDRecorder])
		SELECT CURRENT_TIMESTAMP, Id
		FROM @RecorderInserts ru
		WHERE NOT EXISTS (SELECT 1 FROM [LicenseRecorder] WHERE [IDRecorder] = ru.Id)

	-- insert new hardware licenses
	INSERT INTO [LicenseHardware] ([LastModified], [IDHardware], [License], [UniqueId])
		SELECT CURRENT_TIMESTAMP, Id, Expires, UniqueId
		FROM   @HardwareInserts hu

	-- insert new device licenses
	INSERT INTO [LicenseDevice] ([LastModified], [IDDevice], [License], [UniqueId])
		SELECT CURRENT_TIMESTAMP, Id, Expires, UniqueId
		FROM   @DeviceInserts du

	-- update hardware licenses that have changed
	UPDATE [LicenseHardware]
	SET    LastModified = CURRENT_TIMESTAMP,
		   License = hu.Expires,
		   UniqueId = hu.UniqueId
	FROM   [LicenseHardware] lh
		   INNER JOIN @HardwareUpdates hu ON lh.IDHardware = hu.Id

	-- update device licenses that have changed
	UPDATE [LicenseDevice]
	SET    LastModified = CURRENT_TIMESTAMP,
		   License = du.Expires,
		   UniqueId = du.UniqueId
	FROM   [LicenseDevice] ld
		   INNER JOIN @DeviceUpdates du ON ld.IDDevice = du.Id

	-- tidy up other orphaned hardware records, those where the hardware has been deleted but the license record still exists
	DELETE
	FROM [LicenseHardware]
	WHERE [IDHardware] NOT IN (SELECT CAST([IDHardware] AS UNIQUEIDENTIFIER) FROM [Hardware])

	-- tidy up other orphaned device records, those where the device has been deleted but the license record still exists
	DELETE
	FROM [LicenseDevice]
	WHERE [IDDevice] NOT IN (SELECT CAST([IDDevice] AS UNIQUEIDENTIFIER) FROM [Devices])

	-- tidy up other orphaned recorder records, those where the device has been deleted but the license record still exists
	DELETE
	FROM [LicenseRecorder]
	WHERE [IDRecorder] NOT IN (SELECT CAST([IDRecorder] AS UNIQUEIDENTIFIER) FROM [Recorders])
END
GO