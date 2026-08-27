using System;
using System.Collections.Generic;

namespace VideoOS.Platform.AccessControl.Plugin;

public abstract class ACCredentialHolderManager
{
	public abstract bool CredentialHolderSearchSupported { get; }

	public abstract bool CredentialHolderImageOverrideEnabled { get; }

	public event EventHandler<ACCredentialHoldersChangedEventArgs> CredentialHoldersChanged = delegate
	{
	};

	public event EventHandler<ACCredentialHoldersInvalidatedEventArgs> CredentialHoldersInvalidated = delegate
	{
	};

	public abstract ACCredentialHolder GetCredentialHolder(string credentialHolderId);

	public abstract ACCredentialHolderSearchResults SearchCredentialHolders(string searchString, int searchLimit);

	protected void FireCredentialHoldersChanged(IEnumerable<ACCredentialHolder> acCredentialHolders)
	{
		this.CredentialHoldersChanged(this, new ACCredentialHoldersChangedEventArgs(acCredentialHolders));
	}

	protected void FireCredentialHoldersInvalidated(IEnumerable<string> acCredentialHolderIds)
	{
		this.CredentialHoldersInvalidated(this, new ACCredentialHoldersInvalidatedEventArgs(acCredentialHolderIds));
	}
}
