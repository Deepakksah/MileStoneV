using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VideoOS.Platform.Messaging;

namespace VideoOS.Platform.Client;

public abstract class ImageViewerAddOn
{
	public abstract FQID CameraFQID { get; set; }

	public virtual Guid StreamId { get; set; }

	[Obsolete("This property is deprecated. Windows Forms controls are no longer supported. Use LocationWpf instead.", false)]
	public virtual System.Drawing.Point Location { get; }

	public virtual System.Windows.Point LocationWpf { get; }

	[Obsolete("This property is deprecated. Windows Forms controls are no longer supported. Use SizeWpf instead.", false)]
	public virtual System.Drawing.Size Size { get; }

	public virtual System.Windows.Size SizeWpf { get; }

	public abstract string CameraName { get; }

	public abstract bool InLiveMode { get; }

	public abstract DateTime PlaybackImageDateTime { get; }

	public abstract ImageViewerType ImageViewerType { get; }

	public virtual bool IndependentPlaybackEnabled { get; set; }

	public virtual PlaybackController IndependentPlaybackController { get; }

	public virtual bool IsSelected { get; set; }

	public virtual bool IsGlobalSelected { get; }

	public virtual bool ShowMetadataOverlay
	{
		get
		{
			return true;
		}
		set
		{
		}
	}

	public virtual Effect VideoEffect { get; set; }

	public virtual Rect VideoEffectArea { get; }

	public virtual int ViewItemIndex => -1;

	[Obsolete("This property is deprecated. Windows Forms controls are no longer supported. Use ImageSizeWpf instead.", false)]
	public System.Drawing.Size ImageSize { get; set; }

	public System.Windows.Size ImageSizeWpf { get; set; }

	[Obsolete("This property is deprecated. Windows Forms controls are no longer supported. Use PaintSizeWpf instead.", false)]
	public System.Drawing.Size PaintSize { get; set; }

	public System.Windows.Size PaintSizeWpf { get; set; }

	[Obsolete("This property is deprecated. Windows Forms controls are no longer supported. Use PaintLocationWpf instead.", false)]
	public System.Drawing.Point PaintLocation { get; set; }

	public System.Windows.Point PaintLocationWpf { get; set; }

	public virtual WindowInformation WindowInformation { get; set; }

	public virtual PTZRectangleCommandData DigitalZoomRectangle { get; set; }

	public virtual bool DigitalZoomEnabled { get; set; }

	public abstract event EventHandler UserControlSizeOrLocationChangedEvent;

	public virtual event EventHandler<IndependentPlaybackModeEventArgs> IndependentPlaybackModeChangedEvent;

	public abstract event LiveStreamInformationHandler LiveStreamInformationEvent;

	public abstract event PassRequestEventHandler StartLiveEvent;

	public abstract event PassRequestEventHandler StopLiveEvent;

	public abstract event EventHandler PropertyChangedEvent;

	public abstract event EventHandler CloseEvent;

	public event ImageDisplayedHandler ImageDisplayedEvent;

	public abstract event RecordedImageReceivedHandler RecordedImageReceivedEvent;

	public event MouseEventHandler MouseClickEvent;

	public event MouseEventHandler MouseDoubleClickEvent;

	public event MouseEventHandler MouseRightClickEvent;

	public event MouseEventHandler MouseMoveEvent;

	public event EventHandler MouseEnterEvent;

	public event EventHandler MouseLeaveEvent;

	protected void OnIndependentPlaybackModeChangedEvent(bool independentPlaybackEnabled)
	{
		this.IndependentPlaybackModeChangedEvent?.Invoke(this, new IndependentPlaybackModeEventArgs(independentPlaybackEnabled));
	}

	public virtual void ClearOverlay(int id)
	{
		SetOverlay(null, (short)id, keepAspectRatio: true, scaleWithDigitalZoom: true, scaleOverlay: true, 0.0, DockStyle.None, DockStyle.None, 0.0, 0.0);
	}

	public abstract void SetOverlay(Bitmap bitmap, int id, bool keepAspectRatio, bool scaleWithDigitalZoom, bool scaleOverlay, double scaleFactor, DockStyle verticalDock, DockStyle horizontalDock, double absolutePosX, double absolutePosY);

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

	public virtual Guid ActiveElementsOverlayAdd(IEnumerable<FrameworkElement> activeElements, ActiveElementsOverlayRenderParameters renderParameters)
	{
		return Guid.Empty;
	}

	public virtual bool ActiveElementsOverlayUpdate(Guid id, IEnumerable<FrameworkElement> activeElements, ActiveElementsOverlayRenderParameters renderParameters)
	{
		return false;
	}

