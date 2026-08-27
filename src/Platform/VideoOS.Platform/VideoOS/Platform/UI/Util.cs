using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Client;
using VideoOS.Platform.Properties;
using VideoOS.Platform.Resources;

namespace VideoOS.Platform.UI;

public static class Util
{
	private static ImageList _imageList = new ImageList();

	private static ImageList _imageList32 = new ImageList();

	private static ImageList _imageListBlack = new ImageList();

	private static Dictionary<Guid, int> _itemTypeToIx = new Dictionary<Guid, int>();

	private static Dictionary<Guid, Category> _itemTypeToCategory = new Dictionary<Guid, Category>();

	private static readonly HashSet<Guid> _buildInPluginKinds = new HashSet<Guid>
	{
		Kind.TransactSource,
		Kind.VideoWall,
		Kind.VideoWallMonitor,
		Kind.VideoWallPreset,
		Kind.GisMapLocation
	};

	private static object _initLock = new object();

	private static bool _initialized = false;

	public static Bitmap DisabledOverlay = null;

	public static Bitmap ActiveOverlay = null;

	public static Bitmap AttentionOverlay = null;

	public static int ServerIconIx = 0;

	public static int FolderIconIx = 1;

	public static int AlertIconIx = 2;

	public static int InputIconIx = 3;

	public static int OutputIconIx = 4;

	public static int MatrixIconIx = 5;

	public static int TransactIconIx = 6;

	public static int PresetIconIx = 7;

	public static int CameraIconIx = 8;

	public static int CustomEventIconIx = 9;

	public static int SpeakerIconIx = 10;

	public static int MicrophoneIconIx = 11;

	public static int RecorderIconIx = 12;

	public static int ViewGroupIx = 13;

	public static int GroupIx = 14;

	public static int VideoWallGroupIx = 15;

	public static int VideoWallIx = 16;

	public static int ViewIx = 17;

	public static int ViewItemIx = 18;

	public static int PluginIx = 19;

	public static int SDK_VSIx = 20;

	public static int SDK_GeneralIx = 21;

	public static int SDK_ToolIx = 22;

	public static int WindowIx = 23;

	public static int RoleIx = 24;

	public static int UserIx = 25;

	public static int WorkSpaceIx = 26;

	public static int TimeProfileIx = 27;

	public static int HardwareIx = 28;

	public static int InterConnectedIx = 29;

	public static int CameraPtzIx = 30;

	public static int MetadataIx = 31;

	public static int VideoWallPreset = 32;

	public static int GisMapLocationIx = 33;

	public static int FirstFreeIx = 33;

	private const int MaxBuiltInIx = 33;

	public static ImageList ImageList
	{
		get
		{
			if (!_initialized)
			{
				Init();
			}
			if (EnvironmentManager.Instance != null && EnvironmentManager.Instance.EnvironmentType == EnvironmentType.SmartClient && ClientControl.Instance != null && ClientControl.Instance.Theme.ThemeType == ThemeType.Dark)
			{
				return _imageListBlack;
			}
			return _imageList;
		}
	}

	public static ImageList ImageList32
	{
		get
		{
			if (!_initialized)
			{
				Init();
			}
			return _imageList32;
		}
	}

	public static ImageList ImageListClone
	{
		get
		{
			ImageList imageList = new ImageList();
			ImageList imageList2 = ImageList;
			if (EnvironmentManager.Instance != null && EnvironmentManager.Instance.EnvironmentType == EnvironmentType.SmartClient && ClientControl.Instance != null && ClientControl.Instance.Theme.ThemeType == ThemeType.Dark)
			{
				imageList2 = _imageListBlack;
			}
			for (int i = 0; i < imageList2.Images.Count; i++)
			{
				imageList.Images.Add((Bitmap)imageList2.Images[i].Clone());
			}
			return imageList;
		}
	}

	public static Dictionary<Guid, int> KindToImageIndex
	{
		get
		{
			if (!_initialized)
			{
				Init();
			}
			return _itemTypeToIx;
		}
	}

	public static Dictionary<Guid, Category> KindToCategory
	{
		get
		{
			if (!_initialized)
			{
				Init();
			}
			return _itemTypeToCategory;
		}
	}

