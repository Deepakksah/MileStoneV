using System;
using System.Collections.ObjectModel;

namespace VideoOS.Management.VmoClient;

public sealed class MarkedDataReference
{
	private ReadOnlyCollection<string> devices;

	public IVmoRepositories Repositories { get; set; }

	public string Reference { get; public set; }

	public DateTime Time { get; public set; }

	public ReadOnlyCollection<string> Devices => devices;

	public MarkedDataReference(IVmoRepositories repositories, string reference, DateTime time, string[] devices)
	{
		Repositories = repositories;
		Reference = reference;
		Time = time;
		this.devices = new ReadOnlyCollection<string>(devices);
	}
}
