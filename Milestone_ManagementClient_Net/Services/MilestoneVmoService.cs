using System;
using System.Collections.Generic;
using System.Linq;
using MilestoneManagementClient.Models;

namespace MilestoneManagementClient.Services
{
    public class MilestoneVmoService
    {
        private readonly Dictionary<string, CameraModel> _cameras = new Dictionary<string, CameraModel>();
        private readonly Dictionary<string, CameraModel> _savedConfigurations = new Dictionary<string, CameraModel>();
        private readonly List<RecordingServerModel> _servers = new List<RecordingServerModel>();
        private readonly LicenseInfoModel _license = new LicenseInfoModel();
        private readonly List<RuleModel> _rules = new List<RuleModel>();
        private readonly List<UserModel> _users = new List<UserModel>();
        private readonly List<RoleModel> _roles = new List<RoleModel>();
        private readonly List<TaskModel> _tasks = new List<TaskModel>();
        private readonly List<LogEntryModel> _logs = new List<LogEntryModel>();
        private readonly SqlDatabaseService _sqlDb = new SqlDatabaseService();

        private static readonly string DbFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "surveillance_database.json");

        public MilestoneVmoService()
        {
            if (!LoadFromDatabase())
            {
                InitializeDefaultMilestoneInventory();
                InitializeServers();
                InitializeRules();
                InitializeSecurity();
                InitializeTasks();
                InitializeLogs();
                SaveToDatabase();
            }
        }

        // --- Camera & Group Management ---
        public Dictionary<string, CameraModel> GetAllCameras() { return _cameras; }

        public CameraModel GetCamera(string name)
        {
            CameraModel cam;
            if (_cameras.TryGetValue(name, out cam)) return cam;
            return null;
        }

        public void SaveCameraSettings(CameraModel camera)
        {
            if (camera != null && _cameras.ContainsKey(camera.Name))
            {
                _cameras[camera.Name] = camera;
                _savedConfigurations[camera.Name] = camera.Clone();
                AddLog("INFO", "Configuration", "Updated camera properties for '" + camera.Name + "'", "Administrator");
                SaveToDatabase();
            }
        }

        public void ResetCameraSettings(CameraModel camera)
        {
            if (camera != null)
            {
                CameraModel saved;
                if (_savedConfigurations.TryGetValue(camera.Name, out saved))
                {
                    _cameras[camera.Name] = saved.Clone();
                    SaveToDatabase();
                }
            }
        }

        public void RenameCamera(string oldName, string newName)
        {
            CameraModel cam;
            if (_cameras.TryGetValue(oldName, out cam))
            {
                _cameras.Remove(oldName);
                cam.Name = newName;
                _cameras[newName] = cam;

                CameraModel saved;
                if (_savedConfigurations.TryGetValue(oldName, out saved))
                {
                    _savedConfigurations.Remove(oldName);
                    saved.Name = newName;
                    _savedConfigurations[newName] = saved;
                }
                AddLog("INFO", "Devices", "Renamed camera from '" + oldName + "' to '" + newName + "'", "Administrator");
                SaveToDatabase();
            }
        }

        public void RenameGroup(string oldGroup, string newGroup)
        {
            foreach (var cam in _cameras.Values)
            {
                if (cam.Group == oldGroup)
                {
                    cam.Group = newGroup;
                }
            }
            AddLog("INFO", "Devices", "Renamed group from '" + oldGroup + "' to '" + newGroup + "'", "Administrator");
            SaveToDatabase();
        }

        public void DeleteCamera(string name)
        {
            _cameras.Remove(name);
            _savedConfigurations.Remove(name);
            _license.HardwareDeviceLicensesUsed = _cameras.Count;
            AddLog("WARN", "Devices", "Deleted camera '" + name + "'", "Administrator");
            _sqlDb.DeleteCameraFromSql(name);
            SaveToDatabase();
        }

        public void DeleteGroup(string groupName)
        {
            var toDelete = _cameras.Values.Where(c => c.Group == groupName).Select(c => c.Name).ToList();
            foreach (var name in toDelete)
            {
                DeleteCamera(name);
            }
            AddLog("WARN", "Devices", "Deleted device group '" + groupName + "'", "Administrator");
            SaveToDatabase();
        }

        public void AddDeviceGroup(string groupName)
        {
            var c1 = new CameraModel { Name = groupName + " CAM 01", Group = groupName };
            var c2 = new CameraModel { Name = groupName + " CAM 02", Group = groupName };
            _cameras[c1.Name] = c1;
            _cameras[c2.Name] = c2;
            _savedConfigurations[c1.Name] = c1.Clone();
            _savedConfigurations[c2.Name] = c2.Clone();
            _license.HardwareDeviceLicensesUsed = _cameras.Count;
            AddLog("INFO", "Devices", "Created device group '" + groupName + "' with 2 cameras", "Administrator");
            SaveToDatabase();
        }

