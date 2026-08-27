using System;
using VideoOS.Mobile.Portable.Common;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class InvestigationParamsHelper
{
	public enum ParamMask : uint
	{
		Nothing = 0u,
		[Description("ItemId")]
		InvestigationId = 1u,
		[Description("StartTime")]
		StartTime = 2u,
		[Description("EndTime")]
		EndTime = 4u,
		[Description("Name")]
		Name = 8u,
		[Description("State")]
		State = 16u,
		[Description("Items")]
		Cameras = 32u,
		[Description("IncludePackages")]
		IncludePackages = 64u,
		CreateMandatory = 14u,
		CreatePossible = 62u,
		UpdateMandatory = 15u,
		UpdatePossible = 63u,
		UpdateDataMandatory = 1u,
		UpdateDataPossible = 25u,
		CancelMandatory = 1u,
		CancelPossible = 1u,
		GetDeleteMandatory = 1u,
		GetDeletePossible = 65u,
		All = 127u
	}

	public static readonly Guid AllInvestigations = ExportParamsHelper.AllExports;

	public static readonly Guid AllInvestigationsAllUsers = ExportParamsHelper.AllExportsAllUsers;
}
