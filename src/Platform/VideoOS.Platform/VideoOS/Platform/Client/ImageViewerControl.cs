using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Shapes;
using VideoOS.Platform.Telemetry;

namespace VideoOS.Platform.Client;

[Obsolete("This class is deprecated. Windows Forms controls are no longer supported. Use ImageViewerWpfControl instead.", false)]
public abstract class ImageViewerControl : UserControl
{
	public enum FPSRate
	{
		High = 1,
		Medium,
		Low
	}

	public EventHandler ImageOrPaintInfoChanged;

	private bool InQualityMethod;

	private IContainer components;

	public abstract bool EnableBrowseMode { set; }

	public abstract bool EnableSetupMode { set; }

	public abstract bool SuppressUpdateOnMotionOnly { set; }

	public abstract bool EnableRecordedImageDisplayedEvent { set; }

	public virtual bool EnableLiveStreamInformation
	{
		set
		{
		}
	}

	public virtual bool EnableNextImageReceivedEvent
	{
		set
		{
		}
	}

	public abstract bool EnableSmartSearch { set; }

	public abstract bool EnableMouseControlledPtz { set; }

	public virtual bool EnableMousePtzEmbeddedHandler
	{
		get
		{
			return true;
		}
		set
		{
		}
	}

	public abstract bool EnableScrollWheel { set; }

	public abstract bool EnableMiddleButtonClick { set; }

	public virtual bool EnableDigitalZoom { get; set; }

	public virtual bool MaintainImageAspectRatio { get; set; }

	public Size ImageSize { get; set; }

	public Size PaintSize { get; set; }

	public Point PaintLocation { get; set; }

	public virtual bool EnableVisibleHeader
	{
		get
		{
			return true;
		}
		set
		{
		}
	}

	public virtual bool EnableVisibleCameraName
	{
		get
		{
			return true;
		}
		set
		{
		}
	}

	public virtual bool EnableVisibleLiveIndicator
	{
		get
		{
			return true;
		}
		set
		{
		}
	}

	public virtual bool EnableVisibleTimeStamp
	{
		get
		{
			return true;
		}
		set
		{
		}
	}

	public virtual bool AdaptiveStreaming
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public abstract bool Selected { get; set; }

	public abstract bool Hidden { get; set; }

	public abstract bool Maximized { get; set; }

	public abstract FQID CameraFQID { get; set; }

	public virtual Guid StreamId { get; set; }

	public virtual FQID PlaybackControllerFQID
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public virtual bool UsingMulticast
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public abstract bool ShowImageViewer { get; set; }

	public abstract string InfoText { set; }

	public virtual bool ShowInitialImage { get; set; } = true;

	public abstract event EventHandler ClickEvent;

	public abstract event EventHandler DoubleClickEvent;

	public abstract event EventHandler RightClickEvent;

	public virtual event MouseEventHandler MiddleClickEvent;

	public virtual event KeyEventHandler KeyUpEvent;

	public virtual event EventHandler MouseLeaveEvent;

	public virtual event EventHandler MouseHoverEvent;

	public virtual event MouseEventHandler MouseMoveEvent;

	public virtual event MouseEventHandler MouseScrollWheelEvent;

	public abstract event ConnectResponseEventHandler ConnectResponseEvent;

	public abstract event EventHandler ImageReceivedEvent;

	public event ImageDisplayedHandler ImageDisplayedEvent;

	public virtual event RecordedImageReceivedHandler RecordedImageReceivedEvent;

	public virtual event LiveStreamInformationHandler LiveStreamInformationEvent;

	public ImageViewerControl()
	{
		InitializeComponent();
		new TrackPageViewData(typeof(ImageViewerControl), deprecated: true).Send();
	}

	public void FireImageOrMaintInfoChanged()
	{
		if (ImageOrPaintInfoChanged != null)
		{
			ImageOrPaintInfoChanged(this, new EventArgs());
		}
	}

	public virtual short SetPlaybackFadeImageTime(int msBeforeFading)
	{
		return 0;
	}

	public virtual bool SetRenderingOptions(string xmlParameters)
	{
		return false;
	}

	public abstract void SetOverlay(Bitmap bitmap, int id, bool keepAspectRatio, bool scaleWithDigitalZoom, bool scaleOverlay, double scaleFactor, DockStyle verticalDock, DockStyle horizontalDock, double absolutePosX, double absolutePosY);

	public abstract void ClearOverlay(int id);

	public virtual void SetVideoQuality(int smoothBufferSize, int CPUsToUtilize)
	{
	}

	[Obsolete("This method is no longer supported", false)]
	public virtual void SetVideoResolutionAndFPS(Size resolution, int FPS)
	{
	}

	public virtual void SetVideoResolutionAndFPS(Size resolution, FPSRate fpsRate)
	{
	}

	public virtual bool ConfigureOverlayTextAndImage(OverlayTextId textId, bool enable, bool blank)
	{
		return false;
	}

	public virtual bool ConfigureOverlayTextAndImage(bool enable, bool blank)
	{
		return false;
	}

	public virtual bool ConfigureOverlayText(OverlayTextId textId, bool enable)
	{
		return false;
	}

	public virtual bool ConfigureOverlayText(bool enable)
	{
		return false;
	}

	public virtual void SetImageQuality(int percent)
	{
		if (!InQualityMethod)
		{
			InQualityMethod = true;
			SetVideoQuality(percent);
			InQualityMethod = false;
		}
	}

