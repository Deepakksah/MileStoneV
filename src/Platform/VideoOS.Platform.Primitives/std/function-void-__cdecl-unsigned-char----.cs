using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace std;

[StructLayout(LayoutKind.Sequential, Size = 64)]
[NativeCppClass]
[UnsafeValueType]
public struct function_003Cvoid_0020__cdecl_0028unsigned_0020char_0020_002A_0029_003E
{
	private long _003Calignment_0020member_003E;

	[SpecialName]
	public unsafe static void _003CMarshalCopy_003E(function_003Cvoid_0020__cdecl_0028unsigned_0020char_0020_002A_0029_003E* A_0, function_003Cvoid_0020__cdecl_0028unsigned_0020char_0020_002A_0029_003E* A_1)
	{
		if (A_0 != null)
		{
			*(long*)((ulong)(nint)A_0 + 56uL) = 0L;
			try
			{
				global::_003CModule_003E.std_002E_Func_class_003Cvoid_002Cunsigned_0020char_0020_002A_003E_002E_Reset_move((_Func_class_003Cvoid_002Cunsigned_0020char_0020_002A_003E*)A_0, (_Func_class_003Cvoid_002Cunsigned_0020char_0020_002A_003E*)A_1);
				return;
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<_Func_class_003Cvoid_002Cunsigned_0020char_0020_002A_003E*, void>)(&global::_003CModule_003E.std_002E_Func_class_003Cvoid_002Cunsigned_0020char_0020_002A_003E_002E_007Bdtor_007D), A_0);
				throw;
			}
		}
	}

	[SpecialName]
	public unsafe static void _003CMarshalDestroy_003E(function_003Cvoid_0020__cdecl_0028unsigned_0020char_0020_002A_0029_003E* A_0)
	{
		global::_003CModule_003E.std_002E_Func_class_003Cvoid_002Cunsigned_0020char_0020_002A_003E_002E_Tidy((_Func_class_003Cvoid_002Cunsigned_0020char_0020_002A_003E*)A_0);
	}
}
