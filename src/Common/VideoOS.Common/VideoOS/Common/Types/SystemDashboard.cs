using System.Collections.Generic;
using VideoOS.Common.Interfaces;

namespace VideoOS.Common.Types;

public class SystemDashboard : ISystemDashboard
{
	public int Id { get; set; }

	public string Name { get; set; }

	public IList<int> EventTypes { get; set; }

	public string StateSourceType { get; set; }

	public string SingleStateSourceId { get; set; }

	public int Position { get; set; }
}
