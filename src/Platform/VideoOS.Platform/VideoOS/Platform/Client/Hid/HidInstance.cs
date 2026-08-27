using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Platform.Client.Hid;

public abstract class HidInstance
{
	public Guid Id { get; }

	public virtual string Name { get; protected set; }

	protected HidInstance(Guid id)
	{
		Id = id;
	}

	public virtual void Init()
	{
	}

	public virtual void Close()
	{
	}

	public abstract Task<ConnectResult> ConnectAsync(string userDefinedName, IEnumerable<HidConfigurationProperty> configurationProperties, CancellationToken cancellationToken);

	public abstract Task DisconnectAsync(CancellationToken cancellationToken);
}