	public virtual void SetVideoQuality(int percent)
	{
		if (!InQualityMethod)
		{
			InQualityMethod = true;
			SetImageQuality(percent);
			InQualityMethod = false;
		}
	}

	public abstract void Initialize();

	public abstract void Connect();

	public abstract void Disconnect();

	public abstract void Close();

	public abstract void StartLive();

	public abstract void StartBrowse();

	public abstract void StartSetup();

	public abstract void UpdateStates();

	public void FireImageDisplayed(ImageDisplayedEventArgs e)
	{
		if (this.ImageDisplayedEvent != null)
		{
			this.ImageDisplayedEvent(this, e);
		}
	}

	public void FireRecordedImageReceivedEvent(RecordedImageReceivedEventArgs e)
	{
		RecordedImageReceivedHandler recordedImageReceivedHandler = this.RecordedImageReceivedEvent;
		if (recordedImageReceivedHandler != null)
		{
			try
			{
				recordedImageReceivedHandler(this, e);
			}
			catch (Exception exception)
			{
				EnvironmentManager.Instance.ExceptionHandler("ImageViewerControl::RecordedImageDisplayed", exception);
			}
		}
	}

	public void FireLiveStreamInformationEvent(LiveStreamInformationEventArgs e)
	{
		LiveStreamInformationHandler liveStreamInformationHandler = this.LiveStreamInformationEvent;
		if (liveStreamInformationHandler != null)
		{
			try
			{
				liveStreamInformationHandler(this, e);
			}
			catch (Exception exception)
			{
				EnvironmentManager.Instance.ExceptionHandler("ImageViewerControl::LiveStreamInformationEvent", exception);
			}
		}
	}

	protected virtual void OnKeyUpEvent(KeyEventArgs e)
	{
		KeyEventHandler keyEventHandler = this.KeyUpEvent;
		if (keyEventHandler != null)
		{
			try
			{
				keyEventHandler(this, e);
			}
			catch (Exception exception)
			{
				EnvironmentManager.Instance.ExceptionHandler("ImageViewerControl::KeyUpEvent", exception);
			}
		}
	}

	protected virtual void OnMiddleClickEvent(MouseEventArgs e)
	{
		MouseEventHandler mouseEventHandler = this.MiddleClickEvent;
		if (mouseEventHandler != null)
		{
			try
			{
				mouseEventHandler(this, e);
			}
			catch (Exception exception)
			{
				EnvironmentManager.Instance.ExceptionHandler("ImageViewerControl::MiddleClickEvent", exception);
			}
		}
	}

	protected virtual void OnMouseMoveEvent(MouseEventArgs e)
	{
		MouseEventHandler mouseEventHandler = this.MouseMoveEvent;
		if (mouseEventHandler != null)
		{
			try
			{
				mouseEventHandler(this, e);
			}
			catch (Exception exception)
			{
				EnvironmentManager.Instance.ExceptionHandler("ImageViewerControl::MouseMoveEvent", exception);
			}
		}
	}

	protected virtual void OnMouseHoverEvent()
	{
		EventHandler eventHandler = this.MouseHoverEvent;
		if (eventHandler != null)
		{
			try
			{
				eventHandler(this, EventArgs.Empty);
			}
			catch (Exception exception)
			{
				EnvironmentManager.Instance.ExceptionHandler("ImageViewerControl::MouseHoverEvent", exception);
			}
		}
	}

	protected virtual void OnMouseLeaveEvent()
	{
		EventHandler eventHandler = this.MouseLeaveEvent;
		if (eventHandler != null)
		{
			try
			{
				eventHandler(this, EventArgs.Empty);
			}
			catch (Exception exception)
			{
				EnvironmentManager.Instance.ExceptionHandler("ImageViewerControl::MouseLeaveEvent", exception);
			}
		}
	}

	protected virtual void OnMouseScrollWheel(MouseEventArgs e)
	{
		MouseEventHandler mouseEventHandler = this.MouseScrollWheelEvent;
		if (mouseEventHandler != null)
		{
			try
			{
				mouseEventHandler(this, e);
			}
			catch (Exception exception)
			{
				EnvironmentManager.Instance.ExceptionHandler("ImageViewerControl::MouseScrollWheel", exception);
			}
		}
	}

	public virtual Bitmap GetCurrentDisplayedImageAsBitmap()
	{
		return null;
	}

	public virtual Bitmap GetCurrentDisplayedImageAsBitmap(bool digitalZoomOnly)
	{
		return null;
	}

	public virtual void PtzCenter(int refWidth, int refHeight, int centerX, int centerY, int zoom)
	{
	}

	public virtual Guid ShapesOverlayAdd(List<Shape> overlayShapes, ShapesOverlayRenderParameters renderParameters)
	{
		return Guid.Empty;
	}

	public virtual bool ShapesOverlayUpdate(Guid id, List<Shape> overlayShapes)
	{
		return false;
	}

	public virtual bool ShapesOverlayUpdate(Guid id, ShapesOverlayRenderParameters renderParameters)
	{
		return false;
	}

	public virtual bool ShapesOverlayUpdate(Guid id, List<Shape> overlayShapes, ShapesOverlayRenderParameters renderParameters)
	{
		return false;
	}

	public virtual void ShapesOverlayRemove(Guid id)
	{
	}

	[Obsolete("This method is no longer supported")]
	public virtual void SetCameraXml(UserContext userContext, string xml)
	{
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
	}
}
