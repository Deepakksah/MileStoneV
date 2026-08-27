use eframe::egui::{self, Color32, Pos2, Rounding, Stroke, Vec2};
use crate::state::{clamp_safe, ManagementClientApp};

pub fn render(app: &mut ManagementClientApp, ui: &mut egui::Ui) {
    let available_height = ui.available_height();
    let available_width = ui.available_width();

    // Bound bottom dock height dynamically with safe clamp
    app.bottom_dock_height = clamp_safe(app.bottom_dock_height, 120.0, (available_height - 180.0).max(120.0));
    app.middle_panel_width = clamp_safe(app.middle_panel_width, 200.0, (available_width - 300.0).max(200.0));


    let top_height = (available_height - app.bottom_dock_height - 8.0).max(180.0);

    // -------------------------------------------------------------------------
    // Top Area: Recording Server Devices Tree + Properties Panel (Draggable Left/Right)
    // -------------------------------------------------------------------------
    ui.allocate_ui(Vec2::new(available_width, top_height), |ui| {
        ui.horizontal(|ui| {
            // 1. Middle Column: Recording Server Devices Tree
            ui.allocate_ui(Vec2::new(app.middle_panel_width, top_height), |ui| {
                ui.vertical(|ui| {
                    // Title Bar: Recording Server
                    ui.horizontal(|ui| {
                        ui.label(egui::RichText::new("Recording Server").strong().size(12.0).color(Color32::from_rgb(15, 23, 42)));
                        ui.with_layout(egui::Layout::right_to_left(egui::Align::Center), |ui| {
                            ui.label(egui::RichText::new("✕").size(11.0).color(Color32::from_rgb(100, 116, 139)));
                            ui.label(egui::RichText::new("?").strong().size(11.0).color(Color32::from_rgb(100, 116, 139)));
                        });
                    });

                    ui.add_space(2.0);
                    ui.separator();

                    // Filter Expandable Header
                    egui::CollapsingHeader::new(
                        egui::RichText::new("Filter your devices").size(11.0).color(Color32::from_rgb(51, 65, 85))
                    )
                    .default_open(true)
                    .show(ui, |ui| {
                        ui.horizontal(|ui| {
                            let mut hide_disabled = true;
                            ui.checkbox(&mut hide_disabled, egui::RichText::new("Hide disabled devices").size(10.5));
                        });

                        ui.add_space(2.0);
                        ui.horizontal(|ui| {
                            let mut filter_text = String::new();
                            ui.add(
                                egui::TextEdit::singleline(&mut filter_text)
                                    .hint_text("Type here to filter devices")
                                    .desired_width(180.0)
                            );
                            let _ = ui.button(egui::RichText::new("Apply filter").size(10.0));
                        });
                    });

                    ui.add_space(2.0);
                    ui.separator();

                    // Recording Server Hierarchy Tree
                    egui::ScrollArea::vertical()
                        .auto_shrink([false, false])
                        .show(ui, |ui| {
                            ui.style_mut().spacing.item_spacing = Vec2::new(0.0, 2.0);

                            egui::CollapsingHeader::new(
                                egui::RichText::new("📁 Recording Servers").strong().size(11.5).color(Color32::from_rgb(30, 41, 59))
                            )
                            .default_open(true)
                            .show(ui, |ui| {
                                
                                // Server Node: JSPANGICCCREC01.JSPLICCC.LOCAL
                                let srv_label = ui.horizontal(|ui| {
                                    ui.label(
                                        egui::RichText::new("🖥 JSPANGICCCREC01.JSPLICCC.LOCAL")
                                            .strong()
                                            .size(11.0)
                                            .color(Color32::from_rgb(30, 41, 59))
                                    );
                                }).response;

                                srv_label.context_menu(|ui| {
                                    if ui.button("Collapse").clicked() { ui.close_menu(); }
                                    ui.separator();
                                    if ui.button("🪄  Add Hardware...\tCtrl+N").clicked() {
                                        app.show_add_hardware_wizard = true;
                                        ui.close_menu();
                                    }
                                    if ui.button("📦  Move Hardware...").clicked() {
                                        app.show_move_hardware_modal = true;
                                        ui.close_menu();
                                    }
                                    if ui.button(egui::RichText::new("🗑  Delete All Hardware").color(Color32::from_rgb(220, 38, 38))).clicked() {
                                        let _ = app.management_server.delete_all_hardware("a1b2c3d4-0001-0000-0000-000000000001");
                                        ui.close_menu();
                                    }
                                    if ui.button("🔐  Change Hardware Password...").clicked() {
                                        app.show_change_pwd_modal = true;
                                        ui.close_menu();
                                    }
                                    ui.separator();
                                    if ui.button("✏  Rename Recording Server\tF2").clicked() {
                                        app.show_rename_server_modal = true;
                                        ui.close_menu();
                                    }
                                    if ui.button(egui::RichText::new("❌  Remove Recording Server").color(Color32::from_rgb(220, 38, 38))).clicked() {
                                        let _ = app.management_server.delete_recording_server("a1b2c3d4-0001-0000-0000-000000000001");
                                        ui.close_menu();
                                    }
                                    ui.separator();
                                    if ui.button(egui::RichText::new("🔄  Refresh\tF5").color(Color32::from_rgb(5, 150, 105))).clicked() {
                                        app.refresh_all_modules();
                                        ui.close_menu();
                                    }
                                });

                                // Live Cameras List Matching Exact Screenshot
                                let cameras = get_milestone_live_camera_list();
                                for cam in cameras {
                                    let is_selected = app.selected_hardware_name == cam;
                                    let text_color = if is_selected { Color32::WHITE } else { Color32::from_rgb(30, 41, 59) };
                                    let bg_color = if is_selected { Color32::from_rgb(0, 120, 215) } else { Color32::TRANSPARENT };

                                    let resp = ui.horizontal(|ui| {
                                        ui.add_space(20.0);
                                        ui.label(egui::RichText::new("📹").size(11.0).color(if is_selected { Color32::WHITE } else { Color32::from_rgb(0, 102, 204) }));
                                        ui.label(
                                            egui::RichText::new(cam)
                                                .size(11.0)
                                                .color(text_color)
                                                .background_color(bg_color)
                                        );
                                    }).response;

                                    if resp.clicked() {
                                        app.select_camera_node(cam, "Recording Server 01");
                                    }

                                    // EXACT Milestone Camera / Hardware Right Click Context Menu
                                    resp.context_menu(|ui| {
                                        ui.style_mut().spacing.item_spacing = Vec2::new(0.0, 3.0);
                                        if ui.button("Expand").clicked() { ui.close_menu(); }
                                        ui.separator();
                                        if ui.button("✏  Edit Hardware...").clicked() {
                                            app.show_edit_hardware_modal = true;
                                            ui.close_menu();
                                        }
                                        if ui.button("🔐  Change Hardware Password...").clicked() {
                                            app.show_change_pwd_modal = true;
                                            ui.close_menu();
                                        }
                                        if ui.button("🔄  Update firmware...").clicked() {
                                            app.show_update_fw_modal = true;
                                            ui.close_menu();
                                        }
                                        if ui.button(egui::RichText::new("❌  Delete Hardware\tDel").color(Color32::from_rgb(220, 38, 38))).clicked() {
                                            let _ = app.management_server.delete_hardware(&app.selected_hardware_id);
                                            ui.close_menu();
                                        }
                                        if ui.button("📦  Move Hardware...").clicked() {
                                            app.show_move_hardware_modal = true;
                                            ui.close_menu();
                                        }
                                        if ui.button("🔁  Replace Hardware").clicked() {
                                            app.show_add_hardware_wizard = true;
                                            ui.close_menu();
                                        }
                                        if ui.button(egui::RichText::new("⚡  Update Hardware").color(Color32::from_rgb(16, 185, 129))).clicked() {
                                            app.refresh_all_modules();
                                            ui.close_menu();
                                        }
                                        if ui.button("✏  Rename Hardware\tF2").clicked() {
                                            app.show_rename_hw_modal = true;
                                            ui.close_menu();
                                        }
                                        ui.separator();
                                        let mut enabled_check = app.selected_hardware_enabled;
                                        if ui.checkbox(&mut enabled_check, "Enabled").clicked() {
                                            app.toggle_hardware_enabled();
                                            ui.close_menu();
                                        }
                                        ui.separator();
                                        if ui.button(egui::RichText::new("🔄  Refresh\tF5").color(Color32::from_rgb(5, 150, 105))).clicked() {
                                            app.refresh_all_modules();
                                            ui.close_menu();
                                        }
                                    });
                                }
                            });
                        });
                });
            });

            // -----------------------------------------------------------------
            // Draggable Vertical Splitter (Left / Right Resize)
            // -----------------------------------------------------------------
            let (splitter_rect, splitter_resp) = ui.allocate_exact_size(Vec2::new(6.0, top_height), egui::Sense::drag());
            if splitter_resp.hovered() || splitter_resp.dragged() {
                ui.ctx().set_cursor_icon(egui::CursorIcon::ResizeHorizontal);
            }
            if splitter_resp.dragged() {
                app.middle_panel_width += splitter_resp.drag_delta().x;
            }
            ui.painter().rect_filled(
                splitter_rect,
                Rounding::ZERO,
                if splitter_resp.hovered() || splitter_resp.dragged() { Color32::from_rgb(0, 120, 215) } else { Color32::from_rgb(226, 232, 240) },
            );

            // -----------------------------------------------------------------
            // 2. Right Column: Hardware Information Panel (Stretches to remaining width)
            // -----------------------------------------------------------------
            let right_width = (ui.available_width()).max(100.0);
            ui.allocate_ui(Vec2::new(right_width, top_height), |ui| {
                ui.vertical(|ui| {
                    // Title Bar: Properties
                    ui.horizontal(|ui| {
                        ui.label(egui::RichText::new("Properties").strong().size(12.0).color(Color32::from_rgb(15, 23, 42)));
                        ui.with_layout(egui::Layout::right_to_left(egui::Align::Center), |ui| {
                            ui.label(egui::RichText::new("✕").size(11.0).color(Color32::from_rgb(100, 116, 139)));
                            ui.label(egui::RichText::new("?").strong().size(11.0).color(Color32::from_rgb(100, 116, 139)));
                        });
                    });

                    ui.add_space(2.0);
                    ui.separator();
                    ui.add_space(4.0);

                    let form_height = (top_height - 65.0).max(100.0);
                    egui::ScrollArea::vertical()
                        .max_height(form_height)
                        .auto_shrink([false, false])
                        .show(ui, |ui| {
                            ui.set_width(ui.available_width());

                            ui.label(egui::RichText::new("Hardware information").strong().size(11.0).color(Color32::from_rgb(30, 41, 59)));
                            ui.add_space(4.0);

                            ui.label(egui::RichText::new("Name:").size(10.5).color(Color32::from_rgb(71, 85, 105)));
                            ui.add(egui::TextEdit::singleline(&mut app.selected_hardware_name).desired_width(ui.available_width()));

                            ui.add_space(4.0);
                            ui.label(egui::RichText::new("Description:").size(10.5).color(Color32::from_rgb(71, 85, 105)));
                            let mut desc = String::new();
                            ui.add(egui::TextEdit::multiline(&mut desc).desired_width(ui.available_width()).desired_rows(3));

                            ui.add_space(4.0);
                            ui.label(egui::RichText::new("Model:").size(10.5).color(Color32::from_rgb(71, 85, 105)));
                            ui.add(egui::TextEdit::singleline(&mut app.selected_hardware_model).desired_width(ui.available_width()));

                            ui.add_space(4.0);
                            ui.label(egui::RichText::new("Serial number:").size(10.5).color(Color32::from_rgb(71, 85, 105)));
                            ui.add(egui::TextEdit::singleline(&mut app.selected_hardware_serial).desired_width(ui.available_width()));

                            ui.add_space(4.0);
                            ui.label(egui::RichText::new("Driver:").size(10.5).color(Color32::from_rgb(71, 85, 105)));
                            ui.add(egui::TextEdit::singleline(&mut app.selected_hardware_driver).desired_width(ui.available_width()));

                            ui.add_space(4.0);
                            ui.label(egui::RichText::new("Address:").size(10.5).color(Color32::from_rgb(71, 85, 105)));
                            ui.horizontal(|ui| {
                                ui.add(egui::TextEdit::singleline(&mut app.selected_hardware_addr).desired_width(ui.available_width() - 30.0));
                                let _ = ui.button("🔄");
                            });

                            ui.add_space(4.0);
                            ui.label(egui::RichText::new("MAC address:").size(10.5).color(Color32::from_rgb(71, 85, 105)));
                            ui.add(egui::TextEdit::singleline(&mut app.selected_hardware_mac).desired_width(ui.available_width()));

                            ui.add_space(4.0);
                            ui.label(egui::RichText::new("Firmware version:").size(10.5).color(Color32::from_rgb(71, 85, 105)));
                            ui.add(egui::TextEdit::singleline(&mut app.selected_hardware_fw).desired_width(ui.available_width()));

                            ui.add_space(4.0);
                            ui.label(egui::RichText::new("Password last changed:").size(10.0).color(Color32::from_rgb(100, 116, 139)));
                            ui.label(egui::RichText::new("Hardware data last updated: 27 August 2026 13:32:00").size(10.0).color(Color32::from_rgb(100, 116, 139)));
                        });

                    // Bottom Tabs: [ℹ Info, ⚙ Settings, 🕹 PTZ, 🔔 Events]
                    ui.add_space(4.0);
                    ui.separator();
                    ui.horizontal(|ui| {
                        ui.style_mut().spacing.item_spacing = Vec2::new(8.0, 0.0);
                        
                        let btn_info = egui::Button::new(
                            egui::RichText::new("ℹ Info").strong().size(10.5).color(Color32::from_rgb(0, 102, 204))
                        ).fill(Color32::from_rgb(224, 242, 254));
                        ui.add(btn_info);

                        let btn_settings = egui::Button::new(
                            egui::RichText::new("⚙ Settings").size(10.5).color(Color32::from_rgb(71, 85, 105))
                        ).fill(Color32::TRANSPARENT);
                        if ui.add(btn_settings).clicked() {}

                        let btn_ptz = egui::Button::new(
                            egui::RichText::new("🕹 PTZ").size(10.5).color(Color32::from_rgb(71, 85, 105))
                        ).fill(Color32::TRANSPARENT);
                        if ui.add(btn_ptz).clicked() {}

                        let btn_events = egui::Button::new(
                            egui::RichText::new("🔔 Events").size(10.5).color(Color32::from_rgb(71, 85, 105))
                        ).fill(Color32::TRANSPARENT);
                        if ui.add(btn_events).clicked() {}
                    });
                });
            });
        });
    });

    // -------------------------------------------------------------------------
    // Draggable Horizontal Splitter (Upar / Niche Resize)
    // -------------------------------------------------------------------------
    let (h_splitter_rect, h_splitter_resp) = ui.allocate_exact_size(Vec2::new(available_width, 6.0), egui::Sense::drag());
    if h_splitter_resp.hovered() || h_splitter_resp.dragged() {
        ui.ctx().set_cursor_icon(egui::CursorIcon::ResizeVertical);
    }
    if h_splitter_resp.dragged() {
        let new_h = app.bottom_dock_height - h_splitter_resp.drag_delta().y;
        app.bottom_dock_height = clamp_safe(new_h, 100.0, (available_height - 150.0).max(100.0));
    }
    ui.painter().rect_filled(
        h_splitter_rect,
        Rounding::ZERO,
        if h_splitter_resp.hovered() || h_splitter_resp.dragged() { Color32::from_rgb(0, 120, 215) } else { Color32::from_rgb(226, 232, 240) },
    );

    // -------------------------------------------------------------------------
    // Bottom Dock: High-Resolution Live Preview
    // -------------------------------------------------------------------------
    ui.allocate_ui(Vec2::new(available_width, app.bottom_dock_height), |ui| {
        render_bottom_preview_panel(app, ui);
    });

    // Render Modals
    render_server_modals(app, ui.ctx());
}

