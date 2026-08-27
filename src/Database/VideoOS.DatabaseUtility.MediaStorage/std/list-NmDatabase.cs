using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using _003FA0x9b6941da;

namespace std;

[StructLayout(LayoutKind.Sequential, Size = 16)]
[NativeCppClass]
public struct list_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E
{
	private long _003Calignment_0020member_003E;

	[SpecialName]
	public unsafe static void _003CMarshalCopy_003E(list_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E* A_0, list_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E* A_1)
	{
		//IL_000a: Expected I4, but got I8
		System.Runtime.CompilerServices.Unsafe.SkipInit(out __clr_placement_new_t _clr_placement_new_t);
		// IL initblk instruction
		System.Runtime.CompilerServices.Unsafe.InitBlockUnaligned(ref _clr_placement_new_t, 0, 1);
		__clr_placement_new_t* ptr = &_clr_placement_new_t;
		__clr_placement_new_t _clr_placement_new_t2 = _clr_placement_new_t;
		try
		{
			if (A_0 != null)
			{
				global::_003CModule_003E.std_002Elist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_002E_007Bctor_007D(A_0, A_1);
			}
			return;
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.delete(A_0, A_0, *ptr);
			throw;
		}
	}

	[SpecialName]
	public unsafe static void _003CMarshalDestroy_003E(list_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E* A_0)
	{
		global::_003CModule_003E.std_002Elist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_002E_Tidy(A_0);
	}
}
[StructLayout(LayoutKind.Sequential, Size = 16)]
[NativeCppClass]
public struct list_003CNmDatabase_003A_003AUtility_003A_003ASmVerificationTimeSlice_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ASmVerificationTimeSlice_003E_0020_003E
{
	private long _003Calignment_0020member_003E;
}