	private static void Init()
	{
		try
		{
			lock (_initLock)
			{
				if (_initialized)
				{
					return;
				}
				_imageList = new ImageList();
				_imageListBlack = new ImageList();
				_imageListBlack.ImageSize = new Size(16, 16);
				Assembly executingAssembly = Assembly.GetExecutingAssembly();
				string name = executingAssembly.GetName().Name;
				DisabledOverlay = Images.Disabled16ni8.ToBitmap();
				AttentionOverlay = Images.Attention16ni8.ToBitmap();
				ActiveOverlay = Images.Running16ni8.ToBitmap();
				_imageList.ImageSize = new Size(16, 16);
				_imageList.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.Server.png")));
				_imageList.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.folder_closed.ico")));
				_imageList.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.alert.ico")));
				_imageList.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.input.ico")));
				_imageList.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.output.ico")));
				_imageList.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.matrix.ico")));
				_imageList.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.TransactSource.ico")));
				_imageList.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.Presets.ico")));
				_imageList.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.camera.ico")));
				_imageList.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.event.ico")));
				_imageList.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.speaker.ico")));
				_imageList.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.microphone.ico")));
				_imageList.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.Recorder.ico")));
				_imageList.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.View group closed 16 n p.png")));
				_imageList.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.Folder Closed Flat 16 n p.png")));
				_imageList.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.Clear Video Wall View.png")));
				_imageList.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.Video Wall 16 n i8.ico")));
				_imageList.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.Single View 16 h p.png")));
				_imageList.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.ViewItem.ico")));
				_imageList.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.PlugIn.bmp")));
				_imageList.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.SDK_VS.ico")));
				_imageList.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.SDK_general.ico")));
				_imageList.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.SDK_tool.ico")));
				_imageList.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.Window 16 h p.png")));
				_imageList.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.Role.ico")));
				_imageList.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.User.ico")));
				_imageList.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.PlugIn.bmp")));
				_imageList.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.TimeProfile.ico")));
				_imageList.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.Hardware16ni8.ico")));
				_imageList.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.Interconnected_hardware_16x16.ico")));
				_imageList.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.CameraPTZ_icon.png")));
				_imageList.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.Metadata_device.ico")));
				_imageList.Images.Add(new Bitmap(VideoOS.Platform.Properties.Resources.Smart_Wall_Preset));
				_imageList.Images.Add(new Bitmap(VideoOS.Platform.Properties.Resources.GisMapLocation));
				_imageList32 = new ImageList();
				_imageList32.ImageSize = new Size(32, 32);
				_imageList32.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.Server.png")));
				_imageList32.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.folder_closed.ico")));
				_imageList32.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.alert.ico")));
				_imageList32.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.input.ico")));
				_imageList32.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.output.ico")));
				_imageList32.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.matrix.ico")));
				_imageList32.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.TransactSource.ico")));
				_imageList32.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.Presets.ico")));
				_imageList32.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.camera.ico")));
				_imageList32.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.event.ico")));
				_imageList32.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.speaker.ico")));
				_imageList32.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.microphone.ico")));
				_imageList32.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.Recorder.ico")));
				_imageList32.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.View group closed 16 n p.png")));
				_imageList32.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.Folder Closed Flat 16 n p.png")));
				_imageList32.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.Clear Video Wall View.png")));
				_imageList32.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.Video Wall 16 n i8.ico")));
				_imageList32.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.Single View 16 h p.png")));
				_imageList32.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.ViewItem.ico")));
				_imageList32.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.PlugIn.bmp")));
				_imageList32.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.SDK_VS.ico")));
				_imageList32.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.SDK_general.ico")));
				_imageList32.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.SDK_tool.ico")));
				_imageList32.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.Window 16 h p.png")));
				_imageList32.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.Role.ico")));
				_imageList32.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.User.ico")));
				_imageList32.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.PlugIn.bmp")));
				_imageList32.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.TimeProfile.ico")));
				_imageList32.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.Hardware32ni8.ico")));
				_imageList32.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.Interconnected_hardware_16x16.ico")));
				_imageList32.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.CameraPTZ_icon.png")));
				_imageList32.Images.Add(new Bitmap(executingAssembly.GetManifestResourceStream(name + ".Resources.Metadata_device.ico")));
				_imageList32.Images.Add(new Bitmap(VideoOS.Platform.Properties.Resources.Smart_Wall_Preset));
				_imageList32.Images.Add(new Bitmap(VideoOS.Platform.Properties.Resources.GisMapLocation));
				_itemTypeToIx[Kind.Server] = ServerIconIx;
				_itemTypeToIx[Kind.Folder] = FolderIconIx;
				_itemTypeToIx[Kind.InputEvent] = InputIconIx;
				_itemTypeToIx[Kind.Output] = OutputIconIx;
				_itemTypeToIx[Kind.Matrix] = MatrixIconIx;
				_itemTypeToIx[Kind.TransactSource] = TransactIconIx;
				_itemTypeToIx[Kind.Preset] = PresetIconIx;
				_itemTypeToIx[Kind.Camera] = CameraIconIx;
				_itemTypeToIx[Kind.TriggerEvent] = CustomEventIconIx;
				_itemTypeToIx[Kind.Speaker] = SpeakerIconIx;
				_itemTypeToIx[Kind.Microphone] = MicrophoneIconIx;
				_itemTypeToIx[Kind.View] = ViewGroupIx;
				_itemTypeToIx[Kind.Window] = WindowIx;
				_itemTypeToIx[Kind.Screen] = ViewIx;
				_itemTypeToIx[Kind.ViewItemInstance] = ViewItemIx;
				_itemTypeToIx[Kind.VideoWall] = VideoWallIx;
				_itemTypeToIx[Kind.VideoWallPreset] = VideoWallPreset;
				_itemTypeToIx[Kind.Role] = RoleIx;
				_itemTypeToIx[Kind.User] = UserIx;
				_itemTypeToIx[Kind.WorkSpace] = WorkSpaceIx;
				_itemTypeToIx[Kind.TimeProfile] = TimeProfileIx;
				_itemTypeToIx[Kind.Metadata] = MetadataIx;
				_itemTypeToIx[Kind.Hardware] = HardwareIx;
				_itemTypeToIx[Kind.LayoutGroup] = ViewGroupIx;
				_itemTypeToIx[Kind.Layout] = ViewGroupIx;
				_itemTypeToIx[Kind.VideoWallMonitor] = ViewIx;
				_itemTypeToIx[Kind.GisMapLocation] = GisMapLocationIx;
				foreach (Guid key in Kind.DefaultTypeToCategoryTable.Keys)
				{
					_itemTypeToCategory[key] = (Category)Kind.DefaultTypeToCategoryTable[key];
				}
				int num = FirstFreeIx;
				if (EnvironmentManager.Instance != null && !EnvironmentManager.Instance.PluginInitializationInProgress)
				{
					foreach (ItemNode item in FindItemNodes())
					{
						if (item.Icon != null)
						{
							num++;
							_imageList.Images.Add(item.Icon);
							_imageList32.Images.Add(item.Icon);
							_itemTypeToIx[item.ItemKind] = num;
						}
						_itemTypeToCategory[item.ItemKind] = item.Category;
					}
					if (EnvironmentManager.Instance != null && EnvironmentManager.Instance.EnvironmentType == EnvironmentType.SmartClient)
					{
						for (int i = 0; i < _imageList.Images.Count; i++)
						{
							Bitmap bitmapWhite = new Bitmap(_imageList.Images[i] as Bitmap, 16, 16);
							_imageListBlack.Images.Add(MakeBlackImage(bitmapWhite));
						}
					}
				}
				if (EnvironmentManager.Instance != null && EnvironmentManager.Instance.ApplicationLoggedOn)
				{
					_initialized = true;
				}
			}
		}
		catch (Exception exception)
		{
			if (EnvironmentManager.Instance != null)
			{
				EnvironmentManager.Instance.ExceptionHandler("VideoOS.Platform.UI.Util:Init", exception);
			}
		}
	}

