use chrono::Utc;
use serde::{Deserialize, Serialize};

#[derive(Debug, Clone, Serialize, Deserialize)]
pub enum PtzDirection {
    Up,
    Down,
    Left,
    Right,
    ZoomIn,
    ZoomOut,
    Stop,
}

#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct MobileClientSession {
    pub session_id: String,
    pub user_id: String,
    pub client_platform: String, // "iOS", "Android", "WebSmartClient"
    pub connected_at: String,
    pub active_stream_count: u32,
}

pub struct MobileServerGateway {
    active_sessions: Vec<MobileClientSession>,
    server_port: u16,
}

impl Default for MobileServerGateway {
    fn default() -> Self {
        Self {
            active_sessions: vec![
                MobileClientSession {
                    session_id: "mob-sess-001".to_string(),
                    user_id: "u-0001-admin".to_string(),
                    client_platform: "WebSmartClient".to_string(),
                    connected_at: Utc::now().to_rfc3339(),
                    active_stream_count: 4,
                },
            ],
            server_port: 8081,
        }
    }
}

impl MobileServerGateway {
    pub fn get_port(&self) -> u16 {
        self.server_port
    }

    pub fn list_sessions(&self) -> &[MobileClientSession] {
        &self.active_sessions
    }
}
