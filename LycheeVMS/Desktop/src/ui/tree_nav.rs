use eframe::egui::{self, Color32, Vec2};
use crate::state::{ManagementClientApp, SelectedNav};

pub fn render(app: &mut ManagementClientApp, ctx: &egui::Context) {
    egui::SidePanel::left("site_navigation_panel")
        .min_width(210.0)
        .max_width(230.0)
        .show(ctx, |ui| {
            // Header
            ui.horizontal(|ui| {
                ui.label(egui::RichText::new("Site Navigation").strong().size(11.5).color(Color32::from_rgb(30, 41, 59)));
                ui.with_layout(egui::Layout::right_to_left(egui::Align::Center), |ui| {
                    ui.label(egui::RichText::new("✕").size(10.0).color(Color32::from_rgb(100, 116, 139)));
                    ui.label(egui::RichText::new("🗖").size(10.0).color(Color32::from_rgb(100, 116, 139)));
                    ui.label(egui::RichText::new("📌").size(10.0).color(Color32::from_rgb(100, 116, 139)));
                });
            });

            ui.add_space(2.0);
            ui.separator();
            ui.add_space(2.0);

            let site_name = app.management_server.get_site_info().map(|s| s.name).unwrap_or_else(|_| "MXM-PS-01".to_string());
            let servers_count = app.management_server.list_recording_servers().len();
            let cameras_count = app.management_server.list_cameras().len();
            let alarms_count = app.event_server.list_alarms().len();
            let users_count = app.identity_server.list_users().len();
            let lpr_count = app.analytics_engine.list_lpr_records().len();

            egui::ScrollArea::vertical().show(ui, |ui| {
                ui.style_mut().spacing.item_spacing = Vec2::new(0.0, 1.0);

                // Root Node: MXM-PS-01 - (25.2a) (Azure Blue)
                let root_header = egui::CollapsingHeader::new(
                    egui::RichText::new(format!("🖥 {} - (25.2a)", site_name))
                        .strong()
                        .size(11.5)
                        .color(Color32::from_rgb(0, 102, 204))
                )
                .default_open(true);

                root_header.show(ui, |ui| {
                    
                    // 1. Basics (Warm Amber)
                    egui::CollapsingHeader::new(
                        egui::RichText::new("📁 Basics").size(11.0).color(Color32::from_rgb(217, 119, 6))
                    )
                    .default_open(true)
                    .show(ui, |ui| {
                        render_leaf(ui, app, "📄", Color32::from_rgb(37, 99, 235), "License Information", SelectedNav::LicenseInformation);
                        render_leaf(ui, app, "🌐", Color32::from_rgb(13, 148, 136), "Site Information", SelectedNav::SiteInformation);
                    });

                    // 2. Servers (Tech Indigo / Purple)
                    egui::CollapsingHeader::new(
                        egui::RichText::new("📁 Servers").size(11.0).color(Color32::from_rgb(79, 70, 229))
                    )
                    .default_open(true)
                    .show(ui, |ui| {
                        render_leaf_with_context(
                            ui, app, "🖧", Color32::from_rgb(124, 58, 237), 
                            &format!("Recording Servers ({})", servers_count), 
                            SelectedNav::RecordingServers,
                            |app, ui| {
                                if ui.button("➕ Add Recording Server...").clicked() {
                                    app.show_add_server_modal = true;
                                    ui.close_menu();
                                }
                                if ui.button("➕ Add Hardware / Camera...").clicked() {
                                    app.show_add_camera_modal = true;
                                    ui.close_menu();
                                }
                                ui.separator();
                                if ui.button("🔄 Refresh Servers").clicked() {
                                    app.refresh_all_modules();
                                    ui.close_menu();
                                }
                            }
                        );
                        render_leaf(ui, app, "🛡️", Color32::from_rgb(16, 185, 129), "Failover Servers", SelectedNav::FailoverServers);
                        render_leaf(ui, app, "📱", Color32::from_rgb(14, 165, 233), "Mobile Servers (1)", SelectedNav::MobileServers);
                    });

                    // 3. Devices (Emerald / Teal)
                    egui::CollapsingHeader::new(
                        egui::RichText::new("📁 Devices").size(11.0).color(Color32::from_rgb(5, 150, 105))
                    )
                    .default_open(true)
                    .show(ui, |ui| {
                        render_leaf_with_context(
                            ui, app, "📹", Color32::from_rgb(14, 165, 233), 
                            &format!("Cameras ({})", cameras_count), 
                            SelectedNav::Cameras,
                            |app, ui| {
                                if ui.button("➕ Add Hardware / Camera Wizard...").clicked() {
                                    app.show_add_camera_modal = true;
                                    ui.close_menu();
                                }
                                if ui.button("🔍 Scan for ONVIF Cameras...").clicked() {
                                    app.show_add_camera_modal = true;
                                    ui.close_menu();
                                }
                                ui.separator();
                                if ui.button("🔄 Refresh Cameras").clicked() {
                                    app.refresh_all_modules();
                                    ui.close_menu();
                                }
                            }
                        );
                        render_leaf(ui, app, "🎤", Color32::from_rgb(245, 158, 11), "Microphones", SelectedNav::Microphones);
                        render_leaf(ui, app, "📢", Color32::from_rgb(249, 115, 22), "Speakers", SelectedNav::Speakers);
                        render_leaf(ui, app, "🗃", Color32::from_rgb(139, 92, 246), "Metadata", SelectedNav::Metadata);
                        render_leaf(ui, app, "🔌", Color32::from_rgb(101, 163, 13), "Input", SelectedNav::Input);
                        render_leaf(ui, app, "💡", Color32::from_rgb(234, 179, 8), "Output", SelectedNav::Output);
                    });

                    // 4. Client (Sky Blue / Violet)
                    egui::CollapsingHeader::new(
                        egui::RichText::new("📁 Client").size(11.0).color(Color32::from_rgb(2, 132, 199))
                    )
                    .default_open(false)
                    .show(ui, |ui| {
                        render_leaf(ui, app, "👁", Color32::from_rgb(37, 99, 235), "View Groups", SelectedNav::ViewGroups);
                        render_leaf(ui, app, "👤", Color32::from_rgb(99, 102, 241), "Smart Client Profiles", SelectedNav::SmartClientProfiles);
                        render_leaf(ui, app, "🎛", Color32::from_rgb(217, 70, 239), "Matrix", SelectedNav::Matrix);
                    });

                    // 5. Rules and Events (Crimson / Orange)
                    egui::CollapsingHeader::new(
                        egui::RichText::new("📁 Rules and Events").size(11.0).color(Color32::from_rgb(234, 88, 12))
                    )
                    .default_open(false)
                    .show(ui, |ui| {
                        render_leaf(ui, app, "📜", Color32::from_rgb(217, 119, 6), "Rules", SelectedNav::Rules);
                        render_leaf(ui, app, "⏱", Color32::from_rgb(13, 148, 136), "Time Profiles", SelectedNav::TimeProfiles);
                        render_leaf(ui, app, "🔔", Color32::from_rgb(234, 179, 8), "Notification Profiles", SelectedNav::NotificationProfiles);
                        render_leaf(ui, app, "👤", Color32::from_rgb(6, 182, 212), "User-defined Events", SelectedNav::UserDefinedEvents);
                        render_leaf(ui, app, "🧠", Color32::from_rgb(217, 70, 239), "Analytics Events", SelectedNav::AnalyticsEvents);
                        render_leaf(ui, app, "⚡", Color32::from_rgb(245, 158, 11), "Generic Events", SelectedNav::GenericEvents);
                        render_leaf(ui, app, "🌐", Color32::from_rgb(16, 185, 129), "Webhooks", SelectedNav::Webhooks);
                    });

                    // 6. Security (Royal Purple)
                    egui::CollapsingHeader::new(
                        egui::RichText::new("📁 Security").size(11.0).color(Color32::from_rgb(124, 58, 237))
                    )
                    .default_open(false)
                    .show(ui, |ui| {
                        render_leaf(ui, app, "👥", Color32::from_rgb(99, 102, 241), "Roles (3)", SelectedNav::Roles);
                        render_leaf(ui, app, "👤", Color32::from_rgb(100, 116, 139), &format!("Basic Users ({})", users_count), SelectedNav::BasicUsers);
                    });

                    // 7. System Dashboard (Rose / Pink)
                    egui::CollapsingHeader::new(
                        egui::RichText::new("📁 System Dashboard").size(11.0).color(Color32::from_rgb(225, 29, 72))
                    )
                    .default_open(false)
                    .show(ui, |ui| {
                        render_leaf(ui, app, "📋", Color32::from_rgb(245, 158, 11), "Current Tasks", SelectedNav::CurrentTasks);
                        render_leaf(ui, app, "📊", Color32::from_rgb(16, 185, 129), "System Monitor", SelectedNav::SystemMonitor);
                        render_leaf(ui, app, "📈", Color32::from_rgb(6, 182, 212), "System Monitor Thresholds", SelectedNav::SystemMonitorThresholds);
                        render_leaf(ui, app, "📑", Color32::from_rgb(37, 99, 235), "Configuration Reports", SelectedNav::ConfigurationReports);
                        render_leaf(ui, app, "📜", Color32::from_rgb(100, 116, 139), "Server Logs", SelectedNav::ServerLogs);
                    });

                    // 8. Metadata Use (Teal Cyan)
                    egui::CollapsingHeader::new(
                        egui::RichText::new("📁 Metadata Use").size(11.0).color(Color32::from_rgb(13, 148, 136))
                    )
                    .default_open(false)
                    .show(ui, |ui| {
                        render_leaf(ui, app, "🔍", Color32::from_rgb(37, 99, 235), &format!("Metadata Search / ANPR ({})", lpr_count), SelectedNav::MetadataSearch);
                    });

                    // 9. Access Control (Golden Brown)
                    egui::CollapsingHeader::new(
                        egui::RichText::new("📁 Access Control").size(11.0).color(Color32::from_rgb(180, 83, 9))
                    )
                    .default_open(false)
                    .show(ui, |ui| {
                        render_leaf(ui, app, "🚪", Color32::from_rgb(217, 119, 6), "Access Control Units", SelectedNav::AccessControl);
                    });

                    // 10. Alarms (Crimson Red)
                    egui::CollapsingHeader::new(
                        egui::RichText::new("📁 Alarms").size(11.0).color(Color32::from_rgb(220, 38, 38))
                    )
                    .default_open(false)
                    .show(ui, |ui| {
                        render_leaf(ui, app, "🚨", Color32::from_rgb(239, 68, 68), &format!("Alarm Definitions ({})", alarms_count), SelectedNav::Alarms);
                    });
                });
            });
        });
}