	public static bool IsBuiltInKind(Guid kind)
	{
		if (!_initialized)
		{
			Init();
		}
		if (_itemTypeToIx.ContainsKey(kind) && !_buildInPluginKinds.Contains(kind))
		{
			return _itemTypeToIx[kind] <= 33;
		}
		return false;
	}

	private static List<ItemNode> FindItemNodes()
	{
		List<ItemNode> list = new List<ItemNode>();
		if (EnvironmentManager.Instance != null && EnvironmentManager.Instance.AllPluginDefinitions != null)
		{
			foreach (PluginDefinition allPluginDefinition in EnvironmentManager.Instance.AllPluginDefinitions)
			{
				if (allPluginDefinition.ItemNodes != null)
				{
					list.AddRange(FindItemNodes(allPluginDefinition.ItemNodes));
				}
			}
		}
		return list;
	}

	private static List<ItemNode> FindItemNodes(List<ItemNode> itemNodes)
	{
		List<ItemNode> list = new List<ItemNode>();
		if (itemNodes != null)
		{
			foreach (ItemNode itemNode in itemNodes)
			{
				list.Add(itemNode);
				if (itemNode.ItemNodes != null)
				{
					list.AddRange(FindItemNodes(itemNode.ItemNodes));
				}
			}
		}
		return list;
	}

