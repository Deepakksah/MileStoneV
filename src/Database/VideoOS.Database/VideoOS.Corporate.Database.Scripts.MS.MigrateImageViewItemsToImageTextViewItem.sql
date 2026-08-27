-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Script migrates ImageViewItems to ImageTextViewItems in [VideoWallViewItem] 
-- This is needed because the existing image and text view item types have been combined into one view item and extracted into a Smart Client MIP plugin.

DECLARE @id [GuidType]
DECLARE @cur_xml XML
        
DECLARE @temp_table TABLE(id [GuidType], def_xml XML)

-- Select all view items of the type ImageViewItem and put into imageViewItemCursor
DECLARE imageViewItemCursor CURSOR FOR
  SELECT [pkid], 
         [definition_xml]
  FROM [VideoWallViewItem]
    WHERE
    [definition_xml].exist('/viewitem[@type="VideoOS.RemoteClient.Application.Data.ContentTypes.ImageContentType.ImageViewItem, VideoOS.RemoteClient.Application"]') = 1 

-- Loop over image view items and convert
OPEN imageViewItemCursor
BEGIN TRY
  FETCH NEXT FROM imageViewItemCursor INTO @id, @cur_xml
  WHILE(@@FETCH_STATUS = 0)
  BEGIN
    BEGIN TRY
 	  DECLARE @idAttribute NVARCHAR(MAX);
  	  DECLARE @shortcutAttribute NVARCHAR(MAX);
  	  DECLARE @smartClientIdAttribute NVARCHAR(MAX);
  	  DECLARE @imageFilenameAttribute NVARCHAR(MAX);
  	  DECLARE @maintainImageAspectRatioAttribute NVARCHAR(MAX);
  	  DECLARE @embedImageAttribute NVARCHAR(MAX);
  	  DECLARE @embeddedImageAttribute NVARCHAR(MAX);

      DECLARE @template XML;
      
      SET @template='
<viewitem id="" displayname="Image and Text" shortcut="" type="VideoOS.RemoteClient.Application.MIP.PlatformViewItem, VideoOS.RemoteClient.Application" smartClientId="">
  <iteminfo id="8de0025a-d92e-4c18-b670-9ae0625f8be1" displayname="Image and Text" type="VideoOS.RemoteClient.Application.MIP.PlatformViewItem, VideoOS.RemoteClient.Application">
    <viewitem id="00000000-0000-0000-0000-000000000000" type="ImageTextViewItem.Client.ImageTextViewItemViewItemManager, VideoOS.ImageTextViewItem.Plugin">
      <properties>
        <property name="ImageFileName" value="" />
        <property name="MaintainImageAspectRatio" value="" />
        <property name="EmbedImage" value="" />
        <property name="EmbeddedImage" value="" />
        <property name="Initialized" value="True" />
      </properties>
    </viewitem>
  </iteminfo>
  <properties />
</viewitem>'

      -- Extract properties from current xml
      SET @idAttribute = @cur_xml.value('(/viewitem/@id)[1]', 'NVARCHAR(MAX)');
      SET @shortcutAttribute = @cur_xml.value('(/viewitem/@shortcut)[1]', 'NVARCHAR(MAX)');
      SET @smartClientIdAttribute = @cur_xml.value('(/viewitem/@smartClientId)[1]', 'NVARCHAR(MAX)');
      SET @imageFilenameAttribute = @cur_xml.value('(/viewitem/properties/property[@name="imagefilename"]/@value)[1]', 'NVARCHAR(MAX)');
      SET @maintainImageAspectRatioAttribute = @cur_xml.value('(/viewitem/properties/property[@name="maintainimageaspectratio"]/@value)[1]', 'NVARCHAR(MAX)');
      SET @embedImageAttribute = @cur_xml.value('(/viewitem/properties/property[@name="EmbedImage"]/@value)[1]', 'NVARCHAR(MAX)');
      SET @embeddedImageAttribute = @cur_xml.value('(/viewitem/properties/property[@name="EmbeddedImage"]/@value)[1]', 'NVARCHAR(MAX)');

      -- Ensure that properties are not null. Will cause the modify below to fail
      IF @shortcutAttribute IS NULL SET @shortcutAttribute = N'';
      IF @smartClientIdAttribute IS NULL SET @smartClientIdAttribute = N'';
      IF @imageFilenameAttribute IS NULL SET @imageFilenameAttribute = N'';
      IF @maintainImageAspectRatioAttribute IS NULL SET @maintainImageAspectRatioAttribute = N'';
      IF @embedImageAttribute IS NULL SET @embedImageAttribute = N'';
      IF @embeddedImageAttribute IS NULL SET @embeddedImageAttribute = N'';

      -- Fill the taken properties in the template and save for updating the table later.
      SET @template.modify('replace value of (/viewitem/@id)[1] with (sql:variable("@idAttribute"))')
      SET @template.modify('replace value of (/viewitem/@shortcut)[1] with (sql:variable("@shortcutAttribute"))')
      SET @template.modify('replace value of (/viewitem/@smartClientId)[1] with (sql:variable("@smartClientIdAttribute"))')
      SET @template.modify('replace value of (/viewitem/iteminfo/viewitem/properties/property[@name="ImageFileName"]/@value)[1] with (sql:variable("@imageFilenameAttribute"))')
      SET @template.modify('replace value of (/viewitem/iteminfo/viewitem/properties/property[@name="MaintainImageAspectRatio"]/@value)[1] with (sql:variable("@maintainImageAspectRatioAttribute"))')
      SET @template.modify('replace value of (/viewitem/iteminfo/viewitem/properties/property[@name="EmbedImage"]/@value)[1] with (sql:variable("@embedImageAttribute"))')
      SET @template.modify('replace value of (/viewitem/iteminfo/viewitem/properties/property[@name="EmbeddedImage"]/@value)[1] with (sql:variable("@embeddedImageAttribute"))')
      INSERT INTO @temp_table VALUES (@id, @template)
    END TRY
    BEGIN CATCH
      PRINT CONCAT('Warning: Could not migrate ImageViewItem item with pkid: ' , @id)
    END CATCH
    
	-- Select next result in the cursor
    FETCH NEXT FROM imageViewItemCursor INTO @id, @cur_xml
  END
END TRY
BEGIN CATCH
  PRINT 'Warning: Unexpected error in update script.';
END CATCH

CLOSE imageViewItemCursor
DEALLOCATE imageViewItemCursor

UPDATE [VideoWallViewItem] SET definition_xml = def_xml 
FROM @temp_table JOIN [VideoWallViewItem] ON pkid=id

GO