fn render_server_modals(app: &mut ManagementClientApp, ctx: &egui::Context) {
    if app.show_edit_hardware_modal {
        egui::Window::new("✏ Edit Hardware Properties")
            .fixed_size(Vec2::new(420.0, 260.0))
            .collapsible(false)
            .resizable(false)
            .anchor(egui::Align2::CENTER_CENTER, Vec2::ZERO)
            .show(ctx, |ui| {
                ui.heading("Edit Hardware Configuration");
                ui.add_space(8.0);
                ui.separator();
                ui.add_space(8.0);
                egui::Grid::new("edit_hw_grid").min_col_width(120.0).spacing(Vec2::new(12.0, 8.0)).show(ui, |ui| {
                    ui.label(egui::RichText::new("Hardware Name:").strong());
                    ui.text_edit_singleline(&mut app.selected_hardware_name);
                    ui.end_row();
                    ui.label(egui::RichText::new("Address / IP:").strong());
                    ui.text_edit_singleline(&mut app.selected_hardware_addr);
                    ui.end_row();
                    ui.label(egui::RichText::new("Model:").strong());
                    ui.text_edit_singleline(&mut app.selected_hardware_model);
                    ui.end_row();
                });
                ui.add_space(16.0);
                ui.separator();
                ui.horizontal(|ui| {
                    if ui.button("Cancel").clicked() {
                        app.show_edit_hardware_modal = false;
                    }
                    ui.with_layout(egui::Layout::right_to_left(egui::Align::Center), |ui| {
                        let btn_save = egui::Button::new(
                            egui::RichText::new("💾 Save Changes").strong().color(Color32::WHITE)
                        ).fill(Color32::from_rgb(16, 185, 129));
                        if ui.add(btn_save).clicked() {
                            app.apply_edit_hardware();
                        }
                    });
                });
            });
    }

    if app.show_update_fw_modal {
        egui::Window::new("🔄 Update Camera / Hardware Firmware")
            .fixed_size(Vec2::new(400.0, 200.0))
            .collapsible(false)
            .resizable(false)
            .anchor(egui::Align2::CENTER_CENTER, Vec2::ZERO)
            .show(ctx, |ui| {
                ui.label(egui::RichText::new(format!("Target: {}", app.selected_hardware_name)).strong());
                ui.add_space(6.0);
                ui.label("Enter new Firmware Version string or path:");
                ui.text_edit_singleline(&mut app.new_firmware_ver);
                ui.add_space(16.0);
                ui.separator();
                ui.horizontal(|ui| {
                    if ui.button("Cancel").clicked() {
                        app.show_update_fw_modal = false;
                    }
                    ui.with_layout(egui::Layout::right_to_left(egui::Align::Center), |ui| {
                        let btn_fw = egui::Button::new(
                            egui::RichText::new("🔄 Flash Firmware").strong().color(Color32::WHITE)
                        ).fill(Color32::from_rgb(0, 102, 204));
                        if ui.add(btn_fw).clicked() {
                            app.apply_update_firmware();
                        }
                    });
                });
            });
    }

    if app.show_rename_hw_modal {
        egui::Window::new("✏ Rename Hardware Device")
            .fixed_size(Vec2::new(380.0, 160.0))
            .collapsible(false)
            .resizable(false)
            .anchor(egui::Align2::CENTER_CENTER, Vec2::ZERO)
            .show(ctx, |ui| {
                ui.label(egui::RichText::new("Enter new hardware device name:").strong());
                ui.add_space(8.0);
                ui.text_edit_singleline(&mut app.rename_hw_text);
                ui.add_space(16.0);
                ui.separator();
                ui.horizontal(|ui| {
                    if ui.button("Cancel").clicked() {
                        app.show_rename_hw_modal = false;
                    }
                    ui.with_layout(egui::Layout::right_to_left(egui::Align::Center), |ui| {
                        let btn_rn = egui::Button::new(
                            egui::RichText::new("💾 Save Name").strong().color(Color32::WHITE)
                        ).fill(Color32::from_rgb(0, 120, 215));
                        if ui.add(btn_rn).clicked() {
                            app.apply_rename_hardware();
                        }
                    });
                });
            });
    }

    if app.show_change_pwd_modal {
        egui::Window::new("🔐 Change Hardware Password")
            .fixed_size(Vec2::new(380.0, 200.0))
            .collapsible(false)
            .resizable(false)
            .anchor(egui::Align2::CENTER_CENTER, Vec2::ZERO)
            .show(ctx, |ui| {
                ui.label(egui::RichText::new("Update ONVIF / RTSP authentication password:").strong());
                ui.add_space(8.0);
                ui.label("New Password:");
                ui.add(egui::TextEdit::singleline(&mut app.new_hardware_pwd).password(true));
                ui.label("Confirm Password:");
                ui.add(egui::TextEdit::singleline(&mut app.confirm_hardware_pwd).password(true));
                ui.add_space(16.0);
                ui.separator();
                ui.horizontal(|ui| {
                    if ui.button("Cancel").clicked() {
                        app.show_change_pwd_modal = false;
                    }
                    ui.with_layout(egui::Layout::right_to_left(egui::Align::Center), |ui| {
                        let btn_pwd = egui::Button::new(
                            egui::RichText::new("🔐 Update Password").strong().color(Color32::WHITE),
                        ).fill(Color32::from_rgb(16, 185, 129));
                        if ui.add(btn_pwd).clicked() {
                            app.apply_change_pwd();
                        }
                    });
                });
            });
    }

    if app.show_add_hardware_wizard {
        egui::Window::new("🪄 Add Hardware Wizard (ONVIF & Universal Driver)")
            .fixed_size(Vec2::new(480.0, 360.0))
            .collapsible(false)
            .resizable(false)
            .anchor(egui::Align2::CENTER_CENTER, Vec2::ZERO)
            .show(ctx, |ui| {
                ui.heading("Hardware Device Discovery & Setup");
                ui.label(egui::RichText::new("Enter the IP and channel details for the Hardware device / NVR to auto-create camera channels.").size(11.0).color(Color32::from_rgb(100, 116, 139)));
                ui.add_space(8.0);
                ui.separator();
                ui.add_space(8.0);
                egui::Grid::new("add_hw_grid").min_col_width(140.0).spacing(Vec2::new(12.0, 8.0)).show(ui, |ui| {
                    ui.label(egui::RichText::new("Hardware Name:").strong());
                    ui.text_edit_singleline(&mut app.new_hw_name);
                    ui.end_row();
                    ui.label(egui::RichText::new("IP Address / Host:").strong());
                    ui.text_edit_singleline(&mut app.new_hw_ip);
                    ui.end_row();
                    ui.label(egui::RichText::new("Hardware Model:").strong());
                    ui.text_edit_singleline(&mut app.new_hw_model);
                    ui.end_row();
                    ui.label(egui::RichText::new("Camera Channels:").strong());
                    ui.add(egui::Slider::new(&mut app.new_hw_channels, 1..=64).suffix(" channels"));
                    ui.end_row();
                });
                ui.add_space(16.0);
                ui.separator();
                ui.add_space(8.0);
                ui.horizontal(|ui| {
                    if ui.button("  Cancel  ").clicked() {
                        app.show_add_hardware_wizard = false;
                    }
                    ui.with_layout(egui::Layout::right_to_left(egui::Align::Center), |ui| {
                        let btn_save = egui::Button::new(
                            egui::RichText::new("💾 Add Hardware & Auto-Detect Cameras")
                                .strong()
                                .color(Color32::WHITE),
                        )
                        .fill(Color32::from_rgb(16, 185, 129))
                        .rounding(Rounding::same(3.0));

                        if ui.add(btn_save).clicked() {
                            app.add_hardware_from_modal();
                        }
                    });
                });
            });
    }
}

