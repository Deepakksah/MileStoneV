-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_device_group_member_rows]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_device_group_member_rows]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_device_group_member_row]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_device_group_member_row]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_device_group_by_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_device_group_by_id]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[move_device_group]') AND type in (N'P', N'PC'))
DROP PROCEDURE [move_device_group]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_device_group_member]') AND type in (N'P', N'PC'))
DROP PROCEDURE [delete_device_group_member]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_device_groups_by_type]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_device_groups_by_type]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_device_groups_by_member]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_device_groups_by_member]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_device_groups]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_device_groups]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_device_group_row]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_device_group_row]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_tasks_by_target]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_tasks_by_target]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_task_by_type]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_task_by_type]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_query_recorders_by_name]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_query_recorders_by_name]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_query_recorders]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_query_recorders]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_update_recorder]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_update_recorder]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_hardware]') AND type in (N'P', N'PC'))
DROP PROCEDURE [create_hardware]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_hardware_template]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_hardware_template]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_hardware_settings]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_hardware_settings]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_hardware_templates]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_hardware_templates]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_hardware_template_row]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_hardware_template_row]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_hardware_template]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_hardware_template]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_hardware_row]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_hardware_row]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_hardware_driver_list]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_hardware_driver_list]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_hardware_template]') AND type in (N'P', N'PC'))
DROP PROCEDURE [delete_hardware_template]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_hardware]') AND type in (N'P', N'PC'))
DROP PROCEDURE [delete_hardware]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_hardware_template]') AND type in (N'P', N'PC'))
DROP PROCEDURE [create_hardware_template]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_device_template_rows]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_device_template_rows]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_device_rows]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_device_rows]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_device_row]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_device_row]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_device_link_template_rows]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_device_link_template_rows]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_device_link_rows]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_device_link_rows]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_device_transaction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [delete_device_transaction]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_device_template]') AND type in (N'P', N'PC'))
DROP PROCEDURE [create_device_template]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_device_link]') AND type in (N'P', N'PC'))
DROP PROCEDURE [create_device_link]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_device]') AND type in (N'P', N'PC'))
DROP PROCEDURE [create_device]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_recorder_plugin_device_rows]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_recorder_plugin_device_rows]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_recorder_plugin_device]') AND type in (N'P', N'PC'))
DROP PROCEDURE [create_recorder_plugin_device]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_recorder_plugin_device_template_rows]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_recorder_plugin_device_template_rows]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_recorder_plugin_device_rows_by_hardware]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_recorder_plugin_device_rows_by_hardware]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_archive_storages_row]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_archive_storages_row]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_recording_storages_row]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_recording_storages_row]
GO
