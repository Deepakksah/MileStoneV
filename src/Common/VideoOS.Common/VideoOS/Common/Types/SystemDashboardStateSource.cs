using System;
using VideoOS.Common.Interfaces;

namespace VideoOS.Common.Types;

public class SystemDashboardStateSource : ISystemDashboardStateSource
{
	public string Name { get; set; }

	public Guid Id { get; set; }
}
