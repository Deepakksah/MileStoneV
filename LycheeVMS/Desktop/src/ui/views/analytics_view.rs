use eframe::egui::{self, Color32};
use crate::state::ManagementClientApp;

pub fn render(app: &mut ManagementClientApp, ui: &mut egui::Ui) {
    let records = app.analytics_engine.list_lpr_records();
    ui.heading(format!("🔍 License Plate Recognition (ANPR) & Analytics ({})", records.len()));
    ui.add_space(8.0);

    let header_bg = Color32::from_rgb(46, 117, 182);
    egui::Grid::new("lpr_records_grid").striped(true).min_col_width(130.0).show(ui, |ui| {
        let th = |ui: &mut egui::Ui, text: &str| {
            ui.colored_label(Color32::WHITE, egui::RichText::new(text).strong().size(11.0).background_color(header_bg));
        };
        th(ui, " Record ID ");
        th(ui, " Camera Name ");
        th(ui, " Plate Number ");
        th(ui, " AI Confidence ");
        th(ui, " Vehicle Classification ");
        ui.end_row();

        for r in records {
            ui.label(&r.record_id);
            ui.label(egui::RichText::new(&r.camera_name).strong());
            ui.label(egui::RichText::new(&r.plate_text).monospace().strong().color(Color32::from_rgb(0, 102, 204)));
            ui.label(format!("{:.1}%", r.confidence * 100.0));
            ui.label(&r.vehicle_type);
            ui.end_row();
        }
    });
}
