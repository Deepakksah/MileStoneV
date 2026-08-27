-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @namespace_id UNIQUEIDENTIFIER = '55BF1320-0224-48C7-8F57-BD41090576C8'
DECLARE @security_object_id UNIQUEIDENTIFIER = '48214F5E-53B5-4D61-8150-F4AE081F44BE'
DECLARE @separator CHAR(1) = '/'

-- Attempt to find the order_no of the Alarm Definition security namespace
DECLARE @alarm_order_no INT = 21 -- Use a default namespace order_no if the Alarm Definition namespace is not found
IF EXISTS (SELECT [order_no] FROM [SecurityNamespace] WHERE [namespace_id] = '46BB0392-FD06-465B-8776-B90AC16639B3')
BEGIN
  SELECT @alarm_order_no = [order_no] FROM [SecurityNamespace] WHERE [namespace_id] = '46BB0392-FD06-465B-8776-B90AC16639B3'
END

-- MatadataSarch Namespace
IF NOT EXISTS(SELECT * FROM [SecurityNamespace] WHERE [namespace_id] = @namespace_id)
BEGIN
  UPDATE [SecurityNamespace]
  SET [order_no] = [order_no] + 1
  WHERE [order_no] >= @alarm_order_no + 1

  INSERT [SecurityNamespace] ([namespace_id], [name], [display_name], [separator], [structure], [order_no], [user_defined])
  SELECT @namespace_id, 'MetadataSearch', 'Metadata Search', @separator, 0, @alarm_order_no + 1, 0
END

-- Metadata search Actions
IF NOT EXISTS(SELECT * FROM [SecurityAction] WHERE [namespace_id] = @namespace_id)
INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible], [security_bit])
SELECT @namespace_id, 'GENERIC_READ', 'Read the metadata search configuration', 0, 0, 1 UNION ALL
SELECT @namespace_id, 'EDIT_METADATA_SEARCHCONF', 'Edit the metadata search configuration', 1, 0, 2 UNION ALL
SELECT @namespace_id, 'ADMIN_SECURITY', 'Administrate security', 2, 0, 4

-- Metadata search Security Object
IF NOT EXISTS(SELECT * FROM [SecurityObject] WHERE [namespace_id] = @namespace_id AND [security_object_id] = @security_object_id AND [object_id] = @separator)
INSERT [SecurityObject] ([namespace_id], [parent_path], [object_id], [object_type], [object_name], [security_object_id], [inherits_from_id])
SELECT @namespace_id, '', @separator, 1, 'MetadataSearch', @security_object_id, NULL

GO