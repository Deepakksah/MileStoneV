using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace VideoOS.Common.Integration;

[GeneratedCode("CreateLogEntryDefinitions build task", "0.0.0.0")]
public static class LogEntryDefinitions
{
	public static IEnumerable<LogEntryDefinition> AllDefinitions
	{
		get
		{
			yield return new LogEntryDefinition(1001, LogLevel.Info, LogType.SystemLog, "Management server started.", 0);
			yield return new LogEntryDefinition(1004, LogLevel.Error, LogType.SystemLog, "Error initializing database connection: {0}", 1);
			yield return new LogEntryDefinition(1005, LogLevel.Error, LogType.SystemLog, "Error loading the system configuration password or the encryption key: {0}", 1);
			yield return new LogEntryDefinition(1006, LogLevel.Error, LogType.SystemLog, "Error initializing management server command processor: {0}", 1);
			yield return new LogEntryDefinition(1008, LogLevel.Error, LogType.SystemLog, "Error initializing recording server communication manager: {0}", 1);
			yield return new LogEntryDefinition(1009, LogLevel.Error, LogType.SystemLog, "Error starting management server rule processor: {0}", 1);
			yield return new LogEntryDefinition(1010, LogLevel.Error, LogType.SystemLog, "Error initializing log manager: {0}", 1);
			yield return new LogEntryDefinition(1011, LogLevel.Error, LogType.SystemLog, "Error initializing VideoOS Management Objects Client Module: {0}", 1);
			yield return new LogEntryDefinition(1012, LogLevel.Error, LogType.SystemLog, "Cannot host the VideoOS Management Objects on the server: {0}", 1);
			yield return new LogEntryDefinition(1013, LogLevel.Error, LogType.SystemLog, "Error initializing task manager: {0}", 1);
			yield return new LogEntryDefinition(1020, LogLevel.Error, LogType.SystemLog, "Management server terminated due to errors: {0}", 1);
			yield return new LogEntryDefinition(1041, LogLevel.Error, LogType.SystemLog, "Error scanning for drivers and plug-ins: {0}", 1);
			yield return new LogEntryDefinition(1042, LogLevel.Error, LogType.SystemLog, "Error initializing recording server command processor: {0}", 1);
			yield return new LogEntryDefinition(1043, LogLevel.Error, LogType.SystemLog, "Error initializing management server communication manager: {0}", 1);
			yield return new LogEntryDefinition(1044, LogLevel.Error, LogType.SystemLog, "Error initializing media database: {0}", 1);
			yield return new LogEntryDefinition(1047, LogLevel.Error, LogType.SystemLog, "Error initializing rule processor: {0}", 1);
			yield return new LogEntryDefinition(1071, LogLevel.Error, LogType.SystemLog, "This recording server is not allowed to start up. Will shut down again.", 0);
			yield return new LogEntryDefinition(1048, LogLevel.Error, LogType.SystemLog, "Error connecting to management server: {0}", 1);
			yield return new LogEntryDefinition(1052, LogLevel.Error, LogType.SystemLog, "Error loading cached recording server configuration: {0}", 1);
			yield return new LogEntryDefinition(1053, LogLevel.Error, LogType.SystemLog, "Error loading cached security configuration: {0}", 1);
			yield return new LogEntryDefinition(1055, LogLevel.Error, LogType.SystemLog, "Error initializing media database: {0}", 1);
			yield return new LogEntryDefinition(1056, LogLevel.Error, LogType.SystemLog, "Error initializing license manager: {0}", 1);
			yield return new LogEntryDefinition(1057, LogLevel.Error, LogType.SystemLog, "Error loading patrolling controller: {0}", 1);
			yield return new LogEntryDefinition(1054, LogLevel.Error, LogType.SystemLog, "Error applying new configuration: {0}", 1);
			yield return new LogEntryDefinition(1058, LogLevel.Error, LogType.SystemLog, "Error applying new configuration: {0}", 1);
			yield return new LogEntryDefinition(1059, LogLevel.Error, LogType.SystemLog, "Error starting smart search manager: {0}", 1);
			yield return new LogEntryDefinition(1065, LogLevel.Error, LogType.SystemLog, "Error starting status manager: {0}", 1);
			yield return new LogEntryDefinition(1060, LogLevel.Error, LogType.SystemLog, "Error starting driver services: {0}", 1);
			yield return new LogEntryDefinition(1061, LogLevel.Error, LogType.SystemLog, "Error starting pipelines: {0}", 1);
			yield return new LogEntryDefinition(1062, LogLevel.Error, LogType.SystemLog, "Error initializing web server: {0}", 1);
			yield return new LogEntryDefinition(1063, LogLevel.Error, LogType.SystemLog, "Error starting recording server rule processor: {0}", 1);
			yield return new LogEntryDefinition(1066, LogLevel.Error, LogType.SystemLog, "Error starting SNMP manager: {0}", 1);
			yield return new LogEntryDefinition(1068, LogLevel.Error, LogType.SystemLog, "Error starting SNMP manager: {0}", 1);
			yield return new LogEntryDefinition(1067, LogLevel.Error, LogType.SystemLog, "Error starting driver event listener: {0}", 1);
			yield return new LogEntryDefinition(1069, LogLevel.Error, LogType.SystemLog, "Error initializing live multicast processor: {0}", 1);
			yield return new LogEntryDefinition(1072, LogLevel.Warning, LogType.SystemLog, "The cached recording server configuration was empty.", 0);
			yield return new LogEntryDefinition(1073, LogLevel.Warning, LogType.SystemLog, "The cached security configuration was empty.", 0);
			yield return new LogEntryDefinition(1074, LogLevel.Error, LogType.SystemLog, "Error starting remote recording manager: {0}", 1);
			yield return new LogEntryDefinition(1075, LogLevel.Error, LogType.SystemLog, "Error initializing audio message manager: {0}", 1);
			yield return new LogEntryDefinition(1064, LogLevel.Error, LogType.SystemLog, "Failed applying external plug-in configurations: {0}", 1);
			yield return new LogEntryDefinition(1070, LogLevel.Error, LogType.SystemLog, "Recording server terminated due to errors: {0}", 1);
			yield return new LogEntryDefinition(2000, LogLevel.Info, LogType.SystemLog, "Recording server '{0}' started.", 1);
			yield return new LogEntryDefinition(4079, LogLevel.Warning, LogType.SystemLog, "Microsoft SNMP Service not installed. SNMP support will be disabled until the Microsoft SNMP Service has been installed.", 0);
			yield return new LogEntryDefinition(4080, LogLevel.Warning, LogType.SystemLog, "Microsoft SNMP Service not installed. SNMP support will be disabled until the Microsoft SNMP Service has been installed.", 0);
			yield return new LogEntryDefinition(4078, LogLevel.Warning, LogType.SystemLog, "Microsoft SNMP Service not started. SNMP support will be disabled until the Microsoft SNMP Service has been started.", 0);
			yield return new LogEntryDefinition(4081, LogLevel.Warning, LogType.SystemLog, "Microsoft SNMP Service not started. SNMP support will be disabled until the Microsoft SNMP Service has been started.", 0);
			yield return new LogEntryDefinition(4076, LogLevel.Info, LogType.SystemLog, "Management server failover started.", 0);
			yield return new LogEntryDefinition(1002, LogLevel.Info, LogType.SystemLog, "Management server stopped.", 0);
			yield return new LogEntryDefinition(1031, LogLevel.Error, LogType.SystemLog, "Error shutting down command processor: {0}", 1);
			yield return new LogEntryDefinition(1033, LogLevel.Error, LogType.SystemLog, "Error shutting down management server rule processor: {0}", 1);
			yield return new LogEntryDefinition(1034, LogLevel.Info, LogType.SystemLog, "Shutdown complete.", 0);
			yield return new LogEntryDefinition(1081, LogLevel.Error, LogType.SystemLog, "Error shutting down recording server rule processor: {0}", 1);
			yield return new LogEntryDefinition(1082, LogLevel.Error, LogType.SystemLog, "Error shutting down patrolling controller: {0}", 1);
			yield return new LogEntryDefinition(1083, LogLevel.Error, LogType.SystemLog, "Error shutting down recording server: {0}", 1);
			yield return new LogEntryDefinition(1086, LogLevel.Error, LogType.SystemLog, "Error shutting down web server: {0}", 1);
			yield return new LogEntryDefinition(1087, LogLevel.Error, LogType.SystemLog, "Error shutting down smart search manager: {0}", 1);
			yield return new LogEntryDefinition(1093, LogLevel.Error, LogType.SystemLog, "Error shutting down, Status Manager: {0}", 1);
			yield return new LogEntryDefinition(1088, LogLevel.Error, LogType.SystemLog, "Error shutting down media database: {0}", 1);
			yield return new LogEntryDefinition(1089, LogLevel.Error, LogType.SystemLog, "Error shutting down command processor: {0}", 1);
			yield return new LogEntryDefinition(1090, LogLevel.Error, LogType.SystemLog, "Error shutting down driver services: {0}", 1);
			yield return new LogEntryDefinition(1092, LogLevel.Error, LogType.SystemLog, "Error shutting down management server communication manager: {0}", 1);
			yield return new LogEntryDefinition(1094, LogLevel.Error, LogType.SystemLog, "Error shutting down driver event listener: {0}", 1);
			yield return new LogEntryDefinition(1096, LogLevel.Error, LogType.SystemLog, "Error shutting down, failover response server: {0}", 1);
			yield return new LogEntryDefinition(1097, LogLevel.Error, LogType.SystemLog, "Error shutting down remote recording manager: {0}", 1);
			yield return new LogEntryDefinition(1098, LogLevel.Error, LogType.SystemLog, "Error shutting down task manager: {0}", 1);
			yield return new LogEntryDefinition(2001, LogLevel.Info, LogType.SystemLog, "Recording server '{0}' stopped.", 1);
			yield return new LogEntryDefinition(1091, LogLevel.Error, LogType.SystemLog, "Error shutting down, SNMP manager: {0}", 1);
			yield return new LogEntryDefinition(1095, LogLevel.Error, LogType.SystemLog, "Error shutting down, SNMP manager: {0}", 1);
			yield return new LogEntryDefinition(1102, LogLevel.Error, LogType.SystemLog, "The system could not obtain a license. No MAC address defined for the hardware. {0}", 1);
			yield return new LogEntryDefinition(1104, LogLevel.Warning, LogType.SystemLog, "Note: Management server is running without a license.", 0);
			yield return new LogEntryDefinition(1105, LogLevel.Warning, LogType.SystemLog, "Error applying license on management server. Error code: {0}", 1);
			yield return new LogEntryDefinition(1106, LogLevel.Info, LogType.SystemLog, "License is valid.", 0);
			yield return new LogEntryDefinition(1107, LogLevel.Warning, LogType.SystemLog, "License has expired.", 0);
			yield return new LogEntryDefinition(1108, LogLevel.Warning, LogType.SystemLog, "License is invalid.", 0);
			yield return new LogEntryDefinition(1110, LogLevel.Info, LogType.SystemLog, "Automatic license activation succeeded.", 0);
			yield return new LogEntryDefinition(1111, LogLevel.Error, LogType.SystemLog, "Automatic license activation failed", 0);
			yield return new LogEntryDefinition(1109, LogLevel.Error, LogType.SystemLog, "Error initializing licensing module: {0}", 1);
			yield return new LogEntryDefinition(2043, LogLevel.Error, LogType.SystemLog, "The hardware is not licensed. Hardware: {0}", 1);
			yield return new LogEntryDefinition(2044, LogLevel.Error, LogType.SystemLog, "The device is not licensed. Device: {0}", 1);
			yield return new LogEntryDefinition(1201, LogLevel.Error, LogType.SystemLog, "Error sending notification: {0}", 1);
			yield return new LogEntryDefinition(1202, LogLevel.Error, LogType.SystemLog, "Error sending e-mail to: {0}", 1);
			yield return new LogEntryDefinition(1203, LogLevel.Error, LogType.SystemLog, "<P>The selected codec is not supported. Select a valid codec.</P>", 0);
			yield return new LogEntryDefinition(1206, LogLevel.Error, LogType.SystemLog, "E-mail sent to: {0}. Recording server image unavailable '{1}'", 1);
			yield return new LogEntryDefinition(1210, LogLevel.Error, LogType.SystemLog, "Database connection lost.", 0);
			yield return new LogEntryDefinition(1211, LogLevel.Info, LogType.SystemLog, "Database connection restored.", 0);
			yield return new LogEntryDefinition(1103, LogLevel.Error, LogType.SystemLog, "Unable to load the certificate used for SSL communication.", 0);
			yield return new LogEntryDefinition(2070, LogLevel.Warning, LogType.SystemLog, "Repair of database table started for device '{0}' in storage '{1}'", 2);
			yield return new LogEntryDefinition(2072, LogLevel.Info, LogType.SystemLog, "Repair of database table completed for device '{0}' in storage '{1}'", 2);
			yield return new LogEntryDefinition(2073, LogLevel.Error, LogType.SystemLog, "The minimum requirement for free disk space in the storage '{0}' has been exceeded.", 1);
			yield return new LogEntryDefinition(2071, LogLevel.Error, LogType.SystemLog, "Repair of database table failed for device '{0}' in storage '{1}'", 2);
			yield return new LogEntryDefinition(2075, LogLevel.Error, LogType.SystemLog, "The storage '{0}' is unavailable. The path is '{1}'.", 2);
			yield return new LogEntryDefinition(2076, LogLevel.Info, LogType.SystemLog, "The storage '{0}' is available again. The path is '{1}'.", 2);
			yield return new LogEntryDefinition(2077, LogLevel.Error, LogType.SystemLog, "The archive '{0}' is unavailable. The path is '{1}'.", 2);
			yield return new LogEntryDefinition(2078, LogLevel.Info, LogType.SystemLog, "The archive '{0}' is available again. The path is '{1}'.", 2);
			yield return new LogEntryDefinition(2080, LogLevel.Warning, LogType.SystemLog, "Repair of database table started for an unknown device in storage '{1}'. The device might have been removed from the system. Device ID: '{0}'", 2);
			yield return new LogEntryDefinition(2082, LogLevel.Info, LogType.SystemLog, "Repair of database table completed for an unknown device in storage '{1}'. The device might have been removed from the system. Device ID: '{0}'", 2);
			yield return new LogEntryDefinition(2081, LogLevel.Error, LogType.SystemLog, "Repair of database table failed for an unknown device in storage '{1}'. The device might have been removed from the system. Device ID: '{0}'", 2);
			yield return new LogEntryDefinition(2085, LogLevel.Error, LogType.SystemLog, "Could not delete data from the storage. Device: '{0}'. Storage name: '{1}'", 2);
			yield return new LogEntryDefinition(2087, LogLevel.Error, LogType.SystemLog, "Could not delete data from storage from an unknown device in the storage '{0}'. Device ID: '{1}'", 2);
			yield return new LogEntryDefinition(2088, LogLevel.Warning, LogType.SystemLog, "Creating a table split took an unusually long time. Device: '{0}'. Storage name: '{1}'", 2);
			yield return new LogEntryDefinition(2089, LogLevel.Warning, LogType.SystemLog, "Creating a table split took an unusually long time. Table: '{0}'. Storage name: '{1}'", 2);
			yield return new LogEntryDefinition(2502, LogLevel.Error, LogType.SystemLog, "Archiving failed. Device '{0}'. Archive name: '{1}'", 2);
			yield return new LogEntryDefinition(2507, LogLevel.Error, LogType.SystemLog, "Archiving failed from unknown device in archive '{0}'. The device ID is: '{1}'", 2);
			yield return new LogEntryDefinition(2600, LogLevel.Info, LogType.SystemLog, "Move of media data has begun.", 0);
			yield return new LogEntryDefinition(2601, LogLevel.Info, LogType.SystemLog, "Move of media data has finished successfully", 0);
			yield return new LogEntryDefinition(2604, LogLevel.Error, LogType.SystemLog, "Move of media data has failed", 0);
			yield return new LogEntryDefinition(2605, LogLevel.Warning, LogType.SystemLog, "Move of media data has failed temporarily. Trying again later.", 0);
			yield return new LogEntryDefinition(2610, LogLevel.Info, LogType.SystemLog, "Move recording storage '{0}' started.", 1);
			yield return new LogEntryDefinition(2611, LogLevel.Info, LogType.SystemLog, "Move recording storage '{0}' finished successfully.", 1);
			yield return new LogEntryDefinition(2612, LogLevel.Warning, LogType.SystemLog, "Move recording storage '{0}' failed temporarily. Trying again later.", 1);
			yield return new LogEntryDefinition(2613, LogLevel.Error, LogType.SystemLog, "Move recording storage '{0}' failed.", 1);
			yield return new LogEntryDefinition(2614, LogLevel.Error, LogType.SystemLog, "Move recording storage '{0}' failed.", 1);
			yield return new LogEntryDefinition(2615, LogLevel.Info, LogType.SystemLog, "Delete recording storage '{0}' started.", 1);
			yield return new LogEntryDefinition(2616, LogLevel.Info, LogType.SystemLog, "Delete recording storage '{0}' finished successfully.", 1);
			yield return new LogEntryDefinition(2617, LogLevel.Warning, LogType.SystemLog, "Delete recording storage '{0}' failed temporarily. Trying again later.", 1);
			yield return new LogEntryDefinition(2618, LogLevel.Error, LogType.SystemLog, "Delete recording storage '{0}' failed.", 1);
			yield return new LogEntryDefinition(2619, LogLevel.Error, LogType.SystemLog, "Delete recording storage '{0}' failed.", 1);
			yield return new LogEntryDefinition(2620, LogLevel.Info, LogType.SystemLog, "Deletion of the archive '{0}' was started.", 1);
			yield return new LogEntryDefinition(2621, LogLevel.Info, LogType.SystemLog, "The archive '{0}' was deleted.", 1);
			yield return new LogEntryDefinition(2622, LogLevel.Warning, LogType.SystemLog, "Deletion of the archive '{0}' failed temporarily. Trying again later.", 1);
			yield return new LogEntryDefinition(2623, LogLevel.Error, LogType.SystemLog, "Deletion of the archive '{0}' failed.", 1);
			yield return new LogEntryDefinition(2624, LogLevel.Error, LogType.SystemLog, "Deletion of the archive '{0}' failed.", 1);
			yield return new LogEntryDefinition(2625, LogLevel.Info, LogType.SystemLog, "Deletion of the archive '{0}' was started.", 1);
			yield return new LogEntryDefinition(2626, LogLevel.Info, LogType.SystemLog, "The archive '{0}' was deleted.", 1);
			yield return new LogEntryDefinition(2627, LogLevel.Warning, LogType.SystemLog, "Deletion of the archive '{0}' failed temporarily. Trying again later.", 1);
			yield return new LogEntryDefinition(2628, LogLevel.Error, LogType.SystemLog, "Deletion of the archive '{0}' failed.", 1);
			yield return new LogEntryDefinition(2629, LogLevel.Error, LogType.SystemLog, "Deletion of the archive '{0}' failed.", 1);
			yield return new LogEntryDefinition(2635, LogLevel.Info, LogType.SystemLog, "Upgrade device media database started.", 0);
			yield return new LogEntryDefinition(2636, LogLevel.Info, LogType.SystemLog, "Upgrade device media database finished successfully.", 0);
			yield return new LogEntryDefinition(2637, LogLevel.Warning, LogType.SystemLog, "Upgrade device media database failed temporarily. Trying again later.", 0);
			yield return new LogEntryDefinition(2638, LogLevel.Error, LogType.SystemLog, "Upgrade device media database failed.", 0);
			yield return new LogEntryDefinition(2639, LogLevel.Error, LogType.SystemLog, "Upgrade device media database failed.", 0);
			yield return new LogEntryDefinition(2650, LogLevel.Info, LogType.SystemLog, "Unmounting the archive '{0}' was started.", 1);
			yield return new LogEntryDefinition(2651, LogLevel.Info, LogType.SystemLog, "The archive '{0}' was unmounted.", 1);
			yield return new LogEntryDefinition(2652, LogLevel.Error, LogType.SystemLog, "Unmounting the archive '{0}' failed.", 1);
			yield return new LogEntryDefinition(2653, LogLevel.Info, LogType.SystemLog, "Mounting the archive '{0}' was started.", 1);
			yield return new LogEntryDefinition(2654, LogLevel.Info, LogType.SystemLog, "The archive '{0}' was mounted.", 1);
			yield return new LogEntryDefinition(2655, LogLevel.Error, LogType.SystemLog, "The archive '{0}' was mounted, but currently unavailable.", 1);
			yield return new LogEntryDefinition(2660, LogLevel.Info, LogType.SystemLog, "Delete all device recordings started.", 0);
			yield return new LogEntryDefinition(2661, LogLevel.Info, LogType.SystemLog, "Recordings deleted.", 0);
			yield return new LogEntryDefinition(2662, LogLevel.Warning, LogType.SystemLog, "Delete all device recordings failed temporarily. Trying again later.", 0);
			yield return new LogEntryDefinition(2663, LogLevel.Error, LogType.SystemLog, "Delete all device recordings failed.", 0);
			yield return new LogEntryDefinition(2664, LogLevel.Error, LogType.SystemLog, "Delete all device recordings failed.", 0);
			yield return new LogEntryDefinition(20126, LogLevel.Error, LogType.SystemLog, "Unable to repair table: {0}. The unrepairable media data has been moved out of media database {1}", 2);
			yield return new LogEntryDefinition(4082, LogLevel.Info, LogType.SystemLog, "Set new encryption password on recording storage '{0}' started.", 1);
			yield return new LogEntryDefinition(4083, LogLevel.Info, LogType.SystemLog, "Set new encryption password on recording storage '{0}' finished successfully.", 1);
			yield return new LogEntryDefinition(4084, LogLevel.Warning, LogType.SystemLog, "Set new encryption password on recording storage '{0}' failed temporarily. Trying again later.", 1);
			yield return new LogEntryDefinition(4085, LogLevel.Error, LogType.SystemLog, "Set new encryption password on recording storage '{0}' failed.", 1);
			yield return new LogEntryDefinition(4086, LogLevel.Error, LogType.SystemLog, "Set new encryption password on recording storage '{0}' failed.", 1);
			yield return new LogEntryDefinition(4087, LogLevel.Info, LogType.SystemLog, "Set new encryption method on recording storage '{0}' started.", 1);
			yield return new LogEntryDefinition(4088, LogLevel.Info, LogType.SystemLog, "Set new encryption method on recording storage '{0}' finished successfully.", 1);
			yield return new LogEntryDefinition(2670, LogLevel.Info, LogType.SystemLog, "Retrieving remote recordings...", 0);
			yield return new LogEntryDefinition(2671, LogLevel.Info, LogType.SystemLog, "Retrieval of remote recordings has finished successfully.", 0);
			yield return new LogEntryDefinition(2672, LogLevel.Error, LogType.SystemLog, "Could not retrieve remote recordings.", 0);
			yield return new LogEntryDefinition(2673, LogLevel.Info, LogType.SystemLog, "Retrieval of remote recordings has been stopped.", 0);
			yield return new LogEntryDefinition(4700, LogLevel.Info, LogType.SystemLog, "Failover server service has returned from failover state.", 0);
			yield return new LogEntryDefinition(4701, LogLevel.Info, LogType.SystemLog, "The failover server {0} has been removed, forcing shutdown of the service...", 1);
			yield return new LogEntryDefinition(4702, LogLevel.Info, LogType.SystemLog, "Taking over the responsibilities for recording server '{0}'", 1);
			yield return new LogEntryDefinition(4703, LogLevel.Error, LogType.SystemLog, "Unable to update the recording server id. The configuration file was not found.", 0);
			yield return new LogEntryDefinition(4704, LogLevel.Info, LogType.SystemLog, "Starting the hot standby server for recording server '{0}'.", 1);
			yield return new LogEntryDefinition(4705, LogLevel.Info, LogType.SystemLog, "Stopping the hot standby server for recording server '{0}'.", 1);
			yield return new LogEntryDefinition(4706, LogLevel.Error, LogType.SystemLog, "Unable to establish encrypted connection to recording server '{0}'.", 1);
			yield return new LogEntryDefinition(4707, LogLevel.Error, LogType.SystemLog, "Unable to establish encrypted connection to failover recording server.", 0);
			yield return new LogEntryDefinition(4708, LogLevel.Error, LogType.SystemLog, "Authorization failed with the recording server '{0}'.", 1);
			yield return new LogEntryDefinition(4709, LogLevel.Error, LogType.SystemLog, "Authorization failed with the failover recording server.", 0);
			yield return new LogEntryDefinition(2700, LogLevel.Info, LogType.SystemLog, "Merging of media data has been started. Source server:{0}", 1);
			yield return new LogEntryDefinition(2701, LogLevel.Error, LogType.SystemLog, "Failed to merge media data. Error: {0}", 1);
			yield return new LogEntryDefinition(2702, LogLevel.Info, LogType.SystemLog, "Merging of media data has ended.", 0);
			yield return new LogEntryDefinition(4711, LogLevel.Error, LogType.SystemLog, "Unable to start failover recording server.", 0);
			yield return new LogEntryDefinition(1250, LogLevel.Error, LogType.SystemLog, "Actions for the rule '{0}' were not distributed to the required recording servers. Error: {1}", 2);
			yield return new LogEntryDefinition(1251, LogLevel.Error, LogType.SystemLog, "Global event '{0}' was not distributed to the required recording servers. Error: {1}", 2);
			yield return new LogEntryDefinition(2002, LogLevel.Info, LogType.SystemLog, "Recording server '{0}' connected", 1);
			yield return new LogEntryDefinition(2003, LogLevel.Warning, LogType.SystemLog, "Recording server '{0}' disconnected", 1);
			yield return new LogEntryDefinition(2008, LogLevel.Info, LogType.SystemLog, "Recording server '{0}' added (not authorized)", 1);
			yield return new LogEntryDefinition(2009, LogLevel.Warning, LogType.SystemLog, "Recording server connected, but is not the same version as the management server.", 0);
			yield return new LogEntryDefinition(4500, LogLevel.Warning, LogType.SystemLog, "Updates to event definitions could not be sent to recording server '{0}' because it is not the latest version.", 1);
			yield return new LogEntryDefinition(4501, LogLevel.Warning, LogType.SystemLog, "Updates to the security configuration could not be sent to recording server '{0}' because it is not the latest version.", 1);
			yield return new LogEntryDefinition(4502, LogLevel.Warning, LogType.SystemLog, "Updates to the configuration could not be sent to recording server '{0}' because it is not the latest version.", 1);
			yield return new LogEntryDefinition(2042, LogLevel.Info, LogType.SystemLog, "Changed MAC address found for hardware: {0}", 1);
			yield return new LogEntryDefinition(2041, LogLevel.Error, LogType.SystemLog, "The recording server could not obtain the MAC address from the hardware: {0}", 1);
			yield return new LogEntryDefinition(2045, LogLevel.Error, LogType.SystemLog, "Cannot update MAC address for hardware '{0}'. The recording server service version is no longer supported. You can still access recordings on the recording server, but it must be upgraded to the current version before you can change its configuration.", 1);
			yield return new LogEntryDefinition(2046, LogLevel.Error, LogType.SystemLog, "Cannot update MAC address for hardware '{0}'. The updated MAC address is invalid.", 1);
			yield return new LogEntryDefinition(2051, LogLevel.Error, LogType.SystemLog, "Plug-in has not been configured for device: {0}", 1);
			yield return new LogEntryDefinition(3010, LogLevel.Info, LogType.SystemLog, "Communication established.", 0);
			yield return new LogEntryDefinition(3011, LogLevel.Info, LogType.SystemLog, "Communication stopped.", 0);
			yield return new LogEntryDefinition(3012, LogLevel.Info, LogType.SystemLog, "Settings changed", 0);
			yield return new LogEntryDefinition(3013, LogLevel.Info, LogType.SystemLog, "Media queue normalized. The device is no longer in overflow mode.", 0);
			yield return new LogEntryDefinition(3020, LogLevel.Warning, LogType.SystemLog, "Media overflow. The device is now in overflow mode.", 0);
			yield return new LogEntryDefinition(3021, LogLevel.Error, LogType.SystemLog, "Change of settings failed", 0);
			yield return new LogEntryDefinition(3022, LogLevel.Error, LogType.SystemLog, "Communication error.", 0);
			yield return new LogEntryDefinition(3501, LogLevel.Error, LogType.SystemLog, "Failed to load driver for the hardware: {0}", 1);
			yield return new LogEntryDefinition(3601, LogLevel.Error, LogType.SystemLog, "Failed to update hardware. The hardware is not found in the recording server configuration. Hardware: {0}", 1);
			yield return new LogEntryDefinition(3602, LogLevel.Error, LogType.SystemLog, "An updated driver is not compatible with already configured hardware. Hardware: {0}. Hardware model: {1}", 2);
			yield return new LogEntryDefinition(3603, LogLevel.Error, LogType.SystemLog, "Unable to find driver for configured hardware. Hardware: {0}. Hardware model: {1}", 2);
			yield return new LogEntryDefinition(3604, LogLevel.Info, LogType.SystemLog, "A new driver is installed for configured hardware. Updating driver. Hardware: {0}. Hardware model: {1}", 2);
			yield return new LogEntryDefinition(3605, LogLevel.Info, LogType.SystemLog, "Driver was successfully updated for hardware '{0}'", 1);
			yield return new LogEntryDefinition(3606, LogLevel.Error, LogType.SystemLog, "Failed to update driver for hardware '{0}'", 1);
			yield return new LogEntryDefinition(3607, LogLevel.Error, LogType.SystemLog, "Cannot update driver for hardware '{0}'. The recording server service version is no longer supported. You can still access recordings on the recording server, but it must be upgraded to the current version before you can change its configuration.", 1);
			yield return new LogEntryDefinition(3608, LogLevel.Error, LogType.SystemLog, "Cannot update driver for hardware '{0}'. The recording server service version is no longer supported. You can still access recordings on the recording server, but it must be upgraded to the current version before you can change its configuration.", 1);
			yield return new LogEntryDefinition(3702, LogLevel.Error, LogType.SystemLog, "The driver definition was not updated during the driver update because the definition was unchanged. Hardware: {0}", 1);
			yield return new LogEntryDefinition(3700, LogLevel.Info, LogType.SystemLog, "Communication established (hardware)", 0);
			yield return new LogEntryDefinition(3701, LogLevel.Info, LogType.SystemLog, "Communication stopped (hardware)", 0);
			yield return new LogEntryDefinition(3609, LogLevel.Error, LogType.SystemLog, "Communication error (hardware)", 0);
			yield return new LogEntryDefinition(3619, LogLevel.Error, LogType.SystemLog, "The system found inconsistencies in the hardware settings of hardware {0}, model {1}", 2);
			yield return new LogEntryDefinition(3622, LogLevel.Error, LogType.SystemLog, "The password of hardware device {0} could not be changed. The password is too long.", 1);
			yield return new LogEntryDefinition(3623, LogLevel.Error, LogType.SystemLog, "The password of hardware device {0} could not be changed. The password is too short or too weak.", 1);
			yield return new LogEntryDefinition(3624, LogLevel.Error, LogType.SystemLog, "The password of hardware device {0} could not be changed. Invalid password.", 1);
			yield return new LogEntryDefinition(3625, LogLevel.Error, LogType.SystemLog, "The password of hardware device {0} could not be changed. The hardware device does not support password changes.", 1);
			yield return new LogEntryDefinition(3626, LogLevel.Error, LogType.SystemLog, "The password of hardware device {0} could not be changed because of a hardware error.", 1);
			yield return new LogEntryDefinition(3627, LogLevel.Error, LogType.SystemLog, "The password of hardware device {0} could not be changed. The hardware device is switched off.", 1);
			yield return new LogEntryDefinition(3628, LogLevel.Error, LogType.SystemLog, "The password of hardware device {0} could not be changed. The hardware device is disabled.", 1);
			yield return new LogEntryDefinition(3629, LogLevel.Error, LogType.SystemLog, "The password of hardware device {0} could not be changed. The hardware device is temporarily blocked.", 1);
			yield return new LogEntryDefinition(3630, LogLevel.Error, LogType.SystemLog, "The password of hardware device {0} was changed successfully, but it failed to start up with the new password.", 1);
			yield return new LogEntryDefinition(3631, LogLevel.Error, LogType.SystemLog, "The password of hardware device {0} could not be changed. The hardware device is read-only.", 1);
			yield return new LogEntryDefinition(3632, LogLevel.Error, LogType.SystemLog, "The password of hardware device {0} could not be changed. The recording server does not recognize the response from the hardware driver.", 1);
			yield return new LogEntryDefinition(3633, LogLevel.Error, LogType.SystemLog, "The password of hardware device {0} could not be changed because of network issues.", 1);
			yield return new LogEntryDefinition(3634, LogLevel.Info, LogType.SystemLog, "The scheduled password change was successful for the following hardware device: '{0}'.", 1);
			yield return new LogEntryDefinition(3635, LogLevel.Error, LogType.SystemLog, "The scheduled password change failed for the following hardware device: '{0}'. Failure reason: '{1}'.", 2);
			yield return new LogEntryDefinition(3636, LogLevel.Error, LogType.SystemLog, "There is no access to hardware device {0}.", 1);
			yield return new LogEntryDefinition(3637, LogLevel.Error, LogType.SystemLog, "User level anonymous is not allowed on hardware device {0}.", 1);
			yield return new LogEntryDefinition(3639, LogLevel.Error, LogType.SystemLog, "The password or the user level may not be changed on hardware device {0}.", 1);
			yield return new LogEntryDefinition(3640, LogLevel.Error, LogType.SystemLog, "The user does not have access for the operation on hardware device {0}.", 1);
			yield return new LogEntryDefinition(3641, LogLevel.Error, LogType.SystemLog, "Operation failed on {0}. The hardware device is disabled.", 1);
			yield return new LogEntryDefinition(3642, LogLevel.Error, LogType.SystemLog, "Operation failed on {0}. The hardware device is temporarily blocked.", 1);
			yield return new LogEntryDefinition(3643, LogLevel.Error, LogType.SystemLog, "Operation failed on {0}. The hardware device is read-only.", 1);
			yield return new LogEntryDefinition(3644, LogLevel.Error, LogType.SystemLog, "Operation failed on {0}. The hardware device is switched off.", 1);
			yield return new LogEntryDefinition(3645, LogLevel.Error, LogType.SystemLog, "No access to hardware device {0}.", 1);
			yield return new LogEntryDefinition(3646, LogLevel.Error, LogType.SystemLog, "Operation failed on: {1}. User '{0}' does not have access to perform the operation.", 2);
			yield return new LogEntryDefinition(3647, LogLevel.Error, LogType.SystemLog, "Operation failed on: {0}. User handling not supported.", 1);
			yield return new LogEntryDefinition(3648, LogLevel.Error, LogType.SystemLog, "Operation failed on: {0}. An unknown error occurred during user handling.", 1);
			yield return new LogEntryDefinition(3649, LogLevel.Error, LogType.SystemLog, "Operation failed on: {1}. User '{0}' not recognized.", 2);
			yield return new LogEntryDefinition(3650, LogLevel.Error, LogType.SystemLog, "Operation failed on: {0}. The currently active user cannot be deleted.", 1);
			yield return new LogEntryDefinition(3651, LogLevel.Error, LogType.SystemLog, "Operation failed on: {1}. User '{0}' is locked by the manufacturer and cannot be edited.", 2);
			yield return new LogEntryDefinition(3652, LogLevel.Error, LogType.SystemLog, "Operation failed on: {1}. User '{0}' already exists.", 2);
			yield return new LogEntryDefinition(3653, LogLevel.Error, LogType.SystemLog, "Operation failed on: {1}. User '{0}' was not created.", 2);
			yield return new LogEntryDefinition(3654, LogLevel.Error, LogType.SystemLog, "Operation failed on: {0}. Network settings management not supported.", 1);
			yield return new LogEntryDefinition(3655, LogLevel.Error, LogType.SystemLog, "Operation failed on: {0}. An unknown error occurred during a network settings operation.", 1);
			yield return new LogEntryDefinition(3656, LogLevel.Error, LogType.SystemLog, "Operation failed on: {0}. Unable to find a suitable network interface address.", 1);
			yield return new LogEntryDefinition(3710, LogLevel.Info, LogType.SystemLog, "The firmware update was successful for hardware device: '{0}'. The new firmware version is: '{1}'", 2);
			yield return new LogEntryDefinition(3711, LogLevel.Error, LogType.SystemLog, "The firmware update operation failed for hardware device: '{0}'.", 1);
			yield return new LogEntryDefinition(3712, LogLevel.Info, LogType.SystemLog, "The transfer of file '{0}' to recording server '{1}' completed successfully.", 2);
			yield return new LogEntryDefinition(3713, LogLevel.Error, LogType.SystemLog, "The transfer of file '{0}' to recording server '{1}' failed.", 2);
			yield return new LogEntryDefinition(3714, LogLevel.Info, LogType.SystemLog, "The file upload to the server completed successfully. File name '{0}', size {1}.", 2);
			yield return new LogEntryDefinition(3715, LogLevel.Error, LogType.SystemLog, "The file upload to the server failed. File name '{0}', size {1}.", 2);
			yield return new LogEntryDefinition(4710, LogLevel.Error, LogType.SystemLog, "The proxy server serving the device driver '{0}' is unavailable. This affects the following hardware: '{1}'. An attempt to restart the driver will be initiated.", 2);
			yield return new LogEntryDefinition(2031, LogLevel.Error, LogType.SystemLog, "Failed to send command to Matrix recipient: {0} or {1} may not be running.", 2);
			yield return new LogEntryDefinition(20123, LogLevel.Info, LogType.SystemLog, "One or more fisheye lens definitions added to the management server.", 0);
			yield return new LogEntryDefinition(20124, LogLevel.Info, LogType.SystemLog, "One or more fisheye lens definitions removed from the management server.", 0);
			yield return new LogEntryDefinition(20125, LogLevel.Info, LogType.SystemLog, "One or more fisheye lens definitions changed on the management server.", 0);
			yield return new LogEntryDefinition(24020, LogLevel.Info, LogType.SystemLog, "The user {0} is created.", 1);
			yield return new LogEntryDefinition(24021, LogLevel.Info, LogType.SystemLog, "The claims from an external IDP are updated for the user {0}.", 1);
			yield return new LogEntryDefinition(3617, LogLevel.Info, LogType.AuditLog, "Hardware password read for '{0}'", 1);
			yield return new LogEntryDefinition(3618, LogLevel.Info, LogType.AuditLog, "Cannot read hardware password for '{0}'. Insufficient user permissions", 1);
			yield return new LogEntryDefinition(4009, LogLevel.Info, LogType.AuditLog, "User not allowed logging on from '{0}'. Access denied.", 1);
			yield return new LogEntryDefinition(4010, LogLevel.Info, LogType.AuditLog, "User not allowed logging in to this system. Access denied.", 0);
			yield return new LogEntryDefinition(4011, LogLevel.Info, LogType.AuditLog, "Login failed for user. Insufficient user permissions", 0);
			yield return new LogEntryDefinition(4038, LogLevel.Info, LogType.AuditLog, "Login failed for user. Insufficient user permissions", 0);
			yield return new LogEntryDefinition(4012, LogLevel.Info, LogType.AuditLog, "Login failed for user. Insufficient user permissions", 0);
			yield return new LogEntryDefinition(4014, LogLevel.Info, LogType.AuditLog, "Login failed for user. Insufficient user permissions", 0);
			yield return new LogEntryDefinition(4013, LogLevel.Info, LogType.AuditLog, "Login failed for user. Insufficient user permissions", 0);
			yield return new LogEntryDefinition(4039, LogLevel.Info, LogType.AuditLog, "User successfully logged in", 0);
			yield return new LogEntryDefinition(4015, LogLevel.Info, LogType.AuditLog, "User successfully logged in to the system from the {0}.", 1);
			yield return new LogEntryDefinition(4016, LogLevel.Info, LogType.AuditLog, "User successfully logged in", 0);
			yield return new LogEntryDefinition(4017, LogLevel.Info, LogType.AuditLog, "User successfully logged in to the system from an external source.", 0);
			yield return new LogEntryDefinition(4018, LogLevel.Info, LogType.AuditLog, "User successfully logged in to the system from HTTP request.", 0);
			yield return new LogEntryDefinition(4020, LogLevel.Info, LogType.AuditLog, "User logged out of the system from the {0}.", 1);
			yield return new LogEntryDefinition(4034, LogLevel.Info, LogType.AuditLog, "Unsuccessful login - Unknown user", 0);
			yield return new LogEntryDefinition(4035, LogLevel.Info, LogType.AuditLog, "Unsuccessful login - Incorrect password", 0);
			yield return new LogEntryDefinition(4036, LogLevel.Info, LogType.AuditLog, "Logout", 0);
			yield return new LogEntryDefinition(4037, LogLevel.Info, LogType.AuditLog, "User connection timed out", 0);
			yield return new LogEntryDefinition(4100, LogLevel.Info, LogType.AuditLog, "Client access denied due to time based security restrictions", 0);
			yield return new LogEntryDefinition(4101, LogLevel.Info, LogType.AuditLog, "Client logged out due to time based security restrictions", 0);
			yield return new LogEntryDefinition(10206, LogLevel.Info, LogType.AuditLog, "{0} preset activation denied due to time based security restrictions", 1);
			yield return new LogEntryDefinition(10116, LogLevel.Info, LogType.AuditLog, "Apply {0} layout denied due to time based security restrictions", 1);
			yield return new LogEntryDefinition(23000, LogLevel.Info, LogType.AuditLog, "User authorized '{0}'", 1);
			yield return new LogEntryDefinition(23001, LogLevel.Info, LogType.AuditLog, "Authorization failed: User can not authorize himself.", 0);
			yield return new LogEntryDefinition(23002, LogLevel.Info, LogType.AuditLog, "User failed to authorize {0}: Insufficient user permissions.", 1);
			yield return new LogEntryDefinition(4030, LogLevel.Info, LogType.AuditLog, "Archive settings changed on '{0}'", 1);
			yield return new LogEntryDefinition(4023, LogLevel.Info, LogType.AuditLog, "Device '{0}' added", 1);
			yield return new LogEntryDefinition(4024, LogLevel.Info, LogType.AuditLog, "Device '{0}' removed", 1);
			yield return new LogEntryDefinition(4025, LogLevel.Info, LogType.AuditLog, "Device '{0}' enabled", 1);
			yield return new LogEntryDefinition(4026, LogLevel.Info, LogType.AuditLog, "Device '{0}' disabled", 1);
			yield return new LogEntryDefinition(4027, LogLevel.Info, LogType.AuditLog, "Device '{0}' renamed to '{1}'", 2);
			yield return new LogEntryDefinition(4028, LogLevel.Info, LogType.AuditLog, "Recording settings changed on '{0}'", 1);
			yield return new LogEntryDefinition(4029, LogLevel.Info, LogType.AuditLog, "Stream settings changed on '{0}'", 1);
			yield return new LogEntryDefinition(4031, LogLevel.Info, LogType.AuditLog, "Motion detection settings changed on '{0}'", 1);
			yield return new LogEntryDefinition(4032, LogLevel.Info, LogType.AuditLog, "Emptying storage for '{0}' requested on the recording server '{1}'.", 2);
			yield return new LogEntryDefinition(4033, LogLevel.Info, LogType.AuditLog, "Database for '{0}' moved to use '{1}'", 2);
			yield return new LogEntryDefinition(4022, LogLevel.Info, LogType.AuditLog, "Settings changed on '{0}'", 1);
			yield return new LogEntryDefinition(4041, LogLevel.Info, LogType.AuditLog, "The device '{0}' was replaced with '{1}'. It inherits the database.", 2);
			yield return new LogEntryDefinition(4042, LogLevel.Info, LogType.AuditLog, "The device '{0}' was replaced with '{1}'.", 2);
			yield return new LogEntryDefinition(3610, LogLevel.Info, LogType.AuditLog, "Hardware '{0}' added", 1);
			yield return new LogEntryDefinition(3611, LogLevel.Info, LogType.AuditLog, "Hardware '{0}' removed", 1);
			yield return new LogEntryDefinition(3612, LogLevel.Info, LogType.AuditLog, "Hardware '{0}' enabled", 1);
			yield return new LogEntryDefinition(3613, LogLevel.Info, LogType.AuditLog, "Hardware '{0}' disabled", 1);
			yield return new LogEntryDefinition(3614, LogLevel.Info, LogType.AuditLog, "Hardware '{0}' renamed to '{1}'", 2);
			yield return new LogEntryDefinition(3615, LogLevel.Info, LogType.AuditLog, "Settings changed on '{0}'", 1);
			yield return new LogEntryDefinition(3616, LogLevel.Info, LogType.AuditLog, "Hardware '{0}' replaced by '{1}'", 2);
			yield return new LogEntryDefinition(4040, LogLevel.Info, LogType.AuditLog, "Privacy mask settings changed on '{0}'", 1);
			yield return new LogEntryDefinition(7101, LogLevel.Info, LogType.AuditLog, "Rule '{0}' created", 1);
			yield return new LogEntryDefinition(7102, LogLevel.Info, LogType.AuditLog, "Rule '{0}' deleted", 1);
			yield return new LogEntryDefinition(7103, LogLevel.Info, LogType.AuditLog, "Rule '{0}' edited", 1);
			yield return new LogEntryDefinition(7104, LogLevel.Info, LogType.AuditLog, "Rule '{0}' disabled", 1);
			yield return new LogEntryDefinition(7105, LogLevel.Info, LogType.AuditLog, "Rule '{0}' enabled", 1);
			yield return new LogEntryDefinition(7106, LogLevel.Info, LogType.AuditLog, "Rule '{0}' renamed to '{1}'", 2);
			yield return new LogEntryDefinition(7120, LogLevel.Info, LogType.AuditLog, "Created playback restriction '{0}'.", 1);
			yield return new LogEntryDefinition(7121, LogLevel.Info, LogType.AuditLog, "Updated playback restriction '{0}'.", 1);
			yield return new LogEntryDefinition(7122, LogLevel.Info, LogType.AuditLog, "Deleted playback restriction '{0}'.", 1);
			yield return new LogEntryDefinition(7123, LogLevel.Info, LogType.AuditLog, "'{0}' has entered restricted live mode.", 1);
			yield return new LogEntryDefinition(7124, LogLevel.Info, LogType.AuditLog, "'{0}' has exited restricted live mode and has been transformed into playback restriction '{1}'", 2);
			yield return new LogEntryDefinition(7125, LogLevel.Info, LogType.AuditLog, "Updated live restriction for '{0}'.", 1);
			yield return new LogEntryDefinition(7126, LogLevel.Info, LogType.AuditLog, "Deleted live restriction '{0}'.", 1);
			yield return new LogEntryDefinition(8001, LogLevel.Info, LogType.AuditLog, "Time profile '{0}' created", 1);
			yield return new LogEntryDefinition(8002, LogLevel.Info, LogType.AuditLog, "Time profile '{0}' deleted", 1);
			yield return new LogEntryDefinition(8003, LogLevel.Info, LogType.AuditLog, "Time profile '{0}' edited", 1);
			yield return new LogEntryDefinition(8004, LogLevel.Info, LogType.AuditLog, "Time profile '{0}' renamed to '{1}'", 2);
			yield return new LogEntryDefinition(9001, LogLevel.Info, LogType.AuditLog, "Role '{0}' created", 1);
			yield return new LogEntryDefinition(9002, LogLevel.Info, LogType.AuditLog, "Role '{0}' deleted", 1);
			yield return new LogEntryDefinition(9003, LogLevel.Info, LogType.AuditLog, "Role '{0}' access permissions changed", 1);
			yield return new LogEntryDefinition(9004, LogLevel.Info, LogType.AuditLog, "Role '{0}' renamed to '{1}'", 2);
			yield return new LogEntryDefinition(9005, LogLevel.Info, LogType.AuditLog, "User '{0}' added to the '{1}' role", 2);
			yield return new LogEntryDefinition(9006, LogLevel.Info, LogType.AuditLog, "User '{0}' removed from the '{1}' role", 2);
			yield return new LogEntryDefinition(9007, LogLevel.Info, LogType.AuditLog, "User group '{0}' added to the '{1}' role", 2);
			yield return new LogEntryDefinition(9008, LogLevel.Info, LogType.AuditLog, "User group '{0}' removed from the '{1}' role", 2);
			yield return new LogEntryDefinition(9009, LogLevel.Info, LogType.AuditLog, "The access permission '{0}' - '{1}' on role '{2}' has been changed to: {3}", 4);
			yield return new LogEntryDefinition(9010, LogLevel.Info, LogType.AuditLog, "The state Allow for the access permission '{0}'-'{1}' is granted", 2);
			yield return new LogEntryDefinition(9011, LogLevel.Info, LogType.AuditLog, "The state Allow for the access permission '{0}'-'{1}' is revoked", 2);
			yield return new LogEntryDefinition(9012, LogLevel.Info, LogType.AuditLog, "The state Deny for the access permission '{0}'-'{1}' is granted", 2);
			yield return new LogEntryDefinition(9013, LogLevel.Info, LogType.AuditLog, "The state Deny for the access permission '{0}'-'{1}' is revoked", 2);
			yield return new LogEntryDefinition(9014, LogLevel.Info, LogType.AuditLog, "The access permission '{0}'-'{1}' is granted on '{2}'", 3);
			yield return new LogEntryDefinition(9015, LogLevel.Info, LogType.AuditLog, "The access permission '{0}'-'{1}' is revoked on '{2}'", 3);
			yield return new LogEntryDefinition(9016, LogLevel.Info, LogType.AuditLog, "The time profile is changed for access permission '{0}'-'{1}' from time profile '{2}' to time profile '{3}' on '{4}'", 5);
			yield return new LogEntryDefinition(9017, LogLevel.Info, LogType.AuditLog, "The time profile is changed for access permission '{0}'-'{1}' from time profile '{2}' to system time profile '{3}' on '{4}'", 5);
			yield return new LogEntryDefinition(9018, LogLevel.Info, LogType.AuditLog, "The time profile is changed for access permission '{0}'-'{1}' from system time profile '{2}' to time profile '{3}' on '{4}'", 5);
			yield return new LogEntryDefinition(9019, LogLevel.Info, LogType.AuditLog, "The time profile is changed for access permission '{0}'-'{1}' from system time profile '{2}' to system time profile '{3}' on '{4}'", 5);
			yield return new LogEntryDefinition(9020, LogLevel.Info, LogType.AuditLog, "The restriction is changed for access permission '{0}'-'{1}' from no restriction to be restricted '{2}' days on '{3}'", 4);
			yield return new LogEntryDefinition(9021, LogLevel.Info, LogType.AuditLog, "The restriction is changed for access permission '{0}'-'{1}' from no restriction to be restricted '{2}' minutes on '{3}'", 4);
			yield return new LogEntryDefinition(9022, LogLevel.Info, LogType.AuditLog, "The restriction is changed for access permission '{0}'-'{1}' from restricted '{2}' minutes to be without any restriction on '{3}'", 4);
			yield return new LogEntryDefinition(9023, LogLevel.Info, LogType.AuditLog, "The restriction is changed for access permission '{0}'-'{1}' from restricted '{2}' minutes to restricted '{3}' minutes on '{4}'", 5);
			yield return new LogEntryDefinition(9024, LogLevel.Info, LogType.AuditLog, "The restriction is changed for access permission '{0}'-'{1}' from restricted '{2}' minutes to restricted '{3}' days on '{4}'", 5);
			yield return new LogEntryDefinition(9025, LogLevel.Info, LogType.AuditLog, "The restriction is changed for access permission '{0}'-'{1}' from restricted '{2}' days to be without any restriction on '{3}'", 4);
			yield return new LogEntryDefinition(9026, LogLevel.Info, LogType.AuditLog, "The restriction is changed for access permission '{0}'-'{1}' from restricted '{2}' days to restricted '{3}' minutes on '{4}'", 5);
			yield return new LogEntryDefinition(9027, LogLevel.Info, LogType.AuditLog, "The restriction is changed for access permission '{0}'-'{1}' from restricted '{2}' days to restricted '{3}' days on '{4}'", 5);
			yield return new LogEntryDefinition(9028, LogLevel.Info, LogType.AuditLog, "The priority is changed for access permission '{0}'-'{1}' from '{2}' to '{3}' on '{4}'", 5);
			yield return new LogEntryDefinition(9029, LogLevel.Info, LogType.AuditLog, "The access permission '{0}'-'{1}' is granted on '{2}' with default priority '{3}'", 4);
			yield return new LogEntryDefinition(9030, LogLevel.Info, LogType.AuditLog, "The access permission '{0}'-'{1}' is revoked on '{2}' with default priority '{3}'", 4);
			yield return new LogEntryDefinition(9031, LogLevel.Info, LogType.AuditLog, "The {0} profile option is changed from '{1}' to '{2}' for the role '{3}'", 4);
			yield return new LogEntryDefinition(9032, LogLevel.Info, LogType.AuditLog, "The Default time profile option is changed from '{0}' to '{1}' for the role '{2}'", 3);
			yield return new LogEntryDefinition(9033, LogLevel.Info, LogType.AuditLog, "The Only allow login within time profile option is changed from '{0}' to '{1}' for the role '{2}'", 3);
			yield return new LogEntryDefinition(9034, LogLevel.Info, LogType.AuditLog, "The Allow {0} login option is granted for the role '{1}'", 2);
			yield return new LogEntryDefinition(9035, LogLevel.Info, LogType.AuditLog, "The Allow {0} login option is revoked for the role '{1}'", 2);
			yield return new LogEntryDefinition(9036, LogLevel.Info, LogType.AuditLog, "The Login authorization required option is granted for the role '{0}'", 1);
			yield return new LogEntryDefinition(9037, LogLevel.Info, LogType.AuditLog, "The Login authorization required option is revoked for the role '{0}'", 1);
			yield return new LogEntryDefinition(9038, LogLevel.Info, LogType.AuditLog, "The Make users anonymous during PTZ sessions option is granted for the role '{0}'", 1);
			yield return new LogEntryDefinition(9039, LogLevel.Info, LogType.AuditLog, "The Make users anonymous during PTZ sessions option is revoked for the role '{0}'", 1);
			yield return new LogEntryDefinition(9050, LogLevel.Info, LogType.AuditLog, "The '{0}' access permissions could not be changed due to insufficient user permissions.", 1);
			yield return new LogEntryDefinition(9051, LogLevel.Info, LogType.AuditLog, "The '{0}' access permissions on '{1}' could not be changed due to insufficient user permissions.", 2);
			yield return new LogEntryDefinition(9098, LogLevel.Info, LogType.AuditLog, "Claim '{2}' with value '{3}' from external IDP '{1}' was added to the role '{0}'", 4);
			yield return new LogEntryDefinition(9099, LogLevel.Info, LogType.AuditLog, "Claim '{2}' with value '{3}' from external IDP '{1}' was removed from the role '{0}'", 4);
			yield return new LogEntryDefinition(9095, LogLevel.Info, LogType.AuditLog, "External IDP '{0}' was created.", 1);
			yield return new LogEntryDefinition(9096, LogLevel.Info, LogType.AuditLog, "Settings for the external IDP '{0}' were changed.", 1);
			yield return new LogEntryDefinition(9097, LogLevel.Info, LogType.AuditLog, "External IDP '{0}' was deleted.", 1);
			yield return new LogEntryDefinition(9100, LogLevel.Info, LogType.AuditLog, "Claim '{0}' was created.", 1);
			yield return new LogEntryDefinition(9101, LogLevel.Info, LogType.AuditLog, "Claim '{0}' was changed.", 1);
			yield return new LogEntryDefinition(9102, LogLevel.Info, LogType.AuditLog, "Claim '{0}' was deleted.", 1);
			yield return new LogEntryDefinition(9103, LogLevel.Info, LogType.AuditLog, "'{0}' URIs were set to '{1}'.", 2);
			yield return new LogEntryDefinition(9110, LogLevel.Info, LogType.AuditLog, "The trusted issuer {0} is created.", 1);
			yield return new LogEntryDefinition(9111, LogLevel.Info, LogType.AuditLog, "The trusted issuer {0} is updated.", 1);
			yield return new LogEntryDefinition(9112, LogLevel.Info, LogType.AuditLog, "The trusted issuer {0} is deleted.", 1);
			yield return new LogEntryDefinition(4021, LogLevel.Info, LogType.AuditLog, "A user has authorized the recording server: {0}.", 1);
			yield return new LogEntryDefinition(4043, LogLevel.Info, LogType.AuditLog, "User has created the recording server: {0}.", 1);
			yield return new LogEntryDefinition(4044, LogLevel.Info, LogType.AuditLog, "GPS coordinates of device '{0}' changed to '{1}'", 2);
			yield return new LogEntryDefinition(4045, LogLevel.Info, LogType.AuditLog, "Short name of device '{0}' changed to '{1}'", 2);
			yield return new LogEntryDefinition(4050, LogLevel.Info, LogType.AuditLog, "Shortcut of device '{0}' changed to '{1}'", 2);
			yield return new LogEntryDefinition(4051, LogLevel.Info, LogType.AuditLog, "Failover settings of device '{0}' changed to '{1}'", 2);
			yield return new LogEntryDefinition(4046, LogLevel.Info, LogType.AuditLog, "Icon identifier for device '{0}' changed to '{1}'", 2);
			yield return new LogEntryDefinition(4047, LogLevel.Info, LogType.AuditLog, "Direction of device '{0}' changed to '{1}' degrees", 2);
			yield return new LogEntryDefinition(4048, LogLevel.Info, LogType.AuditLog, "Depth of device '{0}' changed to '{1}' meters", 2);
			yield return new LogEntryDefinition(4049, LogLevel.Info, LogType.AuditLog, "Field of view of device '{0}' changed to '{1}' degrees", 2);
			yield return new LogEntryDefinition(2101, LogLevel.Info, LogType.AuditLog, "Device multicast changed on '{0}' to '{1}'", 2);
			yield return new LogEntryDefinition(2102, LogLevel.Info, LogType.AuditLog, "Timeout for manual PTZ session changed on '{0}' to '{1}'", 2);
			yield return new LogEntryDefinition(2103, LogLevel.Info, LogType.AuditLog, "Timeout for reserved PTZ session changed on '{0}' to '{1}'", 2);
			yield return new LogEntryDefinition(2104, LogLevel.Info, LogType.AuditLog, "Timeout for pause patrolling session changed on '{0}' to '{1}'", 2);
			yield return new LogEntryDefinition(2011, LogLevel.Info, LogType.AuditLog, "Recording server '{0}' removed", 1);
			yield return new LogEntryDefinition(2012, LogLevel.Info, LogType.AuditLog, "Recording server '{0}' renamed to '{1}'", 2);
			yield return new LogEntryDefinition(2013, LogLevel.Info, LogType.AuditLog, "New directory created on recording server '{0}': '{1}'", 2);
			yield return new LogEntryDefinition(10001, LogLevel.Info, LogType.AuditLog, "{0} '{1}' created", 2);
			yield return new LogEntryDefinition(10002, LogLevel.Info, LogType.AuditLog, "{0} '{1}' deleted", 2);
			yield return new LogEntryDefinition(10003, LogLevel.Info, LogType.AuditLog, "{0} '{1}' renamed to '{2}'", 3);
			yield return new LogEntryDefinition(10201, LogLevel.Info, LogType.AuditLog, "{0} preset '{1}' added to the '{2}' {0}", 3);
			yield return new LogEntryDefinition(10203, LogLevel.Info, LogType.AuditLog, "{0} preset '{1}' for the '{2}' {0} deleted", 3);
			yield return new LogEntryDefinition(10202, LogLevel.Info, LogType.AuditLog, "{0} preset '{1}' for the '{2}' {0} monitor edited", 3);
			yield return new LogEntryDefinition(10204, LogLevel.Info, LogType.AuditLog, "{0} preset '{1}' renamed to '{2}'", 3);
			yield return new LogEntryDefinition(10205, LogLevel.Info, LogType.AuditLog, "{0} preset '{1}' selected", 2);
			yield return new LogEntryDefinition(10101, LogLevel.Info, LogType.AuditLog, "{0} monitor '{1}' created", 2);
			yield return new LogEntryDefinition(10102, LogLevel.Info, LogType.AuditLog, "{0} monitor '{1}' deleted", 2);
			yield return new LogEntryDefinition(10104, LogLevel.Info, LogType.AuditLog, "{0} monitor '{1}' renamed to '{2}'", 3);
			yield return new LogEntryDefinition(10103, LogLevel.Info, LogType.AuditLog, "{0} monitor '{1}' edited", 2);
			yield return new LogEntryDefinition(10119, LogLevel.Info, LogType.AuditLog, "{0} '{1}' edited", 2);
			yield return new LogEntryDefinition(11001, LogLevel.Info, LogType.AuditLog, "Map '{0}' created with this image '{1}'", 2);
			yield return new LogEntryDefinition(11002, LogLevel.Info, LogType.AuditLog, "Map '{0}' deleted", 1);
			yield return new LogEntryDefinition(11004, LogLevel.Info, LogType.AuditLog, "Map image for map '{0}' replaced with this image '{1}'", 2);
			yield return new LogEntryDefinition(11003, LogLevel.Info, LogType.AuditLog, "Map '{0}' renamed to '{1}'", 2);
			yield return new LogEntryDefinition(11005, LogLevel.Info, LogType.AuditLog, "Hot zone link to map '{0}' created on '{1}'", 2);
			yield return new LogEntryDefinition(11006, LogLevel.Info, LogType.AuditLog, "Hot zone link to map '{0}' removed from '{1}'", 2);
			yield return new LogEntryDefinition(11007, LogLevel.Info, LogType.AuditLog, "'{0}' placed on map '{1}'", 2);
			yield return new LogEntryDefinition(11008, LogLevel.Info, LogType.AuditLog, "'{0}' removed from map '{1}'", 2);
			yield return new LogEntryDefinition(18001, LogLevel.Info, LogType.AuditLog, "The {0} profile '{1}' created.", 2);
			yield return new LogEntryDefinition(18002, LogLevel.Info, LogType.AuditLog, "Settings for the {0} profile '{1}' changed.", 2);
			yield return new LogEntryDefinition(18003, LogLevel.Info, LogType.AuditLog, "The {0} profile '{1}' deleted.", 2);
			yield return new LogEntryDefinition(18004, LogLevel.Info, LogType.AuditLog, "The {0} profile '{1}' renamed to '{2}'.", 3);
			yield return new LogEntryDefinition(19001, LogLevel.Info, LogType.AuditLog, "The site '{0}' on '{1}' was added to the hierarchy.", 2);
			yield return new LogEntryDefinition(19002, LogLevel.Info, LogType.AuditLog, "The site '{0}' on '{1}' was detached from hierarchy.", 2);
			yield return new LogEntryDefinition(19003, LogLevel.Info, LogType.AuditLog, "Site detached from hierarchy.", 0);
			yield return new LogEntryDefinition(19004, LogLevel.Info, LogType.AuditLog, "The site administrator accepted the inclusion of the site {0} in the hierarchy under {1}.", 2);
			yield return new LogEntryDefinition(19005, LogLevel.Info, LogType.AuditLog, "The site administrator denied the inclusion of the site {0} in the hierarchy under {1}.", 2);
			yield return new LogEntryDefinition(19006, LogLevel.Info, LogType.AuditLog, "Surveillance system '{0}' renamed to '{1}'.", 2);
			yield return new LogEntryDefinition(20001, LogLevel.Info, LogType.AuditLog, "Storage '{0}' added.", 1);
			yield return new LogEntryDefinition(20002, LogLevel.Info, LogType.AuditLog, "Storage settings in '{0}' edited.", 1);
			yield return new LogEntryDefinition(20003, LogLevel.Info, LogType.AuditLog, "Storage '{0}' deleted.", 1);
			yield return new LogEntryDefinition(20004, LogLevel.Info, LogType.AuditLog, "Storage '{0}' moved from '{1}' to '{2}'.", 3);
			yield return new LogEntryDefinition(20005, LogLevel.Info, LogType.AuditLog, "Archive '{0}' added to '{1}'.", 2);
			yield return new LogEntryDefinition(20006, LogLevel.Info, LogType.AuditLog, "Archive '{0}' in '{1}' edited.", 2);
			yield return new LogEntryDefinition(20007, LogLevel.Info, LogType.AuditLog, "Archive '{0}' in '{1}' moved from '{2}' to '{3}'.", 4);
			yield return new LogEntryDefinition(20008, LogLevel.Info, LogType.AuditLog, "Archive '{0}' deleted from '{1}'.", 2);
			yield return new LogEntryDefinition(20009, LogLevel.Info, LogType.AuditLog, "Archive '{0}' in '{1}' has been mounted.", 2);
			yield return new LogEntryDefinition(20010, LogLevel.Info, LogType.AuditLog, "Archive '{0}' in '{1}' has been unmounted.", 2);
			yield return new LogEntryDefinition(20100, LogLevel.Info, LogType.AuditLog, "Storage '{0}' selected for recording on '{1}'.", 2);
			yield return new LogEntryDefinition(20101, LogLevel.Info, LogType.AuditLog, "Recording enabled on '{0}'.", 1);
			yield return new LogEntryDefinition(20102, LogLevel.Info, LogType.AuditLog, "Recording disabled on '{0}'.", 1);
			yield return new LogEntryDefinition(20103, LogLevel.Info, LogType.AuditLog, "Pre-buffer enabled on '{0}'.", 1);
			yield return new LogEntryDefinition(20104, LogLevel.Info, LogType.AuditLog, "Pre-buffer disabled on '{0}'.", 1);
			yield return new LogEntryDefinition(20105, LogLevel.Info, LogType.AuditLog, "Remote recording enabled on '{0}'.", 1);
			yield return new LogEntryDefinition(20106, LogLevel.Info, LogType.AuditLog, "Remote recording disabled on '{0}'.", 1);
			yield return new LogEntryDefinition(20107, LogLevel.Info, LogType.AuditLog, "Playback of remote recording enabled on '{0}'.", 1);
			yield return new LogEntryDefinition(20108, LogLevel.Info, LogType.AuditLog, "Playback of remote recording disabled on '{0}'.", 1);
			yield return new LogEntryDefinition(20109, LogLevel.Info, LogType.AuditLog, "Retrieval of remote recordings requested on '{0}'.", 1);
			yield return new LogEntryDefinition(20120, LogLevel.Info, LogType.AuditLog, "Fisheye lens settings enabled on '{0}'.", 1);
			yield return new LogEntryDefinition(20121, LogLevel.Info, LogType.AuditLog, "Fisheye lens settings disabled on '{0}'.", 1);
			yield return new LogEntryDefinition(20122, LogLevel.Info, LogType.AuditLog, "Fisheye lens settings changed on '{0}'.", 1);
			yield return new LogEntryDefinition(20200, LogLevel.Info, LogType.AuditLog, "System configuration was exported.", 0);
			yield return new LogEntryDefinition(20201, LogLevel.Info, LogType.AuditLog, "System configuration was restored.", 0);
			yield return new LogEntryDefinition(20210, LogLevel.Info, LogType.AuditLog, "Remote connect server '{0}' added.", 1);
			yield return new LogEntryDefinition(20211, LogLevel.Info, LogType.AuditLog, "Remote connect server '{0}' modified.", 1);
			yield return new LogEntryDefinition(20212, LogLevel.Info, LogType.AuditLog, "Remote connect server '{0}' removed.", 1);
			yield return new LogEntryDefinition(20213, LogLevel.Info, LogType.AuditLog, "Remote connect hardware '{0}' added.", 1);
			yield return new LogEntryDefinition(20214, LogLevel.Info, LogType.AuditLog, "Remote connect hardware '{0}' modified.", 1);
			yield return new LogEntryDefinition(20217, LogLevel.Info, LogType.AuditLog, "Remote connect hardware '{0}' renamed to '{1}'.", 2);
			yield return new LogEntryDefinition(20215, LogLevel.Info, LogType.AuditLog, "Remote connect hardware '{0}' removed.", 1);
			yield return new LogEntryDefinition(20216, LogLevel.Info, LogType.AuditLog, "Remote connect server '{0}' renamed to '{1}'.", 2);
			yield return new LogEntryDefinition(21001, LogLevel.Info, LogType.AuditLog, "Configuration report exported.", 0);
			yield return new LogEntryDefinition(22001, LogLevel.Info, LogType.AuditLog, "Basic user '{0}' created.", 1);
			yield return new LogEntryDefinition(22002, LogLevel.Info, LogType.AuditLog, "Basic user '{0}' deleted.", 1);
			yield return new LogEntryDefinition(22003, LogLevel.Info, LogType.AuditLog, "The settings for basic user '{0}' are changed.", 1);
			yield return new LogEntryDefinition(22004, LogLevel.Info, LogType.AuditLog, "Basic user '{0}' renamed to '{1}'.", 2);
			yield return new LogEntryDefinition(22005, LogLevel.Info, LogType.AuditLog, "The password for basic user '{0}' is changed.", 1);
			yield return new LogEntryDefinition(22006, LogLevel.Info, LogType.AuditLog, "The site information '{0}' is created with value '{1}'", 2);
			yield return new LogEntryDefinition(22007, LogLevel.Info, LogType.AuditLog, "The site information '{0}' is deleted.", 1);
			yield return new LogEntryDefinition(22008, LogLevel.Info, LogType.AuditLog, "The settings for the site information '{0}' is changed to '{1}'", 2);
			yield return new LogEntryDefinition(22009, LogLevel.Info, LogType.AuditLog, "Basic user '{0}' is locked out.", 1);
			yield return new LogEntryDefinition(22010, LogLevel.Info, LogType.AuditLog, "The default evidence lock time option for the role '{0}' was changed to '{1}'", 2);
			yield return new LogEntryDefinition(22011, LogLevel.Info, LogType.AuditLog, "Basic user '{0}' is now enabled.", 1);
			yield return new LogEntryDefinition(22012, LogLevel.Info, LogType.AuditLog, "Basic user '{0}' must change password.", 1);
			yield return new LogEntryDefinition(22013, LogLevel.Info, LogType.AuditLog, "The marked data (with evidence locks or bookmarks) '{0}' was updated.", 1);
			yield return new LogEntryDefinition(22014, LogLevel.Info, LogType.AuditLog, "Marked data (with evidence locks or bookmarks) '{0}' was deleted.", 1);
			yield return new LogEntryDefinition(22015, LogLevel.Info, LogType.AuditLog, "Marked data (with evidence locks or bookmarks) '{0}' was created.", 1);
			yield return new LogEntryDefinition(22018, LogLevel.Info, LogType.AuditLog, "The system dashboard threshold '{0}' was created.", 1);
			yield return new LogEntryDefinition(22019, LogLevel.Info, LogType.AuditLog, "The system dashboard threshold '{0}' was updated.", 1);
			yield return new LogEntryDefinition(22020, LogLevel.Info, LogType.AuditLog, "The system dashboard threshold '{0}' was deleted.", 1);
			yield return new LogEntryDefinition(23004, LogLevel.Info, LogType.AuditLog, "The system dashboard configuration '{0}' was updated.", 1);
			yield return new LogEntryDefinition(23005, LogLevel.Info, LogType.AuditLog, "The system dashboard configuration '{0}' was deleted.", 1);
			yield return new LogEntryDefinition(23006, LogLevel.Info, LogType.AuditLog, "The system dashboard configuration '{0}' was created.", 1);
			yield return new LogEntryDefinition(23003, LogLevel.Info, LogType.AuditLog, "User initiated a move of the hardware '{0}' to the recording server '{1}'.", 2);
			yield return new LogEntryDefinition(3620, LogLevel.Info, LogType.AuditLog, "User initiated a password change of the hardware '{0}' on recording server '{1}'.", 2);
			yield return new LogEntryDefinition(23007, LogLevel.Info, LogType.AuditLog, "Automatic activation is enabled.", 0);
			yield return new LogEntryDefinition(23008, LogLevel.Info, LogType.AuditLog, "Automatic activation is disabled.", 0);
			yield return new LogEntryDefinition(23009, LogLevel.Info, LogType.AuditLog, "The credentials for online activation have been changed.", 0);
			yield return new LogEntryDefinition(23016, LogLevel.Info, LogType.AuditLog, "Audio message '{0}' created.", 1);
			yield return new LogEntryDefinition(23017, LogLevel.Info, LogType.AuditLog, "The content of the audio message '{0}' is changed.", 1);
			yield return new LogEntryDefinition(23018, LogLevel.Info, LogType.AuditLog, "Audio message '{0}' deleted.", 1);
			yield return new LogEntryDefinition(3660, LogLevel.Info, LogType.AuditLog, "User initiated pre-configuration of credentials for hardware '{0}' on recording server '{1}'.", 2);
			yield return new LogEntryDefinition(3670, LogLevel.Info, LogType.AuditLog, "User initiated firmware update for hardware '{0}' on recording server '{1}'.", 2);
			yield return new LogEntryDefinition(3672, LogLevel.Info, LogType.AuditLog, "User initiated transfer of file '{0}' to recording server '{1}'.", 2);
			yield return new LogEntryDefinition(3671, LogLevel.Info, LogType.AuditLog, "User initiated a file upload to the server. Target file name '{0}', size {1}.", 2);
			yield return new LogEntryDefinition(2032, LogLevel.Info, LogType.AuditLog, "Matrix created: '{0}' ", 1);
			yield return new LogEntryDefinition(2033, LogLevel.Info, LogType.AuditLog, "Matrix updated: '{0}' ", 1);
			yield return new LogEntryDefinition(2034, LogLevel.Info, LogType.AuditLog, "Matrix deleted: '{0}' ", 1);
			yield return new LogEntryDefinition(4060, LogLevel.Info, LogType.AuditLog, "Client live feed requested.", 0);
			yield return new LogEntryDefinition(4061, LogLevel.Info, LogType.AuditLog, "Client live feed terminated.", 0);
			yield return new LogEntryDefinition(12001, LogLevel.Info, LogType.AuditLog, "Client live feed requested", 0);
			yield return new LogEntryDefinition(12002, LogLevel.Info, LogType.AuditLog, "Client live feed terminated", 0);
			yield return new LogEntryDefinition(12003, LogLevel.Info, LogType.AuditLog, "Client live feed denied", 0);
			yield return new LogEntryDefinition(12013, LogLevel.Info, LogType.AuditLog, "Client live feed denied due to time based security restrictions", 0);
			yield return new LogEntryDefinition(12004, LogLevel.Info, LogType.AuditLog, "Client live feed requested", 0);
			yield return new LogEntryDefinition(12005, LogLevel.Info, LogType.AuditLog, "Client live feed terminated", 0);
			yield return new LogEntryDefinition(12006, LogLevel.Info, LogType.AuditLog, "Client live feed denied", 0);
			yield return new LogEntryDefinition(12007, LogLevel.Info, LogType.AuditLog, "Start speaking to speakers", 0);
			yield return new LogEntryDefinition(12008, LogLevel.Info, LogType.AuditLog, "Stop speaking to speakers", 0);
			yield return new LogEntryDefinition(12009, LogLevel.Info, LogType.AuditLog, "Speaking to speaker denied", 0);
			yield return new LogEntryDefinition(12010, LogLevel.Info, LogType.AuditLog, "Client live feed requested", 0);
			yield return new LogEntryDefinition(12011, LogLevel.Info, LogType.AuditLog, "Client live feed terminated", 0);
			yield return new LogEntryDefinition(12012, LogLevel.Info, LogType.AuditLog, "Client live feed denied", 0);
			yield return new LogEntryDefinition(12014, LogLevel.Info, LogType.AuditLog, "Client live feed denied due to time based security restrictions", 0);
			yield return new LogEntryDefinition(12015, LogLevel.Info, LogType.AuditLog, "Speech denied due to time based security restriction", 0);
			yield return new LogEntryDefinition(12016, LogLevel.Info, LogType.AuditLog, "Client live feed denied due to time based security restrictions", 0);
			yield return new LogEntryDefinition(12017, LogLevel.Info, LogType.AuditLog, "Client live feed requested", 0);
			yield return new LogEntryDefinition(12018, LogLevel.Info, LogType.AuditLog, "Client live feed terminated", 0);
			yield return new LogEntryDefinition(12019, LogLevel.Info, LogType.AuditLog, "Client live feed denied", 0);
			yield return new LogEntryDefinition(12020, LogLevel.Info, LogType.AuditLog, "Client live feed denied due to time based security restrictions", 0);
			yield return new LogEntryDefinition(14301, LogLevel.Info, LogType.AuditLog, "Recording sequence list retrieved. {0} instances from {1} to {2} (UTC time)", 3);
			yield return new LogEntryDefinition(4070, LogLevel.Info, LogType.AuditLog, "Playback of recorded sequence from '{0}' to '{1}' started.", 2);
			yield return new LogEntryDefinition(16001, LogLevel.Info, LogType.AuditLog, "Playing back recorded images '{0}' to '{1}' (UTC time).", 2);
			yield return new LogEntryDefinition(16004, LogLevel.Info, LogType.AuditLog, "The client was not allowed to play back video.", 0);
			yield return new LogEntryDefinition(16007, LogLevel.Info, LogType.AuditLog, "The client was not allowed to play back video (time based).", 0);
			yield return new LogEntryDefinition(16010, LogLevel.Info, LogType.AuditLog, "Retrieving recorded video in main track from '{0}' to '{1}' (UTC time).", 2);
			yield return new LogEntryDefinition(16017, LogLevel.Info, LogType.AuditLog, "Retrieving recorded video in edge track from '{0}' to '{1}' (UTC time).", 2);
			yield return new LogEntryDefinition(16002, LogLevel.Info, LogType.AuditLog, "Playing back recorded audio from microphones '{0}' to '{1}' (UTC time).", 2);
			yield return new LogEntryDefinition(16003, LogLevel.Info, LogType.AuditLog, "Playing back recorded audio from speakers '{0}' to '{1}' (UTC time).", 2);
			yield return new LogEntryDefinition(16005, LogLevel.Info, LogType.AuditLog, "The client was not allowed to play back audio.", 0);
			yield return new LogEntryDefinition(16006, LogLevel.Info, LogType.AuditLog, "The client was not allowed to play back audio.", 0);
			yield return new LogEntryDefinition(16008, LogLevel.Info, LogType.AuditLog, "The client was not allowed to play back audio (time based).", 0);
			yield return new LogEntryDefinition(16009, LogLevel.Info, LogType.AuditLog, "The client was not allowed to play back audio", 0);
			yield return new LogEntryDefinition(16011, LogLevel.Info, LogType.AuditLog, "Retrieving recorded audio in main track from '{0}' to '{1}' (UTC time).", 2);
			yield return new LogEntryDefinition(16012, LogLevel.Info, LogType.AuditLog, "Retrieving recorded audio in main track from '{0}' to '{1}' (UTC time).", 2);
			yield return new LogEntryDefinition(16018, LogLevel.Info, LogType.AuditLog, "Retrieving recorded audio in edge track from '{0}' to '{1}' (UTC time).", 2);
			yield return new LogEntryDefinition(16019, LogLevel.Info, LogType.AuditLog, "Retrieving recorded audio in edge track from '{0}' to '{1}' (UTC time).", 2);
			yield return new LogEntryDefinition(16013, LogLevel.Info, LogType.AuditLog, "Playing back recorded metadata '{0}' to '{1}' (UTC time).", 2);
			yield return new LogEntryDefinition(16014, LogLevel.Info, LogType.AuditLog, "Retrieving recorded metadata in main track from '{0}' to '{1}' (UTC time).", 2);
			yield return new LogEntryDefinition(16015, LogLevel.Info, LogType.AuditLog, "The client was not allowed to play back metadata.", 0);
			yield return new LogEntryDefinition(16016, LogLevel.Info, LogType.AuditLog, "The client was not allowed to play back metadata (time based).", 0);
			yield return new LogEntryDefinition(16020, LogLevel.Info, LogType.AuditLog, "Retrieving recorded metadata in edge track from '{0}' to '{1}' (UTC time).", 2);
			yield return new LogEntryDefinition(13001, LogLevel.Info, LogType.AuditLog, "PTZ preset position '{0}' activated", 1);
			yield return new LogEntryDefinition(13002, LogLevel.Info, LogType.AuditLog, "PTZ center activated. Width '{0}', Height '{1}', Center-X '{2}', Center-Y '{3}', Zoom '{4}'", 5);
			yield return new LogEntryDefinition(13003, LogLevel.Info, LogType.AuditLog, "PTZ center activated. Relative location", 0);
			yield return new LogEntryDefinition(13004, LogLevel.Info, LogType.AuditLog, "PTZ rectangle activated. Width '{0}', Height '{1}', Left '{2}', Top '{3}', Right '{4}', Bottom '{5}'.", 6);
			yield return new LogEntryDefinition(13005, LogLevel.Info, LogType.AuditLog, "PTZ move '{0}'", 1);
			yield return new LogEntryDefinition(13006, LogLevel.Info, LogType.AuditLog, "PTZ move '{0}'", 1);
			yield return new LogEntryDefinition(13007, LogLevel.Info, LogType.AuditLog, "PTZ move 'Home'", 0);
			yield return new LogEntryDefinition(13009, LogLevel.Info, LogType.AuditLog, "PTZ move 'Started'", 0);
			yield return new LogEntryDefinition(13010, LogLevel.Info, LogType.AuditLog, "PTZ move 'Stopped'", 0);
			yield return new LogEntryDefinition(13011, LogLevel.Info, LogType.AuditLog, "PTZ move absolute. Pan '{0}', tilt '{1}', zoom '{2}'.", 3);
			yield return new LogEntryDefinition(13012, LogLevel.Info, LogType.AuditLog, "Request for a PTZ session denied due to time based security restrictions.", 0);
			yield return new LogEntryDefinition(13013, LogLevel.Info, LogType.AuditLog, "Reserve PTZ session", 0);
			yield return new LogEntryDefinition(13014, LogLevel.Info, LogType.AuditLog, "Reserve PTZ session", 0);
			yield return new LogEntryDefinition(13015, LogLevel.Info, LogType.AuditLog, "PTZ session was released", 0);
			yield return new LogEntryDefinition(13016, LogLevel.Info, LogType.AuditLog, "PTZ session release was denied", 0);
			yield return new LogEntryDefinition(13017, LogLevel.Info, LogType.AuditLog, "PTZ patrolling profile '{0}' was started", 1);
			yield return new LogEntryDefinition(13018, LogLevel.Info, LogType.AuditLog, "Start of PTZ patrolling profile '{0}' was denied", 1);
			yield return new LogEntryDefinition(13020, LogLevel.Info, LogType.AuditLog, "New PTZ session was started by user {0}", 1);
			yield return new LogEntryDefinition(13021, LogLevel.Info, LogType.AuditLog, "New PTZ session was denied user {0}", 1);
			yield return new LogEntryDefinition(13022, LogLevel.Info, LogType.AuditLog, "PTZ preset position '{0}' was created", 1);
			yield return new LogEntryDefinition(13023, LogLevel.Info, LogType.AuditLog, "PTZ preset position '{0}' was changed", 1);
			yield return new LogEntryDefinition(13024, LogLevel.Info, LogType.AuditLog, "PTZ preset position '{0}' was removed", 1);
			yield return new LogEntryDefinition(13025, LogLevel.Info, LogType.AuditLog, "Patrolling profile '{0}' was created", 1);
			yield return new LogEntryDefinition(13026, LogLevel.Info, LogType.AuditLog, "Patrolling profile '{0}' was changed", 1);
			yield return new LogEntryDefinition(13027, LogLevel.Info, LogType.AuditLog, "Patrolling profile '{0}' was deleted", 1);
			yield return new LogEntryDefinition(14001, LogLevel.Info, LogType.AuditLog, "Output '{0}' activated", 1);
			yield return new LogEntryDefinition(14002, LogLevel.Info, LogType.AuditLog, "Activating output '{0}' denied due to time based security restrictions", 1);
			yield return new LogEntryDefinition(14101, LogLevel.Info, LogType.AuditLog, "User-defined event '{0}' activated", 1);
			yield return new LogEntryDefinition(14102, LogLevel.Info, LogType.AuditLog, "Activation user-defined event '{0}' denied due to time based security restrictions", 1);
			yield return new LogEntryDefinition(14401, LogLevel.Info, LogType.AuditLog, "Generic events activated. Data sources enabled: {0}", 1);
			yield return new LogEntryDefinition(14402, LogLevel.Info, LogType.AuditLog, "Generic events deactivated. All data sources are disabled.", 0);
			yield return new LogEntryDefinition(4075, LogLevel.Info, LogType.AuditLog, "The user-defined event '{0}' was triggered.", 1);
			yield return new LogEntryDefinition(15001, LogLevel.Info, LogType.AuditLog, "AVI export started. Sequence '{0}' to '{1}'", 2);
			yield return new LogEntryDefinition(15002, LogLevel.Info, LogType.AuditLog, "Database export started. No encryption. Sequence '{0}' to '{1}'", 2);
			yield return new LogEntryDefinition(15003, LogLevel.Info, LogType.AuditLog, "Database export started. With encryption. Sequence '{0}' to '{1}'", 2);
			yield return new LogEntryDefinition(15005, LogLevel.Info, LogType.AuditLog, "Export cancelled.", 0);
			yield return new LogEntryDefinition(15004, LogLevel.Info, LogType.AuditLog, "Export completed. Sequence '{0}' to '{1}'", 2);
			yield return new LogEntryDefinition(15011, LogLevel.Info, LogType.AuditLog, "AVI export started. Sequence '{0}' to '{1}'", 2);
			yield return new LogEntryDefinition(15012, LogLevel.Info, LogType.AuditLog, "Database export started. No encryption. Sequence '{0}' to '{1}'", 2);
			yield return new LogEntryDefinition(15013, LogLevel.Info, LogType.AuditLog, "Database export started. With encryption. Sequence '{0}' to '{1}'", 2);
			yield return new LogEntryDefinition(15015, LogLevel.Info, LogType.AuditLog, "Export cancelled.", 0);
			yield return new LogEntryDefinition(15014, LogLevel.Info, LogType.AuditLog, "Export completed. Sequence '{0}' to '{1}'", 2);
			yield return new LogEntryDefinition(15006, LogLevel.Info, LogType.AuditLog, "AVI export started. Sequence '{0}' to '{1}'", 2);
			yield return new LogEntryDefinition(15007, LogLevel.Info, LogType.AuditLog, "Database export started. No encryption. Sequence '{0}' to '{1}'", 2);
			yield return new LogEntryDefinition(15008, LogLevel.Info, LogType.AuditLog, "Database export started. With encryption. Sequence '{0}' to '{1}'", 2);
			yield return new LogEntryDefinition(15010, LogLevel.Info, LogType.AuditLog, "Export cancelled.", 0);
			yield return new LogEntryDefinition(15009, LogLevel.Info, LogType.AuditLog, "Export completed. Sequence '{0}' to '{1}'.", 2);
			yield return new LogEntryDefinition(15016, LogLevel.Info, LogType.AuditLog, "Database export started. No encryption. Sequence '{0}' to '{1}'", 2);
			yield return new LogEntryDefinition(15017, LogLevel.Info, LogType.AuditLog, "Database export started. With encryption. Sequence '{0}' to '{1}'", 2);
			yield return new LogEntryDefinition(15018, LogLevel.Info, LogType.AuditLog, "Export completed. Sequence '{0}' to '{1}'", 2);
			yield return new LogEntryDefinition(15019, LogLevel.Info, LogType.AuditLog, "Export cancelled.", 0);
			yield return new LogEntryDefinition(10207, LogLevel.Info, LogType.AuditLog, "Camera '{0}' removed from position '{1}'", 2);
			yield return new LogEntryDefinition(10105, LogLevel.Info, LogType.AuditLog, "Layout changed to '{0}'", 1);
			yield return new LogEntryDefinition(10107, LogLevel.Info, LogType.AuditLog, "Camera '{0}' placed at position '{1}'", 2);
			yield return new LogEntryDefinition(10109, LogLevel.Info, LogType.AuditLog, "Position '{0}' cleared", 1);
			yield return new LogEntryDefinition(10111, LogLevel.Info, LogType.AuditLog, "{0} preset '{1}' selected", 2);
			yield return new LogEntryDefinition(10113, LogLevel.Info, LogType.AuditLog, "Layout cleared", 0);
			yield return new LogEntryDefinition(10114, LogLevel.Info, LogType.AuditLog, "View '{0}' applied to layout", 1);
			yield return new LogEntryDefinition(10115, LogLevel.Info, LogType.AuditLog, "View '{0}' applied to layout", 1);
			yield return new LogEntryDefinition(10130, LogLevel.Info, LogType.AuditLog, "Text message ('{0}') displayed", 1);
			yield return new LogEntryDefinition(11101, LogLevel.Info, LogType.AuditLog, "Map '{0}' displayed", 1);
			yield return new LogEntryDefinition(17001, LogLevel.Info, LogType.AuditLog, "Bookmark '{0}' added. Headline: '{1}', clip start time: '{2}' (utc), bookmark time: '{3}' (utc), clip end time: '{4}' (utc).", 5);
			yield return new LogEntryDefinition(17002, LogLevel.Info, LogType.AuditLog, "Bookmark '{0}' changed. Headline: '{1}', clip start time: '{2}' (utc), bookmark time: '{3}' (utc), clip end time: '{4}' (utc).", 5);
			yield return new LogEntryDefinition(17003, LogLevel.Info, LogType.AuditLog, "Bookmark '{0}' deleted.", 1);
			yield return new LogEntryDefinition(17004, LogLevel.Info, LogType.AuditLog, "Bookmark(s) viewed: '{0}'.", 1);
			yield return new LogEntryDefinition(22016, LogLevel.Info, LogType.AuditLog, "Manual recording started.", 0);
			yield return new LogEntryDefinition(22017, LogLevel.Info, LogType.AuditLog, "Manual recording stopped.", 0);
			yield return new LogEntryDefinition(7001, LogLevel.Info, LogType.RuleLog, "{0}", 1);
			yield return new LogEntryDefinition(22030, LogLevel.Info, LogType.AuditLog, "Search '{0}' saved.", 1);
			yield return new LogEntryDefinition(22031, LogLevel.Info, LogType.AuditLog, "Search '{0}' edited.", 1);
			yield return new LogEntryDefinition(22032, LogLevel.Info, LogType.AuditLog, "Search '{0}' deleted.", 1);
			yield return new LogEntryDefinition(22033, LogLevel.Info, LogType.AuditLog, "Search '{0}' renamed to '{1}'.", 2);
			yield return new LogEntryDefinition(22034, LogLevel.Info, LogType.AuditLog, "Visibility of search category '{0}' enabled", 1);
			yield return new LogEntryDefinition(22035, LogLevel.Info, LogType.AuditLog, "Visibility of search category '{0}' disabled", 1);
			yield return new LogEntryDefinition(22036, LogLevel.Info, LogType.AuditLog, "Visibility of search filter '{0}' enabled", 1);
			yield return new LogEntryDefinition(22037, LogLevel.Info, LogType.AuditLog, "Visibility of search filter '{0}' disabled", 1);
			yield return new LogEntryDefinition(24000, LogLevel.Info, LogType.AuditLog, "Administrator login was rejected by the Identity Provider. Access denied.", 0);
			yield return new LogEntryDefinition(24001, LogLevel.Info, LogType.AuditLog, "Login by the user succeeded.", 0);
			yield return new LogEntryDefinition(22038, LogLevel.Info, LogType.AuditLog, "Login by the user failed. The password must be changed.", 0);
			yield return new LogEntryDefinition(22039, LogLevel.Info, LogType.AuditLog, "Login by the user failed. The password is invalid.", 0);
			yield return new LogEntryDefinition(22040, LogLevel.Info, LogType.AuditLog, "Basic user '{0}' has been locked out by the system due to too many failed login attempts.", 1);
			yield return new LogEntryDefinition(22041, LogLevel.Info, LogType.AuditLog, "Basic user '{0}' locked out time has been cleared.", 1);
			yield return new LogEntryDefinition(22042, LogLevel.Info, LogType.AuditLog, "Basic user '{0}' description has changed.", 1);
			yield return new LogEntryDefinition(22043, LogLevel.Info, LogType.AuditLog, "Basic user '{0}' created.", 1);
			yield return new LogEntryDefinition(22044, LogLevel.Info, LogType.AuditLog, "Basic user '{0}' deleted.", 1);
			yield return new LogEntryDefinition(22045, LogLevel.Info, LogType.AuditLog, "Basic user '{0}' must change password.", 1);
			yield return new LogEntryDefinition(22046, LogLevel.Info, LogType.AuditLog, "Basic user '{0}' is now enabled.", 1);
			yield return new LogEntryDefinition(22047, LogLevel.Info, LogType.AuditLog, "Basic user '{0}' is disabled.", 1);
			yield return new LogEntryDefinition(22048, LogLevel.Info, LogType.AuditLog, "Basic user '{0}' not forced to change password.", 1);
			yield return new LogEntryDefinition(22049, LogLevel.Info, LogType.AuditLog, "Login by the user failed. The user is locked out by the administrator.", 0);
			yield return new LogEntryDefinition(22053, LogLevel.Info, LogType.AuditLog, "Login by the user failed. The user is unknown.", 0);
			yield return new LogEntryDefinition(22054, LogLevel.Info, LogType.AuditLog, "Logout by the user succeeded.", 0);
			yield return new LogEntryDefinition(24002, LogLevel.Info, LogType.AuditLog, "Service registered successfully.", 0);
			yield return new LogEntryDefinition(24003, LogLevel.Info, LogType.AuditLog, "Service registration changed successfully.", 0);
			yield return new LogEntryDefinition(24004, LogLevel.Info, LogType.AuditLog, "The password for basic user '{0}' is changed.", 1);
		}
	}

