using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class RemoveCamerasServerTask : ServerTask
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

	public RemoveCamerasServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("RemoveCameras"));
	}
}
