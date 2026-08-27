-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================


IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_detected_device_credentials]') AND type in (N'P', N'PC'))
DROP PROCEDURE [create_detected_device_credentials]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_detected_device_info]') AND type in (N'P', N'PC'))
DROP PROCEDURE [create_detected_device_info]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_detected_device_credentials]') AND type in (N'P', N'PC'))
DROP PROCEDURE [delete_detected_device_credentials]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_detected_device_info]') AND type in (N'P', N'PC'))
DROP PROCEDURE [delete_detected_device_info]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_detected_device_credentials_by_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_detected_device_credentials_by_ids]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_detected_device_credentials_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_detected_device_credentials_ids]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_detected_device_info_by_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_detected_device_info_by_ids]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_detected_device_info_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_detected_device_info_ids]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_detected_device_credentials]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_detected_device_credentials]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_detected_device_info]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_detected_device_info]
GO


IF OBJECT_ID ('TR_Hardware_AfterInsert','TR') IS NOT NULL
   DROP TRIGGER TR_Hardware_AfterInsert;
GO

IF OBJECT_ID ('TR_Hardware_AfterDelete','TR') IS NOT NULL
   DROP TRIGGER TR_Hardware_AfterDelete;
GO


IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[DetectedDeviceInfo]') AND type in (N'U'))
DROP TABLE [DetectedDeviceInfo]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[DetectedDeviceCredentials]') AND type in (N'U'))
DROP TABLE [DetectedDeviceCredentials]
GO
