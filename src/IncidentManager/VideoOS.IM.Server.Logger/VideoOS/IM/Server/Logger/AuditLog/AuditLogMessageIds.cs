using System;

namespace VideoOS.IM.Server.Logger.AuditLog;

public static class AuditLogMessageIds
{
	public static readonly Guid IncidentCreated = new Guid("5FB6AE9F-27E6-47C5-A8D6-5D58345F29E4");

	public static readonly Guid IncidentDeleted = new Guid("56094E23-5C60-494D-8F49-575416FF73AA");

	public static readonly Guid IncidentCategoriesModified = new Guid("7AFA913F-0244-47BF-92AD-ABCF13C8C79E");

	public static readonly Guid TypeCreated = new Guid("6321173F-F23E-4FA2-BFBB-777E099BA2CE");

	public static readonly Guid TypeModified = new Guid("07A778CC-1DD5-4935-B45B-4EC117C17E17");

	public static readonly Guid TypeDeleted = new Guid("F0E37255-5F9E-4299-BE90-1E0A554C01D2");

	public static readonly Guid StatusCreated = new Guid("A7636736-1CF5-4D17-BA41-82570A343405");

	public static readonly Guid StatusModified = new Guid("34ED99D0-E77A-4B93-968B-E78ABFAD4385");

	public static readonly Guid StatusDeleted = new Guid("B9CA39B2-A694-42AF-9888-8CE0A7C1A65E");

	public static readonly Guid CategoryCreated = new Guid("418404BC-EF0B-475A-8C4D-EFB43C33DFAC");

	public static readonly Guid CategoryModified = new Guid("8DFAAB9A-363C-4FE1-B7DE-8A1E221D903A");

	public static readonly Guid CategoryDeleted = new Guid("EB78676A-04D3-47F0-9572-05FC51C4ED2A");

	public static readonly Guid ReportPrinted = new Guid("157E3591-86B2-4667-925F-17E892B680CA");
}
