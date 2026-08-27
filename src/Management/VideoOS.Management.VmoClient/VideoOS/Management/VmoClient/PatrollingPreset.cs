using System;

namespace VideoOS.Management.VmoClient;

public sealed class PatrollingPreset
{
	public string Id { get; private set; }

	public PatrollingPreset(string id)
	{
		if (id == null)
		{
			throw new ArgumentNullException("id");
		}
		if (id.Trim().Length == 0)
		{
			throw new ArgumentException("Id can not be an empty", "id");
		}
		if (id.Equals(Guid.Empty.ToString(), StringComparison.OrdinalIgnoreCase))
		{
			throw new ArgumentException("Id can not be an empty guid", "id");
		}
		Id = id;
	}
}
