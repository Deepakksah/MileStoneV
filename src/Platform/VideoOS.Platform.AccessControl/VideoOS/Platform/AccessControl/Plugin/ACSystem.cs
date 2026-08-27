using System.Collections.Generic;
using VideoOS.Platform.AccessControl.Alarms;

namespace VideoOS.Platform.AccessControl.Plugin;

public abstract class ACSystem
{
	public abstract ACConnectionManager ConnectionManager { get; }

	public abstract ACConfigurationManager ConfigurationManager { get; }

	public abstract ACCredentialHolderManager CredentialHolderManager { get; }

	public abstract ACCommandManager CommandManager { get; }

	public abstract ACStateManager StateManager { get; }

	public abstract ACEventManager EventManager { get; }

	public abstract IEnumerable<ACExternalCommand> ExternalCommands { get; }

	public virtual bool PersonalizedLoginSupported => false;

	public IACAlarmRepository ACAlarmRepository { get; private set; }

	public ACSystem()
	{
		ACAlarmRepository = InternalAccessControlUtil.Instance.CreateACAlarmRepository();
	}

	public abstract void SetProperties(IEnumerable<ACProperty> properties);

	public abstract void Init(ACConfiguration acConfiguration);

	public abstract void Close();
}
