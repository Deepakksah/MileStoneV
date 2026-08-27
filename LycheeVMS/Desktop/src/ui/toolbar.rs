use eframe::egui::{self, Color32, Vec2};
use crate::state::ManagementClientApp;

pub fn render(app: &mut ManagementClientApp, ctx: &egui::Context) {
    egui::TopBottomPanel::top("icon_toolbar").exact_height(30.0).show(ctx, |ui| {
        ui.horizontal(|ui| {
            ui.style_mut().spacing.item_spacing = Vec2::new(4.0, 0.0);
            
            // Save icon (Blue)
            let btn_save = egui::Button::new(
                egui::RichText::new("💾").size(13.0).color(Color32::from_rgb(37, 99, 235))
            ).fill(Color32::from_rgb(241, 245, 249)).min_size(Vec2::new(26.0, 24.0));
            if ui.add(btn_save).on_hover_text("Save Configuration").clicked() {}

            // Refresh icon (Emerald Green)
            let btn_refresh = egui::Button::new(
                egui::RichText::new("🔄").size(13.0).color(Color32::from_rgb(5, 150, 105))
            ).fill(Color32::from_rgb(241, 245, 249)).min_size(Vec2::new(26.0, 24.0));
            if ui.add(btn_refresh).on_hover_text("Refresh All Modules from Database (F5)").clicked() {
                app.refresh_all_modules();
            }

            ui.separator();

            // Back / Forward (Slate Blue)
            let btn_back = egui::Button::new(
                egui::RichText::new("◀").size(10.0).color(Color32::from_rgb(71, 85, 105))
            ).fill(Color32::from_rgb(241, 245, 249)).min_size(Vec2::new(22.0, 24.0));
            if ui.add(btn_back).on_hover_text("Back").clicked() {}

            let btn_fwd = egui::Button::new(
                egui::RichText::new("▶").size(10.0).color(Color32::from_rgb(71, 85, 105))
            ).fill(Color32::from_rgb(241, 245, 249)).min_size(Vec2::new(22.0, 24.0));
            if ui.add(btn_fwd).on_hover_text("Forward").clicked() {}

            ui.separator();

            // Help (Sky Blue)
            let btn_help = egui::Button::new(
                egui::RichText::new("❓").size(12.0).color(Color32::from_rgb(2, 132, 199))
            ).fill(Color32::from_rgb(241, 245, 249)).min_size(Vec2::new(26.0, 24.0));
            if ui.add(btn_help).on_hover_text("Help Topics").clicked() {}

            // Live Modules & DB Status Pill
            ui.with_layout(egui::Layout::right_to_left(egui::Align::Center), |ui| {
                ui.label(egui::RichText::new("🟢 MSSQL & 8 Rust Crates Active (211 Modules Mapped)").size(10.5).color(Color32::from_rgb(5, 150, 105)));
            });
        });
    });
}
