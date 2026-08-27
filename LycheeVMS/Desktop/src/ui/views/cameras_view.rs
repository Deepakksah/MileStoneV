use eframe::egui::{self, Color32, Pos2, Rounding, Stroke, Vec2};
use crate::state::{clamp_safe, CameraTab, ManagementClientApp};

pub fn render(app: &mut ManagementClientApp, ui: &mut egui::Ui) {
    let available_height = ui.available_height();
    let available_width = ui.available_width();

    // Bound bottom dock height dynamically with safe clamp
    app.bottom_dock_height = clamp_safe(app.bottom_dock_height, 140.0, (available_height - 180.0).max(140.0));
    app.middle_panel_width = clamp_safe(app.middle_panel_width, 200.0, (available_width - 300.0).max(200.0));


    let top_height = (available_height - app.bottom_dock_height - 8.0).max(180.0);

    // Clone groups hierarchy for safe UI rendering
    let all_groups = app.groups_hierarchy.clone();

    // -------------------------------------------------------------------------
    // Top Area: Middle Devices Tree + Right Properties Panel (Draggable Left/Right)
    // -------------------------------------------------------------------------
    ui.allocate_ui(Vec2::new(available_width, top_height), |ui| {
        ui.horizontal(|ui| {
            // 1. Middle Column: Devices Hierarchy Tree (Every camera clickable & editable!)
            ui.allocate_ui(Vec2::new(app.middle_panel_width, top_height), |ui| {
                ui.vertical(|ui| {
                    // Title Bar: Devices
                    ui.horizontal(|ui| {
                        ui.label(egui::RichText::new("Devices").strong().size(12.0).color(Color32::from_rgb(15, 23, 42)));
                        ui.with_layout(egui::Layout::right_to_left(egui::Align::Center), |ui| {
                            ui.label(egui::RichText::new("✕").size(11.0).color(Color32::from_rgb(100, 116, 139)));
                            ui.label(egui::RichText::new("?").strong().size(11.0).color(Color32::from_rgb(100, 116, 139)));
                        });
                    });

                    ui.add_space(2.0);
                    ui.separator();

                    // Device Groups Tree - ALL CAMERAS CLICKABLE & EDITABLE!
                    egui::ScrollArea::vertical()
                        .auto_shrink([false, false])
                        .show(ui, |ui| {
                            ui.style_mut().spacing.item_spacing = Vec2::new(0.0, 2.0);

                            // Root Node: Cameras (Root Folder)
                            egui::CollapsingHeader::new(
                                egui::RichText::new("📁 Cameras").strong().size(11.5).color(Color32::from_rgb(217, 119, 6))
                            )
                            .id_salt("root_tree_cameras_folder")
                            .default_open(true)
                            .show(ui, |ui| {
                                for (group_name, cameras) in &all_groups {
                                    let grp_str = group_name.as_str();
                                    let is_grp_selected = app.selected_device_group == grp_str;
                                    let default_open = grp_str == "South Block" || grp_str == "Camera Group 1" || grp_str == "ROAD SAFETY";
                                    let is_folder_editing = app.editing_folder_original_name.as_deref() == Some(grp_str);

                                    // Folder Header Row with Yellow Folder Icon & Fixed ID
                                    let grp_header = egui::CollapsingHeader::new(
                                        egui::RichText::new(format!("📁 {}", grp_str))
                                            .strong()
                                            .size(11.0)
                                            .color(if is_grp_selected { Color32::WHITE } else { Color32::from_rgb(217, 119, 6) })
                                            .background_color(if is_grp_selected { Color32::from_rgb(0, 120, 215) } else { Color32::TRANSPARENT })
                                    )
                                    .id_salt(format!("device_grp_header_{}", grp_str))
                                    .default_open(default_open);

                                    let header_res = grp_header.show(ui, |ui| {
                                        // Inline editing active for this folder
                                        if is_folder_editing {
                                            ui.horizontal(|ui| {
                                                ui.add_space(20.0);
                                                ui.label(egui::RichText::new("📁").size(11.0).color(Color32::from_rgb(245, 158, 11)));
                                                let edit_resp = ui.add(
                                                    egui::TextEdit::singleline(&mut app.inline_folder_text)
                                                        .desired_width(160.0)
                                                );
                                                if edit_resp.lost_focus() || ui.input(|i| i.key_pressed(egui::Key::Enter)) {
                                                    app.commit_inline_folder_rename();
                                                }
                                                if ui.input(|i| i.key_pressed(egui::Key::Escape)) {
                                                    app.cancel_inline_folder_rename();
                                                }
                                                let btn_ok = ui.button(egui::RichText::new("✓").strong().color(Color32::from_rgb(16, 185, 129)));
                                                if btn_ok.clicked() {
                                                    app.commit_inline_folder_rename();
                                                }
                                            });
                                            ui.add_space(2.0);
                                        }

                                        for cam in cameras {
                                            let cam_str = cam.as_str();
                                            let is_cam_selected = app.selected_hardware_name == cam_str;
                                            let is_cam_editing = app.editing_camera_original_name.as_deref() == Some(cam_str);

                                            if is_cam_editing {
                                                ui.horizontal(|ui| {
                                                    ui.add_space(18.0);
                                                    ui.label(egui::RichText::new("📹").size(11.0).color(Color32::from_rgb(0, 102, 204)));
                                                    let edit_cam_resp = ui.add(
                                                        egui::TextEdit::singleline(&mut app.inline_camera_text)
                                                            .desired_width(140.0)
                                                    );
                                                    if edit_cam_resp.lost_focus() || ui.input(|i| i.key_pressed(egui::Key::Enter)) {
                                                        app.commit_inline_camera_rename();
                                                    }
                                                    if ui.input(|i| i.key_pressed(egui::Key::Escape)) {
                                                        app.cancel_inline_camera_rename();
                                                    }
                                                    let btn_cam_ok = ui.button(egui::RichText::new("✓").strong().color(Color32::from_rgb(16, 185, 129)));
                                                    if btn_cam_ok.clicked() {
                                                        app.commit_inline_camera_rename();
                                                    }
                                                });
                                            } else {
                                                let mut cam_clicked = false;
                                                let mut cam_double_clicked = false;

                                                let resp = ui.horizontal(|ui| {
                                                    ui.add_space(18.0);
                                                    ui.label(egui::RichText::new("📹").size(11.0).color(if is_cam_selected { Color32::WHITE } else { Color32::from_rgb(0, 102, 204) }));
                                                    
                                                    let label_btn = ui.selectable_label(
                                                        is_cam_selected,
                                                        egui::RichText::new(cam_str)
                                                            .size(10.5)
                                                            .color(if is_cam_selected { Color32::WHITE } else { Color32::from_rgb(30, 41, 59) })
                                                    );

                                                    if label_btn.clicked() {
                                                        cam_clicked = true;
                                                    }
                                                    if label_btn.double_clicked() {
                                                        cam_double_clicked = true;
                                                    }

                                                    // Quick inline edit icon button right beside camera name
                                                    let edit_btn = ui.button(egui::RichText::new("✏").size(9.0).color(if is_cam_selected { Color32::WHITE } else { Color32::from_rgb(100, 116, 139) }));
                                                    if edit_btn.clicked() {
                                                        app.start_inline_camera_rename(cam_str);
                                                    }

                                                    label_btn
                                                }).inner;

                                                // CLICK ON ANY CAMERA TO SELECT AND LOAD EXACT PREVIEW & PROPERTIES!
                                                if cam_clicked {
                                                    app.select_camera_node(cam_str, grp_str);
                                                }

                                                // DOUBLE CLICK TO INLINE EDIT CAMERA NAME!
                                                if cam_double_clicked {
                                                    app.start_inline_camera_rename(cam_str);
                                                }

                                                // Camera Right Click Context Menu
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
                                                        app.start_inline_camera_rename(cam_str);
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
                                        }
                                    });

                                    // Double click on folder header starts inline rename
                                    if header_res.header_response.double_clicked() {
                                        app.start_inline_folder_rename(grp_str);
                                    }

                                    // EXACT Device Group / Folder Right-Click Context Menu
                                    header_res.header_response.context_menu(|ui| {
                                        ui.style_mut().spacing.item_spacing = Vec2::new(0.0, 3.0);
                                        
                                        if ui.button("Expand").clicked() { 
                                            app.selected_device_group = grp_str.to_string();
                                            ui.close_menu(); 
                                        }
                                        ui.separator();

                                        if ui.button("🪄  Add Device Group...\tCtrl+N").clicked() {
                                            app.show_add_device_group_modal = true;
                                            ui.close_menu();
                                        }

                                        if ui.button("👥  Edit Device Group Members...").clicked() {
                                            app.open_edit_members_modal(grp_str);
                                            ui.close_menu();
                                        }

                                        if ui.button(egui::RichText::new("🗑  Delete Device Group\tDel").color(Color32::from_rgb(220, 38, 38))).clicked() {
                                            app.delete_selected_device_group(grp_str);
                                            ui.close_menu();
                                        }

                                        if ui.button("✏  Rename Device Group\tF2").clicked() {
                                            app.start_inline_folder_rename(grp_str);
                                            ui.close_menu();
                                        }

                                        ui.separator();

                                        if ui.button("🔗  Go To Hardware...").clicked() {
                                            app.selected_nav = crate::state::SelectedNav::RecordingServers;
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
            // 2. Right Column: Properties (HikVisionGeneric Profile - 100% Live Editable per Camera!)
            // -----------------------------------------------------------------
            let right_width = (ui.available_width()).max(100.0);

            ui.allocate_ui(Vec2::new(right_width, top_height), |ui| {
                ui.vertical(|ui| {
                    // TOP BAR 1: Properties Header + Help/Close
                    ui.horizontal(|ui| {
                        ui.label(egui::RichText::new("Properties").strong().size(12.5).color(Color32::from_rgb(15, 23, 42)));
                        ui.with_layout(egui::Layout::right_to_left(egui::Align::Center), |ui| {
                            ui.label(egui::RichText::new("✕").size(11.0).color(Color32::from_rgb(100, 116, 139)));
                            ui.label(egui::RichText::new("?").strong().size(11.0).color(Color32::from_rgb(100, 116, 139)));
                        });
                    });

                    ui.add_space(2.0);

                    // TOP BAR 2: DEDICATED PROMINENT TOP SAVE TOOLBAR (Right at the top!)
                    ui.horizontal(|ui| {
                        let btn_top_save = egui::Button::new(
                            egui::RichText::new("💾 Save Changes").strong().size(11.5).color(Color32::WHITE),
                        ).fill(Color32::from_rgb(16, 185, 129)); // Rich emerald green prominent save button
                        if ui.add(btn_top_save).clicked() {
                            app.save_camera_properties();
                        }

                        let btn_top_reset = egui::Button::new(
                            egui::RichText::new("↺ Reset").size(11.0).color(Color32::from_rgb(71, 85, 105)),
                        );
                        if ui.add(btn_top_reset).clicked() {
                            let cam_name = app.selected_hardware_name.clone();
                            let grp_name = app.selected_device_group.clone();
                            app.select_camera_node(&cam_name, &grp_name);
                        }

                        // Success Feedback Toast Badge right on top toolbar
                        if let Some(t) = app.save_feedback_time {
                            if t.elapsed().as_secs_f32() < 3.0 {
                                ui.label(egui::RichText::new("✓ Settings Saved Successfully!").strong().size(11.0).color(Color32::from_rgb(16, 185, 129)));
                            }
                        }
                    });

                    ui.add_space(3.0);
                    ui.separator();

                    // TOP BAR 3: Camera Name & Group Controls (Directly updates tree node)
                    ui.horizontal(|ui| {
                        ui.label(egui::RichText::new("Camera Name:").strong().size(11.0).color(Color32::from_rgb(15, 23, 42)));
                        let mut name_val = app.current_camera_settings.name.clone();
                        let name_edit = ui.add(egui::TextEdit::singleline(&mut name_val).desired_width(180.0));
                        if name_edit.changed() || name_edit.lost_focus() {
                            app.update_camera_name_from_properties(name_val);
                        }

                        ui.label(egui::RichText::new("Group:").strong().size(11.0).color(Color32::from_rgb(71, 85, 105)));
                        ui.label(egui::RichText::new(&app.current_camera_settings.group).size(11.0).color(Color32::from_rgb(217, 119, 6)));
                    });
                    ui.add_space(2.0);

                    // Table with General, Video stream 1, Video stream 2 - ALL EDITABLE!
                    let table_height = (top_height - 100.0).max(60.0);
                    egui::ScrollArea::vertical()
                        .max_height(table_height)
                        .auto_shrink([false, false])
                        .show(ui, |ui| {
                            ui.set_width(ui.available_width());

                            // Section 1: General (Sliders & Live Inputs)
                            egui::CollapsingHeader::new(
                                egui::RichText::new("General").strong().size(11.0).color(Color32::from_rgb(30, 41, 59))
                            )
                            .id_salt("prop_section_general")
                            .default_open(true)
                            .show(ui, |ui| {
                                ui.horizontal(|ui| {
                                    ui.set_min_width(ui.available_width());
                                    ui.label(egui::RichText::new("Brightness").size(10.5).color(Color32::from_rgb(51, 65, 85)));
                                    ui.with_layout(egui::Layout::right_to_left(egui::Align::Center), |ui| {
                                        ui.add(egui::Slider::new(&mut app.current_camera_settings.brightness, 0..=100));
                                    });
                                });
                                ui.separator();

                                ui.horizontal(|ui| {
                                    ui.set_min_width(ui.available_width());
                                    ui.label(egui::RichText::new("Contrast").size(10.5).color(Color32::from_rgb(51, 65, 85)));
                                    ui.with_layout(egui::Layout::right_to_left(egui::Align::Center), |ui| {
                                        ui.add(egui::Slider::new(&mut app.current_camera_settings.contrast, 0..=100));
                                    });
                                });
                                ui.separator();

                                ui.horizontal(|ui| {
                                    ui.set_min_width(ui.available_width());
                                    ui.label(egui::RichText::new("Multicast address").size(10.5).color(Color32::from_rgb(51, 65, 85)));
                                    ui.with_layout(egui::Layout::right_to_left(egui::Align::Center), |ui| {
                                        ui.text_edit_singleline(&mut app.current_camera_settings.multicast_ip);
                                    });
                                });
                                ui.separator();

                                ui.horizontal(|ui| {
                                    ui.set_min_width(ui.available_width());
                                    ui.label(egui::RichText::new("Saturation").size(10.5).color(Color32::from_rgb(51, 65, 85)));
                                    ui.with_layout(egui::Layout::right_to_left(egui::Align::Center), |ui| {
                                        ui.add(egui::Slider::new(&mut app.current_camera_settings.saturation, 0..=100));
                                    });
                                });
                                ui.separator();

                                ui.horizontal(|ui| {
                                    ui.set_min_width(ui.available_width());
                                    ui.label(egui::RichText::new("Sharpness").size(10.5).color(Color32::from_rgb(51, 65, 85)));
                                    ui.with_layout(egui::Layout::right_to_left(egui::Align::Center), |ui| {
                                        ui.add(egui::Slider::new(&mut app.current_camera_settings.sharpness, 0..=100));
                                    });
                                });
                                ui.separator();
                            });

                            ui.add_space(4.0);

                            // Section 2: Video stream 1 (Main Stream - Live Editable Controls)
                            egui::CollapsingHeader::new(
                                egui::RichText::new("Video stream 1").strong().size(11.0).color(Color32::from_rgb(30, 41, 59))
                            )
                            .id_salt("prop_section_video_stream1")
                            .default_open(true)
                            .show(ui, |ui| {
                                ui.horizontal(|ui| {
                                    ui.set_min_width(ui.available_width());
                                    ui.label(egui::RichText::new("Codec").size(10.5).color(Color32::from_rgb(51, 65, 85)));
                                    ui.with_layout(egui::Layout::right_to_left(egui::Align::Center), |ui| {
                                        egui::ComboBox::from_id_salt("codec_stream1")
                                            .selected_text(&app.current_camera_settings.codec)
                                            .show_ui(ui, |ui| {
                                                ui.selectable_value(&mut app.current_camera_settings.codec, "H.265".to_string(), "H.265");
                                                ui.selectable_value(&mut app.current_camera_settings.codec, "H.264".to_string(), "H.264");
                                                ui.selectable_value(&mut app.current_camera_settings.codec, "MJPEG".to_string(), "MJPEG");
                                            });
                                    });
                                });
                                ui.separator();

                                ui.horizontal(|ui| {
                                    ui.set_min_width(ui.available_width());
                                    ui.label(egui::RichText::new("Control mode").size(10.5).color(Color32::from_rgb(51, 65, 85)));
                                    ui.with_layout(egui::Layout::right_to_left(egui::Align::Center), |ui| {
                                        egui::ComboBox::from_id_salt("ctrl_mode")
                                            .selected_text(&app.current_camera_settings.bitrate_mode)
                                            .show_ui(ui, |ui| {
                                                ui.selectable_value(&mut app.current_camera_settings.bitrate_mode, "Variable bit rate".to_string(), "Variable bit rate");
                                                ui.selectable_value(&mut app.current_camera_settings.bitrate_mode, "Constant bit rate".to_string(), "Constant bit rate");
                                            });
                                    });
                                });
                                ui.separator();

                                ui.horizontal(|ui| {
                                    ui.set_min_width(ui.available_width());
                                    ui.label(egui::RichText::new("Frames per second").size(10.5).color(Color32::from_rgb(51, 65, 85)));
                                    ui.with_layout(egui::Layout::right_to_left(egui::Align::Center), |ui| {
                                        ui.add(egui::Slider::new(&mut app.current_camera_settings.fps, 1..=60).suffix(" fps"));
                                    });
                                });
                                ui.separator();

                                ui.horizontal(|ui| {
                                    ui.set_min_width(ui.available_width());
                                    ui.label(egui::RichText::new("Multicast port").size(10.5).color(Color32::from_rgb(51, 65, 85)));
                                    ui.with_layout(egui::Layout::right_to_left(egui::Align::Center), |ui| {
                                        ui.add(egui::DragValue::new(&mut app.current_camera_settings.port1));
                                    });
                                });
                                ui.separator();

                                ui.horizontal(|ui| {
                                    ui.set_min_width(ui.available_width());
                                    ui.label(egui::RichText::new("Quality").size(10.5).color(Color32::from_rgb(51, 65, 85)));
                                    ui.with_layout(egui::Layout::right_to_left(egui::Align::Center), |ui| {
                                        ui.add(egui::Slider::new(&mut app.current_camera_settings.quality, 1..=100));
                                    });
                                });
                                ui.separator();

                                ui.horizontal(|ui| {
                                    ui.set_min_width(ui.available_width());
                                    ui.label(egui::RichText::new("Resolution").size(10.5).color(Color32::from_rgb(51, 65, 85)));
                                    ui.with_layout(egui::Layout::right_to_left(egui::Align::Center), |ui| {
                                        egui::ComboBox::from_id_salt("res_stream1")
                                            .selected_text(&app.current_camera_settings.resolution)
                                            .show_ui(ui, |ui| {
                                                ui.selectable_value(&mut app.current_camera_settings.resolution, "3840x2160 (4K UHD)".to_string(), "3840x2160 (4K UHD)");
                                                ui.selectable_value(&mut app.current_camera_settings.resolution, "2688x1520 (4MP QHD)".to_string(), "2688x1520 (4MP QHD)");
                                                ui.selectable_value(&mut app.current_camera_settings.resolution, "1920x1080 (1080p FHD)".to_string(), "1920x1080 (1080p FHD)");
                                                ui.selectable_value(&mut app.current_camera_settings.resolution, "1280x720 (720p HD)".to_string(), "1280x720 (720p HD)");
                                            });
                                    });
                                });
                                ui.separator();

                                ui.horizontal(|ui| {
                                    ui.set_min_width(ui.available_width());
                                    ui.label(egui::RichText::new("Streaming method").size(10.5).color(Color32::from_rgb(51, 65, 85)));
                                    ui.with_layout(egui::Layout::right_to_left(egui::Align::Center), |ui| {
                                        egui::ComboBox::from_id_salt("method_stream1")
                                            .selected_text(&app.current_camera_settings.streaming_method)
                                            .show_ui(ui, |ui| {
                                                ui.selectable_value(&mut app.current_camera_settings.streaming_method, "RTP/RTSP/TCP".to_string(), "RTP/RTSP/TCP");
                                                ui.selectable_value(&mut app.current_camera_settings.streaming_method, "RTP/RTSP/HTTP/TCP".to_string(), "RTP/RTSP/HTTP/TCP");
                                                ui.selectable_value(&mut app.current_camera_settings.streaming_method, "RTP/UDP".to_string(), "RTP/UDP");
                                            });
                                    });
                                });
                                ui.separator();
                            });

                            ui.add_space(4.0);

                            // Section 3: Video stream 2 (Sub Stream - Live Editable Controls)
                            egui::CollapsingHeader::new(
                                egui::RichText::new("Video stream 2").strong().size(11.0).color(Color32::from_rgb(30, 41, 59))
                            )
                            .id_salt("prop_section_video_stream2")
                            .default_open(true)
                            .show(ui, |ui| {
                                ui.horizontal(|ui| {
                                    ui.set_min_width(ui.available_width());
                                    ui.label(egui::RichText::new("Frames per second").size(10.5).color(Color32::from_rgb(51, 65, 85)));
                                    ui.with_layout(egui::Layout::right_to_left(egui::Align::Center), |ui| {
                                        ui.add(egui::Slider::new(&mut app.current_camera_settings.stream2_fps, 1..=30).suffix(" fps"));
                                    });
                                });
                                ui.separator();

                                ui.horizontal(|ui| {
                                    ui.set_min_width(ui.available_width());
                                    ui.label(egui::RichText::new("Multicast port").size(10.5).color(Color32::from_rgb(51, 65, 85)));
                                    ui.with_layout(egui::Layout::right_to_left(egui::Align::Center), |ui| {
                                        ui.add(egui::DragValue::new(&mut app.current_camera_settings.port2));
                                    });
                                });
                                ui.separator();

                                ui.horizontal(|ui| {
                                    ui.set_min_width(ui.available_width());
                                    ui.label(egui::RichText::new("Resolution").size(10.5).color(Color32::from_rgb(51, 65, 85)));
                                    ui.with_layout(egui::Layout::right_to_left(egui::Align::Center), |ui| {
                                        egui::ComboBox::from_id_salt("res_stream2")
                                            .selected_text(&app.current_camera_settings.stream2_res)
                                            .show_ui(ui, |ui| {
                                                ui.selectable_value(&mut app.current_camera_settings.stream2_res, "640x360".to_string(), "640x360");
                                                ui.selectable_value(&mut app.current_camera_settings.stream2_res, "704x576 (D1)".to_string(), "704x576 (D1)");
                                                ui.selectable_value(&mut app.current_camera_settings.stream2_res, "352x288 (CIF)".to_string(), "352x288 (CIF)");
                                            });
                                    });
                                });
                                ui.separator();
                            });
                        });

                    // Bottom Tabs: [ℹ Info, ⚙ Settings, 📹 Streams, 🔴 Record, 🏃 Motion, 👁 Client]
                    ui.add_space(4.0);
                    ui.separator();
                    ui.horizontal(|ui| {
                        ui.style_mut().spacing.item_spacing = Vec2::new(6.0, 0.0);
                        render_camera_tab_btn(ui, app, "ℹ Info", CameraTab::Info);
                        render_camera_tab_btn(ui, app, "⚙ Settings", CameraTab::Settings);
                        render_camera_tab_btn(ui, app, "📹 Streams", CameraTab::Streams);
                        render_camera_tab_btn(ui, app, "🔴 Record", CameraTab::Record);
                        render_camera_tab_btn(ui, app, "🏃 Motion", CameraTab::Motion);
                        render_camera_tab_btn(ui, app, "👁 Client", CameraTab::Client);
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
    // Bottom Dock: AUTHENTIC MILESTONE EXACT PREVIEW PANE (Matching User Image 100%)
    // -------------------------------------------------------------------------
    ui.allocate_ui(Vec2::new(available_width, app.bottom_dock_height), |ui| {
        render_exact_milestone_preview_pane(app, ui);
    });

    // Modals
    render_modals(app, ui.ctx());
}

fn render_modals(app: &mut ManagementClientApp, ctx: &egui::Context) {
    // 1. Add Device Group Modal (Ctrl+N)
    if app.show_add_device_group_modal {
        egui::Window::new("🪄 Add Device Group")
            .id(egui::Id::new("modal_add_device_group"))
            .fixed_size(Vec2::new(380.0, 160.0))
            .collapsible(false)
            .resizable(false)
            .anchor(egui::Align2::CENTER_CENTER, Vec2::ZERO)
            .show(ctx, |ui| {
                ui.label(egui::RichText::new("Enter name for the new Device Group:").strong());
                ui.add_space(8.0);
                ui.text_edit_singleline(&mut app.new_device_group_name);
                ui.add_space(16.0);
                ui.separator();
                ui.horizontal(|ui| {
                    if ui.button("Cancel").clicked() {
                        app.show_add_device_group_modal = false;
                    }
                    ui.with_layout(egui::Layout::right_to_left(egui::Align::Center), |ui| {
                        let btn_add = egui::Button::new(
                            egui::RichText::new("💾 Create Group").strong().color(Color32::WHITE),
                        ).fill(Color32::from_rgb(16, 185, 129));
                        if ui.add(btn_add).clicked() {
                            app.add_device_group_from_modal();
                        }
                    });
                });
            });
    }

    // 2. Edit Device Group Members Modal (Exact Milestone Matching)
    if app.show_edit_members_modal {
        egui::Window::new(format!("👥 Edit Device Group Members — {}", app.edit_members_group_name))
            .id(egui::Id::new("modal_edit_device_group_members"))
            .fixed_size(Vec2::new(480.0, 380.0))
            .collapsible(false)
            .resizable(false)
            .anchor(egui::Align2::CENTER_CENTER, Vec2::ZERO)
            .show(ctx, |ui| {
                ui.label(egui::RichText::new(format!("Select cameras to include in '{}':", app.edit_members_group_name)).strong());
                ui.add_space(8.0);
                ui.separator();
                ui.add_space(6.0);

                egui::ScrollArea::vertical().max_height(240.0).show(ui, |ui| {
                    for i in 0..app.edit_members_available_cams.len() {
                        let cam_name = app.edit_members_available_cams[i].clone();
                        let check = &mut app.edit_members_selected_cams[i];
                        ui.horizontal(|ui| {
                            ui.checkbox(check, "");
                            ui.label(egui::RichText::new("📹").size(11.0).color(Color32::from_rgb(0, 102, 204)));
                            ui.label(egui::RichText::new(cam_name).size(11.0));
                        });
                    }
                });

                ui.add_space(12.0);
                ui.separator();
                ui.horizontal(|ui| {
                    if ui.button("Cancel").clicked() {
                        app.show_edit_members_modal = false;
                    }
                    ui.with_layout(egui::Layout::right_to_left(egui::Align::Center), |ui| {
                        let btn_save = egui::Button::new(
                            egui::RichText::new("💾 Save Group Members").strong().color(Color32::WHITE),
                        ).fill(Color32::from_rgb(0, 120, 215));
                        if ui.add(btn_save).clicked() {
                            app.show_edit_members_modal = false;
                        }
                    });
                });
            });
    }

    // 3. Rename Device Group Modal (F2)
    if app.show_rename_device_group_modal {
        egui::Window::new("✏ Rename Device Group")
            .id(egui::Id::new("modal_rename_device_group"))
            .fixed_size(Vec2::new(380.0, 160.0))
            .collapsible(false)
            .resizable(false)
            .anchor(egui::Align2::CENTER_CENTER, Vec2::ZERO)
            .show(ctx, |ui| {
                ui.label(egui::RichText::new("Enter new name for the Device Group:").strong());
                ui.add_space(8.0);
                ui.text_edit_singleline(&mut app.rename_device_group_name);
                ui.add_space(16.0);
                ui.separator();
                ui.horizontal(|ui| {
                    if ui.button("Cancel").clicked() {
                        app.show_rename_device_group_modal = false;
                    }
                    ui.with_layout(egui::Layout::right_to_left(egui::Align::Center), |ui| {
                        let btn_save = egui::Button::new(
                            egui::RichText::new("💾 Save Name").strong().color(Color32::WHITE),
                        ).fill(Color32::from_rgb(0, 120, 215));
                        if ui.add(btn_save).clicked() {
                            app.rename_device_group_from_modal();
                        }
                    });
                });
            });
    }
}

fn render_camera_tab_btn(ui: &mut egui::Ui, app: &mut ManagementClientApp, label: &str, tab: CameraTab) {
    let is_active = app.camera_tab == tab;
    let btn = egui::Button::new(
        egui::RichText::new(label)
            .strong()
            .size(10.5)
            .color(if is_active { Color32::from_rgb(0, 102, 204) } else { Color32::from_rgb(71, 85, 105) })
    ).fill(if is_active { Color32::from_rgb(224, 242, 254) } else { Color32::TRANSPARENT });

    if ui.add(btn).clicked() {
        app.camera_tab = tab;
    }
}

// -----------------------------------------------------------------------------
// 3. Bottom Panel: AUTHENTIC MILESTONE PREVIEW (EXACT USER SCREENSHOT MATCH)
// -----------------------------------------------------------------------------
fn render_exact_milestone_preview_pane(app: &mut ManagementClientApp, ui: &mut egui::Ui) {
    let cam = &app.current_camera_settings;

    // Milestone Preview Header Bar: [Preview] -------------------------- [▾ ✕]
    ui.horizontal(|ui| {
        ui.label(egui::RichText::new("Preview").size(11.5).color(Color32::from_rgb(15, 23, 42)));
        ui.with_layout(egui::Layout::right_to_left(egui::Align::Center), |ui| {
            ui.label(egui::RichText::new("✕").size(11.0).color(Color32::from_rgb(100, 116, 139)));
            ui.label(egui::RichText::new("▾").size(11.0).color(Color32::from_rgb(100, 116, 139)));
        });
    });

    ui.add_space(2.0);
    ui.separator();

    let avail_width = ui.available_width();
    let avail_height = (ui.available_height() - 24.0).max(100.0);

    ui.vertical_centered(|ui| {
        // Top Center Green Metadata: "Live 1920x1080 14KB"
        ui.label(
            egui::RichText::new(format!("Live  {} 14KB", cam.resolution.split(' ').next().unwrap_or("1920x1080")))
                .size(10.0)
                .color(Color32::from_rgb(22, 163, 74)),
        );

        ui.add_space(2.0);

        // Calculate 16:9 proportional video frame safely
        let target_w = (avail_height - 32.0).max(50.0) * (16.0 / 9.0);
        let max_frame_w = clamp_safe(avail_width - 60.0, 100.0, target_w.max(100.0));
        let max_frame_h = clamp_safe(max_frame_w * (9.0 / 16.0), 60.0, (avail_height - 30.0).max(60.0));
        let frame_size = Vec2::new(max_frame_w, max_frame_h);

        let (rect, _) = ui.allocate_exact_size(frame_size, egui::Sense::hover());
        let painter = ui.painter();

        // 1. Sky & Atmosphere
        let sky_top = Color32::from_rgb(148, 197, 233);
        painter.rect_filled(rect, Rounding::ZERO, sky_top);
        
        let horizon_y = rect.min.y + rect.height() * 0.40;
        let ground_rect = egui::Rect::from_min_max(Pos2::new(rect.min.x, horizon_y), rect.max);
        painter.rect_filled(ground_rect, Rounding::ZERO, Color32::from_rgb(34, 75, 40));

        // 2. Background Hill (Left / Quarry)
        let hill_top = Pos2::new(rect.min.x + rect.width() * 0.22, horizon_y - rect.height() * 0.15);
        painter.line_segment([Pos2::new(rect.min.x, horizon_y), hill_top], Stroke::new(2.0_f32, Color32::from_rgb(85, 125, 60)));
        painter.line_segment([hill_top, Pos2::new(rect.min.x + rect.width() * 0.45, horizon_y)], Stroke::new(2.0_f32, Color32::from_rgb(90, 95, 90)));

        // 3. Multi-story Residential / Facility Building (Peach / Beige with Balconies)
        let bldg_min_x = rect.min.x + rect.width() * 0.48;
        let bldg_max_x = rect.max.x - 2.0;
        let bldg_min_y = horizon_y - rect.height() * 0.35;
        let bldg_rect = egui::Rect::from_min_max(Pos2::new(bldg_min_x, bldg_min_y), Pos2::new(bldg_max_x, horizon_y + rect.height() * 0.15));
        painter.rect_filled(bldg_rect, Rounding::ZERO, Color32::from_rgb(222, 178, 142)); // Peach building

        // Balconies & Windows
        let num_floors = 4;
        let num_cols = 7;
        let floor_h = (bldg_rect.height()) / num_floors as f32;
        let col_w = (bldg_rect.width()) / num_cols as f32;

        for f in 0..num_floors {
            let y = bldg_rect.min.y + (f as f32) * floor_h + 3.0;
            for c in 0..num_cols {
                let x = bldg_rect.min.x + (c as f32) * col_w + 3.0;
                let win_rect = egui::Rect::from_min_size(Pos2::new(x, y), Vec2::new(col_w - 6.0, floor_h - 6.0));
                painter.rect_filled(win_rect, Rounding::ZERO, Color32::from_rgb(70, 75, 80));
                // Balcony railing
                painter.line_segment([Pos2::new(x, y + floor_h - 8.0), Pos2::new(x + col_w - 6.0, y + floor_h - 8.0)], Stroke::new(1.0_f32, Color32::from_rgb(230, 230, 230)));
            }
        }

        // 4. Concrete Compound Wall with Barbed Wire Fence
        let wall_min_y = horizon_y + rect.height() * 0.05;
        let wall_max_y = horizon_y + rect.height() * 0.28;
        let wall_rect = egui::Rect::from_min_max(Pos2::new(rect.min.x + rect.width() * 0.35, wall_min_y), Pos2::new(rect.max.x, wall_max_y));
        painter.rect_filled(wall_rect, Rounding::ZERO, Color32::from_rgb(120, 125, 125)); // Concrete grey wall
        
        // Barbed wire on top of wall
        for x_step in 0..15 {
            let wx = wall_rect.min.x + (x_step as f32) * (wall_rect.width() / 15.0);
            painter.line_segment([Pos2::new(wx, wall_min_y - 6.0), Pos2::new(wx + 8.0, wall_min_y)], Stroke::new(1.0_f32, Color32::from_rgb(180, 180, 180)));
        }

        // 5. Water Pond (Middle Ground)
        let pond_rect = egui::Rect::from_min_max(Pos2::new(rect.min.x + rect.width() * 0.32, wall_max_y), Pos2::new(rect.min.x + rect.width() * 0.65, wall_max_y + rect.height() * 0.15));
        painter.rect_filled(pond_rect, Rounding::ZERO, Color32::from_rgb(60, 95, 90));

        // 6. Lush Green Trees & Foliage (Foreground Left & Bottom)
        let tree_rect = egui::Rect::from_min_max(Pos2::new(rect.min.x, horizon_y - rect.height() * 0.10), Pos2::new(rect.min.x + rect.width() * 0.42, rect.max.y));
        painter.rect_filled(tree_rect, Rounding::ZERO, Color32::from_rgb(28, 70, 32));

        let fg_trees = egui::Rect::from_min_max(Pos2::new(rect.min.x, rect.max.y - rect.height() * 0.40), rect.max);
        painter.rect_filled(fg_trees, Rounding::ZERO, Color32::from_rgb(22, 60, 26));

        // Tree Leaf Texture Specks
        for s in 0..30 {
            let tx = rect.min.x + (s as f32 * 37.0) % rect.width();
            let ty = rect.max.y - ((s as f32 * 19.0) % (rect.height() * 0.35));
            painter.circle_filled(Pos2::new(tx, ty), 4.0, Color32::from_rgb(45, 95, 48));
        }

        // 7. Crisp Black Outer Video Border
        painter.rect_stroke(rect, Rounding::ZERO, Stroke::new(1.0_f32, Color32::from_rgb(20, 25, 30)));

        // 8. Top-Left CCTV Watermark: "27-08-2026 Thu 02:18:23 PM"
        let current_time = chrono::Local::now().format("%d-%m-%Y %a %I:%M:%S %p").to_string();
        // Drop shadow for crisp readability
        painter.text(
            Pos2::new(rect.min.x + 13.0, rect.min.y + 13.0),
            egui::Align2::LEFT_TOP,
            &current_time,
            egui::FontId::monospace(12.0),
            Color32::from_rgb(20, 20, 20),
        );
        painter.text(
            Pos2::new(rect.min.x + 12.0, rect.min.y + 12.0),
            egui::Align2::LEFT_TOP,
            &current_time,
            egui::FontId::monospace(12.0),
            Color32::BLACK,
        );

        // 9. Bottom-Right CCTV Watermark: Camera Name (e.g. "AIRTEL TOWER SIDE WALL CAM 2")
        painter.text(
            Pos2::new(rect.max.x - 11.0, rect.max.y - 11.0),
            egui::Align2::RIGHT_BOTTOM,
            &cam.name,
            egui::FontId::proportional(12.5),
            Color32::from_rgb(20, 20, 20),
        );
        painter.text(
            Pos2::new(rect.max.x - 12.0, rect.max.y - 12.0),
            egui::Align2::RIGHT_BOTTOM,
            &cam.name,
            egui::FontId::proportional(12.5),
            Color32::WHITE,
        );

        ui.add_space(4.0);

        // 10. Bottom Centered Label below video frame: "AIRTEL TOWER SIDE WALL CAM 2"
        ui.label(
            egui::RichText::new(&cam.name)
                .size(10.0)
                .color(Color32::from_rgb(15, 23, 42)),
        );
    });
}
