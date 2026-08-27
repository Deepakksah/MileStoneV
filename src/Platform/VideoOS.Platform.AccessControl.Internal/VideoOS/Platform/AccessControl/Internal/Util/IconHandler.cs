using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using VideoOS.Platform.AccessControl.Internal.Providers;
using VideoOS.Platform.AccessControl.Internal.Providers.Strategies;

namespace VideoOS.Platform.AccessControl.Internal.Util;

public static class IconHandler
{
	private static Guid UnspecifiedIconKey;

	private static Icon NoneIcon;

	private static Icon UnspecifiedIcon;

	private static Icon CommandIcon;

	private static Icon DoorClosedIcon;

	private static Icon DoorClosedLockedIcon;

	private static Icon DoorClosedUnlockedIcon;

	private static Icon DoorOpenIcon;

	private static Icon DoorOpenLockedIcon;

	private static Icon DoorOpenUnlockedIcon;

	private static Icon DoorUnknownIcon;

	private static Icon DoorErrorIcon;

	private static Icon ServerIcon;

	private static Icon ServerConnectedIcon;

	private static Icon ServerDisconnectedIcon;

	private static Icon ServerUnknownIcon;

	private static Icon ControllerIcon;

	private static Icon ControllerConnectedIcon;

	private static Icon ControllerDisconnectedIcon;

	private static Icon ControllerUnknownIcon;

	private static Icon AccessPointIcon;

	private static Dictionary<string, Icon> _builtInIconDictionary;

	private static Dictionary<Guid, Dictionary<string, Icon>> _accessControlPluginIconDictionary;

	private static Dictionary<Guid, Icon> _globalIconDictionary;

	private static Dictionary<Guid, Bitmap> _globalIconBitmapDictionary;

	private static object _lockObject;

	static IconHandler()
	{
		UnspecifiedIconKey = new Guid("5676E8E6-ED62-48C0-ABA3-8A05274963A7");
		_builtInIconDictionary = new Dictionary<string, Icon>();
		_accessControlPluginIconDictionary = new Dictionary<Guid, Dictionary<string, Icon>>();
		_globalIconDictionary = new Dictionary<Guid, Icon>();
		_globalIconBitmapDictionary = new Dictionary<Guid, Bitmap>();
		_lockObject = new object();
		Assembly executingAssembly = Assembly.GetExecutingAssembly();
		NoneIcon = new Icon(executingAssembly.GetManifestResourceStream("VideoOS.Platform.AccessControl.Internal.Resources.None.ico"));
		UnspecifiedIcon = new Icon(executingAssembly.GetManifestResourceStream("VideoOS.Platform.AccessControl.Internal.Resources.Unspecified.ico"));
		CommandIcon = new Icon(executingAssembly.GetManifestResourceStream("VideoOS.Platform.AccessControl.Internal.Resources.Command.ico"));
		DoorClosedIcon = new Icon(executingAssembly.GetManifestResourceStream("VideoOS.Platform.AccessControl.Internal.Resources.DoorClosed.ico"));
		DoorClosedUnlockedIcon = new Icon(executingAssembly.GetManifestResourceStream("VideoOS.Platform.AccessControl.Internal.Resources.DoorClosed.ico"));
		DoorClosedLockedIcon = new Icon(executingAssembly.GetManifestResourceStream("VideoOS.Platform.AccessControl.Internal.Resources.DoorClosedLocked.ico"));
		DoorOpenIcon = new Icon(executingAssembly.GetManifestResourceStream("VideoOS.Platform.AccessControl.Internal.Resources.DoorOpen.ico"));
		DoorOpenUnlockedIcon = new Icon(executingAssembly.GetManifestResourceStream("VideoOS.Platform.AccessControl.Internal.Resources.DoorOpen.ico"));
		DoorOpenLockedIcon = new Icon(executingAssembly.GetManifestResourceStream("VideoOS.Platform.AccessControl.Internal.Resources.DoorOpenLocked.ico"));
		DoorUnknownIcon = new Icon(executingAssembly.GetManifestResourceStream("VideoOS.Platform.AccessControl.Internal.Resources.DoorUnknown.ico"));
		DoorErrorIcon = new Icon(executingAssembly.GetManifestResourceStream("VideoOS.Platform.AccessControl.Internal.Resources.DoorError.ico"));
		ServerIcon = new Icon(executingAssembly.GetManifestResourceStream("VideoOS.Platform.AccessControl.Internal.Resources.Server.ico"));
		ServerConnectedIcon = new Icon(executingAssembly.GetManifestResourceStream("VideoOS.Platform.AccessControl.Internal.Resources.ServerConnected.ico"));
		ServerDisconnectedIcon = new Icon(executingAssembly.GetManifestResourceStream("VideoOS.Platform.AccessControl.Internal.Resources.ServerDisconnected.ico"));
		ServerUnknownIcon = new Icon(executingAssembly.GetManifestResourceStream("VideoOS.Platform.AccessControl.Internal.Resources.ServerUnknown.ico"));
		ControllerIcon = new Icon(executingAssembly.GetManifestResourceStream("VideoOS.Platform.AccessControl.Internal.Resources.DoorController.ico"));
		ControllerConnectedIcon = new Icon(executingAssembly.GetManifestResourceStream("VideoOS.Platform.AccessControl.Internal.Resources.DoorControllerConnected.ico"));
		ControllerDisconnectedIcon = new Icon(executingAssembly.GetManifestResourceStream("VideoOS.Platform.AccessControl.Internal.Resources.DoorControllerDisconnected.ico"));
		ControllerUnknownIcon = new Icon(executingAssembly.GetManifestResourceStream("VideoOS.Platform.AccessControl.Internal.Resources.DoorControllerUnknown.ico"));
		AccessPointIcon = new Icon(executingAssembly.GetManifestResourceStream("VideoOS.Platform.AccessControl.Internal.Resources.AccessPoint.ico"));
		Clear();
	}

