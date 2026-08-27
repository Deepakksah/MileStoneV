-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Script updates the view items in [VideoWallViewItem] that use an Access 
-- Control Add Ins to the new style Access Monitor view item
--  and
-- changes the old style of Access Control overlay buttons to the new style
-- Note: Not all old buttons can be converted, wso we try to convert only 
-- the door command buttons. 
-- This is needed because the decoupling of the Access Control from Smart 
-- Client Solution.

--- Access Monitor View Items
-- Use this template to fill in with data from the old view item

--- Overlay Buttons:
IF OBJECT_ID(N'apply_mask', N'FN') IS NOT NULL
  DROP FUNCTION apply_mask
GO
-- Temporary function so we can update the ac button ids
CREATE FUNCTION apply_mask (
   @guid UNIQUEIDENTIFIER,
   @mask UNIQUEIDENTIFIER
) RETURNS UNIQUEIDENTIFIER
AS BEGIN
  -- variables
  DECLARE @vb1 BINARY(16), @vb2 BINARY(16), @lo BINARY(8), @hi BINARY(8)

  -- split every 8 bytes into a binary(8), which is a bigint, the largest size usable with XOR
  SELECT @vb1 = @guid, @vb2 = @mask

  -- xor the high and low parts separately
  SELECT @hi = CONVERT(BINARY(8), SUBSTRING(@vb1,1,8)) ^ CONVERT(BIGINT, SUBSTRING(@vb2,1,8))
  SELECT @lo = CONVERT(BINARY(8), SUBSTRING(@vb1,9,8)) ^ CONVERT(BIGINT, SUBSTRING(@vb2,9,8))

  -- the final result, concatenating the bytes using char(8) - binary -> uniqueidentifier
  RETURN CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16),CONVERT(CHAR(8),@hi) + CONVERT(CHAR(8),@lo)))
END;
GO

With Fixed as 
   (SELECT tmp.pkid as pkid, 
           STUFF(tmp.overlayxml, CHARINDEX('encoding="utf-8"', tmp.overlayxml, 0), 16, 'encoding="utf-16"') as FixedXML
    FROM (select pkid as pkid,
		         [definition_xml].value('(/viewitem/properties/property[@name="overlaybuttons"]/@value)[1]', 'NVARCHAR(MAX)') as overlayxml
		   from [VideoWallViewItem]
           WHERE [definition_xml].exist('/viewitem/properties/property[@name="overlaybuttonsaccesscontrol"]') = 1
	       AND [definition_xml].value('(/viewitem/properties/property[@name="overlaybuttonsaccesscontrol"]/@value)[1]', 'NVARCHAR(MAX)') <> ''
	       AND [definition_xml].exist('/viewitem/properties/property[@name="overlaybuttons"]') = 1) as tmp
	where  LEFT(tmp.overlayxml, 5)='<?xml' AND CHARINDEX('encoding="utf-8"', tmp.overlayxml, 0)>0 ) 