	public virtual bool ActiveElementsOverlayUpdate(Guid id, IEnumerable<FrameworkElement> activeElements)
	{
		return false;
	}

	public virtual bool ActiveElementsOverlayUpdate(Guid id, ActiveElementsOverlayRenderParameters renderParameters)
	{
		return false;
	}

	public virtual void ActiveElementsOverlayRemove(Guid id)
	{
	}

	[Obsolete("This method is deprecated. Windows Forms controls are no longer supported. Use GetCurrentDisplayedImageAsImageSource() instead.", false)]
	public virtual Bitmap GetCurrentDisplayedImageAsBitmap()
	{
		return null;
	}

	[Obsolete("This method is deprecated. Windows Forms controls are no longer supported. Use GetCurrentDisplayedImageAsImageSource(bool) instead.", false)]
	public virtual Bitmap GetCurrentDisplayedImageAsBitmap(bool digitalZoomOnly)
	{
		return null;
	}

	public virtual System.Windows.Media.Imaging.BitmapSource GetCurrentDisplayedImageAsImageSource()
	{
		return null;
	}

	public virtual System.Windows.Media.Imaging.BitmapSource GetCurrentDisplayedImageAsImageSource(bool digitalZoomOnly)
	{
		return null;
	}

	public void FireImageDisplayed(ImageDisplayedEventArgs e)
	{
		if (this.ImageDisplayedEvent != null)
		{
			this.ImageDisplayedEvent(this, e);
		}
	}

	[Obsolete("This method is deprecated. Windows Forms controls are no longer supported. Use ConvertUserControlToImagePointWpf instead.", false)]
	public System.Drawing.Point ConvertUserControlToImagePoint(System.Drawing.Point position)
	{
		int x = position.X - PaintLocation.X;
		int y = position.Y - PaintLocation.Y;
		return new System.Drawing.Point(x, y);
	}

	public System.Windows.Point ConvertUserControlToImagePointWpf(System.Windows.Point position)
	{
		double x = position.X - PaintLocationWpf.X;
		double y = position.Y - PaintLocationWpf.Y;
		return new System.Windows.Point(x, y);
	}

	[Obsolete("This method is deprecated. Windows Forms controls are no longer supported. Use ConvertUserControlToRelativeImagePositionWpf instead.", false)]
	public Tuple<double, double> ConvertUserControlToRelativeImagePosition(System.Drawing.Point position)
	{
		int value = position.X - PaintLocation.X;
		int value2 = position.Y - PaintLocation.Y;
		double item = Convert.ToDouble(value) / (double)PaintSize.Width;
		double item2 = Convert.ToDouble(value2) / (double)PaintSize.Height;
		return new Tuple<double, double>(item, item2);
	}

	public Tuple<double, double> ConvertUserControlToRelativeImagePositionWpf(System.Windows.Point position)
	{
		double num = position.X - PaintLocationWpf.X;
		double num2 = position.Y - PaintLocationWpf.Y;
		double item = num / PaintSizeWpf.Width;
		double item2 = num2 / PaintSizeWpf.Height;
		return new Tuple<double, double>(item, item2);
	}

	public virtual void RegisterTimelineSequenceSource(TimelineSequenceSource timelineSequenceSource)
	{
	}

	public virtual void UnregisterTimelineSequenceSource(TimelineSequenceSource timelineSequenceSource)
	{
	}

	protected virtual void FireClickEvent(MouseEventArgs e)
	{
		if (this.MouseClickEvent != null)
		{
			this.MouseClickEvent(this, e);
		}
	}

	protected virtual void FireDoubleClickEvent(MouseEventArgs e)
	{
		if (this.MouseDoubleClickEvent != null)
		{
			this.MouseDoubleClickEvent(this, e);
		}
	}

	protected virtual void FireRightClickEvent(MouseEventArgs e)
	{
		if (this.MouseRightClickEvent != null)
		{
			this.MouseRightClickEvent(this, e);
		}
	}

	protected virtual void FireMouseMoveEvent(MouseEventArgs e)
	{
		if (this.MouseMoveEvent != null)
		{
			this.MouseMoveEvent(this, e);
		}
	}

	protected virtual void FireMouseEnterEvent(EventArgs e)
	{
		if (this.MouseEnterEvent != null)
		{
			this.MouseEnterEvent(this, e);
		}
	}

	protected virtual void FireMouseLeaveEvent(EventArgs e)
	{
		if (this.MouseLeaveEvent != null)
		{
			this.MouseLeaveEvent(this, e);
		}
	}

	public virtual short DigitalZoomMove(string command)
	{
		throw new NotImplementedException();
	}
}
