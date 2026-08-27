using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Web.Administration;

namespace VideoOS.Server.Admin.Utilities;

public class VirtualDirectory
{
	private string _path;

	public string HttpRedirect { get; set; }

	public string AppPoolId { get; }

	public string AnonymousUserPass { get; set; }

	public string PhysicalPath
	{
		get
		{
			return _path;
		}
		set
		{
			_path = value;
			if (_path != null && _path.Length > 0 && !Directory.Exists(_path))
			{
				Directory.CreateDirectory(_path);
			}
		}
	}

	public bool EnableDefaultDoc { get; set; }

	public string DefaultDoc { get; set; }

	public AccessRightTypes AccessRights { get; set; }

	public AuthorizationTypes Authorization { get; set; }

	public SslFlags SslSettings { get; set; }

	public bool EnableDirectoryBrowsing { get; set; }

	public IEnumerable<HandlerMapping> ScriptMapsToAdd { get; set; }

	public IEnumerable<HandlerMapping> ScriptMapsToRemove { get; set; }

	public string VirtualLocation { get; }

	public static VirtualDirectory Open(string websiteId, string virtualDirectoryName)
	{
		using ServerManager serverManager = new ServerManager();
		ConfigurationElement item = Iis7ConfigHelpers.FindVirtualDirectoryConfiguration(Iis7ConfigHelpers.GetSite(websiteId, serverManager).Name, virtualDirectoryName, serverManager).virtualDirectory;
		if (item == null)
		{
			return null;
		}
		VirtualDirectory virtualDirectory = new VirtualDirectory(websiteId, null, virtualDirectoryName, null);
		virtualDirectory.PopulateProperties(item, serverManager);
		return virtualDirectory;
	}

	public VirtualDirectory(string websiteId, string appPoolId, string virtualApplicationPath, string physicalPath)
	{
		AppPoolId = appPoolId;
		virtualApplicationPath = virtualApplicationPath?.Replace("\\", "/") ?? throw new ArgumentNullException("virtualApplicationPath");
		InitializeDefaultVariableValues();
		using ServerManager serverManager = new ServerManager();
		string name = Iis7ConfigHelpers.GetSite(websiteId, serverManager).Name;
		VirtualLocation = name + "/" + virtualApplicationPath;
		ConfigurationElement siteConfiguration = Iis7ConfigHelpers.GetSiteConfiguration(name, serverManager);
		if (siteConfiguration == null)
		{
			throw new InvalidOperationException("Element not found!");
		}
		var (configurationElement, configurationElement2) = Iis7ConfigHelpers.FindVirtualDirectoryConfiguration(siteConfiguration, virtualApplicationPath);
		if (configurationElement == null)
		{
			(configurationElement, configurationElement2) = Iis7ConfigHelpers.CreateVirtualDirectory(siteConfiguration, virtualApplicationPath);
		}
		if (!string.IsNullOrEmpty(physicalPath))
		{
			PhysicalPath = physicalPath;
			configurationElement.SetAttributeValue(AttributeName.PhysicalPath, physicalPath);
		}
		if (!string.IsNullOrEmpty(appPoolId))
		{
			configurationElement2.SetAttributeValue(AttributeName.ApplicationPool, appPoolId);
		}
		serverManager.CommitChanges();
	}

	private void InitializeDefaultVariableValues()
	{
		AccessRights = AccessRightTypes.Read | AccessRightTypes.Script;
		Authorization = AuthorizationTypes.None;
	}

	private void PopulateProperties(ConfigurationElement virtualDirectoryConfiguration, ServerManager serverManager)
	{
		HostSection hostSection = new HostSection(serverManager, VirtualLocation);
		WebSection webSection = new WebSection(serverManager, VirtualLocation);
		AnonymousUserPass = hostSection.AnonymousAuthentication.GetAttributeValue(AttributeName.Password).ToString();
		PhysicalPath = virtualDirectoryConfiguration.GetAttributeValue(AttributeName.PhysicalPath).ToString();
		EnableDefaultDoc = (bool)webSection.DefaultDocument.GetAttributeValue(AttributeName.Enabled);
		DefaultDoc = ((!EnableDefaultDoc) ? null : webSection.DefaultDocument.GetCollection()?.FirstOrDefault()?.ToString());
		AccessPolicy value = (AccessPolicy)webSection.Handlers.GetAttributeValue(AttributeName.AccessPolicy);
		AccessRights = AccessRightTypes.None;
		if (FlagIsSet(value, AccessPolicy.Read))
		{
			AccessRights |= AccessRightTypes.Read;
		}
		if (FlagIsSet(value, AccessPolicy.Execute))
		{
			AccessRights |= AccessRightTypes.Execute;
		}
		if (FlagIsSet(value, AccessPolicy.Write))
		{
			AccessRights |= AccessRightTypes.Write;
		}
		if (FlagIsSet(value, AccessPolicy.Script))
		{
			AccessRights |= AccessRightTypes.Script;
		}
		SslSettings = (SslFlags)hostSection.Access.GetAttributeValue(AttributeName.SslFlags);
		Authorization = AuthorizationTypes.None;
		if ((bool)hostSection.BasicAuthentication.GetAttributeValue(AttributeName.Enabled))
		{
			Authorization |= AuthorizationTypes.Basic;
		}
		if ((bool)hostSection.WindowsAuthentication.GetAttributeValue(AttributeName.Enabled))
		{
			Authorization |= AuthorizationTypes.Ntlm;
		}
		if ((bool)hostSection.AnonymousAuthentication.GetAttributeValue(AttributeName.Enabled))
		{
			Authorization |= AuthorizationTypes.Anonymous;
		}
		if ((bool)hostSection.DigestAuthentication.GetAttributeValue(AttributeName.Enabled))
		{
			Authorization |= AuthorizationTypes.Digest;
		}
		bool flag = (bool)webSection.HttpRedirect.GetAttributeValue(AttributeName.Enabled);
		HttpRedirect = (flag ? webSection.HttpRedirect.GetAttributeValue(AttributeName.Destination).ToString() : null);
	}

