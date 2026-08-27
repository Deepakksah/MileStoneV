-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_query_access_entries_device_time_profile]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_query_access_entries_device_time_profile]
GO

CREATE PROCEDURE [security_query_access_entries_device_time_profile]
	@role_id GuidType,
	@object_id NVARCHAR(300)
AS
BEGIN
	SET NOCOUNT ON

	SELECT [LastModified] as "last_modified", [SecurityAttributeID] as "action_name",[IDRole] as "role_id", [IDDevice] as "device_id", 
		[Value] as "value", [IDTimeProfile] as "time_profile_id", [TimeBackward] as "time_backward", [IsDefaultTimeProfile] as "is_default"
	FROM [SecurityPermissionsDevice] WHERE [IDRole] = @role_id AND [IDDevice] = @object_id
END
GO