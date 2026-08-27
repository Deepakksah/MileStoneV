use eframe::egui::{self, Color32};
use crate::state::ManagementClientApp;

pub fn render_roles(_app: &mut ManagementClientApp, ui: &mut egui::Ui) {
    ui.heading("👥 Roles & Security Permissions");
    ui.add_space(8.0);

    let header_bg = Color32::from_rgb(46, 117, 182);
    egui::Grid::new("roles_grid").striped(true).min_col_width(150.0).show(ui, |ui| {
        let th = |ui: &mut egui::Ui, text: &str| {
            ui.colored_label(Color32::WHITE, egui::RichText::new(text).strong().size(11.0).background_color(header_bg));
        };
        th(ui, " Role Name ");
        th(ui, " Description ");
        th(ui, " Administrator ");
        th(ui, " Members Count ");
        ui.end_row();

        ui.label(egui::RichText::new("Administrators").strong());
        ui.label("Full configuration and management privileges");
        ui.label("🛡️ Yes");
        ui.label("3 users");
        ui.end_row();

        ui.label(egui::RichText::new("Security Operators").strong());
        ui.label("Live viewing, PTZ control, alarm acknowledgment");
        ui.label("No");
        ui.label("14 users");
        ui.end_row();
    });
}

pub fn render_basic_users(app: &mut ManagementClientApp, ui: &mut egui::Ui) {
    let users = app.identity_server.list_users();
    ui.heading(format!("👤 Basic Users ({})", users.len()));
    ui.add_space(8.0);

    let header_bg = Color32::from_rgb(46, 117, 182);
    egui::Grid::new("basic_users_grid").striped(true).min_col_width(130.0).show(ui, |ui| {
        let th = |ui: &mut egui::Ui, text: &str| {
            ui.colored_label(Color32::WHITE, egui::RichText::new(text).strong().size(11.0).background_color(header_bg));
        };
        th(ui, " User ID ");
        th(ui, " Username ");
        th(ui, " Display Name ");
        th(ui, " Email ");
        th(ui, " Status ");
        ui.end_row();

        for u in users {
            ui.label(&u.user_id);
            ui.label(egui::RichText::new(&u.username).strong());
            ui.label(&u.display_name);
            ui.label(&u.email);
            ui.label(if u.is_enabled { egui::RichText::new("🟢 Active").color(Color32::from_rgb(5, 150, 105)) } else { egui::RichText::new("Disabled").color(Color32::GRAY) });
            ui.end_row();
        }
    });
}
