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

        public void DeleteRecordingServer(string name)
        {
            var srv = _servers.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (srv != null)
            {
                _servers.Remove(srv);
                AddLog("WARN", "Servers", "Deleted recording server '" + name + "'", "Administrator");
                SaveToDatabase();
            }
        }

        // --- License Info ---
        public LicenseInfoModel GetLicense() { return _license; }

        // --- Rules Management ---
        public List<RuleModel> GetRules() { return _rules; }

        public void AddRule(string name, string trigger, string target, string timeProfile, string action, bool enabled)
        {
            _rules.Add(new RuleModel(name, trigger, target, timeProfile, action, enabled));
            AddLog("INFO", "Rules", "Created new rule: '" + name + "'", "Administrator");
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

        public void DeleteUser(string username)
        {
            var u = _users.FirstOrDefault(x => x.Username == username);
            if (u != null)
            {
                _users.Remove(u);
                AddLog("WARN", "Security", "Removed user '" + username + "'", "Administrator");
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
            var groups = new Dictionary<string, string[]>
            {
                { "Lychee Recording Cluster", new[] { "LYCHEE MAIN ENTRANCE CAM 01", "LYCHEE PERIMETER PTZ CAM 02" } },
                { "Camera Group 1", new[] { "MAIN GATE ENTRY CAM", "MAIN GATE EXIT CAM", "VISITOR LOBBY CAM 01", "PARKING NORTH CAM 01" } },
                { "ROAD SAFETY", new[] { "SPEED CAM 01 - NORTH ROAD", "SPEED CAM 02 - JUNCTION", "ANPR HIGHWAY CAM 01", "ANPR HIGHWAY CAM 02" } },
                { "SECTOR-1", new[] { "SEC1 BOUNDARY CAM 1", "SEC1 BOUNDARY CAM 2", "SEC1 SUBSTATION CAM", "SEC1 TOWER CAM" } },
                { "SECTOR-2", new[] { "SEC2 CONVEYER CAM 1", "SEC2 CONVEYER CAM 2", "SEC2 PUMP HOUSE CAM" } },
                { "SECTOR-3", new[] { "SEC3 HOPPER CAM 1", "SEC3 CRUSHER CAM 1", "SEC3 LOADING AREA CAM" } },
                { "SECTOR-4", new[] { "SEC4 WAREHOUSE EAST", "SEC4 WAREHOUSE WEST", "SEC4 DISPATCH CAM" } },
                { "SECTOR - UTKAL -C- MINES", new[] { "MINES PIT CAM 01", "MINES PIT CAM 02", "HAUL ROAD CAM 01", "WEIGHBRIDGE CAM 01" } },
                { "Sharmik Vihar", new[] { "SHARMIK VIHAR GATE CAM", "SHARMIK VIHAR RESIDENTIAL 01", "SHARMIK VIHAR CLUB CAM" } },
                { "South Block", new[] {
                    "AIRTEL TOWER SIDE WALL CAM 2",
                    "AIRTEL TOWER SIDE WALL CAM 1",
                    "B32 WALL CAM 1",
                    "B32 WALL CAM 2",
                    "B38 SIDE WALL CAM 1",
                    "B38 SIDE WALL CAM 2",
                    "B47 WALL CAMERA 1",
                    "B47 WALL CAMERA 2",
                    "B47 WALL CAMERA 3",
                    "B53 SIDE WALL CAM 1",
                    "B58 WALL CAM 1",
                    "B58 WALL CAM 2",
                    "B65 FRONT WALL CAM 1",
                    "B65 FRONT WALL CAM 2",
                    "B69 FRONT WALL CAM",
                    "B73 SIDE WALL CAM 1",
                    "B73 SIDE WALL CAM 2",
                    "B75 BACK WALL CAM 1",
                    "B77 BACK WALL CAM",
                    "B79 SIDE WALL CAM 1",
                    "B79 SIDE WALL CAM 2",
                    "B80 SIDE WALL CAM 1",
                    "B80 SIDE WALL CAM 2",
                    "EP-02 cam-1",
                    "SERVER ROOM FRONT CAM 1",
                    "SERVER ROOM FRONT CAM 2",
                    "SERVER ROOM FRONT CAM 3"
                }},
                { "Universal", new[] { "UNIVERSAL PTZ 01 (360 DEG)", "UNIVERSAL THERMAL CAM 01", "PANORAMIC MULTI-SENSOR CAM" } }
            };

            foreach (var kvp in groups)
            {
                foreach (var camName in kvp.Value)
                {
                    bool is4k = camName.Contains("B32") || camName.Contains("B38") || camName.Contains("B58") || camName.Contains("B80") || camName.Contains("PTZ");
                    var cam = new CameraModel
                    {
                        Name = camName,
                        Group = kvp.Key,
                        Resolution = is4k ? "2688x1520 (4MP QHD)" : "1920x1080 (1080p FHD)",
                        Codec = "H.265",
                        Fps = 25,
                        BitrateMode = "Variable bit rate",
                        Quality = 60,
                        Port1 = 8860,
                        Port2 = 8866,
                        Stream2Res = "640x360",
                        Stream2Fps = 25,
                        Brightness = 50,
                        Contrast = 50,
                        Saturation = 50,
                        Sharpness = 50,
                        MulticastIp = "0.0.0.0",
                        StreamingMethod = "RTP/RTSP/TCP"
                    };
                    _cameras[camName] = cam;
                    _savedConfigurations[camName] = cam.Clone();
                }
            }
            _license.HardwareDeviceLicensesUsed = _cameras.Count;
        }

        private void InitializeServers()
        {
            _servers.Add(new RecordingServerModel
            {
                Name = "REC-SRV-01 (Primary Production)",
                Hostname = "192.168.1.101",
                Port = 7563,
                Status = "Running (Active)",
                DeviceCount = 14,
                CpuUsage = 32,
                RamUsage = 48,
                PrimaryStoragePath = @"D:\Milestone\MediaDatabase\Live01",
                StorageUsedGb = 4200,
                StorageTotalGb = 10000,
                ArchiveStoragePath = @"E:\Milestone\Archive01",
                ArchiveUsedGb = 18000,
                ArchiveTotalGb = 40000,
                IsFailoverConfigured = true,
                FailoverServerName = "FAILOVER-SRV-01 (Hot Standby)"
            });

            _servers.Add(new RecordingServerModel
            {
                Name = "REC-SRV-02 (Storage Pool Alpha)",
                Hostname = "192.168.1.102",
                Port = 7563,
                Status = "Running (Active)",
                DeviceCount = 8,
                CpuUsage = 24,
                RamUsage = 38,
                PrimaryStoragePath = @"D:\Milestone\MediaDatabase\Live02",
                StorageUsedGb = 2800,
                StorageTotalGb = 8000,
                ArchiveStoragePath = @"E:\Milestone\Archive02",
                ArchiveUsedGb = 12000,
                ArchiveTotalGb = 32000,
                IsFailoverConfigured = true,
                FailoverServerName = "FAILOVER-SRV-01 (Hot Standby)"
            });

            _servers.Add(new RecordingServerModel
            {
                Name = "REC-SRV-03 (Storage Pool Beta)",
                Hostname = "192.168.1.103",
                Port = 7563,
                Status = "Running (Active)",
                DeviceCount = 4,
                CpuUsage = 18,
                RamUsage = 29,
                PrimaryStoragePath = @"D:\Milestone\MediaDatabase\Live03",
                StorageUsedGb = 1500,
                StorageTotalGb = 8000,
                ArchiveStoragePath = @"E:\Milestone\Archive03",
                ArchiveUsedGb = 6500,
                ArchiveTotalGb = 24000,
                IsFailoverConfigured = false,
                FailoverServerName = "None"
            });

            _servers.Add(new RecordingServerModel
            {
                Name = "REC-SRV-04 (Edge Processing & AI)",
                Hostname = "192.168.1.104",
                Port = 7563,
                Status = "Running (Active)",
                DeviceCount = 0,
                CpuUsage = 44,
                RamUsage = 62,
                PrimaryStoragePath = @"D:\Milestone\MediaDatabase\Live04",
                StorageUsedGb = 800,
                StorageTotalGb = 4000,
                ArchiveStoragePath = @"E:\Milestone\Archive04",
                ArchiveUsedGb = 2000,
                ArchiveTotalGb = 16000,
                IsFailoverConfigured = false,
                FailoverServerName = "None"
            });
        }

        private void InitializeRules()
        {
            _rules.Add(new RuleModel("Rule 01: Motion -> Start Recording", "Motion Started", "All Cameras in South Block", "Always", "Start recording on related device at 25 fps", true));
            _rules.Add(new RuleModel("Rule 02: ANPR Speed Violation Email Alert", "Speed > 40 km/h detected", "ROAD SAFETY Cameras", "Always", "Send Email Notification to transport@company.com with Snapshot", true));
            _rules.Add(new RuleModel("Rule 03: Perimeter Line Crossing Alarm", "Analytics: Line Crossing", "SECTOR-1 & SECTOR-4 Cameras", "Night Shift (20:00 - 06:00)", "Trigger Siren Output 1 & PTZ Preset 03", true));
            _rules.Add(new RuleModel("Rule 04: Server Room Door Access Recording", "Access Control Door Opened", "SERVER ROOM FRONT CAM 1, 2, 3", "Always", "Bookmark event & record at High Quality 60fps", true));
            _rules.Add(new RuleModel("Rule 05: Daily Midnight Archive Backup", "Timer: 00:00 Daily", "All Recording Servers", "Always", "Move recorded footage older than 7 days to Archive Pool", true));
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
            sb.AppendLine("  \"Cameras\": [");
            for (int i = 0; i < snap.Cameras.Count; i++)
            {
                var c = snap.Cameras[i];
                sb.AppendLine("    {");
                sb.AppendLine("      \"Name\": \"" + Escape(c.Name) + "\",");
                sb.AppendLine("      \"Group\": \"" + Escape(c.Group) + "\",");
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
                sb.AppendLine("      \"MulticastIp\": \"" + Escape(c.MulticastIp) + "\",");
                sb.AppendLine("      \"EventsCount\": " + c.DriverEvents.Count);
                sb.Append("    }");
                if (i < snap.Cameras.Count - 1) sb.Append(",");
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

                foreach (var line in lines)
                {
                    string tr = line.Trim();
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
