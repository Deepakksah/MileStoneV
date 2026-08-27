using System;
using System.Collections.Generic;

namespace VideoOS.Common.Integration.Configuration;

public static class SettingsXmlTransformer
{
	private const string EncryptionMarker = "enc-0eb9d4f72ef7-";

	private static readonly HashSet<string> EncryptedSettingIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "1F7026A6-BF2D-4D08-9A4F-4D9CAC61D004" };

	public static ISettingsXmlTransformer Instance { get; } = new SettingsXmlTransformerImplementation(EncryptedSettingIds, "enc-0eb9d4f72ef7-");
}
