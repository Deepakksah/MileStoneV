using System;
using System.Collections;
using System.Collections.Generic;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Resources;

namespace VideoOS.Platform;

public class Kind
{
	public static class SecurityNamespaceIdentifier
	{
		public static readonly Guid ManagementServer = new Guid("E9914E39-A67A-496C-A3C1-5B4AAAB28F15");

		public static readonly Guid RecordingServer = new Guid("9710735D-EB10-44EA-888D-FA8AB05A6433");

		public static readonly Guid FailoverServer = new Guid("6339122C-04F5-41F2-B3E3-3555997CFCEF");

		public static readonly Guid Hardware = new Guid("F97772D8-7B6D-4EA4-B6B5-EE4EA010FA2C");

		public static readonly Guid Camera = new Guid("623D03F8-C5D5-46BC-A2F4-4C03562D4F85");

		public static readonly Guid Microphone = new Guid("15F48F88-CA89-4926-9A84-2B02864EC77A");

		public static readonly Guid Speaker = new Guid("48B602BC-E752-4BBF-8E2A-7DE01F53A6DD");

		public static readonly Guid Metadata = new Guid("EDE4D51C-F691-4894-9C0B-C3AE096DC04D");

		public static readonly Guid Input = new Guid("3ED79EF0-EE2F-40B8-A49A-17E9380C5932");

		public static readonly Guid Output = new Guid("A95FCE90-753D-4CB7-86FB-F8CAD9F93B67");

		public static readonly Guid SmartWall = new Guid("8732FCAC-A63C-46EA-9683-532B4C6644E7");

		public static readonly Guid Matrix = new Guid("988D493D-1597-4BE6-AFF0-396BFD5D047A");

		public static readonly Guid ViewGroup = new Guid("EC543C56-ADF7-4600-B9AD-97E166552BE9");

		public static readonly Guid UserDefinedEvent = new Guid("D4FF31CD-D9AC-46DF-8E92-5B545D191BAF");

		public static readonly Guid Rule = new Guid("C24C91A7-7D89-459B-B78E-C4548C74640D");

		public static readonly Guid Site = new Guid("875F4A4E-4D78-4A8D-A978-290227253475");

		public static readonly Guid SystemMonitor = new Guid("2D4048FA-41DA-435A-A800-2419137E4A3E");

		public static readonly Guid LogServer = new Guid("C201AE6F-6FF3-41D4-BCBF-0F9DB5049988");

		public static readonly Guid SaveSearch = new Guid("D2D396E4-819E-4226-AA7F-AF36896D78BC");

		public static readonly Guid MetadataSearch = new Guid("55BF1320-0224-48C7-8F57-BD41090576C8");

		public static readonly Guid Alarms = new Guid("46BB0392-FD06-465B-8776-B90AC16639B3");

		public static readonly Guid AccessControlUnits = new Guid("6d3bdb33-d913-470f-8784-3dd4ee94db8c");
	}

	public static Guid Server = new Guid("3B25FE94-7C2F-499a-86DF-2FA68AA3E1B5");

	[Obsolete("Alerts are no longer supported by XProtect")]
	public static Guid Alert = new Guid("3F7F49E3-D027-450b-8BC5-590EC9AA5376");

	public static Guid Camera = new Guid("5135BA21-F1DC-4321-806A-6CE2017343C0");

	public static Guid Speaker = new Guid("B77D68FC-B231-441b-8EB5-901C89234111");

	public static Guid Microphone = new Guid("DF6284F6-18EE-4506-B8C4-65B5F31A140C");

	public static Guid InputEvent = new Guid("5FC737A9-BBF6-4473-A421-7E8075D45D9C");

	public static Guid OutputEvent = new Guid("5FC737A2-BBF2-4472-A422-7E8075D45D92");

	public static Guid TriggerEvent = new Guid("C9BDAC3F-41DC-4afa-B057-61767A3914B7");

	public static Guid Output = new Guid("CBAAA726-A089-4db6-8F0D-48772E595B1B");

	public static Guid Preset = new Guid("083A635C-8412-4ea2-8DD9-9223FCA35AB6");

	public static Guid TransactSource = new Guid("9A1492D1-FCA0-44d7-BA9C-3699C99063A7");

	public static Guid Folder = new Guid("F0E91DE0-4E81-4f00-8BB6-B9C932D5B598");

	public static Guid Matrix = new Guid("B4F8C44A-84B4-4333-8B8D-4B621A149C0B");

