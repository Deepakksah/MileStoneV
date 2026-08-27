using System;
using System.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace VideoOS.Common.Integration.Configuration;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class DeviceTrack : IConfigurationItemRow
{
	private ConfigurationDataSet.DeviceTrackRow datarow;

	public string DebuggerDisplay => FormattableString.Invariant(FormattableStringFactory.Create("{0}. {1}{2}{3}{4}", "DeviceTrack", IsPrimary ? "Primary" : (IsEdge ? "Edge" : (IsSecondary ? "Secondary" : IDTrackDefinition.ToString())), DefaultPlayback ? ", DefaultPlayback" : "", MergeEdge ? ", MergeEdge" : "", InUse ? ", InUse" : ""));

	public DataRow DataRow
	{
		[DebuggerStepThrough]
		get
		{
			return datarow;
		}
	}

	public Guid IDDevice
	{
		[DebuggerStepThrough]
		get
		{
			return new Guid(datarow.IDDevice);
		}
	}

	public Guid IDTrack
	{
		[DebuggerStepThrough]
		get
		{
			return datarow.IDTrack;
		}
	}

	public Guid IDTrackDefinition
	{
		[DebuggerStepThrough]
		get
		{
			return datarow.IDTrackDefinition;
		}
	}

	public bool MergeEdge
	{
		[DebuggerStepThrough]
		get
		{
			return datarow.MergeEdge;
		}
		[DebuggerStepThrough]
		set
		{
			datarow.MergeEdge = value;
		}
	}

	public bool DefaultPlayback
	{
		[DebuggerStepThrough]
		get
		{
			return datarow.DefaultPlayback;
		}
		[DebuggerStepThrough]
		set
		{
			datarow.DefaultPlayback = value;
		}
	}

	public bool InUse
	{
		[DebuggerStepThrough]
		get
		{
			return datarow.InUse;
		}
		[DebuggerStepThrough]
		set
		{
			datarow.InUse = value;
		}
	}

	public bool IsPrimary
	{
		[DebuggerStepThrough]
		get
		{
			return IDTrackDefinition == WellKnownDeviceTrackDefinition.Primary;
		}
	}

	public bool IsSecondary
	{
		[DebuggerStepThrough]
		get
		{
			return IDTrackDefinition == WellKnownDeviceTrackDefinition.Secondary;
		}
	}

	public bool IsEdge
	{
		[DebuggerStepThrough]
		get
		{
			return IDTrackDefinition == WellKnownDeviceTrackDefinition.Edge;
		}
	}

	public DeviceTrack(ConfigurationDataSet.DeviceTrackRow row)
	{
		datarow = row;
	}

	public void SetStartOfMedia(DateTime startTime)
	{
		datarow.CreatedDate = startTime;
		datarow.OnlyUseMediaAfter = startTime;
	}
}
