-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_query_all_slave_site_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_query_all_slave_site_ids]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_bookmarks_by_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_bookmarks_by_ids]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_device_playback_from_edgestorage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_device_playback_from_edgestorage]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_hardware_drivers_by_recorder]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_hardware_drivers_by_recorder]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_hardware_driver_by_recorder_and_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_hardware_driver_by_recorder_and_id]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_devices_enabled_by_recorder_and_sid]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_devices_enabled_by_recorder_and_sid]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_hardware_enabled_by_recorder]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_hardware_enabled_by_recorder]
GO

