using System;
using System.Collections.Generic;
using System.Drawing;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Background;
using VideoOS.Platform.Properties;

namespace VideoOS.Platform.Util.GisMap;

public class GisMapLocationHelperPlugin : PluginDefinition
{
	private Guid _id = new Guid("5E0119E3-37A6-413B-907C-08DF9C84525E");

	private Guid _backgrounPluginId = new Guid("303186C9-CEE9-4826-B8C7-F35581E23F1E");

	private static Image _image;

	private List<ItemNode> _list = new List<ItemNode>();

	private List<BackgroundPlugin> _backgroundPlugins = new List<BackgroundPlugin>();

	public override Guid Id => _id;

	public override string Name => "GisMapLocationHelper";

	public override Image Icon => _image;

	public override bool ExcludeFromAboutBox
	{
		get
		{
			return true;
		}
		set
		{
		}
	}

	public override List<ItemNode> ItemNodes => _list;

	public override List<BackgroundPlugin> BackgroundPlugins => _backgroundPlugins;

	public override AdminPlacementHint AdminPlacementHint
	{
		get
		{
			return AdminPlacementHint.Hidden;
		}
		set
		{
		}
	}

	public override string Manufacturer => "Internal MIP Component";

	public override string VersionString => "1.0.0.0";

	public GisMapLocationHelperPlugin()
	{
		_image = VideoOS.Platform.Properties.Resources.GisMapLocation;
	}

	public override void Init()
	{
		if (EnvironmentManager.Instance != null)
		{
			if (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.Service)
			{
				_backgroundPlugins.Add(new EventServerDummyBackgroundPlugin(_backgrounPluginId, Name));
			}
			InitializeItemNodes();
		}
	}

	public override void Close()
	{
		_list.Clear();
	}

	private void InitializeItemNodes()
	{
		_list.Clear();
		if (EnvironmentManager.Instance != null && (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.Service || EnvironmentManager.Instance.LicenseManager.Features.Contains("SmartMap")))
		{
			_list.Add(new ItemNode(Kind.GisMapLocation, Guid.Empty, "GIS map location", _image, "GIS map locations", _image, Category.Unknown, selectable: true, ItemsAllowed.Many, new GisMapLocationItemManager(), null)
			{
				PlacementHint = PlacementHint.Hidden,
				MapIconDictionary = null
			});
		}
	}
}
