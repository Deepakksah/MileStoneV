using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace VideoOS.Mobile.VideoChannel.MediaFoundation;

[StructLayout(LayoutKind.Explicit)]
public class ConstPropVariant : IDisposable
{
	public enum VariantType : short
	{
		None = 0,
		Short = 2,
		Int32 = 3,
		Float = 4,
		Double = 5,
		IUnknown = 13,
		UByte = 17,
		UShort = 18,
		UInt32 = 19,
		Int64 = 20,
		UInt64 = 21,
		String = 31,
		Guid = 72,
		Blob = 4113,
		StringArray = 4127
	}

	protected struct Blob
	{
		public int cbSize;

		public IntPtr pBlobData;
	}

	protected struct CALPWstr
	{
		public int cElems;

		public IntPtr pElems;
	}

	[FieldOffset(0)]
	protected VariantType type;

	[FieldOffset(2)]
	protected short reserved1;

	[FieldOffset(4)]
	protected short reserved2;

	[FieldOffset(6)]
	protected short reserved3;

	[FieldOffset(8)]
	protected short iVal;

	[FieldOffset(8)]
	protected ushort uiVal;

	[FieldOffset(8)]
	protected byte bVal;

	[FieldOffset(8)]
	protected int intValue;

	[FieldOffset(8)]
	protected uint uintVal;

	[FieldOffset(8)]
	protected float fltVal;

	[FieldOffset(8)]
	protected long longValue;

	[FieldOffset(8)]
	protected ulong ulongValue;

	[FieldOffset(8)]
	protected double doubleValue;

	[FieldOffset(8)]
	protected Blob blobValue;

	[FieldOffset(8)]
	protected IntPtr ptr;

	[FieldOffset(8)]
	protected CALPWstr calpwstrVal;

	public ConstPropVariant()
	{
		type = VariantType.None;
	}

	protected ConstPropVariant(VariantType v)
	{
		type = v;
	}

	public static explicit operator string(ConstPropVariant f)
	{
		return f.GetString();
	}

	public static explicit operator string[](ConstPropVariant f)
	{
		return f.GetStringArray();
	}

	public static explicit operator byte(ConstPropVariant f)
	{
		return f.GetUByte();
	}

	public static explicit operator short(ConstPropVariant f)
	{
		return f.GetShort();
	}

	public static explicit operator ushort(ConstPropVariant f)
	{
		return f.GetUShort();
	}

	public static explicit operator int(ConstPropVariant f)
	{
		return f.GetInt();
	}

	public static explicit operator uint(ConstPropVariant f)
	{
		return f.GetUInt();
	}

	public static explicit operator float(ConstPropVariant f)
	{
		return f.GetFloat();
	}

	public static explicit operator double(ConstPropVariant f)
	{
		return f.GetDouble();
	}

	public static explicit operator long(ConstPropVariant f)
	{
		return f.GetLong();
	}

	public static explicit operator ulong(ConstPropVariant f)
	{
		return f.GetULong();
	}

	public static explicit operator Guid(ConstPropVariant f)
	{
		return f.GetGuid();
	}

	public static explicit operator byte[](ConstPropVariant f)
	{
		return f.GetBlob();
	}

	public MFAttributeType GetMFAttributeType()
	{
		switch (type)
		{
		case VariantType.None:
		case VariantType.Double:
		case VariantType.IUnknown:
		case VariantType.UInt32:
		case VariantType.UInt64:
		case VariantType.String:
		case VariantType.Guid:
		case VariantType.Blob:
			return (MFAttributeType)type;
		default:
			throw new Exception("Type is not a MFAttributeType");
		}
	}

	public VariantType GetVariantType()
	{
		return type;
	}

	public string[] GetStringArray()
	{
		if (type == VariantType.StringArray)
		{
			int cElems = calpwstrVal.cElems;
			string[] array = new string[cElems];
			for (int i = 0; i < cElems; i++)
			{
				array[i] = Marshal.PtrToStringUni(Marshal.ReadIntPtr(calpwstrVal.pElems, i * IntPtr.Size));
			}
			return array;
		}
		throw new ArgumentException("PropVariant contents not a string array");
	}

	public string GetString()
	{
		if (type == VariantType.String)
		{
			return Marshal.PtrToStringUni(ptr);
		}
		throw new ArgumentException("PropVariant contents not a string");
	}

