using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace VideoOS.Mobile.AuditLog;

public class ExportAuditLogData
{
	public ExportType Type { get; set; }

	public bool IsPasswordProtected { get; set; }

	public bool IncludeAudio { get; set; }

	public ExportAuditLogData()
	{
	}

	public ExportAuditLogData(ExportParams entry)
		: this()
	{
		Type = entry.Type;
		IsPasswordProtected = !string.IsNullOrWhiteSpace(entry.Password);
		IncludeAudio = entry.IncludeAudio;
	}
}
