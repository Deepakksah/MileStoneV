using System;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Data;

public class InternalCommandServiceImplementerBitmap : IInternalCommandServiceImplementer<BitmapData>
{
	public Func<object, FQID, DateTime, BitmapData> GetNearest => InternalCommandService.Instance.ImageExporterBitmapGetNearest;

	public Func<object, FQID, DateTime, BitmapData> GetAt => InternalCommandService.Instance.ImageExporterBitmapGetAt;

	public Func<object, FQID, DateTime, BitmapData> GetAtOrBefore => InternalCommandService.Instance.ImageExporterBitmapGetAtOrBefore;

	public Func<object, FQID, DateTime, BitmapData> GetAfter => null;

	public Func<object, FQID, DateTime, BitmapData> GetBefore => null;

	public Func<object, string, BitmapData> Command => InternalCommandService.Instance.ImageExporterBitmapCommand;

	public Func<BitmapData, ISourceData> Convertor => (BitmapData bitmapData) => new SourceDataOnBitmapDataProxy(bitmapData);

	public Action<BitmapData> OnData => delegate(BitmapData data)
	{
		SizeAndCropInfo.FillCropInfo(data);
	};

	public SizeAndCropInfo SizeAndCropInfo { get; set; }
}
