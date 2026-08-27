using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace VideoOS.Platform.Client;

public abstract class ImageViewerWpfControlInternal(WindowInformation windowInformation) : UserControl, IDisposable
{
	private bool disposedValue;

	public abstract bool EnableBrowseMode { get; set; }

	public abstract bool EnableSetupMode { get; set; }

	public abstract bool EnableMouseControlledPtz { get; set; }

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

	public virtual bool EnableDigitalZoom { get; set; }

	public virtual bool MaintainImageAspectRatio { get; set; }

	public System.Windows.Size ImageSize { get; set; }

	public System.Windows.Size PaintSize { get; set; }

	public System.Windows.Point PaintLocation { get; set; }

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

	public virtual bool EnableVisibleTimestamp
	{
		get
		{
			return true;
		}
		set
		{
		}
	}

	public virtual bool SuppressUpdateOnMotionOnly
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

	public abstract string InfoText { get; set; }

	public virtual bool ShowInitialImage { get; set; } = true;

	public abstract event EventHandler<ImageOrPaintInfoChangedEventArgs> ImageOrPaintInfoChanged;

	public abstract event EventHandler<ConnectResponseEventArgs> ConnectResponseReceived;

	public abstract event EventHandler<ImageDisplayedEventArgs> ImageDisplayed;

	public abstract event EventHandler<RecordedImageReceivedEventArgs> RecordedImageReceived;

	public abstract event EventHandler<LiveStreamInformationReceivedEventArgs> LiveStreamInformationReceived;

	public virtual bool SetRenderingOptions(string xmlParameters)
	{
		return false;
	}

	public abstract void SetOverlay(Bitmap bitmap, int id, bool keepAspectRatio, bool scaleWithDigitalZoom, bool scaleOverlay, double scaleFactor, VerticalAlignment verticalAlignment, HorizontalAlignment horizontalAlignment, double absolutePositionX, double absolutePositionY);

	public abstract void ClearOverlay(int id);

	public virtual void SetVideoQuality(int smoothBufferSize, int cpusToUtilize)
	{
	}

	public virtual void SetVideoResolutionAndFps(System.Windows.Size resolution, int fps)
	{
	}

	public virtual void SetVideoResolutionAndFps(System.Windows.Size resolution, ImageViewerWpfControl.FpsRate fpsRate)
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
	}

	public abstract void Initialize();

	public abstract void Connect();

	public abstract void Disconnect();

	public abstract void Close();

	public abstract void StartLive();

	public abstract void StartBrowse();

	public abstract void StartSetup();

	public abstract void UpdateStates();

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

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
