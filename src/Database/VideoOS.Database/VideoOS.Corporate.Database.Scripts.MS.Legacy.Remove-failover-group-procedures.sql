-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_update_failover_group]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_update_failover_group]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_query_failover_groups]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_query_failover_groups]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_query_failover_group_by_name]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_query_failover_group_by_name]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_query_failover_group_by_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_query_failover_group_by_id]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_delete_failover_group]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_delete_failover_group]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_create_failover_group]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_create_failover_group]
GO
