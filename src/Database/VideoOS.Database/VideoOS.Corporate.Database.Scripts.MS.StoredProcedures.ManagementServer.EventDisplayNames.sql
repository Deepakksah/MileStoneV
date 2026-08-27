-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[EventDisplayNames]') AND type in (N'P', N'PC'))
DROP PROCEDURE [EventDisplayNames]
GO

CREATE PROCEDURE [EventDisplayNames]
AS
BEGIN
	DECLARE @EventTypeNames TABLE ( [EventDisplayName] nvarchar(4000) )

	DECLARE @IDEventType char(36)
	DECLARE @Definition nvarchar(4000)

	DECLARE eventtype_cursor CURSOR FOR
	SELECT [Definition] FROM EventTypes

	OPEN eventtype_cursor
	BEGIN TRY
		FETCH NEXT FROM eventtype_cursor
		INTO @Definition

		WHILE @@FETCH_STATUS = 0
		BEGIN

			DECLARE @docHandle  int

			EXEC sp_xml_preparedocument @docHandle OUTPUT, @Definition

			INSERT INTO @EventTypeNames
			SELECT [Name]
			FROM OPENXML (@docHandle , '/')
			WITH ([Name] NameType 'EventType/DisplayName')

			EXEC sp_xml_removedocument @docHandle

			FETCH NEXT FROM eventtype_cursor
			INTO @Definition
		END 

		SELECT [EventDisplayName] FROM @EventTypeNames
	END TRY
	BEGIN CATCH
		CLOSE eventtype_cursor
		DEALLOCATE eventtype_cursor
		;THROW
	END CATCH

	CLOSE eventtype_cursor
	DEALLOCATE eventtype_cursor
END
GO