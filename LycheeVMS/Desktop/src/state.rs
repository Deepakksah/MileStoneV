use lychee_analytics::AnalyticsEngine;
use lychee_common::{CameraDevice, RecordingServerNode};
use lychee_events::EventServer;
use lychee_identity::IdentityServer;
use lychee_management::{CameraStreamProfile, HardwareUnit, ManagementServer};
use lychee_mobile::MobileServerGateway;
use lychee_platform::VideoRecordingEngine;
use std::collections::HashMap;
use std::time::Instant;

#[inline]
pub fn clamp_safe(val: f32, min: f32, max: f32) -> f32 {
    let lower = min.min(max);
    let upper = min.max(max);
    val.clamp(lower, upper)
}


#[derive(PartialEq, Clone, Copy, Debug)]
pub enum SelectedNav {
    LicenseInformation,
    SiteInformation,
    RecordingServers,
    FailoverServers,
    MobileServers,
    Cameras,
    Microphones,
    Speakers,
    Metadata,
    Input,
    Output,
    ViewGroups,
    SmartClientProfiles,
    Matrix,
    Rules,
    TimeProfiles,
    NotificationProfiles,
    UserDefinedEvents,
    AnalyticsEvents,
    GenericEvents,
    Webhooks,
    Roles,
    BasicUsers,
    CurrentTasks,
    SystemMonitor,
    SystemMonitorThresholds,
    ConfigurationReports,
    ServerLogs,
    MetadataSearch,
    AccessControl,
    Alarms,
}

#[derive(PartialEq, Clone, Copy, Debug)]
pub enum CameraTab {
    Info,
    Settings,
    Streams,
    Record,
    Motion,
    Client,
}

#[derive(Clone, Debug)]
pub struct CameraNodeInfo {
    pub name: String,
    pub group: String,
    pub resolution: String,
    pub codec: String,
    pub fps: u32,
    pub bitrate_mode: String,
    pub quality: u32,
    pub port1: u16,
    pub port2: u16,
    pub stream2_res: String,
    pub stream2_fps: u32,
    pub brightness: i32,
    pub contrast: i32,
    pub saturation: i32,
    pub sharpness: i32,
    pub multicast_ip: String,
    pub streaming_method: String,
}

#[allow(dead_code)]
pub struct ManagementClientApp {
    pub selected_nav: SelectedNav,
    pub management_server: ManagementServer,
    pub identity_server: IdentityServer,
    pub recording_engine: VideoRecordingEngine,
    pub event_server: EventServer,
    pub mobile_gateway: MobileServerGateway,
    pub analytics_engine: AnalyticsEngine,
    pub auto_activate_license: bool,

    // Dynamic Resizable Flexible Splitters
    pub middle_panel_width: f32,
    pub bottom_dock_height: f32,

    // Modals
    pub show_add_camera_modal: bool,
    pub new_cam_name: String,
    pub new_cam_ip: String,
    pub new_cam_res: String,
    pub new_cam_fps: u32,
    pub new_cam_codec: String,
    pub new_cam_rec: bool,

    pub show_add_server_modal: bool,
    pub new_server_name: String,
    pub new_server_ip: String,
    pub new_server_port: u16,
    pub new_server_storage_tb: f64,

    pub show_add_hardware_wizard: bool,
    pub new_hw_name: String,
    pub new_hw_ip: String,
    pub new_hw_model: String,
    pub new_hw_driver: u32,
    pub new_hw_channels: u32,

    pub show_rename_server_modal: bool,
    pub rename_target_id: String,
    pub rename_target_name: String,

    pub show_change_pwd_modal: bool,
    pub new_hardware_pwd: String,
    pub confirm_hardware_pwd: String,

    pub show_move_hardware_modal: bool,

