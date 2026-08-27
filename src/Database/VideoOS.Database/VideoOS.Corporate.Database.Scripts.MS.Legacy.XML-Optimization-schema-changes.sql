-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- The SUPPORTS_TRANSACTION property specifies if the scripts supports
-- running in multi-statement transaction or if it should be running
-- using implicit transactions
-- =========================================================================
-- TARGET: Surveillance
-- SUPPORTS_TRANSACTION: FALSE
-- =========================================================================

-- Enable CLR
EXEC sp_configure 'clr enabled' , '1';
RECONFIGURE;
GO

-- Add column to devices table for device index
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Devices]') AND name = N'DeviceIndex')
ALTER TABLE [Devices]
ADD [DeviceIndex] INT
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Hardware]') AND name = N'Definition')
BEGIN
	UPDATE [Devices]
	SET [DeviceIndex] = x.[DeviceHwid]
	FROM (
		SELECT 
			Device.value('@ID', 'char(36)') AS IDDevice,
			DeviceHwid.value('fn:substring(@hwid, fn:string-length(@hwid))', 'INT') AS DeviceHwid
		FROM [Hardware] h
		CROSS APPLY h.[Definition].nodes('/definition/hardware/devices/device') MyDevices(Device)
		OUTER APPLY Device.nodes('capabilities') MyDeviceHwids(DeviceHwid)
		INNER JOIN [Devices] d ON d.[IDDevice] = Device.value('@ID', 'char(36)')
	) AS x
	WHERE x.IDDevice = [Devices].[IDDevice]
END	
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Devices]') AND name = N'DeviceIndex')
ALTER TABLE [Devices]
ALTER COLUMN [DeviceIndex] INT NOT NULL
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[DF_DevicesStreams]') AND type = N'D')
ALTER TABLE [Devices]
DROP CONSTRAINT [DF_DevicesStreams]
GO

-- Change data type for streams column in device table
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Devices]') AND name = N'Streams')
ALTER TABLE [Devices]
ALTER COLUMN [Streams] XML
GO

-- Insert hwid and referenceid into stream xml on devices from hardware definition
DECLARE @IDHardware GuidType
DECLARE @DefinitionInfo TABLE(StreamHwid VARCHAR(30), IDReference GuidType, IDStream GuidType, IDDevice GuidType)

DECLARE hardware_cursor CURSOR
FOR SELECT
		IDHardware
	FROM Hardware

OPEN hardware_cursor
FETCH NEXT FROM hardware_cursor
	INTO @IDHardware
WHILE @@FETCH_STATUS = 0
BEGIN

	DELETE FROM @DefinitionInfo

	INSERT INTO @DefinitionInfo(IDDevice, IDStream, IDReference, StreamHwid)
	SELECT
		Stream.value('../../@ID', 'char(36)') AS IDDevice,
		Stream.value('@ID', 'char(36)') AS IDStream,
		Stream.value('@referenceid', 'char(36)') AS IDReference,
		Stream.value('capabilities[1]/@hwid', 'varchar(30)') AS StreamHwid
	FROM Hardware hw
	CROSS APPLY hw.[definition].nodes('/definition/hardware/devices/device/streams/stream') MyStreams(Stream)
	WHERE hw.IDHardware = @IDHardware
	
	DECLARE @IDDevice GuidType
	DECLARE @IDStream GuidType
	
	DECLARE device_stream_cursor CURSOR
	FOR SELECT StreamNode.value('(id)[1]', 'char(36)') AS StreamId, IDDevice
	FROM Devices
	CROSS APPLY [Streams].nodes('/streams/stream') AS R(StreamNode)
	WHERE IDHardware = @IDHardware


	OPEN device_stream_cursor
	FETCH NEXT FROM device_stream_cursor
		INTO @IDStream, @IDDevice
	WHILE @@FETCH_STATUS = 0
	BEGIN
		DECLARE @IDReference GuidType
		DECLARE @StreamHwid VARCHAR(30)
		
		SELECT @StreamHwid = (SELECT StreamHwid FROM @DefinitionInfo WHERE IDStream = @IDStream AND IDDevice = @IDDevice)
		SELECT @IDReference =(SELECT IDReference FROM @DefinitionInfo WHERE IDStream = @IDStream AND IDDevice = @IDDevice)
		
		UPDATE Devices
		SET Streams.modify('
			insert
			( 
				element hwid {xs:string(sql:variable("@StreamHwid"))},
				element referenceid {xs:string(sql:variable("@IDReference"))}
			)
			as last into ((streams/stream/id[text() = sql:variable("@IDStream")])/..)[1]
		')
		WHERE IDDevice = @IDDevice
		

	FETCH NEXT FROM device_stream_cursor
		INTO @IDStream, @IDDevice
	END

	CLOSE device_stream_cursor
	DEALLOCATE device_stream_cursor

FETCH NEXT FROM hardware_cursor 
	INTO @IDHardware
END

CLOSE hardware_cursor
DEALLOCATE hardware_cursor
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Hardware]') AND name = N'C18NDigest')
ALTER TABLE [Hardware]
ADD [C18NDigest] CHAR(24) NULL
GO