        public void AddCameraToGroup(string camName, string groupName, string resolution)
        {
            var cam = new CameraModel { Name = camName, Group = groupName, Resolution = resolution };
            _cameras[camName] = cam;
            _savedConfigurations[camName] = cam.Clone();
            _license.HardwareDeviceLicensesUsed = _cameras.Count;
            AddLog("INFO", "Hardware Wizard", "Added new camera '" + camName + "' to group '" + groupName + "'", "Administrator");
            SaveToDatabase();
        }

        public void AddHardwareWithDetails(string camName, string groupName, string manufacturer, string model, string driverName, string driverVersion, string ip, int port, string resolution)
        {
            var cam = new CameraModel
            {
                Name = camName,
                Group = string.IsNullOrWhiteSpace(groupName) ? manufacturer : groupName,
                ServerName = groupName,
                Manufacturer = manufacturer,
                Model = model,
                DriverName = driverName,
                DriverVersion = driverVersion,
                IpAddress = ip,
                MulticastIp = ip,
                HttpPort = port,
                Port1 = port,
                Resolution = resolution,
                Codec = "H.265+",
                Fps = 30,
                IsOnline = true
            };
            _cameras[camName] = cam;
            _savedConfigurations[camName] = cam.Clone();
            _license.HardwareDeviceLicensesUsed = _cameras.Count;
            AddLog("INFO", "Hardware Wizard", "Registered " + manufacturer + " device '" + camName + "' [" + model + "] with driver '" + driverName + "' (" + driverVersion + ")", "Administrator");
            SaveToDatabase();
        }

        public void AddCamera(CameraModel cam)
        {
            if (cam == null || string.IsNullOrWhiteSpace(cam.Name)) return;
            _cameras[cam.Name] = cam;
            _savedConfigurations[cam.Name] = cam.Clone();
            _license.HardwareDeviceLicensesUsed = _cameras.Count;
            AddLog("INFO", "Hardware", "Registered camera '" + cam.Name + "' (" + (cam.Manufacturer ?? "Generic") + ")", "Administrator");
            SaveToDatabase();
        }

        // --- Servers Management ---
        public List<RecordingServerModel> GetServers() { return _servers; }

