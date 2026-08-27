use chrono::Utc;
use serde::{Deserialize, Serialize};

#[derive(Debug, Clone, Serialize, Deserialize)]
pub enum EventSeverity {
    Info,
    Warning,
    Critical,
    Emergency,
}

#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct SystemEvent {
    pub event_id: String,
    pub source_id: String,
    pub source_name: String,
    pub event_type: String,
    pub message: String,
    pub severity: EventSeverity,
    pub timestamp: String,
}

#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct AlarmDefinition {
    pub alarm_id: String,
    pub name: String,
    pub triggered_by_event: String,
    pub source_device_id: String,
    pub is_enabled: bool,
    pub auto_close_minutes: u32,
}

pub struct EventServer {
    alarms: Vec<AlarmDefinition>,
    recent_events: Vec<SystemEvent>,
}

impl Default for EventServer {
    fn default() -> Self {
        Self {
            alarms: vec![
                AlarmDefinition {
                    alarm_id: "alm-0001".to_string(),
                    name: "Main Gate Perimeter Intrusion".to_string(),
                    triggered_by_event: "Motion.PerimeterSectorNorth".to_string(),
                    source_device_id: "c1a2b3c4-0003-0000-0000-000000000003".to_string(),
                    is_enabled: true,
                    auto_close_minutes: 15,
                },
                AlarmDefinition {
                    alarm_id: "alm-0002".to_string(),
                    name: "Recording Server Storage Warning (90%)".to_string(),
                    triggered_by_event: "System.StorageThresholdExceeded".to_string(),
                    source_device_id: "a1b2c3d4-0001-0000-0000-000000000001".to_string(),
                    is_enabled: true,
                    auto_close_minutes: 60,
                },
            ],
            recent_events: vec![
                SystemEvent {
                    event_id: "ev-001".to_string(),
                    source_id: "c1a2b3c4-0001-0000-0000-000000000001".to_string(),
                    source_name: "Main Gate - ANPR Entrance 01".to_string(),
                    event_type: "Analytics.PlateRecognized".to_string(),
                    message: "Vehicle Plate Detected: OD-02-AK-9921".to_string(),
                    severity: EventSeverity::Info,
                    timestamp: Utc::now().to_rfc3339(),
                },
            ],
        }
    }
}

impl EventServer {
    pub fn trigger_event(&mut self, event: SystemEvent) {
        self.recent_events.push(event);
    }

    pub fn list_alarms(&self) -> &[AlarmDefinition] {
        &self.alarms
    }

    pub fn list_recent_events(&self) -> &[SystemEvent] {
        &self.recent_events
    }
}