// -----------------------------------------------------------------------------
// 3. Bottom Panel: High-Resolution Live Preview of Clicked Camera
// -----------------------------------------------------------------------------
fn render_bottom_preview_panel(_app: &mut ManagementClientApp, ui: &mut egui::Ui) {
    // Title Bar: Preview
    ui.horizontal(|ui| {
        ui.label(egui::RichText::new("Preview").strong().size(12.0).color(Color32::from_rgb(15, 23, 42)));
        ui.with_layout(egui::Layout::right_to_left(egui::Align::Center), |ui| {
            ui.label(egui::RichText::new("✕").size(11.0).color(Color32::from_rgb(100, 116, 139)));
            ui.label(egui::RichText::new("?").strong().size(11.0).color(Color32::from_rgb(100, 116, 139)));
        });
    });

    ui.add_space(2.0);
    ui.separator();

    // High-Resolution Live Camera Preview Center Stage
    ui.vertical_centered(|ui| {
        ui.label(
            egui::RichText::new("Live  1920x1080  18KB")
                .size(10.0)
                .color(Color32::from_rgb(5, 150, 105)),
        );

        let preview_size = Vec2::new(320.0, 130.0);
        let preview_rect = ui.allocate_exact_size(preview_size, egui::Sense::hover()).0;
        let painter = ui.painter();

        // Realistic Simulated Outdoor Tree Perimeter Road Scene
        painter.rect_filled(preview_rect, Rounding::same(1.0), Color32::from_rgb(38, 55, 38));
        painter.rect_stroke(preview_rect, Rounding::same(1.0), Stroke::new(1.0_f32, Color32::from_rgb(30, 41, 59)));

        // Central Asphalt Road Surface
        let road_top_l = Pos2::new(preview_rect.center().x - 18.0, preview_rect.min.y + 18.0);
        let road_top_r = Pos2::new(preview_rect.center().x + 18.0, preview_rect.min.y + 18.0);
        let road_bot_l = Pos2::new(preview_rect.min.x + 50.0, preview_rect.max.y);
        let road_bot_r = Pos2::new(preview_rect.max.x - 25.0, preview_rect.max.y);

        let road_pts = vec![road_top_l, road_top_r, road_bot_r, road_bot_l];
        painter.add(egui::Shape::convex_polygon(road_pts, Color32::from_rgb(120, 125, 130), Stroke::NONE));

        // Right Side White Perimeter Boundary Wall with Green Panels
        let wall_top_l = Pos2::new(preview_rect.center().x + 20.0, preview_rect.min.y + 20.0);
        let wall_top_r = Pos2::new(preview_rect.center().x + 50.0, preview_rect.min.y + 20.0);
        let wall_bot_l = Pos2::new(preview_rect.max.x - 24.0, preview_rect.max.y);
        let wall_bot_r = Pos2::new(preview_rect.max.x, preview_rect.max.y);

        let wall_pts = vec![wall_top_l, wall_top_r, wall_bot_r, wall_bot_l];
        painter.add(egui::Shape::convex_polygon(wall_pts, Color32::from_rgb(210, 225, 205), Stroke::new(0.5_f32, Color32::from_rgb(100, 130, 100))));

        for step in 1..=6 {
            let t = step as f32 / 6.0;
            let post_top = Pos2::new(wall_top_l.x + (wall_bot_l.x - wall_top_l.x) * t, wall_top_l.y + (wall_bot_l.y - wall_top_l.y) * t - 6.0);
            let post_bot = Pos2::new(wall_top_l.x + (wall_bot_l.x - wall_top_l.x) * t, wall_top_l.y + (wall_bot_l.y - wall_top_l.y) * t + 10.0);
            painter.line_segment([post_top, post_bot], Stroke::new(1.2_f32, Color32::from_rgb(60, 90, 60)));
        }

        // Left Side Dense Green Foliage
        painter.line_segment(
            [Pos2::new(preview_rect.center().x - 12.0, preview_rect.min.y + 15.0), Pos2::new(preview_rect.center().x - 6.0, preview_rect.max.y - 10.0)],
            Stroke::new(3.5_f32, Color32::from_rgb(45, 30, 20)),
        );
        painter.circle_filled(Pos2::new(preview_rect.min.x + 35.0, preview_rect.center().y - 15.0), 26.0, Color32::from_rgba_unmultiplied(20, 60, 20, 220));
        painter.circle_filled(Pos2::new(preview_rect.min.x + 60.0, preview_rect.center().y + 8.0), 22.0, Color32::from_rgba_unmultiplied(35, 75, 30, 200));

        // Watermarks
        painter.text(Pos2::new(preview_rect.min.x + 8.0, preview_rect.min.y + 6.0), egui::Align2::LEFT_TOP, "2026-08-27 13:32:00", egui::FontId::monospace(8.0), Color32::WHITE);
        painter.text(Pos2::new(preview_rect.max.x - 8.0, preview_rect.min.y + 6.0), egui::Align2::RIGHT_TOP, "MAIN STREAM 25FPS", egui::FontId::monospace(8.0), Color32::from_rgb(74, 222, 128));

        ui.add_space(2.0);
        ui.label(
            egui::RichText::new("E BLOCK CAMERA 3 FIX")
                .strong()
                .size(10.0)
                .color(Color32::from_rgb(30, 41, 59)),
        );
    });
}

