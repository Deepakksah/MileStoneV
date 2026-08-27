-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Add unique constraint to EventTypes
IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[EventTypes]') AND name = N'IX_UniqueExternalEventName')
ALTER TABLE [EventTypes]
ADD CONSTRAINT [IX_UniqueExternalEventName] UNIQUE NONCLUSTERED
(
	[Name] ASC,
	[GeneratorType] ASC
) ON [PRIMARY]
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[EventTypes]') AND name = N'IX_EventNameGeneratorType')
CREATE INDEX [IX_EventNameGeneratorType]
ON [EventTypes]( [Name], [GeneratorType] )
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[EventTypes]') AND name = N'IX_EventGeneratorType')
CREATE INDEX [IX_EventGeneratorType]
ON [EventTypes]( [GeneratorType] )
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SecurityRoles]') AND name = N'IX_RoleName')
CREATE UNIQUE INDEX [IX_RoleName]
ON [SecurityRoles]( [Name] )
GO
