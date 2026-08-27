use eframe::egui::{self, Color32, Pos2, Rounding, Stroke, Vec2};
use crate::state::ManagementClientApp;

pub fn render(app: &mut ManagementClientApp, ui: &mut egui::Ui) {
    egui::ScrollArea::vertical().show(ui, |ui| {
        ui.style_mut().spacing.item_spacing = Vec2::new(0.0, 10.0);

        // Top Banner
        let banner_height = 56.0;
        let banner_rect = ui.allocate_exact_size(Vec2::new(ui.available_width(), banner_height), egui::Sense::hover()).0;
        let painter = ui.painter();
        let grad_left = Color32::from_rgb(0, 138, 206);
        painter.rect_filled(banner_rect, Rounding::ZERO, grad_left);

        let right_x = banner_rect.max.x;
        let line_color = Color32::from_rgba_unmultiplied(255, 255, 255, 45);
        painter.line_segment([Pos2::new(right_x - 120.0, banner_rect.min.y), Pos2::new(right_x - 40.0, banner_rect.max.y)], Stroke::new(1.0_f32, line_color));
        painter.line_segment([Pos2::new(right_x - 40.0, banner_rect.max.y), Pos2::new(right_x, banner_rect.min.y + 15.0)], Stroke::new(1.0_f32, line_color));
        painter.line_segment([Pos2::new(right_x - 120.0, banner_rect.min.y), Pos2::new(right_x, banner_rect.min.y + 15.0)], Stroke::new(1.0_f32, line_color));

        painter.text(
            Pos2::new(banner_rect.min.x + 20.0, banner_rect.center().y),
            egui::Align2::LEFT_CENTER,
            "◆ milestone | XProtect®",
            egui::FontId::proportional(19.0),
            Color32::WHITE,
        );

        ui.add_space(8.0);

        // Licensed to & Milestone Care
        ui.horizontal(|ui| {
            ui.horizontal(|ui| {
                ui.set_min_width(380.0);
                let seal_rect = ui.allocate_exact_size(Vec2::new(34.0, 34.0), egui::Sense::hover()).0;
                ui.painter().rect_filled(seal_rect, Rounding::same(17.0), Color32::from_rgb(220, 38, 38));
                ui.painter().rect_stroke(seal_rect, Rounding::same(17.0), Stroke::new(1.5_f32, Color32::from_rgb(251, 191, 36)));
                ui.painter().text(seal_rect.center(), egui::Align2::CENTER_CENTER, "🏅", egui::FontId::proportional(16.0), Color32::WHITE);

                ui.add_space(8.0);
                ui.vertical(|ui| {
                    ui.label(egui::RichText::new("Licensed to:").strong().size(12.0).color(Color32::from_rgb(15, 23, 42)));
                    ui.label(egui::RichText::new("JINDAL STEEL POWER LIMITED").size(11.0).color(Color32::from_rgb(30, 41, 59)));
                    ui.label(egui::RichText::new("AT - NISHA JINDAL NAGAR").size(11.0).color(Color32::from_rgb(30, 41, 59)));
                    ui.label(egui::RichText::new("759122 Angul").size(11.0).color(Color32::from_rgb(30, 41, 59)));
                    ui.label(egui::RichText::new("Odisha India").size(11.0).color(Color32::from_rgb(30, 41, 59)));
                    ui.add_space(4.0);
                    ui.hyperlink_to(egui::RichText::new("End-user license agreement").size(10.5).underline().color(Color32::from_rgb(0, 102, 204)), "https://www.milestonesys.com");
                });
            });

            ui.vertical(|ui| {
                ui.label(egui::RichText::new("Milestone Care").strong().size(12.0).color(Color32::from_rgb(15, 23, 42)));
                ui.horizontal(|ui| {
                    ui.label(egui::RichText::new("Your current level:").size(11.0).color(Color32::from_rgb(30, 41, 59)));
                    ui.label(egui::RichText::new(" Plus ").strong().size(11.0).color(Color32::from_rgb(30, 41, 59)).background_color(Color32::from_rgb(254, 240, 138)));
                });
                ui.add_space(8.0);
                ui.hyperlink_to(egui::RichText::new("Access Milestone Care portal...").size(10.5).underline().color(Color32::from_rgb(0, 102, 204)), "https://www.milestonesys.com");
                ui.hyperlink_to(egui::RichText::new("Information about Milestone Care...").size(10.5).underline().color(Color32::from_rgb(0, 102, 204)), "https://www.milestonesys.com");
            });
        });

        ui.add_space(12.0);

        // Installed Products Table
        ui.label(egui::RichText::new("Installed Products").strong().size(11.5).color(Color32::from_rgb(15, 23, 42)));
        ui.add_space(2.0);

        let header_bg = Color32::from_rgb(46, 117, 182);
        let border_color = Color32::from_rgb(203, 213, 225);

        egui::Frame::none().stroke(Stroke::new(1.0_f32, border_color)).show(ui, |ui| {
            ui.set_width(ui.available_width());
            egui::Grid::new("installed_products_grid").striped(true).min_col_width(130.0).show(ui, |ui| {
                let th = |ui: &mut egui::Ui, text: &str| {
                    ui.colored_label(Color32::WHITE, egui::RichText::new(text).strong().size(11.0).background_color(header_bg));
                };
                th(ui, " Product Version ");
                th(ui, " Software License Code ");
                th(ui, " Expiration Date ");
                th(ui, " Milestone Care Plus ");
                th(ui, " Milestone Care Premium ");
                ui.end_row();

                ui.label(egui::RichText::new("XProtect Expert 2025 R2").size(11.0));
                ui.label(egui::RichText::new("M01-C02-252-01-6C43F7").size(11.0).monospace());
                ui.label(egui::RichText::new("Unrestricted").size(11.0));
                ui.label(egui::RichText::new("01-03-2029").size(11.0));
                ui.label(egui::RichText::new("N/A").size(11.0));
                ui.end_row();

                ui.label(egui::RichText::new("XProtect Access 2025 R2").size(11.0));
                ui.label(egui::RichText::new("N/A").size(11.0));
                ui.label(egui::RichText::new("20-02-2026").size(11.0));
                ui.label(egui::RichText::new("20-02-2026").size(11.0));
                ui.label(egui::RichText::new("N/A").size(11.0));
                ui.end_row();
            });
        });

        ui.add_space(12.0);

        // License Overview
        ui.horizontal(|ui| {
            ui.label(egui::RichText::new("License Overview - All sites").strong().size(11.5).color(Color32::from_rgb(15, 23, 42)));
            ui.add_space(40.0);
            ui.hyperlink_to(egui::RichText::new("License Details - All Sites...").size(10.5).underline().color(Color32::from_rgb(0, 102, 204)), "https://www.milestonesys.com");
        });
        ui.add_space(2.0);

        egui::Frame::none().stroke(Stroke::new(1.0_f32, border_color)).show(ui, |ui| {
            ui.set_width(320.0);
            egui::Grid::new("license_overview_grid").striped(true).min_col_width(150.0).show(ui, |ui| {
                let th = |ui: &mut egui::Ui, text: &str| {
                    ui.colored_label(Color32::WHITE, egui::RichText::new(text).strong().size(11.0).background_color(header_bg));
                };
                th(ui, " License Type ");
                th(ui, " Activated ");
                ui.end_row();

                ui.label(egui::RichText::new("Device Licenses").size(11.0));
                ui.label(egui::RichText::new("399 out of 400").size(11.0));
                ui.end_row();

                ui.label(egui::RichText::new("Access control door").size(11.0));
                ui.label(egui::RichText::new("0 out of 4").size(11.0));
                ui.end_row();
            });
        });

        ui.add_space(12.0);

        // License Details
        ui.label(egui::RichText::new("License Details - Current Site: MXM-PS-01").strong().size(11.5).color(Color32::from_rgb(15, 23, 42)));
        ui.add_space(2.0);

        egui::Frame::none().stroke(Stroke::new(1.0_f32, border_color)).show(ui, |ui| {
            ui.set_width(ui.available_width());
            egui::Grid::new("site_license_details_grid").striped(true).min_col_width(110.0).show(ui, |ui| {
                let th = |ui: &mut egui::Ui, text: &str| {
                    ui.colored_label(Color32::WHITE, egui::RichText::new(text).strong().size(11.0).background_color(header_bg));
                };
                th(ui, " License Type ");
                th(ui, " Activated ");
                th(ui, " Changes without activation ");
                th(ui, " In Grace Period ");
                th(ui, " Grace Period Expired ");
                th(ui, " Without License ");
                ui.end_row();

                ui.label(egui::RichText::new("Device Licenses").size(11.0));
                ui.label(egui::RichText::new("391").size(11.0));
                ui.label(egui::RichText::new("13 out of 59").size(11.0));
                ui.label(egui::RichText::new("0").size(11.0));
                ui.label(egui::RichText::new("0").size(11.0));
                ui.label(egui::RichText::new("0").size(11.0));
                ui.end_row();

                ui.label(egui::RichText::new("Access control door").size(11.0));
                ui.label(egui::RichText::new("0").size(11.0));
                ui.label(egui::RichText::new("N/A").size(11.0));
                ui.label(egui::RichText::new("0").size(11.0));
                ui.label(egui::RichText::new("0").size(11.0));
                ui.label(egui::RichText::new("0").size(11.0));
                ui.end_row();
            });
        });

        ui.add_space(14.0);
        ui.checkbox(&mut app.auto_activate_license, egui::RichText::new("Enable automatic license activation").size(11.0));

        ui.add_space(6.0);
        let btn_activate = egui::Button::new(egui::RichText::new("Activate License Manually...  ▾").size(11.0).color(Color32::from_rgb(15, 23, 42)))
            .fill(Color32::from_rgb(241, 245, 249))
            .stroke(Stroke::new(1.0_f32, Color32::from_rgb(203, 213, 225)))
            .rounding(Rounding::same(2.0));
        if ui.add(btn_activate).clicked() {}
    });
}
