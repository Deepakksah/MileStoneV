using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.Win32;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI;
using VideoOS.Platform.Util.GisMap;
using VideoOS.Platform.Util.VideoWall;

namespace VideoOS.Platform.Util;

public static class PluginManager
{
	private static ArrayList _list;

	private static Hashtable _itemNodeHashtable;

	private static Hashtable _kindToPluginDefinition;

	private static AppDomain _domain;

	private const string InstallRegistryLocation = "SOFTWARE\\Milestone\\MIP";

	private static List<string> _folderList;

	private static readonly HashSet<Guid> PluginsToExclude;

	private static Hashtable _alreadyUnresolvedAndNotFoundDlls;

	private static string _previousAssembly;

	private static bool _pluginInitCalled;

	private static HashSet<Guid> _unknownKinds;

	static PluginManager()
	{
		_itemNodeHashtable = new Hashtable();
		_kindToPluginDefinition = new Hashtable();
		_folderList = new List<string>();
		PluginsToExclude = new HashSet<Guid>
		{
			new Guid("5E0119E3-37A6-413B-907C-08DF9C84525E"),
			new Guid("B364E0D4-63B9-41CF-9336-0A4BAC8505B4"),
			new Guid("71E86D1A-30EE-4D58-8AD6-2CF7245DDA9F"),
			new Guid("FF9DE88E-45F2-4B1E-A551-30AD84E25102"),
			new Guid("A4A339FD-A4A6-4DA7-88C9-5D07AAE61CCA"),
			new Guid("6F714690-E09F-4D6A-A0CA-9F913CA125E8"),
			new Guid("dd258552-403d-47cc-a486-d3eccd93af9e"),
			new Guid("9F6E7E9D-3FD2-49B2-9CA0-F7E7F8F8C3A7"),
			new Guid("A1C400F7-C61E-46FF-B8A9-F268EE3DD20C"),
			new Guid("7DA29D2F-04EF-4913-B886-FE148BC1A99C"),
			new Guid("FE9AA43F-508C-469F-93E7-94AB47F8E823"),
			new Guid("BB529DE8-608E-4EBB-B796-889504E54670"),
			new Guid("252aa581-4633-422b-8aed-7ccc72df5f19"),
			new Guid("46BB0392-FD06-465B-8776-B90AC16639B3"),
			new Guid("1a5f5c8f-815b-4c77-8f33-7ecabce8a431"),
			new Guid("0a55275c-0a8d-435e-9831-df3a4acd1df8"),
			new Guid("24A01849-A322-4AD3-8284-0EBCEB1A8036"),
			new Guid("B0650FF4-06BD-4076-9F30-BBA7DF90CEB4"),
			new Guid("32C415C6-6DDB-4931-AE35-E971590DD8B6"),
			new Guid("72A98A59-337A-4CE1-9CA2-32F531E778E4"),
			new Guid("A20D8523-547B-4BB3-B5AA-7899C62C68F6"),
			new Guid("6053b0b4-3e97-4a86-8e46-d1a9ce807ed1"),
			new Guid("e08321c8-e252-487f-9059-0a7e1514b1f5")
		};
		_alreadyUnresolvedAndNotFoundDlls = new Hashtable();
		_previousAssembly = "";
		_pluginInitCalled = false;
		_unknownKinds = new HashSet<Guid>();
		_list = new ArrayList();
		if (EnvironmentManager.Instance != null)
		{
			_list.Add(new VideoWallHelperPlugin());
		}
		_list.Add(new GisMapLocationHelperPlugin());
	}

	public static void Initialize()
	{
		Initialize(loadPluginsFromGlobalFolders: true);
	}

