using System;
using System.Collections.Generic;

namespace VideoOS.Platform.Data;

public class DataType
{
	public static class SequenceTypeGuids
	{
		public static readonly Guid RecordingSequence = new Guid("F9C62604-D0C5-4050-AE25-72DE51639B14");

		public static readonly Guid MotionSequence = new Guid("53CB5E33-2183-44bd-9491-8364D2457480");

		public static readonly Guid RecordingWithTriggerSequence = new Guid("0601D294-B7E5-4d93-9614-9658561AD5E4");

		public static readonly Guid TimelineRecording = new Guid("78289503-CBF4-43be-9DC3-9F34A8B60E6D");

		public static readonly Guid TimelineMotionDetected = new Guid("F445CE14-0038-4302-994E-39D99E4C0733");
	}

	public static class JPEGTypeGuids
	{
		public static readonly Guid JPEGDataType = new Guid("F195CCE6-55C2-4531-AE77-B88BC759740F");
	}

	public static class TimeLineInformationTypeGuids
	{
		public static readonly Guid RecordedDataAvailable = new Guid("78289503-CBF4-43be-9DC3-9F34A8B60E6D");

		public static readonly Guid ActivitiesDetected = new Guid("F445CE14-0038-4302-994E-39D99E4C0733");
	}

	public Guid Id;

	public string Name;

	public Dictionary<string, string> Properties = new Dictionary<string, string>();

	public override string ToString()
	{
		return Name;
	}
}
