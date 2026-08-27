using System;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Data;

public abstract class GenericTranscodedVideoSource<TDataType> : GenericVideoSource<TDataType> where TDataType : class
{
	private object _customData;

	private bool _setCustomData;

	private readonly SizeAndCropInfo _sizeAndCropInfo;

	public virtual int Height
	{
		get
		{
			return _sizeAndCropInfo.Height;
		}
		set
		{
			_sizeAndCropInfo.Height = value;
		}
	}

	public virtual int Width
	{
		get
		{
			return _sizeAndCropInfo.Width;
		}
		set
		{
			_sizeAndCropInfo.Width = value;
		}
	}

	public bool AllowUpscaling
	{
		get
		{
			return _sizeAndCropInfo.AllowUpscaling;
		}
		set
		{
			_sizeAndCropInfo.AllowUpscaling = value;
		}
	}

	public bool KeepAspectRatio => _sizeAndCropInfo.KeepAspectRatio;

	public bool FillWithBlack => _sizeAndCropInfo.FillWithBlack;

	public int Compression
	{
		get
		{
			return _sizeAndCropInfo.Compression;
		}
		set
		{
			lock (base.SyncObject)
			{
				_sizeAndCropInfo.Compression = value;
				_sizeAndCropInfo.ChangeCompression = true;
				if (base.SessionId != null && !_sizeAndCropInfo.SuspendSettingsUpdate)
				{
					CheckSetCompression();
				}
			}
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
			lock (base.SyncObject)
			{
				_customData = value;
				_setCustomData = true;
				if (base.SessionId != null && !_sizeAndCropInfo.SuspendSettingsUpdate)
				{
					CheckSetCustomData();
				}
			}
		}
	}

	protected GenericTranscodedVideoSource(Item item, IInternalCommandServiceImplementer<TDataType> implementer, SizeAndCropInfo sizeAndCropInfo)
		: base(item, new Guid[1] { Kind.Camera }, implementer)
	{
		_sizeAndCropInfo = sizeAndCropInfo;
	}

	public virtual void SetKeepAspectRatio(bool keepAspectRatio, bool fillSpace)
	{
		_sizeAndCropInfo.KeepAspectRatio = keepAspectRatio;
		_sizeAndCropInfo.FillWithBlack = fillSpace;
	}

	public override void Init()
	{
		Init(_sizeAndCropInfo.Width, _sizeAndCropInfo.Height);
	}

	protected void InitInternal(int width, int height, Func<object> imageExporterConnect)
	{
		_sizeAndCropInfo.Width = width;
		_sizeAndCropInfo.Height = height;
		_sizeAndCropInfo.SetWidthHeight = false;
		InitInternal(imageExporterConnect, delegate
		{
			CheckSetCrop();
			SetDecoderWidthHeight();
			CheckSetCompression();
			CheckSetCustomData();
			SubscribePrivacyMaskLiftChanged();
		});
	}

	private bool CheckSetWidthHeight()
	{
		bool result = false;
		if (_sizeAndCropInfo.SetWidthHeight)
		{
			InternalCommandService.Instance.SetWidthHeight(base.SessionId, _sizeAndCropInfo.Width, _sizeAndCropInfo.Height, _sizeAndCropInfo.AllowUpscaling);
			_sizeAndCropInfo.SetWidthHeight = false;
			result = true;
		}
		return result;
	}

	private bool CheckSetCrop()
	{
		bool result = false;
		if (_sizeAndCropInfo.SetCrop)
		{
			InternalCommandService.Instance.SetCropRectangle(base.SessionId, _sizeAndCropInfo.CropX, _sizeAndCropInfo.CropY, _sizeAndCropInfo.CropWidth, _sizeAndCropInfo.CropHeight);
			_sizeAndCropInfo.CropXSet = _sizeAndCropInfo.CropX;
			_sizeAndCropInfo.CropYSet = _sizeAndCropInfo.CropY;
			_sizeAndCropInfo.CropWidthSet = _sizeAndCropInfo.CropWidth;
			_sizeAndCropInfo.CropHeightSet = _sizeAndCropInfo.CropHeight;
			_sizeAndCropInfo.SetCrop = false;
			result = true;
		}
		return result;
	}

