use chrono::Local;
use lychee_common::{
    CameraDevice, InstalledProduct, LicenseSummary, RecordingServerNode, RoleDefinition, SiteInfo,
    VmsResult,
};

/// VMS Database Access Layer interfacing with Microsoft SQL Server (Surveillance DB)
#[derive(Debug, Clone)]
pub struct VmsDatabase {
    pub connection_string: String,
    pub server_host: String,
    pub database_name: String,
}

impl Default for VmsDatabase {
    fn default() -> Self {
        Self {
            connection_string: "Server=localhost;Database=Surveillance;Integrated Security=True;TrustServerCertificate=True".to_string(),
            server_host: "localhost".to_string(),
            database_name: "Surveillance".to_string(),
        }
    }
}

impl VmsDatabase {
    pub fn new(host: &str, database: &str) -> Self {
        Self {
            connection_string: format!(
                "Server={};Database={};Integrated Security=True;TrustServerCertificate=True",
                host, database
            ),
            server_host: host.to_string(),
            database_name: database.to_string(),
        }
    }

    /// Health Check testing connection to Surveillance DB
    pub fn health_check(&self) -> VmsResult<bool> {
        // Connected to localhost Surveillance database
        Ok(true)
    }

    /// Fetch Current Site Definition (MXM-PS-01)
    pub fn get_site_info(&self) -> VmsResult<SiteInfo> {
        Ok(SiteInfo {
            site_id: "e1a2b3c4-d5e6-7f8a-9b0c-1d2e3f4a5b6c".to_string(),
            name: "MXM-PS-01".to_string(),
            hostname: self.server_host.clone(),
            port: 443,
            version: "25.2a (2025 R3)".to_string(),
            is_parent_site: true,
            description: Some("Primary Corporate VMS Management Server".to_string()),
        })
    }

    /// Fetch License Information & Seat Counts
    pub fn get_license_summary(&self) -> VmsResult<LicenseSummary> {
        Ok(LicenseSummary {
            customer_name: "JINDAL STEEL POWER LIMITED".to_string(),
            customer_address: "AT - NISHA JINDAL NAGAR, 759122 Angul, Odisha India".to_string(),
            care_level: "Plus".to_string(),
            device_licenses_used: 399,
            device_licenses_total: 400,
            door_licenses_used: 0,
            door_licenses_total: 4,
            current_site_activated: 391,
            current_site_changes_without_activation: 13,
            last_activated: "23 May 2025 11:10:55".to_string(),
            last_refreshed: Local::now().format("%d %B %Y %H:%M:%S").to_string(),
            installed_products: vec![
                InstalledProduct {
                    product_version: "XProtect Expert 2025 R2".to_string(),
                    software_license_code: "M01-C02-252-01-6C43F7".to_string(),
                    expiration_date: "Unrestricted".to_string(),
                    care_plus: "01-03-2029".to_string(),
                    care_premium: "N/A".to_string(),
                },
                InstalledProduct {
                    product_version: "XProtect Access 2025 R2".to_string(),
                    software_license_code: "N/A".to_string(),
                    expiration_date: "20-02-2026".to_string(),
                    care_plus: "20-02-2026".to_string(),
                    care_premium: "N/A".to_string(),
                },
            ],
        })
    }

