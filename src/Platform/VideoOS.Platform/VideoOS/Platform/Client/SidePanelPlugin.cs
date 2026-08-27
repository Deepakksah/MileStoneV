using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using VideoOS.Platform.Telemetry;
using VideoOS.Platform.UI.Controls;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.Client;

public abstract class SidePanelPlugin : PropertyClass
{
	private bool _generateUserControlCalledInternally;

	public abstract Guid Id { get; }

	public abstract string Name { get; }

	[Obsolete("This property is deprecated. Windows Forms controls are no longer supported. Use IconSource instead.", false)]
	public virtual Image Icon
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public virtual VideoOSIconSourceBase IconSource
	{
		get
		{
			return Icon.ToIconBitmapSource();
		}
		set
		{
		}
	}

	[Obsolete("This is a deprecated property. Use SidePanelPlaceDifinitions instead")]
	public virtual SidePanelPluginPlace SidePanelPluginPlace => SidePanelPluginPlace.Undefined;

	public virtual List<SidePanelPlaceDefinition> SidePanelPlaceDefinitions => new List<SidePanelPlaceDefinition>();

	protected SidePanelPlugin()
	{
		new TrackPageViewData(typeof(SidePanelPlugin)).Send();
	}

	[Obsolete("This method is deprecated. Windows Forms controls are no longer supported. Use GenerateWpfUserControl instead.", false)]
	public virtual SidePanelUserControl GenerateUserControl()
	{
		if (_generateUserControlCalledInternally)
		{
			return null;
		}
		_generateUserControlCalledInternally = true;
		SidePanelWpfUserControl sidePanelWpfUserControl = GenerateWpfUserControl();
		_generateUserControlCalledInternally = false;
		if (sidePanelWpfUserControl == null)
		{
			EnvironmentManager.Instance.Log(error: true, "SidePanelPlugin", "GenerateUserControl() called on SidePanelPlugin where both GenerateUserControl and GenerateWpfUserControl have not been overridden");
			return null;
		}
		return new SidePanelWpfUserControlWinFormsWrapper
		{
			WpfControl = sidePanelWpfUserControl
		};
	}

	public virtual SidePanelWpfUserControl GenerateWpfUserControl()
	{
		if (_generateUserControlCalledInternally)
		{
			return null;
		}
		_generateUserControlCalledInternally = true;
		SidePanelUserControl sidePanelUserControl = GenerateUserControl();
		_generateUserControlCalledInternally = false;
		if (sidePanelUserControl == null)
		{
			EnvironmentManager.Instance.Log(error: true, "SidePanelPlugin", "GenerateWpfUserControl() called on SidePanelPlugin where both GenerateUserControl and GenerateWpfUserControl have not been overridden");
			return null;
		}
		ClientControl.Instance.RegisterUIControlForAutoTheming(sidePanelUserControl);
		return new SidePanelUserControlWpfWrapper
		{
			WinFormsControl = sidePanelUserControl
		};
	}

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

	public bool SaveProperties(bool userPrivate)
	{
		Configuration.Instance.SaveOptionsConfiguration(Id, userPrivate, SerializeToXmlNode());
		return true;
	}
}
