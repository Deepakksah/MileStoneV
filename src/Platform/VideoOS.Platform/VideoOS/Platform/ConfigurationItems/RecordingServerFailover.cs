using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class RecordingServerFailover : IConfigurationItem
{
	public string Id => base.Wrapper.GetPropertyValue("Id");

	public Dictionary<string, string> PrimaryFailoverGroupValues => base.Wrapper.GetEnumValues("PrimaryFailoverGroup");

	public string PrimaryFailoverGroup
	{
		get
		{
			return base.Wrapper.GetPropertyValue("PrimaryFailoverGroup");
		}
		set
		{
			base.Wrapper.SetPropertyValue("PrimaryFailoverGroup", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> SecondaryFailoverGroupValues => base.Wrapper.GetEnumValues("SecondaryFailoverGroup");

	public string SecondaryFailoverGroup
	{
		get
		{
			return base.Wrapper.GetPropertyValue("SecondaryFailoverGroup");
		}
		set
		{
			base.Wrapper.SetPropertyValue("SecondaryFailoverGroup", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> HotStandbyValues => base.Wrapper.GetEnumValues("HotStandby");

	public string HotStandby
	{
		get
		{
			return base.Wrapper.GetPropertyValue("HotStandby");
		}
		set
		{
			base.Wrapper.SetPropertyValue("HotStandby", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public int FailoverPort
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("FailoverPort");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("FailoverPort", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public RecordingServerFailover(ServerId serverId, ConfigurationItem recordingServerFailover)
		: base(serverId, recordingServerFailover)
	{
	}

	public RecordingServerFailover(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public RecordingServerFailover(FQID fqid)
		: base(fqid)
	{
	}
}