	private void CheckSetCompression()
	{
		if (_sizeAndCropInfo.ChangeCompression)
		{
			InternalCommandService.Instance.SetCompression(base.SessionId, _sizeAndCropInfo.Compression);
			_sizeAndCropInfo.ChangeCompression = false;
		}
	}

	private void CheckSetCustomData()
	{
		if (_setCustomData)
		{
			InternalCommandService.Instance.SetCustomData(base.SessionId, _customData);
			_setCustomData = false;
		}
	}

	private void SetDecoderWidthHeight()
	{
		int width = _sizeAndCropInfo.Width;
		int height = _sizeAndCropInfo.Height;
		if (_sizeAndCropInfo.CropDefined)
		{
			width = (height = 0);
		}
		InternalCommandService.Instance.SetDecoderWidthHeight(base.SessionId, width, height);
	}

	private void SubscribePrivacyMaskLiftChanged()
	{
		if (Item.FQID.ServerId.UserContext != null)
		{
			UserContext userContext = Item.FQID.ServerId.UserContext;
			userContext.PrivacyMaskLiftChanged = (EventHandler)Delegate.Combine(userContext.PrivacyMaskLiftChanged, new EventHandler(RemovableMasksLiftedChangedHandler));
		}
	}

	private void UnsubscribePrivacyMaskLiftChanged()
	{
		if (Item.FQID.ServerId.UserContext != null)
		{
			UserContext userContext = Item.FQID.ServerId.UserContext;
			userContext.PrivacyMaskLiftChanged = (EventHandler)Delegate.Remove(userContext.PrivacyMaskLiftChanged, new EventHandler(RemovableMasksLiftedChangedHandler));
		}
	}

	public void SuspendSettingsUpdate()
	{
		if (base.SessionId == null)
		{
			return;
		}
		lock (base.SyncObject)
		{
			_sizeAndCropInfo.SuspendSettingsUpdate = true;
		}
	}

	public void ResumeSettingsUpdate()
	{
		if (base.SessionId == null)
		{
			return;
		}
		lock (base.SyncObject)
		{
			_sizeAndCropInfo.SuspendSettingsUpdate = false;
			if ((0u | (CheckSetWidthHeight() ? 1u : 0u) | (CheckSetCrop() ? 1u : 0u)) != 0)
			{
				SetDecoderWidthHeight();
			}
			CheckSetCompression();
			CheckSetCustomData();
		}
	}

	public void SetWidthHeight()
	{
		lock (base.SyncObject)
		{
			bool flag = false;
			_sizeAndCropInfo.SetWidthHeight = true;
			if (base.SessionId != null && !_sizeAndCropInfo.SuspendSettingsUpdate)
			{
				flag = CheckSetWidthHeight();
			}
			if (flag)
			{
				SetDecoderWidthHeight();
			}
		}
	}

	public void SetCropRectangle(double x, double y, double width, double height)
	{
		lock (base.SyncObject)
		{
			bool flag = false;
			_sizeAndCropInfo.CropX = x;
			_sizeAndCropInfo.CropY = y;
			_sizeAndCropInfo.CropWidth = width;
			_sizeAndCropInfo.CropHeight = height;
			_sizeAndCropInfo.SetCrop = true;
			if (base.SessionId != null && !_sizeAndCropInfo.SuspendSettingsUpdate)
			{
				flag = CheckSetCrop();
			}
			if (flag)
			{
				SetDecoderWidthHeight();
			}
		}
	}

	public override void Close()
	{
		base.Close();
		UnsubscribePrivacyMaskLiftChanged();
	}

	private void RemovableMasksLiftedChangedHandler(object sender, EventArgs e)
	{
		if (base.SessionId == null)
		{
			return;
		}
		lock (base.SyncObject)
		{
			try
			{
				if (Item.FQID.ServerId.UserContext != null)
				{
					InternalCommandService.Instance.SetRemovableMasksLifted(base.SessionId, Item.FQID.ServerId.UserContext.PrivacyMaskLifted);
				}
			}
			catch (Exception)
			{
			}
		}
	}
}