	public byte GetUByte()
	{
		if (type == VariantType.UByte)
		{
			return bVal;
		}
		throw new ArgumentException("PropVariant contents not a byte");
	}

	public short GetShort()
	{
		if (type == VariantType.Short)
		{
			return iVal;
		}
		throw new ArgumentException("PropVariant contents not an Short");
	}

	public ushort GetUShort()
	{
		if (type == VariantType.UShort)
		{
			return uiVal;
		}
		throw new ArgumentException("PropVariant contents not an UShort");
	}

	public int GetInt()
	{
		if (type == VariantType.Int32)
		{
			return intValue;
		}
		throw new ArgumentException("PropVariant contents not an int32");
	}

	public uint GetUInt()
	{
		if (type == VariantType.UInt32)
		{
			return uintVal;
		}
		throw new ArgumentException("PropVariant contents not an uint32");
	}

	public long GetLong()
	{
		if (type == VariantType.Int64)
		{
			return longValue;
		}
		throw new ArgumentException("PropVariant contents not an int64");
	}

	public ulong GetULong()
	{
		if (type == VariantType.UInt64)
		{
			return ulongValue;
		}
		throw new ArgumentException("PropVariant contents not an uint64");
	}

	public float GetFloat()
	{
		if (type == VariantType.Float)
		{
			return fltVal;
		}
		throw new ArgumentException("PropVariant contents not a Float");
	}

	public double GetDouble()
	{
		if (type == VariantType.Double)
		{
			return doubleValue;
		}
		throw new ArgumentException("PropVariant contents not a double");
	}

	public Guid GetGuid()
	{
		if (type == VariantType.Guid)
		{
			return (Guid)Marshal.PtrToStructure(ptr, typeof(Guid));
		}
		throw new ArgumentException("PropVariant contents not a Guid");
	}

	public byte[] GetBlob()
	{
		if (type == VariantType.Blob)
		{
			byte[] array = new byte[blobValue.cbSize];
			Marshal.Copy(blobValue.pBlobData, array, 0, blobValue.cbSize);
			return array;
		}
		throw new ArgumentException("PropVariant contents are not a Blob");
	}

	public object GetIUnknown()
	{
		if (type == VariantType.IUnknown)
		{
			return Marshal.GetObjectForIUnknown(ptr);
		}
		throw new ArgumentException("PropVariant contents not an IUnknown");
	}

	public override string ToString()
	{
		switch (type)
		{
		case VariantType.None:
			return "<Empty>";
		case VariantType.Blob:
		{
			byte[] blob = GetBlob();
			int num = Math.Min(16, blob.Length);
			if (num == 0)
			{
				return "<Empty Array>";
			}
			StringBuilder stringBuilder2 = new StringBuilder(blob[0].ToString("x2"));
			for (int j = 1; j < num; j++)
			{
				stringBuilder2.AppendFormat(",{0}", blob[j].ToString("x2"));
			}
			if (blob.Length > num)
			{
				stringBuilder2.Append("...");
			}
			return stringBuilder2.ToString();
		}
		case VariantType.Float:
			return GetFloat().ToString(CultureInfo.InvariantCulture);
		case VariantType.Double:
			return GetDouble().ToString(CultureInfo.InvariantCulture);
		case VariantType.Guid:
			return GetGuid().ToString();
		case VariantType.IUnknown:
			return GetIUnknown().ToString();
		case VariantType.String:
			return GetString();
		case VariantType.Short:
			return GetShort().ToString(CultureInfo.InvariantCulture);
		case VariantType.UByte:
			return GetUByte().ToString(CultureInfo.InvariantCulture);
		case VariantType.UShort:
			return GetUShort().ToString(CultureInfo.InvariantCulture);
		case VariantType.Int32:
			return GetInt().ToString(CultureInfo.InvariantCulture);
		case VariantType.UInt32:
			return GetUInt().ToString(CultureInfo.InvariantCulture);
		case VariantType.Int64:
			return GetLong().ToString(CultureInfo.InvariantCulture);
		case VariantType.UInt64:
			return GetULong().ToString(CultureInfo.InvariantCulture);
		case VariantType.StringArray:
		{
			StringBuilder stringBuilder = new StringBuilder();
			string[] stringArray = GetStringArray();
			foreach (string text in stringArray)
			{
				stringBuilder.Append(((stringBuilder.Length == 0) ? "\"" : ",\"") + text + "\"");
			}
			return stringBuilder.ToString();
		}
		default:
			return base.ToString();
		}
	}

