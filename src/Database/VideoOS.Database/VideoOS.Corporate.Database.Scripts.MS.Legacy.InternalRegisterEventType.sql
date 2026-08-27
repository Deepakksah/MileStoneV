-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[InternalRegisterEventType]') AND type in (N'P', N'PC'))
DROP PROCEDURE [InternalRegisterEventType]
GO

CREATE PROCEDURE [InternalRegisterEventType]
	@EventDefinitionXml AS NTEXT,
	@CurrentTime AS DATETIME
AS

	DECLARE @idoc INT
	DECLARE @IDEventType GuidType
	DECLARE @GeneratorType InternalIdType
	DECLARE @GeneratorSubType VARCHAR(36)
	DECLARE @IDGenerator GuidType
	DECLARE @Name NameType
	DECLARE @BuiltIn AS BIT

	-- Create an internal representation of the XML document.
	EXEC sp_xml_preparedocument @idoc OUTPUT, @EventDefinitionXml

	-- Execute a SELECT statement using OPENXML rowset provider.
	SELECT 
		@IDEventType = [IDEventType], 
		@Name = [Name],
		@GeneratorType = [GeneratorType],
		@GeneratorSubType = [GeneratorSubType],
		@IDGenerator = [IDGenerator],
		@BuiltIn = [BuiltIn]
	FROM OPENXML (@idoc, '/')
		WITH (IDEventType      GuidType       'EventType/Id',
		      Name             NameType       'EventType/Name',
		      GeneratorType    InternalIdType 'EventType/Generator/Type',
		      GeneratorSubType InternalIdType 'EventType/Generator/SubType',
		      IDGenerator      GuidType       'EventType/Generator/Id',
			  BuiltIn          BIT            'EventType/BuiltIn'
		)
	EXEC sp_xml_removedocument @idoc

	DELETE [EventTypes] WHERE [IDEventType] = @IDEventType

	INSERT INTO 
		EventTypes 
	Values (
	  @CurrentTime,
		@IDEventType,
		@EventDefinitionXml,
		@GeneratorType,
		@IDGenerator,
		@Builtin,
		@Name,
		@GeneratorSubType
	)
GO
