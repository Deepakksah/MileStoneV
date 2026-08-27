-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_unregister_object_transaction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_unregister_object_transaction]
GO


IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[remove_server_group_member_transaction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [remove_server_group_member_transaction]
GO

-- Adds stored procedure to delete failover servers.
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_failover_server_transaction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [delete_failover_server_transaction]
GO


IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[change_failover_server_servicestate]') AND type = 'P')
DROP PROCEDURE [change_failover_server_servicestate]
GO

IF  EXISTS (
	SELECT * 
	FROM sys.columns 
	WHERE object_id = OBJECT_ID(N'[FailoverServers]') AND name = N'ServiceState')
BEGIN
	DECLARE @ConstraintName nvarchar(200)
	SELECT @ConstraintName = Name FROM SYS.DEFAULT_CONSTRAINTS
	WHERE PARENT_OBJECT_ID = OBJECT_ID('FailoverServers')
	AND PARENT_COLUMN_ID = (SELECT column_id FROM sys.columns
							WHERE NAME = N'ServiceState'
							AND object_id = OBJECT_ID(N'FailoverServers'))
	
	IF @ConstraintName IS NOT NULL

	EXEC('ALTER TABLE [FailoverServers] DROP CONSTRAINT ' + @ConstraintName)
END
GO


IF  EXISTS (
	SELECT * 
	FROM sys.columns 
	WHERE object_id = OBJECT_ID(N'[FailoverServers]') AND name = N'ServiceState')
BEGIN
	ALTER TABLE [FailoverServers] DROP COLUMN [ServiceState]
END
GO

IF  EXISTS (
	SELECT * 
	FROM sys.columns 
	WHERE object_id = OBJECT_ID(N'[FailoverServers]') AND name = N'IDFailoverGroup')
BEGIN
	ALTER TABLE [FailoverServers] DROP COLUMN [IDFailoverGroup]
END
GO
