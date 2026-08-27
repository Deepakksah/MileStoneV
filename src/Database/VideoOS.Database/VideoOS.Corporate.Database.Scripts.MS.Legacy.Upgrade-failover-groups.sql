-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Create Failover Servers Root
DECLARE @CurrentUtcTime DATETIME
SET @CurrentUtcTime = GETUTCDATE()

IF NOT EXISTS(SELECT * FROM [ServerGroup] WHERE [IDServerGroup] = '6A85FFC3-C134-4338-A072-024817CB5820' AND [StructureType] = 'FAILOVER_GROUP')
INSERT [ServerGroup] ([IDServerGroup], [Name], [Description], [IDParentGroup], [StructureType], [OrderNo], [Level], [Path], [CreatedDate], [LastModified])
VALUES(
	'6A85FFC3-C134-4338-A072-024817CB5820',
	'FAILOVER_GROUP',
	'',
	'',
	'FAILOVER_GROUP',
	1,
	0,
	'\Failover',
	@CurrentUtcTime,
	@CurrentUtcTime
)
GO

-- Create server groups for existing failover server groups - except for "unused failover servers"
DECLARE @CurrentUtcTime DATETIME
SET @CurrentUtcTime = GETUTCDATE()

INSERT ServerGroup
	SELECT 
		IDFailoverGroup, Name, Description, '6A85FFC3-C134-4338-A072-024817CB5820', 'FAILOVER_GROUP', 0, 1, '\Failover\' + Name, @CurrentUtcTime, @CurrentUtcTime 
	FROM 
		FailoverGroups 
	WHERE IDFailoverGroup != '1F812D00-A152-4C29-AF2E-CB3387A93833'
GO

-- Move all failover servers except "unused failover server members" to their new server groups
INSERT ServerGroupMembership
	SELECT 
		IDFailoverGroup, IDFailoverServer, 'FAILOVERSERVER', GETUTCDATE()
	FROM
		FailoverServers
	WHERE IDFailoverGroup != '1F812D00-A152-4C29-AF2E-CB3387A93833'
GO

-- Move "unused failover server members" to server group root
INSERT ServerGroupMembership
	SELECT 
		'6A85FFC3-C134-4338-A072-024817CB5820', IDFailoverServer, 'FAILOVERSERVER', GETUTCDATE()
	FROM
		FailoverServers
	WHERE IDFailoverGroup = '1F812D00-A152-4C29-AF2E-CB3387A93833'
GO


IF EXISTS (SELECT name FROM sys.objects WHERE name = 
            N'FK_FailoverRecorders_FailoverGroups')
BEGIN
	ALTER TABLE [FailoverServers]
		DROP CONSTRAINT FK_FailoverRecorders_FailoverGroups
END
GO

IF EXISTS (SELECT name FROM sys.indexes WHERE name = 
            N'IX_UniqueFailoverRecorderNameInGroup')
BEGIN
	ALTER TABLE [FailoverServers]
		DROP CONSTRAINT IX_UniqueFailoverRecorderNameInGroup 
END
GO

IF EXISTS (SELECT name FROM sys.indexes WHERE name = 
            N'IX_UniqueSequenceInGroup')
BEGIN
	ALTER TABLE [FailoverServers]
		DROP CONSTRAINT IX_UniqueSequenceInGroup 
END
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[FailoverServers]') AND name = N'IDFailoverGroup')
BEGIN
	ALTER TABLE [FailoverServers]
	ALTER COLUMN IDFailoverGroup [GuidType] NULL
	
	UPDATE FailoverServers
	SET IDFailoverGroup = NULL
END
GO

DROP TABLE FailoverGroups
GO