	public static void Initialize(bool loadPluginsFromGlobalFolders)
	{
		_domain = AppDomain.CurrentDomain;
		AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
		string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		_list.AddRange(LoadFromFolder(Path.Combine(directoryName, "MIPPlugins"), typeof(PluginDefinition)));
		foreach (PluginDefinition item2 in _list)
		{
			LogPluginPath(item2);
		}
		if (loadPluginsFromGlobalFolders)
		{
			InitializeMore(ReadRegistryValue("Path", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Milestone\\MIPPlugins")));
			InitializeMore(ReadRegistryValue("Path", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "VideoOS\\MIPPlugins")));
		}
		if (EnvironmentManager.Instance != null && EnvironmentManager.Instance.EnvironmentOptions.ContainsKey("CompanyNameFolder"))
		{
			string text = EnvironmentManager.Instance.EnvironmentOptions["CompanyNameFolder"];
			if (text != "Milestone")
			{
				InitializeMore(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), text + "\\MIPPlugins"));
			}
		}
		string fullName = Assembly.GetExecutingAssembly().FullName;
		int num = fullName.LastIndexOf('\\');
		if (num > 0)
		{
			string item = fullName.Substring(0, num + 1);
			_folderList.Add(item);
		}
		_list.Add(new ServiceHelperPlugin());
	}

	public static void AddEmbeddedPlugins(List<PluginDefinition> plugins)
	{
		_list.AddRange(plugins);
	}

	public static void InitializeMore(string path)
	{
		if (path == null)
		{
			return;
		}
		_domain = AppDomain.CurrentDomain;
		try
		{
			foreach (PluginDefinition item in LoadFromFolder(path, typeof(PluginDefinition)))
			{
				bool flag = false;
				foreach (PluginDefinition item2 in _list)
				{
					if (item2.Id == item.Id)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					_list.Add(item);
					LogPluginPath(item);
				}
			}
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: true, "PluginManager", "Unable to load plugins from path:" + path + "  -  " + ex.Message);
		}
	}

	public static void InitializeOnly(string path)
	{
		_domain = AppDomain.CurrentDomain;
		AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
		_list = new ArrayList();
		_list.AddRange(LoadFromFolder(path, typeof(PluginDefinition)));
		foreach (PluginDefinition item in _list)
		{
			LogPluginPath(item);
		}
	}

	public static void PluginInitCheckForStandalone()
	{
		if (EnvironmentManager.Instance.EnvironmentType != EnvironmentType.Standalone || EnvironmentManager.UsingUserContext || _pluginInitCalled)
		{
			return;
		}
		_pluginInitCalled = true;
		foreach (PluginDefinition item in _list)
		{
			try
			{
				item.Init();
				if (item.ItemNodes == null)
				{
					continue;
				}
				foreach (ItemNode itemNode in item.ItemNodes)
				{
					if (itemNode.ItemManager != null)
					{
						itemNode.ItemManager.Init();
					}
				}
			}
			catch (Exception)
			{
			}
		}
		RebuildFromPlugins();
	}

	public static void PluginCloseCheckForStandalone()
	{
		if (!_pluginInitCalled)
		{
			return;
		}
		_pluginInitCalled = false;
		foreach (PluginDefinition item in _list)
		{
			try
			{
				if (item.ItemNodes != null)
				{
					foreach (ItemNode itemNode in item.ItemNodes)
					{
						if (itemNode.ItemManager != null)
						{
							itemNode.ItemManager.Close();
						}
					}
				}
				item.Close();
			}
			catch (Exception)
			{
			}
		}
	}

	private static void AfterPluginDefinitionInit(Guid missingGuid)
	{
		if (!VideoOS.Platform.UI.Util.IsBuiltInKind(missingGuid))
		{
			RebuildFromPlugins();
		}
	}

	public static void RebuildFromPlugins()
	{
		if (!EnvironmentManager.Instance.PluginInitializationInProgress)
		{
			BuildIndexForPlugins();
			VideoOS.Platform.UI.Util.ReInit();
		}
	}

	private static void LogPluginPath(PluginDefinition pd)
	{
		EnvironmentManager.Instance.Log(error: false, "PluginLoader", pd.Name + ", loaded from:" + pd.GetType().Assembly.Location);
	}

	private static Assembly LookupNonContiguousSatelliteAssemblyResolution(ResolveEventArgs args)
	{
		string text = "languageValue";
		char c = ',';
		string text2 = args.Name.Split(c).First();
		Regex regex = new Regex(string.Format(CultureInfo.InvariantCulture, "(?<languageLabel>Culture=)(?<{0}>.*-*.),", text));
		string text3 = text2.Replace(".resources", string.Empty);
		Match match = regex.Match(args.Name);
		if (match.Success)
		{
			string value = match.Groups[text].Value;
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies)
			{
				if (!(text3 == assembly.FullName.Split(c).First()))
				{
					continue;
				}
				string directoryName = Path.GetDirectoryName(assembly.Location);
				if (!string.IsNullOrWhiteSpace(directoryName))
				{
					string path = Path.Combine(directoryName, value, text2) + ".dll";
					if (File.Exists(path))
					{
						return Assembly.LoadFile(path);
					}
				}
			}
		}
		return null;
	}

	private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
	{
		string text = args.Name.Split(',')[0];
		if (text.EndsWith(".resources", StringComparison.Ordinal))
		{
			return LookupNonContiguousSatelliteAssemblyResolution(args);
		}
		if (_previousAssembly == text)
		{
			return null;
		}
		_previousAssembly = text;
		if (_alreadyUnresolvedAndNotFoundDlls[text] == null)
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies)
			{
				if (text == assembly.FullName.Split(',')[0])
				{
					break;
				}
			}
			foreach (string folder in _folderList)
			{
				Assembly assembly2 = CheckDirectory(folder, text);
				if (assembly2 != null)
				{
					return assembly2;
				}
			}
			_alreadyUnresolvedAndNotFoundDlls[text] = "NotFound";
		}
		return null;
	}

	private static Assembly CheckDirectory(string dir, string assemblyName)
	{
		if (Directory.Exists(dir))
		{
			string[] files = Directory.GetFiles(dir);
			foreach (string path in files)
			{
				string fileName = Path.GetFileName(path);
				if (assemblyName.ToLower(CultureInfo.InvariantCulture) + ".dll" == fileName.ToLower(CultureInfo.InvariantCulture))
				{
					return Assembly.LoadFile(path);
				}
			}
		}
		return null;
	}

	public static void UnloadPluginDefinition(PluginDefinition pluginDefinition)
	{
		lock (_list)
		{
			if (_list.Contains(pluginDefinition))
			{
				_list.Remove(pluginDefinition);
			}
		}
	}

	public static string ReadRegistryValue(string propertyName, string defaultValue)
	{
		string result = defaultValue;
		try
		{
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Milestone\\MIP");
			if (registryKey != null)
			{
				result = (string)registryKey.GetValue(propertyName);
			}
		}
		catch (Exception)
		{
		}
		return result;
	}

	public static ArrayList GetPluginDefinitions()
	{
		return _list;
	}

	public static bool ShouldBeExcludedFromRegistration(Guid pluginId)
	{
		if (PluginsToExclude.Contains(pluginId))
		{
			return true;
		}
		return false;
	}

	public static PluginDefinition GetPluginDefinition(Guid id)
	{
		foreach (PluginDefinition item in _list)
		{
			if (item.Id == id)
			{
				return item;
			}
		}
		return null;
	}

	public static PluginDefinition GetViewItemPlugins(Guid clientId, out ViewItemPlugin viewItemPlugin)
	{
		lock (_list)
		{
			foreach (PluginDefinition item in _list)
			{
				if (item.ViewItemPlugins == null)
				{
					continue;
				}
				foreach (ViewItemPlugin viewItemPlugin2 in item.ViewItemPlugins)
				{
					if (viewItemPlugin2.Id == clientId)
					{
						viewItemPlugin = viewItemPlugin2;
						return item;
					}
				}
			}
		}
		viewItemPlugin = null;
		return null;
	}

	public static PluginDefinition GetPluginDefinitionFromKind(Guid kind)
	{
		if (_kindToPluginDefinition[kind] == null)
		{
			if (_unknownKinds.Contains(kind))
			{
				return null;
			}
			AfterPluginDefinitionInit(kind);
			if (_kindToPluginDefinition[kind] == null)
			{
				_unknownKinds.Add(kind);
				return null;
			}
		}
		return (PluginDefinition)_kindToPluginDefinition[kind];
	}

	public static ItemNode GetItemNode(Guid kind)
	{
		if (VideoOS.Platform.UI.Util.IsBuiltInKind(kind))
		{
			return null;
		}
		if (_itemNodeHashtable[kind] == null)
		{
			AfterPluginDefinitionInit(kind);
		}
		return (ItemNode)_itemNodeHashtable[kind];
	}

	public static bool PlatformDefinitionIdExists(Guid id)
	{
		foreach (PluginDefinition item in _list)
		{
			if (item.Id == id)
			{
				return true;
			}
		}
		return false;
	}

	public static bool PlatformCustomSettingsIdExists(Guid id)
	{
		foreach (PluginDefinition item in _list)
		{
			if (item.Id == id)
			{
				return true;
			}
			if (item.CustomSettingsGuids == null)
			{
				continue;
			}
			foreach (Guid customSettingsGuid in item.CustomSettingsGuids)
			{
				if (customSettingsGuid == id)
				{
					return true;
				}
			}
		}
		return false;
	}

	private static void BuildIndexForPlugins()
	{
		foreach (PluginDefinition item in _list)
		{
			try
			{
				if (item.ItemNodes == null)
				{
					continue;
				}
				foreach (ItemNode itemNode in item.ItemNodes)
				{
					BuildIndexEntry(itemNode, item);
				}
			}
			catch (Exception ex)
			{
				EnvironmentManager.Instance.Log(error: true, "PluginManager", item.Name + ":Unable to access ItemNodes:" + ex.Message);
			}
		}
	}

	private static void BuildIndexEntry(ItemNode ItemNode, PluginDefinition pluginDefinition)
	{
		_itemNodeHashtable[ItemNode.ItemKind] = ItemNode;
		_kindToPluginDefinition[ItemNode.ItemKind] = pluginDefinition;
		Kind.DefaultTypeToNameTable[ItemNode.ItemKind] = ItemNode.Name;
		List<ItemNode> itemNodes = ItemNode.ItemNodes;
		if (itemNodes == null)
		{
			return;
		}
		foreach (ItemNode item in itemNodes)
		{
			BuildIndexEntry(item, pluginDefinition);
		}
	}

	public static List<object> LoadCustomPlugins(Type pluginType, string pluginDefName)
	{
		List<object> list = new List<object>();
		string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		list.AddRange(LoadFromFolder(Path.Combine(directoryName, "MIPPlugins"), pluginType, pluginDefName));
		string folderPath = ReadRegistryValue("Path", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Milestone\\MIPPlugins"));
		list.AddRange(LoadFromFolder(folderPath, pluginType, pluginDefName));
		string folderPath2 = ReadRegistryValue("Path", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "VideoOS\\MIPPlugins"));
		list.AddRange(LoadFromFolder(folderPath2, pluginType, pluginDefName));
		if (EnvironmentManager.Instance != null && EnvironmentManager.Instance.EnvironmentOptions.ContainsKey("CompanyNameFolder"))
		{
			string text = EnvironmentManager.Instance.EnvironmentOptions["CompanyNameFolder"];
			if (text != "Milestone")
			{
				string folderPath3 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), text + "\\MIPPlugins");
				list.AddRange(LoadFromFolder(folderPath3, pluginType, pluginDefName));
			}
		}
		return list;
	}

	public static List<object> LoadFromFolder(string folderPath, Type pluginInterfaceType, string pluginDefName = "plugin.def")
	{
		List<object> list = new List<object>();
		List<string> list2 = new List<string>();
		try
		{
			string value = ((EnvironmentManager.Instance.EnvironmentType != EnvironmentType.SmartClient) ? EnvironmentManager.Instance.EnvironmentType.ToString().ToLower(CultureInfo.InvariantCulture) : "client");
			if (Directory.Exists(folderPath))
			{
				string text = Path.Combine(folderPath, pluginDefName);
				if (File.Exists(text))
				{
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.Load(text);
					bool flag = true;
					XmlNode xmlNode = xmlDocument.SelectSingleNode("plugin/load/@env");
					if (xmlNode != null)
					{
						flag = xmlNode.InnerText.ToLowerInvariant().Contains(value);
					}
					if (flag)
					{
						foreach (XmlNode item in xmlDocument.SelectNodes("plugin/file/@name"))
						{
							list2.Add(Path.Combine(folderPath, item.Value));
						}
					}
				}
				foreach (string item2 in list2)
				{
					List<object> list3 = FindPlugins(item2, pluginInterfaceType);
					if (list3 != null)
					{
						list.AddRange(list3);
					}
				}
				string[] directories = Directory.GetDirectories(folderPath);
				for (int i = 0; i < directories.Length; i++)
				{
					List<object> list4 = LoadFromFolder(directories[i], pluginInterfaceType, pluginDefName);
					if (list4 != null)
					{
						list.AddRange(list4);
					}
				}
			}
		}
		catch (DirectoryNotFoundException)
		{
		}
		return list;
	}

	public static List<object> FindPlugins(string fileName, Type typeToLoad)
	{
		List<object> list = new List<object>();
		string nameOfClass = fileName;
		try
		{
			Type[] types = Assembly.LoadFrom(fileName).GetTypes();
			foreach (Type type in types)
			{
				nameOfClass = type.FullName;
				if (!type.IsClass || type.IsAbstract || !typeToLoad.IsAssignableFrom(type))
				{
					continue;
				}
				ObjectHandle objectHandle = Activator.CreateInstanceFrom(fileName, type.ToString());
				if (objectHandle == null)
				{
					continue;
				}
				object obj = objectHandle.Unwrap();
				if (obj != null)
				{
					list.Add(obj);
					int num = fileName.LastIndexOf('\\');
					if (num > 0)
					{
						string item = fileName.Substring(0, num + 1);
						_folderList.Add(item);
					}
				}
			}
		}
		catch (Exception ex)
		{
			if (fileName != null)
			{
				int num2 = fileName.LastIndexOf("\\");
				nameOfClass = ((num2 >= 0) ? fileName.Substring(num2 + 1) : fileName);
			}
			ReflectionTypeLoadException ex2 = ex as ReflectionTypeLoadException;
			if (EnvironmentManager.Instance != null)
			{
				if (ex2 != null && ex2.LoaderExceptions != null && ex2.LoaderExceptions.Length != 0)
				{
					EnvironmentManager.Instance.ExceptionHandler(nameOfClass, "MIPPluginLoad", new Exception(ex2.LoaderExceptions[0].Message, ex2.LoaderExceptions[0]));
				}
				else
				{
					EnvironmentManager.Instance.ExceptionHandler(nameOfClass, "MIPPluginLoad", ex);
				}
			}
			if (ex2 != null && ex2.LoaderExceptions != null)
			{
				Exception[] loaderExceptions = ex2.LoaderExceptions;
				foreach (Exception ex3 in loaderExceptions)
				{
					if (EnvironmentManager.Instance != null)
					{
						EnvironmentManager.Instance.Log(error: true, "MIPPluginLoad", ex3.Message, null);
					}
				}
			}
		}
		return list;
	}
}