	public static void Clear()
	{
		_builtInIconDictionary.Clear();
		_accessControlPluginIconDictionary.Clear();
		_globalIconDictionary.Clear();
		_globalIconBitmapDictionary.Clear();
		_builtInIconDictionary.Add("Unspecified", UnspecifiedIcon);
		_builtInIconDictionary.Add(ACBuiltInIconKeys.None, NoneIcon);
		_builtInIconDictionary.Add(ACBuiltInIconKeys.Command, CommandIcon);
		_builtInIconDictionary.Add(ACBuiltInIconKeys.DoorClosed, DoorClosedIcon);
		_builtInIconDictionary.Add(ACBuiltInIconKeys.DoorClosedLocked, DoorClosedLockedIcon);
		_builtInIconDictionary.Add(ACBuiltInIconKeys.DoorClosedUnlocked, DoorClosedUnlockedIcon);
		_builtInIconDictionary.Add(ACBuiltInIconKeys.DoorOpen, DoorOpenIcon);
		_builtInIconDictionary.Add(ACBuiltInIconKeys.DoorOpenLocked, DoorOpenLockedIcon);
		_builtInIconDictionary.Add(ACBuiltInIconKeys.DoorOpenUnlocked, DoorOpenUnlockedIcon);
		_builtInIconDictionary.Add(ACBuiltInIconKeys.DoorUnknown, DoorUnknownIcon);
		_builtInIconDictionary.Add(ACBuiltInIconKeys.DoorError, DoorErrorIcon);
		_builtInIconDictionary.Add(ACBuiltInIconKeys.Server, ServerIcon);
		_builtInIconDictionary.Add(ACBuiltInIconKeys.ServerConnected, ServerConnectedIcon);
		_builtInIconDictionary.Add(ACBuiltInIconKeys.ServerDisconnected, ServerDisconnectedIcon);
		_builtInIconDictionary.Add(ACBuiltInIconKeys.ServerUnknown, ServerUnknownIcon);
		_builtInIconDictionary.Add(ACBuiltInIconKeys.Controller, ControllerIcon);
		_builtInIconDictionary.Add(ACBuiltInIconKeys.ControllerConnected, ControllerConnectedIcon);
		_builtInIconDictionary.Add(ACBuiltInIconKeys.ControllerDisconnected, ControllerDisconnectedIcon);
		_builtInIconDictionary.Add(ACBuiltInIconKeys.ControllerUnknown, ControllerUnknownIcon);
		_builtInIconDictionary.Add(ACBuiltInIconKeys.AccessPoint, AccessPointIcon);
		_globalIconDictionary.Add(UnspecifiedIconKey, UnspecifiedIcon);
		_globalIconBitmapDictionary.Add(UnspecifiedIconKey, UnspecifiedIcon.ToBitmap());
	}

