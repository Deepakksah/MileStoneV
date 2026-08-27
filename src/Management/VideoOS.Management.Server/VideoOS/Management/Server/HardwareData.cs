using System;
using System.Collections.Generic;
using VideoOS.Common.Integration.Definition;

namespace VideoOS.Management.Server;

public sealed class HardwareData
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public Guid RecorderId { get; set; }

	public Subtypes Subtype { get; set; }

	public string Uri { get; set; }

	public DateTime LastModified { get; set; }

	public static IEnumerable<HardwareData> QueryHardwareByIds(IGeminiRequestContext requestContext, Guid[] ids)
	{
		using IHardwareDataComponent hardwareDataComponent = requestContext.CreateHardwareDataComponent();
		return hardwareDataComponent.QueryHardwareByIds(ids);
	}

	public static IEnumerable<HardwareModifiedData> QueryHardwareChangedAfter(IGeminiRequestContext requestContext, DateTime changesFrom)
	{
		using IHardwareDataComponent hardwareDataComponent = requestContext.CreateHardwareDataComponent();
		return hardwareDataComponent.QueryHardwareChangedAfter(changesFrom);
	}
}
