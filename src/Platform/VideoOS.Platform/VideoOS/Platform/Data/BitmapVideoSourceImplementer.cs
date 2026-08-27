using VideoOS.Platform.Util;

namespace VideoOS.Platform.Data;

public class BitmapVideoSourceImplementer : GenericTranscodedVideoSource<BitmapData>
{
	public BitmapVideoSourceImplementer(Item item)
		: this(item, new SizeAndCropInfo())
	{
	}

	private BitmapVideoSourceImplementer(Item item, SizeAndCropInfo sizeAndCropInfo)
		: base(item, (IInternalCommandServiceImplementer<BitmapData>)new InternalCommandServiceImplementerBitmap
		{
			SizeAndCropInfo = sizeAndCropInfo
		}, sizeAndCropInfo)
	{
	}

	public override void Init(int width, int height)
	{
		Init(width, height, BitmapFormat.BGR24);
	}

	public virtual void Init(int width, int height, BitmapFormat format)
	{
		InitInternal(width, height, () => InternalCommandService.Instance.ImageExporterConnect(Item.FQID, width, height, base.KeepAspectRatio, base.FillWithBlack, format));
	}
}
