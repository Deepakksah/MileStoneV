namespace VideoOS.Management.Common;

public class SolidMaskingMethod : MaskingMethod
{
	public SolidMaskingMethod(PrivacyMask parent, int methodId, bool includeInMotionDetection, bool removable)
		: base(parent, methodId, MaskType.Solid, includeInMotionDetection, removable)
	{
	}
}
