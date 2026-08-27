using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Principal;

namespace VideoOS.Management.VmoClient;

public class VmoRepositories : IVmoRepositories
{
	private IVmoClient _vmoClient;

	private readonly Dictionary<Type, GenericRepositoryBase> _repositories;

	private readonly object _repositoryLocker = new object();

	private INotificationEngine _notification;

	private IServer _serverCommuncation;

	object IVmoRepositories.RepositoryLocker => _repositoryLocker;

	INotificationEngine IVmoRepositories.Notification => _notification;

	string IVmoRepositories.ConnectedUser
	{
		get
		{
			string result = null;
			if (_vmoClient.ServerConnection.ConnectAsUser)
			{
				result = _vmoClient.ServerConnection.ConnectAsUserName;
			}
			else
			{
				using WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent();
				result = windowsIdentity.Name;
			}
			return result;
		}
	}

	IServer IVmoRepositories.ServerCommunication
	{
		[DebuggerStepThrough]
		get
		{
			if (_serverCommuncation == null)
			{
				if (_vmoClient.ServerConnection.ServerVersion < ServerVersions.v22_1)
				{
					_serverCommuncation = new ServerCommunicationGenericBase(_vmoClient);
				}
				else if (_vmoClient.ServerConnection.ServerVersion >= ServerVersions.v22_1 && _vmoClient.ServerConnection.ServerVersion < ServerVersions.v22_2)
				{
					_serverCommuncation = new ServerCommunicationGeneric221(_vmoClient);
				}
				else
				{
					_serverCommuncation = new ServerCommunicationGeneric222(_vmoClient);
				}
			}
			return _serverCommuncation;
		}
	}

	public ServerVersion ServerVersion => (ServerVersion)_vmoClient.ServerConnection.ServerVersion.Clone();

	public VmoRepositories(IVmoClient vmoClient)
	{
		_notification = new NotificationEngine(this);
		_vmoClient = vmoClient;
		_repositories = CreateRepositories(this);
	}

