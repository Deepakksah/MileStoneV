using System;
using VideoOS.Mobile.Portable.Common;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class ExportParamsHelper
{
	public enum ParamMask : uint
	{
		Nothing = 0u,
		[Description("CameraId")]
		CameraId = 1u,
		[Description("StartTime")]
		StartTime = 2u,
		[Description("EndTime")]
		EndTime = 4u,
		[Description("Type")]
		ExportType = 8u,
		[Description("ExportId")]
		ExportId = 16u,
		[Description("CodecName")]
		CodecName = 32u,
		[Description("Name")]
		ExportName = 64u,
		[Description("KeepOriginals")]
		KeepOriginals = 128u,
		[Description("IncludeAudio")]
		IncludeAudio = 144u,
		[Description("InvestigationId")]
		InvestigationId = 256u,
		[Description("ReExportFromId")]
		ReExportFromId = 512u,
		[Description("SubExportIds")]
		SubExportIds = 1024u,
		[Description("Password")]
		Password = 2048u,
		StartMandatory = 11u,
		StartMandatoryPicture = 11u,
		StartMandatoryVideo = 15u,
		ReStartMandatory = 16u,
		StartPossible = 3071u,
		StartFileExportMandatory = 88u,
		GetDeleteMandatory = 16u,
		DeletePossible = 16u,
		CreateDownloadLinkMandatory = 280u,
		StartInvestigationExportMandatory = 264u,
		StartInvestigationExportPossible = 2456u,
		DeleteInvestigationExportMandatory = 264u,
		All = 3071u
	}

	public static readonly Guid AllExports = Guid.Empty;

	public static readonly Guid AllExportsAllUsers = new Guid("{A3B9C5FB-FAAD-42C8-AB73-B79D6FFFDBC1}");
}