	public static Guid View = new Guid("84ED10D8-D5BC-4785-9C8E-F84440218F24");

	public static Guid ViewItemInstance = new Guid("69B9A865-20B0-4FFD-85B7-B062FE54CD8C");

	public static Guid Window = new Guid("15DA80D4-7595-431f-9866-674D2B80ACC5");

	public static Guid Screen = new Guid("BBB64F9C-E3C7-41d4-A77E-D488390D7FCB");

	public static Guid Role = new Guid("63F8BF0B-4185-4ae4-96B4-C46A2057A1FE");

	public static Guid User = new Guid("E18C7649-3D38-45dc-9CE1-B0DD06E77B24");

	public static Guid SmartWall = new Guid("EBA9E28B-143C-42EA-8D5A-BB72781F5472");

	public static Guid SmartWallPreset = new Guid("F8C3FC22-3622-4C43-AC02-D06BC87638EA");

	public static Guid SmartWallMonitor = new Guid("A4BB0877-1640-403F-B6E1-AA0B48383C92");

	public static Guid VideoWall = new Guid("EBA9E28B-143C-42EA-8D5A-BB72781F5472");

	public static Guid VideoWallPreset = new Guid("F8C3FC22-3622-4C43-AC02-D06BC87638EA");

	public static Guid VideoWallMonitor = new Guid("A4BB0877-1640-403F-B6E1-AA0B48383C92");

	public static Guid PlaybackController = new Guid("F618E8DA-E640-4646-9070-6BBC1277E533");

	public static Guid WorkSpace = new Guid("FCF7FDAF-BB7B-4569-B724-C6C2E35B7686");

	public static Guid TimeProfile = new Guid("3d0d6636-7914-4431-9935-3b4ea5905018");

	public static Guid Bookmark = new Guid("34A2E176-5666-4296-89E0-4DFCEEDA7DC0");

	public static Guid RestrictedMedia = new Guid("17F97151-F392-4C33-90DA-84B6B5DBA7AD");

	public static Guid RestrictedMediaLive = new Guid("0C37250C-C20E-47B3-9B9E-0AAD260E991B");

	public static Guid Metadata = new Guid("3C829278-37AE-4EE8-8C1D-D94412CEEB74");

	public static Guid Hardware = new Guid("C9DDC944-FB27-4E34-88F8-76023FCC9CE9");

	public static Guid Layout = new Guid("C20AB189-8568-4885-9CE0-41EFB153FEB0");

	public static Guid LayoutGroup = new Guid("B301A435-1A25-41E3-86B4-4786D9D876CD");

	public static Guid GisMapLocation = new Guid("47EE4E8B-E5A4-4C0B-9C99-B8031B3D118F");

	public static Guid AlarmDefinition = new Guid("9807CA8A-46C2-45ed-9446-6423DAE2CD88");

	public static Guid Webhook = new Guid("B9A5BC9C-E9A5-4A15-8453-FFA41F2815AC");

	public static readonly Guid WebhookAction = new Guid("e4852d57-f44e-4514-950b-15ed63908266");

	public static Guid AccessControlSystem = new Guid("DBB3A246-122E-498D-A804-6363A154A127");

	public static Guid AccessControlUnit = new Guid("F3B0A519-CE2F-4292-B637-97F93D441859");

	public static Guid AccessControlDoor = new Guid("a2076f49-1ca9-5a6e-ac96-3f7ab51a04ac");

	public static Guid LprServer = new Guid("6b1845a7-851f-4f0b-9644-940463a5c2e7");

