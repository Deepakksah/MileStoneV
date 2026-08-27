using System;

namespace VideoOS.Common.Interfaces;

public interface ISystemDashboardStateSource
{
	string Name { get; set; }

	Guid Id { get; set; }
}
