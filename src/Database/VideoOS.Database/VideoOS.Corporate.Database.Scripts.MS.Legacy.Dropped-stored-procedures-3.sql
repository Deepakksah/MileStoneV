-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_recording_server_dataset]') AND type in (N'P', N'PC'))
DROP PROCEDURE [delete_recording_server_dataset]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_hardware_dataset]') AND type in (N'P', N'PC'))
DROP PROCEDURE [create_hardware_dataset]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_hardware_dataset]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_hardware_dataset]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[hardware_FillByHardwareIds]') AND type in (N'P', N'PC'))
DROP PROCEDURE [hardware_FillByHardwareIds]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[device_FillByDeviceIds]') AND type in (N'P', N'PC'))
DROP PROCEDURE [device_FillByDeviceIds]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_recording_server_service_dataset]') AND type in (N'P', N'PC'))
DROP PROCEDURE [delete_recording_server_service_dataset]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_create_user_defined_event]') AND type = 'P')
DROP PROCEDURE [admin_create_user_defined_event]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_delete_user_defined_event]') AND type = 'P')
DROP PROCEDURE [admin_delete_user_defined_event]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_update_user_defined_event]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_update_user_defined_event]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_attach_master_site]') AND type = 'P')
DROP PROCEDURE [admin_attach_master_site]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_attach_slave_site]') AND type = 'P')
DROP PROCEDURE [admin_attach_slave_site]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_create_slave_site_transaction]') AND type = 'P')
DROP PROCEDURE [admin_create_slave_site_transaction]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_delete_site_transaction]') AND type = 'P')
DROP PROCEDURE [admin_delete_site_transaction]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_delete_site]') AND type = 'P')
DROP PROCEDURE [admin_delete_site]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_syncronize_slave_site]') AND type = 'P')
DROP PROCEDURE [admin_syncronize_slave_site]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[insert_hardwaredefinition_dataset]') AND type in (N'P', N'PC'))
DROP PROCEDURE [insert_hardwaredefinition_dataset]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[fill_hosted_hardware_by_ids_dataset]') AND type in (N'P', N'PC'))
DROP PROCEDURE [fill_hosted_hardware_by_ids_dataset]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[fill_hosting_servers_by_ids_dataset]') AND type in (N'P', N'PC'))
DROP PROCEDURE [fill_hosting_servers_by_ids_dataset]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[recorderPluginDevice_FillByRecorderPluginIdDeviceIds]') AND type in (N'P', N'PC'))
DROP PROCEDURE [recorderPluginDevice_FillByRecorderPluginIdDeviceIds]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_recording_storage_dataset]') AND type in (N'P', N'PC'))
DROP PROCEDURE [create_recording_storage_dataset]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_legacy_UserSidHasDevicePermission]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_legacy_UserSidHasDevicePermission]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_legacy_DevicePermissions_all_devicetypes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_legacy_DevicePermissions_all_devicetypes]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_query_access_entries_by_sid]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_query_access_entries_by_sid]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_query_object_class]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_query_object_class]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_delete_objectclass]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_delete_objectclass]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_query_class_actions]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_query_class_actions]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_query_access_time_profile_entries]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_query_access_time_profile_entries]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_query_access_time_profile_entry_is_permitted]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_query_access_time_profile_entry_is_permitted]
GO










