using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using VideoOS.Common.Integration.Drivers;
using VideoOS.Common.Integration.Plugins;
using VideoOS.Common.Interfaces.Recorder;

namespace VideoOS.Common.Integration;

public class PluginFactory : IPluginFactory
{
	private string _pluginsPath = "./Plugins";

	private Hashtable _pluginInfos = new Hashtable();

	[ImportMany]
	private List<IDataSecurityExtensionManager> _DataSecurityExtensionManagers;

	[Import(typeof(IPtzExtension), AllowDefault = true)]
	private IPtzExtension _PtzExtension;

	public Hashtable PluginInfoTable => _pluginInfos;

	public string PluginsPath
	{
		get
		{
			return _pluginsPath;
		}
		set
		{
			_pluginsPath = value;
		}
	}

	public void Initialize(string serverAddress, int serverPort)
	{
		lock (this)
		{
			_pluginInfos.Clear();
			DllScanner.Scan(_pluginsPath, ProcessPluginClass);
			MefCompose();
			MefInitialize(serverAddress, serverPort);
		}
	}

	public void Uninitialize()
	{
		lock (this)
		{
			MefDeinitialize();
		}
	}

	public IPlugin CreateObject(Guid id, IDeviceLog logger)
	{
		lock (this)
		{
			if (_pluginInfos.Contains(id))
			{
				if (!(_pluginInfos[id] is PluginInfo pluginInfo))
				{
					return null;
				}
				return Activator.CreateInstanceFrom(pluginInfo.AssemblyFile, pluginInfo.ClassName, ignoreCase: true, BindingFlags.Default, null, new object[1] { logger }, null, null).Unwrap() as IPlugin;
			}
		}
		return null;
	}

	public PluginInfo GetPluginInfo(Guid id)
	{
		lock (this)
		{
			return _pluginInfos[id] as PluginInfo;
		}
	}

	private void ProcessPluginClass(Type type)
	{
		if (type.IsDefined(typeof(PluginAttribute), inherit: false))
		{
			object[] customAttributes = type.GetCustomAttributes(typeof(PluginAttribute), inherit: false);
			if (customAttributes.Length != 0 && customAttributes[0] is PluginAttribute pluginAttribute)
			{
				_pluginInfos.Add(pluginAttribute.Id, new PluginInfo(type.Assembly.Location, type.FullName, pluginAttribute));
			}
		}
	}

	private void MefCompose()
	{
		new CompositionContainer(new DirectoryCatalog(_pluginsPath)).ComposeParts(this);
	}

	private void MefInitialize(string mgtServerAdress, int mgtServerPort)
	{
		foreach (IDataSecurityExtensionManager dataSecurityExtensionManager in _DataSecurityExtensionManagers)
		{
			dataSecurityExtensionManager.Initialize(mgtServerAdress, mgtServerPort);
		}
		if (_PtzExtension != null)
		{
			_PtzExtension.Initialize(mgtServerAdress, mgtServerPort);
		}
	}

	private void MefDeinitialize()
	{
		if (_DataSecurityExtensionManagers != null)
		{
			foreach (IDataSecurityExtensionManager dataSecurityExtensionManager in _DataSecurityExtensionManagers)
			{
				dataSecurityExtensionManager.Uninitialize();
			}
		}
		if (_PtzExtension != null)
		{
			_PtzExtension.Uninitialize();
		}
	}

	public void SetPtzExtensionCallback(IPtzExtensionCallback ptzExtensionCallback)
	{
		if (_PtzExtension != null)
		{
			_PtzExtension.SetPtzExtensionCallback(ptzExtensionCallback);
		}
	}

	public IDataSecurityExtension CreateDataSecurityExtension(Guid deviceId)
	{
		List<IDataSecurityExtension> list = new List<IDataSecurityExtension>();
		foreach (IDataSecurityExtensionManager dataSecurityExtensionManager in _DataSecurityExtensionManagers)
		{
			list.Add(dataSecurityExtensionManager.GetDataSecurityExtension(deviceId));
		}
		return new DataSecurityExtensions(list);
	}
}
