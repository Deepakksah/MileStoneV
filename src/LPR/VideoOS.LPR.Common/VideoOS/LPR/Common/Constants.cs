using System;
using System.Collections.Generic;

namespace VideoOS.LPR.Common;

public static class Constants
{
	public const string UnknownLicencePlateStyle = "Unknown";

	public const string GenericSearchTargetGlobal = "ObjectValue";

	public const string GenericSearchTargetSource = "SourceName";

	public const string GenericSearchTargetSourceId = "ObjectId";

	public const string GenericSearchTargetTime = "Timestamp";

	public const string GenericSearchTargetType = "Type";

	public const string GenericSearchTargetCountry = "CustomTag";

	public const string GenericSearchTargetList = "Message";

	public static Dictionary<string, string> CountryCodeMappingReverse;

	public static Dictionary<string, string> CountryCodeMapping;

	public static Guid LprPluginDefId => new Guid("6f714690-e09f-4d6a-a0ca-9f913ca125e8");

	public static Guid LprVideoSourceKind => new Guid("B4722667-E468-41D3-AD1E-7470FA8F7F79");

	public static Guid LprServerKind => new Guid("6b1845a7-851f-4f0b-9644-940463a5c2e7");

	public static Guid LprTrayControllerId => new Guid("DE77E5C2-5719-45A0-BAD1-CC0FFEF5F54E");

	public static Guid LprApplicationId => new Guid("4020d93c-ac37-419e-867c-36a86afb6e21");

	public static Guid LprIntegrationId => new Guid("2E3257A2-7528-4559-8C87-390E27BC87BD");

	public static Guid LPRPluginServerId => new Guid("6f714690-e09f-4d6a-a0ca-9f913ca125e8");

	public static Guid LPRPluginClientId => new Guid("DD258552-403D-47CC-A486-D3ECCD93AF9E");

	public static Guid PlateListKind => new Guid("FE3450D5-4FCA-4EA4-9424-A03BC1B988F2");

	public static Guid PlateStyleAliasesKind => new Guid("13814896-5978-455f-9b56-11456a0e5c34");

	public static Guid UnregisteredPlateId => new Guid("322B1E5F-7EE4-423E-8DF4-10E27BFD3036");

	public static string UnregisteredPlateName => "Unlisted license plate";

	public static Guid LPRViewItemId => new Guid("3561BA4F-AE52-4DE6-801D-D0134306AE03");

	public static Guid LPRViewItemEventsViewItemPluginId => new Guid("5E40446F-4400-47E4-956E-41CC71FB6E34");

	public static Guid LPRWorkspaceId => new Guid("427F26E6-44F8-4670-AE19-B5BA87562038");

	public static Guid LPRWorkspaceViewItemId => new Guid("132965D8-98A3-4B6C-89A3-2C6F7162DC0E");

	public static Guid LPRCommunicationBackgroundPluginId => new Guid("2FFA32F3-31B4-4DE8-8752-11690E82AB60");

	public static Guid LPRServerServiceType => new Guid("BA177CB7-5A9B-42B8-988D-CFBA91CA44E3");

	public static Guid EventServerServiceType => new Guid("08AB8F23-9AEF-4298-9CAA-F39259FE7CDA");

