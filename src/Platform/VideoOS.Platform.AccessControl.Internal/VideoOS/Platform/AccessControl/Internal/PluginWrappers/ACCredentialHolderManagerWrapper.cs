using System;
using VideoOS.Platform.AccessControl.Plugin;

namespace VideoOS.Platform.AccessControl.Internal.PluginWrappers;

public class ACCredentialHolderManagerWrapper
{
	private ACCredentialHolderManager _acCredentialHolderManager;

	public bool CredentialHolderSearchSupported => ACPluginWrapper.TryCallPlugin(() => _acCredentialHolderManager.CredentialHolderSearchSupported, _acCredentialHolderManager.GetType(), "CredentialHolderSearchSupported (Get)", defaultValue: false);

	public bool CredentialHolderImageOverrideEnabled => ACPluginWrapper.TryCallPlugin(() => _acCredentialHolderManager.CredentialHolderImageOverrideEnabled, _acCredentialHolderManager.GetType(), "CredentialHolderImageOverrideEnabled (Get)", defaultValue: false);

	public event EventHandler<ACCredentialHoldersChangedEventArgs> CredentialHoldersChanged = delegate
	{
	};

	public event EventHandler<ACCredentialHoldersInvalidatedEventArgs> CredentialHoldersInvalidated = delegate
	{
	};

	public ACCredentialHolderManagerWrapper(ACCredentialHolderManager acCredentialHolderManager)
	{
		_acCredentialHolderManager = acCredentialHolderManager;
		_acCredentialHolderManager.CredentialHoldersChanged += _acCredentialHolderManager_CredentialHoldersChanged;
		_acCredentialHolderManager.CredentialHoldersInvalidated += _acCredentialHolderManager_CredentialHoldersInvalidated;
	}

	public ACCredentialHolder GetCredentialHolder(string credentialHolderId)
	{
		return ACPluginWrapper.TryCallPlugin(() => _acCredentialHolderManager.GetCredentialHolder(credentialHolderId), _acCredentialHolderManager.GetType(), "GetCredentialHolder()", null);
	}

	public ACCredentialHolderSearchResults SearchCredentialHolders(string searchString, int searchLimit)
	{
		return ACPluginWrapper.TryCallPlugin(() => _acCredentialHolderManager.SearchCredentialHolders(searchString, searchLimit), _acCredentialHolderManager.GetType(), "SearchCredentialHolders()", null);
	}

	private void _acCredentialHolderManager_CredentialHoldersChanged(object sender, ACCredentialHoldersChangedEventArgs e)
	{
		ACPluginWrapper.TryHandleEvent(delegate
		{
			this.CredentialHoldersChanged(this, e);
		}, _acCredentialHolderManager.GetType(), "CredentialHoldersChanged");
	}

	private void _acCredentialHolderManager_CredentialHoldersInvalidated(object sender, ACCredentialHoldersInvalidatedEventArgs e)
	{
		ACPluginWrapper.TryHandleEvent(delegate
		{
			this.CredentialHoldersInvalidated(this, e);
		}, _acCredentialHolderManager.GetType(), "CredentialHoldersInvalidated");
	}
}
