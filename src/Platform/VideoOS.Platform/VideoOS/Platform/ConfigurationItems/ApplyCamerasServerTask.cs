using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ApplyCamerasServerTask : ServerTask
{
	public Collection<string> CameraPaths
	{
		get
		{
			return ConfigurationApiUtil.SeparatorStringToCollection(base.Wrapper.GetPropertyValue("CameraPaths"));
		}
		set
		{
			base.Wrapper.SetPropertyValue("CameraPaths", ConfigurationApiUtil.ToSeparatorString(value));
			base.Wrapper.IsDirty = true;
		}
	}

	public Collection<string> CameraGroupPaths
	{
		get
		{
			return ConfigurationApiUtil.SeparatorStringToCollection(base.Wrapper.GetPropertyValue("CameraGroupPaths"));
		}
		set
		{
			base.Wrapper.SetPropertyValue("CameraGroupPaths", ConfigurationApiUtil.ToSeparatorString(value));
			base.Wrapper.IsDirty = true;
		}
	}

	public int BeginPosition
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("BeginPosition");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("BeginPosition", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public ApplyCamerasServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("ApplyCameras"));
	}
}
