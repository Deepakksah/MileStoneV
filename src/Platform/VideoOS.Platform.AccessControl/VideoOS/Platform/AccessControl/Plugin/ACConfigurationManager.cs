using System;
using System.Collections.Generic;

namespace VideoOS.Platform.AccessControl.Plugin;

public abstract class ACConfigurationManager
{
	public event EventHandler<ACFetchConfigurationStatusChangedEventArgs> FetchConfigurationStatusChanged = delegate
	{
	};

	public event EventHandler<ACFetchPersonalizedConfigurationStatusChangedEventArgs> FetchPersonalizedConfigurationStatusChanged = delegate
	{
	};

	public event EventHandler<ACPersonalizedConfigurationChangedEventArgs> PersonalizedConfigurationChanged = delegate
	{
	};

	protected void FireFetchConfigurationStatusChanged(ACFetchConfigurationStatusChangedEventArgs acFetchConfigurationStatusChangedEventArgs)
	{
		this.FetchConfigurationStatusChanged(this, acFetchConfigurationStatusChangedEventArgs);
	}

	protected void FireFetchPersonalizedConfigurationStatusChanged(ACFetchPersonalizedConfigurationStatusChangedEventArgs acFetchPersonalizedConfigurationStatusChangedEventArgs)
	{
		this.FetchPersonalizedConfigurationStatusChanged(this, acFetchPersonalizedConfigurationStatusChangedEventArgs);
	}

	protected void FirePersonalizedConfigurationChanged(string username)
	{
		this.PersonalizedConfigurationChanged(this, new ACPersonalizedConfigurationChangedEventArgs(username));
	}

	public abstract void StartFetchConfiguration();

	public virtual bool StartFetchPersonalizedConfiguration(string username, string password, string configVersion)
	{
		throw new NotImplementedException();
	}

	public abstract void CancelFetchConfiguration();

	public virtual void CancelFetchPersonalizedConfiguration(string username)
	{
		throw new NotImplementedException();
	}

	public abstract void ApplyConfiguration(ACConfiguration acConfiguration);

	public virtual void OperationableInstanceEnabledStateChanged(IEnumerable<Tuple<string, bool>> acOperationableInstances)
	{
	}

	public virtual void EventTypeEnabledStateChanged(IEnumerable<Tuple<string, bool>> acEventTypes)
	{
	}

	public virtual void AccessControlUnitPositionChanged(IEnumerable<Tuple<string, double, double>> acUnitPositions)
	{
	}
}
