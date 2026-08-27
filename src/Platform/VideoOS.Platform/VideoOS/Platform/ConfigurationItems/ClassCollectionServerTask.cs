using System.Collections.ObjectModel;
using System.Linq;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

public class ClassCollectionServerTask : ServerTask
{
	private Collection<IConfigurationItem> _classes;

	public Collection<string> SupportedChildItemType = new Collection<string> { "Camera", "Metadata", "Microphone", "Speaker", "Storage", "Task", "Stream", "PtzPreset", "ViewItem" };

	public Collection<IConfigurationItem> ClassCollection
	{
		get
		{
			if (_classes == null)
			{
				_classes = new Collection<IConfigurationItem>();
				ConfigurationItem[] array = base.Wrapper.Children();
				foreach (ConfigurationItem configurationItem in array)
				{
					switch (configurationItem.ItemType)
					{
					case "Camera":
						_classes.Add(new Camera(base.Wrapper.ServerId, configurationItem));
						break;
					case "Metadata":
						_classes.Add(new Metadata(base.Wrapper.ServerId, configurationItem));
						break;
					case "Microphone":
						_classes.Add(new Microphone(base.Wrapper.ServerId, configurationItem));
						break;
					case "Speaker":
						_classes.Add(new Speaker(base.Wrapper.ServerId, configurationItem));
						break;
					case "Storage":
						_classes.Add(new Storage(base.Wrapper.ServerId, configurationItem));
						break;
					case "Task":
						if (configurationItem.Properties.Any((Property p) => p.Key == "Progress") && configurationItem.Properties.Any((Property p) => p.Key == "State"))
						{
							_classes.Add(new TaskDetail(base.Wrapper.ServerId, configurationItem));
						}
						else
						{
							_classes.Add(new ChildTaskDetail(base.Wrapper.ServerId, configurationItem));
						}
						break;
					case "PtzPreset":
						_classes.Add(new PtzPreset(base.Wrapper.ServerId, configurationItem));
						break;
					case "ViewItem":
						_classes.Add(new ViewItem(base.Wrapper.ServerId, configurationItem));
						break;
					default:
						EnvironmentManager.Instance.Log(error: true, "ServerTask", "Missing handling of child with ItemType=" + configurationItem.ItemType);
						break;
					case "Stream":
						break;
					}
				}
			}
			return _classes;
		}
	}

	public Collection<IConfigurationChildItem> ChildItemClassCollection
	{
		get
		{
			Collection<IConfigurationChildItem> collection = new Collection<IConfigurationChildItem>();
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "Stream")
				{
					collection.Add(new StreamUsageChildItem(base.Wrapper.ServerId, configurationItem, base.Wrapper));
				}
			}
			return collection;
		}
	}

	public ClassCollectionServerTask(ConfigurationApiItemWrapper wrapper)
		: base(wrapper)
	{
	}
}
