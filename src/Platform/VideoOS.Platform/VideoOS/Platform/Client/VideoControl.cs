using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Shapes;
using VideoOS.Platform.Telemetry;

namespace VideoOS.Platform.Client;

public abstract class VideoControl : System.Windows.Controls.UserControl
{
	public abstract bool AutoSelectFirstCamera { get; set; }

	public abstract bool Live { get; set; }

	public abstract DateTime PlaybackTime { get; set; }

	public abstract FQID SelectedCamera { get; set; }

	public abstract List<FQID> Cameras { get; set; }

	public virtual TimelineMode TimelineMode { get; set; }

	public virtual TimeSpan TimelineSpan { get; set; }

	public Guid WindowId { get; protected set; }

	public abstract event EventHandler SelectedCameraChanged;

	public virtual event EventHandler PlaybackTimeChanged;

	protected VideoControl()
	{
		new TrackPageViewData(typeof(VideoControl)).Send();
	}

	public virtual Size GetPaintSize(int cameraIndex)
	{
		return Size.Empty;
	}

	public virtual void SetOverlay(int cameraIndex, Bitmap bitmap, int id, bool keepAspectRatio, bool scaleWithDigitalZoom, bool scaleOverlay, double scaleFactor, DockStyle verticalDock, DockStyle horizontalDock, double absolutePosX, double absolutePosY)
	{
	}

	public virtual void ClearOverlay(int cameraIndex, int id)
	{
		SetOverlay(cameraIndex, null, (short)id, keepAspectRatio: true, scaleWithDigitalZoom: true, scaleOverlay: true, 0.0, DockStyle.None, DockStyle.None, 0.0, 0.0);
	}

	public virtual Guid ShapesOverlayAdd(int cameraIndex, List<Shape> overlayShapes, ShapesOverlayRenderParameters renderParameters)
	{
		return Guid.Empty;
	}

	public virtual void ShapesOverlayRemove(int cameraIndex, Guid id)
	{
	}

	public virtual bool ShapesOverlayUpdate(int cameraIndex, Guid id, List<Shape> overlayShapes, ShapesOverlayRenderParameters renderParameters)
	{
		return false;
	}

	public virtual void SetTimelineSelection(DateTime from, DateTime to)
	{
	}

	public abstract void Close();

	protected virtual void OnPlaybackTimeChanged()
	{
		EventHandler eventHandler = this.PlaybackTimeChanged;
		if (eventHandler != null)
		{
			try
			{
				eventHandler(this, EventArgs.Empty);
			}
			catch (Exception exception)
			{
				EnvironmentManager.Instance.ExceptionHandler("VideoControl::OnPlaybackTimeChanged", exception);
			}
		}
	}
}