update [VideoWallViewItem]
set definition_xml.modify('replace value of (/viewitem/properties/property[@name="overlaybuttons"]/@value)[1] 
                            with (sql:column("Fixed.FixedXML"))')
from [VideoWallViewItem] inner join Fixed 
  on [VideoWallViewItem].pkid=Fixed.pkid;

-- Update Access control overlay buttons xml
With FixedAC as 
   (SELECT tmp.pkid as pkid, 
           STUFF(tmp.overlayxml, CHARINDEX('encoding="utf-8"',tmp.overlayxml), 16, 'encoding="utf-16"') as FixedXML
    FROM (select pkid as pkid,
		         [definition_xml].value('(/viewitem/properties/property[@name="overlaybuttonsaccesscontrol"]/@value)[1]', 'NVARCHAR(MAX)') as overlayxml
		   from [VideoWallViewItem]
           WHERE [definition_xml].exist('/viewitem/properties/property[@name="overlaybuttonsaccesscontrol"]') = 1
	       AND [definition_xml].value('(/viewitem/properties/property[@name="overlaybuttonsaccesscontrol"]/@value)[1]', 'NVARCHAR(MAX)') <> '') as tmp
	where LEFT(tmp.overlayxml, 5)='<?xml' AND CHARINDEX('encoding="utf-8"', tmp.overlayxml, 0)>0)
update [VideoWallViewItem]
set definition_xml.modify('replace value of (/viewitem/properties/property[@name="overlaybuttonsaccesscontrol"]/@value)[1] 
                            with (sql:column("FixedAC.FixedXML"))')
from [VideoWallViewItem] inner join FixedAC
  on [VideoWallViewItem].pkid=FixedAC.pkid;

-- Cursor iteration variables
DECLARE @view_item_xml XML
DECLARE @view_item_id [GuidType]
DECLARE @ac_overlay_xml XML
DECLARE @regular_overlay_xml XML
-- Mask to use for updating the ids. Must be the same as in the code!
DECLARE @mask [GuidType] SET @mask='19B566EC-C8BB-42FB-B879-422B84DB77DD'

--Table to save the processed view items
DECLARE @fixed_overlay_buttons TABLE (view_item_id [GuidType], view_xml XML)

DECLARE buttons_cursor CURSOR FOR
  SELECT [pkid], [definition_xml]
    FROM [VideoWallViewItem]
    WHERE [definition_xml].exist('/viewitem/properties/property[@name="overlaybuttonsaccesscontrol"]') = 1
	AND [definition_xml].value('(/viewitem/properties/property[@name="overlaybuttonsaccesscontrol"]/@value)[1]', 'NVARCHAR(MAX)') <> ''

OPEN buttons_cursor
BEGIN TRY
  FETCH NEXT FROM buttons_cursor INTO @view_item_id, @view_item_xml
  WHILE (@@FETCH_STATUS = 0)
  BEGIN
        -- Get both types of overlay buttons. We need the access control overlay buttons, which we are going to modify, as well as the
        -- regular overlay buttons, into which we will insert the modified values. Both are stored as XML strings into the value attribute
        -- of the respective property nodes.
	IF
		TRY_CONVERT(XML, @view_item_xml.value('(/viewitem/properties/property[@name="overlaybuttonsaccesscontrol"]/@value)[1]', 'NVARCHAR(max)')) IS NULL
			OR
		TRY_CONVERT(XML, @view_item_xml.value('(/viewitem/properties/property[@name="overlaybuttons"]/@value)[1]', 'NVARCHAR(max)')) IS NULL
	BEGIN
	  PRINT CONCAT('Warning: Could not migrate AC overlay buttons in VideoWallViewItem item with pkid: ' , @view_item_id )
	  FETCH NEXT FROM buttons_cursor INTO @view_item_id, @view_item_xml
	  CONTINUE
	END

    SET @ac_overlay_xml = CONVERT(XML, @view_item_xml.value('(/viewitem/properties/property[@name="overlaybuttonsaccesscontrol"]/@value)[1]', 'NVARCHAR(max)'))
  	SET @regular_overlay_xml = CONVERT(XML, @view_item_xml.value('(/viewitem/properties/property[@name="overlaybuttons"]/@value)[1]', 'NVARCHAR(max)'))
  	-- We will use this TABLE, so we are able to both do calculations on the old Access Control ids and construct new XML nodes for the buttons.
  	DECLARE @tempTable TABLE (button_xml XML, new_action NVARCHAR(256))

      INSERT INTO @tempTable 
  	  SELECT
  	      -- Create a new overlaybutton node, without the ac specific attributes. This is easier than having to do modifies later.
  	      button.query('<overlaybutton x="{ ./@x}" y="{ ./@y}" width="{ ./@width}" height="{ ./@height}" text="{ ./@text}" action="" />'),
  		  -- Create the new value for the "action" attribute. First two guids are static, refering to the different door group that the buttons are in.
  		  -- Construct the rest of the action by applying the guid mask to the old ids. Convert to lowercase, so it matches the actions defined.
            LOWER(CONCAT('4885688d-6b2d-43e1-ba67-bd4aada77dfb.44e2f890-82c0-467a-8481-473e2b33343f.',
               dbo.apply_mask(button.value('./@acinstanceid', '[GuidType]'), @mask), '.',
               dbo.apply_mask(button.value('./@accommandid', '[GuidType]'), @mask)))
        -- The nodes element constructs a temporary TABLE        |
  	-- with a single XML column, based on the XPath. It can  |
  	-- be used only for XML functions. In our case, we want  |
  	-- to split the XML into single overlaybutton nodes to   | The 'AS' clause gives names to the temporary
  	-- process.                                              | TABLE and the single column.
  	--                       v                                            v
        FROM @ac_overlay_xml.nodes('(/overlaybuttons/overlaybutton)') AS buttons(button)
  	
  	-- Insert the new value for the action attribute. It's hard to do this in one go, that's why we used the temporary TABLE to do it.
  	update @tempTable
  	SET button_xml.modify('replace value of (/overlaybutton/@action)[1] with sql:column("new_action")')

  	-- Compress the modified button nodes into a single XML
  	DECLARE @new_buttons XML
  	--                                                        |  The "FOR XML path" allows the select query to 
  	--                            This makes it so the XML    |  construct an XML based on the rows data.         
  	--                            data is inserted as is.     |  The ('') part removes the wrapping tag for the rows.
  	--                                        v                       v
  	SELECT @new_buttons = (SELECT button_xml "*" FROM @tempTable FOR XML PATH(''))

  	-- Insert the new nodes into the original overlay buttons XML
  	SET @regular_overlay_xml.modify('insert sql:variable("@new_buttons") as last into (/overlaybuttons)[1]')

  	-- Have to convert to string before we can replace the old value with the fixed one
  	DECLARE @overlay_string AS NVARCHAR(max)
  	SET @overlay_string = Convert(NVARCHAR(max), @regular_overlay_xml, 1)

  	-- Replace original value with the fixed one and remove the AC buttons overlays property
  	SET @view_item_xml.modify('replace value of (/viewitem/properties/property[@name="overlaybuttons"]/@value)[1] with sql:variable("@overlay_string")')
  	SET @view_item_xml.modify('delete /viewitem/properties/property[@name="overlaybuttonsaccesscontrol"]')

  	-- Save the fixed view item XML along with the id to update the table later.
  	INSERT INTO @fixed_overlay_buttons
  	VALUES (@view_item_id, @view_item_xml)

  	DELETE FROM @tempTable

	FETCH NEXT FROM buttons_cursor INTO @view_item_id, @view_item_xml
  END
END TRY
BEGIN CATCH
  CLOSE buttons_cursor
  DEALLOCATE buttons_cursor
  DROP FUNCTION apply_mask
  ;throw
END CATCH

CLOSE buttons_cursor
DEALLOCATE buttons_cursor

update [VideoWallViewItem] SET definition_xml = view_xml 
FROM @fixed_overlay_buttons join [VideoWallViewItem] ON pkid=view_item_id

DROP FUNCTION apply_mask
GO

-- Remove the utf-8 encoding tag from the ViewItemAddIns XML fragments 
WITH FixedAddIns AS
(
    SELECT pkid,
           STUFF(tmp.addinsXml, CHARINDEX('encoding="utf-8"', tmp.addinsXml), LEN('encoding="utf-8"'), '') as FixedXML
    FROM
    (
        SELECT pkid,
               definition_xml.value('(/viewitem/properties/property[@name="ViewItemAddIns"]/@value)[1]', 'NVARCHAR(MAX)') AS addinsXml
        FROM [VideoWallViewItem]
        WHERE definition_xml.exist('/viewitem/properties/property[@name="ViewItemAddIns"]') = 1
          AND definition_xml.value('(/viewitem/properties/property[@name="ViewItemAddIns"]/@value)[1]', 'NVARCHAR(MAX)') <> ''
    ) AS tmp
    WHERE LEFT(tmp.addinsXml,5) = '<?xml'
      AND CHARINDEX('encoding="utf-8"', tmp.addinsXml, 0) > 0
)
UPDATE [VideoWallViewItem]
SET definition_xml.modify('replace value of (/viewitem/properties/property[@name="ViewItemAddIns"]/@value)[1] 
                            with (sql:column("FixedAddIns.FixedXML"))')
FROM [VideoWallViewItem]
INNER JOIN FixedAddIns
    ON [VideoWallViewItem].pkid = FixedAddIns.pkid;


DECLARE @cur_xml XML, 
        @id [GuidType], 
		@scId [GuidType],
		@addIns xml

DECLARE @temp_table TABLE(id [GuidType], def_xml XML)

-- Select all view items with ViewItemAddIns
DECLARE xml_cursor CURSOR FOR
  SELECT [pkid], [definition_xml],
         [definition_xml].value('(/viewitem/@smartClientId)[1]', '[GuidType]')
  FROM [VideoWallViewItem]
    WHERE
    [definition_xml].exist('/viewitem/properties/property[@name="ViewItemAddIns"]') = 1

OPEN xml_cursor
BEGIN TRY
  FETCH NEXT FROM xml_cursor INTO @id, @cur_xml, @scId
  WHILE(@@FETCH_STATUS = 0)
  BEGIN
	-- Try to convert the ViewItemAddIns value to XML, if unsuccessful skip this view item 
  	IF TRY_CONVERT(XML, @cur_xml.value('(/viewitem/properties/property[@name="ViewItemAddIns"]/@value)[1]', 'NVARCHAR(max)')) IS NULL
	BEGIN
          PRINT CONCAT('Warning: Could not migrate AC overlay buttons from ViewItemAddIns in VideoWallViewItem item with pkid: ' , @id)
          FETCH NEXT FROM xml_cursor INTO @id, @cur_xml, @scId
          CONTINUE
	END

        SET @addIns = CONVERT(XML, @cur_xml.value('(/viewitem/properties/property[@name="ViewItemAddIns"]/@value)[1]', 'NVARCHAR(max)'))

	-- The add in information is stored as xml string in the value attribute of the view item property with name "ViewItemAddIns".
	-- Check if an Access Control add-in exists in that xml.
	IF @addIns.exist('/addins/addin[@addinid="673f95ca-3701-4068-8bb1-fe95dcf87f2d"]') = 1
	BEGIN
  		DECLARE @ac_config XML
  		DECLARE @ac_config_string NVARCHAR(max)
  		DECLARE @camera_config_no_addin NVARCHAR(max)
  		DECLARE @inner_view_id [GuidType] SET @inner_view_id = NEWID()

  		DECLARE @template XML;
		-- The id in the item info is the same id as the view item gets in the Access Control Plugin
		SET @template='<viewitem id="" displayname="Access monitor" shortcut="" type="VideoOS.RemoteClient.Application.MIP.PlatformViewItem, VideoOS.RemoteClient.Application" smartClientId="">
			<iteminfo id="8bdc83cb-9575-4b33-a77e-bf7a1f07db19" displayname="Access monitor" type="VideoOS.RemoteClient.Application.MIP.PlatformViewItem, VideoOS.RemoteClient.Application">
			<viewitem id="00000000-0000-0000-0000-000000000000" type="VideoOS.AccessControl.ClientPlugin.MipPlugins.AccessMonitorViewItem.AccessMonitorViewItemPluginManager, VideoOS.AccessControl.ClientPlugin">
				<properties>
				<property name="Configuration" value="" />
				</properties>
			</viewitem>
			</iteminfo>
			<properties />
		</viewitem>'

		-- The old configuration is stored first as an xml in the ViewItemAddIns property, and then the addin node has an attribute called
		-- configuration. That's why we need to do two consecutive converts from string to xml type.
  		DECLARE @ac_config_string_bad NVARCHAR(MAX) SET @ac_config_string_bad = @addIns.value('(/addins/addin[@addinid="673f95ca-3701-4068-8bb1-fe95dcf87f2d"]/@configuration)[1]', 'NVARCHAR(max)')
  		SET @ac_config = STUFF(@ac_config_string_bad, CHARINDEX('encoding="utf-8"', @ac_config_string_bad, 0), 16, 'encoding="utf-16"')

		-- We need the entire camera view item, just without the old Access control Add-In
  		SET @cur_xml.modify('delete  /viewitem/properties/property[@name="ViewItemAddIns"]')
  		SET @camera_config_no_addin = CONVERT(NVARCHAR(max), @cur_xml)

  		-- The configuration of the new view item is different than the old one, in that the camera id is stored as attribute on the configuration
  		-- Node, so we insert it in the old configuration.
		SET @ac_config.modify('insert ( attribute Camera {(sql:variable("@camera_config_no_addin"))}) into (/configuration)[1]')
  		SET @ac_config_string = CONVERT(NVARCHAR(max), @ac_config)

		-- Fill the taken properties in the template and save for updating the table later.
  		SET @template.modify('replace value of (/viewitem/@id)[1] with (sql:variable("@id"))')
  		SET @template.modify('replace value of (/viewitem/@smartClientId)[1] with (sql:variable("@scId"))')
  		SET @template.modify('replace value of (/viewitem/iteminfo/viewitem/@id)[1] with (sql:variable("@inner_view_id"))')
  		SET @template.modify('replace value of (/viewitem/iteminfo/viewitem/properties/property[@name="Configuration"]/@value)[1] with (sql:variable("@ac_config_string"))')
  		INSERT INTO @temp_table VALUES (@id, @template) 
	END

	FETCH NEXT FROM xml_cursor INTO @id, @cur_xml, @scId
  END
END TRY
BEGIN CATCH
  CLOSE xml_cursor
  DEALLOCATE xml_cursor
  ;THROW
END CATCH

CLOSE xml_cursor
DEALLOCATE xml_cursor

UPDATE [VideoWallViewItem] SET definition_xml = def_xml 
FROM @temp_table JOIN [VideoWallViewItem] ON pkid=id

GO