	public static Dictionary<Type, GenericRepositoryBase> CreateRepositories(IVmoRepositories owner)
	{
		Dictionary<Type, GenericRepositoryBase> obj = new Dictionary<Type, GenericRepositoryBase>
		{
			{
				typeof(ArchiveStorage),
				new GenericRepository<ArchiveStorage>(owner)
			},
			{
				typeof(AudioMessage),
				new GenericRepository<AudioMessage>(owner)
			},
			{
				typeof(BasicOwnerInfo),
				new GenericRepository<BasicOwnerInfo>(owner)
			},
			{
				typeof(BasicUser),
				new GenericRepository<BasicUser>(owner)
			},
			{
				typeof(RegisteredClaim),
				new GenericRepository<RegisteredClaim>(owner)
			},
			{
				typeof(FailoverServer),
				new GenericRepository<FailoverServer>(owner)
			},
			{
				typeof(Hardware),
				new GenericRepository<Hardware>(owner)
			},
			{
				typeof(LoginProvider),
				new GenericRepository<LoginProvider>(owner)
			},
			{
				typeof(TrustedIssuer),
				new GenericRepository<TrustedIssuer>(owner)
			},
			{
				typeof(ManagementServer),
				new GenericRepository<ManagementServer>(owner)
			},
			{
				typeof(MIPAction),
				new GenericRepository<MIPAction>(owner)
			},
			{
				typeof(MIPObject),
				new GenericRepository<MIPObject>(owner)
			},
			{
				typeof(MotionDetection),
				new GenericRepository<MotionDetection>(owner)
			},
			{
				typeof(MonitorPreset),
				new GenericRepository<MonitorPreset>(owner)
			},
			{
				typeof(Monitor),
				new GenericRepository<Monitor>(owner)
			},
			{
				typeof(PatrollingProfile),
				new GenericRepository<PatrollingProfile>(owner)
			},
			{
				typeof(PerformanceStateRuleConfiguration),
				new GenericRepository<PerformanceStateRuleConfiguration>(owner)
			},
			{
				typeof(PrivacyProtection),
				new GenericRepository<PrivacyProtection>(owner)
			},
			{
				typeof(RecordingServer),
				new GenericRepository<RecordingServer>(owner)
			},
			{
				typeof(RecordingStorage),
				new GenericRepository<RecordingStorage>(owner)
			},
			{
				typeof(RetentionOptionRoleRelation),
				new GenericRepository<RetentionOptionRoleRelation>(owner)
			},
			{
				typeof(Role),
				new GenericRepository<Role>(owner)
			},
			{
				typeof(Rule),
				new GenericRepository<Rule>(owner)
			},
			{
				typeof(RuleSource),
				new GenericRepository<RuleSource>(owner)
			},
			{
				typeof(SystemDashboardConfiguration),
				new GenericRepository<SystemDashboardConfiguration>(owner)
			},
			{
				typeof(UserDefinedEvent),
				new GenericRepository<UserDefinedEvent>(owner)
			},
			{
				typeof(VideoWallPreset),
				new GenericRepository<VideoWallPreset>(owner)
			},
			{
				typeof(VideoWall),
				new GenericRepository<VideoWall>(owner)
			},
			{
				typeof(View),
				new GenericRepository<View>(owner)
			},
			{
				typeof(ViewItem),
				new GenericRepository<ViewItem>(owner)
			},
			{
				typeof(ViewLayout),
				new GenericRepository<ViewLayout>(owner)
			}
		};
		GenericRepository<PluginEvent> value = new GenericRepository<PluginEvent>(owner);
		obj.Add(typeof(PluginEvent), value);
		obj.Add(typeof(MIPEvent), value);
		obj.Add(typeof(MIPDeviceEvent), value);
		GenericRepository<Device> value2 = new GenericRepository<Device>(owner);
		obj.Add(typeof(Device), value2);
		obj.Add(typeof(InputDevice), value2);
		obj.Add(typeof(OutputDevice), value2);
		obj.Add(typeof(RecordingDevice), value2);
		obj.Add(typeof(CameraDevice), value2);
		obj.Add(typeof(SpeakerDevice), value2);
		obj.Add(typeof(MicrophoneDevice), value2);
		obj.Add(typeof(MetadataDevice), value2);
		GenericRepository<DeviceGroup> value3 = new GenericRepository<DeviceGroup>(owner);
		obj.Add(typeof(DeviceGroup), value3);
		obj.Add(typeof(InputDeviceGroup), value3);
		obj.Add(typeof(OutputDeviceGroup), value3);
		obj.Add(typeof(CameraDeviceGroup), value3);
		obj.Add(typeof(SpeakerDeviceGroup), value3);
		obj.Add(typeof(MicrophoneDeviceGroup), value3);
		obj.Add(typeof(MetadataDeviceGroup), value3);
		GenericRepository<Site> value4 = new GenericRepository<Site>(owner);
		obj.Add(typeof(Site), value4);
		obj.Add(typeof(ChildSite), value4);
		obj.Add(typeof(MasterSite), value4);
		GenericRepository<TimeProfile> value5 = new GenericRepository<TimeProfile>(owner);
		obj.Add(typeof(TimeProfile), value5);
		obj.Add(typeof(SunClockTimeProfile), value5);
		obj.Add(typeof(CalendarTimeProfile), value5);
		GenericRepository<HostedHardware> value6 = new GenericRepository<HostedHardware>(owner);
		obj.Add(typeof(HostedHardware), value6);
		obj.Add(typeof(AxisOneClickCamera), value6);
		GenericRepository<HostingServer> value7 = new GenericRepository<HostingServer>(owner);
		obj.Add(typeof(HostingServer), value7);
		obj.Add(typeof(AxisTunnelServer), value7);
		GenericRepository<ServerGroup> value8 = new GenericRepository<ServerGroup>(owner);
		obj.Add(typeof(ServerGroup), value8);
		obj.Add(typeof(ViewGroup), value8);
		obj.Add(typeof(PublicViewGroup), value8);
		obj.Add(typeof(PrivateViewGroup), value8);
		obj.Add(typeof(ViewLayoutGroup), value8);
		obj.Add(typeof(FailoverServerGroup), value8);
		obj.Add(value: new GenericRepository<MIPObjectGroup>(owner), key: typeof(MIPObjectGroup));
		GenericRepository<ClientProfile> value9 = new GenericRepository<ClientProfile>(owner);
		obj.Add(typeof(ClientProfile), value9);
		obj.Add(typeof(ManagementClientProfile), value9);
		obj.Add(typeof(SmartClientProfile), value9);
		obj.Add(typeof(MetadataCategory), new GenericRepository<MetadataCategory>(owner));
		obj.Add(typeof(MetadataClassCandidate), new GenericRepository<MetadataClassCandidate>(owner));
		obj.Add(typeof(MetadataFilter), new GenericRepository<MetadataFilter>(owner));
		obj.Add(typeof(MetadataObjectDefinition), new GenericRepository<MetadataObjectDefinition>(owner));
		obj.Add(typeof(MetadataSearchPath), new GenericRepository<MetadataSearchPath>(owner));
		obj.Add(typeof(MetadataSearchNamespace), new GenericRepository<MetadataSearchNamespace>(owner));
		obj.Add(typeof(MetadataProperty), new GenericRepository<MetadataProperty>(owner));
		return obj;
	}

	GenericRepository<V> IVmoRepositories.GetGenericRepository<V>()
	{
		GenericRepositoryBase value = null;
		_repositories.TryGetValue(typeof(V), out value);
		return value as GenericRepository<V>;
	}

	GenericRepositoryBase IVmoRepositories.GetGenericRepositoryBase(Type type)
	{
		GenericRepositoryBase value = null;
		_repositories.TryGetValue(type, out value);
		return value;
	}

