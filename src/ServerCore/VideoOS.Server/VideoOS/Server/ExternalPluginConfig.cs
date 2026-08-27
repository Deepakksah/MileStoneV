#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Xml;
using Microsoft.Win32;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Drivers;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.Server.Properties;

namespace VideoOS.Server;

public static class ExternalPluginConfig
{
	private static Server _server;

	private static string _pluginsPath;

	private static FileSystemWatcher _fileWatcher;

	public static void Initialize(Server server)
	{
		_server = server;
		_pluginsPath = GetExternalPluginConfigsPath();
		ReadExternalPluginConfigs(_pluginsPath);
		_fileWatcher = new FileSystemWatcher(_pluginsPath);
		_fileWatcher.Filter = "*.xml";
		_fileWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.LastAccess;
		_fileWatcher.Changed += OnChanged;
		_fileWatcher.Created += OnChanged;
		_fileWatcher.Deleted += OnChanged;
		_fileWatcher.Renamed += OnRenamed;
		_fileWatcher.EnableRaisingEvents = true;
	}

	private static void OnChanged(object sender, FileSystemEventArgs e)
	{
		Trace.WriteLine(string.Format(CultureInfo.InvariantCulture, "File changed: {0}", e.Name));
		try
		{
			ReadExternalPluginConfigs(_pluginsPath);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
		}
	}

	private static void OnRenamed(object sender, RenamedEventArgs e)
	{
		Trace.WriteLine(string.Format(CultureInfo.InvariantCulture, "File renamed: {0}", e.Name));
		try
		{
			ReadExternalPluginConfigs(_pluginsPath);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
		}
	}

