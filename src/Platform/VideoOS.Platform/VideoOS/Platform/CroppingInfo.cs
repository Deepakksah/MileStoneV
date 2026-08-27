namespace VideoOS.Platform;

public abstract class CroppingInfo
{
	public bool CroppingDefined { get; set; }

	public double CropX { get; set; }

	public double CropY { get; set; }

	public double CropWidth { get; set; }

	public double CropHeight { get; set; }
}
