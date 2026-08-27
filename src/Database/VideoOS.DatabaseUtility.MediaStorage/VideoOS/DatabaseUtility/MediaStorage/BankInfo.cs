using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using NmDatabase.Utility;
using NmError;
using NmFile;
using msclr.interop;
using std;

namespace VideoOS.DatabaseUtility.MediaStorage;

public class BankInfo : IDisposable
{
	private unsafe CmBankInfo* m_BankInfo;

	public unsafe IDictionary<TableInfo, IList<TableInfo>> ArchiveMap
	{
		get
		{
			//IL_0009: Expected I8, but got I
			//IL_02d8: Expected I, but got I8
			//IL_0285: Expected I, but got I8
			//IL_0033: Expected I, but got I8
			//IL_003f: Expected I, but got I8
			//IL_0056: Expected I, but got I8
			//IL_005b: Expected I8, but got I
			//IL_007a: Expected I, but got I8
			//IL_009e: Expected I, but got I8
			//IL_00f8: Expected I, but got I8
			//IL_012e: Expected I, but got I8
			//IL_014f: Expected I, but got I8
			//IL_01d9: Expected I, but got I8
			//IL_0179: Expected I, but got I8
			long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CmMediaStorageUtilityError* ptr21);
			try
			{
				System.Runtime.CompilerServices.Unsafe.SkipInit(out map_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_002Cstd_003A_003Aless_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_002Cstd_003A_003Aallocator_003Cstd_003A_003Apair_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_0020const_0020_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E_0020_003E_0020_003E obj);
				global::_003CModule_003E.NmDatabase_002EUtility_002ECmBankInfo_002EGetArchiveMap(m_BankInfo, &obj);
				IDictionary<TableInfo, IList<TableInfo>> dictionary;
				try
				{
					dictionary = new Dictionary<TableInfo, IList<TableInfo>>();
					map_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_002Cstd_003A_003Aless_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_002Cstd_003A_003Aallocator_003Cstd_003A_003Apair_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_0020const_0020_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E_0020_003E_0020_003E* ptr = &obj;
					_Tree_val_003Cstd_003A_003A_Tree_simple_types_003Cstd_003A_003Apair_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_0020const_0020_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E_0020_003E_0020_003E* ptr2 = (_Tree_val_003Cstd_003A_003A_Tree_simple_types_003Cstd_003A_003Apair_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_0020const_0020_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E_0020_003E_0020_003E*)(&obj);
					_Tree_val_003Cstd_003A_003A_Tree_simple_types_003Cstd_003A_003Apair_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_0020const_0020_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E_0020_003E_0020_003E* ptr3 = (_Tree_val_003Cstd_003A_003A_Tree_simple_types_003Cstd_003A_003Apair_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_0020const_0020_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E_0020_003E_0020_003E*)(&obj);
					_Tree_val_003Cstd_003A_003A_Tree_simple_types_003Cstd_003A_003Apair_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_0020const_0020_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E_0020_003E_0020_003E* ptr4 = (_Tree_val_003Cstd_003A_003A_Tree_simple_types_003Cstd_003A_003Apair_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_0020const_0020_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E_0020_003E_0020_003E*)(&obj);
					_Tree_node_003Cstd_003A_003Apair_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_0020const_0020_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E_002Cvoid_0020_002A_003E* ptr5 = (_Tree_node_003Cstd_003A_003Apair_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_0020const_0020_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E_002Cvoid_0020_002A_003E*)(*(ulong*)(&obj));
					System.Runtime.CompilerServices.Unsafe.SkipInit(out _Tree_iterator_003Cstd_003A_003A_Tree_val_003Cstd_003A_003A_Tree_simple_types_003Cstd_003A_003Apair_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_0020const_0020_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E_0020_003E_0020_003E_0020_003E obj2);
					global::_003CModule_003E.std_002E_Tree_unchecked_const_iterator_003Cstd_003A_003A_Tree_val_003Cstd_003A_003A_Tree_simple_types_003Cstd_003A_003Apair_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_0020const_0020_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E_0020_003E_0020_003E_002Cstd_003A_003A_Iterator_base0_003E_002E_007Bctor_007D((_Tree_unchecked_const_iterator_003Cstd_003A_003A_Tree_val_003Cstd_003A_003A_Tree_simple_types_003Cstd_003A_003Apair_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_0020const_0020_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E_0020_003E_0020_003E_002Cstd_003A_003A_Iterator_base0_003E*)(&obj2), (_Tree_node_003Cstd_003A_003Apair_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_0020const_0020_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E_002Cvoid_0020_002A_003E*)(*(ulong*)(&obj)), (_Tree_val_003Cstd_003A_003A_Tree_simple_types_003Cstd_003A_003Apair_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_0020const_0020_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E_0020_003E_0020_003E*)(&obj));
					_Tree_const_iterator_003Cstd_003A_003A_Tree_val_003Cstd_003A_003A_Tree_simple_types_003Cstd_003A_003Apair_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_0020const_0020_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E_0020_003E_0020_003E_0020_003E obj3 = (_Tree_const_iterator_003Cstd_003A_003A_Tree_val_003Cstd_003A_003A_Tree_simple_types_003Cstd_003A_003Apair_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_0020const_0020_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E_0020_003E_0020_003E_0020_003E)obj2;
					_Tree_val_003Cstd_003A_003A_Tree_simple_types_003Cstd_003A_003Apair_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_0020const_0020_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E_0020_003E_0020_003E* ptr6 = (_Tree_val_003Cstd_003A_003A_Tree_simple_types_003Cstd_003A_003Apair_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_0020const_0020_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E_0020_003E_0020_003E*)(&obj);
					_Tree_val_003Cstd_003A_003A_Tree_simple_types_003Cstd_003A_003Apair_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_0020const_0020_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E_0020_003E_0020_003E* ptr7 = (_Tree_val_003Cstd_003A_003A_Tree_simple_types_003Cstd_003A_003Apair_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_0020const_0020_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E_0020_003E_0020_003E*)(&obj);
					_Tree_val_003Cstd_003A_003A_Tree_simple_types_003Cstd_003A_003Apair_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_0020const_0020_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E_0020_003E_0020_003E* ptr8 = (_Tree_val_003Cstd_003A_003A_Tree_simple_types_003Cstd_003A_003Apair_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_0020const_0020_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E_0020_003E_0020_003E*)(&obj);
					_Tree_node_003Cstd_003A_003Apair_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_0020const_0020_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E_002Cvoid_0020_002A_003E* ptr9 = (_Tree_node_003Cstd_003A_003Apair_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_0020const_0020_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E_002Cvoid_0020_002A_003E*)(*(ulong*)(*(ulong*)(&obj)));
					System.Runtime.CompilerServices.Unsafe.SkipInit(out _Tree_const_iterator_003Cstd_003A_003A_Tree_val_003Cstd_003A_003A_Tree_simple_types_003Cstd_003A_003Apair_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_0020const_0020_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E_0020_003E_0020_003E_0020_003E obj4);
					*(long*)(&obj4) = (nint)ptr9;
					System.Runtime.CompilerServices.Unsafe.SkipInit(out pair_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_0020const_0020_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E obj5);
					System.Runtime.CompilerServices.Unsafe.SkipInit(out _List_iterator_003Cstd_003A_003A_List_val_003Cstd_003A_003A_List_simple_types_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E obj6);
					System.Runtime.CompilerServices.Unsafe.SkipInit(out _List_iterator_003Cstd_003A_003A_List_val_003Cstd_003A_003A_List_simple_types_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E obj8);
					System.Runtime.CompilerServices.Unsafe.SkipInit(out CmTableInfo cmTableInfo);
					while (true)
					{
						byte b = ((*(long*)(&obj4) == *(long*)(&obj3)) ? ((byte)1) : ((byte)0));
						byte b2 = b;
						if (b != 0)
						{
							break;
						}
						pair_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_0020const_0020_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E* ptr10 = (pair_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_0020const_0020_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E*)(*(long*)(&obj4) + 32);
						pair_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_0020const_0020_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E* ptr11 = ptr10;
						global::_003CModule_003E.NmDatabase_002EUtility_002ECmTableInfo_002E_007Bctor_007D((CmTableInfo*)(&obj5), (CmTableInfo*)ptr10);
						try
						{
							global::_003CModule_003E.std_002Elist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_002E_007Bctor_007D((list_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref obj5, 344)), (list_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E*)((ulong)(nint)ptr10 + 344uL));
						}
						catch
						{
							//try-fault
							global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmTableInfo*, void>)(&global::_003CModule_003E.NmDatabase_002EUtility_002ECmTableInfo_002E_007Bdtor_007D), &obj5);
							throw;
						}
						try
						{
							IList<TableInfo> list = new List<TableInfo>();
							list_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E* ptr12 = (list_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref obj5, 344));
							_List_val_003Cstd_003A_003A_List_simple_types_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E* ptr13 = (_List_val_003Cstd_003A_003A_List_simple_types_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref obj5, 344));
							long num2 = (nint)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref obj5, 344));
							long num3 = System.Runtime.CompilerServices.Unsafe.As<pair_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_0020const_0020_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref obj5, 344));
							global::_003CModule_003E.std_002E_List_unchecked_const_iterator_003Cstd_003A_003A_List_val_003Cstd_003A_003A_List_simple_types_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_002Cstd_003A_003A_Iterator_base0_003E_002E_007Bctor_007D((_List_unchecked_const_iterator_003Cstd_003A_003A_List_val_003Cstd_003A_003A_List_simple_types_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_002Cstd_003A_003A_Iterator_base0_003E*)(&obj6), (_List_node_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cvoid_0020_002A_003E*)System.Runtime.CompilerServices.Unsafe.As<pair_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_0020const_0020_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E, ulong>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref obj5, 344)), (_List_val_003Cstd_003A_003A_List_simple_types_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref obj5, 344)));
							_List_const_iterator_003Cstd_003A_003A_List_val_003Cstd_003A_003A_List_simple_types_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E obj7 = (_List_const_iterator_003Cstd_003A_003A_List_val_003Cstd_003A_003A_List_simple_types_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E)obj6;
							_List_val_003Cstd_003A_003A_List_simple_types_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E* ptr14 = (_List_val_003Cstd_003A_003A_List_simple_types_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref obj5, 344));
							long num4 = (nint)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref obj5, 344));
							long num5 = *(long*)System.Runtime.CompilerServices.Unsafe.As<pair_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_0020const_0020_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E, ulong>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref obj5, 344));
							global::_003CModule_003E.std_002E_List_unchecked_const_iterator_003Cstd_003A_003A_List_val_003Cstd_003A_003A_List_simple_types_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_002Cstd_003A_003A_Iterator_base0_003E_002E_007Bctor_007D((_List_unchecked_const_iterator_003Cstd_003A_003A_List_val_003Cstd_003A_003A_List_simple_types_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_002Cstd_003A_003A_Iterator_base0_003E*)(&obj8), (_List_node_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cvoid_0020_002A_003E*)num5, (_List_val_003Cstd_003A_003A_List_simple_types_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref obj5, 344)));
							_List_const_iterator_003Cstd_003A_003A_List_val_003Cstd_003A_003A_List_simple_types_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E obj9 = (_List_const_iterator_003Cstd_003A_003A_List_val_003Cstd_003A_003A_List_simple_types_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E)obj8;
							while (true)
							{
								byte b3 = ((*(long*)(&obj9) == *(long*)(&obj7)) ? ((byte)1) : ((byte)0));
								byte b4 = b3;
								if (b3 != 0)
								{
									break;
								}
								CmTableInfo* a_ = (CmTableInfo*)(*(long*)(&obj9) + 16);
								global::_003CModule_003E.NmDatabase_002EUtility_002ECmTableInfo_002E_007Bctor_007D(&cmTableInfo, a_);
								try
								{
									CmTableInfo* ptr15 = (CmTableInfo*)global::_003CModule_003E.@new(344uL);
									CmTableInfo* ptr16;
									try
									{
										ptr16 = ((ptr15 == null) ? null : global::_003CModule_003E.NmDatabase_002EUtility_002ECmTableInfo_002E_007Bctor_007D(ptr15, &cmTableInfo));
										CmTableInfo* ptr17 = ptr16;
									}
									catch
									{
										//try-fault
										global::_003CModule_003E.delete(ptr15, 344uL);
										throw;
									}
									list.Add(new TableInfo(ptr16));
								}
								catch
								{
									//try-fault
									global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmTableInfo*, void>)(&global::_003CModule_003E.NmDatabase_002EUtility_002ECmTableInfo_002E_007Bdtor_007D), &cmTableInfo);
									throw;
								}
								global::_003CModule_003E.NmDatabase_002EUtility_002ECmTableInfo_002E_007Bdtor_007D(&cmTableInfo);
								*(long*)(&obj9) = *(long*)(*(ulong*)(&obj9));
							}
							CmTableInfo* ptr18 = (CmTableInfo*)global::_003CModule_003E.@new(344uL);
							CmTableInfo* ptr19;
							try
							{
								ptr19 = ((ptr18 == null) ? null : global::_003CModule_003E.NmDatabase_002EUtility_002ECmTableInfo_002E_007Bctor_007D(ptr18, (CmTableInfo*)(&obj5)));
								CmTableInfo* ptr20 = ptr19;
							}
							catch
							{
								//try-fault
								global::_003CModule_003E.delete(ptr18, 344uL);
								throw;
							}
							dictionary.Add(new TableInfo(ptr19), list);
						}
						catch
						{
							//try-fault
							global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<pair_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_0020const_0020_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Epair_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_0020const_0020_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E_002E_007Bdtor_007D), &obj5);
							throw;
						}
						try
						{
							global::_003CModule_003E.std_002Elist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_002E_Tidy((list_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref obj5, 344)));
						}
						catch
						{
							//try-fault
							global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmTableInfo*, void>)(&global::_003CModule_003E.NmDatabase_002EUtility_002ECmTableInfo_002E_007Bdtor_007D), &obj5);
							throw;
						}
						global::_003CModule_003E.NmDatabase_002EUtility_002ECmTableInfo_002E_007Bdtor_007D((CmTableInfo*)(&obj5));
						global::_003CModule_003E.std_002E_Tree_unchecked_const_iterator_003Cstd_003A_003A_Tree_val_003Cstd_003A_003A_Tree_simple_types_003Cstd_003A_003Apair_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_0020const_0020_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E_0020_003E_0020_003E_002Cstd_003A_003A_Iterator_base0_003E_002E_002B_002B((_Tree_unchecked_const_iterator_003Cstd_003A_003A_Tree_val_003Cstd_003A_003A_Tree_simple_types_003Cstd_003A_003Apair_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_0020const_0020_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E_0020_003E_0020_003E_002Cstd_003A_003A_Iterator_base0_003E*)(&obj4));
					}
					IDictionary<TableInfo, IList<TableInfo>> dictionary2 = dictionary;
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<map_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_002Cstd_003A_003Aless_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_002Cstd_003A_003Aallocator_003Cstd_003A_003Apair_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_0020const_0020_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E_0020_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Emap_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_002Cstd_003A_003Aless_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_002Cstd_003A_003Aallocator_003Cstd_003A_003Apair_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_0020const_0020_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E_0020_003E_0020_003E_002E_007Bdtor_007D), &obj);
					throw;
				}
				global::_003CModule_003E.std_002E_Tree_003Cstd_003A_003A_Tmap_traits_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_002Cstd_003A_003Aless_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_002Cstd_003A_003Aallocator_003Cstd_003A_003Apair_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_0020const_0020_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E_0020_003E_002C0_003E_0020_003E_002E_007Bdtor_007D((_Tree_003Cstd_003A_003A_Tmap_traits_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_002Cstd_003A_003Aless_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_002Cstd_003A_003Aallocator_003Cstd_003A_003Apair_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_0020const_0020_002Cstd_003A_003Alist_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_002Cstd_003A_003Aallocator_003CNmDatabase_003A_003AUtility_003A_003ACmTableInfo_003E_0020_003E_0020_003E_0020_003E_002C0_003E_0020_003E*)(&obj));
				return dictionary;
			}
			catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVCmMediaStorageUtilityError_0040Utility_0040NmDatabase_0040_0040_00408), 9, &ptr21) != 0)
			{
				uint num6 = 0u;
				global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
				try
				{
					try
					{
						System.Runtime.CompilerServices.Unsafe.SkipInit(out std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E obj11);
						std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E* from_obj = global::_003CModule_003E.NmError_002ECmError_002EGetErrorDescription((CmError*)ptr21, &obj11);
						try
						{
							throw new MediaStorageUtilityException(global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020System_003A_003AString_0020_005E_002Cclass_0020std_003A_003Abasic_string_003Cchar_002Cstruct_0020std_003A_003Achar_traits_003Cchar_003E_002Cclass_0020std_003A_003Aallocator_003Cchar_003E_0020_003E_0020_003E(from_obj), (MediaStorageUtilityException.ErrorCode)global::_003CModule_003E.NmDatabase_002EUtility_002ECmMediaStorageUtilityError_002EGetErrorCode(ptr21));
						}
						catch
						{
							//try-fault
							global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_007Bdtor_007D), &obj11);
							throw;
						}
					}
					catch when (((Func<bool>)delegate
					{
						// Could not convert BlockContainer to single expression
						num6 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
						return (byte)num6 != 0;
					}).Invoke())
					{
					}
					if (num6 != 0)
					{
						throw;
					}
				}
				finally
				{
					global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num6);
				}
			}
			return null;
		}
	}

	public unsafe bool Encrypted
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			bool result = global::_003CModule_003E.NmDatabase_002EUtility_002ECmBankInfo_002EIsEncrypted(m_BankInfo);
			GC.KeepAlive(this);
			return result;
		}
	}

	public unsafe BankInfo(string bankPath, string password)
	{
		//IL_0093: Expected I, but got I8
		uint num = 0u;
		CmBankInfo* ptr = (CmBankInfo*)global::_003CModule_003E.@new(120uL);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out msclr.interop.utf8string_t utf8string_t2);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out msclr.interop.utf8string_t utf8string_t3);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out CmDirectory cmDirectory);
		CmBankInfo* bankInfo;
		try
		{
			if (ptr != null)
			{
				msclr.interop.utf8string_t* ptr2 = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020msclr_003A_003Ainterop_003A_003Autf8string_t_002Cclass_0020System_003A_003AString_0020_005E_003E(&utf8string_t2, &password);
				try
				{
					num = 1u;
					msclr.interop.utf8string_t* ptr3 = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020msclr_003A_003Ainterop_003A_003Autf8string_t_002Cclass_0020System_003A_003AString_0020_005E_003E(&utf8string_t3, &bankPath);
					try
					{
						num = 3u;
						global::_003CModule_003E.NmFile_002ECmDirectory_002E_007Bctor_007D(&cmDirectory, (std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*)ptr3);
						try
						{
							num = 7u;
							bankInfo = global::_003CModule_003E.NmDatabase_002EUtility_002ECmBankInfo_002E_007Bctor_007D(ptr, &cmDirectory, (std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*)ptr2);
						}
						catch
						{
							//try-fault
							if ((num & 4) != 0)
							{
								num &= 0xFFFFFFFBu;
								global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmDirectory*, void>)(&global::_003CModule_003E.NmFile_002ECmDirectory_002E_007Bdtor_007D), &cmDirectory);
							}
							throw;
						}
					}
					catch
					{
						//try-fault
						if ((num & 2) != 0)
						{
							num &= 0xFFFFFFFDu;
							global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<msclr.interop.utf8string_t*, void>)(&global::_003CModule_003E.msclr_002Einterop_002Eutf8string_t_002E_007Bdtor_007D), &utf8string_t3);
						}
						throw;
					}
				}
				catch
				{
					//try-fault
					if ((num & 1) != 0)
					{
						num &= 0xFFFFFFFEu;
						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<msclr.interop.utf8string_t*, void>)(&global::_003CModule_003E.msclr_002Einterop_002Eutf8string_t_002E_007Bdtor_007D), &utf8string_t2);
					}
					throw;
				}
			}
			else
			{
				bankInfo = null;
			}
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.delete(ptr, 120uL);
			throw;
		}
		try
		{
			try
			{
				try
				{
					m_BankInfo = bankInfo;
				}
				catch
				{
					//try-fault
					if ((num & 4) != 0)
					{
						num &= 0xFFFFFFFBu;
						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmDirectory*, void>)(&global::_003CModule_003E.NmFile_002ECmDirectory_002E_007Bdtor_007D), &cmDirectory);
					}
					throw;
				}
				if ((num & 4) != 0)
				{
					num &= 0xFFFFFFFBu;
					global::_003CModule_003E.NmFile_002ECmDirectory_002E_007Bdtor_007D(&cmDirectory);
				}
			}
			catch
			{
				//try-fault
				if ((num & 2) != 0)
				{
					num &= 0xFFFFFFFDu;
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<msclr.interop.utf8string_t*, void>)(&global::_003CModule_003E.msclr_002Einterop_002Eutf8string_t_002E_007Bdtor_007D), &utf8string_t3);
				}
				throw;
			}
			if ((num & 2) != 0)
			{
				num &= 0xFFFFFFFDu;
				try
				{
					global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_Tidy_deallocate((std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*)(&utf8string_t3));
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std._Compressed_pair_003Cstd_003A_003Aallocator_003Cchar_003E_002Cstd_003A_003A_String_val_003Cstd_003A_003A_Simple_types_003Cchar_003E_0020_003E_002C1_003E*, void>)(&global::_003CModule_003E.std_002E_Compressed_pair_003Cstd_003A_003Aallocator_003Cchar_003E_002Cstd_003A_003A_String_val_003Cstd_003A_003A_Simple_types_003Cchar_003E_0020_003E_002C1_003E_002E_007Bdtor_007D), &utf8string_t3);
					throw;
				}
			}
		}
		catch
		{
			//try-fault
			if ((num & 1) != 0)
			{
				num &= 0xFFFFFFFEu;
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<msclr.interop.utf8string_t*, void>)(&global::_003CModule_003E.msclr_002Einterop_002Eutf8string_t_002E_007Bdtor_007D), &utf8string_t2);
			}
			throw;
		}
		if ((num & 1) != 0)
		{
			num &= 0xFFFFFFFEu;
			try
			{
				global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_Tidy_deallocate((std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*)(&utf8string_t2));
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std._Compressed_pair_003Cstd_003A_003Aallocator_003Cchar_003E_002Cstd_003A_003A_String_val_003Cstd_003A_003A_Simple_types_003Cchar_003E_0020_003E_002C1_003E*, void>)(&global::_003CModule_003E.std_002E_Compressed_pair_003Cstd_003A_003Aallocator_003Cchar_003E_002Cstd_003A_003A_String_val_003Cstd_003A_003A_Simple_types_003Cchar_003E_0020_003E_002C1_003E_002E_007Bdtor_007D), &utf8string_t2);
				throw;
			}
		}
		base._002Ector();
	}

	public unsafe BankInfo(string bankPath)
	{
		//IL_0067: Expected I, but got I8
		uint num = 0u;
		CmBankInfo* ptr = (CmBankInfo*)global::_003CModule_003E.@new(120uL);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out msclr.interop.utf8string_t utf8string_t2);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out CmDirectory cmDirectory);
		CmBankInfo* bankInfo;
		try
		{
			if (ptr != null)
			{
				msclr.interop.utf8string_t* ptr2 = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020msclr_003A_003Ainterop_003A_003Autf8string_t_002Cclass_0020System_003A_003AString_0020_005E_003E(&utf8string_t2, &bankPath);
				try
				{
					num = 1u;
					global::_003CModule_003E.NmFile_002ECmDirectory_002E_007Bctor_007D(&cmDirectory, (std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*)ptr2);
					try
					{
						num = 3u;
						bankInfo = global::_003CModule_003E.NmDatabase_002EUtility_002ECmBankInfo_002E_007Bctor_007D(ptr, &cmDirectory);
					}
					catch
					{
						//try-fault
						if ((num & 2) != 0)
						{
							num &= 0xFFFFFFFDu;
							global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmDirectory*, void>)(&global::_003CModule_003E.NmFile_002ECmDirectory_002E_007Bdtor_007D), &cmDirectory);
						}
						throw;
					}
				}
				catch
				{
					//try-fault
					if ((num & 1) != 0)
					{
						num &= 0xFFFFFFFEu;
						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<msclr.interop.utf8string_t*, void>)(&global::_003CModule_003E.msclr_002Einterop_002Eutf8string_t_002E_007Bdtor_007D), &utf8string_t2);
					}
					throw;
				}
			}
			else
			{
				bankInfo = null;
			}
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.delete(ptr, 120uL);
			throw;
		}
		try
		{
			try
			{
				m_BankInfo = bankInfo;
			}
			catch
			{
				//try-fault
				if ((num & 2) != 0)
				{
					num &= 0xFFFFFFFDu;
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmDirectory*, void>)(&global::_003CModule_003E.NmFile_002ECmDirectory_002E_007Bdtor_007D), &cmDirectory);
				}
				throw;
			}
			if ((num & 2) != 0)
			{
				num &= 0xFFFFFFFDu;
				global::_003CModule_003E.NmFile_002ECmDirectory_002E_007Bdtor_007D(&cmDirectory);
			}
		}
		catch
		{
			//try-fault
			if ((num & 1) != 0)
			{
				num &= 0xFFFFFFFEu;
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<msclr.interop.utf8string_t*, void>)(&global::_003CModule_003E.msclr_002Einterop_002Eutf8string_t_002E_007Bdtor_007D), &utf8string_t2);
			}
			throw;
		}
		if ((num & 1) != 0)
		{
			num &= 0xFFFFFFFEu;
			try
			{
				global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_Tidy_deallocate((std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*)(&utf8string_t2));
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std._Compressed_pair_003Cstd_003A_003Aallocator_003Cchar_003E_002Cstd_003A_003A_String_val_003Cstd_003A_003A_Simple_types_003Cchar_003E_0020_003E_002C1_003E*, void>)(&global::_003CModule_003E.std_002E_Compressed_pair_003Cstd_003A_003Aallocator_003Cchar_003E_002Cstd_003A_003A_String_val_003Cstd_003A_003A_Simple_types_003Cchar_003E_0020_003E_002C1_003E_002E_007Bdtor_007D), &utf8string_t2);
				throw;
			}
		}
		base._002Ector();
	}

	private void _007EBankInfo()
	{
		_0021BankInfo();
	}

	private unsafe void _0021BankInfo()
	{
		CmBankInfo* bankInfo = m_BankInfo;
		if (bankInfo != null)
		{
			global::_003CModule_003E.NmDatabase_002EUtility_002ECmBankInfo_002E_007Bdtor_007D(bankInfo);
			global::_003CModule_003E.delete(bankInfo, 120uL);
		}
		GC.KeepAlive(this);
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe static bool IsEncrypted(string bankPath)
	{
		uint num = 0u;
		System.Runtime.CompilerServices.Unsafe.SkipInit(out msclr.interop.utf8string_t utf8string_t2);
		msclr.interop.utf8string_t* ptr = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020msclr_003A_003Ainterop_003A_003Autf8string_t_002Cclass_0020System_003A_003AString_0020_005E_003E(&utf8string_t2, &bankPath);
		bool result;
		try
		{
			result = global::_003CModule_003E.NmDatabase_002EUtility_002ECmBankInfo_002EIsEncrypted((std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*)ptr);
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<msclr.interop.utf8string_t*, void>)(&global::_003CModule_003E.msclr_002Einterop_002Eutf8string_t_002E_007Bdtor_007D), &utf8string_t2);
			throw;
		}
		try
		{
			global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_Tidy_deallocate((std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*)(&utf8string_t2));
			return result;
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std._Compressed_pair_003Cstd_003A_003Aallocator_003Cchar_003E_002Cstd_003A_003A_String_val_003Cstd_003A_003A_Simple_types_003Cchar_003E_0020_003E_002C1_003E*, void>)(&global::_003CModule_003E.std_002E_Compressed_pair_003Cstd_003A_003Aallocator_003Cchar_003E_002Cstd_003A_003A_String_val_003Cstd_003A_003A_Simple_types_003Cchar_003E_0020_003E_002C1_003E_002E_007Bdtor_007D), &utf8string_t2);
			throw;
		}
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe static bool IsPasswordValid(string bankPath, string password)
	{
		//IL_0008: Expected I8, but got I
		//IL_018e: Expected I, but got I8
		//IL_0140: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		System.Runtime.CompilerServices.Unsafe.SkipInit(out CmMediaStorageUtilityError* ptr5);
		try
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out msclr.interop.utf8string_t utf8string_t2);
			msclr.interop.utf8string_t* ptr = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020msclr_003A_003Ainterop_003A_003Autf8string_t_002Cclass_0020System_003A_003AString_0020_005E_003E(&utf8string_t2, &bankPath);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CmBankInfo cmBankInfo);
			try
			{
				System.Runtime.CompilerServices.Unsafe.SkipInit(out CmDirectory cmDirectory);
				CmDirectory* ptr2 = global::_003CModule_003E.NmFile_002ECmDirectory_002E_007Bctor_007D(&cmDirectory, (std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*)ptr);
				try
				{
					global::_003CModule_003E.NmDatabase_002EUtility_002ECmBankInfo_002E_007Bctor_007D(&cmBankInfo, ptr2);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmDirectory*, void>)(&global::_003CModule_003E.NmFile_002ECmDirectory_002E_007Bdtor_007D), &cmDirectory);
					throw;
				}
				try
				{
					global::_003CModule_003E.NmFile_002ECmDirectory_002E_007Bdtor_007D(&cmDirectory);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmBankInfo*, void>)(&global::_003CModule_003E.NmDatabase_002EUtility_002ECmBankInfo_002E_007Bdtor_007D), &cmBankInfo);
					throw;
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<msclr.interop.utf8string_t*, void>)(&global::_003CModule_003E.msclr_002Einterop_002Eutf8string_t_002E_007Bdtor_007D), &utf8string_t2);
				throw;
			}
			bool result;
			try
			{
				try
				{
					global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_Tidy_deallocate((std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*)(&utf8string_t2));
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std._Compressed_pair_003Cstd_003A_003Aallocator_003Cchar_003E_002Cstd_003A_003A_String_val_003Cstd_003A_003A_Simple_types_003Cchar_003E_0020_003E_002C1_003E*, void>)(&global::_003CModule_003E.std_002E_Compressed_pair_003Cstd_003A_003Aallocator_003Cchar_003E_002Cstd_003A_003A_String_val_003Cstd_003A_003A_Simple_types_003Cchar_003E_0020_003E_002C1_003E_002E_007Bdtor_007D), &utf8string_t2);
					throw;
				}
				System.Runtime.CompilerServices.Unsafe.SkipInit(out std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E obj);
				std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E* ptr3 = &obj;
				System.Runtime.CompilerServices.Unsafe.SkipInit(out msclr.interop.utf8string_t utf8string_t3);
				global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_007Bctor_007D((std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*)(&utf8string_t3), (sbyte*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_C_0040_00CNPNBAHC_0040_0040));
				try
				{
					System.Runtime.CompilerServices.Unsafe.SkipInit(out msclr.interop.utf8string_t utf8string_t4);
					msclr.interop.utf8string_t* right = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as(&utf8string_t4, &password, &utf8string_t3);
					try
					{
						std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E* ptr4 = global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_007Bctor_007D(&obj, (std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*)right);
						result = global::_003CModule_003E.NmDatabase_002EUtility_002ECmBankInfo_002EIsPasswordValid(&cmBankInfo, ptr4);
					}
					catch
					{
						//try-fault
						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<msclr.interop.utf8string_t*, void>)(&global::_003CModule_003E.msclr_002Einterop_002Eutf8string_t_002E_007Bdtor_007D), &utf8string_t4);
						throw;
					}
					try
					{
						global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_Tidy_deallocate((std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*)(&utf8string_t4));
					}
					catch
					{
						//try-fault
						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std._Compressed_pair_003Cstd_003A_003Aallocator_003Cchar_003E_002Cstd_003A_003A_String_val_003Cstd_003A_003A_Simple_types_003Cchar_003E_0020_003E_002C1_003E*, void>)(&global::_003CModule_003E.std_002E_Compressed_pair_003Cstd_003A_003Aallocator_003Cchar_003E_002Cstd_003A_003A_String_val_003Cstd_003A_003A_Simple_types_003Cchar_003E_0020_003E_002C1_003E_002E_007Bdtor_007D), &utf8string_t4);
						throw;
					}
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<msclr.interop.utf8string_t*, void>)(&global::_003CModule_003E.msclr_002Einterop_002Eutf8string_t_002E_007Bdtor_007D), &utf8string_t3);
					throw;
				}
				try
				{
					global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_Tidy_deallocate((std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*)(&utf8string_t3));
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std._Compressed_pair_003Cstd_003A_003Aallocator_003Cchar_003E_002Cstd_003A_003A_String_val_003Cstd_003A_003A_Simple_types_003Cchar_003E_0020_003E_002C1_003E*, void>)(&global::_003CModule_003E.std_002E_Compressed_pair_003Cstd_003A_003Aallocator_003Cchar_003E_002Cstd_003A_003A_String_val_003Cstd_003A_003A_Simple_types_003Cchar_003E_0020_003E_002C1_003E_002E_007Bdtor_007D), &utf8string_t3);
					throw;
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmBankInfo*, void>)(&global::_003CModule_003E.NmDatabase_002EUtility_002ECmBankInfo_002E_007Bdtor_007D), &cmBankInfo);
				throw;
			}
			global::_003CModule_003E.NmDatabase_002EUtility_002ECmBankInfo_002E_007Bdtor_007D(&cmBankInfo);
			return result;
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVCmMediaStorageUtilityError_0040Utility_0040NmDatabase_0040_0040_00408), 9, &ptr5) != 0)
		{
			uint num2 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					System.Runtime.CompilerServices.Unsafe.SkipInit(out std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E obj3);
					std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E* from_obj = global::_003CModule_003E.NmError_002ECmError_002EGetErrorDescription((CmError*)ptr5, &obj3);
					try
					{
						throw new MediaStorageUtilityException(global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020System_003A_003AString_0020_005E_002Cclass_0020std_003A_003Abasic_string_003Cchar_002Cstruct_0020std_003A_003Achar_traits_003Cchar_003E_002Cclass_0020std_003A_003Aallocator_003Cchar_003E_0020_003E_0020_003E(from_obj), (MediaStorageUtilityException.ErrorCode)global::_003CModule_003E.NmDatabase_002EUtility_002ECmMediaStorageUtilityError_002EGetErrorCode(ptr5));
					}
					catch
					{
						//try-fault
						global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_007Bdtor_007D), &obj3);
						throw;
					}
				}
				catch when (((Func<bool>)delegate
				{
					// Could not convert BlockContainer to single expression
					num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
					return (byte)num2 != 0;
				}).Invoke())
				{
				}
				if (num2 != 0)
				{
					throw;
				}
			}
			finally
			{
				global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
			}
		}
		return false;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe static bool CanDecryptData(string bankPath)
	{
		uint num = 0u;
		System.Runtime.CompilerServices.Unsafe.SkipInit(out msclr.interop.utf8string_t utf8string_t2);
		msclr.interop.utf8string_t* ptr = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020msclr_003A_003Ainterop_003A_003Autf8string_t_002Cclass_0020System_003A_003AString_0020_005E_003E(&utf8string_t2, &bankPath);
		bool result;
		try
		{
			result = global::_003CModule_003E.NmDatabase_002EUtility_002ECmBankInfo_002ECanDecryptData((std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*)ptr);
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<msclr.interop.utf8string_t*, void>)(&global::_003CModule_003E.msclr_002Einterop_002Eutf8string_t_002E_007Bdtor_007D), &utf8string_t2);
			throw;
		}
		try
		{
			global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_Tidy_deallocate((std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*)(&utf8string_t2));
			return result;
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std._Compressed_pair_003Cstd_003A_003Aallocator_003Cchar_003E_002Cstd_003A_003A_String_val_003Cstd_003A_003A_Simple_types_003Cchar_003E_0020_003E_002C1_003E*, void>)(&global::_003CModule_003E.std_002E_Compressed_pair_003Cstd_003A_003Aallocator_003Cchar_003E_002Cstd_003A_003A_String_val_003Cstd_003A_003A_Simple_types_003Cchar_003E_0020_003E_002C1_003E_002E_007Bdtor_007D), &utf8string_t2);
			throw;
		}
	}

	[HandleProcessCorruptedStateExceptions]
	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
	{
		if (A_0)
		{
			_007EBankInfo();
			return;
		}
		try
		{
			_0021BankInfo();
		}
		finally
		{
			base.Finalize();
		}
	}

	public virtual sealed void Dispose()
	{
		Dispose(A_0: true);
		GC.SuppressFinalize(this);
		GC.KeepAlive(this);
	}

	~BankInfo()
	{
		Dispose(A_0: false);
	}
}
