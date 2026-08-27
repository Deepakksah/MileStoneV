use eframe::egui::{self, Color32};
use crate::state::ManagementClientApp;

pub fn render(app: &mut ManagementClientApp, ui: &mut egui::Ui) {
    let events = app.event_server.list_recent_events();
    ui.heading(format!("📜 System Events & Audit Trail ({})", events.len()));
    ui.add_space(8.0);

    let header_bg = Color32::from_rgb(46, 117, 182);
    egui::Grid::new("server_logs_grid").striped(true).min_col_width(130.0).show(ui, |ui| {
        let th = |ui: &mut egui::Ui, text: &str| {
            ui.colored_label(Color32::WHITE, egui::RichText::new(text).strong().size(11.0).background_color(header_bg));
        };
        th(ui, " Event ID ");
        th(ui, " Source Device ");
        th(ui, " Event Type ");
        th(ui, " Message / Details ");
        ui.end_row();

        for e in events {
            ui.label(&e.event_id);
            ui.label(egui::RichText::new(&e.source_name).strong());
            ui.label(&e.event_type);
            ui.label(&e.message);
            ui.end_row();
        }
    });
}
