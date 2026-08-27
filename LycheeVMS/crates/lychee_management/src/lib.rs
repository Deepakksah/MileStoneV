use lychee_common::{CameraDevice, RecordingServerNode, SiteInfo, VmsResult};
use lychee_database::VmsDatabase;
use serde::{Deserialize, Serialize};

#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct HardwareUnit {
    pub hardware_id: String,
    pub name: String,
    pub ip_address: String,
    pub mac_address: String,
    pub driver_number: u32,
    pub driver_name: String,
    pub model: String,
    pub serial_number: String,
    pub firmware_version: String,
    pub is_enabled: bool,
    pub recording_server_id: String,
}

#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct DeviceGroup {
    pub group_id: String,
    pub name: String,
    pub parent_id: Option<String>,
    pub camera_ids: Vec<String>,
}

#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct CameraStreamProfile {
    pub brightness: u32,
    pub contrast: u32,
    pub saturation: u32,
    pub sharpness: u32,
    pub multicast_address: String,
    // Stream 1
    pub stream1_codec: String,
    pub stream1_control_mode: String,
    pub stream1_fps: String,
    pub stream1_h265_plus: bool,
    pub stream1_keyframe_interval: u32,
    pub stream1_keyframe_mode: String,
    pub stream1_multicast_port: u16,
    pub stream1_quality: u32,
    pub stream1_resolution: String,
    pub stream1_method: String,
    // Stream 2
    pub stream2_codec: String,
    pub stream2_control_mode: String,
    pub stream2_fps: u32,
    pub stream2_h265_plus: bool,
    pub stream2_keyframe_interval: u32,
    pub stream2_keyframe_mode: String,
    pub stream2_multicast_port: u16,
    pub stream2_quality: u32,
    pub stream2_resolution: String,
    pub stream2_method: String,
}

impl Default for CameraStreamProfile {
    fn default() -> Self {
        Self {
            brightness: 50,
            contrast: 50,
            saturation: 50,
            sharpness: 50,
            multicast_address: "0.0.0.0".to_string(),
            stream1_codec: "H.265".to_string(),
            stream1_control_mode: "Variable bit rate".to_string(),
            stream1_fps: String::new(),
            stream1_h265_plus: false,
            stream1_keyframe_interval: 8,
            stream1_keyframe_mode: "Default (determined by driver)".to_string(),
            stream1_multicast_port: 8860,
            stream1_quality: 60,
            stream1_resolution: String::new(),
            stream1_method: "RTP/RTSP/TCP".to_string(),
            stream2_codec: String::new(),
            stream2_control_mode: "Variable bit rate".to_string(),
            stream2_fps: 25,
            stream2_h265_plus: false,
            stream2_keyframe_interval: 8,
            stream2_keyframe_mode: "Default (determined by driver)".to_string(),
            stream2_multicast_port: 8866,
            stream2_quality: 60,
            stream2_resolution: "640x360".to_string(),
            stream2_method: "RTP/RTSP/TCP".to_string(),
        }
    }
}

pub struct ManagementServer {
    db: VmsDatabase,
    hardware_units: Vec<HardwareUnit>,
    cameras: Vec<CameraDevice>,
    recording_servers: Vec<RecordingServerNode>,
    device_groups: Vec<DeviceGroup>,
    stream_profile: CameraStreamProfile,
}

