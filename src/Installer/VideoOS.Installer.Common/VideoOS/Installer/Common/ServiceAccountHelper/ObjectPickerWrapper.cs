using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace VideoOS.Installer.Common.ServiceAccountHelper;

public class ObjectPickerWrapper
{
	public static AccountInfo[] ShowUserObjectPicker(IntPtr parentHandle)
	{
		IDataObject lpDataObject = null;
		InitializeWithUsers(out var refScopeInitInfo).InvokeDialog(parentHandle, out lpDataObject);
		if (refScopeInitInfo != IntPtr.Zero)
		{
			Marshal.FreeHGlobal(refScopeInitInfo);
		}
		return ProcessSelections(lpDataObject);
	}

	private static IDsObjectPicker InitializeWithUsers(out IntPtr refScopeInitInfo)
	{
		refScopeInitInfo = IntPtr.Zero;
		IDsObjectPicker obj = (IDsObjectPicker)new DSObjectPicker();
		DSOP_SCOPE_INIT_INFO[] array = new DSOP_SCOPE_INIT_INFO[2];
		array[0].cbSize = (uint)Marshal.SizeOf(typeof(DSOP_SCOPE_INIT_INFO));
		array[0].flType = 6u;
		array[0].flScope = 67u;
		array[0].FilterFlags.Uplevel.flBothModes = 2u;
		array[0].FilterFlags.flDownlevel = 2147483649u;
		array[0].pwzADsPath = null;
		array[0].pwzDcName = null;
		array[0].hr = 0u;
		array[1].cbSize = (uint)Marshal.SizeOf(typeof(DSOP_SCOPE_INIT_INFO));
		array[1].flType = 889u;
		array[1].flScope = 66u;
		array[1].FilterFlags.Uplevel.flBothModes = 2u;
		array[1].FilterFlags.flDownlevel = 2147483649u;
		array[1].pwzADsPath = null;
		array[1].pwzDcName = null;
		array[1].hr = 0u;
		refScopeInitInfo = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(DSOP_SCOPE_INIT_INFO)) * array.Length);
		Marshal.StructureToPtr(array[0], refScopeInitInfo, fDeleteOld: false);
		Marshal.StructureToPtr(array[1], (IntPtr)((long)refScopeInitInfo + Marshal.SizeOf(typeof(DSOP_SCOPE_INIT_INFO))), fDeleteOld: false);
		DSOP_INIT_INFO pInitInfo = default(DSOP_INIT_INFO);
		pInitInfo.cbSize = (uint)Marshal.SizeOf(pInitInfo);
		pInitInfo.pwzTargetComputer = null;
		pInitInfo.cDsScopeInfos = (uint)array.Length;
		pInitInfo.aDsScopeInfos = refScopeInitInfo;
		pInitInfo.cAttributesToFetch = 0u;
		pInitInfo.apwzAttributeNames = IntPtr.Zero;
		obj.Initialize(ref pInitInfo);
		return obj;
	}

	private static AccountInfo[] ProcessSelections(IDataObject dataObj)
	{
		if (dataObj == null)
		{
			return null;
		}
		AccountInfo[] array = null;
		STGMEDIUM b = new STGMEDIUM
		{
			tymed = 1u,
			hGlobal = IntPtr.Zero,
			pUnkForRelease = IntPtr.Zero
		};
		FORMATETC pFormatEtc = new FORMATETC
		{
			cfFormat = (uint)DataFormats.GetFormat("CFSTR_DSOP_DS_SELECTION_LIST").Id,
			ptd = IntPtr.Zero,
			dwAspect = 1u,
			lindex = -1,
			tymed = 1u
		};
		dataObj.GetData(ref pFormatEtc, ref b);
		IntPtr intPtr = PInvoke.GlobalLock(b.hGlobal);
		try
		{
			IntPtr intPtr2 = intPtr;
			int num = Marshal.ReadInt32(intPtr2);
			if (num > 0)
			{
				array = new AccountInfo[num];
				intPtr2 = (IntPtr)((long)intPtr2 + Marshal.SizeOf(typeof(uint)) * 2);
				for (int i = 0; i < num; i++)
				{
					DS_SELECTION dS_SELECTION = (DS_SELECTION)Marshal.PtrToStructure(intPtr2, typeof(DS_SELECTION));
					intPtr2 = (IntPtr)((long)intPtr2 + Marshal.SizeOf(typeof(DS_SELECTION)));
					array[i] = default(AccountInfo);
					array[i].Name = dS_SELECTION.pwzName;
					array[i].ADPath = dS_SELECTION.pwzADsPath;
					array[i].UPN = dS_SELECTION.pwzUPN;
					array[i].ClassName = dS_SELECTION.pwzClass.ToLower();
				}
			}
		}
		finally
		{
			PInvoke.GlobalUnlock(intPtr);
		}
		return array;
	}
}
