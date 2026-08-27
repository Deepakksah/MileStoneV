using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Shapes;
using VideoOS.Platform.Telemetry;

namespace VideoOS.Platform.Client;

public sealed partial class ImageViewerWpfControl : UserControl, IDisposable, IComponentConnector
{
	public enum FpsRate
	{
		High = 1,
		Medium,
		Low
	}

	private ImageViewerWpfControlInternal _internalControl;

	private bool _disposed;

	public bool EnableBrowseMode
	{
		get
		{
			return _internalControl.EnableBrowseMode;
		}
		set
		{
			_internalControl.EnableBrowseMode = value;
		}
	}

	public bool EnableSetupMode
	{
		get
		{
			return _internalControl.EnableSetupMode;
		}
		set
		{
			_internalControl.EnableSetupMode = value;
		}
	}

	public bool AdaptiveStreaming
	{
		get
		{
			return _internalControl.AdaptiveStreaming;
		}
		set
		{
			_internalControl.AdaptiveStreaming = value;
		}
	}

	public bool EnableMouseControlledPtz
	{
		get
		{
			return _internalControl.EnableMouseControlledPtz;
		}
		set
		{
			_internalControl.EnableMouseControlledPtz = value;
		}
	}

	public bool EnableMousePtzEmbeddedHandler
	{
		get
		{
			return _internalControl.EnableMousePtzEmbeddedHandler;
		}
		set
		{
			_internalControl.EnableMousePtzEmbeddedHandler = value;
		}
	}

	public bool EnableDigitalZoom
	{
		get
		{
			return _internalControl.EnableDigitalZoom;
		}
		set
		{
			_internalControl.EnableDigitalZoom = value;
		}
	}

	public bool MaintainImageAspectRatio
	{
		get
		{
			return _internalControl.MaintainImageAspectRatio;
		}
		set
		{
			_internalControl.MaintainImageAspectRatio = value;
		}
	}

	public System.Windows.Size ImageSize
	{
		get
		{
			return _internalControl.ImageSize;
		}
		set
		{
			_internalControl.ImageSize = value;
		}
	}

	public System.Windows.Size PaintSize
	{
		get
		{
			return _internalControl.PaintSize;
		}
		set
		{
			_internalControl.PaintSize = value;
		}
	}

	public System.Windows.Point PaintLocation
	{
		get
		{
			return _internalControl.PaintLocation;
		}
		set
		{
			_internalControl.PaintLocation = value;
		}
	}

	public bool EnableVisibleHeader
	{
		get
		{
			return _internalControl.EnableVisibleHeader;
		}
		set
		{
			_internalControl.EnableVisibleHeader = value;
		}
	}

	public bool EnableVisibleCameraName
	{
		get
		{
			return _internalControl.EnableVisibleCameraName;
		}
		set
		{
			_internalControl.EnableVisibleCameraName = value;
		}
	}

	public bool EnableVisibleLiveIndicator
	{
		get
		{
			return _internalControl.EnableVisibleLiveIndicator;
		}
		set
		{
			_internalControl.EnableVisibleLiveIndicator = value;
		}
	}

	public bool EnableVisibleTimestamp
	{
		get
		{
			return _internalControl.EnableVisibleTimestamp;
		}
		set
		{
			_internalControl.EnableVisibleTimestamp = value;
		}
	}

	public bool SuppressUpdateOnMotionOnly
	{
		get
		{
			return _internalControl.SuppressUpdateOnMotionOnly;
		}
		set
		{
			_internalControl.SuppressUpdateOnMotionOnly = value;
		}
	}

	public bool Selected
	{
		get
		{
			return _internalControl.Selected;
		}
		set
		{
			_internalControl.Selected = value;
		}
	}

	public bool Hidden
	{
		get
		{
			return _internalControl.Hidden;
		}
		set
		{
			_internalControl.Hidden = value;
		}
	}

	public bool Maximized
	{
		get
		{
			return _internalControl.Maximized;
		}
		set
		{
			_internalControl.Maximized = value;
		}
	}

	public FQID CameraFQID
	{
		get
		{
			return _internalControl.CameraFQID;
		}
		set
		{
			_internalControl.CameraFQID = value;
		}
	}

