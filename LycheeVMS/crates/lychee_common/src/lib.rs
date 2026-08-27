use chrono::Utc;
use serde::{Deserialize, Serialize};
use std::fmt;

#[derive(Debug, Clone, Serialize, Deserialize)]
pub enum VmsError {
    DatabaseError(String),
    ConfigError(String),
    AuthError(String),
    DeviceNotFound(String),
    LicenseError(String),
    Internal(String),
}

impl fmt::Display for VmsError {
    fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result {
        match self {
            VmsError::DatabaseError(msg) => write!(f, "Database error: {}", msg),
            VmsError::ConfigError(msg) => write!(f, "Configuration error: {}", msg),
            VmsError::AuthError(msg) => write!(f, "Authentication error: {}", msg),
            VmsError::DeviceNotFound(id) => write!(f, "Device not found: {}", id),
            VmsError::LicenseError(msg) => write!(f, "License error: {}", msg),
            VmsError::Internal(msg) => write!(f, "Internal error: {}", msg),
        }
    }
}

impl std::error::Error for VmsError {}

pub type VmsResult<T> = Result<T, VmsError>;

/// Represents a VMS Site definition (e.g. MXM-PS-01)
#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct SiteInfo {
    pub site_id: String,
    pub name: String,
    pub hostname: String,
    pub port: u16,
    pub version: String,
    pub is_parent_site: bool,
    pub description: Option<String>,
}

impl Default for SiteInfo {
    fn default() -> Self {
        Self {
            site_id: "e1a2b3c4-d5e6-7f8a-9b0c-1d2e3f4a5b6c".to_string(),
            name: "MXM-PS-01".to_string(),
            hostname: "localhost".to_string(),
            port: 443,
            version: "25.2a (2025 R3)".to_string(),
            is_parent_site: true,
            description: Some("Primary Corporate VMS Management Server".to_string()),
        }
    }
}

/// Represents an Installed Milestone Product License
#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct InstalledProduct {
    pub product_version: String,
    pub software_license_code: String,
    pub expiration_date: String,
    pub care_plus: String,
    pub care_premium: String,
}

/// Represents License Seat Overview (All Sites vs Current Site)
#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct LicenseSummary {
    pub customer_name: String,
    pub customer_address: String,
    pub care_level: String,
    pub device_licenses_used: u32,
    pub device_licenses_total: u32,
    pub door_licenses_used: u32,
    pub door_licenses_total: u32,
    pub current_site_activated: u32,
    pub current_site_changes_without_activation: u32,
    pub last_activated: String,
    pub last_refreshed: String,
    pub installed_products: Vec<InstalledProduct>,
}

impl Default for LicenseSummary {
    fn default() -> Self {
        Self {
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
            last_refreshed: Utc::now().format("%d %B %Y %H:%M:%S").to_string(),
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
        }
    }
}

/// Represents a Recording Server node in the VMS Hierarchy
#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct RecordingServerNode {
    pub server_id: String,
    pub name: String,
    pub hostname: String,
    pub port: u16,
    pub is_online: bool,
    pub hardware_count: u32,
    pub camera_count: u32,
    pub storage_used_bytes: u64,
    pub storage_total_bytes: u64,
}

/// Represents a Camera Device
#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct CameraDevice {
    pub device_id: String,
    pub hardware_id: String,
    pub name: String,
    pub address: String,
    pub is_enabled: bool,
    pub is_recording: bool,
    pub resolution: String,
    pub fps: u32,
    pub codec: String,
}

/// Represents a User Role
#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct RoleDefinition {
    pub role_id: String,
    pub name: String,
    pub description: String,
    pub is_admin: bool,
    pub members_count: u32,
}
