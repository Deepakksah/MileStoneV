using System;
using System.Diagnostics;
using System.Globalization;
using VideoOS.Common.Integration.Definition.Schema;

namespace VideoOS.Common.Integration.Definition;

public class StreamCombinations
{
	private DeviceDefinition deviceDefinition;

	private supportedstreamtypeType supportedStreams;

	public MainDefinition MainDefinition => deviceDefinition.MainDefinition;

	public DeviceDefinition DeviceDefinition
	{
		[DebuggerStepThrough]
		get
		{
			return deviceDefinition;
		}
	}

	public supportedstreamtypeType Base
	{
		[DebuggerStepThrough]
		get
		{
			return supportedStreams;
		}
	}

	public Guid StreamTypeId
	{
		[DebuggerStepThrough]
		get
		{
			return new Guid(supportedStreams.referenceid);
		}
	}

	public StreamCombinations(DeviceDefinition deviceDefinition, supportedstreamtypeType supportedStreams)
	{
		this.deviceDefinition = deviceDefinition;
		this.supportedStreams = supportedStreams;
	}

	public int FindStreamMaxCount(Guid streamTypeId)
	{
		streamcountType[] allowedconcurrent = supportedStreams.allowedconcurrent;
		foreach (streamcountType streamcountType2 in allowedconcurrent)
		{
			if (new Guid(streamcountType2.streamtype) == streamTypeId)
			{
				return int.Parse(streamcountType2.value, NumberFormatInfo.InvariantInfo);
			}
		}
		return 0;
	}
}
