use eframe::egui::{self, Vec2};
use crate::state::ManagementClientApp;

pub fn render(app: &mut ManagementClientApp, ctx: &egui::Context) {
    egui::TopBottomPanel::top("menu_bar").exact_height(24.0).show(ctx, |ui| {
        egui::menu::bar(ui, |ui| {
            ui.style_mut().spacing.item_spacing = Vec2::new(12.0, 0.0);

            ui.menu_button("File", |ui| {
                if ui.button("Save Configuration\tCtrl+S").clicked() { ui.close_menu(); }
                ui.separator();
                if ui.button("Exit").clicked() { ctx.send_viewport_cmd(egui::ViewportCommand::Close); }
            });

            ui.menu_button("View", |ui| {
                if ui.button("Refresh\tF5").clicked() {
                    app.refresh_all_modules();
                    ui.close_menu();
                }
            });

            ui.menu_button("Action", |ui| {
                if ui.button("Activate License Manually...").clicked() { ui.close_menu(); }
            });

            ui.menu_button("Maintenance", |ui| {
                if ui.button("Backup Configuration...").clicked() { ui.close_menu(); }
                if ui.button("Restore Configuration...").clicked() { ui.close_menu(); }
            });

            ui.menu_button("Tools", |ui| {
                if ui.button("Options...").clicked() { ui.close_menu(); }
            });

            ui.menu_button("Help", |ui| {
                if ui.button("About Milestone XProtect...").clicked() { ui.close_menu(); }
            });
        });
    });
}
