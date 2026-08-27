using System;
using System.Collections.Generic;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Live;

public abstract class VideoLiveSource : BaseLiveSource<LiveSourceContent, LiveContentEventArgs>
{
	private int _width = 640;

	private int _height = 480;

	private volatile bool _setWidthHeight;

	private int _fps;

	private volatile bool _setFps;

	private int _compression = 75;

	private bool _changeCompression;

	private double _cropX;

	private double _cropY;

	private double _cropWidth = 1.0;

	private double _cropHeight = 1.0;

	private volatile bool _setCrop;

	private double _cropXSet;

	private double _cropYSet;

	private double _cropWidthSet = 1.0;

	private double _cropHeightSet = 1.0;

	private bool _cropDefined;

	private object _customData;

	private bool _setCustomData;

	private object _customDataSeen;

	private bool _keepAspectRatio;

	private bool _fillWithBlack = true;

	private volatile bool _suspendSettingsUpdate;

	private bool _connectedWithDefaultWidthAndHeight;

	private bool _connectedWithDefaultFps;

	private bool _allowUpscaling = true;

	private volatile bool _resolutionJustChanged;

	public int Height
	{
		get
		{
			return _height;
		}
		set
		{
			if (value > 32767)
			{
				EnvironmentManager.Instance.Log(error: false, "VideoLiveSource.Height", "Height is being attempted set to higher value than the supported 32767. Will be limited to this.");
				_height = 32767;
			}
			else
			{
				_height = value;
			}
		}
	}

	public int Width
	{
		get
		{
			return _width;
		}
		set
		{
			if (value > 32767)
			{
				EnvironmentManager.Instance.Log(error: false, "VideoLiveSource.Width", "Width is being attempted set to higher value than the supported 32767. Will be limited to this.");
				_width = 32767;
			}
			else
			{
				_width = value;
			}
		}
	}

	public int FPS
	{
		get
		{
			return _fps;
		}
		set
		{
			_fps = value;
			_setFps = true;
		}
	}

	public bool KeyFramesOnly { get; set; }

	public bool KeepAspectRatio => _keepAspectRatio;

	public bool FillWithBlack => _fillWithBlack;

	public bool EnableMulticast
	{
		get
		{
			return base.MulticastEnabled;
		}
		set
		{
			base.MulticastEnabled = value;
		}
	}

	public int Compression
	{
		get
		{
			return _compression;
		}
		set
		{
			_compression = value;
			_changeCompression = true;
		}
	}

	public object CustomData
	{
		get
		{
			return _customData;
		}
		set
		{
			_customData = value;
			_setCustomData = true;
		}
	}

	public bool AllowUpscaling
	{
		get
		{
			return _allowUpscaling;
		}
		set
		{
			_allowUpscaling = value;
			_setWidthHeight = true;
		}
	}

	protected VideoLiveSource(Item item)
		: base(item, (IEnumerable<Guid>)new Guid[1] { Kind.Camera }, false)
	{
		KeyFramesOnly = false;
	}

	public override void Init()
	{
		base.Init();
		if (Item.FQID.ServerId.UserContext != null)
		{
			UserContext userContext = Item.FQID.ServerId.UserContext;
			userContext.PrivacyMaskLiftChanged = (EventHandler)Delegate.Combine(userContext.PrivacyMaskLiftChanged, new EventHandler(RemovableMasksLiftedChangedHandler));
		}
	}

	public override void Close()
	{
		base.Close();
		if (Item.FQID.ServerId.UserContext != null)
		{
			UserContext userContext = Item.FQID.ServerId.UserContext;
			userContext.PrivacyMaskLiftChanged = (EventHandler)Delegate.Remove(userContext.PrivacyMaskLiftChanged, new EventHandler(RemovableMasksLiftedChangedHandler));
		}
	}

	public void SuspendSettingsUpdate()
	{
		_suspendSettingsUpdate = true;
	}

	public void ResumeSettingsUpdate()
	{
		_suspendSettingsUpdate = false;
	}

	public void SetWidthHeight()
	{
		_setWidthHeight = true;
	}

	public void SetKeepAspectRatio(bool keepAspectRatio, bool fillSpace)
	{
		_keepAspectRatio = keepAspectRatio;
		_fillWithBlack = fillSpace;
	}

	public void SetCropRectangle(double x, double y, double width, double height)
	{
		_cropX = x;
		_cropY = y;
		_cropWidth = width;
		_cropHeight = height;
		_setCrop = true;
	}

	protected override void OnUpdateLiveStart()
	{
		if (_width != 0 && _height != 0)
		{
			InternalMediaService.Instance.SetWidthHeight(base.SessionId, _width, _height, _allowUpscaling);
		}
	}

