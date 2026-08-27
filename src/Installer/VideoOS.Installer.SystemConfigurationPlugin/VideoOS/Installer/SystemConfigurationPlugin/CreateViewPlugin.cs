using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using VideoOS.Installer.Logging;
using VideoOS.Installer.SystemConfigurationPlugin.Properties;
using VideoOS.Management.VmoClient;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class CreateViewPlugin : ProgressPlugin
{
	public CreateViewPlugin(int serverPort)
		: base(serverPort)
	{
	}

	public void Start(IEnumerable<DeviceConfiguration> devices)
	{
		using ServerConnection serverConnection = new ServerConnection("localhost");
		serverConnection.ServerPort = base.ServerPort;
		VmoClient client = new VmoClient(serverConnection);
		try
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Adding views..."), TraceEventType.Information);
			PublicViewGroup publicViewGroup = CreateRootGroup(client.ManagementServer, "Default group", "Default group created by the installer");
			PublicViewGroup parentViewGroup = CreateViewGroup(publicViewGroup, "Default view group", "Default view group created by the installer");
			CallActionWithCameraBatches(devices, delegate(IEnumerable<DeviceConfiguration> devicesInView, string viewName)
			{
				CreateView(client.ManagementServer, devicesInView, parentViewGroup, viewName);
			});
		}
		finally
		{
			if (client != null)
			{
				((IDisposable)client).Dispose();
			}
		}
	}

	public void CallActionWithCameraBatches(IEnumerable<DeviceConfiguration> devices, Action<IEnumerable<DeviceConfiguration>, string> action)
	{
		if (devices == null)
		{
			throw new ArgumentNullException("devices");
		}
		if (action == null)
		{
			throw new ArgumentNullException("action");
		}
		int num = devices.Count();
		if (num == 0)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "No devices, no view added."), TraceEventType.Information);
			return;
		}
		OnProgressChanged(0, Resources.StartAddingViews);
		if (num <= 16)
		{
			action(devices, "Default view");
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Number of devices <= 16, added a single view"), TraceEventType.Information);
			OnProgressChanged(100, Resources.AddedDefaultView);
			return;
		}
		int num2 = 0;
		int num3 = 0;
		while (num3 < num)
		{
			IEnumerable<DeviceConfiguration> arg = devices.Skip(num3).Take(16);
			string arg2 = string.Format(CultureInfo.CurrentUICulture, "Default view {0}", num2 + 1);
			action(arg, arg2);
			num2++;
			num3 = num2 * 16;
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Added Default view {0}", num2), TraceEventType.Information);
			OnProgressChanged(Convert.ToInt32((double)num3 * 100.0 / (double)num), string.Format(CultureInfo.CurrentUICulture, Resources.AddedDefaultViewNumberN, num2 + 1));
		}
	}

	private static void CreateView(ManagementServer managementServer, IEnumerable<DeviceConfiguration> devices, PublicViewGroup parentViewGroup, string name)
	{
		int num = devices.Count();
		if (num >= 1)
		{
			string viewItemDisplayName = "Camera View Item";
			IEnumerable<CustomViewItem> source = devices.Select((DeviceConfiguration camera) => GenerateCustomViewItem(camera, viewItemDisplayName));
			View view = new View(parentViewGroup, source.ToArray());
			view.Name = name;
			string layout = GetLayout(num);
			view.DefinitionXml = GenerateViewXml(name, GenerateDefinitionXml(managementServer, layout));
			view.Create();
		}
	}

	public static string GetLayout(int numberOfDevices)
	{
		string format = "{0} x {0}";
		int num = Convert.ToInt32(Math.Ceiling(Math.Sqrt(numberOfDevices)));
		return string.Format(CultureInfo.InvariantCulture, format, num);
	}

	private static PublicViewGroup CreateViewGroup(PublicViewGroup publicViewGroup, string name, string description)
	{
		PublicViewGroup publicViewGroup2 = publicViewGroup.PublicViewGroups.FirstOrDefault((PublicViewGroup v) => v.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
		if (publicViewGroup2 == null)
		{
			publicViewGroup2 = new PublicViewGroup(publicViewGroup);
			publicViewGroup2.Name = name;
			publicViewGroup2.Description = description;
			publicViewGroup2.Create();
		}
		return publicViewGroup2;
	}

	private static PublicViewGroup CreateRootGroup(ManagementServer managementServer, string name, string description)
	{
		PublicViewGroup publicViewGroup = managementServer.PublicViewGroups.FirstOrDefault((PublicViewGroup v) => v.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
		if (publicViewGroup == null)
		{
			publicViewGroup = new PublicViewGroup(managementServer);
			publicViewGroup.Name = name;
			publicViewGroup.Description = description;
			publicViewGroup.Create();
		}
		return publicViewGroup;
	}

	private static CustomViewItem GenerateCustomViewItem(DeviceConfiguration device, string name)
	{
		Guid id = Guid.NewGuid();
		XElement xElement = new XElement("viewitem", new XAttribute("id", id.ToString()), new XAttribute("displayname", name), new XAttribute("shortcut", ""), new XAttribute("type", "VideoOS.RemoteClient.Application.Data.ContentTypes.CameraContentType.CameraViewItem, VideoOS.RemoteClient.Application"), new XAttribute("smartClientId", Guid.NewGuid().ToString().ToUpper(CultureInfo.InvariantCulture)), new XElement("iteminfo", new XAttribute("cameraid", device.Id), new XAttribute("lastknowncameradisplayname", device.Name), new XAttribute("livestreamid", "00000000-0000-0000-0000-000000000000"), new XAttribute("imagequality", 100), new XAttribute("framerate", 0), new XAttribute("maintainimageaspectratio", "True"), new XAttribute("usedefaultdisplaysettings", "True"), new XAttribute("showtitlebar", "True"), new XAttribute("keepimagequalitywhenmaximized", "False"), new XAttribute("updateonmotiononly", "False"), new XAttribute("soundonmotion", 0), new XAttribute("soundonevent", 0), new XAttribute("smartsearchgridwidth", 0), new XAttribute("smartsearchgridheight", 0), new XAttribute("smartsearchgridmask", ""), new XAttribute("pointandclickmode", 0), new XAttribute("usingproperties", "True")), new XElement("properties", new XElement("property", new XAttribute("name", "cameraid"), new XAttribute("value", device.Id)), new XElement("property", new XAttribute("name", "framerate"), new XAttribute("value", 0)), new XElement("property", new XAttribute("name", "imagequality"), new XAttribute("value", 100)), new XElement("property", new XAttribute("name", "lastknowncameradisplayname"), new XAttribute("value", device.Name))));
		return new CustomViewItem(id)
		{
			DefinitionXml = xElement.ToString()
		};
	}

	private static XElement GenerateDefinitionXml(ManagementServer managementServer, string layoutName)
	{
		return XElement.Parse(managementServer.ViewLayoutGroups.SelectMany((ViewLayoutGroup g) => g.ViewLayouts).First((ViewLayout l) => l.Name == layoutName).DefinitionXml);
	}

	private static string GenerateViewXml(string name, XElement definitionXml)
	{
		return new XElement("view", new XAttribute("id", Guid.NewGuid().ToString().ToUpper(CultureInfo.InvariantCulture)), new XAttribute("displayname", name), new XAttribute("shortcut", ""), new XAttribute("smartClientId", Guid.NewGuid().ToString().ToUpper(CultureInfo.InvariantCulture)), new XElement("properties", new XElement("property", new XAttribute("name", "LayoutProperty"), new XAttribute("value", definitionXml.ToString())))).ToString();
	}
}