impl Default for ManagementServer {
    fn default() -> Self {
        let db = VmsDatabase::default();
        let recording_servers = db.get_recording_servers().unwrap_or_default();
        let cameras = db.get_cameras().unwrap_or_default();

        Self {
            db,
            hardware_units: vec![
                HardwareUnit {
                    hardware_id: "h1a2b3c4-0001-0000-0000-000000000001".to_string(),
                    name: "IPCamera (10.40.56.203)".to_string(),
                    ip_address: "http://10.40.56.203/".to_string(),
                    mac_address: "3C:DA:6D:C7:81:FD".to_string(),
                    driver_number: 1045,
                    driver_name: "ONVIF Conformance Device".to_string(),
                    model: "IPCamera".to_string(),
                    serial_number: "ID0602101001991202530060".to_string(),
                    firmware_version: "ONVIF_V3.3.0.20220325".to_string(),
                    is_enabled: true,
                    recording_server_id: "a1b2c3d4-0001-0000-0000-000000000001".to_string(),
                },
                HardwareUnit {
                    hardware_id: "h1a2b3c4-0002-0000-0000-000000000002".to_string(),
                    name: "Alcon EXCHANGE_ENTRANCE (10.36.10.247)".to_string(),
                    ip_address: "http://10.36.10.247/".to_string(),
                    mac_address: "00:1A:2B:3C:4D:5E".to_string(),
                    driver_number: 1002,
                    driver_name: "Alcon Native Protocol Driver".to_string(),
                    model: "Alcon 4K Pro Ultra".to_string(),
                    serial_number: "ALC20250918820011".to_string(),
                    firmware_version: "ALC_FW_4.2.1.2024".to_string(),
                    is_enabled: true,
                    recording_server_id: "a1b2c3d4-0001-0000-0000-000000000001".to_string(),
                },
            ],
            cameras,
            recording_servers,
            device_groups: vec![
                DeviceGroup { group_id: "g-01".to_string(), name: "Camera Group 1".to_string(), parent_id: None, camera_ids: vec![] },
                DeviceGroup { group_id: "g-02".to_string(), name: "ROAD SAFETY".to_string(), parent_id: None, camera_ids: vec![] },
                DeviceGroup { group_id: "g-03".to_string(), name: "SECTOR-1".to_string(), parent_id: None, camera_ids: vec![] },
                DeviceGroup { group_id: "g-04".to_string(), name: "SECTOR-2".to_string(), parent_id: None, camera_ids: vec![] },
                DeviceGroup { group_id: "g-05".to_string(), name: "SECTOR-3".to_string(), parent_id: None, camera_ids: vec![] },
                DeviceGroup { group_id: "g-06".to_string(), name: "SECTOR-4".to_string(), parent_id: None, camera_ids: vec![] },
                DeviceGroup { group_id: "g-07".to_string(), name: "SECTOR - UTKAL -C- MINES".to_string(), parent_id: None, camera_ids: vec![] },
                DeviceGroup { group_id: "g-08".to_string(), name: "Sharmik Vihar".to_string(), parent_id: None, camera_ids: vec![] },
                DeviceGroup { group_id: "g-09".to_string(), name: "South Block".to_string(), parent_id: None, camera_ids: vec![] },
                DeviceGroup { group_id: "g-10".to_string(), name: "Universal".to_string(), parent_id: None, camera_ids: vec![] },
            ],
            stream_profile: CameraStreamProfile::default(),
        }
    }
}

impl ManagementServer {
    /// Get Site Information
    pub fn get_site_info(&self) -> VmsResult<SiteInfo> {
        self.db.get_site_info()
    }

    /// List all Device Groups
    pub fn list_device_groups(&self) -> &[DeviceGroup] {
        &self.device_groups
    }

    /// Add a new Device Group
    pub fn add_device_group(&mut self, name: &str) -> VmsResult<()> {
        let group_id = format!("g-{:02}", self.device_groups.len() + 1);
        self.device_groups.push(DeviceGroup {
            group_id,
            name: name.to_string(),
            parent_id: None,
            camera_ids: vec![],
        });
        Ok(())
    }

    /// Delete Device Group
    pub fn delete_device_group(&mut self, group_id: &str) -> VmsResult<()> {
        self.device_groups.retain(|g| g.group_id != group_id);
        Ok(())
    }

    /// Rename Device Group
    pub fn rename_device_group(&mut self, group_id: &str, new_name: &str) -> VmsResult<()> {
        if let Some(g) = self.device_groups.iter_mut().find(|g| g.group_id == group_id) {
            g.name = new_name.to_string();
        }
        Ok(())
    }

    /// Get Camera Stream Profile (HikVisionGeneric)
    pub fn get_stream_profile(&self) -> &CameraStreamProfile {
        &self.stream_profile
    }

    /// Update Camera Stream Profile
    pub fn update_stream_profile(&mut self, profile: CameraStreamProfile) {
        self.stream_profile = profile;
    }

    /// List all registered Hardware Units
    pub fn list_hardwares(&self) -> &[HardwareUnit] {
        &self.hardware_units
    }

    /// Get Hardware by name
    pub fn get_hardware_by_name(&self, name: &str) -> Option<&HardwareUnit> {
        self.hardware_units.iter().find(|h| h.name.eq_ignore_ascii_case(name))
    }

    /// Add a new Hardware Unit and its associated cameras
    pub fn add_hardware(&mut self, hardware: HardwareUnit, cameras: Vec<CameraDevice>) -> VmsResult<()> {
        self.hardware_units.push(hardware);
        for cam in cameras {
            self.cameras.push(cam);
        }
        Ok(())
    }

