-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================


-- Update Settings XML on Hardware Table. Sets type attributes in XML so it 
-- isn't neccesary to load DefinitionXML to get settings values
--
CREATE PROCEDURE [xml_hardware_setting_insert_attributes]
	@IDHardware [GuidType]
AS
	-- CREATE A TABLE WITH 
	DECLARE @DefinitionXml xml
	DECLARE @idoc int
	SELECT @DefinitionXml = (SELECT Definition from Hardware where IDHardware = @IDHardware)

	DECLARE @Capabilities TABLE(	[Hwid]				nvarchar(128) NOT NULL, 
									[CapabilityName]	nvarchar(128) NOT NULL, 
									[TypeName]			nvarchar(128) NOT NULL )

	DECLARE @Types        TABLE(	[Name]				nvarchar(128) NOT NULL,
									[ReferenceId]		nvarchar(128) NOT NULL )

	-- Create an internal representation of the XML document.
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
			[TypeName]				NVARCHAR(128) '@type'		
		)

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

	DECLARE @Settings		TABLE(	[Hwid]	nvarchar(128) NOT NULL, 
									[Name]	nvarchar(128) NOT NULL )
	
	DECLARE @SettingXml XML
	SELECT @SettingXml = ( SELECT Settings FROM Hardware WHERE IDHardware = @IDHardware )
	
	DECLARE @settingstable Table(Setting XML not null)
	INSERT INTO @settingstable values(@SettingXml) 

	-- Create an internal representation of the XML document.
	EXEC sp_xml_preparedocument @idoc OUTPUT, @SettingXml

	INSERT INTO @Settings 
	SELECT	[Hwid],	[Name]
	FROM OPENXML (@idoc, '//properties/settings/setting')
	WITH 
	(
		[Hwid]		NVARCHAR(128) '../@hwid',
		[Name]		NVARCHAR(128) 'name'		
	)

	EXEC sp_xml_removedocument @idoc

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

		DECLARE @myTypeReferenceId nvarchar(128) 

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
	
	UPDATE Hardware
	SET Settings = (SELECT TOP 1 Setting FROM @settingstable)
	WHERE IDHardware = @IDHardware
GO

-- DELETES EXSISTING ATTRIBUTES!
UPDATE Hardware
SET Settings.modify('delete (/properties/settings/setting/@typeid)')


DECLARE @IDHardware AS GuidType
DECLARE @DefinitionXml AS NVARCHAR(MAX)

DECLARE hardware_cursor CURSOR
	FOR SELECT [IDHardware] FROM [Hardware]

OPEN hardware_cursor

FETCH NEXT FROM hardware_cursor 
	INTO @IDhardware

WHILE @@FETCH_STATUS = 0
BEGIN
	DECLARE @idoc int

	EXEC [xml_hardware_setting_insert_attributes] @IDhardware

	FETCH NEXT FROM hardware_cursor 
		INTO @IDhardware
END

CLOSE hardware_cursor
DEALLOCATE hardware_cursor
GO

DROP PROCEDURE [xml_hardware_setting_insert_attributes]
GO
