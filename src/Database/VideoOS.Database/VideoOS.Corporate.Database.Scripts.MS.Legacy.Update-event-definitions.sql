-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Add BuiltIn to definition
UPDATE
	[EventTypes]
SET
	[Definition].modify('insert
						(
							element BuiltIn
							{
								if (xs:boolean(sql:column("BuiltIn"))) then
									"True"
								else
									"False"
							}
						)
						after (EventType/OccursGlobally)[1]')
	
-- Add Generator subtype to definition
UPDATE
	[EventTypes]
SET
	[Definition].modify('insert
						(
							element SubType { xs:string(sql:column("GeneratorSubType")) }
						)
						after (EventType/Generator/Type)[1]')
GO