using System;

namespace VideoOS.Common.API;

public static class CommandApiIDs
{
	public class SequenceTypeGuids
	{
		public static readonly Guid RecordingSequence = new Guid("F9C62604-D0C5-4050-AE25-72DE51639B14");

		public static readonly Guid MotionSequence = new Guid("53CB5E33-2183-44bd-9491-8364D2457480");

		public static readonly Guid RecordingWithTriggerSequence = new Guid("0601D294-B7E5-4d93-9614-9658561AD5E4");

		public static readonly Guid CompletedRetrieval = new Guid("D829B7D5-40B4-4A10-96E6-BEFECE93CBE5");

		public static readonly Guid InProgressRetrieval = new Guid("2E01C2EF-0D67-44BF-B0F9-97D0EEB06A01");
	}

	public class TimeLineInformationTypeGuids
	{
		public static readonly Guid RecordedDataAvailable = new Guid("78289503-CBF4-43be-9DC3-9F34A8B60E6D");

		public static readonly Guid ActivitiesDetected = new Guid("F445CE14-0038-4302-994E-39D99E4C0733");
	}
}