	public Guid StreamId
	{
		get
		{
			return _internalControl.StreamId;
		}
		set
		{
			_internalControl.StreamId = value;
		}
	}

	public FQID PlaybackControllerFQID
	{
		get
		{
			return _internalControl.PlaybackControllerFQID;
		}
		set
		{
			_internalControl.PlaybackControllerFQID = value;
		}
	}

	public bool UsingMulticast
	{
		get
		{
			return _internalControl.UsingMulticast;
		}
		set
		{
			_internalControl.UsingMulticast = value;
		}
	}

	public bool ShowImageViewer
	{
		get
		{
			return _internalControl.ShowImageViewer;
		}
		set
		{
			_internalControl.ShowImageViewer = value;
		}
	}

	public string InfoText
	{
		get
		{
			return _internalControl.InfoText;
		}
		set
		{
			_internalControl.InfoText = value;
		}
	}

	public bool ShowInitialImage
	{
		get
		{
			return _internalControl.ShowInitialImage;
		}
		set
		{
			_internalControl.ShowInitialImage = value;
		}
	}

	public event EventHandler<ImageOrPaintInfoChangedEventArgs> ImageOrPaintInfoChanged = delegate
	{
	};

	public event EventHandler<ConnectResponseEventArgs> ConnectResponseReceived = delegate
	{
	};

	public event EventHandler<ImageDisplayedEventArgs> ImageDisplayed = delegate
	{
	};

	public event EventHandler<RecordedImageReceivedEventArgs> RecordedImageReceived = delegate
	{
	};

	public event EventHandler<LiveStreamInformationReceivedEventArgs> LiveStreamInformationReceived = delegate
	{
	};

	public ImageViewerWpfControl()
		: this(null)
	{
	}

	public ImageViewerWpfControl(WindowInformation windowInformation)
	{
		_internalControl = ClientControl.Instance.CreateImageViewerWpfControlInternal(windowInformation);
		if (_internalControl != null)
		{
			_internalControl.HorizontalAlignment = HorizontalAlignment.Stretch;
			_internalControl.VerticalAlignment = VerticalAlignment.Stretch;
			base.Content = _internalControl;
			_internalControl.ConnectResponseReceived += _internalControl_ConnectResponseReceived;
			_internalControl.ImageOrPaintInfoChanged += _internalControl_ImageOrPaintInfoChanged;
			_internalControl.ImageDisplayed += _internalControl_ImageDisplayed;
			_internalControl.RecordedImageReceived += _internalControl_RecordedImageReceived;
			_internalControl.LiveStreamInformationReceived += _internalControl_LiveStreamInformationReceived;
		}
	}

	private void _internalControl_LiveStreamInformationReceived(object sender, LiveStreamInformationReceivedEventArgs e)
	{
		this.LiveStreamInformationReceived(this, e);
	}

	private void _internalControl_RecordedImageReceived(object sender, RecordedImageReceivedEventArgs e)
	{
		this.RecordedImageReceived(this, e);
	}

	private void _internalControl_ImageDisplayed(object sender, ImageDisplayedEventArgs e)
	{
		this.ImageDisplayed(this, e);
	}

	private void _internalControl_ImageOrPaintInfoChanged(object sender, ImageOrPaintInfoChangedEventArgs e)
	{
		this.ImageOrPaintInfoChanged(this, e);
	}

	private void _internalControl_ConnectResponseReceived(object sender, ConnectResponseEventArgs e)
	{
		this.ConnectResponseReceived(this, e);
	}

	public bool SetRenderingOptions(string xmlParameters)
	{
		return _internalControl.SetRenderingOptions(xmlParameters);
	}

	public void SetOverlay(Bitmap bitmap, int id, bool keepAspectRatio, bool scaleWithDigitalZoom, bool scaleOverlay, double scaleFactor, VerticalAlignment verticalAlignment, HorizontalAlignment horizontalAlignment, double absolutePositionX, double absolutePositionY)
	{
		_internalControl.SetOverlay(bitmap, id, keepAspectRatio, scaleWithDigitalZoom, scaleOverlay, scaleFactor, verticalAlignment, horizontalAlignment, absolutePositionX, absolutePositionY);
	}

	public void ClearOverlay(int id)
	{
		_internalControl.ClearOverlay(id);
	}