    // Selected Hardware / Camera Properties
    pub selected_hardware_id: String,
    pub selected_hardware_name: String,
    pub selected_hardware_model: String,
    pub selected_hardware_serial: String,
    pub selected_hardware_driver: String,
    pub selected_hardware_addr: String,
    pub selected_hardware_mac: String,
    pub selected_hardware_fw: String,
    pub selected_hardware_enabled: bool,
    pub selected_preview_camera_name: String,

    // Active Camera Live Settings (Fully Editable)
    pub current_camera_settings: CameraNodeInfo,
    pub camera_settings_db: HashMap<String, CameraNodeInfo>,
    pub save_feedback_time: Option<Instant>,

    pub show_edit_hardware_modal: bool,
    pub show_update_fw_modal: bool,
    pub new_firmware_ver: String,
    pub show_rename_hw_modal: bool,
    pub rename_hw_text: String,

    // Dynamic Device Groups List & Editable Hierarchy
    pub groups_hierarchy: Vec<(String, Vec<String>)>,

    // Inline Editing for Folders
    pub editing_folder_original_name: Option<String>,
    pub inline_folder_text: String,

    // Inline Editing for Cameras
    pub editing_camera_original_name: Option<String>,
    pub inline_camera_text: String,

    // Device Groups & Exact Context Menu Modals
    pub selected_device_group: String,
    pub show_add_device_group_modal: bool,
    pub new_device_group_name: String,
    pub show_rename_device_group_modal: bool,
    pub rename_device_group_name: String,
    pub show_edit_members_modal: bool,
    pub edit_members_group_name: String,
    pub edit_members_available_cams: Vec<String>,
    pub edit_members_selected_cams: Vec<bool>,

    pub camera_tab: CameraTab,
    pub stream_profile: CameraStreamProfile,
}

