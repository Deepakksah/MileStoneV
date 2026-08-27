-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- First we rename FailoverRecorderGroups -> FailoverGroups
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[FailoverRecorderGroups]') AND type = N'U')
EXEC sp_rename 'FailoverRecorderGroups', 'FailoverGroups'
GO

-- Now let's a a type column to indicate the type of the group
-- This is current used to mark the default group.
-- 0: Default; 1: User-defined
IF NOT EXISTS(SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[FailoverGroups]') AND name = N'Type')
ALTER TABLE [FailoverGroups]
ADD [Type] TINYINT DEFAULT (1)
GO

DECLARE @defaultFailoverGroup [GuidType]
SET @defaultFailoverGroup = '1F812D00-A152-4C29-AF2E-CB3387A93833'

-- Add the default group
IF NOT EXISTS (SELECT * FROM [FailoverGroups] WHERE [IDFailoverGroup] = @defaultFailoverGroup)
INSERT [FailoverGroups] ([IDFailoverGroup], [Name], [Description], [Type], [LastModified])
VALUES(
	@defaultFailoverGroup,
	'Unused Failover Servers',
	'This group contains failover servers that are not currently in use',
	0,
	GETUTCDATE()
)
GO

-- Drop existing index on failover groups
IF EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[FailoverGroups]') AND name = N'IX_FailoverRecorderGroupName')
DROP INDEX [FailoverGroups].[IX_FailoverRecorderGroupName]
GO

-- Add a constraint to ensure that two groups doesn't have the same parent and same name
IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[FailoverGroups]') AND name = N'IX_UniqueGroupNameInGroup')
ALTER TABLE [FailoverGroups]
ADD CONSTRAINT [IX_UniqueGroupNameInGroup] UNIQUE NONCLUSTERED
(
	[Name] ASC,
	[IDParentFailoverGroup] ASC
) ON [PRIMARY]
GO

-- Drop existing index on failover recorders
IF EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[FailoverRecorders]') AND name = N'IX_FailoverRecorderName')
DROP INDEX [FailoverRecorders].[IX_FailoverRecorderName]
GO

-- Add a constraint to ensure that two failover recorders doesn't have the same name in the same group
IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[FailoverRecorders]') AND name = N'IX_UniqueFailoverRecorderNameInGroup')
ALTER TABLE [FailoverRecorders]
ADD CONSTRAINT [IX_UniqueFailoverRecorderNameInGroup] UNIQUE NONCLUSTERED
(
	[Name] ASC,
	[IDFailoverGroup] ASC
) ON [PRIMARY]
GO

-- Drop default empty string for description - this will be handled in validation
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'DF_FailoverRecorders_Description') AND type = N'D')
ALTER TABLE [FailoverRecorders]
DROP CONSTRAINT [DF_FailoverRecorders_Description]
GO

-- Drop default LastModified - this will be handled during create and update
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'DF_FailoverRecorders_LastModified') AND type = N'D')
ALTER TABLE [FailoverRecorders]
DROP CONSTRAINT [DF_FailoverRecorders_LastModified]
GO
