using System.Diagnostics;
using System.Reflection;

namespace VideoOS.Management.VmoClient;

public static class ServerVersions
{
	public static readonly ServerVersion v21_1 = new ServerVersion(21, 1, 1);

	public static readonly ServerVersion v21_2 = new ServerVersion(21, 2, 1);

	public static readonly ServerVersion v22_1 = new ServerVersion(22, 1, 1);

	public static readonly ServerVersion v22_2 = new ServerVersion(22, 2, 1);

	public static readonly ServerVersion v22_3 = new ServerVersion(22, 3, 1);

	public static readonly ServerVersion v23_1 = new ServerVersion(23, 1, 1);

	public static readonly ServerVersion v23_2 = new ServerVersion(23, 2, 1);

	public static readonly ServerVersion v23_3 = new ServerVersion(23, 3, 1);

	public static readonly ServerVersion v24_1 = new ServerVersion(24, 1, 1);

	public static readonly ServerVersion v24_2 = new ServerVersion(24, 2, 1);

	public static readonly ServerVersion v25_1 = new ServerVersion(25, 1, 1);

	public static readonly ServerVersion v25_2 = new ServerVersion(25, 2, 1);

	public static readonly ServerVersion v25_3 = new ServerVersion(25, 3, 1);

	public static ServerVersion vLatest
	{
		get
		{
			FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
			return new ServerVersion(versionInfo.ProductMajorPart, versionInfo.ProductMinorPart, versionInfo.ProductPrivatePart);
		}
	}
}