        public RecordingServerModel GetServer(string name)
        {
            return _servers.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public void RenameRecordingServer(string oldName, string newName)
        {
            var srv = _servers.FirstOrDefault(s => s.Name.Equals(oldName, StringComparison.OrdinalIgnoreCase));
            if (srv != null && !string.IsNullOrWhiteSpace(newName))
            {
                srv.Name = newName.Trim();
                AddLog("INFO", "Servers", "Renamed recording server from '" + oldName + "' to '" + newName + "'", "Administrator");
                SaveToDatabase();
            }
        }

        public void AddRecordingServer(string name, string host, int port)
        {
            var srv = new RecordingServerModel
            {
                Name = name,
                Hostname = host,
                Port = port,
                DeviceCount = 0,
                Status = "Running (Active)",
                CpuUsage = 15,
                RamUsage = 30,
                PrimaryStoragePath = @"D:\Milestone\MediaDatabase",
                StorageUsedGb = 500,
                StorageTotalGb = 8000
            };
            _servers.Add(srv);
            AddLog("INFO", "Servers", "Added new recording server '" + name + "'", "Administrator");
            SaveToDatabase();
        }

        public void DeleteRecordingServer(string name, bool deleteAttachedCameras = true)
        {
            var srv = _servers.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (srv != null)
            {
                _servers.Remove(srv);
                if (deleteAttachedCameras)
                {
                    var attachedCams = _cameras.Values.Where(c => !string.IsNullOrEmpty(c.ServerName) && c.ServerName.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();
                    foreach (var c in attachedCams)
                    {
                        _cameras.Remove(c.Name);
                        _savedConfigurations.Remove(c.Name);
                    }
                    _license.HardwareDeviceLicensesUsed = _cameras.Count;
                }
                AddLog("WARN", "Servers", "Deleted recording server '" + name + "'", "Administrator");
                SaveToDatabase();
            }
        }

        // --- License Info ---
        public LicenseInfoModel GetLicense() { return _license; }

        // --- Rules Management ---
        public List<RuleModel> GetRules() { return _rules; }

        public void AddRule(string name, string trigger, string target, string timeProfile, string action, bool enabled, string description = "", string logEntry = "$DeviceName$$Event Name$$TriggerTime$$RuleName$$RecorderName$", bool isDefault = false)
        {
            _rules.Add(new RuleModel(name, trigger, target, timeProfile, action, enabled, description, logEntry, isDefault));
            AddLog("INFO", "Rules", "Created new rule: '" + name + "'", "Administrator");
        }

        public void UpdateRule(string ruleId, string name, string description, bool isActive, string trigger, string target, string logEntry)
        {
            var r = _rules.FirstOrDefault(x => x.Id == ruleId);
            if (r != null)
            {
                r.Name = name;
                r.Description = description;
                r.IsEnabled = isActive;
                if (!string.IsNullOrEmpty(trigger)) r.EventTrigger = trigger;
                if (!string.IsNullOrEmpty(target)) r.TargetSource = target;
                if (!string.IsNullOrEmpty(logEntry)) r.LogEntry = logEntry;
                AddLog("INFO", "Rules", "Updated rule configuration: '" + name + "'", "Administrator");
            }
        }

        public void DuplicateRule(string ruleId)
        {
            var r = _rules.FirstOrDefault(x => x.Id == ruleId);
            if (r != null)
            {
                var copy = new RuleModel(r.Name + " (Copy)", r.EventTrigger, r.TargetSource, r.TimeProfile, r.Action, r.IsEnabled, r.Description, r.LogEntry, false);
                _rules.Add(copy);
                AddLog("INFO", "Rules", "Duplicated rule: '" + r.Name + "' -> '" + copy.Name + "'", "Administrator");
            }
        }

        public void ToggleRule(string ruleId)
        {
            var r = _rules.FirstOrDefault(x => x.Id == ruleId);
            if (r != null)
            {
                r.IsEnabled = !r.IsEnabled;
                AddLog("INFO", "Rules", "Rule '" + r.Name + "' state changed to " + (r.IsEnabled ? "Enabled" : "Disabled"), "Administrator");
            }
        }

        public void DeleteRule(string ruleId)
        {
            var r = _rules.FirstOrDefault(x => x.Id == ruleId);
            if (r != null)
            {
                _rules.Remove(r);
                AddLog("WARN", "Rules", "Deleted rule: '" + r.Name + "'", "Administrator");
            }
        }

        // --- Security Management ---
        public List<UserModel> GetUsers() { return _users; }
        public List<RoleModel> GetRoles() { return _roles; }

        public void AddUser(string username, string displayName, string role, string authType)
        {
            _users.Add(new UserModel(username, displayName, role, authType));
            AddLog("INFO", "Security", "Created user '" + username + "' (" + role + ")", "Administrator");
        }

        public void UpdateUser(string username, string displayName, string role, bool isEnabled)
        {
            var u = _users.FirstOrDefault(x => x.Username == username);
            if (u != null)
            {
                u.DisplayName = displayName;
                u.Role = role;
                u.IsEnabled = isEnabled;
                AddLog("INFO", "Security", "Updated user '" + username + "' profile", "Administrator");
            }
        }

        public void ChangeUserPassword(string username, string newPassword)
        {
            AddLog("INFO", "Security", "Password updated for user '" + username + "'", "Administrator");
        }

        public void DeleteUser(string username)
        {
            var u = _users.FirstOrDefault(x => x.Username == username);
            if (u != null)
            {
                _users.Remove(u);
                AddLog("WARN", "Security", "Removed user '" + username + "'", "Administrator");
            }
        }

        public void AddRole(string roleName, string desc, string[] perms)
        {
            _roles.Add(new RoleModel(roleName, desc, perms));
            AddLog("INFO", "Security", "Created security role: '" + roleName + "'", "Administrator");
        }

        public void UpdateRole(string oldName, string newName, string desc, string[] perms)
        {
            var r = _roles.FirstOrDefault(x => x.RoleName == oldName);
            if (r != null)
            {
                r.RoleName = newName;
                r.Description = desc;
                r.Permissions = new List<string>(perms);
                AddLog("INFO", "Security", "Updated security role: '" + newName + "'", "Administrator");
            }
        }

        public void DuplicateRole(string roleName)
        {
            var r = _roles.FirstOrDefault(x => x.RoleName == roleName);
            if (r != null)
            {
                var copy = new RoleModel(r.RoleName + " (Copy)", r.Description, r.Permissions.ToArray());
                _roles.Add(copy);
                AddLog("INFO", "Security", "Duplicated security role: '" + roleName + "' -> '" + copy.RoleName + "'", "Administrator");
            }
        }

        public void DeleteRole(string roleName)
        {
            var r = _roles.FirstOrDefault(x => x.RoleName == roleName);
            if (r != null)
            {
                _roles.Remove(r);
                AddLog("WARN", "Security", "Deleted security role: '" + roleName + "'", "Administrator");
            }
        }

        // --- Tasks & Monitoring ---
        public List<TaskModel> GetTasks() { return _tasks; }

        // --- Server Logs ---
        public List<LogEntryModel> GetLogs() { return _logs; }

        public void AddLog(string level, string source, string message, string user)
        {
            _logs.Insert(0, new LogEntryModel(level, source, message, user));
            if (_logs.Count > 500) _logs.RemoveAt(_logs.Count - 1);
        }

        // --- Initializers ---
        private void InitializeDefaultMilestoneInventory()
        {
            // Clean slate: No hardcoded dummy cameras.
            // Cameras will only be added via Auto-Detect Scan or Add Camera dialog.
            _cameras.Clear();
            _savedConfigurations.Clear();
            _license.HardwareDeviceLicensesUsed = 0;
        }

        private void InitializeServers()
        {
            _servers.Add(new RecordingServerModel
            {
                Name = "JSPANGICCCREC01.JSPLICCC.LOCAL",
                Hostname = "127.0.0.1",
                Port = 7563,
                Status = "Running (Active)",
                DeviceCount = 0,
                CpuUsage = 12,
                RamUsage = 25,
                PrimaryStoragePath = @"D:\Milestone\MediaDatabase\Live01",
                StorageUsedGb = 0,
                StorageTotalGb = 10000,
                ArchiveStoragePath = @"E:\Milestone\Archive01",
                ArchiveUsedGb = 0,
                ArchiveTotalGb = 40000,
                IsFailoverConfigured = false,
                FailoverServerName = "None"
            });
        }

        private void InitializeRules()
        {
            _rules.Add(new RuleModel("24x7 Recording", "Always", "All Cameras", "Always", "Perform continuous recording on all cameras at 25 fps", true, "Standard continuous recording profile across all cluster devices.", "$DeviceName$$Event Name$$TriggerTime$$RuleName$$RecorderName$"));
            _rules.Add(new RuleModel("Analitics Triger by Intrusion", "Intrusion Started", "SECTOR-1 Cameras", "Night Shift (20:00 - 06:00)", "Start recording & trigger red perimeter strobe", true, "Perimeter security intrusion detection alert rule.", "$DeviceName$$Event Name$$TriggerTime$$RuleName$$RecorderName$"));
            _rules.Add(new RuleModel("Analitics Triger by Motion detection", "Motion Started (HW)", "Camera Group 1", "Always", "Perform an action on Motion Started (HW) from Camera Group 1. Create log entry.", true, "Primary motion detection trigger rule for Camera Group 1.", "$DeviceName$$Event Name$$TriggerTime$$RuleName$$RecorderName$"));
            _rules.Add(new RuleModel("Analitics trigered Tripwire", "Tripwire Crossed", "Boundary Perimeter Cameras", "Always", "Bookmark event & PTZ Preset 01", true, "Virtual tripwire crossing trigger.", "$DeviceName$$Event Name$$TriggerTime$$RuleName$$RecorderName$"));
            _rules.Add(new RuleModel("Analities Triger by crowd dedect start", "Crowd Density High", "VISITOR LOBBY & MAIN GATE", "Always", "Send notification to control room & trigger log entry", true, "Crowd density start threshold trigger rule.", "$DeviceName$$Event Name$$TriggerTime$$RuleName$$RecorderName$"));
            _rules.Add(new RuleModel("Analities Triger by crowd dedect start rising", "Crowd Density Rising", "VISITOR LOBBY & MAIN GATE", "Always", "Increase stream framerate to 30fps", true, "Crowd gathering warning rule.", "$DeviceName$$Event Name$$TriggerTime$$RuleName$$RecorderName$"));
            _rules.Add(new RuleModel("Analities Triger by Line crossing", "Line Crossing Event", "PARKING NORTH & ENTRY ROAD", "Always", "Create log entry & PTZ focus tracking", true, "Directional line crossing analytics trigger.", "$DeviceName$$Event Name$$TriggerTime$$RuleName$$RecorderName$"));
            _rules.Add(new RuleModel("Default Goto Preset when PTZ is done Rule", "PTZ Inactivity Timeout", "All PTZ Cameras", "Always", "Move PTZ Camera back to Default Guard Tour Preset 01", true, "Default system rule to return PTZ cameras after manual operator control.", "$DeviceName$$Event Name$$TriggerTime$$RuleName$$RecorderName$", true));
            _rules.Add(new RuleModel("Default Play Audio on Request Rule", "Audio Request Triggered", "All Speakers", "Always", "Play pre-recorded audio advisory message on requested speaker", true, "Public address audio playback rule.", "$DeviceName$$Event Name$$TriggerTime$$RuleName$$RecorderName$", true));
            _rules.Add(new RuleModel("Default Record on Bookmark Rule", "Bookmark Created", "All Cameras", "Always", "Lock footage 15 minutes before and after bookmark", true, "System evidence preservation rule for bookmarked incidents.", "$DeviceName$$Event Name$$TriggerTime$$RuleName$$RecorderName$", true));
            _rules.Add(new RuleModel("Default Record on Motion Rule", "Motion Started (HW)", "All Motion Enabled Cameras", "Always", "Start recording on related device with 5 seconds pre-buffer", true, "Standard hardware motion based recording trigger.", "$DeviceName$$Event Name$$TriggerTime$$RuleName$$RecorderName$", true));
            _rules.Add(new RuleModel("Default Record on Request Rule", "Manual Record Requested", "All Cameras", "Always", "Trigger manual priority recording feed", true, "Operator manual recording rule.", "$DeviceName$$Event Name$$TriggerTime$$RuleName$$RecorderName$", true));
            _rules.Add(new RuleModel("Default Start Audio Feed Rule", "Audio Streaming Started", "All Microphones", "Always", "Start audio feed decoding", true, "System default audio feed automation.", "$DeviceName$$Event Name$$TriggerTime$$RuleName$$RecorderName$", true));
            _rules.Add(new RuleModel("Default Start Feed Rule", "Live View Requested", "All Cameras", "Always", "Start live video feed", true, "System default live stream activation rule.", "$DeviceName$$Event Name$$TriggerTime$$RuleName$$RecorderName$", true));
            _rules.Add(new RuleModel("Default Start Metadata Feed Rule", "Metadata Feed Requested", "All Analytics Devices", "Always", "Start AI metadata bounding box feed", true, "System default metadata stream feed rule.", "$DeviceName$$Event Name$$TriggerTime$$RuleName$$RecorderName$", true));
            _rules.Add(new RuleModel("Test Analytics rule", "Test Event Triggered", "Camera Group 1", "Always", "Write simulation test entry to server logs", true, "Diagnostics and rule engine test rule.", "$DeviceName$$Event Name$$TriggerTime$$RuleName$$RecorderName$"));
        }

        private void InitializeSecurity()
        {
            _roles.Add(new RoleModel("Administrators", "Full administrative access to all servers, cameras, licenses, and security profiles", new[] { "Full Control", "Manage Servers", "Manage Cameras", "Live & Playback", "Export Video", "Manage Users" }));
            _roles.Add(new RoleModel("Operators", "Live viewing, PTZ control, audio listen, manual recording trigger", new[] { "Live & Playback", "PTZ Control", "Audio Listen", "Export Video" }));
            _roles.Add(new RoleModel("Auditors / View Only", "Read-only access to live streams and audit logs", new[] { "Live View Only", "View Audit Logs" }));

            _users.Add(new UserModel("admin", "System Administrator", "Administrators", "Basic User"));
            _users.Add(new UserModel("operator01", "Control Room Operator 1", "Operators", "Basic User"));
            _users.Add(new UserModel("operator02", "Control Room Operator 2", "Operators", "Basic User"));
            _users.Add(new UserModel("guard_patrol", "Perimeter Guard Patrol", "Operators", "Basic User"));
            _users.Add(new UserModel("auditor_sec", "Chief Security Auditor", "Auditors / View Only", "Windows User"));
            _users.Add(new UserModel("mines_supervisor", "Utkal Mines Supervisor", "Operators", "Basic User"));
        }

        private void InitializeTasks()
        {
            _tasks.Add(new TaskModel("Media Storage Archiving Cycle", "REC-SRV-01", 78, "Running"));
            _tasks.Add(new TaskModel("SQL Database Index Optimization", "MXM-PS-01", 100, "Completed"));
            _tasks.Add(new TaskModel("Camera Device Pack Firmware Audit", "REC-SRV-02", 42, "Running"));
            _tasks.Add(new TaskModel("System Configuration Backup (.xml)", "MXM-PS-01", 100, "Completed"));
            _tasks.Add(new TaskModel("H.265 Transcoding Cache Cleanup", "REC-SRV-04", 15, "Running"));
        }

        private void InitializeLogs()
        {
            AddLog("INFO", "System", "Milestone XProtect Management Server started successfully", "SYSTEM");
            AddLog("INFO", "License", "Enterprise License verified (128 Channels Activated)", "SYSTEM");
            AddLog("INFO", "Recording Server", "REC-SRV-01 connected (14 cameras streaming)", "SYSTEM");
            AddLog("INFO", "Recording Server", "REC-SRV-02 connected (8 cameras streaming)", "SYSTEM");
            AddLog("INFO", "Recording Server", "REC-SRV-03 connected (4 cameras streaming)", "SYSTEM");
            AddLog("INFO", "Recording Server", "REC-SRV-04 connected (AI Edge Processor online)", "SYSTEM");
            AddLog("INFO", "Security", "Administrator session started from 127.0.0.1", "Administrator");
            AddLog("INFO", "Device Pack", "Milestone Device Pack 13.4a drivers loaded", "SYSTEM");
        }

        // --- Database Persistence Layer ---
        public bool LoadFromDatabase()
        {
            try
            {
                if (!System.IO.File.Exists(DbFilePath)) return false;
                string json = System.IO.File.ReadAllText(DbFilePath);
                var data = SimpleJsonSerializer.DeserializeDb(json);
                if (data == null) return false;

                _cameras.Clear();
                _savedConfigurations.Clear();
                foreach (var cam in data.Cameras)
                {
                    _cameras[cam.Name] = cam;
                    _savedConfigurations[cam.Name] = cam.Clone();
                }

                _servers.Clear();
                _servers.AddRange(data.Servers);

                _rules.Clear();
                _rules.AddRange(data.Rules);

                _users.Clear();
                _users.AddRange(data.Users);

                _roles.Clear();
                _roles.AddRange(data.Roles);

                _tasks.Clear();
                _tasks.AddRange(data.Tasks);

                _logs.Clear();
                _logs.AddRange(data.Logs);

                return _cameras.Count > 0;
            }
            catch
            {
                return false;
            }
        }

        public void SaveToDatabase()
        {
            try
            {
                var data = new DatabaseSnapshot
                {
                    Cameras = _cameras.Values.ToList(),
                    Servers = _servers,
                    Rules = _rules,
                    Users = _users,
                    Roles = _roles,
                    Tasks = _tasks,
                    Logs = _logs
                };

                string json = SimpleJsonSerializer.SerializeDb(data);
                System.IO.File.WriteAllText(DbFilePath, json);

                // Direct MSSQL Synchronizer
                _sqlDb.SyncAllToMssql(_cameras.Values.ToList(), _servers, _rules);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Database Save Error: " + ex.Message);
            }
        }
    }

    public class DatabaseSnapshot
    {
        public List<CameraModel> Cameras { get; set; }
        public List<RecordingServerModel> Servers { get; set; }
        public List<RuleModel> Rules { get; set; }
        public List<UserModel> Users { get; set; }
        public List<RoleModel> Roles { get; set; }
        public List<TaskModel> Tasks { get; set; }
        public List<LogEntryModel> Logs { get; set; }
    }

    public static class SimpleJsonSerializer
    {
        public static string SerializeDb(DatabaseSnapshot snap)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("{");
            
            // 1. Cameras
            sb.AppendLine("  \"Cameras\": [");
            for (int i = 0; i < snap.Cameras.Count; i++)
            {
                var c = snap.Cameras[i];
                sb.AppendLine("    {");
                sb.AppendLine("      \"Name\": \"" + Escape(c.Name) + "\",");
                sb.AppendLine("      \"Group\": \"" + Escape(c.Group) + "\",");
                sb.AppendLine("      \"ServerName\": \"" + Escape(c.ServerName) + "\",");
                sb.AppendLine("      \"IpAddress\": \"" + Escape(c.IpAddress) + "\",");
                sb.AppendLine("      \"UserName\": \"" + Escape(c.UserName) + "\",");
                sb.AppendLine("      \"Password\": \"" + Escape(c.Password) + "\",");
                sb.AppendLine("      \"HttpPort\": " + c.HttpPort + ",");
                sb.AppendLine("      \"RtspPort\": " + c.RtspPort + ",");
                sb.AppendLine("      \"Manufacturer\": \"" + Escape(c.Manufacturer) + "\",");
                sb.AppendLine("      \"Model\": \"" + Escape(c.Model) + "\",");
                sb.AppendLine("      \"DriverName\": \"" + Escape(c.DriverName) + "\",");
                sb.AppendLine("      \"DriverVersion\": \"" + Escape(c.DriverVersion) + "\",");
                sb.AppendLine("      \"MacAddress\": \"" + Escape(c.MacAddress) + "\",");
                sb.AppendLine("      \"FirmwareVersion\": \"" + Escape(c.FirmwareVersion) + "\",");
                sb.AppendLine("      \"Resolution\": \"" + Escape(c.Resolution) + "\",");
                sb.AppendLine("      \"Fps\": " + c.Fps + ",");
                sb.AppendLine("      \"Codec\": \"" + Escape(c.Codec) + "\",");
                sb.AppendLine("      \"RetentionDays\": " + c.RetentionDays + ",");
                sb.AppendLine("      \"StoragePath\": \"" + Escape(c.StoragePath) + "\",");
                sb.AppendLine("      \"PrebufferSeconds\": " + c.PrebufferSeconds + ",");
                sb.AppendLine("      \"PrebufferType\": \"" + Escape(c.PrebufferType) + "\",");
                sb.AppendLine("      \"RecordFps\": " + c.RecordFps + ",");
                sb.AppendLine("      \"RecordKeyframesOnly\": " + (c.RecordKeyframesOnly ? "true" : "false") + ",");
                sb.AppendLine("      \"MotionDetectionEnabled\": " + (c.MotionDetectionEnabled ? "true" : "false") + ",");
                sb.AppendLine("      \"MotionSensitivity\": " + c.MotionSensitivity + ",");
                sb.AppendLine("      \"MotionThreshold\": " + c.MotionThreshold + ",");
                sb.AppendLine("      \"MotionHardwareAcceleration\": \"" + Escape(c.MotionHardwareAcceleration) + "\",");
                sb.AppendLine("      \"IsOnline\": " + (c.IsOnline ? "true" : "false") + ",");
                sb.AppendLine("      \"MulticastIp\": \"" + Escape(c.MulticastIp) + "\"");
                sb.Append("    }");
                if (i < snap.Cameras.Count - 1) sb.Append(",");
                sb.AppendLine();
            }
            sb.AppendLine("  ],");

            // 2. Recording Servers
            sb.AppendLine("  \"Servers\": [");
            for (int i = 0; i < snap.Servers.Count; i++)
            {
                var s = snap.Servers[i];
                sb.AppendLine("    {");
                sb.AppendLine("      \"Name\": \"" + Escape(s.Name) + "\",");
                sb.AppendLine("      \"Hostname\": \"" + Escape(s.Hostname) + "\",");
                sb.AppendLine("      \"Port\": " + s.Port + ",");
                sb.AppendLine("      \"Status\": \"" + Escape(s.Status) + "\",");
                sb.AppendLine("      \"DeviceCount\": " + s.DeviceCount + ",");
                sb.AppendLine("      \"PrimaryStoragePath\": \"" + Escape(s.PrimaryStoragePath) + "\",");
                sb.AppendLine("      \"FailoverServerName\": \"" + Escape(s.FailoverServerName) + "\"");
                sb.Append("    }");
                if (i < snap.Servers.Count - 1) sb.Append(",");
                sb.AppendLine();
            }
            sb.AppendLine("  ]");

            sb.AppendLine("}");
            return sb.ToString();
        }

        public static DatabaseSnapshot DeserializeDb(string json)
        {
            var snap = new DatabaseSnapshot
            {
                Cameras = new List<CameraModel>(),
                Servers = new List<RecordingServerModel>(),
                Rules = new List<RuleModel>(),
                Users = new List<UserModel>(),
                Roles = new List<RoleModel>(),
                Tasks = new List<TaskModel>(),
                Logs = new List<LogEntryModel>()
            };

            if (string.IsNullOrWhiteSpace(json)) return snap;

            try
            {
                string[] lines = json.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                CameraModel curCam = null;
                RecordingServerModel curSrv = null;
                string currentSection = "";

                foreach (var line in lines)
                {
                    string tr = line.Trim();
                    if (tr.Contains("\"Cameras\":")) { currentSection = "Cameras"; continue; }
                    if (tr.Contains("\"Servers\":")) { currentSection = "Servers"; continue; }

                    if (currentSection == "Cameras")
                    {
                        if (tr.StartsWith("{") && curCam == null)
                        {
                            curCam = new CameraModel();
                        }
                        else if (tr.StartsWith("}") || tr.StartsWith("},"))
                        {
                            if (curCam != null && !string.IsNullOrWhiteSpace(curCam.Name))
                            {
                                snap.Cameras.Add(curCam);
                                curCam = null;
                            }
                        }
                        else if (curCam != null && tr.Contains(":"))
                        {
                            int colonIdx = tr.IndexOf(':');
                            string key = tr.Substring(0, colonIdx).Replace("\"", "").Trim();
                            string val = tr.Substring(colonIdx + 1).Trim().TrimEnd(',');
                            string cleanVal = val.Trim('\"');

                            if (key == "Name") curCam.Name = cleanVal;
                            else if (key == "Group") curCam.Group = cleanVal;
                            else if (key == "ServerName") curCam.ServerName = cleanVal;
                            else if (key == "IpAddress") curCam.IpAddress = cleanVal;
                            else if (key == "UserName") curCam.UserName = cleanVal;
                            else if (key == "Password") curCam.Password = cleanVal;
                            else if (key == "HttpPort") { int p; if (int.TryParse(cleanVal, out p)) curCam.HttpPort = p; }
                            else if (key == "RtspPort") { int p; if (int.TryParse(cleanVal, out p)) curCam.RtspPort = p; }
                            else if (key == "Manufacturer") curCam.Manufacturer = cleanVal;
                            else if (key == "Model") curCam.Model = cleanVal;
                            else if (key == "DriverName") curCam.DriverName = cleanVal;
                            else if (key == "DriverVersion") curCam.DriverVersion = cleanVal;
                            else if (key == "MacAddress") curCam.MacAddress = cleanVal;
                            else if (key == "FirmwareVersion") curCam.FirmwareVersion = cleanVal;
                            else if (key == "Resolution") curCam.Resolution = cleanVal;
                            else if (key == "Fps") { int v; if (int.TryParse(cleanVal, out v)) curCam.Fps = v; }
                            else if (key == "Codec") curCam.Codec = cleanVal;
                            else if (key == "RetentionDays") { int v; if (int.TryParse(cleanVal, out v)) curCam.RetentionDays = v; }
                            else if (key == "StoragePath") curCam.StoragePath = cleanVal;
                            else if (key == "PrebufferSeconds") { int v; if (int.TryParse(cleanVal, out v)) curCam.PrebufferSeconds = v; }
                            else if (key == "PrebufferType") curCam.PrebufferType = cleanVal;
                            else if (key == "RecordFps") { int v; if (int.TryParse(cleanVal, out v)) curCam.RecordFps = v; }
                            else if (key == "RecordKeyframesOnly") curCam.RecordKeyframesOnly = cleanVal.Equals("true", StringComparison.OrdinalIgnoreCase);
                            else if (key == "MotionDetectionEnabled") curCam.MotionDetectionEnabled = cleanVal.Equals("true", StringComparison.OrdinalIgnoreCase);
                            else if (key == "MotionSensitivity") { int v; if (int.TryParse(cleanVal, out v)) curCam.MotionSensitivity = v; }
                            else if (key == "MotionThreshold") { int v; if (int.TryParse(cleanVal, out v)) curCam.MotionThreshold = v; }
                            else if (key == "MotionHardwareAcceleration") curCam.MotionHardwareAcceleration = cleanVal;
                            else if (key == "IsOnline") curCam.IsOnline = cleanVal.Equals("true", StringComparison.OrdinalIgnoreCase);
                            else if (key == "MulticastIp") curCam.MulticastIp = cleanVal;
                        }
                    }
                    else if (currentSection == "Servers")
                    {
                        if (tr.StartsWith("{") && curSrv == null)
                        {
                            curSrv = new RecordingServerModel();
                        }
                        else if (tr.StartsWith("}") || tr.StartsWith("},"))
                        {
                            if (curSrv != null && !string.IsNullOrWhiteSpace(curSrv.Name))
                            {
                                snap.Servers.Add(curSrv);
                                curSrv = null;
                            }
                        }
                        else if (curSrv != null && tr.Contains(":"))
                        {
                            int colonIdx = tr.IndexOf(':');
                            string key = tr.Substring(0, colonIdx).Replace("\"", "").Trim();
                            string val = tr.Substring(colonIdx + 1).Trim().TrimEnd(',');
                            string cleanVal = val.Trim('\"');

                            if (key == "Name") curSrv.Name = cleanVal;
                            else if (key == "Hostname") curSrv.Hostname = cleanVal;
                            else if (key == "Port") { int p; if (int.TryParse(cleanVal, out p)) curSrv.Port = p; }
                            else if (key == "Status") curSrv.Status = cleanVal;
                            else if (key == "DeviceCount") { int c; if (int.TryParse(cleanVal, out c)) curSrv.DeviceCount = c; }
                            else if (key == "PrimaryStoragePath") curSrv.PrimaryStoragePath = cleanVal;
                            else if (key == "FailoverServerName") curSrv.FailoverServerName = cleanVal;
                        }
                    }
                }
            }
            catch { }
            return snap;
        }

        private static string Escape(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            return s.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }
    }
}
