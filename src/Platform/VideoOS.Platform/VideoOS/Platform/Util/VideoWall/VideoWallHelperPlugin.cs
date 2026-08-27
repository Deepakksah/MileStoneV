using System;
using System.Collections.Generic;
using System.Drawing;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Background;
using VideoOS.Platform.Properties;
using VideoOS.Platform.UI;

namespace VideoOS.Platform.Util.VideoWall;

public class VideoWallHelperPlugin : PluginDefinition
{
	private Guid _id = new Guid("7DA29D2F-04EF-4913-B886-FE148BC1A99C");

	private Guid _backgrounPluginId = new Guid("0314FC59-3027-4F9A-BE46-38F22CBBBCB3");

	private static Image _image;

	private List<ItemNode> _list = new List<ItemNode>();

	private List<BackgroundPlugin> _backgroundPlugins = new List<BackgroundPlugin>();

	private VideoWallMessageHandler _messageHandler;

	public override Guid Id => _id;

	public override string Name => "VideoWallHelper";

	public override Image Icon => VideoOS.Platform.UI.Util.ImageList.Images[VideoOS.Platform.UI.Util.ViewGroupIx];

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

	public VideoWallHelperPlugin()
	{
	}

	public override void Init()
	{
		_messageHandler = new VideoWallMessageHandler();
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
		if (_messageHandler != null)
		{
			_messageHandler.Close();
		}
		_messageHandler = null;
		_list.Clear();
	}

	private void InitializeItemNodes()
	{
		if (EnvironmentManager.Instance != null)
		{
			_list.Clear();
			_image = VideoOS.Platform.Properties.Resources.ViewItem.ToBitmap();
			_list.Add(new ItemNode(Kind.LayoutGroup, Guid.Empty, "Layout Group", _image, "Layout Groups", _image, Category.Layout, selectable: true, ItemsAllowed.Many, new LayoutGroupItemManager(), new List<ItemNode>
			{
				new ItemNode(Kind.Layout, Kind.LayoutGroup, "Layout", _image, "Layouts", _image, Category.Layout, selectable: true, ItemsAllowed.Many, new LayoutItemManager(), null)
				{
					PlacementHint = PlacementHint.Hidden,
					MapIconDictionary = null
				}
			})
			{
				PlacementHint = PlacementHint.Hidden,
				MapIconDictionary = null
			});
			if (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.Service || EnvironmentManager.Instance.LicenseManager.Features.Contains("SmartWall"))
			{
				Image image = VideoOS.Platform.Properties.Resources.Video_Wall_16_n_i8.ToBitmap();
				Image single_View_16_h_p = VideoOS.Platform.Properties.Resources.Single_View_16_h_p;
				Image smart_Wall_Preset = VideoOS.Platform.Properties.Resources.Smart_Wall_Preset;
				_list.Add(new ItemNode(Kind.VideoWall, Guid.Empty, "Video Wall", image, "Video Walls", image, Category.Layout, selectable: true, ItemsAllowed.Many, new VideoWallItemManager(), new List<ItemNode>
				{
					new ItemNode(Kind.VideoWallMonitor, Kind.VideoWall, "Monitor", single_View_16_h_p, "Monitors", single_View_16_h_p, Category.Layout, selectable: true, ItemsAllowed.Many, new MonitorItemManager(), null)
					{
						PlacementHint = PlacementHint.Hidden,
						MapIconDictionary = null
					},
					new ItemNode(Kind.VideoWallPreset, Kind.VideoWall, "MonitorPreset", smart_Wall_Preset, "MonitorPresets", smart_Wall_Preset, Category.Layout, selectable: true, ItemsAllowed.Many, new MonitorPresetItemManager(), null)
					{
						PlacementHint = PlacementHint.Hidden,
						MapIconDictionary = null
					}
				})
				{
					PlacementHint = PlacementHint.Hidden,
					MapIconDictionary = null
				});
			}
		}
	}
}