impl Default for ManagementClientApp {
    fn default() -> Self {
        let initial_cam = CameraNodeInfo {
            name: "AIRTEL TOWER SIDE WALL CAM 2".to_string(),
            group: "South Block".to_string(),
            resolution: "1920x1080 (1080p FHD)".to_string(),
            codec: "H.265".to_string(),
            fps: 25,
            bitrate_mode: "Variable bit rate".to_string(),
            quality: 60,
            port1: 8860,
            port2: 8866,
            stream2_res: "640x360".to_string(),
            stream2_fps: 25,
            brightness: 50,
            contrast: 50,
            saturation: 50,
            sharpness: 50,
            multicast_ip: "0.0.0.0".to_string(),
            streaming_method: "RTP/RTSP/TCP".to_string(),
        };

        let default_hierarchy = get_initial_hierarchy();

        Self {
            selected_nav: SelectedNav::Cameras,
            management_server: ManagementServer::default(),
            identity_server: IdentityServer::default(),
            recording_engine: VideoRecordingEngine::default(),
            event_server: EventServer::default(),
            mobile_gateway: MobileServerGateway::default(),
            analytics_engine: AnalyticsEngine::default(),
            auto_activate_license: false,

            middle_panel_width: 320.0,
            bottom_dock_height: 220.0,

            show_add_camera_modal: false,
            new_cam_name: "Warehouse North Gate - Cam 05".to_string(),
            new_cam_ip: "rtsp://192.168.1.205:554/live/ch0".to_string(),
            new_cam_res: "3840x2160 (4K UHD)".to_string(),
            new_cam_fps: 30,
            new_cam_codec: "H.265 / HEVC".to_string(),
            new_cam_rec: true,

            show_add_server_modal: false,
            new_server_name: "Recording Server 04 - Logistics Hub".to_string(),
            new_server_ip: "192.168.1.104".to_string(),
            new_server_port: 7563,
            new_server_storage_tb: 12.0,

            show_add_hardware_wizard: false,
            new_hw_name: "Alcon 16-Channel 4K NVR Controller".to_string(),
            new_hw_ip: "10.40.56.210".to_string(),
            new_hw_model: "Alcon Ultra HD NVR Pro".to_string(),
            new_hw_driver: 1045,
            new_hw_channels: 16,

            show_rename_server_modal: false,
            rename_target_id: "a1b2c3d4-0001-0000-0000-000000000001".to_string(),
            rename_target_name: "JSPANGICCCREC01.JSPLICCC.LOCAL".to_string(),

            show_change_pwd_modal: false,
            new_hardware_pwd: String::new(),
            confirm_hardware_pwd: String::new(),

            show_move_hardware_modal: false,

            selected_hardware_id: "h1a2b3c4-0001-0000-0000-000000000001".to_string(),
            selected_hardware_name: "AIRTEL TOWER SIDE WALL CAM 2".to_string(),
            selected_hardware_model: "HikVisionGeneric".to_string(),
            selected_hardware_serial: "ID0602101001991202530060".to_string(),
            selected_hardware_driver: "Hikvision Generic Native Driver".to_string(),
            selected_hardware_addr: "http://10.40.56.203/".to_string(),
            selected_hardware_mac: "3C:DA:6D:C7:81:FD".to_string(),
            selected_hardware_fw: "V5.5.80_build180911".to_string(),
            selected_hardware_enabled: true,
            selected_preview_camera_name: "AIRTEL TOWER SIDE WALL CAM 2 (LIVE)".to_string(),

            current_camera_settings: initial_cam,
            camera_settings_db: HashMap::new(),
            save_feedback_time: None,

            show_edit_hardware_modal: false,
            show_update_fw_modal: false,
            new_firmware_ver: "V5.6.0_build20250101".to_string(),
            show_rename_hw_modal: false,
            rename_hw_text: "AIRTEL TOWER SIDE WALL CAM 2".to_string(),

            groups_hierarchy: default_hierarchy,
            editing_folder_original_name: None,
            inline_folder_text: String::new(),
            editing_camera_original_name: None,
            inline_camera_text: String::new(),

            selected_device_group: "South Block".to_string(),
            show_add_device_group_modal: false,
            new_device_group_name: "New Camera Zone".to_string(),
            show_rename_device_group_modal: false,
            rename_device_group_name: "ROAD SAFETY".to_string(),

            show_edit_members_modal: false,
            edit_members_group_name: "ROAD SAFETY".to_string(),
            edit_members_available_cams: vec![
                "SPEED CAM 01 - NORTH ROAD".to_string(),
                "SPEED CAM 02 - JUNCTION".to_string(),
                "ANPR HIGHWAY CAM 01".to_string(),
                "ANPR HIGHWAY CAM 02".to_string(),
                "MAIN GATE ENTRY CAM".to_string(),
                "B32 WALL CAM 1".to_string(),
                "B38 SIDE WALL CAM 1".to_string(),
                "SERVER ROOM FRONT CAM 1".to_string(),
            ],
            edit_members_selected_cams: vec![true, true, true, true, false, false, false, false],

            camera_tab: CameraTab::Settings,
            stream_profile: CameraStreamProfile::default(),
        }
    }
}

impl ManagementClientApp {
    pub fn refresh_all_modules(&mut self) {
        self.management_server = ManagementServer::default();
        self.identity_server = IdentityServer::default();
        self.recording_engine = VideoRecordingEngine::default();
        self.event_server = EventServer::default();
        self.mobile_gateway = MobileServerGateway::default();
        self.analytics_engine = AnalyticsEngine::default();
    }

