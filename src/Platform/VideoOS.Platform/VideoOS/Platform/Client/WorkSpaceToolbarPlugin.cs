using System;
using System.Xml;
using VideoOS.Platform.Telemetry;

namespace VideoOS.Platform.Client;

public abstract class WorkSpaceToolbarPlugin : PropertyClass
{
	private WorkSpaceToolbarPlaceDefinition _workSpaceToolbarPlaceDefinition = new WorkSpaceToolbarPlaceDefinition();

	public abstract Guid Id { get; }

	public abstract string Name { get; }

	public WorkSpaceToolbarPlaceDefinition WorkSpaceToolbarPlaceDefinition => _workSpaceToolbarPlaceDefinition;

	public virtual ToolbarPluginType ToolbarPluginType => ToolbarPluginType.Action;

	public virtual Guid? GroupId { get; }

	protected WorkSpaceToolbarPlugin()
	{
		new TrackPageViewData(typeof(WorkSpaceToolbarPlugin)).Send();
	}

	public abstract void Init();

	public abstract void Close();

	public abstract WorkSpaceToolbarPluginInstance GenerateWorkSpaceToolbarPluginInstance();

	public bool LoadProperties(bool userPrivate)
	{
		XmlNode optionsConfiguration = Configuration.Instance.GetOptionsConfiguration(Id, userPrivate);
		if (optionsConfiguration != null)
		{
			Deserialize(optionsConfiguration);
			return true;
		}
		return false;
	}

	public bool SaveProperties(bool userPrivate)
	{
		Configuration.Instance.SaveOptionsConfiguration(Id, userPrivate, SerializeToXmlNode());
		return true;
	}
}
