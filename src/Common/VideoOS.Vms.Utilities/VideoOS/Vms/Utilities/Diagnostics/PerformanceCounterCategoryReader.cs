using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace VideoOS.Vms.Utilities.Diagnostics;

public static class PerformanceCounterCategoryReader
{
	private static class NativeMethods
	{
		public const uint ERROR_SUCCESS = 0u;

		public const int PERF_DETAIL_NOVICE = 100;

		public const int PERF_DETAIL_ADVANCED = 200;

		public const int PERF_DETAIL_EXPERT = 300;

		public const int PERF_DETAIL_WIZARD = 400;

		public const uint PDH_MORE_DATA = 2147485650u;

		[DllImport("pdh.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern uint PdhEnumObjectsW(string szDataSource, string szMachineName, char[] mszObjectList, ref uint pcchBufferSize, [MarshalAs(UnmanagedType.U4)] uint dwDetailLevel, [MarshalAs(UnmanagedType.Bool)] bool bRefresh);

		[DllImport("pdh.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern uint PdhEnumObjectItemsW(string szDataSource, string szMachineName, string szObjectName, char[] mszCounterList, ref uint pcchCounterListLength, char[] mszInstanceList, ref uint pcchInstanceListLength, [MarshalAs(UnmanagedType.U4)] uint dwDetailLevel, [MarshalAs(UnmanagedType.U4)] uint dwFlags);
	}

	public static bool RefreshCategories()
	{
		uint pcchBufferSize = 0u;
		char[] mszObjectList = new char[pcchBufferSize];
		uint num = NativeMethods.PdhEnumObjectsW(null, null, mszObjectList, ref pcchBufferSize, 400u, bRefresh: true);
		if (num == 2147485650u)
		{
			mszObjectList = new char[pcchBufferSize];
			num = NativeMethods.PdhEnumObjectsW(null, null, mszObjectList, ref pcchBufferSize, 400u, bRefresh: false);
		}
		return num == 0;
	}

	public static string[] ReadInstanceNames(string categoryName)
	{
		uint pcchCounterListLength = 0u;
		char[] mszCounterList = new char[pcchCounterListLength];
		uint pcchInstanceListLength = 0u;
		char[] array = new char[pcchInstanceListLength];
		uint num = NativeMethods.PdhEnumObjectItemsW(null, null, categoryName, mszCounterList, ref pcchCounterListLength, array, ref pcchInstanceListLength, 400u, 0u);
		if (num == 2147485650u)
		{
			mszCounterList = new char[pcchCounterListLength];
			array = new char[pcchInstanceListLength];
			num = NativeMethods.PdhEnumObjectItemsW(null, null, categoryName, mszCounterList, ref pcchCounterListLength, array, ref pcchInstanceListLength, 400u, 0u);
		}
		if (num == 0)
		{
			return ParseSZZSTR(array);
		}
		return Array.Empty<string>();
		unsafe static string[] ParseSZZSTR(char[] szzstr)
		{
			List<string> list = new List<string>();
			int i = 0;
			fixed (char* ptr = szzstr)
			{
				string text;
				for (; ptr[i] != 0; i += text.Length + 1)
				{
					text = new string(ptr + i);
					list.Add(text);
				}
			}
			return list.ToArray();
		}
	}
}
