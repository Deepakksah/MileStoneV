using System;
using System.Collections.ObjectModel;

namespace VideoOS.Common.Integration.Drivers;

public static class GlobalIDs
{
	public static readonly Guid MediaColumnGuid = new Guid("7EA9B53B-749E-420b-891B-43B238F6144E");

	public static readonly Guid RecordingSequenceGuid = new Guid("8B742DF9-5BA1-49df-ACE1-66E54F5DC295");

	public static readonly Guid MotionSequenceGuid = new Guid("908D3E34-3510-4372-9E3F-20C53B92490A");

	public static readonly Guid MotionLevelGuid = new Guid("A127BE95-B3ED-4F0B-999B-07EFE8D16F9F");

	public static readonly Guid SignatureColumnGuid = new Guid("F3F39289-0372-410D-8936-802B1E627567");

	public static readonly Guid ImmerVisionPlugInGuid = new Guid("4807A4B7-6F6B-456E-A062-141586ED733C");

	public static ReadOnlyCollection<Guid> AllColumnIds { get; } = new ReadOnlyCollection<Guid>(new Guid[5] { MediaColumnGuid, RecordingSequenceGuid, MotionSequenceGuid, MotionLevelGuid, SignatureColumnGuid });
}
