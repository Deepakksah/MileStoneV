using System.Collections.Generic;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class CameraRelatedIdsEqualityComparer : IEqualityComparer<CameraRelatedIds>
{
	public bool Equals(CameraRelatedIds x, CameraRelatedIds y)
	{
		if (x.CameraId == y.CameraId && x.RelatedMicrohponeId == y.RelatedMicrohponeId)
		{
			return x.RelatedSpeakerId == y.RelatedSpeakerId;
		}
		return false;
	}

	public int GetHashCode(CameraRelatedIds obj)
	{
		return obj.CameraId.GetHashCode() ^ obj.RelatedMicrohponeId.GetHashCode() ^ obj.RelatedSpeakerId.GetHashCode();
	}
}
