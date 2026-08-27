using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

public class UpdateHardwareServerTask : ServerTask
{
	public long SessionDataId
	{
		get
		{
			return long.Parse(base.Wrapper.GetPropertyValue("SessionDataId"), CultureInfo.InvariantCulture);
		}
		set
		{
			base.Wrapper.SetPropertyValue("SessionDataId", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public string HardwarePath => base.Wrapper.GetPropertyValue("HardwareId");

	public string HardwareName => base.Wrapper.GetPropertyValue("HardwareName");

	public bool AnyEventsUpdated => base.Wrapper.GetPropertyValueBool("HardwareChanged");

	public ReadOnlyCollection<UpdateHardwareDetail> UpdateHardwareDetails
	{
		get
		{
			List<UpdateHardwareDetail> list = new List<UpdateHardwareDetail>();
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem obj in array)
			{
				obj.Properties.TryGetPropertyValue("DevicePath", out var value);
				obj.Properties.TryGetPropertyValue("DeviceName", out var value2);
				obj.Properties.TryGetPropertyValue("ChangeType", out var value3);
				obj.Properties.TryGetPropertyValue("DeviceType", out var value4);
				list.Add(new UpdateHardwareDetail(value, value2, value4, value3));
			}
			return new ReadOnlyCollection<UpdateHardwareDetail>(list);
		}
	}

	public UpdateHardwareServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("UpdateHardware"));
	}
}