	private static string GetExternalPluginConfigsPath()
	{
		string text = null;
		try
		{
			using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\VideoOS\\Server\\Installation", writable: false))
			{
				try
				{
					if (registryKey != null)
					{
						text = registryKey.GetValue("ExternalPluginPath") as string;
					}
					if (text == null)
					{
						text = Path.GetFullPath(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ExternalPlugins")));
					}
				}
				finally
				{
					try
					{
						registryKey?.Close();
					}
					catch
					{
					}
				}
			}
			if (Directory.Exists(text))
			{
				return text;
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
		}
		return null;
	}

	private static bool ReadExternalPluginConfigs(string pluginCfgDirectory)
	{
		bool flag = false;
		Dictionary<Guid, int> dictionary = new Dictionary<Guid, int>();
		if (pluginCfgDirectory == null)
		{
			throw new Exception(Resources.ExternalPluginConfigDirNull);
		}
		ICallContextInfo callContextInfo = Server.Instance.CreateServerServiceCallContextUserInfo();
		DirectoryInfo directoryInfo = new DirectoryInfo(pluginCfgDirectory);
		if (directoryInfo.Exists)
		{
			_server.Data.ConfigurationDataSetCacheLock.EnterWriteLock();
			try
			{
				using ConfigurationManager configurationManager = new ConfigurationManager(new ConfigurationDataSetLoader(_server.Data));
				ExternalPlugin externalPlugin = configurationManager.ExternalPlugins[GlobalIDs.ImmerVisionPlugInGuid];
				if (externalPlugin != null)
				{
					dictionary[externalPlugin.PluginID] = externalPlugin.Data.GetHashCode();
				}
				configurationManager.ExternalPlugins.Clear();
				ConfigurationDataSet changes = configurationManager.Dataset.GetChanges();
				_server.Data.WriteConfigurationDataSetChangesToDatabase(changes, callContextInfo);
				ILicenseManagerCache requiredService = ServiceContainer.Instance.GetRequiredService<ILicenseManagerCache>();
				requiredService.ClearAllCaches();
			}
			finally
			{
				_server.Data.ConfigurationDataSetCacheLock.ExitWriteLock();
			}
			FileInfo[] files = directoryInfo.GetFiles("*.xml");
			FileInfo[] array = files;
			foreach (FileInfo fileInfo in array)
			{
				Trace.WriteLine("Reading plugin config from: " + fileInfo.FullName);
				flag |= ParseExternalPluginsConfig(fileInfo.FullName, dictionary, callContextInfo);
			}
			using ConfigurationManager configurationManager2 = new ConfigurationManager(new ConfigurationDataSetLoader(_server.Data));
			foreach (Guid key in dictionary.Keys)
			{
				if (!configurationManager2.ExternalPlugins.Contains(key))
				{
					ServerLogManager.Instance.AddLogEntry(LogEntryId.ExternalPlugIn360LensDefinitionsRemoved);
					break;
				}
			}
		}
		return flag;
	}

	private static bool ParseExternalPluginsConfig(string configFile, Dictionary<Guid, int> previousPlugInHashes, ICallContextInfo callContextInfo)
	{
		XmlDocument xmlDocument = null;
		XmlTextReader xmlTextReader = null;
		bool result = false;
		try
		{
			xmlDocument = new XmlDocument();
			FileStream input = DelayLoad(configFile, 500, 10);
			xmlTextReader = new XmlTextReader(input);
			xmlDocument.Load(xmlTextReader);
			XmlNode documentElement = xmlDocument.DocumentElement;
			XmlElement xmlElement = documentElement["pluginconfigurations"];
			if (xmlElement != null)
			{
				_server.Data.ConfigurationDataSetCacheLock.EnterWriteLock();
				try
				{
					using ConfigurationManager configurationManager = new ConfigurationManager(new ConfigurationDataSetLoader(_server.Data));
					XmlNodeList xmlNodeList = xmlElement.SelectNodes("descendant::pluginconfiguration[attribute::id]");
					foreach (XmlNode item in xmlNodeList)
					{
						string value = item.Attributes["id"].Value;
						if (string.IsNullOrEmpty(value) || !value.IsGuid())
						{
							continue;
						}
						Guid guid = new Guid(value);
						if (!configurationManager.ExternalPlugins.Contains(guid))
						{
							string outerXml = item.OuterXml;
							configurationManager.ExternalPlugins.Add(guid, documentElement.Name.Trim().ToUpper(CultureInfo.InvariantCulture), outerXml);
							if (GlobalIDs.ImmerVisionPlugInGuid.Equals(guid))
							{
								if (previousPlugInHashes.TryGetValue(guid, out var value2))
								{
									if (!value2.Equals(outerXml.GetHashCode()))
									{
										ServerLogManager.Instance.AddLogEntry(LogEntryId.ExternalPlugIn360LensDefinitionsChanged);
									}
								}
								else
								{
									ServerLogManager.Instance.AddLogEntry(LogEntryId.ExternalPlugIn360LensDefinitionsAdded);
								}
							}
						}
						else
						{
							Trace.WriteLine(" Duplicate plugin id found!:");
						}
						result = true;
					}
					ConfigurationDataSet changes = configurationManager.Dataset.GetChanges();
					_server.Data.WriteConfigurationDataSetChangesToDatabase(changes, callContextInfo);
					ILicenseManagerCache requiredService = ServiceContainer.Instance.GetRequiredService<ILicenseManagerCache>();
					requiredService.ClearAllCaches();
				}
				finally
				{
					_server.Data.ConfigurationDataSetCacheLock.ExitWriteLock();
				}
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Trace.WriteLine(ex2.Message);
		}
		finally
		{
			xmlTextReader?.Close();
		}
		return result;
	}

	private static FileStream DelayLoad(string fileName, int milliseconds, int numberOfRetries)
	{
		Thread.Sleep(milliseconds);
		for (int i = 0; i < numberOfRetries - 1; i++)
		{
			try
			{
				return File.OpenRead(fileName);
			}
			catch (IOException)
			{
				Thread.Sleep(milliseconds);
			}
		}
		return File.OpenRead(fileName);
	}
}