	public static bool IsExtendedLog(LogEntryId logId)
	{
		switch (logId)
		{
		case LogEntryId.AuditClientBrowseSequence:
		case LogEntryId.AuditVideoWallMonitorLayoutChanged:
		case LogEntryId.AuditVideoWallMonitorLayoutCameraChanged:
		case LogEntryId.AuditVideoWallMonitorLayoutPositionCleared:
		case LogEntryId.AuditVideoWallMonitorLayoutPresetActivated:
		case LogEntryId.AuditVideoWallMonitorLayoutCleared:
		case LogEntryId.AuditVideoWallMonitorLayoutMonitorApplied:
		case LogEntryId.AuditVideoWallMonitorLayoutViewApplied:
		case LogEntryId.AuditVideoWallTextViewItemCreated:
		case LogEntryId.AuditVideoWallPresetActivate:
		case LogEntryId.AuditVideoWallMonitorCameraRemoved:
		case LogEntryId.AuditMapDisplayed:
		case LogEntryId.AuditLiveCameraStarted:
		case LogEntryId.AuditLiveCameraEnded:
		case LogEntryId.AuditLiveCameraDenied:
		case LogEntryId.AuditLiveMicrophoneStarted:
		case LogEntryId.AuditLiveMicrophoneEnded:
		case LogEntryId.AuditLiveMicrophoneDenied:
		case LogEntryId.AuditLiveSpeakToSpeakerStarted:
		case LogEntryId.AuditLiveSpeakToSpeakerEnded:
		case LogEntryId.AuditLiveSpeakToSpeakerDenied:
		case LogEntryId.AuditLiveSpeakerStarted:
		case LogEntryId.AuditLiveSpeakerEnded:
		case LogEntryId.AuditLiveSpeakerDenied:
		case LogEntryId.AuditLiveCameraDeniedTimeBased:
		case LogEntryId.AuditLiveMicrophoneDeniedTimeBased:
		case LogEntryId.AuditLiveSpeakToSpeakerDeniedTimeBased:
		case LogEntryId.AuditLiveSpeakerDeniedTimeBased:
		case LogEntryId.AuditLiveMetadataStarted:
		case LogEntryId.AuditLiveMetadataEnded:
		case LogEntryId.AuditLiveMetadataDenied:
		case LogEntryId.AuditLiveMetadataDeniedTimeBased:
		case LogEntryId.AuditPtzMoveToPresetPosition:
		case LogEntryId.AuditPtzMoveCenterZoomAbsolutePositioned:
		case LogEntryId.AuditPtzMoveCenterZoomRelativePositioned:
		case LogEntryId.AuditPtzRectangleZoom:
		case LogEntryId.AuditPtzMoveZoomButtonsActivated:
		case LogEntryId.AuditPtzMoveRelative:
		case LogEntryId.AuditPtzMoveHome:
		case LogEntryId.AuditPtzMoveStart:
		case LogEntryId.AuditPtzMoveStop:
		case LogEntryId.AuditPtzMoveAbsolute:
		case LogEntryId.AuditOutputActivated:
		case LogEntryId.AuditUserDefinedEventActivated:
		case LogEntryId.AuditRecordingSequencesRequested:
		case LogEntryId.AuditExportCameraToAvi:
		case LogEntryId.AuditExportCameraToDBNoEncryption:
		case LogEntryId.AuditExportCameraToDBWithEncryption:
		case LogEntryId.AuditExportCameraCompleted:
		case LogEntryId.AuditExportCameraCancelled:
		case LogEntryId.AuditExportSpeakerToAvi:
		case LogEntryId.AuditExportSpeakerToDBNoEncryption:
		case LogEntryId.AuditExportSpeakerToDBWithEncryption:
		case LogEntryId.AuditExportSpeakerCompleted:
		case LogEntryId.AuditExportSpeakerCancelled:
		case LogEntryId.AuditExportMicrophoneToAvi:
		case LogEntryId.AuditExportMicrophoneToDBNoEncryption:
		case LogEntryId.AuditExportMicrophoneToDBWithEncryption:
		case LogEntryId.AuditExportMicrophoneCompleted:
		case LogEntryId.AuditExportMicrophoneCancelled:
		case LogEntryId.AuditExportMetadataToDBNoEncryption:
		case LogEntryId.AuditExportMetadataToDBWithEncryption:
		case LogEntryId.AuditExportMetadataCompleted:
		case LogEntryId.AuditExportMetadataCancelled:
		case LogEntryId.AuditPlaybackCameraRecordings:
		case LogEntryId.AuditPlaybackMicrophoneRecordings:
		case LogEntryId.AuditPlaybackSpeakerRecordings:
		case LogEntryId.AuditPlaybackCameraRecordingsDenied:
		case LogEntryId.AuditPlaybackMicrophoneRecordingsDenied:
		case LogEntryId.AuditPlaybackSpeakerRecordingsDenied:
		case LogEntryId.AuditPlaybackCameraRecordingsDeniedTimeBased:
		case LogEntryId.AuditPlaybackMicrophoneRecordingsDeniedTimeBased:
		case LogEntryId.AuditPlaybackSpeakerRecordingsDeniedTimeBased:
		case LogEntryId.AuditPlaybackCameraRecordingsOneLogOnly:
		case LogEntryId.AuditPlaybackMicrophoneRecordingsOneLogOnly:
		case LogEntryId.AuditPlaybackSpeakerRecordingsOneLogOnly:
		case LogEntryId.AuditPlaybackMetadataRecordings:
		case LogEntryId.AuditPlaybackMetadataRecordingsOneLogOnly:
		case LogEntryId.AuditPlaybackMetadataRecordingsDenied:
		case LogEntryId.AuditPlaybackMetadataRecordingsDeniedTimeBased:
		case LogEntryId.AuditPlaybackCameraEdgeRecordingsOneLogOnly:
		case LogEntryId.AuditPlaybackMicrophoneEdgeRecordingsOneLogOnly:
		case LogEntryId.AuditPlaybackSpeakerEdgeRecordingsOneLogOnly:
		case LogEntryId.AuditPlaybackMetadataEdgeRecordingsOneLogOnly:
		case LogEntryId.AuditBookmarkCreated:
		case LogEntryId.AuditBookmarkEdited:
		case LogEntryId.AuditBookmarkDeleted:
		case LogEntryId.AuditBookmarkViewed:
		case LogEntryId.AuditManualRecordingStarted:
		case LogEntryId.AuditManualRecordingStopped:
		case LogEntryId.AuditSearchCreated:
		case LogEntryId.AuditSearchEdited:
		case LogEntryId.AuditSearchDeleted:
		case LogEntryId.AuditSearchRenamed:
			return true;
		default:
			return false;
		}
	}
}
