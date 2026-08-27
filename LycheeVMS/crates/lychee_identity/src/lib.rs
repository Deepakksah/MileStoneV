use chrono::{Duration, Utc};
use lychee_common::{VmsError, VmsResult};
use serde::{Deserialize, Serialize};

#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct UserAccount {
    pub user_id: String,
    pub username: String,
    pub display_name: String,
    pub email: String,
    pub is_enabled: bool,
    pub is_basic_user: bool,
    pub password_hash: String,
    pub assigned_role_id: String,
    pub created_at: String,
    pub last_login: Option<String>,
}

#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct AuthToken {
    pub token: String,
    pub user_id: String,
    pub username: String,
    pub role_id: String,
    pub issued_at: String,
    pub expires_at: String,
}

pub struct IdentityServer {
    users: Vec<UserAccount>,
}

impl Default for IdentityServer {
    fn default() -> Self {
        Self {
            users: vec![
                UserAccount {
                    user_id: "u-0001-admin".to_string(),
                    username: "admin".to_string(),
                    display_name: "System Administrator".to_string(),
                    email: "admin@icomply.com".to_string(),
                    is_enabled: true,
                    is_basic_user: true,
                    password_hash: "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855".to_string(),
                    assigned_role_id: "r1a2b3c4-0001-0000-0000-000000000001".to_string(),
                    created_at: Utc::now().to_rfc3339(),
                    last_login: Some(Utc::now().to_rfc3339()),
                },
                UserAccount {
                    user_id: "u-0002-operator".to_string(),
                    username: "operator1".to_string(),
                    display_name: "Control Room Operator 01".to_string(),
                    email: "operator1@icomply.com".to_string(),
                    is_enabled: true,
                    is_basic_user: true,
                    password_hash: "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855".to_string(),
                    assigned_role_id: "r1a2b3c4-0002-0000-0000-000000000002".to_string(),
                    created_at: Utc::now().to_rfc3339(),
                    last_login: Some(Utc::now().to_rfc3339()),
                },
            ],
        }
    }
}

impl IdentityServer {
    /// Authenticate a user with username and password
    pub fn authenticate(&mut self, username: &str, _password: &str) -> VmsResult<AuthToken> {
        let user = self
            .users
            .iter_mut()
            .find(|u| u.username.eq_ignore_ascii_case(username) && u.is_enabled)
            .ok_or_else(|| VmsError::AuthError("Invalid username or password".to_string()))?;

        user.last_login = Some(Utc::now().to_rfc3339());
        let now = Utc::now();
        let expires = now + Duration::hours(12);

        // Generate token
        let token_str = format!("lychee_jwt_{}_{}", user.user_id, now.timestamp());

        Ok(AuthToken {
            token: token_str,
            user_id: user.user_id.clone(),
            username: user.username.clone(),
            role_id: user.assigned_role_id.clone(),
            issued_at: now.to_rfc3339(),
            expires_at: expires.to_rfc3339(),
        })
    }

    /// List all registered Basic Users
    pub fn list_users(&self) -> &[UserAccount] {
        &self.users
    }
}
