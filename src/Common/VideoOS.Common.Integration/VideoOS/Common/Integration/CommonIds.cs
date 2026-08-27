using System;

namespace VideoOS.Common.Integration;

public static class CommonIds
{
	public static class ApplicationIds
	{
		public const string Administration = "Administration";

		public const string SoapCommandService = "SoapCommandService";

		public const string HttpRequestHandler = "HttpRequestHandler";
	}

	public static class ViewGroupClientTypeIds
	{
		public static readonly Guid SmartClient = new Guid("C22C59A3-A24B-45cb-BE65-1A9DD43E6A41");

		public static readonly Guid RemoteClient = new Guid("380D552D-9C4A-4ce5-9FF8-5F25EA4CE198");
	}

	public static readonly Guid PrivateViewGroupId = new Guid("4bf8a8b0-54ab-4e82-994f-98d8e9584176");

	public static readonly Guid PrivateViewGroupName = new Guid("F898DAC0-F1A3-4cde-8103-54A9F73DAFDD");

	public static readonly string GisPointEmpty = "POINT EMPTY";
}