    pub fn select_camera_node(&mut self, cam_name: &str, group_name: &str) {
        self.selected_hardware_name = cam_name.to_string();
        self.selected_preview_camera_name = format!("{} (LIVE)", cam_name);
        self.rename_hw_text = cam_name.to_string();
        self.selected_device_group = group_name.to_string();

        if let Some(saved) = self.camera_settings_db.get(cam_name) {
            self.current_camera_settings = saved.clone();
        } else {
            let hash = cam_name.bytes().fold(0u32, |acc, b| acc.wrapping_add(b as u32));
            let port1 = 8860 + (hash % 10) as u16;
            let port2 = 8870 + (hash % 10) as u16;
            let is_4k = cam_name.contains("B32") || cam_name.contains("B38") || cam_name.contains("B58") || cam_name.contains("B80") || cam_name.contains("PTZ");

            let info = CameraNodeInfo {
                name: cam_name.to_string(),
                group: group_name.to_string(),
                resolution: if is_4k { "2688x1520 (4MP QHD)".to_string() } else { "1920x1080 (1080p FHD)".to_string() },
                codec: if hash % 2 == 0 { "H.265".to_string() } else { "H.264".to_string() },
                fps: 25,
                bitrate_mode: "Variable bit rate".to_string(),
                quality: 60 + (hash % 20),
                port1,
                port2,
                stream2_res: "640x360".to_string(),
                stream2_fps: 25,
                brightness: 50 + ((hash % 11) as i32 - 5),
                contrast: 50 + ((hash % 7) as i32 - 3),
                saturation: 50,
                sharpness: 50,
                multicast_ip: "0.0.0.0".to_string(),
                streaming_method: "RTP/RTSP/TCP".to_string(),
            };
            self.camera_settings_db.insert(cam_name.to_string(), info.clone());
            self.current_camera_settings = info;
        }

        let hash = cam_name.bytes().fold(0u32, |acc, b| acc.wrapping_add(b as u32));
        self.selected_hardware_id = format!("h-{}", hash);
        self.selected_hardware_model = "HikVisionGeneric".to_string();
        self.selected_hardware_serial = format!("DS-2CD2043G0-I-SN{:08X}", hash);
        self.selected_hardware_driver = "Hikvision Native Generic Driver".to_string();
        self.selected_hardware_addr = format!("http://10.40.56.{}/", 100 + (hash % 150));
        self.selected_hardware_mac = format!("3C:DA:6D:{:02X}:{:02X}:{:02X}", (hash >> 16) & 0xFF, (hash >> 8) & 0xFF, hash & 0xFF);
        self.selected_hardware_fw = "V5.5.80_build180911".to_string();
        self.selected_hardware_enabled = true;
    }

    pub fn save_camera_properties(&mut self) {
        let name = self.current_camera_settings.name.clone();
        self.camera_settings_db.insert(name.clone(), self.current_camera_settings.clone());
        self.save_feedback_time = Some(Instant::now());
    }

    // Rename camera from right panel name input
    pub fn update_camera_name_from_properties(&mut self, new_name: String) {
        let old_name = self.selected_hardware_name.clone();
        if !new_name.is_empty() && new_name != old_name {
            for (_, cams) in &mut self.groups_hierarchy {
                for cam in cams {
                    if *cam == old_name {
                        *cam = new_name.clone();
                        break;
                    }
                }
            }
            if let Some(mut settings) = self.camera_settings_db.remove(&old_name) {
                settings.name = new_name.clone();
                self.camera_settings_db.insert(new_name.clone(), settings);
            }
            self.selected_hardware_name = new_name.clone();
            self.current_camera_settings.name = new_name.clone();
            self.selected_preview_camera_name = format!("{} (LIVE)", new_name);
        }
    }

    // Inline Folder Editing
    pub fn start_inline_folder_rename(&mut self, folder_name: &str) {
        self.editing_folder_original_name = Some(folder_name.to_string());
        self.inline_folder_text = folder_name.to_string();
    }

    pub fn commit_inline_folder_rename(&mut self) {
        if let Some(orig) = self.editing_folder_original_name.take() {
            let new_name = self.inline_folder_text.trim().to_string();
            if !new_name.is_empty() && new_name != orig {
                for (grp, _) in &mut self.groups_hierarchy {
                    if *grp == orig {
                        *grp = new_name.clone();
                        break;
                    }
                }
                let _ = self.management_server.rename_device_group("g-09", &new_name);
                self.selected_device_group = new_name;
            }
        }
    }