	static Constants()
	{
		CountryCodeMappingReverse = new Dictionary<string, string>();
		CountryCodeMapping = new Dictionary<string, string>
		{
			{ "A", "A" },
			{ "AL", "AL" },
			{ "AM", "AM" },
			{ "AND", "AND" },
			{ "ANG", "ANG" },
			{ "AUS", "AUS" },
			{ "AUS_ACT", "ACT-AUS" },
			{ "AUS_FI", "FI-AUS" },
			{ "AUS_NSW", "NSW-AUS" },
			{ "AUS_NT", "NT-AUS" },
			{ "AUS_QLD", "QLD-AUS" },
			{ "AUS_SA", "SA-AUS" },
			{ "AUS_TAS", "TAS-AUS" },
			{ "AUS_VIC", "VIC-AUS" },
			{ "AUS_WA", "WA-AUS" },
			{ "AW", "AW" },
			{ "AZ", "AZ" },
			{ "B", "B" },
			{ "BD", "BD" },
			{ "BF", "BF" },
			{ "BG", "BG" },
			{ "BIH", "BIH" },
			{ "BM", "BM" },
			{ "BOL", "BOL" },
			{ "BQ", "BQ" },
			{ "BR", "BR" },
			{ "BRN", "BRN" },
			{ "BRU", "BRU" },
			{ "BS", "BS" },
			{ "BU", "RU" },
			{ "BUR", "MYA" },
			{ "BW", "BW" },
			{ "BY", "BY" },
			{ "BZ", "BZ" },
			{ "CDN", "CDN" },
			{ "CDN_AB", "AB-CDN" },
			{ "CDN_BC", "BC-CDN" },
			{ "CDN_MB", "MB-CDN" },
			{ "CDN_NB", "NB-CDN" },
			{ "CDN_NL", "NL-CDN" },
			{ "CDN_NS", "NS-CDN" },
			{ "CDN_NT", "NT-CDN" },
			{ "CDN_NU", "NU-CDN" },
			{ "CDN_ON", "ON-CDN" },
			{ "CDN_PE", "PE-CDN" },
			{ "CDN_QC", "QC-CDN" },
			{ "CDN_SK", "SK-CDN" },
			{ "CDN_YT", "YT-CDN" },
			{ "CGO", "ZRE" },
			{ "CH", "CH" },
			{ "CI", "CI" },
			{ "CL", "CL" },
			{ "CMR", "CMR" },
			{ "CN", "CN" },
			{ "CO", "CO" },
			{ "CR", "CR" },
			{ "CW", "CW" },
			{ "CZ", "CZ" },
			{ "D", "D" },
			{ "DJI", "DJI" },
			{ "DK", "DK" },
			{ "DOM", "DOM" },
			{ "DZ", "DZ" },
			{ "E", "E" },
			{ "EAK", "EAK" },
			{ "EAT", "EAT" },
			{ "EAU", "EAU" },
			{ "EC", "EC" },
			{ "ES", "ES" },
			{ "EST", "EST" },
			{ "ET", "ET" },
			{ "EU", "EU" },
			{ "F", "F" },
			{ "FIN", "FIN" },
			{ "FL", "FL" },
			{ "FO", "FO" },
			{ "GB", "GB" },
			{ "GCA", "GCA" },
			{ "GE", "GE" },
			{ "GH", "GH" },
			{ "GR", "GR" },
			{ "GUY", "GUY" },
			{ "H", "H" },
			{ "HK", "HK" },
			{ "HKJ", "HKJ" },
			{ "HON", "HON" },
			{ "HR", "HR" },
			{ "I", "I" },
			{ "IL", "IL" },
			{ "IND", "IND" },
			{ "IRQ", "IRQ" },
			{ "IR", "IR" },
			{ "IRL", "IRL" },
			{ "IS", "IS" },
			{ "JA", "JA" },
			{ "KS", "KS" },
			{ "KSA", "KSA" },
			{ "KWT", "KWT" },
			{ "KZ", "KZ" },
			{ "L", "L" },
			{ "LAR", "LAR" },
			{ "LS", "LS" },
			{ "LT", "LT" },
			{ "LV", "LV" },
			{ "M", "M" },
			{ "MA", "MA" },
			{ "MAL", "MAL" },
			{ "MC", "MC" },
			{ "MD", "MD" },
			{ "MEX_NL", "MEX" },
			{ "MGL", "MGL" },
			{ "MK", "MK" },
			{ "MNE", "MNE" },
			{ "MOC", "MOC" },
			{ "MS", "MS" },
			{ "N", "N" },
			{ "NAM", "NAM" },
			{ "NIC", "NIC" },
			{ "NL", "NL" },
			{ "NZ", "NZ" },
			{ "OM", "OM" },
			{ "P", "P" },
			{ "PA", "PA" },
			{ "PE", "PE" },
			{ "PK", "PK" },
			{ "PL", "PL" },
			{ "PNG", "PNG" },
			{ "PR", "PR" },
			{ "PS", "PS" },
			{ "PY", "PY" },
			{ "Q", "Q" },
			{ "RA", "RA" },
			{ "RB", "DY" },
			{ "RC", "RC" },
			{ "RCB", "RCB" },
			{ "RCH", "RCH" },
			{ "RI", "RI" },
			{ "RKS", "RKS" },
			{ "RMM", "RMM" },
			{ "RO", "RO" },
			{ "ROK", "ROK" },
			{ "RP", "RP" },
			{ "ROU", "UY" },
			{ "RUS", "RUS" },
			{ "RWA", "RWA" },
			{ "S", "S" },
			{ "SA", "SA" },
			{ "SD", "SD" },
			{ "SGP", "SGP" },
			{ "SK", "SK" },
			{ "SLO", "SLO" },
			{ "SME", "SME" },
			{ "SN", "SN" },
			{ "SRB", "SRB" },
			{ "T", "T" },
			{ "TG", "TG" },
			{ "TJ", "TJ" },
			{ "TM", "TM" },
			{ "TN", "TN" },
			{ "TR", "TR" },
			{ "TT", "TT" },
			{ "UA", "UA" },
			{ "UAE_AJ", "AJ-UAE" },
			{ "UAE_AZ", "AD-UAE" },
			{ "UAE_DU", "DU-UAE" },
			{ "UAE_FU", "FU-UAE" },
			{ "UAE_RK", "RK-UAE" },
			{ "UAE_SH", "SH-UAE" },
			{ "UAE_UQ", "UQ-UAE" },
			{ "USA", "USA" },
			{ "USA_AK", "AK-USA" },
			{ "USA_AL", "AL-USA" },
			{ "USA_ME", "ME-USA" },
			{ "USA_AR", "AR-USA" },
			{ "USA_AZ", "AZ-USA" },
			{ "USA_CA", "CA-USA" },
			{ "USA_CO", "CO-USA" },
			{ "USA_CT", "CT-USA" },
			{ "USA_DC", "DC-USA" },
			{ "USA_DE", "DE-USA" },
			{ "USA_FL", "FL-USA" },
			{ "USA_GA", "GA-USA" },
			{ "USA_GS", "GS-USA" },
			{ "USA_HI", "HI-USA" },
			{ "USA_IA", "IA-USA" },
			{ "USA_ID", "ID-USA" },
			{ "USA_IL", "IL-USA" },
			{ "USA_IN", "IN-USA" },
			{ "USA_KS", "KS-USA" },
			{ "USA_KY", "KY-USA" },
			{ "USA_LA", "LA-USA" },
			{ "USA_MA", "MA-USA" },
			{ "USA_MD", "MD-USA" },
			{ "USA_MI", "MI-USA" },
			{ "USA_MN", "MN-USA" },
			{ "USA_MO", "MO-USA" },
			{ "USA_MS", "MS-USA" },
			{ "USA_MT", "MT-USA" },
			{ "USA_NC", "NC-USA" },
			{ "USA_ND", "ND-USA" },
			{ "USA_NE", "NE-USA" },
			{ "USA_NH", "NH-USA" },
			{ "USA_NJ", "NJ-USA" },
			{ "USA_NM", "NM-USA" },
			{ "USA_NV", "NV-USA" },
			{ "USA_NY", "NY-USA" },
			{ "USA_OH", "OH-USA" },
			{ "USA_OK", "OK-USA" },
			{ "USA_OR", "OR-USA" },
			{ "USA_PA", "PA-USA" },
			{ "USA_RI", "RI-USA" },
			{ "USA_SC", "SC-USA" },
			{ "USA_SD", "SD-USA" },
			{ "USA_TN", "TN-USA" },
			{ "USA_TX", "TX-USA" },
			{ "USA_UT", "UT-USA" },
			{ "USA_VA", "VA-USA" },
			{ "USA_VT", "VT-USA" },
			{ "USA_WA", "WA-USA" },
			{ "USA_WI", "WI-USA" },
			{ "USA_WV", "WV-USA" },
			{ "USA_WY", "WY-USA" },
			{ "UZ", "UZ" },
			{ "VN", "VN" },
			{ "X_BIC", "BIC-X" },
			{ "X_DG", "DG-X" },
			{ "X_NR", "NR-X" },
			{ "X_USA", "US-USA" },
			{ "X_EE", "EU-EAST" },
			{ "X_EU", "EU-X" },
			{ "X_XX", "Any" },
			{ "YV", "YV" },
			{ "ZA", "ZA" },
			{ "ZA_EC", "EC-ZA" },
			{ "ZA_FS", "FS-ZA" },
			{ "ZA_GP", "GP-ZA" },
			{ "ZA_LP", "LP-ZA" },
			{ "ZA_MP", "MP-ZA" },
			{ "ZA_NC", "NC-ZA" },
			{ "ZA_NW", "NW-ZA" },
			{ "ZA_WC", "WC-ZA" },
			{ "ZA_ZN", "ZN-ZA" },
			{ "ZW", "ZW" }
		};
		foreach (KeyValuePair<string, string> item in CountryCodeMapping)
		{
			CountryCodeMappingReverse.Add(item.Value, item.Key);
		}
	}
}
