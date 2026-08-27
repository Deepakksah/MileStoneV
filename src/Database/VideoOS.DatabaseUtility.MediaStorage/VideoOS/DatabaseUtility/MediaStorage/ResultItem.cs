using System.Collections.Generic;

namespace VideoOS.DatabaseUtility.MediaStorage;

public struct ResultItem(VerificationStatus status, IList<VerificationTimeSlice> timeSlices)
{
	public VerificationStatus Status = status;

	public IList<VerificationTimeSlice> TimeSlices = timeSlices;
}
