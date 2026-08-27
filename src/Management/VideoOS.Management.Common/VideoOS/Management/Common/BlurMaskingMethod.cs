namespace VideoOS.Management.Common;

public class BlurMaskingMethod : MaskingMethod
{
	public int Level { get; }

	public BlurMaskingMethod(PrivacyMask parent, int methodId, bool includeInMotionDetection, bool removable, int level)
		: base(parent, methodId, MaskType.Blurring, includeInMotionDetection, removable)
	{
		Level = level;
	}
}
