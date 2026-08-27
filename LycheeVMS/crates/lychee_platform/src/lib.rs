use lychee_common::VmsResult;
use serde::{Deserialize, Serialize};

#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct VideoFrame {
    pub stream_id: String,
    pub timestamp: String,
    pub sequence_number: u64,
    pub is_keyframe: bool,
    pub width: u32,
    pub height: u32,
    pub payload_size: usize,
}

#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct StorageRetentionPolicy {
    pub storage_path: String,
    pub max_retention_days: u32,
    pub min_free_space_gb: u32,
    pub current_used_gb: f64,
    pub total_capacity_gb: f64,
}

pub struct VideoRecordingEngine {
    active_recordings: Vec<String>,
    retention_policy: StorageRetentionPolicy,
}

impl Default for VideoRecordingEngine {
    fn default() -> Self {
        Self {
            active_recordings: vec![
                "c1a2b3c4-0001-0000-0000-000000000001".to_string(),
                "c1a2b3c4-0002-0000-0000-000000000002".to_string(),
                "c1a2b3c4-0003-0000-0000-000000000003".to_string(),
            ],
            retention_policy: StorageRetentionPolicy {
                storage_path: "D:\\MilestoneMediaStorage\\Recordings".to_string(),
                max_retention_days: 90,
                min_free_space_gb: 500,
                current_used_gb: 14250.0,
                total_capacity_gb: 20000.0,
            },
        }
    }
}

impl VideoRecordingEngine {
    /// Start recording for a camera
    pub fn start_recording(&mut self, camera_id: &str) -> VmsResult<()> {
        if !self.active_recordings.contains(&camera_id.to_string()) {
            self.active_recordings.push(camera_id.to_string());
        }
        Ok(())
    }

    /// Stop recording for a camera
    pub fn stop_recording(&mut self, camera_id: &str) -> VmsResult<()> {
        self.active_recordings.retain(|id| id != camera_id);
        Ok(())
    }

    /// Check if camera is currently recording
    pub fn is_recording(&self, camera_id: &str) -> bool {
        self.active_recordings.contains(&camera_id.to_string())
    }

    /// Get Storage Retention Status
    pub fn get_storage_status(&self) -> &StorageRetentionPolicy {
        &self.retention_policy
    }
}
