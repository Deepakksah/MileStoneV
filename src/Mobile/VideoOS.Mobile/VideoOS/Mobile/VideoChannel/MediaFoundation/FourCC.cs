using System;
using System.Runtime.InteropServices;
using System.Text;

namespace VideoOS.Mobile.VideoChannel.MediaFoundation;

[StructLayout(LayoutKind.Sequential)]
public class FourCC
{
	private int fourCC;

	public FourCC(string fcc)
	{
		if (fcc.Length != 4)
		{
			throw new ArgumentException(fcc + " is not a valid FourCC");
		}
		byte[] bytes = System.Text.Encoding.ASCII.GetBytes(fcc);
		LoadFromBytes(bytes[0], bytes[1], bytes[2], bytes[3]);
	}

	public FourCC(char a, char b, char c, char d)
	{
		LoadFromBytes((byte)a, (byte)b, (byte)c, (byte)d);
	}

	public FourCC(int fcc)
	{
		fourCC = fcc;
	}

	public FourCC(byte a, byte b, byte c, byte d)
	{
		LoadFromBytes(a, b, c, d);
	}

	public FourCC(Guid g)
	{
		if (!IsA4ccSubtype(g))
		{
			throw new Exception("Not a FourCC Guid");
		}
		byte[] array = g.ToByteArray();
		LoadFromBytes(array[0], array[1], array[2], array[3]);
	}

	public void LoadFromBytes(byte a, byte b, byte c, byte d)
	{
		fourCC = a | (b << 8) | (c << 16) | (d << 24);
	}

	public int ToInt32()
	{
		return fourCC;
	}

	public static explicit operator int(FourCC f)
	{
		return f.ToInt32();
	}

	public static explicit operator Guid(FourCC f)
	{
		return f.ToMediaSubtype();
	}

	public Guid ToMediaSubtype()
	{
		return new Guid(fourCC, 0, 16, 128, 0, 0, 170, 0, 56, 155, 113);
	}

	public static bool operator ==(FourCC fcc1, FourCC fcc2)
	{
		if ((object)fcc1 == fcc2)
		{
			return true;
		}
		if ((object)fcc1 == null || (object)fcc2 == null)
		{
			return false;
		}
		return fcc1.fourCC == fcc2.fourCC;
	}

	public static bool operator !=(FourCC fcc1, FourCC fcc2)
	{
		return !(fcc1 == fcc2);
	}

	public override bool Equals(object obj)
	{
		if (!(obj is FourCC))
		{
			return false;
		}
		return (obj as FourCC).fourCC == fourCC;
	}

	public override int GetHashCode()
	{
		return fourCC.GetHashCode();
	}

	public override string ToString()
	{
		char[] array = new char[4];
		char c = Convert.ToChar(fourCC & 0xFF);
		if (!char.IsLetterOrDigit(c))
		{
			c = ' ';
		}
		array[0] = c;
		c = Convert.ToChar((fourCC >> 8) & 0xFF);
		if (!char.IsLetterOrDigit(c))
		{
			c = ' ';
		}
		array[1] = c;
		c = Convert.ToChar((fourCC >> 16) & 0xFF);
		if (!char.IsLetterOrDigit(c))
		{
			c = ' ';
		}
		array[2] = c;
		c = Convert.ToChar((fourCC >> 24) & 0xFF);
		if (!char.IsLetterOrDigit(c))
		{
			c = ' ';
		}
		array[3] = c;
		return new string(array);
	}

	public static bool IsA4ccSubtype(Guid g)
	{
		return g.ToString().EndsWith("-0000-0010-8000-00aa00389b71");
	}
}
