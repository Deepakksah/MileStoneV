-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Copy the Filter value to the type attribute
update [EventTypes]
set Definition.modify('insert attribute type {(/EventType/Sources/Source/Filters/Filter)[1]} into (/EventType/Sources/Source/Filters/Filter)[1]')
where Definition.value('(/EventType/Sources/Source/Filters/Filter)[1]', 'nvarchar(max)') <> ''
and Definition.exist('/EventType/Sources/Source/Filters/Filter/@type') = 0
GO

-- Remove the Filter value 
update [EventTypes]
set Definition.modify('delete (/EventType/Sources/Source/Filters/Filter/text())[1]')
where Definition.value('(/EventType/Sources/Source/Filters/Filter)[1]', 'nvarchar(max)') = Definition.value('(/EventType/Sources/Source/Filters/Filter/@type)[1]', 'nvarchar(max)')
GO

-- Insert Genrerator GroupId
update [EventTypes]
set Definition.modify('insert <GroupId/> as last into (/EventType/Generator)[1]')
where Definition.exist('/EventType/Generator/GroupId') = 0
GO

-- Insert Genrerator GroupName
update [EventTypes]
set Definition.modify('insert <GroupName/> as last into (/EventType/Generator)[1]')
where Definition.exist('/EventType/Generator/GroupName') = 0
GO