	protected override void HandleLiveDataContent(LiveSourceContent data)
	{
		if (data != null)
		{
			LiveContentEventArgs e = new LiveContentEventArgs
			{
				LiveContent = data
			};
			e.LiveContent.CropX = _cropXSet;
			e.LiveContent.CropY = _cropYSet;
			e.LiveContent.CropWidth = _cropWidthSet;
			e.LiveContent.CropHeight = _cropHeightSet;
			e.LiveContent.CroppingDefined = _cropDefined;
			e.LiveContent.CustomData = _customDataSeen;
			e.LiveContent.Source = this;
			bool num = IgnoreFrame(e, _resolutionJustChanged);
			bool flag = false;
			if (!num)
			{
				flag = OnLiveContentEvent(e);
			}
			if (!flag)
			{
				e.LiveContent.Dispose();
			}
			_resolutionJustChanged = false;
		}
	}

	protected override void OnUpdateSettings()
	{
		if (!_suspendSettingsUpdate)
		{
			if (_changeCompression)
			{
				_changeCompression = false;
				InternalMediaService.Instance.SetQuality(base.SessionId, _compression);
			}
			bool flag = false;
			flag = (_resolutionJustChanged = ChangeWidthHeightIfNeeded());
			ChangeFpsIfNeeded();
			if (flag | ChangeCropIfNeeded())
			{
				SetDecoderWidthHeight();
			}
			if (_setCustomData)
			{
				_setCustomData = false;
				_customDataSeen = _customData;
			}
		}
	}

	public override EventArgs CreateExceptionEventArgs(Exception ex)
	{
		return new LiveContentEventArgs
		{
			Exception = ex
		};
	}

	private bool ChangeWidthHeightIfNeeded()
	{
		if (!_setWidthHeight)
		{
			return false;
		}
		_setWidthHeight = false;
		if (ToolkitMustBeReconfigured())
		{
			ReconfigureToolkit();
			_setCrop = _cropDefined;
			_connectedWithDefaultWidthAndHeight = _width == 0 && _height == 0;
		}
		else
		{
			InternalMediaService.Instance.SetWidthHeight(base.SessionId, _width, _height, _allowUpscaling);
		}
		return true;
	}

	private void ChangeFpsIfNeeded()
	{
		if (_setFps)
		{
			_setFps = false;
			if (ToolkitMustBeReconfigured())
			{
				ReconfigureToolkit();
				_connectedWithDefaultFps = _fps == 0;
			}
			else
			{
				InternalMediaService.Instance.SetFps(base.SessionId, _fps);
			}
		}
	}

	private bool ChangeCropIfNeeded()
	{
		if (!_setCrop)
		{
			return false;
		}
		_setCrop = false;
		InternalMediaService.Instance.SetCropRectangle(base.SessionId, _cropX, _cropY, _cropWidth, _cropHeight);
		_cropXSet = _cropX;
		_cropYSet = _cropY;
		_cropWidthSet = _cropWidth;
		_cropHeightSet = _cropHeight;
		_cropDefined = _cropHeightSet != 1.0 || _cropWidthSet != 1.0;
		return true;
	}

	private void SetDecoderWidthHeight()
	{
		int width = _width;
		int height = _height;
		if (_cropDefined)
		{
			width = (height = 0);
		}
		InternalMediaService.Instance.SetDecoderWidthHeight(base.SessionId, width, height);
	}

	private bool ToolkitMustBeReconfigured()
	{
		bool flag = _fps == 0 && _width == 0 && _height == 0;
		return (_connectedWithDefaultWidthAndHeight && _connectedWithDefaultFps) != flag;
	}

	private void ReconfigureToolkit()
	{
		StopAndDisconnectLiveSource();
		ConnectAndStartLiveSource();
	}

	private bool IgnoreFrame(LiveContentEventArgs args, bool resolutionJustChanged)
	{
		int width = args.LiveContent.Width;
		int height = args.LiveContent.Height;
		if (_width != 0 && _height != 0 && (width != _width || height != _height) && (resolutionJustChanged || _setWidthHeight))
		{
			return true;
		}
		return false;
	}

	private void RemovableMasksLiftedChangedHandler(object sender, EventArgs e)
	{
		if (base.SessionId == null)
		{
			return;
		}
		try
		{
			UserContext userContext = Item.FQID.ServerId.UserContext;
			if (userContext != null)
			{
				InternalMediaService.Instance.SetRemovableMasksLifted(base.SessionId, userContext.PrivacyMaskLifted);
			}
		}
		catch (Exception)
		{
		}
	}
}
