using System;
using System.Xml;
using VideoOS.Platform.Telemetry;

namespace VideoOS.Platform.Client;

public abstract class ViewItemToolbarPlugin : PropertyClass
{
	private ViewItemToolbarPlaceDefinition _viewItemToolbarPlaceDefinition = new ViewItemToolbarPlaceDefinition();

	public abstract Guid Id { get; }

	public abstract string Name { get; }

	public ViewItemToolbarPlaceDefinition ViewItemToolbarPlaceDefinition => _viewItemToolbarPlaceDefinition;

	public virtual ToolbarPluginOverflowMode ToolbarPluginOverflowMode => ToolbarPluginOverflowMode.AsNeeded;

	public virtual bool AllowWindowlessActivation => false;

	public virtual ToolbarPluginType ToolbarPluginType => ToolbarPluginType.Action;

	protected ViewItemToolbarPlugin()
	{
		new TrackPageViewData(typeof(ViewItemToolbarPlugin)).Send();
	}

	public abstract void Init();

	public abstract void Close();

	public abstract ViewItemToolbarPluginInstance GenerateViewItemToolbarPluginInstance();

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
