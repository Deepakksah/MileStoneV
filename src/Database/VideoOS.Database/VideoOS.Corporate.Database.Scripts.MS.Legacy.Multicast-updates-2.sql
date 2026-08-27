-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @recorderId [GuidType]
DECLARE @multicastSettings NVARCHAR(MAX)
DECLARE @multicastSettingsXml xml

-- Add MulticastEnabled setting to MulticastSettings xml for all existing recorders
DECLARE recorder_cursor CURSOR FOR 
SELECT [IDRecorder], [MulticastSettings] FROM [Recorders]
OPEN recorder_cursor

FETCH NEXT FROM recorder_cursor INTO @recorderId, @multicastSettings

WHILE @@FETCH_STATUS = 0
BEGIN
	set @multicastSettingsXml = convert (xml, @multicastSettings)
	
	SET @multicastSettingsXml.modify('
	insert <setting><name>MulticastEnabled</name><value>True</value></setting>
	as first
	into (/properties/settings)[1]') 

	UPDATE [Recorders]
	SET [MulticastSettings] = convert (NVARCHAR(MAX), @multicastSettingsXml)
	WHERE [IDRecorder] = @recorderId

	FETCH NEXT FROM recorder_cursor INTO @recorderId, @multicastSettings
END
GO

CLOSE recorder_cursor
DEALLOCATE recorder_cursor