	private static bool FlagIsSet<T>(T value, T flag)
	{
		int num = (int)(object)value;
		int num2 = (int)(object)flag;
		return (num & num2) == num2;
	}

	public void CommitChanges()
	{
		using ServerManager serverManager = new ServerManager();
		HostSection hostSection = new HostSection(serverManager, VirtualLocation);
		WebSection webSection = new WebSection(serverManager, VirtualLocation);
		if (!string.IsNullOrEmpty(AnonymousUserPass))
		{
			hostSection.AnonymousAuthentication.SetAttributeValue(AttributeName.Password, AnonymousUserPass);
		}
		hostSection.AnonymousAuthentication.SetAttributeValue(AttributeName.Enabled, (Authorization & AuthorizationTypes.Anonymous) == AuthorizationTypes.Anonymous);
		SetDefaultDoc(webSection, DefaultDoc);
		webSection.Handlers.SetAttributeValue(AttributeName.AccessPolicy, GetAccessPolicy(AccessRights));
		hostSection.Access.SetAttributeValue(AttributeName.SslFlags, SslSettings);
		hostSection.BasicAuthentication.SetAttributeValue(AttributeName.Enabled, (Authorization & AuthorizationTypes.Basic) == AuthorizationTypes.Basic);
		hostSection.WindowsAuthentication.SetAttributeValue(AttributeName.Enabled, (Authorization & AuthorizationTypes.Ntlm) == AuthorizationTypes.Ntlm);
		webSection.DefaultDocument.SetAttributeValue(AttributeName.Enabled, EnableDefaultDoc);
		webSection.DirectoryBrowse.SetAttributeValue(AttributeName.Enabled, EnableDirectoryBrowsing);
		hostSection.DigestAuthentication.SetAttributeValue(AttributeName.Enabled, (Authorization & AuthorizationTypes.Digest) == AuthorizationTypes.Digest);
		SetHttpRedirect(webSection, HttpRedirect);
		IEnumerable<HandlerMapping> scriptMapsToAdd = ScriptMapsToAdd;
		if (scriptMapsToAdd != null && scriptMapsToAdd.Any())
		{
			ConfigurationElementCollection collection = webSection.Handlers.GetCollection();
			foreach (HandlerMapping item in ScriptMapsToAdd)
			{
				SetHandlerMapping(collection, item);
			}
		}
		IEnumerable<HandlerMapping> scriptMapsToRemove = ScriptMapsToRemove;
		if (scriptMapsToRemove != null && scriptMapsToRemove.Any())
		{
			ConfigurationElementCollection collection2 = webSection.Handlers.GetCollection();
			foreach (HandlerMapping item2 in ScriptMapsToRemove)
			{
				RemoveHandlerMapping(collection2, item2);
			}
		}
		serverManager.CommitChanges();
	}

	private static void SetHandlerMapping(ConfigurationElementCollection handlersCollection, HandlerMapping mapping)
	{
		if (!handlersCollection.Any((ConfigurationElement x) => ((string)x.GetAttributeValue(AttributeName.Name)).Equals(mapping.Name, StringComparison.OrdinalIgnoreCase)))
		{
			ConfigurationElement configurationElement = handlersCollection.CreateElement("add");
			configurationElement.SetAttributeValue(AttributeName.Name, mapping.Name);
			configurationElement.SetAttributeValue(AttributeName.Path, mapping.Path);
			configurationElement.SetAttributeValue(AttributeName.Verb, mapping.Verb);
			configurationElement.SetAttributeValue(AttributeName.Modules, mapping.Module);
			configurationElement.SetAttributeValue(AttributeName.ScriptProcessor, mapping.ScriptProcessor);
			configurationElement.SetAttributeValue(AttributeName.RequireAccess, mapping.RequireAccess);
			configurationElement.SetAttributeValue(AttributeName.ResponseBufferLimit, mapping.ResponseBufferLimit);
			handlersCollection.AddAt(0, configurationElement);
		}
	}

