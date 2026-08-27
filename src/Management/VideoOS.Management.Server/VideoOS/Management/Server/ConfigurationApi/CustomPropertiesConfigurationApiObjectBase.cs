using System;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server.ConfigurationApi;

public abstract class CustomPropertiesConfigurationApiObjectBase : ConfigurationApiBase, IConfigurationApiSupport
{
	private class DefaultRepository : ICustomPropertiesRepository
	{
		public static ICustomPropertiesRepository Instance { get; } = new DefaultRepository();

		private DefaultRepository()
		{
		}

		public CustomProperty[] GetCustomProperties(IGeminiRequestContext requestContext, string objectId)
		{
			return CustomProperty.GetCustomProperties(requestContext, objectId);
		}

		public void UpdateCustomProperties(IGeminiRequestContext requestContext, string objectId, CustomProperty[] customPropertyArray)
		{
			CustomProperty.UpdateCustomProperties(requestContext, objectId, customPropertyArray);
		}

		public void DeleteCustomProperty(IGeminiRequestContext requestContext, string objectId, string name)
		{
			CustomProperty.DeleteCustomProperty(requestContext, objectId, name);
		}

		public bool DeviceExists(IGeminiRequestContext requestContext, string deviceId)
		{
			if (Device.DeviceExists(requestContext, deviceId))
			{
				return true;
			}
			if (Hardware.HardwareExists(requestContext, deviceId))
			{
				return true;
			}
			return false;
		}

		public string GetDeviceType(IGeminiRequestContext requestContext, string deviceId)
		{
			Device device = Device.QueryDeviceById(requestContext, deviceId);
			if (device != null)
			{
				return device.DeviceType switch
				{
					DeviceType.Camera => "Camera", 
					DeviceType.Metadata => "Metadata", 
					DeviceType.Microphone => "Microphone", 
					DeviceType.Speaker => "Speaker", 
					DeviceType.Output => "Output", 
					DeviceType.Input => "InputEvent", 
					_ => throw new ArgumentException("Device type could not be identified for type:" + device.DeviceType, "deviceId"), 
				};
			}
			if (Hardware.QueryHardwareById(requestContext, deviceId) != null)
			{
				return "Hardware";
			}
			throw new ArgumentException("Device type could not be identified", "deviceId");
		}
	}

	protected ICustomPropertiesRepository Repository { get; }

	protected CustomPropertiesConfigurationApiObjectBase(ICustomPropertiesRepository repository)
	{
		Repository = repository ?? DefaultRepository.Instance;
	}

	public abstract ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath);

	public abstract ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath);

	public abstract ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path);

	public abstract ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path);

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string methodId)
	{
		throw new NotSupportedException();
	}

	protected static void ParamNullCheck(object param, string name)
	{
		if (param != null)
		{
			return;
		}
		throw new ArgumentNullException(name ?? string.Empty);
	}

	protected abstract void ThrowIfPathInvalid(ConfigurationItemPath path, IGeminiRequestContext requestContext);

	protected abstract void ThrowIfParentPathInvalid(ConfigurationItemPath path);
}
