using System;
using System.Collections.Generic;
using System.Drawing;
using VideoOS.Platform.Background;
using VideoOS.Platform.UI;

namespace VideoOS.Platform.Util;

public class ServiceHelperPlugin : PluginDefinition
{
	private Guid _id = new Guid("A1C400F7-C61E-46FF-B8A9-F268EE3DD20C");

	private List<BackgroundPlugin> _list = new List<BackgroundPlugin>();

	public override Guid Id => _id;

	public override string Name => "ServiceHelper";

	public override Image Icon => VideoOS.Platform.UI.Util.ImageList.Images[VideoOS.Platform.UI.Util.SDK_ToolIx];

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

	public override List<BackgroundPlugin> BackgroundPlugins
	{
		get
		{
			if (_list.Count == 0)
			{
				_list.Add(new ServiceHelperBackground());
			}
			return _list;
		}
	}

	public override string Manufacturer => "Internal MIP Component";

	public override string VersionString => "1.0.0.0";

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

	public override void Init()
	{
	}

	public override void Close()
	{
		ServiceUtil.Close();
	}
}
