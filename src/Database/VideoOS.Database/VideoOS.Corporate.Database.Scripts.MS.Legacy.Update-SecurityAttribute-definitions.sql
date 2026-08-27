-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @definition NVARCHAR(MAX)


SET @definition = '<attributedefinition>
  <type>enumeration</type>
  <enumeration>
    <value>
      <name>Windows</name>
      <displayname referenceid="2042AD26-21B4-42FE-8461-B8D539344628">Windows Authentication</displayname>
    </value>
    <value>
      <name>Basic</name>
      <displayname referenceid="FD0497C4-9536-4514-9379-194D9B5BE691">Basic Authentication</displayname>
    </value>
    <defaultvalue>Windows</defaultvalue>
    <administratorvalue />
  </enumeration>
</attributedefinition>'
UPDATE [SecurityAttributes] SET [Definition] = @definition WHERE [SecurityAttributeID] = 'ENTERPRISESERVERAUTHENTICATION'


SET @definition = '<attributedefinition>
  <type>passwordstring</type>
  <data>
    <datatype>string</datatype>
    <value>
      <name>Password</name>
      <displayname referenceid="47680598-492E-40C3-AD41-70EFEBD335D5">Password</displayname>
    </value>
    <defaultvalue />
    <administratorvalue />
  </data>
</attributedefinition>'
UPDATE [SecurityAttributes] SET [Definition] = @definition WHERE [SecurityAttributeID] = 'ENTERPRISESERVERPASSWORD'


SET @definition = '<attributedefinition>
  <type>string</type>
  <data>
    <datatype>string</datatype>
    <value>
      <name>Username</name>
      <displayname referenceid="2D71AA88-2B29-4D2C-B1B8-BF517C45C177">User name</displayname>
    </value>
    <defaultvalue />
    <administratorvalue />
  </data>
</attributedefinition>'
UPDATE [SecurityAttributes] SET [Definition] = @definition WHERE [SecurityAttributeID] = 'ENTERPRISESERVERUSERNAME'


SET @definition = '<attributedefinition>
  <type>adobjecteditor</type>
  <data>
    <datatype>string</datatype>
    <value>
      <name>Windows Account Name</name>
      <displayname referenceid="57623BAA-EF91-43B6-A6B3-FA3CF2516E5A">Account name</displayname>
    </value>
    <defaultvalue />
    <administratorvalue />
  </data>
</attributedefinition>'
UPDATE [SecurityAttributes] SET [Definition] = @definition WHERE [SecurityAttributeID] = 'ENTERPRISESERVERWINDOWSACCOUNTNAME'


SET @definition = '<attributedefinition>
  <type>passwordstring</type>
  <data>
    <datatype>string</datatype>
    <value>
      <name>Password</name>
      <displayname referenceid="61BF32F0-E17B-4C9C-BF03-50430920E406">Password</displayname>
    </value>
    <defaultvalue />
    <administratorvalue />
  </data>
</attributedefinition>'
UPDATE [SecurityAttributes] SET [Definition] = @definition WHERE [SecurityAttributeID] = 'ENTERPRISESERVERWINDOWSACCOUNTPASSWORD'

GO