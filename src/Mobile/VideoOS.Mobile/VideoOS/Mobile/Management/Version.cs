using System;
using System.Text;
using System.Text.RegularExpressions;

namespace VideoOS.Mobile.Management;

public class Version
{
	[Flags]
	private enum Fields
	{
		Major = 0,
		Minor = 1,
		Build = 2,
		Revision = 3,
		Max = 4
	}

	private readonly uint[] _versions = new uint[4];

	public static readonly Regex VersionStringFilter = new Regex("(\\d{1,3}\\.\\d{1,2})(\\.\\d{1,6}(\\.\\d{1,2})?)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);

	public uint Major
	{
		get
		{
			return _versions[0];
		}
		set
		{
			_versions[0] = value;
		}
	}

	public uint Minor
	{
		get
		{
			return _versions[1];
		}
		set
		{
			_versions[1] = value;
		}
	}

	public uint Build
	{
		get
		{
			return _versions[2];
		}
		set
		{
			_versions[2] = value;
		}
	}

	public uint Revision
	{
		get
		{
			return _versions[3];
		}
		set
		{
			_versions[3] = value;
		}
	}

	public char Separator { get; set; } = '.';

	public Version(uint major, uint minor, uint build, uint revision)
	{
		Major = major;
		Minor = minor;
		Build = build;
		Revision = revision;
	}

	public Version()
	{
		InternalInit();
	}

	private void InternalInit()
	{
		Major = 0u;
		Minor = 0u;
		Build = 0u;
		Revision = 0u;
	}

	public Version(string version)
	{
		InternalInit();
		InitFromVersionString(version);
	}

	public void InitFromVersionString(string version)
	{
		MatchCollection matchCollection = VersionStringFilter.Matches(version);
		if (matchCollection.Count > 0)
		{
			version = matchCollection[0].Value;
		}
		string[] array = version.Split(Separator);
		for (int i = 0; i < Math.Min(4, array.Length); i++)
		{
			_versions[i] = Convert.ToUInt32(array[i]);
		}
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Major).Append(Separator).Append(Minor)
			.Append(Separator)
			.Append(Build)
			.Append(Separator)
			.Append(Revision);
		return stringBuilder.ToString();
	}

	public static Version FromVersionString(string version)
	{
		return new Version(version);
	}
}
