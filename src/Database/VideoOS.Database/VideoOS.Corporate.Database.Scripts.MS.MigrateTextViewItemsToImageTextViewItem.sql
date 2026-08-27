-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Script migrates TextViewItems to ImageTextViewItems in [VideoWallViewItem] 
-- This is needed because the existing image and text view item types have been combined into one view item and extracted into a Smart Client MIP plugin.

DECLARE @id [GuidType]
DECLARE @cur_xml XML
        
DECLARE @temp_table TABLE(id [GuidType], def_xml XML)

-- Select all view items of the type TextViewItem and put into textViewItemCursor
DECLARE textViewItemCursor CURSOR FOR
  SELECT [pkid], 
         [definition_xml]
  FROM [VideoWallViewItem]
    WHERE
    [definition_xml].exist('/viewitem[@type="VideoOS.RemoteClient.Plugin.TextViewItem.TextViewItem, VideoOS.RemoteClient.Plugin.TextViewItem"]') = 1 

-- Loop over image view items and convert
OPEN textViewItemCursor
BEGIN TRY
  FETCH NEXT FROM textViewItemCursor INTO @id, @cur_xml
  WHILE(@@FETCH_STATUS = 0)
  BEGIN
    BEGIN TRY
  	  DECLARE @idAttribute NVARCHAR(MAX);
  	  DECLARE @shortcutAttribute NVARCHAR(MAX);
  	  DECLARE @smartClientIdAttribute NVARCHAR(MAX);
  	  DECLARE @documentAttribute NVARCHAR(MAX);
  	  DECLARE @documentBackgroundAttribute NVARCHAR(MAX);
  	  DECLARE @documentWidthAttribute NVARCHAR(MAX);
  	  DECLARE @windowWidthAttribute NVARCHAR(MAX);
  	  DECLARE @windowHeightAttribute NVARCHAR(MAX);

      DECLARE @template XML;
      
      SET @template='
<viewitem id="" displayname="Image and Text" shortcut="" type="VideoOS.RemoteClient.Application.MIP.PlatformViewItem, VideoOS.RemoteClient.Application" smartClientId="">
  <iteminfo id="8de0025a-d92e-4c18-b670-9ae0625f8be1" displayname="Image and Text" type="VideoOS.RemoteClient.Application.MIP.PlatformViewItem, VideoOS.RemoteClient.Application">
    <viewitem id="00000000-0000-0000-0000-000000000000" type="ImageTextViewItem.Client.ImageTextViewItemViewItemManager, VideoOS.ImageTextViewItem.Plugin">
      <properties>
        <property name="DocumentXaml" value="" />
        <property name="DocumentBackground" value="" />
        <property name="DocumentWidth" value="" />
        <property name="WindowHeight" value="" />
        <property name="WindowWidth" value="" />
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
      SET @documentAttribute = @cur_xml.value('(/viewitem/properties/property[@name="Document"]/@value)[1]', 'NVARCHAR(MAX)');
      SET @documentBackgroundAttribute = @cur_xml.value('(/viewitem/properties/property[@name="DocumentBackground"]/@value)[1]', 'NVARCHAR(MAX)');
      SET @documentWidthAttribute = @cur_xml.value('(/viewitem/properties/property[@name="DocumentWidth"]/@value)[1]', 'NVARCHAR(MAX)');
      SET @windowWidthAttribute = @cur_xml.value('(/viewitem/properties/property[@name="WindowWidth"]/@value)[1]', 'NVARCHAR(MAX)');
      SET @windowHeightAttribute = @cur_xml.value('(/viewitem/properties/property[@name="WindowHeight"]/@value)[1]', 'NVARCHAR(MAX)');

      -- Ensure that properties are not null. Will cause the modify below to fail
      IF @shortcutAttribute IS NULL SET @shortcutAttribute = N'';
      IF @smartClientIdAttribute IS NULL SET @smartClientIdAttribute = N'';
      IF @documentAttribute IS NULL SET @documentAttribute = N'';
      IF @documentBackgroundAttribute IS NULL SET @documentBackgroundAttribute = N'';
      IF @documentWidthAttribute IS NULL SET @documentWidthAttribute = N'';
      IF @windowWidthAttribute IS NULL SET @windowWidthAttribute = N'';
      IF @windowHeightAttribute IS NULL SET @windowHeightAttribute = N'';

      -- Fill the taken properties in the template and save for updating the table later.
      SET @template.modify('replace value of (/viewitem/@id)[1] with (sql:variable("@idAttribute"))')
      SET @template.modify('replace value of (/viewitem/@shortcut)[1] with (sql:variable("@shortcutAttribute"))')
      SET @template.modify('replace value of (/viewitem/@smartClientId)[1] with (sql:variable("@smartClientIdAttribute"))')
      SET @template.modify('replace value of (/viewitem/iteminfo/viewitem/properties/property[@name="DocumentXaml"]/@value)[1] with (sql:variable("@documentAttribute"))')
      SET @template.modify('replace value of (/viewitem/iteminfo/viewitem/properties/property[@name="DocumentBackground"]/@value)[1] with (sql:variable("@documentBackgroundAttribute"))')
      SET @template.modify('replace value of (/viewitem/iteminfo/viewitem/properties/property[@name="DocumentWidth"]/@value)[1] with (sql:variable("@documentWidthAttribute"))')
      SET @template.modify('replace value of (/viewitem/iteminfo/viewitem/properties/property[@name="WindowHeight"]/@value)[1] with (sql:variable("@windowWidthAttribute"))')
      SET @template.modify('replace value of (/viewitem/iteminfo/viewitem/properties/property[@name="WindowWidth"]/@value)[1] with (sql:variable("@windowHeightAttribute"))')
      INSERT INTO @temp_table VALUES (@id, @template)
    END TRY
	BEGIN CATCH
      PRINT CONCAT('Warning: Could not migrate TextViewItem item with pkid: ' , @id)
	END CATCH
    -- Select next result in the cursor
    FETCH NEXT FROM textViewItemCursor INTO @id, @cur_xml
  END
END TRY
BEGIN CATCH
  PRINT 'Warning: Unexpected error in update script.';
END CATCH

CLOSE textViewItemCursor
DEALLOCATE textViewItemCursor

UPDATE [VideoWallViewItem] SET definition_xml = def_xml 
FROM @temp_table JOIN [VideoWallViewItem] ON pkid=id

GO
