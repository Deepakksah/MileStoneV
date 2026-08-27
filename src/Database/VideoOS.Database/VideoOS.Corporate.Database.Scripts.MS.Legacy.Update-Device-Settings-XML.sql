-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Update Settings XML on Hardware Table. Sets type attributes in XML so it 
-- isn't neccesary to load DefinitionXML to get settings values

-- Temporary STOREPROCEDURE to insert typeid attribute in settings column on devices
-- Will be deleted after script have been run
SET ANSI_PADDING OFF
GO

CREATE PROCEDURE [xml_device_setting_insert_attributes]
	@IDHardware [GuidType]
AS
	-- DELETES EXSISTING ATTRIBUTES!
	UPDATE DEVICES
	SET Settings.modify('delete (/properties/settings/setting/@typeid)')
	WHERE IDHardware = @IDHardware

	DECLARE @DefinitionXml xml
	DECLARE @idoc int
	
	SELECT @DefinitionXml = (	SELECT [Definition]
								FROM Hardware 
								WHERE [IDHardware] = @IDHardware )
	
	-- Temporary table that holds all capabilities for a given hardware
	DECLARE @Capabilities TABLE(	[Hwid]				NVARCHAR(128) NOT NULL, 
									[CapabilityName]	NVARCHAR(128) NOT NULL, 
									[TypeName]			NVARCHAR(128) NOT NULL )

	DECLARE @Types        TABLE(	[Name]			NVARCHAR(128) NOT NULL,
									[ReferenceId]	NVARCHAR(128) NOT NULL )

	-- Get capabilities out of the hardware definition xml
	EXEC sp_xml_preparedocument @idoc OUTPUT, @DefinitionXml

	INSERT INTO @Capabilities 
	SELECT	[Hwid],
			[CapabilityName],
			[TypeName]
	FROM OPENXML (@idoc, '//capabilities/capability')
		WITH 
		(
			[Hwid]					NVARCHAR(128) '../@hwid',
			[CapabilityName]		NVARCHAR(128) 'name',
			[TypeName]				NVARCHAR(128  ) '@type'		
		)

	-- Get types out of the hardware definition xml  
	INSERT INTO @Types 
	SELECT	[TypeName],
			UPPER([TypeReferenceId])
	FROM OPENXML (@idoc, '/definition/types/type')
		WITH 
		(
			[TypeName]				NVARCHAR(128) '@name',
			[TypeReferenceId]		nvarchar(128) '@referenceid'
		)

	EXEC sp_xml_removedocument @idoc

	DECLARE @IDDevice AS GuidType
	DECLARE @SettingXml XML
	DECLARE @settingstable Table(Setting XML not null)
	-- Temporary table that holds settings for the given device
	DECLARE @Settings		TABLE(	[Hwid]	nvarchar(128) NOT NULL, 
									[Name]	nvarchar(128) NOT NULL )


	-- Cursor that update the setting xml of the devices one at a time
	DECLARE devices_cursor CURSOR
		FOR SELECT [IDDevice], [Settings] FROM [Devices] WHERE IDHardware = @IDHardware

	OPEN devices_cursor

	FETCH NEXT FROM devices_cursor 
		INTO @IDDevice, @SettingXml

	WHILE @@FETCH_STATUS = 0
	BEGIN
		DELETE FROM @settingstable
		DELETE FROM @Settings
		-- Temporary table to hold updated settings xml. 
		INSERT INTO @settingstable values(@SettingXml) 

		-- Create an internal representation of the XML document.
		EXEC sp_xml_preparedocument @idoc OUTPUT, @SettingXml

		-- Populate the settings
		INSERT INTO @Settings 
		SELECT	[Hwid],	[Name]
		FROM OPENXML (@idoc, '//properties/settings/setting')
		WITH 
		(
			[Hwid]		NVARCHAR(128) '../@hwid',
			[Name]		NVARCHAR(128) 'name'		
		)
	
		EXEC sp_xml_removedocument @idoc

		-- Cursor to update the Settings XML. Will update the settings one at a time
		DECLARE @Hwid nvarchar(128)
		DECLARE @Name nvarchar(128)
		DECLARE @x XML
		DECLARE setting_cursor CURSOR
			FOR SELECT * FROM @Settings

		OPEN setting_cursor

		FETCH NEXT FROM setting_cursor 
			INTO @Hwid, @Name
		WHILE @@FETCH_STATUS = 0
		BEGIN

			SELECT @x = (SELECT TOP 1 Setting FROM @settingstable)

			DECLARE @myTypeReferenceId NVARCHAR(128) 

			SELECT @myTypeReferenceId = (	SELECT t.[ReferenceId] 
											FROM @Types t inner join @Capabilities c on t.[Name] = c.[TypeName]
											WHERE c.Hwid = @Hwid and c.[CapabilityName] = @Name ) 

			SET @x.modify('insert attribute typeid {sql:variable("@myTypeReferenceId")} as last into
			(/properties/settings[@hwid=sql:variable("@Hwid")]/setting[name=sql:variable("@Name")])[1]')

			UPDATE @settingstable Set Setting = @x

		FETCH NEXT FROM setting_cursor 
			INTO @Hwid, @Name
		END

		CLOSE setting_cursor
		DEALLOCATE setting_cursor
	
		-- Update the device with the new setting
		UPDATE Devices
		SET Settings = (SELECT TOP 1 Setting FROM @settingstable)
		WHERE IDDevice = @IDDevice	

		FETCH NEXT FROM devices_cursor 
			INTO @IDDevice, @SettingXml
	END

	CLOSE devices_cursor
	DEALLOCATE devices_cursor
GO

-- UPDATE Devices

DECLARE @IDHardware AS GuidType

DECLARE hardware_cursor CURSOR
	FOR SELECT [IDHardware] FROM [Hardware]

OPEN hardware_cursor

FETCH NEXT FROM hardware_cursor 
	INTO @IDHardware

WHILE @@FETCH_STATUS = 0
BEGIN
	DECLARE @idoc int

	EXEC [xml_device_setting_insert_attributes] @IDHardware

	FETCH NEXT FROM hardware_cursor 
		INTO @IDhardware
END

CLOSE hardware_cursor
DEALLOCATE hardware_cursor
GO

-- delete temporary storeprocedure
DROP PROCEDURE [xml_device_setting_insert_attributes]
GO

SET ANSI_PADDING ON
GO
