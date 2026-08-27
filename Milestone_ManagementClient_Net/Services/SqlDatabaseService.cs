using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MilestoneManagementClient.Models;

namespace MilestoneManagementClient.Services
{
    public class SqlDatabaseService
    {
        public static readonly string ConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=Surveillance;Integrated Security=True;TrustServerCertificate=True";

        public bool TestConnection()
        {
            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    return conn.State == ConnectionState.Open;
                }
            }
            catch
            {
                return false;
            }
        }

        public void SyncAllToMssql(List<CameraModel> cameras, List<RecordingServerModel> servers, List<RuleModel> rules)
        {
            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                    // 1. Sync Cameras to dbo.Devices & dbo.Hardware
                    foreach (var cam in cameras)
                    {
                        SyncCamera(conn, cam);
                    }

                    // 2. Sync Storage (30, 60, 90 days)
                    foreach (var cam in cameras)
                    {
                        SyncStorage(conn, cam.StoragePath, cam.RetentionDays);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("MSSQL Sync Error: " + ex.Message);
            }
        }

        public void SyncCamera(SqlConnection conn, CameraModel cam)
        {
            try
            {
                string digest = "UNIVERSAL_ONVIF_DRIVER_2025";
                string hwGuid = Guid.NewGuid().ToString();

                string query = @"
IF NOT EXISTS (SELECT 1 FROM dbo.Hardware WHERE Name = @Name + ' HW')
BEGIN
    INSERT INTO dbo.Hardware (
        IDHardware, IDHardwareDriver, Name, Description, URI, LoginID, EncryptedPassword,
        IDRecorder, Enabled, C18NDigest, DriverNumber, DriverProxyName, SubType, LastModified
    ) VALUES (
        @HwGuid, '00000000-0000-0000-0000-000000000001', @Name + ' HW', 'ONVIF Conformance Driver Device', 'http://192.168.1.100/onvif/device_service',
        'root', 'encrypted_pass', '11111111-2222-3333-4444-555555555555', 1, @Digest, 1, 'Universal1Channel', 1, GETUTCDATE()
    );
END

DECLARE @ActualHwId CHAR(36);
SELECT TOP 1 @ActualHwId = IDHardware FROM dbo.Hardware WHERE Name = @Name + ' HW';

IF EXISTS (SELECT 1 FROM dbo.Devices WHERE Name = @Name)
BEGIN
    UPDATE dbo.Devices 
    SET Enabled = @Enabled, 
        Settings = @Settings,
        Streams = @Streams,
        LastModified = GETUTCDATE()
    WHERE Name = @Name;
END
ELSE
BEGIN
    INSERT INTO dbo.Devices (
        IDDevice, IDHardware, Name, Description, Location, DeviceType, Enabled, Settings, Streams,
        Shortcut, MulticastEnabled, DeviceIndex, SubType, FailoverLiveOnly, DisableOnFailover,
        PtzManualSessionTimeout, PtzReservedSessionTimeout, PtzExtendedManualSessionTimeout,
        GisPoint, ShortName, Icon, CoverageDirection, CoverageDepth, CoverageFieldOfView,
        EdgeStorageEnabled, MulticastSettings, LastModified
    ) VALUES (
        NEWID(), @ActualHwId, @Name, @Description, @Group, 'Camera', @Enabled, @Settings, @Streams,
        '', 0, 1, 1, 0, 0,
        30, 30, 60,
        geography::Point(0, 0, 4326), @Name, 0, 0, 0, 0,
        0, '<MulticastSettings><Ip>0.0.0.0</Ip><Port>8860</Port></MulticastSettings>', GETUTCDATE()
    );
END";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", cam.Name ?? "");
                    cmd.Parameters.AddWithValue("@HwGuid", hwGuid);
                    cmd.Parameters.AddWithValue("@Digest", digest);
                    cmd.Parameters.AddWithValue("@Description", "Milestone IP Camera - " + (cam.Resolution ?? "1080p"));
                    cmd.Parameters.AddWithValue("@Group", cam.Group ?? "Default");
                    cmd.Parameters.AddWithValue("@Enabled", cam.IsOnline);
                    cmd.Parameters.AddWithValue("@Settings", string.Format("<Settings><Fps>{0}</Fps><Codec>{1}</Codec><Prebuffer>{2}</Prebuffer><MotionSensitivity>{3}</MotionSensitivity></Settings>", cam.Fps, cam.Codec, cam.PrebufferSeconds, cam.MotionSensitivity));
                    cmd.Parameters.AddWithValue("@Streams", string.Format("<Streams><Stream name=\"Main\" res=\"{0}\" fps=\"{1}\" codec=\"{2}\" /></Streams>", cam.Resolution, cam.Fps, cam.Codec));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Camera SQL Error: " + ex.Message);
            }
        }

        public void SyncStorage(SqlConnection conn, string storagePath, int retentionDays)
        {
            try
            {
                int retainMinutes = retentionDays * 24 * 60;
                string query = @"
IF EXISTS (SELECT 1 FROM dbo.RecordingStorage WHERE path = @Path)
BEGIN
    UPDATE dbo.RecordingStorage 
    SET retain_minutes = @RetainMinutes,
        last_modified = GETUTCDATE()
    WHERE path = @Path;
END
ELSE
BEGIN
    INSERT INTO dbo.RecordingStorage (pkid, recordingserver_id, name, description, path, is_default, retain_minutes, max_size_mb, created_date, last_modified)
    VALUES (NEWID(), NEWID(), 'Primary Media Storage', 'Media database archive', @Path, 1, @RetainMinutes, 8000000, GETUTCDATE(), GETUTCDATE());
END";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Path", string.IsNullOrEmpty(storagePath) ? @"D:\Milestone\MediaDatabase" : storagePath);
                    cmd.Parameters.AddWithValue("@RetainMinutes", retainMinutes);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Storage SQL Error: " + ex.Message);
            }
        }

        public void DeleteCameraFromSql(string camName)
        {
            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("DELETE FROM dbo.Devices WHERE Name = @Name", conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", camName);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch { }
        }
    }
}
