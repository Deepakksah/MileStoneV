-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================


-- Add new column with Edge Track Id
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'EdgeTrackId' and OBJECT_ID = OBJECT_ID(N'Devices'))
ALTER TABLE [Devices]
ADD [EdgeTrackId] [GuidType] NULL
GO

DECLARE @deviceId [GuidType]

DECLARE device_ID_cursor CURSOR
FOR SELECT d.[IDDevice]
FROM [Devices] d
INNER JOIN [RegisteredDeviceTypes] rdt
ON rdt.[DeviceType] = d.[DeviceType]
WHERE rdt.[IsRecordingDevice] = 1
AND d.[EdgeTrackId] IS NULL

OPEN device_ID_cursor

FETCH NEXT FROM device_ID_cursor
INTO @deviceId

WHILE(@@FETCH_STATUS = 0)
BEGIN
	
	UPDATE [Devices]
	SET [EdgeTrackId] = NEWID()
	WHERE [IDDevice] = @deviceId
	
	FETCH NEXT FROM device_ID_cursor
	INTO @deviceId
END

CLOSE device_ID_cursor
DEALLOCATE device_ID_cursor

UPDATE [Devices]
SET [EdgeTrackId] = ''
WHERE [EdgeTrackId] IS NULL

GO

ALTER TABLE [Devices]
ALTER COLUMN[EdgeTrackId] [GuidType] NOT NULL
GO


