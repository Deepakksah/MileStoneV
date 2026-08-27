using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VideoOS.Platform.AccessControl.Alarms;
using VideoOS.Platform.AccessControl.Plugin;

namespace VideoOS.Platform.AccessControl.Internal.PluginWrappers;

public class ACSystemWrapper
{
	private ACSystem _acSystem;

	private ACConnectionManagerWrapper _acConnectionManagerWrapper;

	private ACConfigurationManagerWrapper _acConfigurationManagerWrapper;

	private ACCredentialHolderManagerWrapper _acCredentialHolderManagerWrapper;

	private ACCommandManagerWrapper _acCommandManagerWrapper;

	private ACStateManagerWrapper _acStateManagerWrapper;

	private ACEventManagerWrapper _acEventManagerWrapper;

	public IEnumerable<ACExternalCommand> ExternalCommands
	{
		get
		{
			IEnumerable<ACExternalCommand> enumerable = ACPluginWrapper.TryCallPlugin(() => _acSystem.ExternalCommands, _acSystem.GetType(), "ExternalCommands (Get)", new ACExternalCommand[0]);
			return new ReadOnlyCollection<ACExternalCommand>((enumerable != null) ? enumerable.ToList() : new List<ACExternalCommand>());
		}
	}

	public ACConnectionManagerWrapper ConnectionManager => _acConnectionManagerWrapper;

	public ACConfigurationManagerWrapper ConfigurationManager => _acConfigurationManagerWrapper;

	public ACCredentialHolderManagerWrapper CredentialHolderManager => _acCredentialHolderManagerWrapper;

	public ACCommandManagerWrapper CommandManager => _acCommandManagerWrapper;

	public ACStateManagerWrapper StateManager => _acStateManagerWrapper;

	public ACEventManagerWrapper EventManager => _acEventManagerWrapper;

	public bool PersonalizedLoginSupported => ACPluginWrapper.TryCallPlugin(() => _acSystem.PersonalizedLoginSupported, _acSystem.GetType(), "PersonalizedLoginSupported (Get)", defaultValue: false);

	public IACAlarmRepository ACAlarmRepository => _acSystem.ACAlarmRepository;

	public ACSystemWrapper(ACSystem acSystem)
	{
		_acSystem = acSystem;
	}

	public void SetProperties(IEnumerable<ACProperty> properties)
	{
		properties = new ReadOnlyCollection<ACProperty>((properties != null) ? properties.ToList() : new List<ACProperty>());
		ACPluginWrapper.TryCallPlugin(delegate
		{
			_acSystem.SetProperties(properties);
		}, _acSystem.GetType(), "SetProperties()");
	}

	public void Init(ACConfiguration acConfiguration)
	{
		ACPluginWrapper.TryCallPlugin(delegate
		{
			_acSystem.Init(acConfiguration);
		}, _acSystem.GetType(), "Init()");
		ACConnectionManager aCConnectionManager = ACPluginWrapper.TryCallPlugin(() => _acSystem.ConnectionManager, _acSystem.GetType(), "ConnectionManager (Get)", null);
		_acConnectionManagerWrapper = ((aCConnectionManager != null) ? new ACConnectionManagerWrapper(aCConnectionManager) : null);
		ACConfigurationManager aCConfigurationManager = ACPluginWrapper.TryCallPlugin(() => _acSystem.ConfigurationManager, _acSystem.GetType(), "ConfigurationManager (Get)", null);
		_acConfigurationManagerWrapper = ((aCConfigurationManager != null) ? new ACConfigurationManagerWrapper(aCConfigurationManager) : null);
		ACCredentialHolderManager aCCredentialHolderManager = ACPluginWrapper.TryCallPlugin(() => _acSystem.CredentialHolderManager, _acSystem.GetType(), "CredentialHolderManager (Get)", null);
		_acCredentialHolderManagerWrapper = ((aCCredentialHolderManager != null) ? new ACCredentialHolderManagerWrapper(aCCredentialHolderManager) : null);
		ACCommandManager aCCommandManager = ACPluginWrapper.TryCallPlugin(() => _acSystem.CommandManager, _acSystem.GetType(), "CommandManager (Get)", null);
		_acCommandManagerWrapper = ((aCCommandManager != null) ? new ACCommandManagerWrapper(aCCommandManager) : null);
		ACStateManager aCStateManager = ACPluginWrapper.TryCallPlugin(() => _acSystem.StateManager, _acSystem.GetType(), "StateManager (Get)", null);
		_acStateManagerWrapper = ((aCStateManager != null) ? new ACStateManagerWrapper(aCStateManager) : null);
		ACEventManager aCEventManager = ACPluginWrapper.TryCallPlugin(() => _acSystem.EventManager, _acSystem.GetType(), "EventManager (Get)", null);
		_acEventManagerWrapper = ((aCEventManager != null) ? new ACEventManagerWrapper(aCEventManager) : null);
	}

	public void Close()
	{
		if (_acConfigurationManagerWrapper != null)
		{
			_acConfigurationManagerWrapper.Close();
		}
		ACPluginWrapper.TryCallPlugin(delegate
		{
			_acSystem.Close();
		}, _acSystem.GetType(), "Close()");
	}
}
