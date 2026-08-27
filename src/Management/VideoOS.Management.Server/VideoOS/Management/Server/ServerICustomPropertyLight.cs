using System;
using VideoOS.Common.SharedComponent.ViewHandlingInterfaces;

namespace VideoOS.Management.Server;

public class ServerICustomPropertyLight : ICustomProperty
{
	public string Value { get; set; }

	public string Key { get; set; }

	public void Create()
	{
		throw new NotSupportedException();
	}

	public void Delete()
	{
		throw new NotSupportedException();
	}

	public void Update()
	{
		throw new NotSupportedException();
	}
}
