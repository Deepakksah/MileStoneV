using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Platform.Metadata;

namespace VideoOS.Platform.Data.MetadataSuppliersInternal;

public abstract class InternalMetadataSupplier : IDisposable
{
	protected Item _metadataSourceItem;

	protected int validTimeInSecondsGreatest;

	public DateTime LatestMetadataTimeUtc { get; set; }

	public event InternalMetadataSupplierEventHandler EventHandler;

	public event InternalMetadataSupplierErrorEventHandler ErrorEvent;

	public event EventHandler<InternalMetadataSupplierMetadataInvalidatedEventArgs> MetadataInvalidatedEvent;

	public InternalMetadataSupplier(Item metadataSourceItem)
	{
		_metadataSourceItem = metadataSourceItem;
		IEnumerable<string> source = from dt in new MetadataSource(metadataSourceItem).GetTypes()
			select dt.Properties["ValidTime"];
		if (source.Any())
		{
			validTimeInSecondsGreatest = int.Parse(source.Max(), CultureInfo.InvariantCulture);
		}
	}

	public abstract bool Start();

	public abstract bool Stop();

	protected void ProcessMetadataContent(MetadataContent content, DateTime metadataTime, DateTime? previousTime = null, DateTime? nextTime = null)
	{
		if (content != null && this.EventHandler != null)
		{
			this.EventHandler(this, new InternalMetadataSupplierEventArgs(content, metadataTime, previousTime, nextTime));
		}
	}

	protected void OnError(string errorText)
	{
		if (this.ErrorEvent != null)
		{
			this.ErrorEvent(this, new InternalMetadataSupplierErrorEventArgs(errorText));
		}
	}

	protected void OnInvalidMetadata(DateTime invalidDateTime)
	{
		if (this.MetadataInvalidatedEvent != null)
		{
			this.MetadataInvalidatedEvent(this, new InternalMetadataSupplierMetadataInvalidatedEventArgs(invalidDateTime));
		}
	}

	public abstract void Dispose();
}
