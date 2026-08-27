-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Script updates the view items in [VideoWallViewItem] that use an LPR 
-- Add Ins to the new style LPR view item

-- This is needed because the decoupling of LPR plugin from Smart 
-- Client Solution. The XMLs use utf-8 encoding, by removing
-- the encoding attribute the XMLs will be in utf-16 encoding.

-- LPR View Items
-- Use this template to fill in with data from the old view item


-- Update the view item add ins to remove utf-8 enconding
DECLARE @encodingUtf8 NVARCHAR(MAX) = 'encoding="utf-8"';
DECLARE @encodingUtf8B NVARCHAR(MAX) = 'encoding=&quot;utf-8&quot;';
DECLARE @LprViewItemGuid NVARCHAR(50) = 'dd66185f-1150-4988-9bbe-c45c321fabef';

WITH FixedConfigurationXml as 
   (SELECT tmpOuterXML.pkid as pkid,
           CASE WHEN CHARINDEX(@encodingUtf8, tmpOuterXML.fixedInnerXml, 0) > 0 
			THEN STUFF(tmpOuterXML.fixedInnerXml, CHARINDEX(@encodingUtf8, tmpOuterXML.fixedInnerXml, 0), LEN(@encodingUtf8), '')
            ELSE tmpOuterXML.fixedInnerXml --We must always return an element. If we cannot substitute (maybe missing encoding?) then return the Inner XML
			END as fixedOuterXml
    FROM (SELECT tmpInnerXML.pkid as pkid, 
                 STUFF(tmpInnerXML.viewItemsAddInsXml, CHARINDEX(@encodingUtf8B, tmpInnerXML.viewItemsAddInsXml, 0), LEN(@encodingUtf8B), '') as fixedInnerXml
          FROM (SELECT pkid as pkid,
                       [definition_xml].value('(/viewitem/properties/property[@name="ViewItemAddIns"]/@value)[1]', 'NVARCHAR(MAX)') as viewItemsAddInsXml
                FROM [VideoWallViewItem]
                WHERE [definition_xml].value('(/viewitem/properties/property[@name="ViewItemAddIns"]/@value)[1]', 'NVARCHAR(MAX)') <> ''
                  AND [definition_xml].exist('/viewitem/properties/property[@name="ViewItemAddIns"]') = 1
               ) as tmpInnerXML
          ) as tmpOuterXML
   ) 