	public static void AddAccessControlPluginIcons(Guid accessControlPluginId, Dictionary<string, Icon> iconDictionary)
	{
		lock (_lockObject)
		{
			if (HasAccessControlPluginIcons(accessControlPluginId))
			{
				return;
			}
			_accessControlPluginIconDictionary.Add(accessControlPluginId, iconDictionary);
			foreach (KeyValuePair<string, Icon> item in _builtInIconDictionary)
			{
				foreach (IGuidProviderStrategy guidProviderStrategy in GuidProvider.Instance.GuidProviderStrategies)
				{
					_globalIconDictionary[BuildAccessControlPluginSpecificIconKey(accessControlPluginId, item.Key, guidProviderStrategy)] = item.Value;
				}
			}
			foreach (KeyValuePair<string, Icon> item2 in iconDictionary)
			{
				foreach (IGuidProviderStrategy guidProviderStrategy2 in GuidProvider.Instance.GuidProviderStrategies)
				{
					_globalIconDictionary[BuildAccessControlPluginSpecificIconKey(accessControlPluginId, item2.Key, guidProviderStrategy2)] = item2.Value;
				}
			}
		}
	}

	public static void RemoveAccessControlPluginIcons(Guid accessControlPluginId)
	{
		lock (_lockObject)
		{
			if (!HasAccessControlPluginIcons(accessControlPluginId))
			{
				return;
			}
			Dictionary<string, Icon> dictionary = _accessControlPluginIconDictionary[accessControlPluginId];
			foreach (KeyValuePair<string, Icon> item in _builtInIconDictionary)
			{
				foreach (IGuidProviderStrategy guidProviderStrategy in GuidProvider.Instance.GuidProviderStrategies)
				{
					_globalIconDictionary.Remove(BuildAccessControlPluginSpecificIconKey(accessControlPluginId, item.Key, guidProviderStrategy));
					_globalIconBitmapDictionary.Remove(BuildAccessControlPluginSpecificIconKey(accessControlPluginId, item.Key, guidProviderStrategy));
				}
			}
			foreach (KeyValuePair<string, Icon> item2 in dictionary)
			{
				foreach (IGuidProviderStrategy guidProviderStrategy2 in GuidProvider.Instance.GuidProviderStrategies)
				{
					_globalIconDictionary.Remove(BuildAccessControlPluginSpecificIconKey(accessControlPluginId, item2.Key, guidProviderStrategy2));
					_globalIconBitmapDictionary.Remove(BuildAccessControlPluginSpecificIconKey(accessControlPluginId, item2.Key, guidProviderStrategy2));
				}
			}
			_accessControlPluginIconDictionary.Remove(accessControlPluginId);
		}
	}

	public static bool HasAccessControlPluginIcons(Guid accessControlPluginId)
	{
		lock (_lockObject)
		{
			return _accessControlPluginIconDictionary.ContainsKey(accessControlPluginId);
		}
	}

	public static bool HasIcon(Guid iconKey)
	{
		lock (_lockObject)
		{
			return _globalIconDictionary.ContainsKey(iconKey);
		}
	}

	public static Dictionary<string, Icon> GetAccessControlPluginIcons(Guid accessControlPluginId)
	{
		lock (_lockObject)
		{
			if (HasAccessControlPluginIcons(accessControlPluginId))
			{
				return new Dictionary<string, Icon>(_accessControlPluginIconDictionary[accessControlPluginId]);
			}
			return new Dictionary<string, Icon>();
		}
	}