    /// Edit Hardware properties
    pub fn edit_hardware(&mut self, hardware_id: &str, name: &str, ip: &str, model: &str) -> VmsResult<()> {
        if let Some(hw) = self.hardware_units.iter_mut().find(|h| h.hardware_id == hardware_id) {
            hw.name = name.to_string();
            hw.ip_address = ip.to_string();
            hw.model = model.to_string();
        }
        Ok(())
    }

    /// Move Hardware from one recording server to another
    pub fn move_hardware(&mut self, hardware_id: &str, target_server_id: &str) -> VmsResult<()> {
        if let Some(hw) = self.hardware_units.iter_mut().find(|h| h.hardware_id == hardware_id) {
            hw.recording_server_id = target_server_id.to_string();
        }
        Ok(())
    }

    /// Delete single hardware unit
    pub fn delete_hardware(&mut self, hardware_id: &str) -> VmsResult<()> {
        self.hardware_units.retain(|h| h.hardware_id != hardware_id);
        self.cameras.retain(|c| c.hardware_id != hardware_id);
        Ok(())
    }

    /// Delete all hardware on a recording server
    pub fn delete_all_hardware(&mut self, server_id: &str) -> VmsResult<()> {
        let hw_ids: Vec<String> = self
            .hardware_units
            .iter()
            .filter(|h| h.recording_server_id == server_id)
            .map(|h| h.hardware_id.clone())
            .collect();

        self.hardware_units.retain(|h| h.recording_server_id != server_id);
        self.cameras.retain(|c| !hw_ids.contains(&c.hardware_id));
        Ok(())
    }

    /// Change Hardware Password (ONVIF/RTSP)
    pub fn change_hardware_password(&mut self, _hardware_id: &str, _new_pass: &str) -> VmsResult<()> {
        Ok(())
    }

    /// Update Hardware Firmware
    pub fn update_firmware(&mut self, hardware_id: &str, firmware_version: &str) -> VmsResult<()> {
        if let Some(hw) = self.hardware_units.iter_mut().find(|h| h.hardware_id == hardware_id) {
            hw.firmware_version = firmware_version.to_string();
        }
        Ok(())
    }

    /// Replace Hardware
    pub fn replace_hardware(&mut self, old_hardware_id: &str, new_hardware: HardwareUnit) -> VmsResult<()> {
        if let Some(hw) = self.hardware_units.iter_mut().find(|h| h.hardware_id == old_hardware_id) {
            *hw = new_hardware;
        }
        Ok(())
    }

    /// Rename Hardware
    pub fn rename_hardware(&mut self, hardware_id: &str, new_name: &str) -> VmsResult<()> {
        if let Some(hw) = self.hardware_units.iter_mut().find(|h| h.hardware_id == hardware_id) {
            hw.name = new_name.to_string();
        }
        Ok(())
    }

    /// Set Hardware Enabled / Disabled
    pub fn set_hardware_enabled(&mut self, hardware_id: &str, is_enabled: bool) -> VmsResult<()> {
        if let Some(hw) = self.hardware_units.iter_mut().find(|h| h.hardware_id == hardware_id) {
            hw.is_enabled = is_enabled;
        }
        Ok(())
    }

    /// Rename Recording Server
    pub fn rename_recording_server(&mut self, server_id: &str, new_name: &str) -> VmsResult<()> {
        if let Some(srv) = self.recording_servers.iter_mut().find(|s| s.server_id == server_id) {
            srv.name = new_name.to_string();
        }
        Ok(())
    }

    /// Add a new camera to configuration
    pub fn add_camera(&mut self, camera: CameraDevice) -> VmsResult<()> {
        self.cameras.push(camera);
        Ok(())
    }

    /// Delete a camera by device ID
    pub fn delete_camera(&mut self, device_id: &str) -> VmsResult<()> {
        self.cameras.retain(|c| c.device_id != device_id);
        Ok(())
    }

    /// List all cameras
    pub fn list_cameras(&self) -> &[CameraDevice] {
        &self.cameras
    }

    /// Add a new recording server
    pub fn add_recording_server(&mut self, server: RecordingServerNode) -> VmsResult<()> {
        self.recording_servers.push(server);
        Ok(())
    }

    /// Delete a recording server by server ID
    pub fn delete_recording_server(&mut self, server_id: &str) -> VmsResult<()> {
        self.recording_servers.retain(|s| s.server_id != server_id);
        Ok(())
    }

    /// List all recording servers
    pub fn list_recording_servers(&self) -> &[RecordingServerNode] {
        &self.recording_servers
    }
}
