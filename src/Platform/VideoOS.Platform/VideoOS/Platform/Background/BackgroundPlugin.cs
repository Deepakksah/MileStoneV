using System;
using System.Collections.Generic;
using System.Xml;
using VideoOS.Platform.Client;

namespace VideoOS.Platform.Background;

public abstract class BackgroundPlugin : PropertyClass
{
	public abstract Guid Id { get; }

	public abstract string Name { get; }

	public abstract List<EnvironmentType> TargetEnvironments { get; }

	public abstract void Init();

	public abstract void Close();

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

	public XmlNode LoadSettingsXml(Guid id)
	{
		return Configuration.Instance.GetOptionsConfiguration(id, userPrivate: false);
	}

	public bool SaveProperties(bool userPrivate)
	{
		Configuration.Instance.SaveOptionsConfiguration(Id, userPrivate, SerializeToXmlNode());
		return true;
	}
}
