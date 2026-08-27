use eframe::egui::{self, Color32};
use crate::state::ManagementClientApp;

pub fn render(app: &mut ManagementClientApp, ui: &mut egui::Ui) {
    let alarms = app.event_server.list_alarms();
    ui.heading(format!("🚨 Alarm Definitions ({})", alarms.len()));
    ui.add_space(8.0);

    let header_bg = Color32::from_rgb(46, 117, 182);
    egui::Grid::new("alarms_grid").striped(true).min_col_width(140.0).show(ui, |ui| {
        let th = |ui: &mut egui::Ui, text: &str| {
            ui.colored_label(Color32::WHITE, egui::RichText::new(text).strong().size(11.0).background_color(header_bg));
        };
        th(ui, " Alarm ID ");
        th(ui, " Alarm Name ");
        th(ui, " Triggered By Event ");
        th(ui, " Auto Close (min) ");
        th(ui, " Status ");
        ui.end_row();

        for a in alarms {
            ui.label(&a.alarm_id);
            ui.label(egui::RichText::new(&a.name).strong());
            ui.label(&a.triggered_by_event);
            ui.label(format!("{} min", a.auto_close_minutes));
            ui.label(if a.is_enabled { egui::RichText::new("🟢 Enabled").color(Color32::from_rgb(5, 150, 105)) } else { egui::RichText::new("Disabled").color(Color32::GRAY) });
            ui.end_row();
        }
    });
}
