-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_markeddata_by_device_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [delete_markeddata_by_device_ids]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_markeddata_recording_server_item]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_markeddata_recording_server_item]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_markeddata_device_item]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_markeddata_device_item]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_markeddata]') AND type in (N'P', N'PC'))
DROP PROCEDURE [create_markeddata]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_markeddata]') AND type in (N'P', N'PC'))
DROP PROCEDURE [delete_markeddata]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_markeddata]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_markeddata]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_bookmarks_by_device_id_before_time]') AND type in (N'P', N'PC'))
DROP PROCEDURE [delete_bookmarks_by_device_id_before_time]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_bookmarks_search_device_filter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_bookmarks_search_device_filter]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_bookmarks_search_no_device_filter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_bookmarks_search_no_device_filter]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_bookmarks_search]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_bookmarks_search]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_expired_markeddata]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_expired_markeddata]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_device_has_markeddata]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_device_has_markeddata]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_markeddata_item_nosecurity]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_markeddata_item_nosecurity]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_markeddata_search]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_markeddata_search]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_recording_server_has_markeddata]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_recording_server_has_markeddata]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_old_scheme_recorders_by_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_old_scheme_recorders_by_ids]
GO