    pub fn cancel_inline_folder_rename(&mut self) {
        self.editing_folder_original_name = None;
        self.inline_folder_text.clear();
    }

    // Inline Camera Editing
    pub fn start_inline_camera_rename(&mut self, cam_name: &str) {
        self.editing_camera_original_name = Some(cam_name.to_string());
        self.inline_camera_text = cam_name.to_string();
    }

    pub fn commit_inline_camera_rename(&mut self) {
        if let Some(orig) = self.editing_camera_original_name.take() {
            let new_name = self.inline_camera_text.trim().to_string();
            if !new_name.is_empty() && new_name != orig {
                for (_, cams) in &mut self.groups_hierarchy {
                    for cam in cams {
                        if *cam == orig {
                            *cam = new_name.clone();
                            break;
                        }
                    }
                }
                if let Some(mut settings) = self.camera_settings_db.remove(&orig) {
                    settings.name = new_name.clone();
                    self.camera_settings_db.insert(new_name.clone(), settings);
                }
                self.selected_hardware_name = new_name.clone();
                self.current_camera_settings.name = new_name.clone();
                self.selected_preview_camera_name = format!("{} (LIVE)", new_name);
            }
        }
    }

    pub fn cancel_inline_camera_rename(&mut self) {
        self.editing_camera_original_name = None;
        self.inline_camera_text.clear();
    }

    pub fn open_edit_members_modal(&mut self, group_name: &str) {
        self.edit_members_group_name = group_name.to_string();
        self.show_edit_members_modal = true;
    }

    pub fn add_device_group_from_modal(&mut self) {
        let _ = self.management_server.add_device_group(&self.new_device_group_name);
        self.groups_hierarchy.push((self.new_device_group_name.clone(), vec![
            format!("{} CAM 01", self.new_device_group_name),
            format!("{} CAM 02", self.new_device_group_name),
        ]));
        self.selected_device_group = self.new_device_group_name.clone();
        self.show_add_device_group_modal = false;
    }

    pub fn rename_device_group_from_modal(&mut self) {
        let _ = self.management_server.rename_device_group("g-09", &self.rename_device_group_name);
        for (grp, _) in &mut self.groups_hierarchy {
            if *grp == self.selected_device_group {
                *grp = self.rename_device_group_name.clone();
                break;
            }
        }
        self.selected_device_group = self.rename_device_group_name.clone();
        self.show_rename_device_group_modal = false;
    }

    pub fn delete_selected_device_group(&mut self, group_name: &str) {
        let _ = self.management_server.delete_device_group(group_name);
        self.groups_hierarchy.retain(|(g, _)| g != group_name);
    }

    pub fn add_camera_from_modal(&mut self) {
        let new_id = format!("c1a2b3c4-000{}-0000-0000-000000000000", self.management_server.list_cameras().len() + 1);
        let cam = CameraDevice {
            device_id: new_id.clone(),
            hardware_id: "h1a2b3c4-0001-0000-0000-000000000001".to_string(),
            name: self.new_cam_name.clone(),
            address: self.new_cam_ip.clone(),
            is_enabled: true,
            is_recording: self.new_cam_rec,
            resolution: self.new_cam_res.clone(),
            fps: self.new_cam_fps,
            codec: self.new_cam_codec.clone(),
        };

        if self.new_cam_rec {
            let _ = self.recording_engine.start_recording(&new_id);
        }

        let _ = self.management_server.add_camera(cam);
        self.show_add_camera_modal = false;
    }