	private static void RemoveHandlerMapping(ConfigurationElementCollection handlersCollection, HandlerMapping mapping)
	{
		ConfigurationElement configurationElement = handlersCollection.SingleOrDefault((ConfigurationElement x) => ((string)x[AttributeName.Name]).Equals(mapping.Name, StringComparison.OrdinalIgnoreCase)) ?? FindLegacyHandlerMapping(handlersCollection, mapping);
		if (configurationElement != null)
		{
			handlersCollection.Remove(configurationElement);
		}
	}

	private static ConfigurationElement FindLegacyHandlerMapping(ConfigurationElementCollection handlersCollection, HandlerMapping mapping)
	{
		return (from x in handlersCollection
			where ((string)x.GetAttributeValue(AttributeName.Path)).Equals(mapping.Path, StringComparison.OrdinalIgnoreCase) || ((string)x[AttributeName.Path]).Equals("*" + mapping.Path, StringComparison.OrdinalIgnoreCase)
			where ((string)x.GetAttributeValue(AttributeName.Name)).StartsWith("AboMapperCustom", StringComparison.OrdinalIgnoreCase)
			where ((string)x.GetAttributeValue(AttributeName.Verb)).Equals(mapping.Verb, StringComparison.OrdinalIgnoreCase)
			where ((string)x.GetAttributeValue(AttributeName.Modules)).Equals(mapping.Module, StringComparison.OrdinalIgnoreCase)
			where ((string)x.GetAttributeValue(AttributeName.ScriptProcessor)).Equals(mapping.ScriptProcessor, StringComparison.OrdinalIgnoreCase)
			select x).SingleOrDefault();
	}

	private static void SetHttpRedirect(WebSection webSection, string httpRedirect)
	{
		bool flag = !string.IsNullOrEmpty(httpRedirect);
		webSection.HttpRedirect.SetAttributeValue(AttributeName.Enabled, flag);
		if (flag)
		{
			webSection.HttpRedirect.SetAttributeValue(AttributeName.Destination, httpRedirect);
		}
	}

	private static string GetAccessPolicy(AccessRightTypes accessRights)
	{
		List<string> list = new List<string>();
		if ((accessRights & AccessRightTypes.Execute) == AccessRightTypes.Execute)
		{
			list.Add(VideoOS.Server.Admin.Utilities.AccessRights.Execute);
		}
		if ((accessRights & AccessRightTypes.Read) == AccessRightTypes.Read)
		{
			list.Add(VideoOS.Server.Admin.Utilities.AccessRights.Read);
		}
		if ((accessRights & AccessRightTypes.Script) == AccessRightTypes.Script)
		{
			list.Add(VideoOS.Server.Admin.Utilities.AccessRights.Script);
		}
		if ((accessRights & AccessRightTypes.Write) == AccessRightTypes.Write)
		{
			list.Add(VideoOS.Server.Admin.Utilities.AccessRights.Write);
		}
		if (list.Any())
		{
			return string.Join(", ", list);
		}
		return "None";
	}

	private static void SetDefaultDoc(WebSection webSection, string defaultDoc)
	{
		if (!string.IsNullOrEmpty(defaultDoc))
		{
			ConfigurationElementCollection collection = webSection.DefaultDocument.GetCollection(CollectionName.Files);
			collection.Clear();
			ConfigurationElement configurationElement = collection.CreateElement(ElementTag.Add);
			configurationElement.SetAttributeValue(AttributeName.Value, defaultDoc);
			collection.AddAt(0, configurationElement);
		}
	}

	public static void DeleteVirtualDirectory(string websiteId, string webPath)
	{
		using ServerManager serverManager = new ServerManager();
		Site site = Iis7ConfigHelpers.GetSite(websiteId, serverManager);
		string locationPath = site.Name + "/" + webPath;
		ConfigurationElement siteConfiguration = Iis7ConfigHelpers.GetSiteConfiguration(site.Name, serverManager);
		if (siteConfiguration == null)
		{
			throw new InvalidOperationException("Element not found!");
		}
		var (configurationElement, parentApplication) = Iis7ConfigHelpers.FindVirtualDirectoryConfiguration(siteConfiguration, webPath);
		if (configurationElement != null)
		{
			Iis7ConfigHelpers.DeleteVirtualDirectory(siteConfiguration, parentApplication);
		}
		serverManager.GetApplicationHostConfiguration().RemoveLocationPath(locationPath);
		serverManager.CommitChanges();
	}
}
