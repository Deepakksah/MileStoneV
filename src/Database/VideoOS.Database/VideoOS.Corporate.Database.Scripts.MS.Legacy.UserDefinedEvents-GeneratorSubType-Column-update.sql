-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- USE CCA_Surveillance_50_Genericevents

-- Introduce support for marking of generic events and pre-defined events in the user-defined events table
-- This is done by adding a new column named "GeneratorSubType". The values allowed here are as specified by PHN
-- "UserDefined", indicating a traditional user-defined event.
-- "System", indicating that this is that what in the UI is decided by JRA to be called a "Pre-defined event".
-- "Generic", indicating that this is a shadow-event for a generic event

-- If there is no column named "GeneratorSubType" in the table named "EventTypes", create it as and fill it with "unset" values
IF NOT EXISTS (
	SELECT * 
	FROM sys.columns 
	WHERE object_id = OBJECT_ID(N'[EventTypes]') AND 
		name = N'GeneratorSubType')
BEGIN
	-- Using VARCHAR(36) for an enum. This seems much in line with what other scripts have done. It allows for GUIDs if that is what you wish.
	-- For backwards compatibility, we need a default of 'UserDefined' and NOT NULL, so all old clients will work like before
	-- It is not allowed to alter the DEFAULT later, so we need to set it to 'UserDefined' initially.
	-- If we allow null initially, we can't change back to not null later, because of the DEFAULT constraint
	ALTER TABLE [EventTypes] ADD [GeneratorSubType] VARCHAR(36) NULL
END
GO

-- Internal initial implementation (just remove - testers must add events again)
DELETE [EventTypes] WHERE [IDGenerator]='D1AF534B-1048-4D15-BC4D-A88AF82E8D5B';

-- The new column must be filled with correct values for all rows which were not affected above.
-- The built-in events labeled "pre-defined" in the UI will have subtype "System"
UPDATE [EventTypes] SET [GeneratorSubType]='System' WHERE [GeneratorSubType] IS NULL AND [GeneratorType]='External' AND [Builtin] = 1

-- The not-built-in events labeled "user-defined" in the UI will have subtype "UserDefined"
UPDATE [EventTypes] SET [GeneratorSubType]='UserDefined' WHERE [GeneratorSubType] IS NULL AND [GeneratorType]='External' AND [Builtin] = 0

-- For all event types where the GeneratorType is not external, the new column is irrelevant. We set those values to an empty string, so as to mark that we have processed them
UPDATE [EventTypes] SET [GeneratorSubType]='' WHERE [GeneratorSubType] IS NULL

ALTER TABLE [EventTypes] ALTER COLUMN [GeneratorSubType] VARCHAR(36) NOT NULL
GO
