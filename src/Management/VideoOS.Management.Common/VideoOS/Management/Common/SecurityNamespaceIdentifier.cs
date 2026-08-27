using System;

namespace VideoOS.Management.Common;

public static class SecurityNamespaceIdentifier
{
	public static readonly Guid ManagementServer = new Guid("E9914E39-A67A-496C-A3C1-5B4AAAB28F15");

	public static readonly Guid RecordingServer = new Guid("9710735D-EB10-44EA-888D-FA8AB05A6433");

	public static readonly Guid FailoverServer = new Guid("6339122C-04F5-41F2-B3E3-3555997CFCEF");

	public static readonly Guid Hardware = new Guid("F97772D8-7B6D-4EA4-B6B5-EE4EA010FA2C");

	public static readonly Guid Camera = new Guid("623D03F8-C5D5-46BC-A2F4-4C03562D4F85");

	public static readonly Guid Microphone = new Guid("15F48F88-CA89-4926-9A84-2B02864EC77A");

	public static readonly Guid Speaker = new Guid("48B602BC-E752-4BBF-8E2A-7DE01F53A6DD");

	public static readonly Guid Metadata = new Guid("EDE4D51C-F691-4894-9C0B-C3AE096DC04D");

	public static readonly Guid Input = new Guid("3ED79EF0-EE2F-40B8-A49A-17E9380C5932");

	public static readonly Guid Output = new Guid("A95FCE90-753D-4CB7-86FB-F8CAD9F93B67");

	public static readonly Guid SmartWall = new Guid("8732FCAC-A63C-46EA-9683-532B4C6644E7");

	public static readonly Guid Matrix = new Guid("988D493D-1597-4BE6-AFF0-396BFD5D047A");

	public static readonly Guid ViewGroup = new Guid("EC543C56-ADF7-4600-B9AD-97E166552BE9");

	public static readonly Guid UserDefinedEvent = new Guid("D4FF31CD-D9AC-46DF-8E92-5B545D191BAF");

	public static readonly Guid Rule = new Guid("C24C91A7-7D89-459B-B78E-C4548C74640D");

	public static readonly Guid Site = new Guid("875F4A4E-4D78-4A8D-A978-290227253475");

	public static readonly Guid SystemMonitor = new Guid("2D4048FA-41DA-435A-A800-2419137E4A3E");

	public static readonly Guid LogServer = new Guid("C201AE6F-6FF3-41D4-BCBF-0F9DB5049988");

	public static readonly Guid SaveSearch = new Guid("D2D396E4-819E-4226-AA7F-AF36896D78BC");

	public static readonly Guid MetadataSearch = new Guid("55BF1320-0224-48C7-8F57-BD41090576C8");

	public static readonly Guid Alarms = new Guid("46BB0392-FD06-465B-8776-B90AC16639B3");

	public static readonly Guid AccessControlUnits = new Guid("6d3bdb33-d913-470f-8784-3dd4ee94db8c");
}
