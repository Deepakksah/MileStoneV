using System;
using System.Windows.Media.Imaging;
using VideoOS.Platform.Admin;
using VideoOS.Platform.UI.Controls;
using VideoOS.Platform.UI.Extensions;
using VideoOS.Platform.Util;
using VideoOS.Platform.Util.Images;

namespace VideoOS.Platform.UI.ItemPicker.Utilities;

public class IconSource
{
	public VideoOSIconSourceBase GetIconSource(Item item)
	{
		if (item == null)
		{
			return null;
		}
		VideoOSIconBuiltInSource videoOSIconBuiltInSource = new VideoOSIconBuiltInSource();
		if (item.FQID.Kind == Kind.Server)
		{
			if (item.FQID.ServerId.ServerType == "XPCORS")
			{
				videoOSIconBuiltInSource.Icon = VideoOSIconBuiltInSource.Icons.Recording_Server;
				return videoOSIconBuiltInSource;
			}
			videoOSIconBuiltInSource.Icon = VideoOSIconBuiltInSource.Icons.Server;
			return videoOSIconBuiltInSource;
		}
		if (item.FQID.FolderType != FolderType.No)
		{
			videoOSIconBuiltInSource.Icon = VideoOSIconBuiltInSource.Icons.Folder;
			return videoOSIconBuiltInSource;
		}
		if (item.FQID.Kind == Kind.Camera)
		{
			if (item.Properties.ContainsKey("PTZ") && item.Properties["PTZ"] == "Yes")
			{
				videoOSIconBuiltInSource.Icon = VideoOSIconBuiltInSource.Icons.Camera_PTZ;
				return videoOSIconBuiltInSource;
			}
			videoOSIconBuiltInSource.Icon = VideoOSIconBuiltInSource.Icons.Camera;
			return videoOSIconBuiltInSource;
		}
		if (item.FQID.Kind == Kind.Bookmark)
		{
			videoOSIconBuiltInSource.Icon = VideoOSIconBuiltInSource.Icons.Bookmark;
			return videoOSIconBuiltInSource;
		}
		if (item.FQID.Kind == Kind.GisMapLocation)
		{
			videoOSIconBuiltInSource.Icon = VideoOSIconBuiltInSource.Icons.Location;
			return videoOSIconBuiltInSource;
		}
		if (item.FQID.Kind == Kind.Hardware)
		{
			videoOSIconBuiltInSource.Icon = VideoOSIconBuiltInSource.Icons.Device;
			return videoOSIconBuiltInSource;
		}
		if (item.FQID.Kind == Kind.InputEvent)
		{
			videoOSIconBuiltInSource.Icon = VideoOSIconBuiltInSource.Icons.Input_Device;
			return videoOSIconBuiltInSource;
		}
		if (item.FQID.Kind == Kind.Layout)
		{
			videoOSIconBuiltInSource.Icon = VideoOSIconBuiltInSource.Icons.View_Layout;
			return videoOSIconBuiltInSource;
		}
		if (item.FQID.Kind == Kind.LayoutGroup)
		{
			videoOSIconBuiltInSource.Icon = VideoOSIconBuiltInSource.Icons.Layout_Group;
			return videoOSIconBuiltInSource;
		}
		if (item.FQID.Kind == Kind.Matrix)
		{
			videoOSIconBuiltInSource.Icon = VideoOSIconBuiltInSource.Icons.Matrix_View_Item;
			return videoOSIconBuiltInSource;
		}
		if (item.FQID.Kind == Kind.Metadata)
		{
			videoOSIconBuiltInSource.Icon = VideoOSIconBuiltInSource.Icons.Metadata;
			return videoOSIconBuiltInSource;
		}
		if (item.FQID.Kind == Kind.Microphone)
		{
			videoOSIconBuiltInSource.Icon = VideoOSIconBuiltInSource.Icons.Microphone;
			return videoOSIconBuiltInSource;
		}
		if (item.FQID.Kind == Kind.Output)
		{
			videoOSIconBuiltInSource.Icon = VideoOSIconBuiltInSource.Icons.Output;
			return videoOSIconBuiltInSource;
		}
		if (item.FQID.Kind == Kind.PlaybackController)
		{
			videoOSIconBuiltInSource.Icon = VideoOSIconBuiltInSource.Icons.Playback_Controller;
			return videoOSIconBuiltInSource;
		}
		if (item.FQID.Kind == Kind.Preset)
		{
			videoOSIconBuiltInSource.Icon = VideoOSIconBuiltInSource.Icons.Preset;
			return videoOSIconBuiltInSource;
		}
		if (item.FQID.Kind == Kind.Role)
		{
			videoOSIconBuiltInSource.Icon = VideoOSIconBuiltInSource.Icons.Role;
			return videoOSIconBuiltInSource;
		}
		if (item.FQID.Kind == Kind.Screen)
		{
			videoOSIconBuiltInSource.Icon = VideoOSIconBuiltInSource.Icons.Screen;
			return videoOSIconBuiltInSource;
		}
		if (item.FQID.Kind == Kind.SmartWall)
		{
			videoOSIconBuiltInSource.Icon = VideoOSIconBuiltInSource.Icons.Smartwall_View_Item;
			return videoOSIconBuiltInSource;
		}
		if (item.FQID.Kind == Kind.SmartWallMonitor)
		{
			videoOSIconBuiltInSource.Icon = VideoOSIconBuiltInSource.Icons.Smartwall_Monitor;
			return videoOSIconBuiltInSource;
		}
		if (item.FQID.Kind == Kind.SmartWallPreset)
		{
			videoOSIconBuiltInSource.Icon = VideoOSIconBuiltInSource.Icons.Smartwall_Preset;
			return videoOSIconBuiltInSource;
		}
		if (item.FQID.Kind == Kind.Speaker)
		{
			videoOSIconBuiltInSource.Icon = VideoOSIconBuiltInSource.Icons.Speaker;
			return videoOSIconBuiltInSource;
		}
		if (item.FQID.Kind == Kind.TimeProfile)
		{
			videoOSIconBuiltInSource.Icon = VideoOSIconBuiltInSource.Icons.Time_Selection_Mode;
			return videoOSIconBuiltInSource;
		}
		if (item.FQID.Kind == Kind.TransactSource)
		{
			videoOSIconBuiltInSource.Icon = VideoOSIconBuiltInSource.Icons.Transact_View_Item;
			return videoOSIconBuiltInSource;
		}
		if (item.FQID.Kind == Kind.TriggerEvent)
		{
			videoOSIconBuiltInSource.Icon = VideoOSIconBuiltInSource.Icons.Event;
			return videoOSIconBuiltInSource;
		}
		if (item.FQID.Kind == Kind.User)
		{
			videoOSIconBuiltInSource.Icon = VideoOSIconBuiltInSource.Icons.User;
			return videoOSIconBuiltInSource;
		}
		if (item.FQID.Kind == Kind.VideoWall)
		{
			videoOSIconBuiltInSource.Icon = VideoOSIconBuiltInSource.Icons.Smartwall_View_Item;
			return videoOSIconBuiltInSource;
		}
		if (item.FQID.Kind == Kind.VideoWallMonitor)
		{
			videoOSIconBuiltInSource.Icon = VideoOSIconBuiltInSource.Icons.Smartwall_Monitor;
			return videoOSIconBuiltInSource;
		}
		if (item.FQID.Kind == Kind.VideoWallPreset)
		{
			videoOSIconBuiltInSource.Icon = VideoOSIconBuiltInSource.Icons.Smartwall_Preset;
			return videoOSIconBuiltInSource;
		}
		if (item.FQID.Kind == Kind.View)
		{
			videoOSIconBuiltInSource.Icon = VideoOSIconBuiltInSource.Icons.View;
			return videoOSIconBuiltInSource;
		}
		if (item.FQID.Kind == Kind.ViewItemInstance)
		{
			videoOSIconBuiltInSource.Icon = VideoOSIconBuiltInSource.Icons.View;
			return videoOSIconBuiltInSource;
		}
		if (item.FQID.Kind == Kind.Window)
		{
			videoOSIconBuiltInSource.Icon = VideoOSIconBuiltInSource.Icons.Window;
			return videoOSIconBuiltInSource;
		}
		if (item.FQID.Kind == Kind.WorkSpace)
		{
			videoOSIconBuiltInSource.Icon = VideoOSIconBuiltInSource.Icons.Workspace;
			return videoOSIconBuiltInSource;
		}
		VideoOSIconBitmapSource videoOSIconBitmapSource = item.IconWithOverlay().ToIconBitmapSource();
		if (videoOSIconBitmapSource == null)
		{
			int index = Util.BuiltInItemToImageIndex(item);
			BitmapSource bitmapSource = ImageConverter.ConvertImage(Util.ImageList.Images[index]);
			if (bitmapSource != null)
			{
				videoOSIconBitmapSource = new VideoOSIconBitmapSource();
				videoOSIconBitmapSource.BitmapSource = bitmapSource;
			}
		}
		return videoOSIconBitmapSource;
	}

	public bool IsKindOrParent(Item item, Guid kind)
	{
		if (item == null)
		{
			return false;
		}
		if (item.FQID.Kind == kind)
		{
			return true;
		}
		ItemNode itemNode = PluginManager.GetItemNode(kind);
		while (itemNode != null && itemNode.ParentKind != Guid.Empty)
		{
			itemNode = PluginManager.GetItemNode(itemNode.ParentKind);
			if (itemNode != null && itemNode.ItemKind == kind)
			{
				return true;
			}
		}
		return false;
	}
}
