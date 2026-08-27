using System;
using System.Globalization;
using VideoOS.Management.VmoClient.Properties;

namespace VideoOS.Management.VmoClient;

public sealed class ServerVersion : ICloneable, IComparable, IComparable<ServerVersion>, IEquatable<ServerVersion>
{
	public int Major { get; private set; }

	public int Minor { get; private set; }

	public int Revision { get; private set; }

	public Version Version => new Version(Major, Minor, 0, Revision);

	public string DisplayVersion => GetDisplayVersion(this);

	public ServerVersion(string version)
	{
		if (string.IsNullOrEmpty(version))
		{
			throw new ArgumentException("Version is null or empty");
		}
		ServerVersion serverVersion = Parse(version);
		Major = serverVersion.Major;
		Revision = serverVersion.Revision;
		Minor = serverVersion.Minor;
	}

	public ServerVersion(Version version)
	{
		if (version == null)
		{
			throw new ArgumentNullException("version");
		}
		Major = version.Major;
		Revision = version.Revision;
		Minor = version.Minor;
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
			throw new ArgumentOutOfRangeException("major", Resources.ArgumentOutOfRangeVersion);
		}
		if (minor < 0)
		{
			throw new ArgumentOutOfRangeException("minor", Resources.ArgumentOutOfRangeVersion);
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
			throw new ArgumentOutOfRangeException("revision", Resources.ArgumentOutOfRangeVersion);
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
			throw new ArgumentException(Resources.ArgumentExceptionMustBeServerVersion);
		}
		return CompareTo(serverVersion);
	}

	public int CompareTo(ServerVersion other)
	{
		if (other == null)
		{
			return 1;
		}
		return Version.CompareTo(other.Version);
	}

	public override bool Equals(object obj)
	{
		ServerVersion serverVersion = obj as ServerVersion;
		if (serverVersion == null)
		{
			return false;
		}
		return Version.Equals(serverVersion.Version);
	}

	public bool Equals(ServerVersion other)
	{
		if (other == null)
		{
			return false;
		}
		return Version.Equals(other.Version);
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
		if (version == null)
		{
			throw new ArgumentNullException("version");
		}
		return new Version(version.Major, version.Minor, 0, version.Revision);
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
		string[] array = input.Split('.');
		if (array.Length > 4 || array.Length < 3)
		{
			throw new ArgumentOutOfRangeException("input");
		}
		if (array.Length > 3)
		{
			return new ServerVersion(int.Parse(array[0], CultureInfo.InvariantCulture), int.Parse(array[1], CultureInfo.InvariantCulture), int.Parse(array[3], CultureInfo.InvariantCulture));
		}
		return new ServerVersion(int.Parse(array[0], CultureInfo.InvariantCulture), int.Parse(array[1], CultureInfo.InvariantCulture), int.Parse(array[2], CultureInfo.InvariantCulture));
	}

	private static string GetRevisionLetter(Version version)
	{
		if (version.Revision >= 0 && version.Revision <= 25)
		{
			return Convert.ToChar(version.Revision + 96).ToString(CultureInfo.InvariantCulture);
		}
		return string.Format(CultureInfo.InvariantCulture, " (Rev {0})", version.Revision);
	}

	public static string GetDisplayVersion(ServerVersion version)
	{
		if (version == null)
		{
			throw new ArgumentNullException("version");
		}
		return GetDisplayVersion(version.Version);
	}

	public static string GetDisplayVersion(Version version)
	{
		if (version == null)
		{
			throw new ArgumentNullException("version");
		}
		if (version == new Version(0, 0, 0, 0))
		{
			return Resources.ZeroVersionString;
		}
		if (version.Major >= 12)
		{
			return string.Format(CultureInfo.InvariantCulture, Resources.DisplayVersionString, version.Major, version.Minor, GetRevisionLetter(version));
		}
		return string.Format(CultureInfo.InvariantCulture, Resources.DisplayVersionString, version.Major, version.Minor - 1, GetRevisionLetter(version));
	}
}