    pub fn add_hardware_from_modal(&mut self) {
        let hw_id = format!("h1a2b3c4-000{}-0000-0000-000000000000", self.management_server.list_hardwares().len() + 1);
        let hw = HardwareUnit {
            hardware_id: hw_id.clone(),
            name: self.new_hw_name.clone(),
            ip_address: self.new_hw_ip.clone(),
            mac_address: "00:1A:2B:AA:BB:CC".to_string(),
            driver_number: self.new_hw_driver,
            driver_name: "ONVIF Conformance Device".to_string(),
            model: self.new_hw_model.clone(),
            serial_number: "ID0602101001991202530060".to_string(),
            firmware_version: "ONVIF_V3.3.0.20220325".to_string(),
            is_enabled: true,
            recording_server_id: "a1b2c3d4-0001-0000-0000-000000000001".to_string(),
        };

        let mut cams = Vec::new();
        for i in 1..=self.new_hw_channels {
            cams.push(CameraDevice {
                device_id: format!("{}-cam-{:02}", hw_id, i),
                hardware_id: hw_id.clone(),
                name: format!("{} - Cam {:02}", self.new_hw_name, i),
                address: format!("rtsp://{}:554/ch{:02}/main", self.new_hw_ip, i),
                is_enabled: true,
                is_recording: true,
                resolution: "1920x1080 (1080p FHD)".to_string(),
                fps: 25,
                codec: "H.265 / HEVC".to_string(),
            });
        }

        let _ = self.management_server.add_hardware(hw, cams);
        self.show_add_hardware_wizard = false;
    }

    pub fn add_server_from_modal(&mut self) {
        let new_id = format!("a1b2c3d4-000{}-0000-0000-000000000000", self.management_server.list_recording_servers().len() + 1);
        let server = RecordingServerNode {
            server_id: new_id,
            name: self.new_server_name.clone(),
            hostname: self.new_server_ip.clone(),
            port: self.new_server_port,
            is_online: true,
            hardware_count: 8,
            camera_count: 32,
            storage_used_bytes: 500_000_000_000,
            storage_total_bytes: (self.new_server_storage_tb * 1e12) as u64,
        };

        let _ = self.management_server.add_recording_server(server);
        self.show_add_server_modal = false;
    }

    pub fn apply_rename_server(&mut self) {
        let _ = self.management_server.rename_recording_server(&self.rename_target_id, &self.rename_target_name);
        self.show_rename_server_modal = false;
    }

    pub fn apply_change_pwd(&mut self) {
        let _ = self.management_server.change_hardware_password(&self.selected_hardware_id, &self.new_hardware_pwd);
        self.show_change_pwd_modal = false;
    }

    pub fn apply_edit_hardware(&mut self) {
        let _ = self.management_server.edit_hardware(
            &self.selected_hardware_id,
            &self.selected_hardware_name,
            &self.selected_hardware_addr,
            &self.selected_hardware_model,
        );
        self.show_edit_hardware_modal = false;
    }

    pub fn apply_update_firmware(&mut self) {
        let _ = self.management_server.update_firmware(&self.selected_hardware_id, &self.new_firmware_ver);
        self.selected_hardware_fw = self.new_firmware_ver.clone();
        self.show_update_fw_modal = false;
    }

    pub fn apply_rename_hardware(&mut self) {
        let _ = self.management_server.rename_hardware(&self.selected_hardware_id, &self.rename_hw_text);
        self.selected_hardware_name = self.rename_hw_text.clone();
        self.show_rename_hw_modal = false;
    }

    pub fn toggle_hardware_enabled(&mut self) {
        self.selected_hardware_enabled = !self.selected_hardware_enabled;
        let _ = self.management_server.set_hardware_enabled(&self.selected_hardware_id, self.selected_hardware_enabled);
    }
}

