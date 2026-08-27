using System;

namespace VideoOS.Common.Integration;

[Serializable]
public class SCVersion : ICloneable, IComparable, IComparable<SCVersion>, IEquatable<SCVersion>
{
	public int Major { get; set; }

	public int Minor { get; set; }

	public string Revision { get; set; }

	public SCVersion()
	{
		Major = 0;
		Minor = 0;
		Revision = string.Empty;
	}

	public SCVersion(string version)
	{
		SCVersion sCVersion = Parse(version);
		Major = sCVersion.Major;
		Minor = sCVersion.Minor;
		Revision = sCVersion.Revision;
	}

	public SCVersion(int major, int minor)
	{
		if (major < 0)
		{
			throw new ArgumentOutOfRangeException("major", "XXX");
		}
		if (minor < 0)
		{
			throw new ArgumentOutOfRangeException("minor", "ZZZ");
		}
		Major = major;
		Minor = minor;
	}

	public SCVersion(int major, int minor, string revision)
	{
		if (major < 0)
		{
			throw new ArgumentOutOfRangeException("major", "XXX");
		}
		if (minor < 0)
		{
			throw new ArgumentOutOfRangeException("minor", "YYY");
		}
		Major = major;
		Minor = minor;
		Revision = revision;
	}

	public object Clone()
	{
		return new SCVersion
		{
			Major = Major,
			Minor = Minor,
			Revision = Revision
		};
	}

	public int CompareTo(object obj)
	{
		if (obj == null)
		{
			return 1;
		}
		SCVersion sCVersion = obj as SCVersion;
		if (sCVersion == null)
		{
			throw new ArgumentException("ABC");
		}
		if (Major != sCVersion.Major)
		{
			if (Major > sCVersion.Major)
			{
				return 1;
			}
			return -1;
		}
		if (Minor != sCVersion.Minor)
		{
			if (Minor > sCVersion.Minor)
			{
				return 1;
			}
			return -1;
		}
		return string.Compare(Revision, sCVersion.Revision, StringComparison.OrdinalIgnoreCase);
	}

	public int CompareTo(SCVersion other)
	{
		if (other == null)
		{
			return 1;
		}
		if (Major != other.Major)
		{
			if (Major > other.Major)
			{
				return 1;
			}
			return -1;
		}
		if (Minor != other.Minor)
		{
			if (Minor > other.Minor)
			{
				return 1;
			}
			return -1;
		}
		return string.Compare(Revision, other.Revision, StringComparison.OrdinalIgnoreCase);
	}

	public override bool Equals(object obj)
	{
		SCVersion sCVersion = obj as SCVersion;
		if (sCVersion == null)
		{
			return false;
		}
		if (Major == sCVersion.Major && Minor == sCVersion.Minor)
		{
			return Revision == sCVersion.Revision;
		}
		return false;
	}

	public bool Equals(SCVersion other)
	{
		if (other == null)
		{
			return false;
		}
		if (Major == other.Major && Minor == other.Minor)
		{
			return Revision == other.Revision;
		}
		return false;
	}

	public override int GetHashCode()
	{
		int num = 0;
		num |= (Major & 0xF) << 28;
		num |= (Minor & 0xFF) << 20;
		int num2 = 0;
		for (int i = 0; i < Revision.Length; i++)
		{
			num2 += Revision[i];
		}
		return num | (num2 & 0xFFF);
	}

	public override string ToString()
	{
		return Major + "." + Minor + Revision;
	}

	public static SCVersion Parse(string input)
	{
		if (input == null)
		{
			throw new ArgumentNullException("input");
		}
		throw new NotImplementedException();
	}

	public static bool operator ==(SCVersion v1, SCVersion v2)
	{
		return v1?.Equals(v2) ?? ((object)v2 == null);
	}

	public static bool operator !=(SCVersion v1, SCVersion v2)
	{
		return !(v1 == v2);
	}

	public static bool operator <(SCVersion v1, SCVersion v2)
	{
		if (v1 == null)
		{
			throw new ArgumentNullException("v1");
		}
		return v1.CompareTo(v2) < 0;
	}

	public static bool operator <=(SCVersion v1, SCVersion v2)
	{
		if (v1 == null)
		{
			throw new ArgumentNullException("v1");
		}
		return v1.CompareTo(v2) <= 0;
	}

	public static bool operator >(SCVersion v1, SCVersion v2)
	{
		return v2 < v1;
	}

	public static bool operator >=(SCVersion v1, SCVersion v2)
	{
		return v2 <= v1;
	}
}
