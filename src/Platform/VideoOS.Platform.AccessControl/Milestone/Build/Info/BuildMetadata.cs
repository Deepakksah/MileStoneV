using System;

namespace Milestone.Build.Info;

public static class BuildMetadata
{
	public const int VersionMajor = 25;

	public const int VersionMinor = 3;

	public const int VersionRevision = 1;

	public const int VersionBuildNumber = 17737;

	public const string VersionMajorString = "25";

	public const string VersionMinorString = "3";

	public const string VersionRevisionString = "1";

	public const string VersionBuildNumberString = "17737";

	public static readonly Version Version = new Version(25, 3, 17737, 1);

	public const string CodeRevision = "";

	public const string CodeBranch = "";

	public const string AssemblyVersion = "25.3.0.0";

	public const string AssemblyInformationalVersion = "25.3.17737.1";

	public const string AssemblyFileVersion = "25.3.17737.1";

	public const string AssemblyCopyright = "Copyright © Milestone Systems A/S. All rights reserved";
}