	public static void ReInit()
	{
		_initialized = false;
		Init();
	}

	public static Image BuiltInItemToImage(Item item)
	{
		if (!_initialized)
		{
			Init();
		}
		return ImageList.Images[BuiltInItemToImageIndex(item)];
	}

	public static int BuiltInItemToImageIndex(Item item)
	{
		if (!_initialized)
		{
			Init();
		}
		if (KindToImageIndex.ContainsKey(item.FQID.Kind))
		{
			int num = KindToImageIndex[item.FQID.Kind];
			if (num == ServerIconIx && item.FQID.ServerId.ServerType == "XPCORS")
			{
				num = RecorderIconIx;
			}
			if (num == CameraIconIx && item.Properties.ContainsKey("PTZ") && item.Properties["PTZ"] == "Yes")
			{
				num = CameraPtzIx;
			}
			if (num != ServerIconIx && num != RecorderIconIx && item.FQID.FolderType != FolderType.No)
			{
				return FolderIconIx;
			}
			if (num >= 0 && num < ImageList.Images.Count)
			{
				return num;
			}
		}
		return 0;
	}

	private static Bitmap MakeBlackImage(Bitmap bitmapWhite)
	{
		Bitmap bitmap = new Bitmap(bitmapWhite);
		for (int i = 0; i < 16; i++)
		{
			for (int j = 0; j < 16; j++)
			{
				Color pixel = bitmapWhite.GetPixel(i, j);
				if (pixel.ToArgb() == -1)
				{
					bitmap.SetPixel(i, j, Color.FromArgb(16777215));
				}
				else
				{
					if (pixel.R == 0 && pixel.G == 0 && pixel.B == 0)
					{
						continue;
					}
					if (j < 15)
					{
						Color pixel2 = bitmapWhite.GetPixel(i, j + 1);
						if (pixel2.R < pixel.R && pixel2.G < pixel.G && pixel2.B < pixel.B)
						{
							pixel2 = Color.FromArgb(16777215);
							bitmap.SetPixel(i, j, pixel2);
						}
					}
					break;
				}
			}
			for (int num = 15; num > 0; num--)
			{
				Color pixel3 = bitmapWhite.GetPixel(i, num);
				if (pixel3.ToArgb() == -1)
				{
					bitmap.SetPixel(i, num, Color.FromArgb(16777215));
				}
				else if (pixel3.R != 0 || pixel3.G != 0 || pixel3.B != 0)
				{
					if (num > 0)
					{
						Color pixel4 = bitmapWhite.GetPixel(i, num - 1);
						if (pixel4.R < pixel3.R && pixel4.G < pixel3.G && pixel4.B < pixel3.B)
						{
							pixel4 = Color.FromArgb(16777215);
							bitmap.SetPixel(i, num, pixel4);
						}
					}
					break;
				}
			}
		}
		return bitmap;
	}
}
