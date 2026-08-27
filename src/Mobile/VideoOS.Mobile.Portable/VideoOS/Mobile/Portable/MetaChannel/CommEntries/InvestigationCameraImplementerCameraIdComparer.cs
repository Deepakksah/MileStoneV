using System.Collections.Generic;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class InvestigationCameraImplementerCameraIdComparer : IEqualityComparer<InvestigationCameraImplementer>
{
	public bool Equals(InvestigationCameraImplementer x, InvestigationCameraImplementer y)
	{
		return x.CameraId == y.CameraId;
	}

	public int GetHashCode(InvestigationCameraImplementer obj)
	{
		return obj.CameraId.GetHashCode();
	}
}
