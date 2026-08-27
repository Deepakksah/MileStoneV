-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================


IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_query_failover_main_recorder]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_query_failover_main_recorder]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_query_failover_servers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_query_failover_servers]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_query_failover_server_by_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_query_failover_server_by_ids]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_query_failover_recorder_by_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_query_failover_recorder_by_id]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_query_failover_server_by_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_query_failover_server_by_id]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_query_failover_server_by_hostname]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_query_failover_server_by_hostname]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_update_failover_server]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_update_failover_server]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_move_failover_recorder]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_move_failover_recorder]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_move_failover_server]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_move_failover_server]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_change_failover_server_enabled]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_change_failover_server_enabled]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_change_failover_server_servicestate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_change_failover_server_servicestate]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_create_failover_server]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_create_failover_server]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[deviceGroupMembersAsDataSet]') AND type in (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [deviceGroupMembersAsDataSet]
GO
