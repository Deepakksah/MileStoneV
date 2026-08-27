using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using VideoOS.Common.Crypto;
using VideoOS.Platform.Common;
using VideoOS.Platform.SDK.Config;

namespace VideoOS.Platform.SDK.Export;

public class ScpFileHandler
{
	private class PluginConfiguration
	{
		public Dictionary<string, string> Attributes { get; set; }
	}

	private class SmartClientEncrypter
	{
		private string _projectPassword;

		private string _saltString;

		public SmartClientEncrypter(string projectPassword, string saltString)
		{
			_projectPassword = projectPassword;
			_saltString = saltString;
		}

		public string EncryptData(string inputData)
		{
			if (string.IsNullOrWhiteSpace(inputData) || string.IsNullOrWhiteSpace(_projectPassword) || string.IsNullOrWhiteSpace(_saltString))
			{
				return "";
			}
			return Convert.ToBase64String(EncryptionFactory.CreateSymmetricFromPassword(_projectPassword, Encoding.Unicode.GetBytes(_saltString)).Encrypt(inputData));
		}
	}

	private string _filePath;

	private XmlDocument _doc;

	private Guid _recorder = Guid.NewGuid();

	private static List<PluginConfiguration> _pluginConfigurations = new List<PluginConfiguration>
	{
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "88800e1e-a2ab-4243-a2d6-f8ce24fef6ac" },
				{ "plugin", "VideoOS.RemoteClient.Application.ExtensionPlugins.Audio.AudioExtensionPlugin" },
				{ "twowayaudio", "True" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "3c2851c2-38c2-459a-949c-83f26a805078" },
				{ "plugin", "VideoOS.RemoteClient.Application.Data.ViewLayouts.UnknownViewLayout" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "05ecb5e7-564a-4192-92b8-2ba99f89081c" },
				{ "plugin", "VideoOS.RemoteClient.Application.Data.ViewLayouts.ViewLayout1p3Wide" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "8f62b5ac-6036-4115-aec2-1de3f9708a5a" },
				{ "plugin", "VideoOS.RemoteClient.Application.Data.ViewLayouts.ViewLayout1p5" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "5de4886b-6bb9-42d9-ac32-8575db337764" },
				{ "plugin", "VideoOS.RemoteClient.Application.Data.ViewLayouts.ViewLayout1p7" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "240578d7-6a67-46dc-8d88-09aaedd65378" },
				{ "plugin", "VideoOS.RemoteClient.Application.Data.ViewLayouts.ViewLayout1p8Wide" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "b7c02ce1-5f66-4886-b0c8-018224a59621" },
				{ "plugin", "VideoOS.RemoteClient.Application.Data.ViewLayouts.ViewLayout1x1" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "f4f41dc1-f987-4d74-9246-2d70634d25b0" },
				{ "plugin", "VideoOS.RemoteClient.Application.Data.ViewLayouts.ViewLayout2p4Wide" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "095e5cc8-b81b-46ae-8ba2-31565f13cd56" },
				{ "plugin", "VideoOS.RemoteClient.Application.Data.ViewLayouts.ViewLayout2p8" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "6f499fd2-fddc-4f55-820a-47eaae4f853a" },
				{ "plugin", "VideoOS.RemoteClient.Application.Data.ViewLayouts.ViewLayout2x2" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "ce64ddc3-d27a-42be-8465-ed780e748e49" },
				{ "plugin", "VideoOS.RemoteClient.Application.Data.ViewLayouts.ViewLayout3x3" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "6d299029-ab1e-4d04-9fd5-0b13fa265b90" },
				{ "plugin", "VideoOS.RemoteClient.Application.Data.ViewLayouts.ViewLayout4x3Wide" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "aaaef1e1-f157-4132-adf7-fba45fac722c" },
				{ "plugin", "VideoOS.RemoteClient.Application.Data.ViewLayouts.ViewLayout4x4" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "d6c1bc0e-9319-4cb4-98d7-80d953ccf6b7" },
				{ "plugin", "VideoOS.RemoteClient.Application.Data.ViewLayouts.ViewLayout5x5" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "bdda290a-8413-4b99-b266-b7db2263a3fe" },
				{ "plugin", "VideoOS.RemoteClient.Application.Data.ViewLayouts.ViewLayout6x6" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "a8b0ec85-5f87-489b-adf2-296185bfadd6" },
				{ "plugin", "VideoOS.RemoteClient.Application.Data.ViewLayouts.ViewLayout7x7" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "edb95d6d-a010-4450-b60a-9a0c043c3e9a" },
				{ "plugin", "VideoOS.RemoteClient.Application.Data.ViewLayouts.ViewLayout8x8" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "140f17dc-33a2-490b-867b-79c1950d1f0f" },
				{ "plugin", "VideoOS.RemoteClient.Application.Data.ViewLayouts.ViewLayoutCustom" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "57abe978-8861-4577-8a09-5fa6e43c4109" },
				{ "plugin", "VideoOS.RemoteClient.Application.Data.ContentTypes.EmptyContentType.EmptyContentType" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "ce8eb517-c0cb-45e9-80b3-5adab41eb9a5" },
				{ "plugin", "VideoOS.RemoteClient.Application.Data.ContentTypes.CameraContentType.CameraContentType" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "83d6787e-481c-457c-ae79-5ae4214b9ecd" },
				{ "plugin", "VideoOS.RemoteClient.Application.Data.ContentTypes.BookmarkContentType.BookmarkContentType" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "9e829fe6-3ad6-4ecc-a4bd-2b1438dd092c" },
				{ "plugin", "VideoOS.RemoteClient.Application.Data.ContentTypes.AudioContentType.AudioContentType" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "44553435-b40f-49a9-8b9e-a72c5aa87dfa" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.AddToExportViewItemBasedAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "ce891d7a-0c96-429b-a438-7c584c41dc78" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.BrowseBeginAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "1af3f763-99bc-4837-9586-ca66a0327b9f" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.BrowseEndAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "de534f59-68e4-4970-a1f7-4c556855c565" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.BrowseGotoAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "04610607-c457-4ba7-b3bd-2168a6b7e263" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.BrowseNextAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "e8598ab4-88eb-49b7-ae57-0df063a46d02" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.BrowseNextSequenceAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "144f4a1d-fc2c-41a5-b978-f8d42151f27f" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.BrowsePlayForwardAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "0b0ae2a6-cd42-4d74-9803-5dd0c11dabcf" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.BrowsePlayReverseAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "7d721f53-ecd0-4736-b108-3ae369595444" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.BrowsePlayStopAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "bcb98436-da4c-491d-8789-a534d87c731a" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.BrowsePreviousAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "afcdef26-d34c-49f0-bdaf-87815731b531" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.BrowsePreviousSequenceAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "18c0ece2-5b99-4083-8e2b-ad2cb707b7a7" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.ClearAllSignalsOnAllCamerasInSelectedViewAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "3cd5c6aa-8203-4c7c-b58f-92a727821c14" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.CloseAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "4cf31a8c-5911-42d4-9e00-c28f5399717a" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.ClearMotionOccurredSignalOnAllCamerasInSelectedViewAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "e189a39d-b3be-4411-a73e-30381b8b93df" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.ClearAllSignalsOnSelectedCameraAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "e37bb9d0-a515-42f0-bc0e-590316ce9a5f" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.ClearMotionOccurredSignalOnSelectedCameraAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "bba728b1-49e9-4aff-b7e5-3cb6da97af6a" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.OpenSurfaceDiagnosticsAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "e6d15e3e-04f5-45ee-9895-9fd47ecef514" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.PtzPausePatrollingAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "f2a0ef48-a0ed-4ea8-9642-0e30c5eaad04" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.PtzReleaseAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "7ea9502b-b4fc-43ff-9c9a-11bc6ae4c41f" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.PtzReserveAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "1b37af82-4f92-4a2a-abaa-427ddcf4dcb2" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.RemovablePrivacyMaskUnliftAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "38fe5388-2ba8-4283-8eed-6065fba9af61" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.ActivateAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "ba8cda0a-d312-4d14-8bf9-79fb12858baa" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.ShowEvidenceLockDialogViewItemBasedAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "d3741737-0eb8-4975-87a9-bcf16025c254" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.SearchAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "f17fd9f4-fc0b-4dee-a7fa-272d7c51f62f" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.SelectNextWindowAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "939f21a8-22c4-429d-b0e3-03330a5c179c" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.SelectWindowAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "bb955c27-fcbd-4815-b4ac-582140aa8c97" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.CopySelectedCameraViewItemAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "f9eca679-a9b5-44b1-8621-c0198cd68647" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.PrintSelectedViewItemAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "65917dd8-cd6f-4ab3-8220-f7bdac609ee1" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.ReloadCameraViewItems" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "86d9dc95-a14b-4c6b-8be5-c7c72798eb7f" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.ShowEvidenceLockListAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "cc275ddc-5973-4eba-9d8c-e2a2effbc6ae" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.ShowExportDialogDeviceBasedAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "9bff7d6f-3982-419f-a031-8baa9a7551fa" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.ShowExportDialogViewItemBasedAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "361025ab-747b-486d-8e29-2a60280c67c1" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.HideFullscreenViewAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "c80e1920-eeb4-4c63-911b-8dcb784d7fe3" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.PtzMoveContinuousDownAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "477d1bd3-d863-4b85-ae91-aba97b1c01e8" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.PtzMoveContinuousDownLeftAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "1fe09341-4583-46dc-8729-e441c02f2bbd" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.PtzMoveContinuousDownRightAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "49202cb2-2db7-46be-8f8e-3506076dd006" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.PtzMoveContinuousLeftAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "bffd0061-1676-46df-be17-e2f387cebdfb" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.PtzMoveContinuousRightAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "0109dd80-139a-48b3-93d1-9dcb4f9eaa20" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.PtzMoveContinuousUpAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "9a1eefcc-a117-4d06-a267-0812e7497bd1" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.PtzMoveContinuousUpLeftAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "520f4e0e-30e7-4201-b583-e4493ef9492e" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.PtzMoveContinuousUpRightAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "21fb856b-876f-4c21-a465-320470995736" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.PtzMoveContinuousZoomInAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "a849b353-821a-4fc5-8825-d320d0f77c14" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.PtzMoveContinuousZoomOutAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "136abcad-a1c7-4f08-8792-4ad7c6f4bf11" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.ShowRetrievalDialogViewItemBasedAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "e1e76756-65af-4381-bc49-528740435874" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.SnapshotSelectedViewItemAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "a853216d-bcf9-40fa-82cc-20d0a1fafb34" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.ToggleFullscreenViewAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "b46650d3-25ba-4e9b-b4f7-e06523e676ca" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.PtzToggleDigitalZoomEnableStateAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "5664fd57-35bc-498d-b348-de0638f38f13" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.FindCameraViewItemAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "7bcc957c-2f28-4ccd-ae60-354dbaa9c982" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.GetModeAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "df1c9215-0bb6-4be8-bdff-a310b8ce4526" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.GetSelectedCameraViewItemShortcutAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "3a32c0fd-fece-4959-b39c-02ed1bf56b8a" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.SetPlayBackSpeedAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "f7ba65eb-d9e0-4279-90d2-c846fd9d6bf0" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.GetSelectedViewItemIndexAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "8982c42c-5044-4748-aecf-fcd727f0c211" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.GetBrowseDateAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "6b29e6d1-cb29-4fd3-914a-f39112a8811d" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.PtzEnableDigitalZoomAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "7e23c422-b953-4266-b401-6817baac1e88" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.SelectCameraViewItemAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "33b599c4-af11-477a-8ab7-07c726cb9b00" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.SetCameraViewItemCameraAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "1f8378a6-57f9-4ad9-a8ef-e1e2d17c6506" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.RestoreSelectedViewItemAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "b3b4ddaa-d7c3-416a-ab3e-47644290d951" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.ShowCamerasAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "cf3931af-ece1-46a7-91c2-df5bdfec1a4f" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.ToggleSelectedViewItemMaximizedStateAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "61ddae8d-ba30-4d76-8afa-eccdfa85a690" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.MaximizeSelectedViewItemAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "d5c26632-1492-4eca-a546-5adac7551288" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.MoveViewItemSelectionRightAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "16dd9cc6-03a0-4330-890b-1f72b2ded290" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.MoveViewItemSelectionLeftAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "b183c00a-3a6c-4a50-b5dc-d61847fc2625" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.MoveViewItemSelectionDownAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "d155348b-e990-4332-862f-51d160fff250" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.MoveViewItemSelectionUpAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "7e0c4da1-376f-4bf5-b83e-684a80fdf839" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.DeselectViewItemAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "5656611c-68c0-4bb0-90c5-f3ac21cd8a93" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.RestoreAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "976953ca-5785-4b2a-a676-9518b0ae1dd8" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.MultiWindowOpenFloatingWindowAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "cae33b43-e61e-4bd7-88df-146d044e3d9c" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.MultiWindowOpenFullScreenWindowAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "3f5a6577-68b1-4cef-b311-b92fa6a74bc9" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.FindViewIndexAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "dd9c26b0-f896-4caa-8d44-8beb5893a351" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.GetSelectedGroupIdAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "06fa9e15-6a7e-433a-841f-dd289e7554b1" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.GetSelectedViewFullNameAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "a3fc2ccd-6bb4-4d43-afaf-7b7d455662b5" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.GetSelectedViewIdAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "b24565b2-39a8-4dd8-940f-b1e3f00e0ca7" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.GetViewFullNameAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "9faf0a97-6cd6-498d-b77f-b3a91cdb93bc" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.GetViewIdAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "0605001e-0272-4c0c-a733-a4214564ba1b" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.GetViewIndexAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "5fb685c7-3447-4355-a075-8a4cbd5c7d75" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.GroupCountAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "480e90b4-dc98-4a8f-a1a2-e32780143ea9" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.HideSidePaneAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "2fb05251-8e3a-45a7-aca4-c4de86c5017f" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.MaximizeAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "1d26b0e6-e977-441d-97dc-fbc953151929" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.MinimizeAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "ad8817ce-c0b5-4d2f-bb0b-28c309bc9b83" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.NextViewAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "6abe3bef-e09a-47e6-8b14-8f0e259fbc6c" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.PreviousViewAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "22af2923-29cf-4338-bfd1-7185c1b8e72e" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.PtzMoveDownAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "6ed12a9f-99fc-46a7-a2a7-369c11b164e8" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.PtzMoveDownLeftAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "f68e2ed7-fee9-4056-8740-daa8fa4db31c" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.PtzMoveDownRightAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "0a4ac095-a23b-4b9e-9749-048916d3a38d" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.PtzMoveHomeAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "a2f2f206-f775-4f55-836f-64a1f1793b5d" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.PtzMoveLeftAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "de9eb885-dad3-4d19-a9d4-51ae41b906af" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.PtzMoveRightAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "54324578-e13a-4d07-8517-25b3ab4db9e6" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.PtzMoveUpAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "119f5bd3-d31c-4ede-b1d3-3a01070167df" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.PtzMoveUpLeftAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "f328fa2d-982a-46d7-981d-21d483d5048d" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.PtzMoveUpRightAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "d06dda54-09c8-4330-9efb-32dbe7e17b24" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.PtzMoveZoomInAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "6a7f1f06-71dd-42b2-b39a-f41dbc0e69bc" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.PtzMoveZoomOutAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "27a6601c-4d1a-4c3b-b81b-697371e7ad7f" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.SelectViewAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "856ead6f-165e-4d10-a7c8-56acef57a161" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.SelectViewItemAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "1a76f017-1ad9-4a39-9c24-0775d83476e5" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.ShowBrowseAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "af8e1a09-0ef7-492c-8e8f-91d1770017d9" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.ShowSetupAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "b25396bd-72b9-449f-a9ae-0c06ed07ad75" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.ShowSidePaneAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "8566d726-2ea7-4342-b1b3-86946ea09058" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.ShowSingleCameraAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "32998ec4-937e-4430-a4ec-155127a54bdc" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.ToggleSidePaneAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "213f045c-0d6f-486e-a664-ebccc5373f63" },
				{ "plugin", "VideoOS.RemoteClient.Application.Actions.ViewCountAction" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "2ba838e5-8e27-419d-b922-3cccd2d98244" },
				{ "plugin", "VideoOS.RemoteClient.Plugin.AdvancedKeyboardControl.AdvancedKeyboardControl" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "99c277ed-30a3-426b-8778-0878411e8eb5" },
				{ "plugin", "VideoOS.RemoteClient.Plugin.CentralizedSearch.CentralizedSearchContentType" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "05332ba1-9f08-4fa8-a11b-7ad8aa6186bc" },
				{ "plugin", "VideoOS.RemoteClient.Plugin.HtmlViewItem.HtmlContentType" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "39f290fb-6d5c-412b-88d1-a7bb8c0fa76e" },
				{ "plugin", "VideoOS.RemoteClient.Plugin.KeyboardSupport.KeyboardPlugin" }
			}
		},
		new PluginConfiguration
		{
			Attributes = new Dictionary<string, string>
			{
				{ "id", "7edad11c-3b69-4db2-ac2e-38491d386e8e" },
				{ "plugin", "VideoOS.RemoteClient.Plugin.KeyboardSupport.ShowSetupAction" }
			}
		}
	};

	public ScpFileHandler(string path)
	{
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
		_filePath = Path.Combine(path, "Project.scp");
		if (File.Exists(_filePath))
		{
			throw new MIPException("Project.scp file already exists in the export location. Filepath: " + _filePath);
		}
		StringBuilder stringBuilder = new StringBuilder();
		StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
		try
		{
			using (XmlWriter xmlWriter = new XmlTextWriter(stringWriter))
			{
				stringWriter = null;
				xmlWriter.WriteStartElement("ProjectFile");
				xmlWriter.WriteElementString("Version", "25.3.0.0");
				xmlWriter.WriteStartElement("SystemInformation");
				xmlWriter.WriteStartElement("Recorders");
				xmlWriter.WriteStartElement("OfflineRecorderInfo");
				xmlWriter.WriteStartElement("OfflineSources");
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("RecorderId");
				xmlWriter.WriteValue(_recorder.ToString());
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("Cameras");
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("Microphones");
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("Speakers");
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("PluginConfigurations");
				foreach (PluginConfiguration pluginConfiguration in _pluginConfigurations)
				{
					xmlWriter.WriteStartElement("pluginconfiguration");
					foreach (KeyValuePair<string, string> attribute in pluginConfiguration.Attributes)
					{
						xmlWriter.WriteAttributeString(attribute.Key, attribute.Value);
					}
					xmlWriter.WriteEndElement();
				}
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("DeviceMappings");
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				xmlWriter.Flush();
			}
			_doc = new XmlDocument();
			_doc.LoadXml(stringBuilder.ToString());
			_doc.Save(_filePath);
		}
		finally
		{
			stringWriter?.Close();
		}
	}

	public void Save(bool addTamperingKey)
	{
		if (addTamperingKey)
		{
			new ProjectfileTamperingManager().AssignTamperingKey(_doc);
		}
		_doc.Save(_filePath);
	}

	public void EnsureSourceDefined(Item item, IStreamDevice device, string exportAndSourcePath, string password, Guid trackId)
	{
		try
		{
			string text = item.FQID.ObjectId.ToString();
			string value = text;
			string text2 = "";
			string text3 = "";
			XmlNode xmlNode = _doc.SelectSingleNode("/ProjectFile/SystemInformation/Recorders/OfflineRecorderInfo/OfflineSources");
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("OfflineSourceInfo[SourceId=\"" + text + "\"]");
			if (xmlNode2 == null)
			{
				XmlWriter xmlWriter = xmlNode.CreateNavigator().AppendChild();
				xmlWriter.WriteStartElement("OfflineSourceInfo");
				xmlWriter.WriteElementString("SourceId", text);
				xmlWriter.WriteElementString("DatabaseType", "MediaStorage");
				xmlWriter.WriteElementString("BankPath", exportAndSourcePath);
				xmlWriter.WriteElementString("TableName", trackId.ToString());
				xmlWriter.WriteElementString("IncludeArchives", "True");
				if (!string.IsNullOrEmpty(password))
				{
					xmlWriter.WriteElementString("Password", EncryptData(password, text, password));
				}
				xmlWriter.WriteEndElement();
				xmlWriter.Flush();
				xmlWriter.Close();
			}
			else
			{
				XmlNode xmlNode3 = xmlNode2.SelectSingleNode("TableName");
				if (xmlNode3 != null)
				{
					XPathNavigator xPathNavigator = xmlNode3.CreateNavigator();
					xPathNavigator.SetValue(xPathNavigator.Value + "," + trackId.ToString());
				}
			}
			if (item.FQID.Kind == Kind.Camera)
			{
				text3 = "Cameras";
				text2 = "OfflineCameraInfo";
				CheckMapping(item);
			}
			else if (item.FQID.Kind == Kind.Microphone)
			{
				text3 = "Microphones";
				text2 = "OfflineMicrophoneInfo";
			}
			else if (item.FQID.Kind == Kind.Speaker)
			{
				text3 = "Speakers";
				text2 = "OfflineSpeakerInfo";
			}
			else if (item.FQID.Kind == Kind.Metadata)
			{
				text3 = "MetadataDevices";
				text2 = "OfflineMetadataDeviceInfo";
			}
			XmlNode xmlNode4 = _doc.SelectSingleNode("/ProjectFile/SystemInformation/Recorders/OfflineRecorderInfo");
			XmlNode xmlNode5 = xmlNode4.SelectSingleNode(text3);
			if (xmlNode5 == null)
			{
				XmlWriter xmlWriter2 = xmlNode4.CreateNavigator().AppendChild();
				xmlWriter2.WriteElementString(text3, "");
				xmlWriter2.Flush();
				xmlWriter2.Close();
			}
			xmlNode5 = xmlNode4.SelectSingleNode(text3);
			if (xmlNode5.SelectSingleNode(text2 + "[SourceId=\"" + text + "\"]") == null)
			{
				XmlWriter xmlWriter3 = xmlNode5.CreateNavigator().AppendChild();
				xmlWriter3.WriteStartElement(text2);
				xmlWriter3.WriteElementString("SourceId", text);
				xmlWriter3.WriteElementString("DeviceId", value);
				xmlWriter3.WriteElementString("Name", item.Name);
				if (item.FQID.Kind == Kind.Metadata)
				{
					WriteMetadataInfo(xmlWriter3, device as IMetadataDevice);
				}
				xmlWriter3.WriteEndElement();
				xmlWriter3.Flush();
				xmlWriter3.Close();
			}
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("ScrFileHandler", exception);
		}
	}

	private void WriteMetadataInfo(XmlWriter writer, IMetadataDevice metadataDeviceInfo)
	{
		writer.WriteStartElement("RecorderId");
		writer.WriteValue(_recorder.ToString());
		writer.WriteEndElement();
		if (metadataDeviceInfo.MetadataAccess != null)
		{
			writer.WriteStartElement("MetadataDeviceSecurity");
			if (metadataDeviceInfo.MetadataAccess.Browse)
			{
				writer.WriteStartElement("Browse");
				writer.WriteValue(metadataDeviceInfo.MetadataAccess.Browse);
				writer.WriteEndElement();
			}
			if (metadataDeviceInfo.MetadataAccess.Live)
			{
				writer.WriteStartElement("Live");
				writer.WriteValue(metadataDeviceInfo.MetadataAccess.Live);
				writer.WriteEndElement();
			}
			if (metadataDeviceInfo.MetadataAccess.GetSequences)
			{
				writer.WriteStartElement("GetSequences");
				writer.WriteValue(metadataDeviceInfo.MetadataAccess.GetSequences);
				writer.WriteEndElement();
			}
			writer.WriteStartElement("ExportDatabase");
			writer.WriteValue(metadataDeviceInfo.MetadataAccess.ExportDatabase);
			writer.WriteEndElement();
			writer.WriteEndElement();
		}
		if (metadataDeviceInfo.MetadataTypes == null || !metadataDeviceInfo.MetadataTypes.Any())
		{
			return;
		}
		writer.WriteStartElement("MetadataTypes");
		foreach (MetadataType value in metadataDeviceInfo.MetadataTypes.Values)
		{
			writer.WriteStartElement("MetadataTypeInfo");
			writer.WriteElementString("Id", value.Id.ToString());
			writer.WriteElementString("Name", value.Name);
			writer.WriteElementString("DisplayId", value.DisplayId.ToString());
			writer.WriteElementString("DisplayName", value.DisplayName);
			writer.WriteElementString("ValidTime", value.ValidTime.ToString(CultureInfo.InvariantCulture));
			writer.WriteEndElement();
		}
		writer.WriteEndElement();
	}

	private void CheckMapping(Item item)
	{
		List<Item> related = item.GetRelated();
		if (related == null || related.Count <= 0)
		{
			return;
		}
		foreach (Item item2 in related)
		{
			string text = item.FQID.ObjectId.ToString().ToLower();
			string text2 = item2.FQID.ObjectId.ToString().ToLower();
			XmlNode xmlNode = _doc.SelectSingleNode("/ProjectFile/SystemInformation");
			XmlNode xmlNode2 = _doc.SelectSingleNode("/ProjectFile/SystemInformation/DeviceMappings");
			if (xmlNode2 == null)
			{
				XmlWriter xmlWriter = xmlNode.CreateNavigator().AppendChild();
				xmlWriter.WriteElementString("DeviceMappings", "");
				xmlWriter.Flush();
				xmlWriter.Close();
			}
			xmlNode2 = _doc.SelectSingleNode("/ProjectFile/SystemInformation/DeviceMappings");
			XmlNodeList xmlNodeList = xmlNode2.SelectNodes("DeviceMappingInfo[DeviceA=\"" + text + "\"]");
			bool flag = false;
			foreach (XmlNode item3 in xmlNodeList)
			{
				XmlNode xmlNode3 = item3.SelectSingleNode("DeviceB");
				if (xmlNode3 != null && xmlNode3.InnerText == text2)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				XmlWriter xmlWriter2 = xmlNode2.CreateNavigator().AppendChild();
				xmlWriter2.WriteStartElement("DeviceMappingInfo");
				xmlWriter2.WriteElementString("DeviceA", text);
				xmlWriter2.WriteElementString("DeviceB", text2);
				xmlWriter2.WriteEndElement();
				xmlWriter2.Flush();
				xmlWriter2.Close();
			}
		}
	}

	public void SetupProjectFilePassword(string password, int numberOfItemsInExport)
	{
		if (!string.IsNullOrEmpty(password))
		{
			string salt = new Guid("981C7A76-8F24-4FB0-B0E1-CFAD6CFF8F32").ToString() + numberOfItemsInExport.ToString(CultureInfo.InvariantCulture);
			XmlNode xmlNode = _doc.SelectSingleNode("/ProjectFile");
			if (xmlNode == null)
			{
				throw new Exception("Invalid file format, no ProjectFile Node");
			}
			XmlWriter xmlWriter = xmlNode.CreateNavigator().AppendChild();
			xmlWriter.WriteComment(" Validate ");
			xmlWriter.WriteElementString("Validate", EncryptData(password, salt, "ProjectFile"));
			xmlWriter.Flush();
			xmlWriter.Close();
		}
	}

	private static string EncryptData(string password, string salt, string encryptData)
	{
		return new SmartClientEncrypter(password, salt).EncryptData(encryptData);
	}
}
