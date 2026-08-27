using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public class License
{
	public static Guid MipPluginsLicenseId { get; } = new Guid("B1CB8385-EC7E-492B-B046-1B84E378CB71");

	private static LicenseRow ValidateAndMergeMipPlugins(ILicenseComponent component, string clientData)
	{
		if (!(component.QueryLicenseItemById(MipPluginsLicenseId) is LicenseRow licenseRow))
		{
			return new LicenseRow
			{
				Id = MipPluginsLicenseId,
				DataAsString = clientData,
				LastModified = DateTime.UtcNow
			};
		}
		try
		{
			string dataAsString = licenseRow.DataAsString;
			XDocument xDocument = XDocument.Parse(clientData);
			XDocument xDocument2 = XDocument.Parse(dataAsString);
			HashSet<string> hashSet = new HashSet<string>();
			foreach (XElement item in xDocument.Root.Descendants("plugin"))
			{
				if (item.Element("pluginid") == null)
				{
					throw new InvalidMipLicenseRequestPartException("MIP plugin license request does not contain 'plugin id'.");
				}
				if (item.Element("licensetype") == null)
				{
					throw new InvalidMipLicenseRequestPartException("MIP plugin license request does not contain 'license type' element.");
				}
				string pluginId = item.Element("pluginid").Value;
				string licenseType = item.Element("licensetype").Value;
				if (!hashSet.Contains(pluginId))
				{
					int num = xDocument.Root.Descendants("plugin").Count((XElement x) => x.Element("pluginid").Value == pluginId);
					int num2 = xDocument2.Root.Descendants("plugin").Count((XElement x) => x.Element("pluginid").Value == pluginId);
					if (num2 > num)
					{
						string text = ((item.Element("name") != null) ? item.Element("name").Value : "");
						GeminiTrace.Warning("After the update of the license request for MIP Plugin '{0}' with plugin id '{1}', number of required licenses decreased from '{2}' to '{3}'.", text, pluginId, num2, num);
					}
					hashSet.Add(pluginId);
				}
				IEnumerable<XElement> enumerable = from x in xDocument2.Root.Descendants("plugin")
					where x.Element("pluginid").Value == pluginId && x.Element("licensetype").Value == licenseType
					select x;
				if (enumerable != null)
				{
					if (enumerable.Count() > 1)
					{
						GeminiTrace.Warning("Multiple MIP plugin license requests found for plugin with Id '{0}' and license type '{1}'.", pluginId, licenseType);
					}
					foreach (XElement item2 in new List<XElement>(enumerable))
					{
						item2.Remove();
					}
				}
				xDocument2.Root.Add(item);
			}
			return new LicenseRow
			{
				Id = MipPluginsLicenseId,
				DataAsString = xDocument2.Declaration.ToString() + xDocument2.ToString(),
				LastModified = licenseRow.LastModified
			};
		}
		catch (XmlException ex)
		{
			throw new InvalidMipLicenseRequestPartException(ex.Message);
		}
	}

	private static void AddOrUpdateLicenseWithRetries(IGeminiRequestContext requestContext, Func<LicenseRow> updateFunc)
	{
		int num = 5;
		while (num > 0)
		{
			using ILicenseComponent licenseComponent = requestContext.CreateLicenseComponent();
			LicenseRow licenseRow = updateFunc();
			try
			{
				licenseComponent.SafelyAddOrUpdateLicenseItem(licenseRow.Id, licenseRow.LastModified, licenseRow.Data);
				break;
			}
			catch (LicenseNotRecentVersionException)
			{
				num--;
				if (num == 0)
				{
					throw;
				}
				Thread.Sleep(50);
			}
		}
	}

	public static void UpdateMipPluginsLicenseRequestPart(IGeminiRequestContext requestContext, string data)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		if (string.IsNullOrEmpty(data))
		{
			throw new ArgumentException("Value cannot be an empty string.", "data");
		}
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.ManagementServer, "GENERIC_WRITE", "/");
		Func<LicenseRow> updateFunc = delegate
		{
			using ILicenseComponent component = requestContext.CreateLicenseComponent();
			return ValidateAndMergeMipPlugins(component, data);
		};
		AddOrUpdateLicenseWithRetries(requestContext, updateFunc);
	}
}