	public override int GetHashCode()
	{
		return type switch
		{
			VariantType.None => base.GetHashCode(), 
			VariantType.Blob => GetBlob().GetHashCode(), 
			VariantType.Float => GetFloat().GetHashCode(), 
			VariantType.Double => GetDouble().GetHashCode(), 
			VariantType.Guid => GetGuid().GetHashCode(), 
			VariantType.IUnknown => GetIUnknown().GetHashCode(), 
			VariantType.String => GetString().GetHashCode(), 
			VariantType.UByte => GetUByte().GetHashCode(), 
			VariantType.Short => GetShort().GetHashCode(), 
			VariantType.UShort => GetUShort().GetHashCode(), 
			VariantType.Int32 => GetInt().GetHashCode(), 
			VariantType.UInt32 => GetUInt().GetHashCode(), 
			VariantType.Int64 => GetLong().GetHashCode(), 
			VariantType.UInt64 => GetULong().GetHashCode(), 
			VariantType.StringArray => GetStringArray().GetHashCode(), 
			_ => base.GetHashCode(), 
		};
	}

	public override bool Equals(object obj)
	{
		bool result;
		if (!(obj is PropVariant propVariant) || propVariant.type != type)
		{
			result = false;
		}
		else
		{
			switch (type)
			{
			case VariantType.None:
				result = true;
				break;
			case VariantType.Blob:
			{
				byte[] blob = GetBlob();
				byte[] blob2 = propVariant.GetBlob();
				if (blob.Length == blob2.Length)
				{
					result = true;
					for (int j = 0; j < blob.Length; j++)
					{
						if (blob[j] != blob2[j])
						{
							result = false;
							break;
						}
					}
				}
				else
				{
					result = false;
				}
				break;
			}
			case VariantType.Float:
				result = (double)Math.Abs(GetFloat() - propVariant.GetFloat()) < 0.0001;
				break;
			case VariantType.Double:
				result = Math.Abs(GetDouble() - propVariant.GetDouble()) < 0.0001;
				break;
			case VariantType.Guid:
				result = GetGuid() == propVariant.GetGuid();
				break;
			case VariantType.IUnknown:
				result = GetIUnknown() == propVariant.GetIUnknown();
				break;
			case VariantType.String:
				result = GetString() == propVariant.GetString();
				break;
			case VariantType.UByte:
				result = GetUByte() == propVariant.GetUByte();
				break;
			case VariantType.Short:
				result = GetShort() == propVariant.GetShort();
				break;
			case VariantType.UShort:
				result = GetUShort() == propVariant.GetUShort();
				break;
			case VariantType.Int32:
				result = GetInt() == propVariant.GetInt();
				break;
			case VariantType.UInt32:
				result = GetUInt() == propVariant.GetUInt();
				break;
			case VariantType.Int64:
				result = GetLong() == propVariant.GetLong();
				break;
			case VariantType.UInt64:
				result = GetULong() == propVariant.GetULong();
				break;
			case VariantType.StringArray:
			{
				string[] stringArray = GetStringArray();
				string[] stringArray2 = propVariant.GetStringArray();
				if (stringArray.Length == stringArray2.Length)
				{
					result = true;
					for (int i = 0; i < stringArray.Length; i++)
					{
						if (stringArray[i] != stringArray2[i])
						{
							result = false;
							break;
						}
					}
				}
				else
				{
					result = false;
				}
				break;
			}
			default:
				result = base.Equals(obj);
				break;
			}
		}
		return result;
	}

	public static bool operator ==(ConstPropVariant pv1, ConstPropVariant pv2)
	{
		if ((object)pv1 == pv2)
		{
			return true;
		}
		if ((object)pv1 == null || (object)pv2 == null)
		{
			return false;
		}
		return pv1.Equals(pv2);
	}

	public static bool operator !=(ConstPropVariant pv1, ConstPropVariant pv2)
	{
		return !(pv1 == pv2);
	}

	public void Dispose()
	{
		type = VariantType.None;
	}
}