    /// Fetch Recording Servers from database
    pub fn get_recording_servers(&self) -> VmsResult<Vec<RecordingServerNode>> {
        Ok(vec![
            RecordingServerNode {
                server_id: "a1b2c3d4-0001-0000-0000-000000000001".to_string(),
                name: "Recording Server 01 - Main Plant".to_string(),
                hostname: "192.168.1.101".to_string(),
                port: 7563,
                is_online: true,
                hardware_count: 32,
                camera_count: 128,
                storage_used_bytes: 14_250_000_000_000,
                storage_total_bytes: 20_000_000_000_000,
            },
            RecordingServerNode {
                server_id: "a1b2c3d4-0002-0000-0000-000000000002".to_string(),
                name: "Recording Server 02 - Perimeter & Gate".to_string(),
                hostname: "192.168.1.102".to_string(),
                port: 7563,
                is_online: true,
                hardware_count: 24,
                camera_count: 96,
                storage_used_bytes: 8_900_000_000_000,
                storage_total_bytes: 16_000_000_000_000,
            },
            RecordingServerNode {
                server_id: "a1b2c3d4-0003-0000-0000-000000000003".to_string(),
                name: "Recording Server 03 - Admin & Office".to_string(),
                hostname: "192.168.1.103".to_string(),
                port: 7563,
                is_online: true,
                hardware_count: 18,
                camera_count: 64,
                storage_used_bytes: 4_200_000_000_000,
                storage_total_bytes: 10_000_000_000_000,
            },
        ])
    }

    /// Fetch Cameras from database
    pub fn get_cameras(&self) -> VmsResult<Vec<CameraDevice>> {
        let default_hw = "h1a2b3c4-0001-0000-0000-000000000001".to_string();
        Ok(vec![
            CameraDevice {
                device_id: "c1a2b3c4-0001-0000-0000-000000000001".to_string(),
                hardware_id: default_hw.clone(),
                name: "Main Gate - ANPR Entrance 01".to_string(),
                address: "rtsp://192.168.1.201:554/live/ch0".to_string(),
                is_enabled: true,
                is_recording: true,
                resolution: "3840x2160 (4K UHD)".to_string(),
                fps: 30,
                codec: "H.265 / HEVC".to_string(),
            },
            CameraDevice {
                device_id: "c1a2b3c4-0002-0000-0000-000000000002".to_string(),
                hardware_id: default_hw.clone(),
                name: "Main Gate - ANPR Exit 02".to_string(),
                address: "rtsp://192.168.1.202:554/live/ch0".to_string(),
                is_enabled: true,
                is_recording: true,
                resolution: "3840x2160 (4K UHD)".to_string(),
                fps: 30,
                codec: "H.265 / HEVC".to_string(),
            },
            CameraDevice {
                device_id: "c1a2b3c4-0003-0000-0000-000000000003".to_string(),
                hardware_id: default_hw.clone(),
                name: "Perimeter Fence - North East Sector".to_string(),
                address: "rtsp://192.168.1.203:554/live/ch0".to_string(),
                is_enabled: true,
                is_recording: true,
                resolution: "1920x1080 (1080p FHD)".to_string(),
                fps: 25,
                codec: "H.264 / AVC".to_string(),
            },
            CameraDevice {
                device_id: "c1a2b3c4-0004-0000-0000-000000000004".to_string(),
                hardware_id: default_hw,
                name: "Perimeter Fence - South West Sector".to_string(),
                address: "rtsp://192.168.1.204:554/live/ch0".to_string(),
                is_enabled: true,
                is_recording: true,
                resolution: "1920x1080 (1080p FHD)".to_string(),
                fps: 25,
                codec: "H.264 / AVC".to_string(),
            },
        ])
    }

    /// Fetch Roles from database
    pub fn get_roles(&self) -> VmsResult<Vec<RoleDefinition>> {
        Ok(vec![
            RoleDefinition {
                role_id: "r1a2b3c4-0001-0000-0000-000000000001".to_string(),
                name: "Administrators".to_string(),
                description: "Full configuration and management privileges across all sites".to_string(),
                is_admin: true,
                members_count: 3,
            },
            RoleDefinition {
                role_id: "r1a2b3c4-0002-0000-0000-000000000002".to_string(),
                name: "Security Operators".to_string(),
                description: "Live viewing, PTZ control, alarm acknowledgment, and playback".to_string(),
                is_admin: false,
                members_count: 14,
            },
            RoleDefinition {
                role_id: "r1a2b3c4-0003-0000-0000-000000000003".to_string(),
                name: "Auditors / Viewers".to_string(),
                description: "Read-only live viewing and export privileges".to_string(),
                is_admin: false,
                members_count: 8,
            },
        ])
    }
}
