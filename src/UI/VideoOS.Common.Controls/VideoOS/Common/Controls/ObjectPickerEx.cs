using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace VideoOS.Common.Controls;

public class ObjectPickerEx : CommonDialog
{
	public static class NativeMethods
	{
		[ComImport]
		[Guid("17D6CCD8-3B7B-11D2-B9E0-00C04FD8DBF7")]
		public class DSObjectPicker
		{
		}

		[ComImport]
		[Guid("0C87E64E-3B7A-11D2-B9E0-00C04FD8DBF7")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IDsObjectPicker
		{
			int Initialize(ref DSOP_INIT_INFO pInitInfo);

			int InvokeDialog(IntPtr HWND, ref IntPtr lpDataObject);
		}

		[ComImport]
		[Guid("0000010e-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IDataObject
		{
			[PreserveSig]
			int GetData(ref FORMATETC pFormatEtc, ref STGMEDIUM b);

			[PreserveSig]
			void GetDataHere(ref FORMATETC pFormatEtc, ref STGMEDIUM b);

			[PreserveSig]
			int QueryGetData(IntPtr a);

			[PreserveSig]
			int GetCanonicalFormatEtc(IntPtr a, IntPtr b);

			[PreserveSig]
			int SetData(IntPtr a, IntPtr b, int c);

			[PreserveSig]
			int EnumFormatEtc(uint a, IntPtr b);

			[PreserveSig]
			int DAdvise(IntPtr a, uint b, IntPtr c, ref uint d);

			[PreserveSig]
			int DUnadvise(uint a);

			[PreserveSig]
			int EnumDAdvise(IntPtr a);
		}

		public struct FORMATETC
		{
			public uint cfFormat;

			public IntPtr ptd;

			public uint dwAspect;

			public int lindex;

			public uint tymed;
		}

		public struct STGMEDIUM
		{
			public uint tymed;

			public IntPtr hGlobal;

			public IntPtr pUnkForRelease;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct DSOP_INIT_INFO
		{
			public uint cbSize;

			[MarshalAs(UnmanagedType.LPWStr)]
			public string pwzTargetComputer;

			public uint cDsScopeInfos;

			public IntPtr aDsScopeInfos;

			public uint flOptions;

			public uint cAttributesToFetch;

			public IntPtr apwzAttributeNames;
		}

		[Serializable]
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct DSOP_SCOPE_INIT_INFO
		{
			public uint cbSize;

			public uint flType;

			public uint flScope;

			[MarshalAs(UnmanagedType.Struct)]
			public DSOP_FILTER_FLAGS FilterFlags;

			[MarshalAs(UnmanagedType.LPWStr)]
			public string pwzDcName;

			[MarshalAs(UnmanagedType.LPWStr)]
			public string pwzADsPath;

			public uint hr;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct DSOP_UPLEVEL_FILTER_FLAGS
		{
			public uint flBothModes;

			public uint flMixedModeOnly;

			public uint flNativeModeOnly;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct DSOP_FILTER_FLAGS
		{
			public DSOP_UPLEVEL_FILTER_FLAGS Uplevel;

			public uint flDownlevel;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct DS_SELECTION
		{
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pwzName;

			[MarshalAs(UnmanagedType.LPWStr)]
			public string pwzADsPath;

			[MarshalAs(UnmanagedType.LPWStr)]
			public string pwzClass;

			[MarshalAs(UnmanagedType.LPWStr)]
			public string pwzUPN;

			public IntPtr pvarFetchedAttributes;

			public uint flScopeType;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct DS_SELECTION_LIST
		{
			public uint cItems;

			public uint cFetchedAttributes;
		}

		public class DS_SELECTION_ATTRIBUTES
		{
			public object[] attributeValues;
		}

		public static class DSOP_SCOPE_TYPE_FLAGS
		{
			public const uint DSOP_SCOPE_TYPE_TARGET_COMPUTER = 1u;

			public const uint DSOP_SCOPE_TYPE_UPLEVEL_JOINED_DOMAIN = 2u;

			public const uint DSOP_SCOPE_TYPE_DOWNLEVEL_JOINED_DOMAIN = 4u;

			public const uint DSOP_SCOPE_TYPE_ENTERPRISE_DOMAIN = 8u;

			public const uint DSOP_SCOPE_TYPE_GLOBAL_CATALOG = 16u;

			public const uint DSOP_SCOPE_TYPE_EXTERNAL_UPLEVEL_DOMAIN = 32u;

			public const uint DSOP_SCOPE_TYPE_EXTERNAL_DOWNLEVEL_DOMAIN = 64u;

			public const uint DSOP_SCOPE_TYPE_WORKGROUP = 128u;

			public const uint DSOP_SCOPE_TYPE_USER_ENTERED_UPLEVEL_SCOPE = 256u;

			public const uint DSOP_SCOPE_TYPE_USER_ENTERED_DOWNLEVEL_SCOPE = 512u;
		}

		public static class DSOP_INIT_INFO_FLAGS
		{
			public const uint DSOP_FLAG_MULTISELECT = 1u;

			public const uint DSOP_FLAG_SKIP_TARGET_COMPUTER_DC_CHECK = 2u;
		}

		public static class DSOP_SCOPE_INIT_INFO_FLAGS
		{
			public const uint DSOP_SCOPE_FLAG_STARTING_SCOPE = 1u;

			public const uint DSOP_SCOPE_FLAG_WANT_PROVIDER_WINNT = 2u;

			public const uint DSOP_SCOPE_FLAG_WANT_PROVIDER_LDAP = 4u;

			public const uint DSOP_SCOPE_FLAG_WANT_PROVIDER_GC = 8u;

			public const uint DSOP_SCOPE_FLAG_WANT_SID_PATH = 16u;

			public const uint DSOP_SCOPE_FLAG_WANT_DOWNLEVEL_BUILTIN_PATH = 32u;

			public const uint DSOP_SCOPE_FLAG_DEFAULT_FILTER_USERS = 64u;

			public const uint DSOP_SCOPE_FLAG_DEFAULT_FILTER_GROUPS = 128u;

			public const uint DSOP_SCOPE_FLAG_DEFAULT_FILTER_COMPUTERS = 256u;

			public const uint DSOP_SCOPE_FLAG_DEFAULT_FILTER_CONTACTS = 512u;
		}

		public static class DSOP_FILTER_FLAGS_FLAGS
		{
			public const uint DSOP_FILTER_INCLUDE_ADVANCED_VIEW = 1u;

			public const uint DSOP_FILTER_USERS = 2u;

			public const uint DSOP_FILTER_BUILTIN_GROUPS = 4u;

			public const uint DSOP_FILTER_WELL_KNOWN_PRINCIPALS = 8u;

			public const uint DSOP_FILTER_UNIVERSAL_GROUPS_DL = 16u;

			public const uint DSOP_FILTER_UNIVERSAL_GROUPS_SE = 32u;

			public const uint DSOP_FILTER_GLOBAL_GROUPS_DL = 64u;

			public const uint DSOP_FILTER_GLOBAL_GROUPS_SE = 128u;

			public const uint DSOP_FILTER_DOMAIN_LOCAL_GROUPS_DL = 256u;

			public const uint DSOP_FILTER_DOMAIN_LOCAL_GROUPS_SE = 512u;

			public const uint DSOP_FILTER_CONTACTS = 1024u;

			public const uint DSOP_FILTER_COMPUTERS = 2048u;
		}

		public static class DSOP_DOWNLEVEL_FLAGS
		{
			public const uint DSOP_DOWNLEVEL_FILTER_USERS = 2147483649u;

			public const uint DSOP_DOWNLEVEL_FILTER_LOCAL_GROUPS = 2147483650u;

			public const uint DSOP_DOWNLEVEL_FILTER_GLOBAL_GROUPS = 2147483652u;

			public const uint DSOP_DOWNLEVEL_FILTER_COMPUTERS = 2147483656u;

			public const uint DSOP_DOWNLEVEL_FILTER_WORLD = 2147483664u;

			public const uint DSOP_DOWNLEVEL_FILTER_AUTHENTICATED_USER = 2147483680u;

			public const uint DSOP_DOWNLEVEL_FILTER_ANONYMOUS = 2147483712u;

			public const uint DSOP_DOWNLEVEL_FILTER_BATCH = 2147483776u;

			public const uint DSOP_DOWNLEVEL_FILTER_CREATOR_OWNER = 2147483904u;

			public const uint DSOP_DOWNLEVEL_FILTER_CREATOR_GROUP = 2147484160u;

			public const uint DSOP_DOWNLEVEL_FILTER_DIALUP = 2147484672u;

			public const uint DSOP_DOWNLEVEL_FILTER_INTERACTIVE = 2147485696u;

			public const uint DSOP_DOWNLEVEL_FILTER_NETWORK = 2147487744u;

			public const uint DSOP_DOWNLEVEL_FILTER_SERVICE = 2147491840u;

			public const uint DSOP_DOWNLEVEL_FILTER_SYSTEM = 2147500032u;

			public const uint DSOP_DOWNLEVEL_FILTER_EXCLUDE_BUILTIN_GROUPS = 2147516416u;

			public const uint DSOP_DOWNLEVEL_FILTER_TERMINAL_SERVER = 2147549184u;

			public const uint DSOP_DOWNLEVEL_FILTER_ALL_WELLKNOWN_SIDS = 2147614720u;

			public const uint DSOP_DOWNLEVEL_FILTER_LOCAL_SERVICE = 2147745792u;

			public const uint DSOP_DOWNLEVEL_FILTER_NETWORK_SERVICE = 2148007936u;

			public const uint DSOP_DOWNLEVEL_FILTER_REMOTE_LOGON = 2148532224u;
		}

		public static class HRESULT
		{
			public const int S_OK = 0;

			public const int S_FALSE = 1;

			public const int E_NOTIMPL = -2147467263;
		}

		public static class CLIPBOARD_FORMAT
		{
			public const string CFSTR_DSOP_DS_SELECTION_LIST = "CFSTR_DSOP_DS_SELECTION_LIST";
		}

		public enum TYMED
		{
			TYMED_HGLOBAL = 1,
			TYMED_FILE = 2,
			TYMED_ISTREAM = 4,
			TYMED_ISTORAGE = 8,
			TYMED_GDI = 16,
			TYMED_MFPICT = 32,
			TYMED_ENHMF = 64,
			TYMED_NULL = 0
		}

		public enum DVASPECT
		{
			DVASPECT_CONTENT = 1,
			DVASPECT_THUMBNAIL = 2,
			DVASPECT_ICON = 4,
			DVASPECT_DOCPRINT = 8
		}

		[DllImport("kernel32.dll")]
		public static extern IntPtr GlobalLock(IntPtr hMem);

		[DllImport("kernel32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GlobalUnlock(IntPtr hMem);
	}

	private DirectoryObject[] _selectedObjects;

	private string[] attributeNames;

	private uint length;

	public DirectoryObjectTypes AllowedTypes { get; set; }

	public DirectoryLocations AllowedLocations { get; set; }

	public DirectoryObjectTypes DefaultTypes { get; set; }

	public DirectoryLocations DefaultLocations { get; set; }

	public BuiltInPrincipals AllowedBuiltInPricipals { get; set; }

	public string TargetComputer { get; set; }

	public bool MultiSelect { get; set; }

	public ADsPathType PathType { get; set; }

	public DirectoryObject[] SelectedObjects
	{
		get
		{
			if (_selectedObjects == null)
			{
				return new DirectoryObject[0];
			}
			return (DirectoryObject[])_selectedObjects.Clone();
		}
	}

	public ObjectPickerEx()
	{
		Reset();
		attributeNames = new string[1] { "objectSid" };
		length = (uint)attributeNames.Length;
	}

	public override void Reset()
	{
		AllowedTypes = DirectoryObjectTypes.All;
		AllowedLocations = DirectoryLocations.All;
		DefaultTypes = DirectoryObjectTypes.All;
		DefaultLocations = DirectoryLocations.None;
		PathType = ADsPathType.Ldap;
		MultiSelect = false;
		TargetComputer = null;
		_selectedObjects = null;
	}

	protected override bool RunDialog(IntPtr hwndOwner)
	{
		IntPtr lpDataObject = IntPtr.Zero;
		NativeMethods.IDsObjectPicker dsObjectPicker = Initialize();
		if (dsObjectPicker == null)
		{
			_selectedObjects = null;
			return false;
		}
		int num = dsObjectPicker.InvokeDialog(hwndOwner, ref lpDataObject);
		if (lpDataObject != IntPtr.Zero)
		{
			_selectedObjects = ProcessSelections(lpDataObject);
		}
		return num == 0;
	}

	private NativeMethods.IDsObjectPicker Initialize()
	{
		NativeMethods.IDsObjectPicker dsObjectPicker = (NativeMethods.IDsObjectPicker)new NativeMethods.DSObjectPicker();
		NativeMethods.DSOP_SCOPE_INIT_INFO[] array = new NativeMethods.DSOP_SCOPE_INIT_INFO[2];
		uint defaultFilter = GetDefaultFilter();
		uint upLevelFilter = GetUpLevelFilter();
		uint downLevelFilter = GetDownLevelFilter();
		uint startingScope = GetStartingScope();
		if (startingScope != 0)
		{
			array[0].cbSize = (uint)Marshal.SizeOf(typeof(NativeMethods.DSOP_SCOPE_INIT_INFO));
			array[0].flType = startingScope;
			array[0].flScope = 1 | defaultFilter;
			array[0].FilterFlags.Uplevel.flBothModes = upLevelFilter;
			array[0].FilterFlags.flDownlevel = downLevelFilter;
			array[0].pwzADsPath = null;
			array[0].pwzDcName = null;
			array[0].hr = 0u;
		}
		uint otherScope = GetOtherScope();
		if (otherScope != 0)
		{
			array[1].cbSize = (uint)Marshal.SizeOf(typeof(NativeMethods.DSOP_SCOPE_INIT_INFO));
			array[1].flType = otherScope;
			array[1].flScope = defaultFilter;
			array[1].FilterFlags.Uplevel.flBothModes = upLevelFilter;
			array[1].FilterFlags.flDownlevel = downLevelFilter;
			array[1].pwzADsPath = null;
			array[1].pwzDcName = null;
			array[1].hr = 0u;
		}
		IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NativeMethods.DSOP_SCOPE_INIT_INFO)) * array.Length);
		for (int i = 0; i < array.Length; i++)
		{
			Marshal.StructureToPtr(array[i], (IntPtr)((long)intPtr + i * Marshal.SizeOf(typeof(NativeMethods.DSOP_SCOPE_INIT_INFO))), fDeleteOld: false);
		}
		NativeMethods.DSOP_INIT_INFO pInitInfo = default(NativeMethods.DSOP_INIT_INFO);
		pInitInfo.cbSize = (uint)Marshal.SizeOf(pInitInfo);
		pInitInfo.pwzTargetComputer = TargetComputer;
		pInitInfo.cDsScopeInfos = (uint)array.Length;
		pInitInfo.aDsScopeInfos = intPtr;
		uint num = 0u;
		if (MultiSelect)
		{
			num |= 1;
		}
		pInitInfo.flOptions = num;
		pInitInfo.cAttributesToFetch = length;
		if (length != 0)
		{
			MarshalStrings marshalStrings = new MarshalStrings(attributeNames);
			pInitInfo.apwzAttributeNames = marshalStrings.ArrayPtr;
		}
		else
		{
			pInitInfo.cAttributesToFetch = 0u;
			pInitInfo.apwzAttributeNames = IntPtr.Zero;
		}
		if (dsObjectPicker.Initialize(ref pInitInfo) != 0)
		{
			return null;
		}
		return dsObjectPicker;
	}

	private DirectoryObject[] ProcessSelections(IntPtr dataObj)
	{
		if (dataObj == IntPtr.Zero)
		{
			throw new ArgumentNullException("dataObj");
		}
		DirectoryObject[] array = null;
		NativeMethods.STGMEDIUM b = new NativeMethods.STGMEDIUM
		{
			tymed = 1u,
			hGlobal = IntPtr.Zero,
			pUnkForRelease = IntPtr.Zero
		};
		NativeMethods.FORMATETC pFormatEtc = new NativeMethods.FORMATETC
		{
			cfFormat = (uint)DataFormats.GetFormat("CFSTR_DSOP_DS_SELECTION_LIST").Id,
			ptd = IntPtr.Zero,
			dwAspect = 1u,
			lindex = -1,
			tymed = 1u
		};
		NativeMethods.DS_SELECTION[] array2 = null;
		int num = 0;
		NativeMethods.DS_SELECTION_ATTRIBUTES[] array3 = null;
		if (((NativeMethods.IDataObject)Marshal.GetTypedObjectForIUnknown(dataObj, typeof(NativeMethods.IDataObject))).GetData(ref pFormatEtc, ref b) == 0)
		{
			IntPtr intPtr = NativeMethods.GlobalLock(b.hGlobal);
			try
			{
				NativeMethods.DS_SELECTION_LIST structure = (NativeMethods.DS_SELECTION_LIST)Marshal.PtrToStructure(intPtr, typeof(NativeMethods.DS_SELECTION_LIST));
				int cItems = (int)structure.cItems;
				num = (int)structure.cFetchedAttributes;
				array = new DirectoryObject[cItems];
				if (cItems > 0)
				{
					array2 = new NativeMethods.DS_SELECTION[cItems];
					array3 = new NativeMethods.DS_SELECTION_ATTRIBUTES[cItems];
					IntPtr intPtr2 = (IntPtr)((long)intPtr + Marshal.SizeOf(structure));
					byte[] binarySid = null;
					for (int i = 0; i < cItems; i++)
					{
						array2[i] = default(NativeMethods.DS_SELECTION);
						array2[i] = (NativeMethods.DS_SELECTION)Marshal.PtrToStructure(intPtr2, typeof(NativeMethods.DS_SELECTION));
						if (length != 0 && num > 0)
						{
							array3[i] = new NativeMethods.DS_SELECTION_ATTRIBUTES();
							array3[i].attributeValues = Marshal.GetObjectsForNativeVariants(array2[i].pvarFetchedAttributes, num);
							binarySid = array3[i].attributeValues[0] as byte[];
						}
						string pwzClass = array2[i].pwzClass;
						string pwzName = array2[i].pwzName;
						string pwzUPN = array2[i].pwzUPN;
						string pwzADsPath = array2[i].pwzADsPath;
						array[i] = new DirectoryObject(pwzClass, pwzName, pwzUPN, pwzADsPath);
						array[i].BinarySid = binarySid;
						intPtr2 = (IntPtr)((long)intPtr2 + Marshal.SizeOf(array2[i]));
					}
				}
			}
			finally
			{
				NativeMethods.GlobalUnlock(b.hGlobal);
			}
		}
		return array;
	}

	private uint GetDefaultFilter()
	{
		uint num = 0u;
		if ((DefaultTypes & DirectoryObjectTypes.Users) == DirectoryObjectTypes.Users || (DefaultTypes & DirectoryObjectTypes.WellKnownPrincipals) == DirectoryObjectTypes.WellKnownPrincipals)
		{
			num |= 0x40;
		}
		if ((DefaultTypes & DirectoryObjectTypes.Groups) == DirectoryObjectTypes.Groups || (DefaultTypes & DirectoryObjectTypes.BuiltInGroups) == DirectoryObjectTypes.BuiltInGroups)
		{
			num |= 0x80;
		}
		if ((DefaultTypes & DirectoryObjectTypes.Computers) == DirectoryObjectTypes.Computers)
		{
			num |= 0x100;
		}
		if ((DefaultTypes & DirectoryObjectTypes.Contacts) == DirectoryObjectTypes.Contacts)
		{
			num |= 0x200;
		}
		return num | (uint)PathType;
	}

	private uint GetUpLevelFilter()
	{
		uint num = 0u;
		if ((AllowedTypes & DirectoryObjectTypes.Users) == DirectoryObjectTypes.Users)
		{
			num |= 2;
		}
		if ((AllowedTypes & DirectoryObjectTypes.Groups) == DirectoryObjectTypes.Groups)
		{
			num |= 0x3F0;
		}
		if ((AllowedTypes & DirectoryObjectTypes.Computers) == DirectoryObjectTypes.Computers)
		{
			num |= 0x800;
		}
		if ((AllowedTypes & DirectoryObjectTypes.Contacts) == DirectoryObjectTypes.Contacts)
		{
			num |= 0x400;
		}
		if ((AllowedTypes & DirectoryObjectTypes.BuiltInGroups) == DirectoryObjectTypes.BuiltInGroups)
		{
			num |= 4;
		}
		if ((AllowedTypes & DirectoryObjectTypes.WellKnownPrincipals) == DirectoryObjectTypes.WellKnownPrincipals)
		{
			num |= 8;
		}
		return num;
	}

	private uint GetDownLevelFilter()
	{
		uint num = 0u;
		if ((AllowedTypes & DirectoryObjectTypes.Users) == DirectoryObjectTypes.Users)
		{
			num |= 0x80000001u;
		}
		if ((AllowedTypes & DirectoryObjectTypes.Groups) == DirectoryObjectTypes.Groups)
		{
			num |= 0x80000006u;
		}
		if ((AllowedTypes & DirectoryObjectTypes.Computers) == DirectoryObjectTypes.Computers)
		{
			num |= 0x80000008u;
		}
		if ((AllowedTypes & DirectoryObjectTypes.BuiltInGroups) == 0)
		{
			num |= 0x80008000u;
		}
		if ((AllowedTypes & DirectoryObjectTypes.WellKnownPrincipals) == DirectoryObjectTypes.WellKnownPrincipals)
		{
			num |= (uint)AllowedBuiltInPricipals;
		}
		return num;
	}

	private uint GetStartingScope()
	{
		return GetScope(DefaultLocations);
	}

	private uint GetOtherScope()
	{
		return GetScope(AllowedLocations & ~DefaultLocations);
	}

	public static uint GetScope(DirectoryLocations locations)
	{
		uint num = 0u;
		if ((locations & DirectoryLocations.LocalComputer) == DirectoryLocations.LocalComputer)
		{
			num |= 1;
		}
		if ((locations & DirectoryLocations.JoinedDomain) == DirectoryLocations.JoinedDomain)
		{
			num |= 6;
		}
		if ((locations & DirectoryLocations.EnterpriseDomain) == DirectoryLocations.EnterpriseDomain)
		{
			num |= 8;
		}
		if ((locations & DirectoryLocations.GlobalCatalog) == DirectoryLocations.GlobalCatalog)
		{
			num |= 0x10;
		}
		if ((locations & DirectoryLocations.ExternalDomain) == DirectoryLocations.ExternalDomain)
		{
			num |= 0x60;
		}
		if ((locations & DirectoryLocations.Workgroup) == DirectoryLocations.Workgroup)
		{
			num |= 0x80;
		}
		if ((locations & DirectoryLocations.UserEntered) == DirectoryLocations.UserEntered)
		{
			num |= 0x300;
		}
		return num;
	}

	public static DirectoryObject[] ShowUserObjectPicker(IWin32Window owner)
	{
		using ObjectPickerEx objectPickerEx = new ObjectPickerEx();
		objectPickerEx.AllowedTypes = DirectoryObjectTypes.Users;
		objectPickerEx.AllowedLocations = DirectoryLocations.LocalComputer | DirectoryLocations.JoinedDomain | DirectoryLocations.EnterpriseDomain | DirectoryLocations.GlobalCatalog | DirectoryLocations.ExternalDomain | DirectoryLocations.UserEntered;
		objectPickerEx.DefaultTypes = DirectoryObjectTypes.Users;
		objectPickerEx.DefaultLocations = DirectoryLocations.JoinedDomain;
		objectPickerEx.TargetComputer = null;
		objectPickerEx.AllowedBuiltInPricipals = BuiltInPrincipals.Users;
		objectPickerEx.MultiSelect = true;
		objectPickerEx.PathType = ADsPathType.NT4;
		try
		{
			if (objectPickerEx.ShowDialog(owner) != DialogResult.OK || objectPickerEx.SelectedObjects == null || objectPickerEx.SelectedObjects.Length == 0)
			{
				return null;
			}
		}
		catch (COMException)
		{
			throw;
		}
		return objectPickerEx.SelectedObjects;
	}
}
