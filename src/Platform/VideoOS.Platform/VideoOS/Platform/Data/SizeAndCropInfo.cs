namespace VideoOS.Platform.Data;

public class SizeAndCropInfo
{
	public int Width;

	public int Height;

	public bool SetWidthHeight;

	public int Compression = 87;

	public bool ChangeCompression;

	public double CropX;

	public double CropY;

	public double CropWidth = 1.0;

	public double CropHeight = 1.0;

	public bool SetCrop;

	public double CropXSet;

	public double CropYSet;

	public double CropWidthSet = 1.0;

	public double CropHeightSet = 1.0;

	public bool KeepAspectRatio;

	public bool FillWithBlack = true;

	public bool SuspendSettingsUpdate;

	public bool AllowUpscaling = true;

	public bool CropDefined
	{
		get
		{
			if (CropWidthSet == 1.0)
			{
				return CropHeightSet != 1.0;
			}
			return true;
		}
	}

	public void FillCropInfo(CroppingInfo croppingInfo)
	{
		if (croppingInfo != null)
		{
			croppingInfo.CropX = CropXSet;
			croppingInfo.CropY = CropYSet;
			croppingInfo.CropWidth = CropWidthSet;
			croppingInfo.CropHeight = CropHeightSet;
			croppingInfo.CroppingDefined = CropDefined;
		}
	}

	public void FillCropInfo(JPEGData jpegData)
	{
		if (jpegData != null)
		{
			jpegData.CropX = CropXSet;
			jpegData.CropY = CropYSet;
			jpegData.CropWidth = CropWidthSet;
			jpegData.CropHeight = CropHeightSet;
			jpegData.CroppingDefined = CropDefined;
		}
	}
}