fn get_initial_hierarchy() -> Vec<(String, Vec<String>)> {
    vec![
        ("Camera Group 1".to_string(), vec![
            "MAIN GATE ENTRY CAM".to_string(),
            "MAIN GATE EXIT CAM".to_string(),
            "VISITOR LOBBY CAM 01".to_string(),
            "PARKING NORTH CAM 01".to_string(),
        ]),
        ("ROAD SAFETY".to_string(), vec![
            "SPEED CAM 01 - NORTH ROAD".to_string(),
            "SPEED CAM 02 - JUNCTION".to_string(),
            "ANPR HIGHWAY CAM 01".to_string(),
            "ANPR HIGHWAY CAM 02".to_string(),
        ]),
        ("SECTOR-1".to_string(), vec![
            "SEC1 BOUNDARY CAM 1".to_string(),
            "SEC1 BOUNDARY CAM 2".to_string(),
            "SEC1 SUBSTATION CAM".to_string(),
            "SEC1 TOWER CAM".to_string(),
        ]),
        ("SECTOR-2".to_string(), vec![
            "SEC2 CONVEYER CAM 1".to_string(),
            "SEC2 CONVEYER CAM 2".to_string(),
            "SEC2 PUMP HOUSE CAM".to_string(),
        ]),
        ("SECTOR-3".to_string(), vec![
            "SEC3 HOPPER CAM 1".to_string(),
            "SEC3 CRUSHER CAM 1".to_string(),
            "SEC3 LOADING AREA CAM".to_string(),
        ]),
        ("SECTOR-4".to_string(), vec![
            "SEC4 WAREHOUSE EAST".to_string(),
            "SEC4 WAREHOUSE WEST".to_string(),
            "SEC4 DISPATCH CAM".to_string(),
        ]),
        ("SECTOR - UTKAL -C- MINES".to_string(), vec![
            "MINES PIT CAM 01".to_string(),
            "MINES PIT CAM 02".to_string(),
            "HAUL ROAD CAM 01".to_string(),
            "WEIGHBRIDGE CAM 01".to_string(),
        ]),
        ("Sharmik Vihar".to_string(), vec![
            "SHARMIK VIHAR GATE CAM".to_string(),
            "SHARMIK VIHAR RESIDENTIAL 01".to_string(),
            "SHARMIK VIHAR CLUB CAM".to_string(),
        ]),
        ("South Block".to_string(), vec![
            "AIRTEL TOWER SIDE WALL CAM 2".to_string(),
            "AIRTEL TOWER SIDE WALL CAM 1".to_string(),
            "B32 WALL CAM 1".to_string(),
            "B32 WALL CAM 2".to_string(),
            "B38 SIDE WALL CAM 1".to_string(),
            "B38 SIDE WALL CAM 2".to_string(),
            "B47 WALL CAMERA 1".to_string(),
            "B47 WALL CAMERA 2".to_string(),
            "B47 WALL CAMERA 3".to_string(),
            "B53 SIDE WALL CAM 1".to_string(),
            "B58 WALL CAM 1".to_string(),
            "B58 WALL CAM 2".to_string(),
            "B65 FRONT WALL CAM 1".to_string(),
            "B65 FRONT WALL CAM 2".to_string(),
            "B69 FRONT WALL CAM".to_string(),
            "B73 SIDE WALL CAM 1".to_string(),
            "B73 SIDE WALL CAM 2".to_string(),
            "B75 BACK WALL CAM 1".to_string(),
            "B77 BACK WALL CAM".to_string(),
            "B79 SIDE WALL CAM 1".to_string(),
            "B79 SIDE WALL CAM 2".to_string(),
            "B80 SIDE WALL CAM 1".to_string(),
            "B80 SIDE WALL CAM 2".to_string(),
            "EP-02 cam-1".to_string(),
            "SERVER ROOM FRONT CAM 1".to_string(),
            "SERVER ROOM FRONT CAM 2".to_string(),
            "SERVER ROOM FRONT CAM 3".to_string(),
        ]),
        ("Universal".to_string(), vec![
            "UNIVERSAL PTZ 01 (360 DEG)".to_string(),
            "UNIVERSAL THERMAL CAM 01".to_string(),
            "PANORAMIC MULTI-SENSOR CAM".to_string(),
        ]),
    ]
}
