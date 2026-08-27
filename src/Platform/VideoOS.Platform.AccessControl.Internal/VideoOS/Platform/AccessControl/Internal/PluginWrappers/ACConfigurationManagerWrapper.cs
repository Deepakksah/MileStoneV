using System;
using System.Collections.Generic;
using VideoOS.Platform.AccessControl.Plugin;

namespace VideoOS.Platform.AccessControl.Internal.PluginWrappers;

public class ACConfigurationManagerWrapper
{
	private ACConfigurationManager _acConfigurationManager;

	public event EventHandler<ACFetchConfigurationStatusChangedEventArgs> FetchConfigurationStatusChanged = delegate
	{
	};

	public event EventHandler<ACFetchPersonalizedConfigurationStatusChangedEventArgs> FetchPersonalizedConfigurationStatusChanged = delegate
	{
	};

	public event EventHandler<ACPersonalizedConfigurationChangedEventArgs> PersonalizedConfigurationChanged = delegate
	{
	};

	public ACConfigurationManagerWrapper(ACConfigurationManager acConfigurationManager)
	{
		_acConfigurationManager = acConfigurationManager;
		_acConfigurationManager.FetchConfigurationStatusChanged += _acConfigurationManager_FetchConfigurationStatusChanged;
		_acConfigurationManager.FetchPersonalizedConfigurationStatusChanged += _acConfigurationManager_FetchPersonalizedConfigurationStatusChanged;
		_acConfigurationManager.PersonalizedConfigurationChanged += _acConfigurationManager_PersonalizedConfigurationChanged;
	}

	public void Close()
	{
		_acConfigurationManager.FetchConfigurationStatusChanged -= _acConfigurationManager_FetchConfigurationStatusChanged;
		_acConfigurationManager.FetchPersonalizedConfigurationStatusChanged -= _acConfigurationManager_FetchPersonalizedConfigurationStatusChanged;
		_acConfigurationManager.PersonalizedConfigurationChanged -= _acConfigurationManager_PersonalizedConfigurationChanged;
	}

	public void StartFetchConfiguration()
	{
		ACPluginWrapper.TryCallPlugin(delegate
		{
			_acConfigurationManager.StartFetchConfiguration();
		}, _acConfigurationManager.GetType(), "StartFetchConfiguration()");
	}

	public bool StartFetchPersonalizedConfiguration(string username, string password, string configVersion)
	{
		return ACPluginWrapper.TryCallPlugin(() => _acConfigurationManager.StartFetchPersonalizedConfiguration(username, password, configVersion), _acConfigurationManager.GetType(), "StartFetchPersonalizedConfiguration()", defaultValue: false);
	}

	public void CancelFetchConfiguration()
	{
		ACPluginWrapper.TryCallPlugin(delegate
		{
			_acConfigurationManager.CancelFetchConfiguration();
		}, _acConfigurationManager.GetType(), "CancelFetchConfiguration()");
	}

	public void CancelFetchPersonalizedConfiguration(string username)
	{
		ACPluginWrapper.TryCallPlugin(delegate
		{
			_acConfigurationManager.CancelFetchPersonalizedConfiguration(username);
		}, _acConfigurationManager.GetType(), "CancelFetchPersonalizedConfiguration()");
	}

	public void ApplyConfiguration(ACConfiguration acConfiguration)
	{
		ACPluginWrapper.TryCallPlugin(delegate
		{
			_acConfigurationManager.ApplyConfiguration(acConfiguration);
		}, _acConfigurationManager.GetType(), "ApplyConfiguration()");
	}

	public void OperationableInstanceEnabledStateChanged(IEnumerable<Tuple<string, bool>> acOperationableInstances)
	{
		ACPluginWrapper.TryCallPlugin(delegate
		{
			_acConfigurationManager.OperationableInstanceEnabledStateChanged(acOperationableInstances);
		}, _acConfigurationManager.GetType(), "OperationableInstanceEnabledStateChanged()");
	}

	public void EventTypeEnabledStateChanged(IEnumerable<Tuple<string, bool>> acEventTypes)
	{
		ACPluginWrapper.TryCallPlugin(delegate
		{
			_acConfigurationManager.EventTypeEnabledStateChanged(acEventTypes);
		}, _acConfigurationManager.GetType(), "EventTypeEnabledStateChanged()");
	}

	public void AccessControlUnitPositionChanged(IEnumerable<Tuple<string, double, double>> acUnitPositions)
	{
		ACPluginWrapper.TryCallPlugin(delegate
		{
			_acConfigurationManager.AccessControlUnitPositionChanged(acUnitPositions);
		}, _acConfigurationManager.GetType(), "AccessControlUnitPositionChanged()");
	}

	private void _acConfigurationManager_FetchConfigurationStatusChanged(object sender, ACFetchConfigurationStatusChangedEventArgs e)
	{
		ACPluginWrapper.TryHandleEvent(delegate
		{
			this.FetchConfigurationStatusChanged(this, e);
		}, _acConfigurationManager.GetType(), "FetchConfigurationStatusChanged");
	}

	private void _acConfigurationManager_FetchPersonalizedConfigurationStatusChanged(object sender, ACFetchPersonalizedConfigurationStatusChangedEventArgs e)
	{
		ACPluginWrapper.TryHandleEvent(delegate
		{
			this.FetchPersonalizedConfigurationStatusChanged(this, e);
		}, _acConfigurationManager.GetType(), "FetchPersonalizedConfigurationStatusChanged");
	}

	private void _acConfigurationManager_PersonalizedConfigurationChanged(object sender, ACPersonalizedConfigurationChangedEventArgs e)
	{
		ACPluginWrapper.TryHandleEvent(delegate
		{
			this.PersonalizedConfigurationChanged(this, e);
		}, _acConfigurationManager.GetType(), "PersonalizedConfigurationChanged");
	}
}
