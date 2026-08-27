using System;
using VideoOS.Platform.AccessControl.Plugin;

namespace VideoOS.Platform.AccessControl.Internal.PluginWrappers;

public class ACConnectionManagerWrapper
{
	private ACConnectionManager _acConnectionManager;

	public event EventHandler<ACSystemConnectionStateChangedEventArgs> ACSystemConnectionStateChanged = delegate
	{
	};

	public ACConnectionManagerWrapper(ACConnectionManager acConnectionManager)
	{
		_acConnectionManager = acConnectionManager;
		_acConnectionManager.ACSystemConnectionStateChanged += _acConnectionManager_ACSystemConnectionStateChanged;
	}

	public void Connect()
	{
		ACPluginWrapper.TryCallPlugin(delegate
		{
			_acConnectionManager.Connect();
		}, _acConnectionManager.GetType(), "Connect()");
	}

	public void Disconnect()
	{
		ACPluginWrapper.TryCallPlugin(delegate
		{
			_acConnectionManager.Disconnect();
		}, _acConnectionManager.GetType(), "Disconnect()");
	}

	public ACUserCredentialsValidationResult ValidateUserCredentials(string username, string password)
	{
		return ACPluginWrapper.TryCallPlugin(() => _acConnectionManager.ValidateUserCredentials(username, password), _acConnectionManager.GetType(), "ValidateUserCredentials(username, password)", null);
	}

	private void _acConnectionManager_ACSystemConnectionStateChanged(object sender, ACSystemConnectionStateChangedEventArgs e)
	{
		ACPluginWrapper.TryHandleEvent(delegate
		{
			this.ACSystemConnectionStateChanged(this, e);
		}, _acConnectionManager.GetType(), "ACSystemConnectionStateChanged");
	}
}
