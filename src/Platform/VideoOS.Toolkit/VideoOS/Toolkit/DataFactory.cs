using System.Runtime.CompilerServices;
using NmToolkit;

namespace VideoOS.Toolkit;

public class DataFactory : IDataFactory
{
	private DataFactory()
	{
	}

	public unsafe static IData CreateFromImData(ImData* nativeData)
	{
		ImImage* ptr = (ImImage*)global::_003CModule_003E.__RTDynamicCast(nativeData, 0, System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImData_0040NmToolkit_0040_0040_00408), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImImage_0040NmToolkit_0040_0040_00408), 0);
		if (ptr != null)
		{
			return new Image(ptr);
		}
		ImMediaDataVector* ptr2 = (ImMediaDataVector*)global::_003CModule_003E.__RTDynamicCast(nativeData, 0, System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImData_0040NmToolkit_0040_0040_00408), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImMediaDataVector_0040NmToolkit_0040_0040_00408), 0);
		if (ptr2 != null)
		{
			return new MediaDataVector(ptr2);
		}
		ImMediaData* ptr3 = (ImMediaData*)global::_003CModule_003E.__RTDynamicCast(nativeData, 0, System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImData_0040NmToolkit_0040_0040_00408), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImMediaData_0040NmToolkit_0040_0040_00408), 0);
		if (ptr3 != null)
		{
			return new MediaData(ptr3);
		}
		ImMetaData* ptr4 = (ImMetaData*)global::_003CModule_003E.__RTDynamicCast(nativeData, 0, System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImData_0040NmToolkit_0040_0040_00408), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImMetaData_0040NmToolkit_0040_0040_00408), 0);
		if (ptr4 != null)
		{
			return new MetaData(ptr4);
		}
		ImLiveStatusData* ptr5 = (ImLiveStatusData*)global::_003CModule_003E.__RTDynamicCast(nativeData, 0, System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImData_0040NmToolkit_0040_0040_00408), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImLiveStatusData_0040NmToolkit_0040_0040_00408), 0);
		if (ptr5 != null)
		{
			return new LiveStatusData(ptr5);
		}
		ImOutOfBandData* ptr6 = (ImOutOfBandData*)global::_003CModule_003E.__RTDynamicCast(nativeData, 0, System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImData_0040NmToolkit_0040_0040_00408), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImOutOfBandData_0040NmToolkit_0040_0040_00408), 0);
		if (ptr6 != null)
		{
			return new OutOfBandData(ptr6);
		}
		ImExportData* ptr7 = (ImExportData*)global::_003CModule_003E.__RTDynamicCast(nativeData, 0, System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImData_0040NmToolkit_0040_0040_00408), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImExportData_0040NmToolkit_0040_0040_00408), 0);
		if (ptr7 != null)
		{
			return new ExportData(ptr7);
		}
		ImEventData* ptr8 = (ImEventData*)global::_003CModule_003E.__RTDynamicCast(nativeData, 0, System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImData_0040NmToolkit_0040_0040_00408), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImEventData_0040NmToolkit_0040_0040_00408), 0);
		if (ptr8 != null)
		{
			return new EventData(ptr8);
		}
		ImSearchData* ptr9 = (ImSearchData*)global::_003CModule_003E.__RTDynamicCast(nativeData, 0, System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImData_0040NmToolkit_0040_0040_00408), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImSearchData_0040NmToolkit_0040_0040_00408), 0);
		if (ptr9 != null)
		{
			return new SearchData(ptr9);
		}
		if (nativeData != null)
		{
			return new Data(nativeData);
		}
		return null;
	}
}
