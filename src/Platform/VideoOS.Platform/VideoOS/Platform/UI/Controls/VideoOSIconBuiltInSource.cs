using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSIconBuiltInSource : VideoOSIconSourceBase
{
	public enum Appearances
	{
		Default,
		Error,
		Warning,
		Confirmation,
		Neutral
	}

	public enum Icons
	{
		None = 1,
		Blank = 2,
		Error_Combined = 100,
		Warning_Combined = 101,
		Information_Combined = 102,
		Confirmation_Combined = 103,
		Server_State_Confirmation_Combined = 104,
		Server_State_Warning_Combined = 105,
		Server_State_Error_Combined = 106,
		User_State_Confirmation_Combined = 107,
		User_State_Warning_Combined = 108,
		User_State_Error_Combined = 109,
		Chevron_Double_Down_With_Badge_Combined = 110,
		Arrow_Down = 1000,
		Arrow_Left = 1001,
		Arrow_Right = 1002,
		Arrow_Up = 1003,
		Chevron_Down = 1004,
		Chevron_Left = 1005,
		Chevron_Right = 1006,
		Chevron_Up = 1007,
		Chevron_Double_Left = 1008,
		Chevron_Double_Right = 1009,
		Chevron_Double_Up = 1010,
		Chevron_Double_Down = 1011,
		Arrow_Short_Down = 1012,
		Arrow_Short_Left = 1013,
		Arrow_Short_Right = 1014,
		Arrow_Short_Up = 1015,
		Checkmark = 1016,
		Close = 1017,
		Search_Clear_Field = 1018,
		Selected = 1019,
		Refresh = 1020,
		Replay = 1021,
		Paste = 1022,
		Edit = 1023,
		Rename = 1024,
		Copy = 1025,
		Delete = 1026,
		Comment = 1027,
		Comment_Add = 1028,
		Comment_Edit = 1029,
		PDF_Create = 1030,
		Image_Copy = 1031,
		Image = 1032,
		Image_Missing = 1033,
		Ellipsis_Large = 1034,
		Ellipsis = 1035,
		Save = 1036,
		Save_As = 1037,
		Export = 1038,
		Export_Add_To_List = 1039,
		Print = 1040,
		Clock = 1041,
		Clipboard = 1042,
		Color_Fill = 1043,
		Colorpicker = 1044,
		Calendar = 1045,
		Link = 1046,
		Link_Add = 1047,
		Time_Date_Select = 1048,
		Questionmark = 1049,
		User = 1050,
		Status_Connected = 1051,
		Status_Disconnected = 1052,
		Notification = 1053,
		Logout = 1054,
		Key = 1055,
		Lock_Off = 1056,
		Home = 1057,
		Help = 1058,
		Settings = 1059,
		Globe = 1060,
		Language = 1061,
		Plus = 1062,
		Minus = 1063,
		User_State_Base = 1064,
		Face_Recognition = 1065,
		Role = 1066,
		Filter_Selection = 1067,
		Filter = 1068,
		Suppress = 1069,
		Level_Up = 1070,
		Star = 1071,
		Share = 1072,
		Theme = 1073,
		Server_Jobs = 1074,
		Unknown = 1075,
		Toast_Message = 1076,
		Fullscreen_Enter = 1077,
		Fullscreen_Exit = 1078,
		Maximize = 1079,
		Minimize = 1080,
		Restore = 1081,
		Window = 1082,
		Workspace = 1083,
		Previous = 1084,
		Next = 1085,
		Drag_Grippers = 1086,
		Drag_Out = 1087,
		Drag_Handle = 1088,
		Pin = 1089,
		Unpin = 1090,
		Keyword = 1091,
		Lock_Overlay = 1092,
		Selection_Cursor = 1093,
		Selection_Hand = 1094,
		Zoom_In = 1095,
		Zoom_Out = 1096,
		Zoom_In_Slider = 1097,
		Zoom_Out_Slider = 1098,
		Search = 1099,
		Recording_Search = 1100,
		Status_Disc_Small = 1101,
		Status_Error_Exclamation_Mark_Color = 1102,
		Status_Information_Mark_Color = 1103,
		Status_Confirmation_Check_Mark_Color = 1104,
		Status_Warning_Exclamation_Mark_Color = 1105,
		Status_Round_Disc = 1106,
		Status_Error_Exclamation_Mark = 1107,
		Status_Information_Mark = 1108,
		Status_Confirmation_Check_Mark = 1109,
		Status_Triangle_Warning = 1110,
		Status_Warning_Exclamation_Mark = 1111,
		Caps_Lock = 1112,
		Text = 1113,
		Text_Bold = 1114,
		Text_Italic = 1115,
		Text_Underline = 1116,
		Text_Color = 1117,
		Text_Size = 1118,
		Text_Size_Larger = 1119,
		Text_Size_Smaller = 1120,
		Text_Align_Left = 1121,
		Text_Align_Center = 1122,
		Text_Align_Right = 1123,
		Text_Size_Large = 1124,
		Text_Size_Medium = 1125,
		Text_Size_Small = 1126,
		Folder = 1127,
		Folder_Add = 1128,
		Folder_Locked = 1129,
		Folder_Private = 1130,
		Folder_Read_Only = 1131,
		Actions = 1132,
		Mask_Hide = 1133,
		Mask_Show = 1134,
		Microphone = 1135,
		Microphone_Muted = 1136,
		Microphone_Unmuted = 1137,
		Microphone_Add = 1138,
		Event = 1139,
		PTZ_Control = 1140,
		Create_Floating_Window = 1141,
		Image_Snapshot = 1142,
		Evidence_Lock_Add = 1143,
		Selection_Add = 1144,
		Selection_Remove = 1145,
		Output = 1146,
		Output_Active = 1147,
		Output_Off = 1148,
		Output_Add = 1149,
		Independent_Playback = 1150,
		Sync_To_Playback = 1151,
		Database = 1152,
		Database_Add = 1153,
		Database_End = 1154,
		Project = 1155,
		Project_Add = 1156,
		View = 1157,
		Recording_Server = 1158,
		Server_Service = 1159,
		Screen = 1160,
		Layout_Group = 1161,
		Preset = 1162,
		Playback_Controller = 1163,
		Send_To_Window = 1164,
		Speaker_Active = 1165,
		Speaker_Muted = 1166,
		Speaker = 1167,
		Speaker_Add = 1168,
		Start_Recording = 1169,
		Camera = 1170,
		Camera_Add = 1171,
		Camera_Multiple = 1172,
		Camera_Marquee = 1173,
		Camera_Wall_Mounted = 1174,
		Camera_PTZ = 1175,
		CD_Drive = 1176,
		Object_Cube = 1177,
		Devices_Add = 1178,
		Server = 1179,
		Server_Site = 1180,
		Server_Add = 1181,
		Server_State_Base = 1182,
		State_Indicator = 1183,
		Input_Device = 1184,
		Input_Add = 1185,
		Metadata = 1186,
		Unit = 1187,
		Interconnected_Hardware = 1188,
		Network_Drive = 1189,
		Application = 1190,
		Device = 1191,
		Adaptive_View = 1192,
		View_Add = 1193,
		View_Reload = 1194,
		View_Options = 1195,
		View_Layout = 1196,
		View_Layout_Transact2 = 1197,
		View_Change = 1198,
		View_Duplicate = 1199,
		View_Layout_1_1 = 1200,
		View_Layout_1_1_2 = 1201,
		View_Layout_1_1_4 = 1202,
		View_Layout_1_1_6 = 1203,
		View_Layout_1_1x = 1204,
		View_Layout_1_2P = 1205,
		View_Layout_1_2 = 1206,
		View_Layout_1_2x_P = 1207,
		View_Layout_1_2x = 1208,
		View_Layout_1_3 = 1209,
		View_Layout_1_4_P = 1210,
		View_Layout_1_5 = 1211,
		View_Layout_1_7 = 1212,
		View_Layout_1_8 = 1213,
		View_Layout_1_11 = 1214,
		View_Layout_1_16 = 1215,
		View_Layout_1x1 = 1216,
		View_Layout_1x2 = 1217,
		View_Layout_1x3 = 1218,
		View_Layout_2_1_2 = 1219,
		View_Layout_2_4 = 1220,
		View_Layout_2_8 = 1221,
		View_Layout_2x2 = 1222,
		View_Layout_2x3 = 1223,
		View_Layout_2x4_P = 1224,
		View_Layout_2x6_P = 1225,
		View_Layout_3x2 = 1226,
		View_Layout_3x3 = 1227,
		View_Layout_4x1 = 1228,
		View_Layout_4x3 = 1229,
		View_Layout_4x4 = 1230,
		View_Layout_4x6 = 1231,
		View_Layout_5x5 = 1232,
		View_Layout_6x3 = 1233,
		View_Layout_6x6 = 1234,
		View_Layout_7x7 = 1235,
		View_Layout_8x8 = 1236,
		View_Layout_10x10 = 1237,
		View_Layout_12x6 = 1238,
		Hardware_Acceleration_Tag = 1239,
		Overlay_Buttons = 1240,
		Plugin = 1241,
		Plugin_Add = 1242,
		System_overview = 1243,
		Music_Note = 1244,
		Music_Note_Muted = 1245,
		Smartwall_Monitor = 1246,
		Smartwall_View_Item = 1247,
		Smartwall_Preset = 1248,
		Access_Control_System = 1249,
		Access_Control_View_Item = 1250,
		Alarm = 1251,
		Alarm_Disabled = 1252,
		Alarm_Definition = 1253,
		Alarm_List_View = 1254,
		Alarm_List_View_Item = 1255,
		Alarm_Tooltip_Map = 1256,
		Alarm_Related_Tooltip_Map = 1257,
		Carousel_View_Item = 1258,
		Hotspot_View_Item = 1259,
		Matrix_View_Item = 1260,
		HTML_View_Item = 1261,
		LPR_View_Item = 1262,
		Text_View_Item = 1263,
		Note = 1264,
		Controller_Device = 1265,
		Retrieve_From_Storage = 1266,
		Go_To_Video_Tutorials = 1267,
		Sequence_First = 1268,
		Sequence_Last = 1269,
		Sequence_Previous = 1270,
		Sequence_Next = 1271,
		Sequence_Split = 1272,
		Play_Forward = 1273,
		Play_Backward = 1274,
		Playback_Speed = 1275,
		Pause = 1276,
		View_In_Bookmarks_Tab = 1277,
		Go_To_Selection_End_From_Right_12px = 1282,
		Go_To_Selection_End_From_Left_12px = 1283,
		Go_To_Selection_Start_From_Right_12px = 1284,
		Go_To_Selection_Start_From_Left_12px = 1285,
		Bookmark_Timeline = 1286,
		Selection_Bookmark_End = 1287,
		Selection_Bookmark_Start = 1288,
		Selection_Bookmark_End_12px = 1289,
		Selection_Bookmark_Start_12px = 1290,
		Bookmark_Timeline_12px = 1291,
		Time_Selection_Mode = 1292,
		Time_Selection_Mode_Date_Time = 1293,
		Frame_Previous = 1294,
		Frame_Next = 1295,
		Go_To_Start = 1296,
		Go_To_End = 1297,
		Mobile_Previous_Frame = 1298,
		Mobile_Next_Frame = 1299,
		Building = 1300,
		Building_Add = 1301,
		Layers = 1302,
		Layers_Add = 1303,
		Location = 1304,
		Location_Add = 1305,
		Map_View_Item = 1306,
		Map_Settings = 1307,
		Smart_Map_View_Item = 1308,
		Zone = 1309,
		Map_Recenter = 1310,
		Location_Off = 1311,
		Bookmark = 1312,
		Bookmark_Delete = 1313,
		Bookmark_Edit = 1314,
		Search_Vehicle = 1315,
		Search_People = 1316,
		Search_Motion = 1317,
		Search_Sequence = 1318,
		Search_Private = 1319,
		Search_Location = 1320,
		Search_Temperature = 1321,
		Open_In_Search = 1322,
		Video_Lock = 1323,
		Blur_video = 1324,
		Card_Reader = 1325,
		Keycard = 1326,
		Keypad = 1327,
		Door_Open = 1328,
		Door_Closed = 1329,
		Commands = 1330,
		Zones = 1331,
		Matchlist_Add = 1332,
		Matchlist_Remove = 1333,
		Sources = 1334,
		Receipt = 1335,
		Transact_View_Item = 1336,
		Checkmark_12px = 1337,
		Chevron_Down_12px = 1338,
		Chevron_Left_12px = 1339,
		Chevron_Right_12px = 1340,
		Chevron_Up_12px = 1341,
		Close_12px = 1342,
		Search_Clear_Field_12px = 1343,
		Set_Selection_Start_12px = 1344,
		Set_Selection_End_12px = 1345,
		Chevron_Double_Down_Base = 1346,
		State_Indicator_Top_Right = 1347
	}

	private static Dictionary<string, VideoOSIconSourceBase> _builtInIconSourceDictionary = new Dictionary<string, VideoOSIconSourceBase>();

	private Icons __icon = Icons.Blank;

	private Appearances __appearance;

	private Icons __rtlIcon = Icons.None;

	public Icons Icon
	{
		get
		{
			return __icon;
		}
		set
		{
			if (__icon != value)
			{
				__icon = value;
				OnPropertyChanged("BuiltInIconSourcePropertyChanged");
			}
		}
	}

	public Appearances Appearance
	{
		get
		{
			return __appearance;
		}
		set
		{
			if (__appearance != value)
			{
				__appearance = value;
				OnPropertyChanged("BuiltInIconSourcePropertyChanged");
			}
		}
	}

	public Icons RtlIcon
	{
		get
		{
			return __rtlIcon;
		}
		set
		{
			if (__rtlIcon != value)
			{
				__rtlIcon = value;
				OnPropertyChanged("RtlIcon");
			}
		}
	}

	public VideoOSIconSourceBase ResolveIconSource(FlowDirection flowDirection, DependencyObject self)
	{
		Icons icons = Icon;
		if (flowDirection == FlowDirection.RightToLeft && RtlIcon != Icons.None)
		{
			icons = RtlIcon;
		}
		string arg = (string)self.FindResource("VideoOSResourceKeysInternalThemeName");
		string key = string.Format(CultureInfo.InvariantCulture, "{0}#{1}#{2}", icons, Appearance, arg);
		if (!_builtInIconSourceDictionary.ContainsKey(key))
		{
			VideoOSIconSourceBase value;
			switch (icons)
			{
			case Icons.None:
				return null;
			case Icons.Blank:
				value = new VideoOSIconPathSource();
				break;
			case Icons.Error_Combined:
				value = new VideoOSIconMultiPathSource
				{
					Paths = new List<VideoOSIconMultiPathPathInfo>
					{
						new VideoOSIconMultiPathPathInfo
						{
							AutoColor = false,
							Path = new Path
							{
								Data = ReadGeometryFromSvg(Icons.Status_Round_Disc),
								Fill = (Brush)self.FindResource("VideoOSResourceKeysInternalVideoOSIconBuiltInErrorCombinedBackgroundBrush"),
								StrokeThickness = 0.0
							}
						},
						new VideoOSIconMultiPathPathInfo
						{
							AutoColor = false,
							Path = new Path
							{
								Data = ReadGeometryFromSvg(Icons.Status_Error_Exclamation_Mark),
								Fill = (Brush)self.FindResource("VideoOSResourceKeysInternalVideoOSIconBuiltInErrorCombinedForegroundBrush"),
								StrokeThickness = 0.0
							}
						}
					}
				};
				break;
			case Icons.Warning_Combined:
				value = new VideoOSIconMultiPathSource
				{
					Paths = new List<VideoOSIconMultiPathPathInfo>
					{
						new VideoOSIconMultiPathPathInfo
						{
							AutoColor = false,
							Path = new Path
							{
								Data = ReadGeometryFromSvg(Icons.Status_Triangle_Warning),
								Fill = (Brush)self.FindResource("VideoOSResourceKeysInternalVideoOSIconBuiltInWarningColorBackgroundBrush"),
								StrokeThickness = 0.0
							}
						},
						new VideoOSIconMultiPathPathInfo
						{
							AutoColor = false,
							Path = new Path
							{
								Data = ReadGeometryFromSvg(Icons.Status_Warning_Exclamation_Mark),
								Fill = (Brush)self.FindResource("VideoOSResourceKeysInternalVideoOSIconBuiltInWarningColorForegroundBrush"),
								StrokeThickness = 0.0
							}
						}
					}
				};
				break;
			case Icons.Information_Combined:
				value = new VideoOSIconMultiPathSource
				{
					Paths = new List<VideoOSIconMultiPathPathInfo>
					{
						new VideoOSIconMultiPathPathInfo
						{
							AutoColor = false,
							Path = new Path
							{
								Data = ReadGeometryFromSvg(Icons.Status_Round_Disc),
								Fill = (Brush)self.FindResource("VideoOSResourceKeysInternalVideoOSIconBuiltInInformationCombinedBackgroundBrush"),
								StrokeThickness = 0.0
							}
						},
						new VideoOSIconMultiPathPathInfo
						{
							AutoColor = false,
							Path = new Path
							{
								Data = ReadGeometryFromSvg(Icons.Status_Information_Mark),
								Fill = (Brush)self.FindResource("VideoOSResourceKeysInternalVideoOSIconBuiltInInformationCombinedForegroundBrush"),
								StrokeThickness = 0.0
							}
						}
					}
				};
				break;
			case Icons.Confirmation_Combined:
				value = new VideoOSIconMultiPathSource
				{
					Paths = new List<VideoOSIconMultiPathPathInfo>
					{
						new VideoOSIconMultiPathPathInfo
						{
							AutoColor = false,
							Path = new Path
							{
								Data = ReadGeometryFromSvg(Icons.Status_Round_Disc),
								Fill = (Brush)self.FindResource("VideoOSResourceKeysInternalVideoOSIconBuiltInConfirmationCombinedBackgroundBrush"),
								StrokeThickness = 0.0
							}
						},
						new VideoOSIconMultiPathPathInfo
						{
							AutoColor = false,
							Path = new Path
							{
								Data = ReadGeometryFromSvg(Icons.Status_Confirmation_Check_Mark),
								Fill = (Brush)self.FindResource("VideoOSResourceKeysInternalVideoOSIconBuiltInConfirmationCombinedForegroundBrush"),
								StrokeThickness = 0.0
							}
						}
					}
				};
				break;
			case Icons.Server_State_Confirmation_Combined:
				value = new VideoOSIconMultiPathSource
				{
					Paths = new List<VideoOSIconMultiPathPathInfo>
					{
						new VideoOSIconMultiPathPathInfo
						{
							AutoColor = ResolveAutoColorValue(),
							Path = new Path
							{
								Data = ReadGeometryFromSvg(Icons.Server_State_Base),
								Fill = ResolveColorValue(self),
								StrokeThickness = 0.0
							}
						},
						new VideoOSIconMultiPathPathInfo
						{
							AutoColor = false,
							Path = new Path
							{
								Data = ReadGeometryFromSvg(Icons.State_Indicator),
								Fill = (Brush)self.FindResource("VideoOSResourceKeysInternalVideoOSIconBuiltInStateConfirmationCombinedForegroundBrush"),
								StrokeThickness = 0.0
							}
						}
					}
				};
				break;
			case Icons.Server_State_Warning_Combined:
				value = new VideoOSIconMultiPathSource
				{
					Paths = new List<VideoOSIconMultiPathPathInfo>
					{
						new VideoOSIconMultiPathPathInfo
						{
							AutoColor = ResolveAutoColorValue(),
							Path = new Path
							{
								Data = ReadGeometryFromSvg(Icons.Server_State_Base),
								Fill = ResolveColorValue(self),
								StrokeThickness = 0.0
							}
						},
						new VideoOSIconMultiPathPathInfo
						{
							AutoColor = false,
							Path = new Path
							{
								Data = ReadGeometryFromSvg(Icons.State_Indicator),
								Fill = (Brush)self.FindResource("VideoOSResourceKeysInternalVideoOSIconBuiltInStateWarningCombinedForegroundBrush"),
								StrokeThickness = 0.0
							}
						}
					}
				};
				break;
			case Icons.Server_State_Error_Combined:
				value = new VideoOSIconMultiPathSource
				{
					Paths = new List<VideoOSIconMultiPathPathInfo>
					{
						new VideoOSIconMultiPathPathInfo
						{
							AutoColor = ResolveAutoColorValue(),
							Path = new Path
							{
								Data = ReadGeometryFromSvg(Icons.Server_State_Base),
								Fill = ResolveColorValue(self),
								StrokeThickness = 0.0
							}
						},
						new VideoOSIconMultiPathPathInfo
						{
							AutoColor = false,
							Path = new Path
							{
								Data = ReadGeometryFromSvg(Icons.State_Indicator),
								Fill = (Brush)self.FindResource("VideoOSResourceKeysInternalVideoOSIconBuiltInStateErrorCombinedForegroundBrush"),
								StrokeThickness = 0.0
							}
						}
					}
				};
				break;
			case Icons.User_State_Confirmation_Combined:
				value = new VideoOSIconMultiPathSource
				{
					Paths = new List<VideoOSIconMultiPathPathInfo>
					{
						new VideoOSIconMultiPathPathInfo
						{
							AutoColor = ResolveAutoColorValue(),
							Path = new Path
							{
								Data = ReadGeometryFromSvg(Icons.User_State_Base),
								Fill = ResolveColorValue(self),
								StrokeThickness = 0.0
							}
						},
						new VideoOSIconMultiPathPathInfo
						{
							AutoColor = false,
							Path = new Path
							{
								Data = ReadGeometryFromSvg(Icons.State_Indicator),
								Fill = (Brush)self.FindResource("VideoOSResourceKeysInternalVideoOSIconBuiltInStateConfirmationCombinedForegroundBrush"),
								StrokeThickness = 0.0
							}
						}
					}
				};
				break;
			case Icons.User_State_Warning_Combined:
				value = new VideoOSIconMultiPathSource
				{
					Paths = new List<VideoOSIconMultiPathPathInfo>
					{
						new VideoOSIconMultiPathPathInfo
						{
							AutoColor = ResolveAutoColorValue(),
							Path = new Path
							{
								Data = ReadGeometryFromSvg(Icons.User_State_Base),
								Fill = ResolveColorValue(self),
								StrokeThickness = 0.0
							}
						},
						new VideoOSIconMultiPathPathInfo
						{
							AutoColor = false,
							Path = new Path
							{
								Data = ReadGeometryFromSvg(Icons.State_Indicator),
								Fill = (Brush)self.FindResource("VideoOSResourceKeysInternalVideoOSIconBuiltInStateWarningCombinedForegroundBrush"),
								StrokeThickness = 0.0
							}
						}
					}
				};
				break;
			case Icons.User_State_Error_Combined:
				value = new VideoOSIconMultiPathSource
				{
					Paths = new List<VideoOSIconMultiPathPathInfo>
					{
						new VideoOSIconMultiPathPathInfo
						{
							AutoColor = ResolveAutoColorValue(),
							Path = new Path
							{
								Data = ReadGeometryFromSvg(Icons.User_State_Base),
								Fill = ResolveColorValue(self),
								StrokeThickness = 0.0
							}
						},
						new VideoOSIconMultiPathPathInfo
						{
							AutoColor = false,
							Path = new Path
							{
								Data = ReadGeometryFromSvg(Icons.State_Indicator),
								Fill = (Brush)self.FindResource("VideoOSResourceKeysInternalVideoOSIconBuiltInStateErrorCombinedForegroundBrush"),
								StrokeThickness = 0.0
							}
						}
					}
				};
				break;
			case Icons.Chevron_Double_Down_With_Badge_Combined:
				value = new VideoOSIconMultiPathSource
				{
					Paths = new List<VideoOSIconMultiPathPathInfo>
					{
						new VideoOSIconMultiPathPathInfo
						{
							AutoColor = ResolveAutoColorValue(),
							Path = new Path
							{
								Data = ReadGeometryFromSvg(Icons.Chevron_Double_Down_Base),
								Fill = ResolveColorValue(self),
								StrokeThickness = 0.0
							}
						},
						new VideoOSIconMultiPathPathInfo
						{
							AutoColor = false,
							Path = new Path
							{
								Data = ReadGeometryFromSvg(Icons.State_Indicator_Top_Right),
								Fill = (Brush)self.FindResource("VideoOSResourceKeysInternalVideoOSIconBuiltInStateErrorCombinedForegroundBrush"),
								StrokeThickness = 0.0
							}
						}
					}
				};
				break;
			default:
				value = new VideoOSIconPathSource
				{
					AutoColor = ResolveAutoColorValue(),
					Path = new Path
					{
						Data = ReadGeometryFromSvg(icons),
						Fill = ResolveColorValue(self),
						StrokeThickness = 0.0
					}
				};
				break;
			}
			_builtInIconSourceDictionary.Add(key, value);
		}
		return _builtInIconSourceDictionary[key];
	}

	private Geometry ReadGeometryFromSvg(Icons icon)
	{
		List<string> list = new List<string>();
		using (XmlReader xmlReader = XmlReader.Create(Application.GetResourceStream(new Uri(FormattableString.Invariant($"pack://application:,,,/VideoOS.Platform;component/UI/Resources/BuiltInIcons/{icon}.svg"))).Stream))
		{
			while (xmlReader.ReadToFollowing("path"))
			{
				list.Add(xmlReader.GetAttribute("d"));
			}
		}
		if (!list.Any())
		{
			throw new Exception(FormattableString.Invariant($"No path(s) found for the icon {icon} in the file {icon}.svg."));
		}
		return Geometry.Parse(string.Join(" ", list));
	}

	private bool ResolveAutoColorValue()
	{
		return Appearance == Appearances.Default;
	}

	private Brush ResolveColorValue(DependencyObject self)
	{
		return Appearance switch
		{
			Appearances.Default => null, 
			Appearances.Error => (Brush)self.FindResource("VideoOSResourceKeysInternalVideoOSIconBuiltInSourceAppearanceErrorBrush"), 
			Appearances.Warning => (Brush)self.FindResource("VideoOSResourceKeysInternalVideoOSIconBuiltInSourceAppearanceWarningBrush"), 
			Appearances.Confirmation => (Brush)self.FindResource("VideoOSResourceKeysInternalVideoOSIconBuiltInSourceAppearancesConfirmationBrush"), 
			Appearances.Neutral => (Brush)self.FindResource("VideoOSResourceKeysInternalVideoOSIconBuiltInSourceAppearancesNeutralBrush"), 
			_ => throw new ArgumentOutOfRangeException("Appearance", Appearance, null), 
		};
	}
}
