using System.Collections.Generic;
using VideoOS.Mobile.Portable.MetaChannel;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

public class EncryptionProperties
{
	public bool ServerSupportsCommandEncryption { get; set; }

	public bool RequireCommandEncryption { get; set; }

	public List<Command.CommandKinds> CommandEncryptionList { get; set; }
}
