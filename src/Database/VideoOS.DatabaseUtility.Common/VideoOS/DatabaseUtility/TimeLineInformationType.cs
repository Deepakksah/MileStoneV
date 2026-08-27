using System;

namespace VideoOS.DatabaseUtility;

[Flags]
public enum TimeLineInformationType
{
	RecordedDataAvailable = 1,
	ActivitiesDetected = 2
}
