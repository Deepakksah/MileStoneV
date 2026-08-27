/**
 * LycheeVMS 2025 R3 — Interactive Application Logic & Video Engine
 */

(function () {
  'use strict';

  // State Management
  const state = {
    currentTab: 'live-matrix',
    currentGrid: '2x2',
    selectedCameraId: 'cam-1',
    aiOverlayEnabled: true,
    isPlaying: true,
    playbackSpeed: 1,
    zoomLevel: 1.0,
    panX: 0,
    panY: 0,
    cameras: [
      { id: 'cam-1', name: 'CAM-01 Entrance Main 4K', ip: '192.168.1.101', type: 'PTZ', codec: 'H.265', res: '4K @ 30fps', bank: 'BANK-01 (SSD)', retention: '60 Days', recording: true, status: 'Online', fps: 30, bitrate: '6.4 Mbps', scene: 'entrance' },
      { id: 'cam-2', name: 'CAM-02 North Perimeter Gate', ip: '192.168.1.102', type: 'Fixed', codec: 'H.264', res: '1080p @ 60fps', bank: 'BANK-01 (SSD)', retention: '30 Days', recording: true, status: 'Online', fps: 60, bitrate: '4.8 Mbps', scene: 'perimeter' },
      { id: 'cam-3', name: 'CAM-03 Server Room Corridor', ip: '192.168.1.103', type: 'AI Dome', codec: 'H.265', res: '1080p @ 60fps', bank: 'BANK-02 (NAS)', retention: '90 Days', recording: true, status: 'Online', fps: 60, bitrate: '3.9 Mbps', scene: 'corridor' },
      { id: 'cam-4', name: 'CAM-04 Warehouse Loading Bay', ip: '192.168.1.104', type: 'Thermal', codec: 'H.264', res: '720p @ 30fps', bank: 'BANK-02 (NAS)', retention: '45 Days', recording: true, status: 'Online', fps: 30, bitrate: '2.1 Mbps', scene: 'warehouse' },
      { id: 'cam-5', name: 'CAM-05 Parking Lot East LPR', ip: '192.168.1.105', type: 'LPR', codec: 'H.264', res: '1080p @ 60fps', bank: 'BANK-01 (SSD)', retention: '30 Days', recording: true, status: 'Online', fps: 60, bitrate: '5.2 Mbps', scene: 'parking' },
      { id: 'cam-6', name: 'CAM-06 Parking Lot West 360°', ip: '192.168.1.106', type: 'Fisheye', codec: 'H.265', res: '4K @ 30fps', bank: 'BANK-01 (SSD)', retention: '30 Days', recording: true, status: 'Online', fps: 30, bitrate: '7.1 Mbps', scene: 'parking' },
      { id: 'cam-7', name: 'CAM-07 Executive Floor Lobby', ip: '192.168.1.107', type: 'Fixed', codec: 'H.264', res: '1080p @ 60fps', bank: 'BANK-01 (SSD)', retention: '30 Days', recording: false, status: 'Online', fps: 60, bitrate: '3.5 Mbps', scene: 'corridor' },
      { id: 'cam-8', name: 'CAM-08 Rooftop Heli-Pad PTZ', ip: '192.168.1.108', type: 'PTZ', codec: 'H.265', res: '4K @ 30fps', bank: 'BANK-02 (NAS)', retention: '60 Days', recording: true, status: 'Online', fps: 30, bitrate: '6.8 Mbps', scene: 'perimeter' }
    ],
    alarms: [
      { id: 'ALM-8091', time: '12:08:12', event: 'Unauthorized Perimeter Crossing', device: 'CAM-02 North Perimeter Gate', severity: 'Critical', state: 'Active', details: 'AI Line-crossing rule triggered in restricted perimeter sector B.' },
      { id: 'ALM-8090', time: '12:06:40', event: 'License Plate Match (Watchlist)', device: 'CAM-05 Parking Lot East LPR', severity: 'Warning', state: 'Active', details: 'Matched Plate [MH 12 AB 4098] against VIP / Restricted database.' },
      { id: 'ALM-8089', time: '12:01:15', event: 'Motion in Restricted Server Rack', device: 'CAM-03 Server Room Corridor', severity: 'Warning', state: 'Active', details: 'Motion detected after scheduled business hours.' },
      { id: 'ALM-8088', time: '11:45:00', event: 'Storage Retention Archive Warning', device: 'Surveillance Database', severity: 'Info', state: 'Acknowledged', details: 'Bank 02 auto-archiving triggered to cold storage.' }
    ]
  };

  // DOM Elements
  const tabs = document.querySelectorAll('.tab-btn');
  const panels = document.querySelectorAll('.view-panel');
  const matrixContainer = document.getElementById('camera-matrix');
  const playbackMatrixContainer = document.getElementById('playback-matrix');
  const gridBtns = document.querySelectorAll('.grid-btn');
  const deviceTableBody = document.getElementById('device-table-body');
  const alarmTableBody = document.getElementById('alarm-table-body');
  const alarmCounter = document.getElementById('alarm-counter');

  // PTZ Elements
  const sliderZoom = document.getElementById('slider-zoom');
  const zoomValDisplay = document.getElementById('zoom-val');
  const ptzBtns = document.querySelectorAll('[data-ptz]');
  const presetBtns = document.querySelectorAll('[data-preset]');
  const selectedCamPtzTag = document.getElementById('selected-cam-ptz-tag');

  // Modal Elements
  const deviceModal = document.getElementById('device-modal');
  const btnAddDevice = document.getElementById('btn-add-camera');
  const btnAddDeviceModal = document.getElementById('btn-add-device-modal');
  const btnCloseModal = document.getElementById('btn-close-modal');
  const btnCancelModal = document.getElementById('btn-cancel-modal');
  const btnSaveDevice = document.getElementById('btn-save-device');

  // Timeline Elements
  const timelineCanvas = document.getElementById('timeline-canvas');
  const timelineTrack = document.getElementById('timeline-track');
  const timelineScrubber = document.getElementById('timeline-scrubber');
  const playbackTimeDisplay = document.getElementById('playback-time-display');
  const btnPlayPause = document.getElementById('btn-play-pause');

  /* =========================================================================
     1. Tab Switching
     ========================================================================= */
  tabs.forEach(btn => {
    btn.addEventListener('click', () => {
      tabs.forEach(t => t.classList.remove('active'));
      panels.forEach(p => p.classList.remove('active'));

      btn.classList.add('active');
      const targetId = `panel-${btn.dataset.tab}`;
      const targetPanel = document.getElementById(targetId);
      if (targetPanel) {
        targetPanel.classList.add('active');
      }
      state.currentTab = btn.dataset.tab;

      if (state.currentTab === 'playback-view') {
        renderPlaybackGrid();
        drawTimeline();
      }
    });
  });

  /* =========================================================================
     2. Grid Layout Manager & Camera Renderer
     ========================================================================= */
  gridBtns.forEach(btn => {
    btn.addEventListener('click', () => {
      gridBtns.forEach(b => b.classList.remove('active'));
      btn.classList.add('active');
      state.currentGrid = btn.dataset.grid;
      renderMatrixGrid();
    });
  });

  function getGridCapacity(gridType) {
    switch (gridType) {
      case '1x1': return 1;
      case '2x2': return 4;
      case '1p5': return 6;
      case '3x3': return 9;
      case '4x4': return 16;
      default: return 4;
    }
  }

  function renderMatrixGrid() {
    matrixContainer.className = `matrix-container grid-${state.currentGrid}`;
    matrixContainer.innerHTML = '';

    const count = getGridCapacity(state.currentGrid);
    for (let i = 0; i < count; i++) {
      const cam = state.cameras[i % state.cameras.length];
      const cell = createCameraCell(cam, i);
      matrixContainer.appendChild(cell);
    }
  }

  function renderPlaybackGrid() {
    playbackMatrixContainer.className = `matrix-container grid-2x2`;
    playbackMatrixContainer.innerHTML = '';

    for (let i = 0; i < 4; i++) {
      const cam = state.cameras[i % state.cameras.length];
      const cell = createCameraCell(cam, i, true);
      playbackMatrixContainer.appendChild(cell);
    }
  }

  function createCameraCell(cam, index, isPlayback = false) {
    const cell = document.createElement('div');
    cell.className = `camera-cell ${cam.id === state.selectedCameraId ? 'selected' : ''}`;
    cell.dataset.camId = cam.id;

    cell.innerHTML = `
      <div class="camera-header-overlay">
        <div class="cam-name-tag">
          <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M23 7l-7 5 7 5V7z"/><rect x="1" y="5" width="15" height="14" rx="2" ry="2"/></svg>
          <span>${cam.name}</span>
        </div>
        ${cam.recording ? '<div class="rec-indicator"><div class="rec-dot"></div><span>REC</span></div>' : ''}
      </div>

      <canvas class="cam-canvas" id="canvas-${isPlayback ? 'pb-' : ''}${cam.id}-${index}"></canvas>

      <div class="camera-footer-overlay">
        <div class="cam-telemetry">
          <span>${cam.res}</span>
          <span>${cam.codec}</span>
          <span class="cam-fps">${cam.fps} FPS</span>
          <span>${cam.bitrate}</span>
        </div>
        <div class="cam-controls-quick">
          <button class="mini-icon-btn btn-ptz-select" title="Control PTZ">🕹️</button>
          <button class="mini-icon-btn" title="Snapshot">📸</button>
          <button class="mini-icon-btn" title="Full Screen">⛶</button>
        </div>
      </div>
    `;

    cell.addEventListener('click', () => {
      document.querySelectorAll('.camera-cell').forEach(c => c.classList.remove('selected'));
      cell.classList.add('selected');
      state.selectedCameraId = cam.id;
      if (selectedCamPtzTag) selectedCamPtzTag.textContent = cam.name.split(' ')[0];

      // Update active in sidebar
      document.querySelectorAll('.tree-item').forEach(ti => {
        ti.classList.toggle('active', ti.dataset.camId === cam.id);
      });
    });

    return cell;
  }

  /* =========================================================================
     3. Canvas Video Feed Animation Engine (Procedural Realistic Feeds)
     ========================================================================= */
  let animationFrameId;

  function startVideoSimulation() {
    let tick = 0;

    function renderLoop() {
      tick++;
      const canvases = document.querySelectorAll('.cam-canvas');
      const now = new Date();
      const timeString = now.toISOString().replace('T', ' ').substring(0, 19);

      canvases.forEach(canvas => {
        if (!canvas.getContext) return;
        const ctx = canvas.getContext('2d');
        const width = canvas.width = canvas.clientWidth || 320;
        const height = canvas.height = canvas.clientHeight || 180;

        const camId = canvas.id.split('-')[1] || 'cam';
        const camData = state.cameras.find(c => c.id === camId) || state.cameras[0];

        // Draw dynamic background scenery
        drawCameraScene(ctx, width, height, camData.scene, tick, camData.id === state.selectedCameraId ? state.panX : 0, camData.id === state.selectedCameraId ? state.panY : 0, camData.id === state.selectedCameraId ? state.zoomLevel : 1.0);

        // Draw Simulated AI Object Detection Bounding Boxes
        if (state.aiOverlayEnabled) {
          drawAIAnalytics(ctx, width, height, camData.scene, tick);
        }

        // Draw Timestamp Overlay on Top Right of Video
        ctx.fillStyle = 'rgba(0, 0, 0, 0.6)';
        ctx.fillRect(width - 170, 8, 162, 20);
        ctx.fillStyle = '#f8fafc';
        ctx.font = '10px JetBrains Mono, monospace';
        ctx.fillText(timeString, width - 164, 22);
      });

      animationFrameId = requestAnimationFrame(renderLoop);
    }

    renderLoop();
  }

  function drawCameraScene(ctx, w, h, scene, tick, panX, panY, zoom) {
    ctx.save();
    
    // Base dark background
    ctx.fillStyle = '#0a0e17';
    ctx.fillRect(0, 0, w, h);

    // Apply PTZ Transforms
    ctx.translate(w / 2, h / 2);
    ctx.scale(zoom, zoom);
    ctx.translate(-w / 2 + panX, -h / 2 + panY);

    if (scene === 'entrance') {
      // Perspective building entrance & glass doors
      const grad = ctx.createLinearGradient(0, 0, 0, h);
      grad.addColorStop(0, '#131c2e');
      grad.addColorStop(1, '#080c14');
      ctx.fillStyle = grad;
      ctx.fillRect(0, 0, w, h);

      // Floor tiles
      ctx.strokeStyle = 'rgba(255, 255, 255, 0.08)';
      ctx.lineWidth = 1;
      for (let y = h * 0.55; y < h; y += 15) {
        ctx.beginPath();
        ctx.moveTo(0, y);
        ctx.lineTo(w, y);
        ctx.stroke();
      }

      // Pillars & Glass Doors
      ctx.fillStyle = '#1e293b';
      ctx.fillRect(w * 0.1, h * 0.2, w * 0.15, h * 0.6);
      ctx.fillRect(w * 0.75, h * 0.2, w * 0.15, h * 0.6);

      // Moving human figure
      const personX = (w * 0.45) + Math.sin(tick * 0.03) * (w * 0.12);
      const personY = h * 0.62;
      drawHumanSilhouette(ctx, personX, personY);
    } 
    else if (scene === 'parking') {
      // Asphalt & road markings
      ctx.fillStyle = '#0f172a';
      ctx.fillRect(0, 0, w, h);

      // Parking bays
      ctx.strokeStyle = 'rgba(245, 158, 11, 0.4)';
      ctx.lineWidth = 2;
      for (let i = 0; i < 4; i++) {
        ctx.strokeRect(w * 0.15 + i * (w * 0.2), h * 0.45, w * 0.16, h * 0.35);
      }

      // Vehicle
      const carX = (w * 0.35) + Math.sin(tick * 0.02) * 10;
      ctx.fillStyle = '#334155';
      ctx.fillRect(carX, h * 0.5, w * 0.14, h * 0.22);
      ctx.fillStyle = '#e2e8f0';
      ctx.fillRect(carX + 4, h * 0.52, w * 0.12, h * 0.08); // windshield
    }
    else if (scene === 'corridor') {
      // Server Room corridor perspective
      ctx.fillStyle = '#0b0f19';
      ctx.fillRect(0, 0, w, h);

      // Perspective lines
      ctx.strokeStyle = 'rgba(6, 182, 212, 0.2)';
      ctx.beginPath();
      ctx.moveTo(0, 0); ctx.lineTo(w * 0.5, h * 0.45);
      ctx.moveTo(w, 0); ctx.lineTo(w * 0.5, h * 0.45);
      ctx.moveTo(0, h); ctx.lineTo(w * 0.5, h * 0.45);
      ctx.moveTo(w, h); ctx.lineTo(w * 0.5, h * 0.45);
      ctx.stroke();

      // Server rack blinking LEDs
      for (let i = 0; i < 6; i++) {
        const ledOn = (tick + i * 5) % 20 < 10;
        ctx.fillStyle = ledOn ? '#10b981' : '#064e3b';
        ctx.fillRect(w * 0.15, h * 0.25 + i * 15, 6, 4);
        ctx.fillStyle = (tick + i * 3) % 30 < 15 ? '#e11d48' : '#4c0519';
        ctx.fillRect(w * 0.82, h * 0.25 + i * 15, 6, 4);
      }
    }
    else {
      // Perimeter fence & outdoor
      ctx.fillStyle = '#050a14';
      ctx.fillRect(0, 0, w, h);

      // Fence mesh lines
      ctx.strokeStyle = 'rgba(255, 255, 255, 0.06)';
      for (let x = 0; x < w; x += 12) {
        ctx.beginPath(); ctx.moveTo(x, h * 0.4); ctx.lineTo(x, h); ctx.stroke();
      }
    }

    ctx.restore();
  }

  function drawHumanSilhouette(ctx, x, y) {
    ctx.fillStyle = '#475569';
    // Head
    ctx.beginPath();
    ctx.arc(x, y - 28, 6, 0, Math.PI * 2);
    ctx.fill();
    // Body
    ctx.fillRect(x - 5, y - 20, 10, 20);
  }

  function drawAIAnalytics(ctx, w, h, scene, tick) {
    ctx.save();
    ctx.lineWidth = 1.5;

    if (scene === 'entrance') {
      const personX = (w * 0.45) + Math.sin(tick * 0.03) * (w * 0.12);
      const personY = h * 0.62;

      // Draw bounding box
      ctx.strokeStyle = '#e11d48';
      ctx.strokeRect(personX - 12, personY - 38, 24, 46);

      // Label
      ctx.fillStyle = 'rgba(225, 29, 72, 0.85)';
      ctx.fillRect(personX - 12, personY - 50, 68, 12);
      ctx.fillStyle = '#fff';
      ctx.font = '8px Plus Jakarta Sans, sans-serif';
      ctx.fillText('Person 98.4%', personX - 10, personY - 41);
    } 
    else if (scene === 'parking') {
      const carX = (w * 0.35) + Math.sin(tick * 0.02) * 10;
      ctx.strokeStyle = '#06b6d4';
      ctx.strokeRect(carX - 4, h * 0.48, w * 0.15, h * 0.26);

      ctx.fillStyle = 'rgba(6, 182, 212, 0.85)';
      ctx.fillRect(carX - 4, h * 0.48 - 12, 85, 12);
      ctx.fillStyle = '#000';
      ctx.font = 'bold 8px Plus Jakarta Sans, sans-serif';
      ctx.fillText('LPR: MH 12 AB 4098', carX - 2, h * 0.48 - 3);
    }

    ctx.restore();
  }

  /* =========================================================================
     4. PTZ Joystick & Preset Interactions
     ========================================================================= */
  ptzBtns.forEach(btn => {
    btn.addEventListener('click', () => {
      const dir = btn.dataset.ptz;
      const step = 20;
      if (dir === 'up') state.panY += step;
      if (dir === 'down') state.panY -= step;
      if (dir === 'left') state.panX += step;
      if (dir === 'right') state.panX -= step;
      if (dir === 'home') { state.panX = 0; state.panY = 0; state.zoomLevel = 1.0; sliderZoom.value = 1; zoomValDisplay.textContent = '1.0x'; }
    });
  });

  sliderZoom.addEventListener('input', (e) => {
    state.zoomLevel = parseFloat(e.target.value);
    zoomValDisplay.textContent = `${state.zoomLevel.toFixed(1)}x`;
  });

  presetBtns.forEach(btn => {
    btn.addEventListener('click', () => {
      presetBtns.forEach(b => b.style.borderColor = 'rgba(255, 255, 255, 0.07)');
      btn.style.borderColor = '#e11d48';

      const preset = btn.dataset.preset;
      if (preset === 'entrance') { state.panX = 0; state.panY = 0; state.zoomLevel = 1.2; }
      if (preset === 'parking') { state.panX = 40; state.panY = -20; state.zoomLevel = 2.0; }
      if (preset === 'perimeter') { state.panX = -50; state.panY = 10; state.zoomLevel = 2.5; }
      if (preset === 'lobby') { state.panX = 20; state.panY = 30; state.zoomLevel = 1.5; }

      sliderZoom.value = state.zoomLevel;
      zoomValDisplay.textContent = `${state.zoomLevel.toFixed(1)}x`;
    });
  });

  /* =========================================================================
     5. Device Explorer Table Rendering
     ========================================================================= */
  function renderDeviceTable() {
    deviceTableBody.innerHTML = '';
    state.cameras.forEach(cam => {
      const tr = document.createElement('tr');
      tr.innerHTML = `
        <td><strong style="color: #fff;">${cam.name}</strong></td>
        <td><code>${cam.ip}</code></td>
        <td><span class="device-badge">${cam.codec}</span></td>
        <td>${cam.res}</td>
        <td>${cam.bank}</td>
        <td>${cam.retention}</td>
        <td><span class="badge" style="background: rgba(16,185,129,0.15); color: #10b981;">Online</span></td>
        <td>
          <button class="action-pill-btn" style="padding: 0.25rem 0.5rem; font-size: 0.72rem;" onclick="alert('Camera settings updated in Surveillance DB!')">Configure</button>
        </td>
      `;
      deviceTableBody.appendChild(tr);
    });
  }

  /* =========================================================================
     6. Alarm Center Real-Time Table & Actions
     ========================================================================= */
  function renderAlarmTable() {
    alarmTableBody.innerHTML = '';
    state.alarms.forEach(alm => {
      const tr = document.createElement('tr');
      const badgeClass = alm.severity === 'Critical' ? 'badge-critical' : alm.severity === 'Warning' ? 'badge-warning' : 'badge-info';
      tr.innerHTML = `
        <td><code>${alm.time}</code></td>
        <td><strong style="color: #fff;">${alm.event}</strong></td>
        <td>${alm.device}</td>
        <td><span class="badge ${badgeClass}">${alm.severity}</span></td>
        <td><span class="device-badge">${alm.state}</span></td>
        <td style="max-width: 260px; font-size: 0.78rem;">${alm.details}</td>
        <td>
          ${alm.state === 'Active' 
            ? `<button class="action-pill-btn btn-ack" data-id="${alm.id}" style="padding: 0.2rem 0.5rem; font-size: 0.7rem; background: var(--crimson-primary); color: #fff;">Acknowledge</button>` 
            : '<span style="color: #10b981; font-size: 0.75rem;">✓ Acknowledged</span>'}
        </td>
      `;
      alarmTableBody.appendChild(tr);
    });

    const activeCount = state.alarms.filter(a => a.state === 'Active').length;
    alarmCounter.textContent = activeCount;
  }

  document.addEventListener('click', (e) => {
    if (e.target.classList.contains('btn-ack')) {
      const id = e.target.dataset.id;
      const alm = state.alarms.find(a => a.id === id);
      if (alm) {
        alm.state = 'Acknowledged';
        renderAlarmTable();
      }
    }
  });

  document.getElementById('btn-ack-all')?.addEventListener('click', () => {
    state.alarms.forEach(a => a.state = 'Acknowledged');
    renderAlarmTable();
  });

  document.getElementById('btn-clear-alarms')?.addEventListener('click', () => {
    state.alarms = state.alarms.filter(a => a.state === 'Active');
    renderAlarmTable();
  });

  /* =========================================================================
     7. Multi-Track Playback Scrubber Timeline
     ========================================================================= */
  function drawTimeline() {
    if (!timelineCanvas.getContext) return;
    const ctx = timelineCanvas.getContext('2d');
    const width = timelineCanvas.width = timelineCanvas.clientWidth || 800;
    const height = timelineCanvas.height = timelineCanvas.clientHeight || 70;

    ctx.clearRect(0, 0, width, height);

    // Background hour markings
    ctx.strokeStyle = 'rgba(255, 255, 255, 0.08)';
    ctx.fillStyle = '#64748b';
    ctx.font = '9px JetBrains Mono, monospace';

    const hours = 24;
    for (let i = 0; i <= hours; i++) {
      const x = (i / hours) * width;
      ctx.beginPath();
      ctx.moveTo(x, 0);
      ctx.lineTo(x, height);
      ctx.stroke();

      const hrStr = (i < 10 ? '0' : '') + i + ':00';
      ctx.fillText(hrStr, x + 4, 14);
    }

    // Track 1: Continuous Recording (Blue bars)
    ctx.fillStyle = 'rgba(59, 130, 246, 0.45)';
    ctx.fillRect(0, 24, width * 0.7, 14);
    ctx.fillRect(width * 0.75, 24, width * 0.25, 14);

    // Track 2: AI Motion Events (Crimson red markers)
    ctx.fillStyle = '#e11d48';
    ctx.fillRect(width * 0.12, 42, 18, 12);
    ctx.fillRect(width * 0.28, 42, 35, 12);
    ctx.fillRect(width * 0.44, 42, 12, 12);
    ctx.fillRect(width * 0.62, 42, 28, 12);
    ctx.fillRect(width * 0.88, 42, 45, 12);
  }

  timelineTrack.addEventListener('click', (e) => {
    const rect = timelineTrack.getBoundingClientRect();
    const clickX = e.clientX - rect.left;
    const pct = Math.max(0, Math.min(1, clickX / rect.width));
    timelineScrubber.style.left = `${pct * 100}%`;

    const totalSeconds = pct * 86400;
    const h = Math.floor(totalSeconds / 3600);
    const m = Math.floor((totalSeconds % 3600) / 60);
    const s = Math.floor(totalSeconds % 60);

    const pad = (n) => (n < 10 ? '0' : '') + n;
    playbackTimeDisplay.textContent = `2026-08-27 ${pad(h)}:${pad(m)}:${pad(s)} UTC+05:30`;
  });

  btnPlayPause.addEventListener('click', () => {
    state.isPlaying = !state.isPlaying;
    btnPlayPause.innerHTML = state.isPlaying 
      ? '<svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><rect x="6" y="4" width="4" height="16"/><rect x="14" y="4" width="4" height="16"/></svg>'
      : '<svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><polygon points="5 3 19 12 5 21 5 3"/></svg>';
  });

  /* =========================================================================
     8. Modal Add Device Actions
     ========================================================================= */
  function openModal() { deviceModal.classList.add('active'); }
  function closeModal() { deviceModal.classList.remove('active'); }

  btnAddDevice?.addEventListener('click', openModal);
  btnAddDeviceModal?.addEventListener('click', openModal);
  btnCloseModal?.addEventListener('click', closeModal);
  btnCancelModal?.addEventListener('click', closeModal);

  btnSaveDevice?.addEventListener('click', () => {
    const name = document.getElementById('form-cam-name').value;
    const ip = document.getElementById('form-cam-ip').value;
    const codec = document.getElementById('form-cam-codec').value;
    const res = document.getElementById('form-cam-res').value;
    const ret = document.getElementById('form-cam-retention').value + ' Days';

    const newCam = {
      id: `cam-${state.cameras.length + 1}`,
      name: name || `CAM-0${state.cameras.length + 1}`,
      ip: ip,
      type: 'IP Camera',
      codec: codec,
      res: res,
      bank: 'BANK-01 (SSD)',
      retention: ret,
      recording: true,
      status: 'Online',
      fps: 60,
      bitrate: '5.5 Mbps',
      scene: 'entrance'
    };

    state.cameras.push(newCam);
    renderMatrixGrid();
    renderDeviceTable();
    closeModal();
    alert(`Success: [${newCam.name}] added and bound to local Surveillance SQL database!`);
  });

  /* =========================================================================
     9. Real-Time Telemetry & Hardware Gauges Fluctuation
     ========================================================================= */
  setInterval(() => {
    const cpu = Math.floor(16 + Math.random() * 8);
    const gpu = Math.floor(32 + Math.random() * 12);
    const bitrate = (45 + Math.random() * 8).toFixed(1);

    const statCpu = document.getElementById('stat-cpu');
    const barCpu = document.getElementById('bar-cpu');
    const statGpu = document.getElementById('stat-gpu');
    const barGpu = document.getElementById('bar-gpu');
    const statBitrate = document.getElementById('stat-bitrate');
    const barBitrate = document.getElementById('bar-bitrate');

    if (statCpu) statCpu.textContent = `${cpu}%`;
    if (barCpu) barCpu.style.width = `${cpu}%`;
    if (statGpu) statGpu.textContent = `${gpu}%`;
    if (barGpu) barGpu.style.width = `${gpu}%`;
    if (statBitrate) statBitrate.textContent = `${bitrate} MB/s`;
    if (barBitrate) barBitrate.style.width = `${Math.min(100, bitrate * 1.5)}%`;
  }, 2500);

  // Fullscreen button
  document.getElementById('btn-fullscreen')?.addEventListener('click', () => {
    if (!document.fullscreenElement) {
      document.documentElement.requestFullscreen();
    } else {
      document.exitFullscreen();
    }
  });

  // AI Overlay Toggle
  document.getElementById('btn-ai-toggle')?.addEventListener('click', () => {
    state.aiOverlayEnabled = !state.aiOverlayEnabled;
    alert(`AI Analytics bounding boxes ${state.aiOverlayEnabled ? 'Enabled' : 'Disabled'}`);
  });

  // Search Filter in Sidebar
  document.getElementById('device-search-input')?.addEventListener('input', (e) => {
    const q = e.target.value.toLowerCase();
    document.querySelectorAll('.tree-item').forEach(item => {
      const txt = item.textContent.toLowerCase();
      item.style.display = txt.includes(q) ? 'flex' : 'none';
    });
  });

  // Initialization
  renderMatrixGrid();
  renderDeviceTable();
  renderAlarmTable();
  startVideoSimulation();

})();