fn render_leaf(ui: &mut egui::Ui, app: &mut ManagementClientApp, icon: &str, icon_color: Color32, text: &str, nav: SelectedNav) {
    let is_selected = app.selected_nav == nav;
    let text_color = if is_selected { Color32::WHITE } else { Color32::from_rgb(30, 41, 59) };
    let active_icon_color = if is_selected { Color32::WHITE } else { icon_color };

    let mut job = egui::text::LayoutJob::default();
    job.append(
        icon,
        0.0,
        egui::TextFormat {
            color: active_icon_color,
            font_id: egui::FontId::proportional(12.0),
            ..Default::default()
        },
    );
    job.append("  ", 0.0, egui::TextFormat::default());
    job.append(
        text,
        0.0,
        egui::TextFormat {
            color: text_color,
            font_id: egui::FontId::proportional(11.0),
            ..Default::default()
        },
    );

    let response = ui.selectable_label(is_selected, job);
    if response.clicked() {
        app.selected_nav = nav;
    }
}

fn render_leaf_with_context<F>(ui: &mut egui::Ui, app: &mut ManagementClientApp, icon: &str, icon_color: Color32, text: &str, nav: SelectedNav, context_fn: F)
where
    F: FnOnce(&mut ManagementClientApp, &mut egui::Ui),
{
    let is_selected = app.selected_nav == nav;
    let text_color = if is_selected { Color32::WHITE } else { Color32::from_rgb(30, 41, 59) };
    let active_icon_color = if is_selected { Color32::WHITE } else { icon_color };

    let mut job = egui::text::LayoutJob::default();
    job.append(
        icon,
        0.0,
        egui::TextFormat {
            color: active_icon_color,
            font_id: egui::FontId::proportional(12.0),
            ..Default::default()
        },
    );
    job.append("  ", 0.0, egui::TextFormat::default());
    job.append(
        text,
        0.0,
        egui::TextFormat {
            color: text_color,
            font_id: egui::FontId::proportional(11.0),
            ..Default::default()
        },
    );

    let response = ui.selectable_label(is_selected, job);
    if response.clicked() {
        app.selected_nav = nav;
    }

    response.context_menu(|ui| {
        context_fn(app, ui);
    });
}
