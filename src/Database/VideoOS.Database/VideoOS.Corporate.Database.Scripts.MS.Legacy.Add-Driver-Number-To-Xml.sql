-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DELETE FROM HardwareDriverRecorder
GO

Update HardwareDefinition
set [Definition].modify('delete
			( 
				definition/driver/number
			)')
GO

Update [HardwareDefinition]
set [Definition].modify('insert
			( 
				element number {xs:string(sql:column("DriverNumber"))}
			)
			before (definition/driver/firmware)[1]')
	FROM (
		SELECT DriverNumber, C18NDigest
		FROM Hardware
		) as h
	where h.C18NDigest = [HardwareDefinition].C18NDigest
GO