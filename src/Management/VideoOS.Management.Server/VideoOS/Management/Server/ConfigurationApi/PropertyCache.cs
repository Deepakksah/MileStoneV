using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Common.SharedComponent;
using VideoOS.ConfigurationAPI;

namespace VideoOS.Management.Server.ConfigurationApi;

public class PropertyCache : IDisposable
{
	private bool disposedValue;

	private Dictionary<Guid, CustomProperty> _customProperties = new Dictionary<Guid, CustomProperty>();

	public void FillConfigurationItemProperties(IGeminiRequestContext requestContext, ConfigurationItem[] configurationItems, List<string> itemTypes)
	{
		bool flag = false;
		if (itemTypes.Contains("ViewGroup"))
		{
			flag = true;
		}
		if (flag)
		{
			CustomProperty[] customPropertiesByNameNoSecurity = CustomProperty.GetCustomPropertiesByNameNoSecurity(requestContext, VMOConstants.CustomPropertyXmlDefinition);
			bool flag2 = false;
			CustomProperty[] array = customPropertiesByNameNoSecurity;
			foreach (CustomProperty customProperty in array)
			{
				if (Guid.TryParse(customProperty.ObjectId, out var result))
				{
					_customProperties[result] = customProperty;
				}
				else
				{
					flag2 = true;
				}
			}
			if (flag2)
			{
				GeminiTrace.Warning(TraceType.General, "CustomProperty with empty ObjectId found. This should not happen.");
			}
		}
		FillConfigurationItems(configurationItems);
	}

	private void FillConfigurationItems(ConfigurationItem[] configurationItems)
	{
		foreach (ConfigurationItem configurationItem in configurationItems)
		{
			if (configurationItem.ItemType == "ViewGroup")
			{
				FillCustomProperty(configurationItem);
			}
			if (configurationItem.ChildrenFilled && configurationItem.Children != null && configurationItem.Children.Any())
			{
				FillConfigurationItems(configurationItem.Children);
			}
		}
	}

	private void FillCustomProperty(ConfigurationItem item)
	{
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreatePath(item.Path);
		Guid key = new Guid(configurationItemPath.Id);
		if (_customProperties.TryGetValue(key, out var value))
		{
			Property property = item.Properties.FirstOrDefault((Property p) => p.Key == "ViewGroupDataXml");
			if (property != null)
			{
				property.Value = value.Value;
			}
		}
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{
				_customProperties.Clear();
			}
			disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