UPDATE [VideoWallViewItem]
SET definition_xml.modify('replace value of (/viewitem/properties/property[@name="ViewItemAddIns"]/@value)[1] 
                           with (sql:column("FixedConfigurationXml.fixedOuterXml"))')
FROM [VideoWallViewItem] inner join FixedConfigurationXml
 ON [VideoWallViewItem].pkid=FixedConfigurationXml.pkid
 WHERE fixedOuterXml <> 'NULL';

-- Update the overlay buttons to remove utf-8 enconding
WITH FixedOverlayButtonsXml as 
   (SELECT tmpOverlayButtons.pkid as pkid, 
           STUFF(tmpOverlayButtons.overlayButtonsXml, CHARINDEX(@encodingUtf8, tmpOverlayButtons.overlayButtonsXml, 0), LEN(@encodingUtf8), '') as fixedOverlayXml
    FROM (SELECT pkid as pkid,
                 [definition_xml].value('(/viewitem/properties/property[@name="overlaybuttons"]/@value)[1]', 'NVARCHAR(MAX)') as overlayButtonsXml
           FROM [VideoWallViewItem]
           WHERE [definition_xml].exist('/viewitem/properties/property[@name="overlaybuttons"]') = 1) as tmpOverlayButtons
    WHERE  LEFT(tmpOverlayButtons.overlayButtonsXml, 5)='<?xml' AND CHARINDEX(@encodingUtf8, tmpOverlayButtons.overlayButtonsXml, 0)>0 ) 
UPDATE [VideoWallViewItem]
SET definition_xml.modify('replace value of (/viewitem/properties/property[@name="overlaybuttons"]/@value)[1] 
                            with (sql:column("FixedOverlayButtonsXml.fixedOverlayXml"))')
FROM [VideoWallViewItem] inner join FixedOverlayButtonsXml 
  ON [VideoWallViewItem].pkid=FixedOverlayButtonsXml.pkid
  WHERE fixedOverlayXml <> 'NULL'

DECLARE @cur_xml XML, 
        @id [GuidType], 
        @scId [GuidType]

DECLARE @temp_table TABLE(id [GuidType], def_xml XML)

-- Select all view items, that define an add in with the id of LPR Add In
DECLARE xml_cursor CURSOR FOR
  SELECT [pkid], 
         [definition_xml],
         [definition_xml].value('(/viewitem/@smartClientId)[1]', '[GuidType]')
  FROM [VideoWallViewItem]
    WHERE [definition_xml].exist('/viewitem/properties/property[@name="ViewItemAddIns"]') = 1 

OPEN xml_cursor
BEGIN TRY
  FETCH NEXT FROM xml_cursor INTO @id, @cur_xml, @scId
  WHILE(@@FETCH_STATUS = 0)
  BEGIN
      DECLARE @lpr_config XML
      DECLARE @lpr_config_string NVARCHAR(max)
      DECLARE @inner_view_id [GuidType] SET @inner_view_id = NEWID()
      DECLARE @viewitem NVARCHAR(MAX);
      DECLARE @livestreamidAttribute NVARCHAR(MAX);
      DECLARE @addInsXML XML;

      DECLARE @template XML;
      -- The id in the item info is the same id as the view item gets in the LPR Plugin
      SET @template='
        <viewitem id="" displayname="LPR" shortcut="" type="VideoOS.RemoteClient.Application.MIP.PlatformViewItem, VideoOS.RemoteClient.Application" smartClientId="">
          <iteminfo id="3561ba4f-ae52-4de6-801d-d0134306ae03" displayname="LPR" type="VideoOS.RemoteClient.Application.MIP.PlatformViewItem, VideoOS.RemoteClient.Application">
            <viewitem id="00000000-0000-0000-0000-000000000000" type="VideoOS.LPR.Plugin.Client.LPRViewItem.LPRViewItemPluginManager, VideoOS.LPR.Plugin">
              <properties>
                <property name="Configuration" value="" />
              </properties>
            </viewitem>
          </iteminfo>
          <properties />
        </viewitem>'
      
      -- We try to see if the ViewItem XML can be converted first, if so we do and continue with the configuration XML and check. 
      -- In case we do not then continue with the next view item.
      IF 
        TRY_CONVERT(XML, @cur_xml.value('(/viewitem/properties/property[@name="ViewItemAddIns"]/@value)[1]', 'NVARCHAR(max)')) IS NULL
      BEGIN
        PRINT CONCAT('Warning: Could not migrate LPR ViewItemAddIns in VideoWallViewItem item with pkid: ' , @id)
        FETCH NEXT FROM xml_cursor INTO @id, @cur_xml, @scId
        CONTINUE
      END

      SET @AddInsXML = CONVERT(XML, @cur_xml.value('(/viewitem/properties/property[@name="ViewItemAddIns"]/@value)[1]', 'NVARCHAR(max)'));

      IF @AddInsXML.exist('/addins/addin[@addinid=sql:variable("@LprViewItemGuid")]') = 1
      BEGIN
          -- Try to convert the ViewItemAddIns value to XML, if unsuccessful skip this view item 
          IF 
            TRY_CONVERT(XML, @AddInsXML.value('(/addins/addin[@addinid=sql:variable("@LprViewItemGuid")]/@configuration)[1]', 'NVARCHAR(max)')) IS NULL
          BEGIN
            PRINT CONCAT('Warning: Could not migrate LPR ViewItemAddIns Configuration in VideoWallViewItem item with pkid: ' , @id)
            FETCH NEXT FROM xml_cursor INTO @id, @cur_xml, @scId
            CONTINUE
          END

          -- The old configuration is stored first as an xml in the ViewItemAddIns property, and then the addin node has an attribute called
          -- configuration. That's why we need to do two consecutive converts from string to xml type.
          SET @lpr_config = CONVERT(XML, @AddInsXML.value('(/addins/addin[@addinid=sql:variable("@LprViewItemGuid")]/@configuration)[1]', 'NVARCHAR(max)'))

          -- Add the camera attribute to the configuration XML. As we do not know if there viewitem has changed configuration (compared to default)
          -- we must copy the whole viewitem into the camera attribute:
          -- There are a few things that must be modified: 
          -- * Remove ViewItemAddIns
          -- * Remove smartClientId
          -- * Add livestreamid property
          SET @cur_xml.modify('delete  /viewitem/properties/property[@name="ViewItemAddIns"]')
          SET @cur_xml.modify('delete  /viewitem/@smartClientId')
          SET @livestreamidAttribute = @cur_xml.value('(/viewitem/iteminfo/@livestreamid)[1]', 'NVARCHAR(MAX)');
          SET @cur_xml.modify('insert <property name="livestreamid" value="{sql:variable("@livestreamidAttribute")}"/> into (/viewitem/properties/property)[1]');

          SET @viewitem = CONVERT(NVARCHAR(max), @cur_xml)
          SET @lpr_config.modify('insert attribute camera {sql:variable("@viewitem")} into (/configuration)[1]');

          SET @lpr_config_string = CONVERT(NVARCHAR(max), @lpr_config)

          -- Fill the taken properties in the template and save for updating the table later.
          SET @template.modify('replace value of (/viewitem/@id)[1] with (sql:variable("@id"))')
          SET @template.modify('replace value of (/viewitem/@smartClientId)[1] with (sql:variable("@scId"))')
          SET @template.modify('replace value of (/viewitem/iteminfo/viewitem/@id)[1] with (sql:variable("@inner_view_id"))')
          SET @template.modify('replace value of (/viewitem/iteminfo/viewitem/properties/property[@name="Configuration"]/@value)[1] with (sql:variable("@lpr_config_string"))')
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