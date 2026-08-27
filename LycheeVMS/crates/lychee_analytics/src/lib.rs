use chrono::Utc;
use serde::{Deserialize, Serialize};

#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct LicensePlateRecord {
    pub record_id: String,
    pub camera_id: String,
    pub camera_name: String,
    pub plate_text: String,
    pub confidence: f32,
    pub vehicle_type: String,
    pub timestamp: String,
    pub snapshot_path: Option<String>,
}

#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct IncidentReport {
    pub incident_id: String,
    pub title: String,
    pub description: String,
    pub operator_name: String,
    pub severity: String,
    pub status: String,
    pub created_at: String,
    pub associated_cameras: Vec<String>,
}

pub struct AnalyticsEngine {
    lpr_records: Vec<LicensePlateRecord>,
    incidents: Vec<IncidentReport>,
}

impl Default for AnalyticsEngine {
    fn default() -> Self {
        Self {
            lpr_records: vec![
                LicensePlateRecord {
                    record_id: "lpr-001".to_string(),
                    camera_id: "c1a2b3c4-0001-0000-0000-000000000001".to_string(),
                    camera_name: "Main Gate - ANPR Entrance 01".to_string(),
                    plate_text: "OD-02-AK-9921".to_string(),
                    confidence: 0.984,
                    vehicle_type: "Commercial Truck / Heavy Vehicle".to_string(),
                    timestamp: Utc::now().to_rfc3339(),
                    snapshot_path: Some("snapshots/lpr_001.jpg".to_string()),
                },
                LicensePlateRecord {
                    record_id: "lpr-002".to_string(),
                    camera_id: "c1a2b3c4-0002-0000-0000-000000000002".to_string(),
                    camera_name: "Main Gate - ANPR Exit 02".to_string(),
                    plate_text: "DL-10-CA-4028".to_string(),
                    confidence: 0.991,
                    vehicle_type: "Sedan / Passenger Car".to_string(),
                    timestamp: Utc::now().to_rfc3339(),
                    snapshot_path: Some("snapshots/lpr_002.jpg".to_string()),
                },
            ],
            incidents: vec![
                IncidentReport {
                    incident_id: "inc-2025-001".to_string(),
                    title: "Perimeter Motion Alert Review".to_string(),
                    description: "Verified minor perimeter movement near North-East fence".to_string(),
                    operator_name: "admin".to_string(),
                    severity: "Low".to_string(),
                    status: "Closed / Resolved".to_string(),
                    created_at: Utc::now().to_rfc3339(),
                    associated_cameras: vec!["c1a2b3c4-0003-0000-0000-000000000003".to_string()],
                },
            ],
        }
    }
}

impl AnalyticsEngine {
    pub fn list_lpr_records(&self) -> &[LicensePlateRecord] {
        &self.lpr_records
    }

    pub fn list_incidents(&self) -> &[IncidentReport] {
        &self.incidents
    }
}
