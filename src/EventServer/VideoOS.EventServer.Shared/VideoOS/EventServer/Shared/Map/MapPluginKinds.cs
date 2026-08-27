using System;

namespace VideoOS.EventServer.Shared.Map;

public static class MapPluginKinds
{
	private static Guid _mipPluginKind = new Guid("0AA2F247-3F48-4C45-8774-E715A0A1C053");

	private static Guid _accessControlPluginKind = new Guid("8053CF92-64A1-49DD-BB24-AA188F33BD75");

	public static Guid MipPluginKind => _mipPluginKind;

	public static Guid AccessControlPluginKind => _accessControlPluginKind;
}
