mod state;
mod ui;

use eframe::egui::{self, Color32, Stroke};
use state::{ManagementClientApp, SelectedNav};
use std::time::Duration;

impl eframe::App for ManagementClientApp {
    fn update(&mut self, ctx: &egui::Context, _frame: &mut eframe::Frame) {
        // Request repaint after 500ms so live clock and timestamps update smoothly without CPU spin
        ctx.request_repaint_after(Duration::from_millis(500));

        // 1. Top Menu Bar Module
        ui::menu_bar::render(self, ctx);

        // 2. Icon Toolbar Module
        ui::toolbar::render(self, ctx);

        // 3. Bottom Status Bar (Date and Local Time matching screenshot)
        egui::TopBottomPanel::bottom("milestone_status_bar")
            .exact_height(20.0)
            .show(ctx, |ui| {
                ui.horizontal(|ui| {
                    ui.label(egui::RichText::new("Ready").size(10.0).color(Color32::from_rgb(71, 85, 105)));
                    ui.with_layout(egui::Layout::right_to_left(egui::Align::Center), |ui| {
                        let time_str = chrono::Local::now().format("%a %H:%M [Local time]").to_string();
                        let date_str = chrono::Local::now().format("%d %B %Y").to_string();
                        ui.label(egui::RichText::new(time_str).size(9.5).color(Color32::from_rgb(71, 85, 105)));
                        ui.separator();
                        ui.label(egui::RichText::new(date_str).size(9.5).color(Color32::from_rgb(71, 85, 105)));
                    });
                });
            });

        // 4. Left Site Navigation Tree Module
        ui::tree_nav::render(self, ctx);

        // 5. Central Panel Modular Views
        egui::CentralPanel::default().show(ctx, |ctx_ui| {
            match self.selected_nav {
                SelectedNav::LicenseInformation => ui::views::license_view::render(self, ctx_ui),
                SelectedNav::SiteInformation => ui::views::site_view::render(self, ctx_ui),
                SelectedNav::RecordingServers => ui::views::servers_view::render(self, ctx_ui),
                SelectedNav::MobileServers => ui::views::mobile_view::render(self, ctx_ui),
                SelectedNav::Cameras => ui::views::cameras_view::render(self, ctx_ui),
                SelectedNav::Roles => ui::views::security_view::render_roles(self, ctx_ui),
                SelectedNav::BasicUsers => ui::views::security_view::render_basic_users(self, ctx_ui),
                SelectedNav::Alarms => ui::views::alarms_view::render(self, ctx_ui),
                SelectedNav::ServerLogs => ui::views::logs_view::render(self, ctx_ui),
                SelectedNav::MetadataSearch => ui::views::analytics_view::render(self, ctx_ui),
                _ => {
                    ctx_ui.heading("Milestone Management Module");
                    ctx_ui.label("Select a category from the Site Navigation Tree to manage configurations.");
                }
            }
        });
    }
}

fn main() -> eframe::Result<()> {
    let native_options = eframe::NativeOptions {
        viewport: egui::ViewportBuilder::default()
            .with_title("Milestone XProtect Management Client 2025 R2")
            .with_inner_size([1366.0, 768.0])
            .with_min_inner_size([1024.0, 640.0])
            .with_visible(true)
            .with_active(true),
        ..Default::default()
    };

    eframe::run_native(
        "Milestone XProtect Management Client 2025 R2",
        native_options,
        Box::new(|cc| {
            // Apply clean Milestone light theme ONCE at startup to prevent state resets
            let mut visuals = egui::Visuals::light();
            visuals.panel_fill = Color32::from_rgb(240, 243, 246);
            visuals.window_fill = Color32::from_rgb(255, 255, 255);
            visuals.selection.bg_fill = Color32::from_rgb(0, 120, 215);
            visuals.selection.stroke = Stroke::new(1.0_f32, Color32::from_rgb(255, 140, 0));
            cc.egui_ctx.set_visuals(visuals);

            Ok(Box::new(ManagementClientApp::default()))
        }),
    )
}
