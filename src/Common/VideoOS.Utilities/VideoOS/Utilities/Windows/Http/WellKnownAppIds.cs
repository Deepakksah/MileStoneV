using System;

namespace VideoOS.Utilities.Windows.Http;

public static class WellKnownAppIds
{
	public static readonly Guid ServerApiService = new Guid("9172FFF6-FA78-4E6B-8C3D-2FDFE21E0606");

	public static readonly Guid RecorderApiService = new Guid("327A57CD-4A77-4022-BA4F-E6BF5AF3F751");

	public static readonly Guid FailoverApiService = new Guid("AFE7D87B-B658-4330-8126-AAB1AD4D5C3C");

	public static readonly Guid DataCollectorApiService = new Guid("DF73501D-E6E7-4999-BB1C-B2351199109A");

	public static readonly Guid MobileServerApiService = new Guid("9ECD84D0-8193-40FD-ADA9-84E59EA91AFF");
}
