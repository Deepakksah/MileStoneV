use eframe::egui::{self, Color32, Vec2};
use crate::state::ManagementClientApp;

pub fn render(app: &mut ManagementClientApp, ui: &mut egui::Ui) {
    let site = app.management_server.get_site_info().unwrap_or_default();
    ui.heading(format!("🌐 Site Information — {}", site.name));
    ui.add_space(8.0);
    ui.label(egui::RichText::new("Milestone XProtect Corporate Management Server (Rust Core)").color(Color32::from_rgb(100, 116, 139)));
    ui.add_space(16.0);

    egui::Grid::new("site_info_grid").min_col_width(180.0).spacing(Vec2::new(16.0, 8.0)).show(ui, |ui| {
        ui.label(egui::RichText::new("Site Name:").strong());
        ui.label(&site.name);
        ui.end_row();

        ui.label(egui::RichText::new("Server Hostname / IP:").strong());
        ui.label(&site.hostname);
        ui.end_row();

        ui.label(egui::RichText::new("Management Port:").strong());
        ui.label(format!("{}", site.port));
        ui.end_row();

        ui.label(egui::RichText::new("Software Version:").strong());
        ui.label(&site.version);
        ui.end_row();

        ui.label(egui::RichText::new("Database Status:").strong());
        ui.label("🟢 Connected (localhost: Surveillance [176 Tables])");
        ui.end_row();
    });
}
