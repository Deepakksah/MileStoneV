using System;
using VideoOS.Platform.Client;
using VideoOS.Platform.Data.MetadataSuppliersInternal;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.Telemetry;

namespace VideoOS.Platform.Data;

public class MetadataSupplier
{
	private Item _metadataSourceItem;

	private InternalMetadataSupplier _supplier;

	private object _modeChanged;

	private readonly WindowInformation _windowInformation;

	public Item SourceItem => _metadataSourceItem;

	public event EventHandler<MetadataSupplierEventArgs> NewMetadataEvent;

	public event EventHandler<MetadataSupplierErrorEventArgs> ErrorEvent;

	public event EventHandler<MetadataSupplierMetadataInvalidatedEventArgs> MetadataInvalidatedEvent;

	public MetadataSupplier(Item metadataSourceItem)
		: this(metadataSourceItem, null)
	{
	}

	public MetadataSupplier(Item metadataSourceItem, WindowInformation windowInformation)
	{
		if (metadataSourceItem == null)
		{
			throw new ArgumentNullException("metadataSourceItem");
		}
		_metadataSourceItem = metadataSourceItem;
		_windowInformation = windowInformation;
		new TrackPageViewData(typeof(MetadataSupplier)).Send();
	}

	public bool Start()
	{
		if (_supplier != null)
		{
			return false;
		}
		_modeChanged = EnvironmentManager.Instance.RegisterReceiver(ModeChangedReceiver, new MessageIdFilter(MessageId.System.ModeChangedIndication));
		return DoStart();
	}

	public void Stop()
	{
		if (_modeChanged != null)
		{
			EnvironmentManager.Instance.UnRegisterReceiver(_modeChanged);
			_modeChanged = null;
		}
		DoStop();
	}

	private bool DoStart()
	{
		Mode mode = ((_windowInformation == null) ? EnvironmentManager.Instance.Mode : _windowInformation.Mode);
		if (mode == Mode.ClientLive || mode == Mode.ClientPlayback)
		{
			if (mode == Mode.ClientLive)
			{
				_supplier = new LiveMetadataSupplier(_metadataSourceItem);
			}
			else
			{
				_supplier = new PlaybackMetadataSupplier(_metadataSourceItem, _windowInformation);
			}
			_supplier.EventHandler += SupplierOnEventHandler;
			_supplier.ErrorEvent += SupplierOnErrorEvent;
			_supplier.MetadataInvalidatedEvent += SupplierOnMetadataInvalidatedEvent;
			return _supplier.Start();
		}
		return true;
	}

	private void SupplierOnMetadataInvalidatedEvent(object sender, InternalMetadataSupplierMetadataInvalidatedEventArgs e)
	{
		OnMetadataSupplierMetadataInvalidatedEvent(new MetadataSupplierMetadataInvalidatedEventArgs(e.InvalidDataDateTime));
	}

	private void DoStop()
	{
		if (_supplier != null)
		{
			try
			{
				_supplier.Stop();
				_supplier.EventHandler -= SupplierOnEventHandler;
				_supplier.ErrorEvent -= SupplierOnErrorEvent;
				_supplier.MetadataInvalidatedEvent -= SupplierOnMetadataInvalidatedEvent;
			}
			finally
			{
				_supplier.Dispose();
				_supplier = null;
			}
		}
	}

	private void SupplierOnErrorEvent(object sender, InternalMetadataSupplierErrorEventArgs e)
	{
		if (this.ErrorEvent != null)
		{
			try
			{
				this.ErrorEvent(this, new MetadataSupplierErrorEventArgs(e.ErrorText));
			}
			catch (Exception ex)
			{
				EnvironmentManager.Instance.Log(error: true, "MetadataSupplier.SupplierOnErrorEvent", "Exception when calling event handler: " + ex.Message);
			}
		}
	}

	private void SupplierOnEventHandler(object sender, InternalMetadataSupplierEventArgs e)
	{
		if (this.NewMetadataEvent != null)
		{
			try
			{
				this.NewMetadataEvent(this, new MetadataSupplierEventArgs(e.Data, e.Time, e.PreviousTime, e.NextTime));
			}
			catch (Exception ex)
			{
				EnvironmentManager.Instance.Log(error: true, "MetadataSupplier.SupplierOnEventHandler", "Exception when calling event handler: " + ex.Message);
			}
		}
	}

	private void OnMetadataSupplierMetadataInvalidatedEvent(MetadataSupplierMetadataInvalidatedEventArgs e)
	{
		this.MetadataInvalidatedEvent?.Invoke(this, e);
	}

	private bool IsSentFromMyWindow(FQID sender)
	{
		if (_windowInformation != null)
		{
			return _windowInformation.IsSentFromMyWindow(sender);
		}
		if (!(sender == null))
		{
			return sender.ObjectId == Guid.Empty;
		}
		return true;
	}

	private object ModeChangedReceiver(Message message, FQID destination, FQID sender)
	{
		if (IsSentFromMyWindow(sender))
		{
			if (_supplier != null)
			{
				if (_supplier.LatestMetadataTimeUtc != DateTime.MinValue)
				{
					OnMetadataSupplierMetadataInvalidatedEvent(new MetadataSupplierMetadataInvalidatedEventArgs(_supplier.LatestMetadataTimeUtc));
				}
				DoStop();
			}
			DoStart();
		}
		return null;
	}
}
