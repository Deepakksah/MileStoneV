---- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @streams_xml AS XML
DECLARE @definition_xml AS XML

DECLARE devices_cursor CURSOR FOR 
SELECT
	d.[Streams], hd.[Definition]
FROM [Devices] d
	INNER JOIN [Hardware] h ON d.[IDHardware] = h.[IDHardware]
	INNER JOIN [HardwareDefinition] hd ON h.[C18NDigest] = hd.[C18NDigest]
WHERE
	d.[Streams] IS NOT NULL
	AND LEN(CAST(d.[Streams] AS NVARCHAR(MAX))) > 0
	AND d.[Streams].exist('/streams/stream/livedefault') = 0
FOR UPDATE OF d.[Streams]

OPEN devices_cursor

FETCH NEXT FROM devices_cursor
INTO @streams_xml, @definition_xml

WHILE @@FETCH_STATUS = 0
BEGIN
	DECLARE @updated_xml AS XML
	
	SELECT @updated_xml = 
			(SELECT
				node.value('data(referenceid)[1]', 'GuidType') AS [referenceid],
				node.value('data(hwid)[1]', 'varchar(100)') AS [hwid],
				COALESCE([DisplayNames].displayname, '?') AS [name],
				'True' AS [livefeed],
				node.value('data(livefeed)[1]', 'varchar(100)') AS [livedefault],
				'True' AS [liverunalways],
				node.value('data(plugins)[1]', 'varchar(100)') AS [plugins],
				node.value('data(record)[1]', 'varchar(100)') AS [record]
			FROM
				@streams_xml.nodes('/streams/stream') streams_xml(node)
			LEFT JOIN (SELECT 
							streams.value('data(capabilities/@hwid)[1]', 'varchar(100)') as [hwid],
							streams.value('data(displayname)[1]', 'varchar(100)') as [displayname]
						FROM
							@definition_xml.nodes('/definition/hardware/devices/device/streams/stream') definition_xml(streams))
							[DisplayNames] ON [DisplayNames].hwid = node.value('data(hwid)[1]', 'varchar(100)')
			FOR XML PATH('stream'), ROOT('streams'))
	
	UPDATE [Devices] SET [Streams] = @updated_xml 
	--OUTPUT CAST(DELETED.[Streams] AS XML) AS [Before], CAST(INSERTED.[Streams] AS XML) AS [After]
	WHERE CURRENT OF devices_cursor

	FETCH NEXT FROM devices_cursor
	INTO @streams_xml, @definition_xml
END 

CLOSE devices_cursor
DEALLOCATE devices_cursor
GO
