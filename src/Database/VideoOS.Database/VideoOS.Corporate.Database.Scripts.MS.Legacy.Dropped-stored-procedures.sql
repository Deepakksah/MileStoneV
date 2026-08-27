-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_query_recorder_by_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_query_recorder_by_id]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_recording_server]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_recording_server]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_recording_servers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_recording_servers]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[recorder_create_or_update]') AND type = 'P')
DROP PROCEDURE [recorder_create_or_update]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[scs_query_recorder_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [scs_query_recorder_ids]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_query_failover_recorder_by_name]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_query_failover_recorder_by_name]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_query_failover_recorders]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_query_failover_recorders]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_update_failover_recorder]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_update_failover_recorder]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_create_failover_recorder]') AND type = 'P')
DROP PROCEDURE [admin_create_failover_recorder]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_tasks]') AND type = 'P')
DROP PROCEDURE [query_tasks]
GO