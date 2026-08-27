using System;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Data;

public class InternalCommandServiceImplementerJpegData : IInternalCommandServiceImplementer<JPEGData>
{
	public Func<object, FQID, DateTime, JPEGData> GetNearest => InternalCommandService.Instance.ImageExporterJPEGGetNearest;

	public Func<object, FQID, DateTime, JPEGData> GetAt => InternalCommandService.Instance.ImageExporterJPEGGetAt;

	public Func<object, FQID, DateTime, JPEGData> GetAtOrBefore => InternalCommandService.Instance.ImageExporterJPEGGetAtOrBefore;

	public Func<object, FQID, DateTime, JPEGData> GetAfter => null;

	public Func<object, FQID, DateTime, JPEGData> GetBefore => null;

	public Func<object, string, JPEGData> Command => InternalCommandService.Instance.ImageExporterCommand;

	public Func<JPEGData, ISourceData> Convertor => (JPEGData jpegData) => new SourceDataOnJpegDataProxy(jpegData);

	public Action<JPEGData> OnData => delegate(JPEGData data)
	{
		SizeAndCropInfo.FillCropInfo(data);
	};

	public SizeAndCropInfo SizeAndCropInfo { get; set; }
}