	public void SetVideoQuality(int smoothBufferSize, int cpusToUtilize)
	{
		_internalControl.SetVideoQuality(smoothBufferSize, cpusToUtilize);
	}

	[Obsolete("This method is no longer supported", false)]
	public void SetVideoResolutionAndFps(System.Windows.Size resolution, int fps)
	{
		_internalControl.SetVideoResolutionAndFps(resolution, fps);
	}

	public void SetVideoResolutionAndFps(System.Windows.Size resolution, FpsRate fpsRate)
	{
		_internalControl.SetVideoResolutionAndFps(resolution, fpsRate);
	}

	public bool ConfigureOverlayTextAndImage(OverlayTextId textId, bool enable, bool blank)
	{
		return _internalControl.ConfigureOverlayTextAndImage(textId, enable, blank);
	}

	public bool ConfigureOverlayTextAndImage(bool enable, bool blank)
	{
		return _internalControl.ConfigureOverlayTextAndImage(enable, blank);
	}

	public bool ConfigureOverlayText(OverlayTextId textId, bool enable)
	{
		return _internalControl.ConfigureOverlayText(textId, enable);
	}

	public bool ConfigureOverlayText(bool enable)
	{
		return _internalControl.ConfigureOverlayText(enable);
	}

	public void SetImageQuality(int percent)
	{
		_internalControl.SetImageQuality(percent);
	}

	public void Initialize()
	{
		_internalControl.Initialize();
		new TrackPageViewData(typeof(ImageViewerWpfControl)).Send();
	}

	public void Connect()
	{
		_internalControl.Connect();
	}

	public void Disconnect()
	{
		_internalControl.Disconnect();
	}

	public void Close()
	{
		_internalControl.Close();
	}

	public void StartLive()
	{
		_internalControl.StartLive();
	}

	public void StartBrowse()
	{
		_internalControl.StartBrowse();
	}

	public void StartSetup()
	{
		_internalControl.StartSetup();
	}

	public void UpdateStates()
	{
		_internalControl.UpdateStates();
	}

	public Bitmap GetCurrentDisplayedImageAsBitmap()
	{
		return _internalControl.GetCurrentDisplayedImageAsBitmap();
	}

	public Bitmap GetCurrentDisplayedImageAsBitmap(bool digitalZoomOnly)
	{
		return _internalControl.GetCurrentDisplayedImageAsBitmap(digitalZoomOnly);
	}

	public void PtzCenter(int refWidth, int refHeight, int centerX, int centerY, int zoom)
	{
		_internalControl.PtzCenter(refWidth, refHeight, centerX, centerY, zoom);
	}

	public Guid ShapesOverlayAdd(List<Shape> overlayShapes, ShapesOverlayRenderParameters renderParameters)
	{
		return _internalControl.ShapesOverlayAdd(overlayShapes, renderParameters);
	}

	public bool ShapesOverlayUpdate(Guid id, List<Shape> overlayShapes)
	{
		return _internalControl.ShapesOverlayUpdate(id, overlayShapes);
	}

	public bool ShapesOverlayUpdate(Guid id, ShapesOverlayRenderParameters renderParameters)
	{
		return _internalControl.ShapesOverlayUpdate(id, renderParameters);
	}

	public bool ShapesOverlayUpdate(Guid id, List<Shape> overlayShapes, ShapesOverlayRenderParameters renderParameters)
	{
		return _internalControl.ShapesOverlayUpdate(id, overlayShapes, renderParameters);
	}

	public void ShapesOverlayRemove(Guid id)
	{
		_internalControl.ShapesOverlayRemove(id);
	}

	private void Dispose(bool disposing)
	{
		if (!_disposed)
		{
			if (disposing && _internalControl != null)
			{
				_internalControl.ConnectResponseReceived -= _internalControl_ConnectResponseReceived;
				_internalControl.ImageOrPaintInfoChanged -= _internalControl_ImageOrPaintInfoChanged;
				_internalControl.ImageDisplayed -= _internalControl_ImageDisplayed;
				_internalControl.RecordedImageReceived -= _internalControl_RecordedImageReceived;
				_internalControl.LiveStreamInformationReceived -= _internalControl_LiveStreamInformationReceived;
				_internalControl.Dispose();
				_internalControl = null;
			}
			_disposed = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}
}