	public static Hashtable DefaultTypeToCategoryTable = new Hashtable
	{
		{
			Server,
			Category.Server
		},
		{
			Camera,
			Category.VideoIn
		},
		{
			Speaker,
			Category.AudioOut
		},
		{
			Microphone,
			Category.AudioIn
		},
		{
			InputEvent,
			Category.TriggerIn
		},
		{
			TriggerEvent,
			Category.TriggerOut
		},
		{
			Folder,
			Category.Unknown
		},
		{
			Output,
			Category.TriggerOut
		},
		{
			Preset,
			Category.TriggerOut
		},
		{
			TransactSource,
			Category.Text
		},
		{
			Matrix,
			Category.TriggerOut
		},
		{
			View,
			Category.Layout
		},
		{
			ViewItemInstance,
			Category.Layout
		},
		{
			Window,
			Category.Layout
		},
		{
			Screen,
			Category.Layout
		},
		{
			VideoWall,
			Category.Layout
		},
		{
			VideoWallPreset,
			Category.Layout
		},
		{
			VideoWallMonitor,
			Category.Layout
		},
		{
			Role,
			Category.Unknown
		},
		{
			User,
			Category.Unknown
		},
		{
			WorkSpace,
			Category.Layout
		},
		{
			TimeProfile,
			Category.Unknown
		},
		{
			Bookmark,
			Category.Text
		},
		{
			RestrictedMedia,
			Category.Text
		},
		{
			RestrictedMediaLive,
			Category.Text
		},
		{
			Hardware,
			Category.Unknown
		},
		{
			Metadata,
			Category.Text
		},
		{
			Layout,
			Category.Layout
		},
		{
			LayoutGroup,
			Category.Layout
		},
		{
			GisMapLocation,
			Category.Unknown
		}
	};

	public static Hashtable DefaultTypeToNameTable = new Hashtable
	{
		{
			Server,
			Strings.Server
		},
		{
			Camera,
			Strings.Camera
		},
		{
			Speaker,
			Strings.Speaker
		},
		{
			Microphone,
			Strings.Microphone
		},
		{
			InputEvent,
			Strings.InputEvent
		},
		{
			TriggerEvent,
			Strings.Event
		},
		{
			Folder,
			Strings.SystemFolder
		},
		{
			Output,
			Strings.Output
		},
		{
			Preset,
			Strings.Preset
		},
		{
			TransactSource,
			Strings.Transact
		},
		{
			Matrix,
			Strings.Matrix
		},
		{
			View,
			Strings.SmartClientView
		},
		{
			ViewItemInstance,
			Strings.SmartClientViewItem
		},
		{
			Window,
			Strings.SmartClientWindow
		},
		{
			Screen,
			Strings.PhysicalScreen
		},
		{
			VideoWall,
			Strings.SmartWall
		},
		{
			VideoWallPreset,
			Strings.SmartwallPreset
		},
		{
			WorkSpace,
			Strings.WorkSpace
		},
		{
			TimeProfile,
			Strings.TimeProfile
		},
		{
			User,
			Strings.User
		},
		{
			Role,
			Strings.Role
		},
		{
			Bookmark,
			Strings.Bookmark
		},
		{
			RestrictedMedia,
			Strings.RestrictedMedia
		},
		{
			RestrictedMediaLive,
			Strings.RestrictedMediaLive
		},
		{
			Hardware,
			Strings.Hardware
		},
		{
			Metadata,
			Strings.Metadata
		},
		{
			Layout,
			Strings.Layout
		},
		{
			LayoutGroup,
			Strings.LayoutGroup
		},
		{
			VideoWallMonitor,
			Strings.Monitor
		},
		{
			GisMapLocation,
			Strings.GisMapLocation
		}
	};

	public static Dictionary<Guid, Guid> KindToNamespace = new Dictionary<Guid, Guid>
	{
		{
			Hardware,
			SecurityNamespaceIdentifier.Hardware
		},
		{
			Camera,
			SecurityNamespaceIdentifier.Camera
		},
		{
			Microphone,
			SecurityNamespaceIdentifier.Microphone
		},
		{
			Speaker,
			SecurityNamespaceIdentifier.Speaker
		},
		{
			Metadata,
			SecurityNamespaceIdentifier.Metadata
		},
		{
			InputEvent,
			SecurityNamespaceIdentifier.Input
		},
		{
			Output,
			SecurityNamespaceIdentifier.Output
		},
		{
			SmartWall,
			SecurityNamespaceIdentifier.SmartWall
		},
		{
			Matrix,
			SecurityNamespaceIdentifier.Matrix
		},
		{
			TriggerEvent,
			SecurityNamespaceIdentifier.UserDefinedEvent
		},
		{
			Server,
			SecurityNamespaceIdentifier.RecordingServer
		}
	};

	public static Hashtable KindToLogMessageSourceType = new Hashtable
	{
		{ Server, "Server" },
		{ Camera, "Device" },
		{ Speaker, "Device" },
		{ Microphone, "Device" },
		{ Hardware, "Hardware" },
		{ Metadata, "Device" },
		{ InputEvent, "Device" },
		{ Output, "Device" },
		{ TriggerEvent, "External" },
		{ TransactSource, "External" }
	};
}
