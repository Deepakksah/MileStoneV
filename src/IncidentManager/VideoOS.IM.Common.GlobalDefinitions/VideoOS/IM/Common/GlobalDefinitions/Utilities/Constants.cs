using System;

namespace VideoOS.IM.Common.GlobalDefinitions.Utilities;

public static class Constants
{
	public static class Translations
	{
		public static string TranslationPrefix => "##Translate##";
	}

	public static readonly Guid IncidentManagerPluginId = new Guid("6053b0b4-3e97-4a86-8e46-d1a9ce807ed1");

	public static readonly string IncidentManagerLicenseId = "XPIM";

	public static readonly int DaysToKeepIncidentsDefault = 7;

	public static readonly string WebApiIdentityName = "Incident Manager Web API";
}
