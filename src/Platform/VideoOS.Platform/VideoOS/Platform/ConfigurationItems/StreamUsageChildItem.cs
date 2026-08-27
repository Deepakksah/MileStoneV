using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class StreamUsageChildItem : IConfigurationChildItem
{
	public Dictionary<string, string> StreamReferenceIdValues => base.Wrapper.GetEnumValues("StreamReferenceId");

	public string StreamReferenceId
	{
		get
		{
			return base.Wrapper.GetPropertyValue("StreamReferenceId");
		}
		set
		{
			base.Wrapper.SetPropertyValue("StreamReferenceId", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool LiveDefault
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("LiveDefault");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("LiveDefault", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> LiveModeValues => base.Wrapper.GetEnumValues("LiveMode");

	public string LiveMode
	{
		get
		{
			return base.Wrapper.GetPropertyValue("LiveMode");
		}
		set
		{
			base.Wrapper.SetPropertyValue("LiveMode", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> RecordToValues => base.Wrapper.GetEnumValues("RecordTo");

	public string RecordTo
	{
		get
		{
			return base.Wrapper.GetPropertyValue("RecordTo");
		}
		set
		{
			base.Wrapper.SetPropertyValue("RecordTo", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool DefaultPlayback
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("DefaultPlayback");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("DefaultPlayback", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public bool UseEdge
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("UseEdge");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("UseEdge", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public string EdgeTrackId => base.Wrapper.GetPropertyValue("EdgeTrackId");

	[Obsolete("Deprecated. Please use 'RecordTo' instead")]
	public bool Record
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("Record");
			if (propertyValue == null && RecordTo != null)
			{
				string b = "16CE3AA1-5F93-458A-ABE5-5C95D9ED1372";
				return string.Equals(RecordTo, b, StringComparison.OrdinalIgnoreCase);
			}
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("Record", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public StreamUsageChildItem(ServerId serverId, ConfigurationItem streamUsage, ConfigurationApiItemWrapper parentWrapper)
		: base(serverId, streamUsage, parentWrapper)
	{
	}

	public StreamUsageChildItem(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public StreamUsageChildItem(FQID fqid)
		: base(fqid)
	{
	}
}