	bool IVmoRepositories.CheckVersion(ServerVersion requiredVersion)
	{
		return _vmoClient.ServerConnection.CheckVersion(requiredVersion);
	}

	public TObject GetObjectById<TObject>(int id, bool refreshCachedObjects) where TObject : VmoObjectIntBased
	{
		return ((IVmoRepositories)this).GetGenericRepositoryBase(typeof(TObject)).GetObjectById<TObject>(id.ToString(CultureInfo.InvariantCulture), refreshCachedObjects);
	}

	public TObject GetObjectById<TObject>(Guid id, bool refreshCachedObjects) where TObject : VmoObjectGuidBased
	{
		return ((IVmoRepositories)this).GetGenericRepositoryBase(typeof(TObject)).GetObjectById<TObject>(id.ToString(), refreshCachedObjects);
	}

	public TObject[] GetObjectsByIds<TObject>(int[] id, bool refreshCachedObjects) where TObject : VmoObjectIntBased
	{
		return ((IVmoRepositories)this).GetGenericRepositoryBase(typeof(TObject)).GetObjectsByIds<TObject>(id.Select((int f) => f.ToString(CultureInfo.InvariantCulture)).ToArray(), refreshCachedObjects);
	}

	public TObject[] GetObjectsByIds<TObject>(Guid[] id, bool refreshCachedObjects) where TObject : VmoObjectGuidBased
	{
		return ((IVmoRepositories)this).GetGenericRepositoryBase(typeof(TObject)).GetObjectsByIds<TObject>(id.Select((Guid f) => f.ToString(null, CultureInfo.InvariantCulture)).ToArray(), refreshCachedObjects);
	}

	public TObject[] GetObjectsByParentId<TParent, TObject>(int parentId, bool refreshCachedObjects) where TParent : VmoObjectIntBased where TObject : VmoObject
	{
		GenericRepositoryBase genericRepositoryBase = ((IVmoRepositories)this).GetGenericRepositoryBase(typeof(TObject));
		string[] objectRelations = genericRepositoryBase.GetObjectRelations(typeof(TParent), parentId.ToString(CultureInfo.InvariantCulture), typeof(TObject));
		return genericRepositoryBase.GetObjectsByIds<TObject>(objectRelations, refreshCachedObjects);
	}

	public TObject[] GetObjectsByParentId<TParent, TObject>(Guid parentId, bool refreshCachedObjects) where TParent : VmoObjectGuidBased where TObject : VmoObject
	{
		GenericRepositoryBase genericRepositoryBase = ((IVmoRepositories)this).GetGenericRepositoryBase(typeof(TObject));
		string[] objectRelations = genericRepositoryBase.GetObjectRelations(typeof(TParent), parentId.ToString(), typeof(TObject));
		return genericRepositoryBase.GetObjectsByIds<TObject>(objectRelations, refreshCachedObjects);
	}

	public TObject GetNonVmoObjectById<TObject>(Guid Id) where TObject : NonCachedVmoObject
	{
		return GetNonVmoObjectObjectById<TObject>(Id.ToString());
	}

	public TObject GetNonVmoObjectById<TObject>(int Id) where TObject : NonCachedVmoObject
	{
		return GetNonVmoObjectObjectById<TObject>(Id.ToString(CultureInfo.InvariantCulture));
	}

	private TObject GetNonVmoObjectObjectById<TObject>(string Id) where TObject : NonCachedVmoObject
	{
		TObject result = null;
		VMOWebserviceObject objectById = _serverCommuncation.GetObjectById(typeof(TObject), Id);
		if (objectById != null)
		{
			return NonCachedVmoObject.MapObject<TObject>(objectById, this);
		}
		return result;
	}

	public TObject[] GetNonVmoObjectsByParentId<TParent, TObject>(Guid parentId) where TParent : VmoObject where TObject : NonCachedVmoObject
	{
		return GetNonVmoObjectObjectByParentId<TParent, TObject>(parentId.ToString());
	}

	public TObject[] GetNonVmoObjectsByParentId<TParent, TObject>(int parentId) where TParent : VmoObject where TObject : NonCachedVmoObject
	{
		return GetNonVmoObjectObjectByParentId<TParent, TObject>(parentId.ToString(CultureInfo.InvariantCulture));
	}

	private TObject[] GetNonVmoObjectObjectByParentId<TParent, TObject>(string parentId) where TParent : VmoObject where TObject : NonCachedVmoObject
	{
		string[] objectRelations = _serverCommuncation.GetObjectRelations(typeof(TParent), parentId.ToString(), typeof(TObject));
		VMOWebserviceObject[] objectsByIds = _serverCommuncation.GetObjectsByIds(typeof(TObject), objectRelations);
		TObject[] result = null;
		if (objectsByIds != null)
		{
			result = objectsByIds.Select((VMOWebserviceObject w) => NonCachedVmoObject.MapObject<TObject>(w, this)).ToArray();
		}
		return result;
	}
}
