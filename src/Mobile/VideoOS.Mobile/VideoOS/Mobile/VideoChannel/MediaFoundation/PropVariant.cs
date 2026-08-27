using System;
using System.Runtime.InteropServices;
using System.Security;

namespace VideoOS.Mobile.VideoChannel.MediaFoundation;

[StructLayout(LayoutKind.Explicit)]
public class PropVariant : ConstPropVariant
{
	[DllImport("ole32.dll", ExactSpelling = true, PreserveSig = false)]
	[SuppressUnmanagedCodeSecurity]
	protected static extern void PropVariantCopy([Out][MarshalAs(UnmanagedType.LPStruct)] PropVariant pvarDest, [In][MarshalAs(UnmanagedType.LPStruct)] ConstPropVariant pvarSource);

	[DllImport("ole32.dll", ExactSpelling = true, PreserveSig = false)]
	[SuppressUnmanagedCodeSecurity]
	protected static extern void PropVariantClear([In][MarshalAs(UnmanagedType.LPStruct)] PropVariant pvar);

	public PropVariant()
		: base(VariantType.None)
	{
	}

	public PropVariant(string value)
		: base(VariantType.String)
	{
		ptr = Marshal.StringToCoTaskMemUni(value);
	}

	public PropVariant(string[] value)
		: base(VariantType.StringArray)
	{
		calpwstrVal.cElems = value.Length;
		calpwstrVal.pElems = Marshal.AllocCoTaskMem(IntPtr.Size * value.Length);
		for (int i = 0; i < value.Length; i++)
		{
			Marshal.WriteIntPtr(calpwstrVal.pElems, i * IntPtr.Size, Marshal.StringToCoTaskMemUni(value[i]));
		}
	}

	public PropVariant(byte value)
		: base(VariantType.UByte)
	{
		bVal = value;
	}

	public PropVariant(short value)
		: base(VariantType.Short)
	{
		iVal = value;
	}

	public PropVariant(ushort value)
		: base(VariantType.UShort)
	{
		uiVal = value;
	}

	public PropVariant(int value)
		: base(VariantType.Int32)
	{
		intValue = value;
	}

	public PropVariant(uint value)
		: base(VariantType.UInt32)
	{
		uintVal = value;
	}

	public PropVariant(float value)
		: base(VariantType.Float)
	{
		fltVal = value;
	}

	public PropVariant(double value)
		: base(VariantType.Double)
	{
		doubleValue = value;
	}

	public PropVariant(long value)
		: base(VariantType.Int64)
	{
		longValue = value;
	}

	public PropVariant(ulong value)
		: base(VariantType.UInt64)
	{
		ulongValue = value;
	}

	public PropVariant(Guid value)
		: base(VariantType.Guid)
	{
		ptr = Marshal.AllocCoTaskMem(Marshal.SizeOf(value));
		Marshal.StructureToPtr(value, ptr, fDeleteOld: false);
	}

	public PropVariant(byte[] value)
		: base(VariantType.Blob)
	{
		blobValue.cbSize = value.Length;
		blobValue.pBlobData = Marshal.AllocCoTaskMem(value.Length);
		Marshal.Copy(value, 0, blobValue.pBlobData, value.Length);
	}

	public PropVariant(object value)
		: base(VariantType.IUnknown)
	{
		ptr = Marshal.GetIUnknownForObject(value);
	}

	public PropVariant(IntPtr value)
	{
		Marshal.PtrToStructure(value, this);
	}

	public PropVariant(ConstPropVariant value)
	{
		if (value != null)
		{
			PropVariantCopy(this, value);
			return;
		}
		throw new NullReferenceException("null passed to PropVariant constructor");
	}

	~PropVariant()
	{
		Clear();
	}

	public void Copy(PropVariant pval)
	{
		if (pval == null)
		{
			throw new Exception("Null PropVariant sent to Copy");
		}
		pval.Clear();
		PropVariantCopy(pval, this);
	}

	public void Clear()
	{
		PropVariantClear(this);
	}

	public new void Dispose()
	{
		Clear();
		GC.SuppressFinalize(this);
	}
}
