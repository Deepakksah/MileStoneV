use eframe::egui::{self, Color32};
use crate::state::ManagementClientApp;

pub fn render(app: &mut ManagementClientApp, ui: &mut egui::Ui) {
    ui.heading("📱 Mobile Server Gateway & WebRTC Streaming");
    ui.add_space(8.0);
    ui.label(format!("Listening Port: {}", app.mobile_gateway.get_port()));
    ui.add_space(12.0);

    let sessions = app.mobile_gateway.list_sessions();
    ui.label(egui::RichText::new(format!("Active Mobile & Web Sessions ({})", sessions.len())).strong());
    ui.add_space(4.0);

    let header_bg = Color32::from_rgb(46, 117, 182);
    egui::Grid::new("mobile_sessions_grid").striped(true).min_col_width(140.0).show(ui, |ui| {
        let th = |ui: &mut egui::Ui, text: &str| {
            ui.colored_label(Color32::WHITE, egui::RichText::new(text).strong().size(11.0).background_color(header_bg));
        };
        th(ui, " Session ID ");
        th(ui, " User ID ");
        th(ui, " Client Type ");
        th(ui, " Active Video Streams ");
        ui.end_row();

        for sess in sessions {
            ui.label(&sess.session_id);
            ui.label(&sess.user_id);
            ui.label(&sess.client_platform);
            ui.label(format!("{} streams (WebRTC / HLS)", sess.active_stream_count));
            ui.end_row();
        }
    });
}
