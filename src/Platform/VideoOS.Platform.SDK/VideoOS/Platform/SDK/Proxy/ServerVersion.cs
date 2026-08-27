using System;

namespace VideoOS.Platform.SDK.Proxy;

public sealed class ServerVersion : ICloneable, IComparable, IComparable<ServerVersion>, IEquatable<ServerVersion>
{
	public int Major { get; set; }

	public int Minor { get; set; }

	public int Revision { get; set; }

	public ServerVersion(string version)
	{
		ServerVersion serverVersion = Parse(version);
		Major = serverVersion.Major;
		Revision = serverVersion.Revision;
		Minor = serverVersion.Minor;
	}

	public ServerVersion()
	{
		Major = 0;
		Minor = 0;
		Revision = 0;
	}

	public ServerVersion(int major, int minor)
	{
		if (major < 0)
		{
			throw new ArgumentOutOfRangeException("major", "Version not valid");
		}
		if (minor < 0)
		{
			throw new ArgumentOutOfRangeException("minor", "Version not valid");
		}
		Major = major;
		Minor = minor;
		Revision = 0;
	}

	public ServerVersion(int major, int minor, int revision)
		: this(major, minor)
	{
		if (revision < 0)
		{
			throw new ArgumentOutOfRangeException("revision", "Version not valid");
		}
		Revision = revision;
	}

	public object Clone()
	{
		return new ServerVersion
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
		ServerVersion serverVersion = obj as ServerVersion;
		if (serverVersion == null)
		{
			throw new ArgumentException("Version not valid");
		}
		if (Major != serverVersion.Major)
		{
			if (Major > serverVersion.Major)
			{
				return 1;
			}
			return -1;
		}
		if (Minor != serverVersion.Minor)
		{
			if (Minor > serverVersion.Minor)
			{
				return 1;
			}
			return -1;
		}
		if (Revision == serverVersion.Revision)
		{
			return 0;
		}
		if (Revision > serverVersion.Revision)
		{
			return 1;
		}
		return -1;
	}

	public int CompareTo(ServerVersion other)
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
		if (Revision == other.Revision)
		{
			return 0;
		}
		if (Revision > other.Revision)
		{
			return 1;
		}
		return -1;
	}

	public override bool Equals(object obj)
	{
		ServerVersion serverVersion = obj as ServerVersion;
		if (serverVersion == null)
		{
			return false;
		}
		if (Major == serverVersion.Major && Minor == serverVersion.Minor)
		{
			return Revision == serverVersion.Revision;
		}
		return false;
	}

	public bool Equals(ServerVersion other)
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
		return 0 | ((Major & 0xF) << 28) | ((Minor & 0xFF) << 20) | (Revision & 0xFFF);
	}

	public static bool operator ==(ServerVersion v1, ServerVersion v2)
	{
		return v1?.Equals(v2) ?? ((object)v2 == null);
	}

	public static bool operator !=(ServerVersion v1, ServerVersion v2)
	{
		return !(v1 == v2);
	}

	public static bool operator <(ServerVersion v1, ServerVersion v2)
	{
		if (v1 == null)
		{
			throw new ArgumentNullException("v1");
		}
		return v1.CompareTo(v2) < 0;
	}

	public static bool operator <=(ServerVersion v1, ServerVersion v2)
	{
		if (v1 == null)
		{
			throw new ArgumentNullException("v1");
		}
		return v1.CompareTo(v2) <= 0;
	}

	public static bool operator >(ServerVersion v1, ServerVersion v2)
	{
		return v2 < v1;
	}

	public static bool operator >=(ServerVersion v1, ServerVersion v2)
	{
		return v2 <= v1;
	}

	public static explicit operator Version(ServerVersion version)
	{
		return new Version(version.Major, version.Minor, version.Revision, 0);
	}

	public override string ToString()
	{
		return Major + "." + Minor + "." + Revision;
	}

	public static ServerVersion Parse(string input)
	{
		if (input == null)
		{
			throw new ArgumentNullException("input");
		}
		throw new NotImplementedException();
	}
}
