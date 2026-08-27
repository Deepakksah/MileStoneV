-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================


IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_markeddata_items]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_markeddata_items]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_camera_and_output_devices_enabled_by_recorder]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_camera_and_output_devices_enabled_by_recorder]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_markeddata_item]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_markeddata_item]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_devices_enabled]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_devices_enabled]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_devices_enabled_by_recorder_and_roleids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_devices_enabled_by_recorder_and_roleids]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_metadatainfo_for_devices_enabled]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_metadatainfo_for_devices_enabled]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_metadatainfo_for_devices_enabled_by_recorder_and_roleids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_metadatainfo_for_devices_enabled_by_recorder_and_roleids]
GO