// -----------------------------------------------------------------------------
// Real Milestone Camera Names extracted directly from user screenshot
// -----------------------------------------------------------------------------
fn get_milestone_live_camera_list() -> Vec<&'static str> {
    vec![
        "IPCamera (10.40.56.203)",
        "IPCamera (10.40.56.204)",
        "IPCamera (10.40.57.177)",
        "IPCamera (10.40.59.26)",
        "AIRPORT HIGHWAY",
        "Alcon EXCHANGE_ENTRANCE (10.36.10.247)",
        "Alcon H101 (10.40.56.101)",
        "Alcon IPC (10.36.14.12)",
        "Alcon IPC (10.36.14.14)",
        "Alcon IPC (10.36.14.33)",
        "Alcon IPC (10.40.56.90)",
        "Alcon IPC (10.40.56.94)",
        "Alcon IPC (10.40.56.96)",
        "Alcon IPC (10.40.56.97)",
        "Alcon IPC (10.40.56.98)",
        "Alcon IPC (10.40.56.99)",
        "Alcon IPC (10.40.56.100)",
        "Alcon IPC (10.40.56.106)",
        "Alcon IPC (10.40.56.108)",
        "Alcon IPC (10.40.56.109)",
        "Alcon IPC (10.40.56.110)",
        "Alcon OHCBACKAREA (10.36.9.40)",
        "ATC FRONT PTZ",
        "BOF FRONT",
        "CENTRAL STORE",
        "COAL PIPE CONVEYER",
        "E-25 BACK BOUNDARY",
        "E&F SECURITY POST",
        "FUSION BACKSIDE CAM-2",
        "GATE PARKING",
    ]
}
