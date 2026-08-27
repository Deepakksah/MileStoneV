using System.Collections.Generic;

namespace VideoOS.Common.Interfaces;

public interface ISystemDashboard
{
	int Id { get; set; }

	string Name { get; set; }

	IList<int> EventTypes { get; set; }

	string StateSourceType { get; set; }

	string SingleStateSourceId { get; set; }

	int Position { get; set; }
}
