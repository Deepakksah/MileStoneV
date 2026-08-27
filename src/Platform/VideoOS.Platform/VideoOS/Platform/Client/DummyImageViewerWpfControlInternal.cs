using System;
using System.Drawing;
using System.Windows;
using System.Windows.Markup;

namespace VideoOS.Platform.Client;

public partial class DummyImageViewerWpfControlInternal : ImageViewerWpfControlInternal, IComponentConnector
{
	public override bool EnableBrowseMode { get; set; }

	public override bool EnableSetupMode { get; set; }

	public override bool EnableMouseControlledPtz { get; set; }

	public override bool Selected { get; set; }

	public override bool Hidden { get; set; }

	public override bool Maximized { get; set; }

	public override FQID CameraFQID { get; set; }

	public override bool ShowImageViewer { get; set; }

	public override string InfoText { get; set; }

	public override event EventHandler<ImageOrPaintInfoChangedEventArgs> ImageOrPaintInfoChanged = delegate
	{
	};

	public override event EventHandler<ConnectResponseEventArgs> ConnectResponseReceived = delegate
	{
	};

	public override event EventHandler<ImageDisplayedEventArgs> ImageDisplayed = delegate
	{
	};

	public override event EventHandler<RecordedImageReceivedEventArgs> RecordedImageReceived = delegate
	{
	};

	public override event EventHandler<LiveStreamInformationReceivedEventArgs> LiveStreamInformationReceived = delegate
	{
	};

	public DummyImageViewerWpfControlInternal(WindowInformation windowInformation)
		: base(windowInformation)
	{
		InitializeComponent();
	}

	public override void ClearOverlay(int id)
	{
	}

	public override void Close()
	{
	}

	public override void Connect()
	{
	}

	public override void Disconnect()
	{
	}

	public override void Initialize()
	{
	}

	public override void SetOverlay(Bitmap bitmap, int id, bool keepAspectRatio, bool scaleWithDigitalZoom, bool scaleOverlay, double scaleFactor, VerticalAlignment verticalAlignment, HorizontalAlignment horizontalAlignment, double absolutePositionX, double absolutePositionY)
	{
	}

	public override void StartBrowse()
	{
	}

	public override void StartLive()
	{
	}

	public override void StartSetup()
	{
	}

	public override void UpdateStates()
	{
	}
}