-- Delete all hardware IDs from Hardware table
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Hardware]') AND name = N'Definition')
UPDATE [Hardware]
SET [Definition].modify('
	delete
	(
		/definition/properties/settings[contains(@hwid, "device")],
		/definition/properties/settings[contains(@hwid, "hardware")],
		/definition/properties/settings[contains(@hwid, "stream")],
		/definition/hardware/@*[local-name()="ID"],
		/definition/hardware/devices/device/@*[local-name()="ID"],
		/definition/hardware/devices/device/streams/stream/@*[local-name()="ID"]
	)
')
GO

-- Removed all file-paths from implementation/ (assemblyfilename and tag)
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Hardware]') AND name = N'Definition')
WHILE EXISTS(
	SELECT Implementation.value('.', 'VARCHAR(256)')
	FROM [Hardware] hw
	CROSS APPLY hw.[Definition].nodes('//*/implementation/*') MyImplementation(Implementation)
	WHERE Implementation.value('.', 'varchar(200)') LIKE '%\%'
)
BEGIN
	UPDATE [Hardware]
	SET [Definition].modify('
		replace value of (
			(//*/implementation/*[local-name()=sql:column("x.fileType")][text() = sql:column("x.originalFilename")]/text())[1]
		)
		with (
			sql:column("newFilename")
		)
	')
	FROM (
		SELECT 
			hw.[IDHardware],
			Implementation.value('.', 'VARCHAR(256)') AS originalFilename,
			Implementation.value('local-name(.)', 'varchar(50)') AS fileType,
			'newFilename' = (
				CASE WHEN (CHARINDEX('\', Implementation.value('.', 'VARCHAR(256)')) <> 0) THEN
					RIGHT
					(
						Implementation.value('.[1]', 'VARCHAR(256)'),
						CHARINDEX
						(
							'\',
							REVERSE(Implementation.value('.[1]', 'VARCHAR(256)'))
						) - 1
					)
				ELSE
					Implementation.value('.', 'VARCHAR(256)')
				END)
		FROM Hardware hw
		CROSS APPLY hw.[Definition].nodes('//*/implementation/*') MyImplementation(Implementation)
		WHERE Implementation.value('.', 'varchar(200)') LIKE '%\%'
	) AS x
	WHERE x.IDHardware = [Hardware].[IDHardware]
END
GO

-- Change data type for definition column in HardwareDriverRecorder table
IF NOT EXISTS(
	SELECT * FROM sys.columns sc 
	INNER JOIN sys.types st ON sc.user_type_id = st.user_type_id
	WHERE 
		sc.object_id = OBJECT_ID(N'[HardwareDriverRecorder]') AND 
		sc.name = N'Definition' AND
		st.name = 'xml')
ALTER TABLE [HardwareDriverRecorder]
ALTER COLUMN [Definition] XML
GO

-- Delete all hardware IDs from HardwareDriverRecorder table
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[HardwareDriverRecorder]') AND name = N'Definition')
UPDATE [HardwareDriverRecorder]
SET [Definition].modify('
	delete
	(
		/definition/hardware/@*[local-name()="ID"],
		/definition/hardware/devices/device/@*[local-name()="ID"],
		/definition/hardware/devices/device/streams/stream/@*[local-name()="ID"]
	)
')
GO

-- Create new HardwareDefinition table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[HardwareDefinition]') AND type = N'U')
CREATE TABLE [HardwareDefinition] (
	[C18NDigest] CHAR(24) PRIMARY KEY,
	[Definition] XML
)
GO

-- First create new rows in HardwareDefinition table to contain
-- all unique original definitions from hardware table
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Hardware]') AND name = N'Definition')
INSERT [HardwareDefinition] (dbo.[C18NDigest])
SELECT dbo.[C18NDigest]([definition])
FROM [Hardware]
GROUP BY dbo.[C18NDigest]([definition])
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Hardware]') AND name = N'Definition')
BEGIN
	DECLARE @tmpTable AS TABLE (
		[IDHardware] GuidType,
		[C18NDigest] CHAR(24)
	)

	INSERT @tmpTable (IDHardware, [C18NDigest])
	SELECT h.IDhardware, dbo.[C18NDigest](h.[Definition])
	FROM [Hardware] h

	UPDATE h
	SET h.[C18NDigest] = tmp.[C18NDigest]
	FROM [Hardware] h
	INNER JOIN @tmpTable tmp ON tmp.[IDHardware] = h.[IDHardware]
END
GO

-- Copy all (unique) hardware definitions to new
-- HardwareDefinition table
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Hardware]') AND name = N'Definition')
UPDATE hd
SET hd.[Definition] = (
	SELECT TOP 1 h.[Definition]
	FROM [Hardware] h
	WHERE h.[C18NDigest] = hd.[C18NDigest])
FROM [HardwareDefinition] hd
GO

-- Do not allow NULLs as Definition
ALTER TABLE [HardwareDefinition]
ALTER COLUMN [Definition] XML NOT NULL
GO

-- Now we're ready to drop the original hardware definition column
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Hardware]') AND name = N'Definition')
ALTER TABLE [Hardware]
DROP COLUMN [Definition]
GO

-- Do not allow NULLs as C18NDigest
ALTER TABLE [Hardware]
ALTER COLUMN [C18NDigest] CHAR(24) NOT NULL
GO

-- Add foreign key to hardware that references 
-- the ID of the definition

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_Hardware_Definition]') AND parent_object_id = OBJECT_ID(N'[Hardware]'))
ALTER TABLE [Hardware] WITH CHECK
ADD CONSTRAINT [FK_Hardware_Definition]
	FOREIGN KEY([C18NDigest])
	REFERENCES [HardwareDefinition] ([C18NDigest])
GO

ALTER TABLE [Hardware] CHECK CONSTRAINT [FK_Hardware_Definition]
GO

-- Add unique index to Hardware.C18NDigest
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'Hardware') AND name = N'IX_C18NDigest')
CREATE NONCLUSTERED INDEX [IX_C18NDigest] ON [Hardware] ([C18NDigest]) ON [PRIMARY]
GO

EXEC sp_configure 'clr enabled' , '0';
RECONFIGURE;
