using VideoOS.Platform.Util;

namespace VideoOS.Platform.Data;

public class JPEGVideoSourceImplementer : GenericTranscodedVideoSource<JPEGData>
{
	public JPEGVideoSourceImplementer(Item item)
		: this(item, new SizeAndCropInfo())
	{
	}

	private JPEGVideoSourceImplementer(Item item, SizeAndCropInfo sizeAndCropInfo)
		: base(item, (IInternalCommandServiceImplementer<JPEGData>)new InternalCommandServiceImplementerJpegData
		{
			SizeAndCropInfo = sizeAndCropInfo
		}, sizeAndCropInfo)
	{
	}

	public override void Init(int width, int height)
	{
		InitInternal(width, height, () => InternalCommandService.Instance.ImageExporterConnect(Item.FQID, width, height, base.KeepAspectRatio, base.FillWithBlack, base.AllowUpscaling));
	}
}