	public static Guid GetUnspecifiedIconKey()
	{
		return UnspecifiedIconKey;
	}

	public static Icon GetUnspecifiedIcon()
	{
		return UnspecifiedIcon;
	}

	public static Dictionary<Guid, Icon> GetGlobalIconDictionary()
	{
		lock (_lockObject)
		{
			return new Dictionary<Guid, Icon>(_globalIconDictionary);
		}
	}

	public static Guid GetIconKey(Guid accessControlPluginId, string externalIconKey)
	{
		lock (_lockObject)
		{
			foreach (IGuidProviderStrategy guidProviderStrategy in GuidProvider.Instance.GuidProviderStrategies)
			{
				Guid guid = BuildAccessControlPluginSpecificIconKey(accessControlPluginId, externalIconKey, guidProviderStrategy);
				if (_globalIconDictionary.ContainsKey(guid))
				{
					return guid;
				}
			}
			return UnspecifiedIconKey;
		}
	}

	public static Icon GetIcon(Guid iconKey)
	{
		lock (_lockObject)
		{
			if (_globalIconDictionary.ContainsKey(iconKey))
			{
				return _globalIconDictionary[iconKey];
			}
			return UnspecifiedIcon;
		}
	}

	public static Bitmap GetIconBitmap(Guid iconKey)
	{
		lock (_lockObject)
		{
			if (_globalIconBitmapDictionary.ContainsKey(iconKey))
			{
				return _globalIconBitmapDictionary[iconKey];
			}
			if (_globalIconDictionary.ContainsKey(iconKey))
			{
				Bitmap bitmap = IconToBitmap(_globalIconDictionary[iconKey]);
				_globalIconBitmapDictionary[iconKey] = bitmap;
				return bitmap;
			}
			return _globalIconBitmapDictionary[UnspecifiedIconKey];
		}
	}

	public static Icon GetIcon(Guid accessControlPluginId, string externalIconKey)
	{
		lock (_lockObject)
		{
			foreach (IGuidProviderStrategy guidProviderStrategy in GuidProvider.Instance.GuidProviderStrategies)
			{
				Guid key = BuildAccessControlPluginSpecificIconKey(accessControlPluginId, externalIconKey, guidProviderStrategy);
				if (_globalIconDictionary.ContainsKey(key))
				{
					return _globalIconDictionary[key];
				}
			}
			return UnspecifiedIcon;
		}
	}

	public static Bitmap GetIconBitmap(Guid accessControlPluginId, string externalIconKey)
	{
		Bitmap bitmap = _globalIconBitmapDictionary[UnspecifiedIconKey];
		foreach (IGuidProviderStrategy guidProviderStrategy in GuidProvider.Instance.GuidProviderStrategies)
		{
			Bitmap iconBitmap = GetIconBitmap(BuildAccessControlPluginSpecificIconKey(accessControlPluginId, externalIconKey, guidProviderStrategy));
			if (iconBitmap != bitmap)
			{
				bitmap = iconBitmap;
				break;
			}
		}
		return bitmap;
	}

	public static Bitmap IconToBitmap(Icon icon)
	{
		Bitmap result = null;
		if (icon != null)
		{
			result = new Icon(icon, 16, 16).ToBitmap();
		}
		return result;
	}

	public static Bitmap IconToBitmap(Icon icon, int width, int height)
	{
		if (icon == null)
		{
			return null;
		}
		Bitmap bitmap = new Icon(icon, width, height).ToBitmap();
		bitmap.SetResolution(96f, 96f);
		return bitmap;
	}

	private static Guid BuildAccessControlPluginSpecificIconKey(Guid accessControlPluginId, string externalIconKey, IGuidProviderStrategy guidProviderStrategy)
	{
		return guidProviderStrategy.Create(accessControlPluginId, guidProviderStrategy.GetIconKey(externalIconKey));
	}
}
