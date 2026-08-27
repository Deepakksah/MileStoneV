using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ChangeDeviceRecordingStorageServerTask : ServerTask
{
	public Dictionary<string, string> ItemSelectionValues => base.Wrapper.GetEnumValues("ItemSelection");

	public string ItemSelection
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ItemSelection");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ItemSelection", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool MoveData
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("moveData");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("moveData", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public ChangeDeviceRecordingStorageServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("ChangeDeviceRecordingStorage"));
	}
}
