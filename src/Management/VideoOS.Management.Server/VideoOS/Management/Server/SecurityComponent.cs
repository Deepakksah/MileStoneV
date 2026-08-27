using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using VideoOS.Common.SharedTimeProfile;
using VideoOS.Management.Common;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public class SecurityComponent : SecurityResourceComponent, ISecurityComponent, IDisposable
{
	private static class QuerySecurityObject
	{
		private static DatabaseColumnBinder ObjectId = new DatabaseColumnBinder("object_id");

		public static string Bind(DatabaseDataReader reader)
		{
			return ObjectId.GetString(reader);
		}
	}

	private static class QuerySecurityObjectMap
	{
		private static DatabaseColumnBinder ObjectId = new DatabaseColumnBinder("object_id");

		private static DatabaseColumnBinder FullPath = new DatabaseColumnBinder("full_path");

		public static Tuple<string, string> Bind(DatabaseDataReader reader)
		{
			string item = ObjectId.GetString(reader);
			string item2 = FullPath.GetString(reader);
			return new Tuple<string, string>(item, item2);
		}
	}

	private static class QueryRolePermissionEntry
	{
		private static DatabaseColumnBinder ObjectId = new DatabaseColumnBinder("object_id");

		private static DatabaseColumnBinder ActionName = new DatabaseColumnBinder("action_name");

		public static RolePermissionObjectEntry Bind(DatabaseDataReader reader)
		{
			return new RolePermissionObjectEntry
			{
				ObjectId = ObjectId.GetString(reader),
				ActionName = ActionName.GetString(reader)
			};
		}
	}

	private static class QueryRolePermissionEntryAll
	{
		private static DatabaseColumnBinder ObjectId = new DatabaseColumnBinder("object_id");

		private static DatabaseColumnBinder ActionName = new DatabaseColumnBinder("action_name");

		private static DatabaseColumnBinder RoleId = new DatabaseColumnBinder("role_id");

		public static RolePermissionObjectEntry Bind(DatabaseDataReader reader)
		{
			return new RolePermissionObjectEntry
			{
				ObjectId = ObjectId.GetString(reader),
				ActionName = ActionName.GetString(reader),
				RoleId = RoleId.GetString(reader)
			};
		}
	}

	private static class QueryMatrixPermissionEntry
	{
		private static DatabaseColumnBinder ObjectId = new DatabaseColumnBinder("IDMatrix");

		private static DatabaseColumnBinder ActionName = new DatabaseColumnBinder("SecurityAttributeID");

		public static RolePermissionObjectEntry Bind(DatabaseDataReader reader)
		{
			string text = ActionName.GetString(reader);
			return new RolePermissionObjectEntry
			{
				ObjectId = ObjectId.GetString(reader),
				ActionName = ((text == "MATRIXVISIBLE") ? "GENERIC_READ" : text)
			};
		}
	}

	private static class QueryMatrixRolePermissionEntry
	{
		private static DatabaseColumnBinder ObjectId = new DatabaseColumnBinder("IDMatrix");

		private static DatabaseColumnBinder ActionName = new DatabaseColumnBinder("SecurityAttributeID");

		private static DatabaseColumnBinder Role = new DatabaseColumnBinder("IDRole");

		public static RolePermissionObjectEntry Bind(DatabaseDataReader reader)
		{
			string text = ActionName.GetString(reader);
			return new RolePermissionObjectEntry
			{
				ObjectId = ObjectId.GetString(reader),
				RoleId = Role.GetString(reader),
				ActionName = ((text == "MATRIXVISIBLE") ? "GENERIC_READ" : text)
			};
		}
	}

	private static class UpdateSecurityColumns
	{
		private static DatabaseColumnBinder SecurityChanged = new DatabaseColumnBinder("security_changed");

		public static bool Bind(DatabaseDataReader reader)
		{
			return SecurityChanged.GetBoolean(reader);
		}
	}

	private class SecurityNamespaceBinder : ObjectBinder<SecurityNamespaceInfo>
	{
		private DatabaseColumnBinder IdColumn = new DatabaseColumnBinder("namespace_id");

		private DatabaseColumnBinder NameColumn = new DatabaseColumnBinder("name");

		private DatabaseColumnBinder DisplayNameColumn = new DatabaseColumnBinder("display_name");

		private DatabaseColumnBinder SeparatorColumn = new DatabaseColumnBinder("separator");

		private DatabaseColumnBinder StructureColumn = new DatabaseColumnBinder("structure");

		private DatabaseColumnBinder UserDefinedColumn = new DatabaseColumnBinder("user_defined");

		protected override SecurityNamespaceInfo Bind()
		{
			return new SecurityNamespaceInfo
			{
				NamespaceId = IdColumn.GetGuid2(base.Reader).ToString(),
				Name = NameColumn.GetString(base.Reader, allowNulls: false),
				DisplayName = DisplayNameColumn.GetString(base.Reader, allowNulls: false),
				Separator = SeparatorColumn.GetString(base.Reader, allowNulls: false)[0],
				Structure = StructureColumn.GetInt32(base.Reader),
				UserDefined = UserDefinedColumn.GetBoolean(base.Reader)
			};
		}
	}

	private class ActionDefinitionBinder : ObjectBinder<ActionDefinition>
	{
		private DatabaseColumnBinder NamespaceIdColumn = new DatabaseColumnBinder("namespace_id");

		private DatabaseColumnBinder NameColumn = new DatabaseColumnBinder("name");

		private DatabaseColumnBinder DisplayNameColumn = new DatabaseColumnBinder("display_name");

		private DatabaseColumnBinder VisibleColumn = new DatabaseColumnBinder("visible");

		private DatabaseColumnBinder PermissionBitColumn = new DatabaseColumnBinder("security_bit");

		protected override ActionDefinition Bind()
		{
			return new ActionDefinition
			{
				NamespaceId = NamespaceIdColumn.GetGuid2(base.Reader).ToString(),
				Name = NameColumn.GetString(base.Reader, allowNulls: false),
				DisplayName = DisplayNameColumn.GetString(base.Reader, allowNulls: false),
				Visible = VisibleColumn.GetBoolean(base.Reader),
				PermissionBit = Convert.ToUInt64(PermissionBitColumn.GetDecimal(base.Reader))
			};
		}
	}

	private class AccessControlEntryBinder : ObjectBinder<AccessControlEntry>
	{
		private DatabaseColumnBinder SidColumn = new DatabaseColumnBinder("user_sid");

		private DatabaseColumnBinder ActionIdColumn = new DatabaseColumnBinder("action_name");

		private DatabaseColumnBinder AccessDenyColumn = new DatabaseColumnBinder("access_deny");

		protected override AccessControlEntry Bind()
		{
			return new AccessControlEntry
			{
				Sid = SidColumn.GetString(base.Reader).ToString(),
				ActionId = ActionIdColumn.GetString(base.Reader),
				AccessDeny = AccessDenyColumn.GetBoolean(base.Reader)
			};
		}
	}

	private class AccessControlTimeProfileObjectEntryBinder : ObjectBinder<AccessControlTimeProfileObjectEntry>
	{
		private DatabaseColumnBinder ObjectIdColumn = new DatabaseColumnBinder("object_id");

		private DatabaseColumnBinder ActionIdColumn = new DatabaseColumnBinder("action_name");

		private DatabaseColumnBinder TimeProfileId = new DatabaseColumnBinder("timeprofile_id");

		private DatabaseColumnBinder IsDefaultColumn = new DatabaseColumnBinder("is_default");

		protected override AccessControlTimeProfileObjectEntry Bind()
		{
			return new AccessControlTimeProfileObjectEntry
			{
				ObjectId = ObjectIdColumn.GetString(base.Reader).ToString(),
				ActionName = ActionIdColumn.GetString(base.Reader),
				TimeProfileId = TimeProfileId.GetString(base.Reader, allowNulls: true),
				IsDefaultTimeProfile = IsDefaultColumn.GetBoolean(base.Reader)
			};
		}
	}

	private class AccessControlTimeProfileObjectEntryRoleBinder : ObjectBinder<AccessControlTimeProfileObjectEntry>
	{
		private DatabaseColumnBinder ActionIdColumn = new DatabaseColumnBinder("action_name");

		private DatabaseColumnBinder TimeProfileId = new DatabaseColumnBinder("timeprofile_id");

		private DatabaseColumnBinder IsDefaultColumn = new DatabaseColumnBinder("is_default");

		private DatabaseColumnBinder RoleIdColumn = new DatabaseColumnBinder("role_id");

		protected override AccessControlTimeProfileObjectEntry Bind()
		{
			return new AccessControlTimeProfileObjectEntry
			{
				ActionName = ActionIdColumn.GetString(base.Reader),
				TimeProfileId = TimeProfileId.GetString(base.Reader, allowNulls: true),
				IsDefaultTimeProfile = IsDefaultColumn.GetBoolean(base.Reader),
				RoleId = RoleIdColumn.GetString(base.Reader)
			};
		}
	}

	private class AccessControlEntry2Binder : ObjectBinder<AccessControlEntry2>
	{
		private DatabaseColumnBinder TokenColumn = new DatabaseColumnBinder("security_token");

		private DatabaseColumnBinder SidColumn = new DatabaseColumnBinder("identity_sid");

		private DatabaseColumnBinder AllowPermissionColumn = new DatabaseColumnBinder("allow_permission");

		private DatabaseColumnBinder DenyPermissionColumn = new DatabaseColumnBinder("deny_permission");

		protected override AccessControlEntry2 Bind()
		{
			return new AccessControlEntryDetails
			{
				Token = TokenColumn.GetString(base.Reader).ToString(),
				Descriptor = new IdentityDescriptor(SidColumn.GetString(base.Reader).ToString()),
				Allow = Convert.ToUInt64(AllowPermissionColumn.GetDecimal(base.Reader)),
				Deny = Convert.ToUInt64(DenyPermissionColumn.GetDecimal(base.Reader))
			};
		}
	}

	private class AccessControlEntryDeviceTimeProfileBinder : ObjectBinder<AccessControlDeviceTimeProfileEntry>
	{
		private DatabaseColumnBinder LastModifiedColumn = new DatabaseColumnBinder("last_modified");

		private DatabaseColumnBinder ActionIdColumn = new DatabaseColumnBinder("action_name");

		private DatabaseColumnBinder RoleIdColumn = new DatabaseColumnBinder("role_id");

		private DatabaseColumnBinder DeviceIdColumn = new DatabaseColumnBinder("device_id");

		private DatabaseColumnBinder ValueColumn = new DatabaseColumnBinder("value");

		private DatabaseColumnBinder TimeProfileIdColumn = new DatabaseColumnBinder("time_profile_id");

		private DatabaseColumnBinder TimeBackwardColumn = new DatabaseColumnBinder("time_backward");

		private DatabaseColumnBinder IsDefaultColumn = new DatabaseColumnBinder("is_default");

		protected override AccessControlDeviceTimeProfileEntry Bind()
		{
			DateTime dateTime = LastModifiedColumn.GetDateTime(base.Reader);
			string actionId = ActionIdColumn.GetString(base.Reader);
			Guid roleId = RoleIdColumn.GetGuid(base.Reader).ToGuid();
			Guid deviceId = DeviceIdColumn.GetGuid(base.Reader).ToGuid();
			string value = ValueColumn.GetString(base.Reader);
			bool boolean = IsDefaultColumn.GetBoolean(base.Reader);
			int @int = TimeBackwardColumn.GetInt32(base.Reader);
			string text = TimeProfileIdColumn.GetString(base.Reader, allowNulls: true);
			return new AccessControlDeviceTimeProfileEntry
			{
				LastModified = dateTime,
				ActionId = actionId,
				RoleId = roleId,
				DeviceId = deviceId,
				Value = value,
				TimeProfileId = (text ?? string.Empty),
				TimeBackward = @int,
				IsDefaultTimeProfile = boolean
			};
		}
	}

	private class NoInheritanceBinder : ObjectBinder<string>
	{
		private DatabaseColumnBinder TokenColumn = new DatabaseColumnBinder("security_token");

		protected override string Bind()
		{
			return TokenColumn.GetString(base.Reader);
		}
	}

	private class ObjectTokenBinder : ObjectBinder<ObjectToken>
	{
		private DatabaseColumnBinder ObjectIdColumn = new DatabaseColumnBinder("object_id");

		private DatabaseColumnBinder TokenColumn = new DatabaseColumnBinder("token");

		protected override ObjectToken Bind()
		{
			return new ObjectToken
			{
				ObjectId = ObjectIdColumn.GetString(base.Reader).ToString(),
				Token = TokenColumn.GetString(base.Reader).ToString()
			};
		}
	}

	private static class AccessControlTimeProfileColumns
	{
		private static DatabaseColumnBinder TimeProfileId = new DatabaseColumnBinder("timeprofile_id");

		private static DatabaseColumnBinder IsDefaultTimeProfile = new DatabaseColumnBinder("is_default");

		public static AccessControlTimeProfile Bind(DatabaseDataReader reader)
		{
			AccessControlTimeProfile accessControlTimeProfile = new AccessControlTimeProfile();
			string text = TimeProfileId.GetString(reader, allowNulls: true, Guid.Empty.ToString());
			if (!text.Equals(Guid.Empty.ToString()))
			{
				accessControlTimeProfile.TimeProfileId = text.ToString();
			}
			accessControlTimeProfile.IsDefaultTimeProfile = IsDefaultTimeProfile.GetBoolean(reader);
			return accessControlTimeProfile;
		}
	}

	private static class AccessControlTimeProfileEntryColumns
	{
		private static DatabaseColumnBinder Sid = new DatabaseColumnBinder("user_sid");

		private static DatabaseColumnBinder IDAction = new DatabaseColumnBinder("action_name");

		private static DatabaseColumnBinder TimeProfileId = new DatabaseColumnBinder("timeprofile_id");

		private static DatabaseColumnBinder IsDefaultTimeProfile = new DatabaseColumnBinder("is_default");

		public static AccessControlTimeProfileEntry Bind(DatabaseDataReader reader)
		{
			AccessControlTimeProfileEntry accessControlTimeProfileEntry = new AccessControlTimeProfileEntry();
			accessControlTimeProfileEntry.ActionId = IDAction.GetString(reader, allowNulls: true);
			accessControlTimeProfileEntry.Sid = Sid.GetString(reader, allowNulls: false);
			string text = TimeProfileId.GetString(reader, allowNulls: true, Guid.Empty.ToString());
			if (!text.Equals(Guid.Empty.ToString()))
			{
				accessControlTimeProfileEntry.TimeProfileId = text.ToString();
			}
			accessControlTimeProfileEntry.IsDefaultTimeProfile = IsDefaultTimeProfile.GetBoolean(reader);
			return accessControlTimeProfileEntry;
		}
	}

	public class VmoTimeProfile
	{
		public DateTime LastModified { get; set; }

		public string TimeProfileId { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public string Definition { get; set; }

		public VideoOS.Common.SharedTimeProfile.TimeProfileType TimeProfileType { get; set; }
	}

	private static class VmoTimeProfileColumns
	{
		private static DatabaseColumnBinder IDTimeProfile = new DatabaseColumnBinder("pkid");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("name");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("description");

		private static DatabaseColumnBinder Definition = new DatabaseColumnBinder("xml_data");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("last_modified");

		private static DatabaseColumnBinder TimeProfileType = new DatabaseColumnBinder("type");

		public static VmoTimeProfile Bind(DatabaseDataReader reader)
		{
			return new VmoTimeProfile
			{
				TimeProfileId = IDTimeProfile.GetString(reader),
				Name = Name.GetString(reader),
				Description = Description.GetString(reader),
				Definition = Definition.GetString(reader),
				LastModified = LastModified.GetDateTime(reader),
				TimeProfileType = (VideoOS.Common.SharedTimeProfile.TimeProfileType)Enum.Parse(typeof(VideoOS.Common.SharedTimeProfile.TimeProfileType), TimeProfileType.GetString(reader), ignoreCase: true)
			};
		}
	}

	private class DefaultTimeProfile
	{
		public string TimeProfileId { get; set; }

		public DefaultTimeProfile()
		{
		}

		public DefaultTimeProfile(string timeProfileId)
		{
			TimeProfileId = timeProfileId;
		}
	}

	private static class DefaultTimeProfileColumns
	{
		private static DatabaseColumnBinder IDTimeProfile = new DatabaseColumnBinder("IDTimeProfile");

		public static DefaultTimeProfile Bind(DatabaseDataReader reader)
		{
			DefaultTimeProfile defaultTimeProfile = new DefaultTimeProfile();
			defaultTimeProfile.TimeProfileId = IDTimeProfile.GetString(reader, allowNulls: true);
			if (defaultTimeProfile.TimeProfileId != null && !Guid.TryParse(defaultTimeProfile.TimeProfileId, out var _))
			{
				return null;
			}
			return defaultTimeProfile;
		}
	}

	private static HashSet<Guid> _deviceSecurityNamespaces = new HashSet<Guid>
	{
		SecurityNamespaceIdentifier.Camera,
		SecurityNamespaceIdentifier.Microphone,
		SecurityNamespaceIdentifier.Speaker,
		SecurityNamespaceIdentifier.Metadata,
		SecurityNamespaceIdentifier.Input,
		SecurityNamespaceIdentifier.Output
	};

	public bool RegisterObject(Guid namespaceId, string objectId, string parentObjectId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_register_object");
			BindUniqueIdentifier("@namespace_id", namespaceId);
			BindString("@object_id", objectId, 260, allowNull: false, SqlDbType.NVarChar);
			BindString("@parent_object_id", parentObjectId, 260, allowNull: true, SqlDbType.NVarChar);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new SecurityDatabaseConfigMissingRowException();
			}
			bool result = UpdateSecurityColumns.Bind(databaseDataReader);
			if (databaseDataReader.Read() || databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public bool RegisterObjectWithName(Guid namespaceId, string objectId, string parentObjectId, string objectName)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_register_object");
			BindUniqueIdentifier("@namespace_id", namespaceId);
			BindString("@object_id", objectId, 260, allowNull: false, SqlDbType.NVarChar);
			BindString("@parent_object_id", parentObjectId, 260, allowNull: true, SqlDbType.NVarChar);
			BindString("@object_name", objectName, 100, allowNull: false, SqlDbType.NVarChar);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new SecurityDatabaseConfigMissingRowException();
			}
			bool result = UpdateSecurityColumns.Bind(databaseDataReader);
			if (databaseDataReader.Read() || databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public bool UnregisterObject(Guid namespaceId, string objectId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_unregister_object");
			BindUniqueIdentifier("@namespace_id", namespaceId);
			BindString("@object_id", objectId, 260, allowNull: false, SqlDbType.NVarChar);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new SecurityDatabaseConfigMissingRowException();
			}
			bool result = UpdateSecurityColumns.Bind(databaseDataReader);
			if (databaseDataReader.Read() || databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public ResultCollection QuerySecurityNamespaces(string namespaceId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_query_namespace");
			if (string.IsNullOrEmpty(namespaceId))
			{
				BindNullValue("@namespace_id", SqlDbType.UniqueIdentifier);
			}
			else
			{
				BindGuid("@namespace_id", new Guid(namespaceId));
			}
			ResultCollection resultCollection = new ResultCollection(ExecuteReader(), base.StoredProcedureName, RequestContext);
			resultCollection.AddBinder(new SecurityNamespaceBinder());
			resultCollection.AddBinder(new ActionDefinitionBinder());
			return resultCollection;
		});
	}

	public ResultCollection QuerySecurityAces(string namespaceId, bool getInheritanceInformation)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_query_aces");
			BindUniqueIdentifier("@namespace_id", namespaceId);
			BindBoolean("@get_inherit_information", getInheritanceInformation);
			ResultCollection resultCollection = new ResultCollection(ExecuteReader(), base.StoredProcedureName, RequestContext);
			resultCollection.AddBinder(new AccessControlEntry2Binder());
			if (getInheritanceInformation)
			{
				resultCollection.AddBinder(new NoInheritanceBinder());
			}
			return resultCollection;
		});
	}

	public void CreateSecurityNamespace(SecurityNamespaceInfo description, string orderAfterNamespaceId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_create_namespace");
			BindGuid("namespace_id", new Guid(description.NamespaceId));
			BindString("name", description.Name, 260, allowNull: false, SqlDbType.NVarChar);
			BindString("display_name", description.DisplayName, 260, allowNull: false, SqlDbType.NVarChar);
			BindString("separator", description.Separator.ToString(CultureInfo.InvariantCulture), 1, allowNull: false, SqlDbType.Char);
			BindInt("structure", description.Structure);
			BindGuid("order_after_namespace_id", new Guid(orderAfterNamespaceId));
			BindXml("@action_values", ConvertActionsToXml(description.Actions));
			ExecuteNonQuery();
		});
	}

	public void UpdateSecurityNamespace(SecurityNamespaceInfo description)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_update_namespace");
			BindGuid("namespace_id", new Guid(description.NamespaceId));
			BindString("name", description.Name, 260, allowNull: false, SqlDbType.NVarChar);
			BindString("display_name", description.DisplayName, 260, allowNull: false, SqlDbType.NVarChar);
			BindString("separator", description.Separator.ToString(CultureInfo.InvariantCulture), 1, allowNull: false, SqlDbType.Char);
			BindInt("structure", description.Structure);
			BindXml("action_values", ConvertActionsToXml(description.Actions));
			ExecuteNonQuery();
		});
	}

	public void DeleteSecurityNamespace(Guid namespaceId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_delete_namespace");
			BindGuid("namespace_id", namespaceId);
			ExecuteNonQuery();
		});
	}

	public ResultCollection QueryPermissions(Guid namespaceId, string securityToken)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_query_access_entries");
			BindUniqueIdentifier("@namespace_id", namespaceId);
			BindString("@object_id", securityToken, 300, allowNull: false, SqlDbType.NVarChar);
			ResultCollection resultCollection = new ResultCollection(ExecuteReader(), base.StoredProcedureName, RequestContext);
			resultCollection.AddBinder(new AccessControlEntryBinder());
			return resultCollection;
		});
	}

	public bool AddPermission(Guid namespaceId, string securityToken, string objectName, AccessControlEntry[] entries, bool replace, Guid sessionId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_create_access_entry");
			BindUniqueIdentifier("@namespace_id", namespaceId);
			BindString("@object_id", securityToken, 260, allowNull: false, SqlDbType.NVarChar);
			BindNameType("@object_Name", objectName, allowNull: false);
			BindBoolean("@replace", replace);
			BindGuidType("@session_id", sessionId.ToString());
			if (entries == null || entries.Length == 0)
			{
				BindNullValue("@entries", SqlDbType.Xml);
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder))
				{
					xmlWriter.WriteStartElement("AccessControlEntries");
					for (int i = 0; i < entries.Length; i++)
					{
						xmlWriter.WriteStartElement("AccessControlEntry");
						xmlWriter.WriteAttributeString("Sid", entries[i].Sid);
						xmlWriter.WriteAttributeString("ActionId", entries[i].ActionId);
						xmlWriter.WriteAttributeString("AccessDeny", entries[i].AccessDeny.ToString(CultureInfo.InvariantCulture));
						xmlWriter.WriteEndElement();
					}
					xmlWriter.WriteEndElement();
				}
				BindXml("@entries", stringBuilder.ToString());
			}
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new SecurityDatabaseConfigMissingRowException();
			}
			bool result = UpdateSecurityColumns.Bind(databaseDataReader);
			if (databaseDataReader.Read() || databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public bool RemovePermissions(Guid namespaceId, string securityToken, string objectName, AccessControlEntry[] entries, Guid sessionId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_delete_access_entry");
			BindUniqueIdentifier("@namespace_id", namespaceId);
			BindString("@object_id", securityToken, 260, allowNull: false, SqlDbType.NVarChar);
			BindNameType("@object_name", objectName, allowNull: false);
			BindGuidType("@session_id", sessionId.ToString());
			if (entries == null || entries.Length == 0)
			{
				BindNullValue("@entries", SqlDbType.Xml);
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder))
				{
					xmlWriter.WriteStartElement("AccessControlEntries");
					for (int i = 0; i < entries.Length; i++)
					{
						xmlWriter.WriteStartElement("AccessControlEntry");
						xmlWriter.WriteAttributeString("Sid", entries[i].Sid);
						xmlWriter.WriteAttributeString("ActionId", entries[i].ActionId);
						xmlWriter.WriteAttributeString("AccessDeny", entries[i].AccessDeny.ToString(CultureInfo.InvariantCulture));
						xmlWriter.WriteEndElement();
					}
					xmlWriter.WriteEndElement();
				}
				BindXml("@entries", stringBuilder.ToString());
			}
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new SecurityDatabaseConfigMissingRowException();
			}
			bool result = UpdateSecurityColumns.Bind(databaseDataReader);
			if (databaseDataReader.Read() || databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public bool AddMatrixPermission(string idMatrix, string objectName, AccessControlEntry[] entries, bool replace, Guid sessionId)
	{
		if (entries.Length == 0)
		{
			return false;
		}
		if (entries.Length > 1)
		{
			throw new ArgumentException("AddMatrixPermission only expects one AccessControlEntry");
		}
		AccessControlEntry ace = entries[0];
		if (ace.ActionId != "GENERIC_READ")
		{
			throw new ArgumentException("Matrix only supports GENERIC_READ security access");
		}
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_create_matrix_access_entry");
			BindString("@SecurityAtribute", "MATRIXVISIBLE", 100, allowNull: false, SqlDbType.NVarChar);
			BindGuidType("@IDRole", ace.Sid);
			BindGuidType("@IDMatrix", idMatrix);
			BindString("@Value", "True", 256, allowNull: false, SqlDbType.NVarChar);
			BindBoolean("@replace", replace);
			BindString("@MatrixName", objectName, 250, allowNull: false, SqlDbType.NVarChar);
			BindGuidType("@session_id", sessionId.ToString());
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new SecurityDatabaseConfigMissingRowException();
			}
			bool result = UpdateSecurityColumns.Bind(databaseDataReader);
			if (databaseDataReader.Read() || databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public bool RemoveMatrixPermissions(string idMatrix, string objectName, AccessControlEntry[] entries, Guid sessionId)
	{
		if (entries.Length == 0)
		{
			return false;
		}
		AccessControlEntry ace = entries[0];
		if (ace.ActionId != "GENERIC_READ")
		{
			throw new NotImplementedException("Matrix only supports GENERIC_READ security access");
		}
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_delete_matrix_access_entry");
			BindString("@SecurityAtribute", "MATRIXVISIBLE", 260, allowNull: false, SqlDbType.NVarChar);
			BindGuidType("@IDRole", ace.Sid);
			BindGuidType("@IDMatrix", idMatrix);
			BindString("@MatrixName", objectName, 250, allowNull: false, SqlDbType.NVarChar);
			BindGuidType("@session_id", sessionId.ToString());
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new SecurityDatabaseConfigMissingRowException();
			}
			bool result = UpdateSecurityColumns.Bind(databaseDataReader);
			if (databaseDataReader.Read() || databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public bool QueryMatrixPermission(string securityToken, string matrixId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_query_matrix_access_entry");
			BindString("@SecurityAtribute", "MATRIXVISIBLE", 260, allowNull: false, SqlDbType.NVarChar);
			BindGuidType("@IDRole", securityToken);
			BindGuidType("@IDMatrix", matrixId);
			return ExecuteReader().Read() ? true : false;
		});
	}

	public List<AccessControlDeviceTimeProfileEntry> QueryDeviceTimeProfile(string roleId, string objectId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_query_access_entries_device_time_profile");
			BindUniqueIdentifier("@role_id", roleId);
			BindString("@object_id", objectId.ToString(), 300, allowNull: false, SqlDbType.NVarChar);
			using ResultCollection resultCollection = new ResultCollection(ExecuteReader(), base.StoredProcedureName, RequestContext);
			resultCollection.AddBinder(new AccessControlEntryDeviceTimeProfileBinder());
			return resultCollection.GetCurrent<AccessControlDeviceTimeProfileEntry>().Items;
		});
	}

	public List<AccessControlDeviceTimeProfileEntry> QueryTimeProfiles(string roleId, HashSet<Guid> objectIds)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_query_access_entries_device_time_profiles");
			BindUniqueIdentifier("@role_id", roleId);
			BindIdTable("@object_ids", objectIds.Select((Guid id) => id.ToString()));
			using ResultCollection resultCollection = new ResultCollection(ExecuteReader(), base.StoredProcedureName, RequestContext);
			resultCollection.AddBinder(new AccessControlEntryDeviceTimeProfileBinder());
			return resultCollection.GetCurrent<AccessControlDeviceTimeProfileEntry>().Items;
		});
	}

	public List<AccessControlDeviceTimeProfileEntry> QueryTimeProfilesAllRoles(Guid objectId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_query_access_entries_device_time_profiles_all_roles");
			BindUniqueIdentifier("@object_id", objectId);
			using ResultCollection resultCollection = new ResultCollection(ExecuteReader(), base.StoredProcedureName, RequestContext);
			resultCollection.AddBinder(new AccessControlEntryDeviceTimeProfileBinder());
			return resultCollection.GetCurrent<AccessControlDeviceTimeProfileEntry>().Items;
		});
	}

	public List<AccessControlTimeProfileObjectEntry> QueryNonDeviceTimeProfiles(Guid namespaceId, string roleId, HashSet<Guid> objectIds)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_query_access_time_profile_entries");
			BindUniqueIdentifier("@namespace_id", namespaceId);
			BindUniqueIdentifier("@role_id", roleId);
			BindIdTable("@object_ids", objectIds.Select((Guid id) => id.ToString()));
			using ResultCollection resultCollection = new ResultCollection(ExecuteReader(), base.StoredProcedureName, RequestContext);
			resultCollection.AddBinder(new AccessControlTimeProfileObjectEntryBinder());
			return resultCollection.GetCurrent<AccessControlTimeProfileObjectEntry>().Items;
		});
	}

	public List<AccessControlTimeProfileObjectEntry> QueryNonDeviceTimeProfilesAllRoles(Guid namespaceId, Guid objectId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_query_access_time_profile_entries_all_roles");
			BindUniqueIdentifier("@namespace_id", namespaceId);
			BindString("@object_id", objectId.ToString(), 100, allowNull: false, SqlDbType.NVarChar);
			using ResultCollection resultCollection = new ResultCollection(ExecuteReader(), base.StoredProcedureName, RequestContext);
			resultCollection.AddBinder(new AccessControlTimeProfileObjectEntryRoleBinder());
			return resultCollection.GetCurrent<AccessControlTimeProfileObjectEntry>().Items;
		});
	}

	public void UpdateDeviceTimeProfile(AccessControlDeviceTimeProfileEntry entry, string deviceName, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_update_access_entry_device_time_profile");
			BindGuidType("@roleId", entry.RoleId);
			BindGuidType("@deviceId", entry.DeviceId);
			BindNameType("@deviceName", deviceName);
			BindString("@actionName", entry.ActionId, 100, allowNull: false, SqlDbType.NVarChar);
			BindString("@value", entry.Value, 256, allowNull: false, SqlDbType.NVarChar);
			if (string.IsNullOrEmpty(entry.TimeProfileId))
			{
				BindGuidType("@timeProfileId", (string)null, true);
			}
			else
			{
				BindGuidType("@timeProfileId", entry.TimeProfileId, allowNullOrEmpty: true);
			}
			BindInt("@timeBackward", entry.TimeBackward);
			BindBoolean("@isDefault", entry.IsDefaultTimeProfile);
			BindGuidType("@session_id", sessionId.ToString());
			ExecuteNonQuery();
		});
	}

	public void SetInheritance(Guid namespaceId, string securityToken, string parentObjectId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_set_inheritance");
			BindUniqueIdentifier("@namespace_id", namespaceId);
			BindString("@object_id", securityToken, 260, allowNull: false, SqlDbType.NVarChar);
			BindString("@parent_object_id", parentObjectId, 260, allowNull: true, SqlDbType.NVarChar);
			ExecuteNonQuery();
		});
	}

	public bool HasPermission(Guid namespaceId, string userSid, string actionId, string securityToken, string legacySecurityArea, string legacySecurityAttributeId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_is_permitted");
			BindUniqueIdentifier("namespace_id", namespaceId);
			BindSid("@user_sid", userSid);
			BindString("@action_name", actionId, 260, allowNull: false, SqlDbType.NVarChar);
			BindString("@object_id", securityToken, 300, allowNull: false, SqlDbType.NVarChar);
			BindString("@legacy_security_area", legacySecurityArea, 260, allowNull: true, SqlDbType.NVarChar);
			BindString("@legacy_security_attribute", legacySecurityAttributeId, 260, allowNull: true, SqlDbType.NVarChar);
			return (bool)ExecuteScalar();
		});
	}

	public string GetSecurityNamespaceIdFromToken(string securityToken)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_query_namespace_from_token");
			BindString("@object_id", securityToken, 260, allowNull: false, SqlDbType.NVarChar);
			return ExecuteScalar()?.ToString();
		});
	}

	public ResultCollection QueryPrivateViewGroupTokens()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_query_private_viewgroup_tokens");
			ResultCollection resultCollection = new ResultCollection(ExecuteReader(), base.StoredProcedureName, RequestContext);
			resultCollection.AddBinder(new ObjectTokenBinder());
			return resultCollection;
		});
	}

	public string[] QuerySecurityObjects(Guid namespaceId, bool userDefined)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_query_objectids");
			BindUniqueIdentifier("@namespace_id", namespaceId);
			BindInt("@userDefined", userDefined ? 1 : 0);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			while (databaseDataReader.Read())
			{
				list.Add(QuerySecurityObject.Bind(databaseDataReader));
			}
			return list.ToArray();
		});
	}

	public List<Tuple<string, string>> QuerySecurityObjectMaps(Guid namespaceId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_query_objectid_maps");
			BindUniqueIdentifier("@namespace_id", namespaceId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<Tuple<string, string>> list = new List<Tuple<string, string>>();
			while (databaseDataReader.Read())
			{
				list.Add(QuerySecurityObjectMap.Bind(databaseDataReader));
			}
			return list;
		});
	}

	public RolePermissionObjectEntry[] QueryRolePermissionsForObjects(Guid namespaceId, string roleId, HashSet<Guid> objectIds)
	{
		if (namespaceId == SecurityNamespaceIdentifier.Matrix)
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				PrepareStoredProcedure("security_query_matrix_role_access_entries");
				BindGuidType("@IDRole", roleId);
				DatabaseDataReader databaseDataReader = ExecuteReader();
				List<RolePermissionObjectEntry> list = new List<RolePermissionObjectEntry>();
				while (databaseDataReader.Read())
				{
					list.Add(QueryMatrixPermissionEntry.Bind(databaseDataReader));
				}
				return list.ToArray();
			});
		}
		if (_deviceSecurityNamespaces.Contains(namespaceId))
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				PrepareStoredProcedure("security_query_access_entries_for_objectids");
				BindUniqueIdentifier("@namespace_id", namespaceId);
				BindGuidType("@role_id", roleId);
				BindIdTable("@object_ids", objectIds.Select((Guid id) => id.ToString()));
				DatabaseDataReader databaseDataReader = ExecuteReader();
				List<RolePermissionObjectEntry> list = new List<RolePermissionObjectEntry>();
				while (databaseDataReader.Read())
				{
					list.Add(QueryRolePermissionEntry.Bind(databaseDataReader));
				}
				return list.ToArray();
			});
		}
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_query_non_device_role_access_entries");
			BindUniqueIdentifier("@namespace_id", namespaceId);
			BindGuidType("@role_id", roleId);
			BindIdTable("@object_ids", objectIds.Select((Guid id) => id.ToString()));
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<RolePermissionObjectEntry> list = new List<RolePermissionObjectEntry>();
			while (databaseDataReader.Read())
			{
				list.Add(QueryRolePermissionEntry.Bind(databaseDataReader));
			}
			return list.ToArray();
		});
	}

	public RolePermissionObjectEntry[] QueryPermissionsForObject(Guid namespaceId, Guid objectId)
	{
		if (namespaceId == SecurityNamespaceIdentifier.Matrix)
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				PrepareStoredProcedure("security_query_matrix_access_entry_all_roles");
				BindGuidType("@IDMatrix", objectId);
				DatabaseDataReader databaseDataReader = ExecuteReader();
				List<RolePermissionObjectEntry> list = new List<RolePermissionObjectEntry>();
				while (databaseDataReader.Read())
				{
					list.Add(QueryMatrixRolePermissionEntry.Bind(databaseDataReader));
				}
				return list.ToArray();
			});
		}
		if (_deviceSecurityNamespaces.Contains(namespaceId))
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				PrepareStoredProcedure("security_query_access_entries_for_objectid_all_roles");
				BindUniqueIdentifier("@namespace_id", namespaceId);
				BindGuidType("@object_id", objectId);
				DatabaseDataReader databaseDataReader = ExecuteReader();
				List<RolePermissionObjectEntry> list = new List<RolePermissionObjectEntry>();
				while (databaseDataReader.Read())
				{
					list.Add(QueryRolePermissionEntryAll.Bind(databaseDataReader));
				}
				return list.ToArray();
			});
		}
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_query_non_device_role_access_entries_all_roles");
			BindUniqueIdentifier("@namespace_id", namespaceId);
			BindGuidType("@object_id", objectId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<RolePermissionObjectEntry> list = new List<RolePermissionObjectEntry>();
			while (databaseDataReader.Read())
			{
				list.Add(QueryRolePermissionEntryAll.Bind(databaseDataReader));
			}
			return list.ToArray();
		});
	}

	public List<string> VerifySecurityObjects(Guid namespaceId, HashSet<Guid> objectIds)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_verify_objectIds");
			BindUniqueIdentifier("@namespace_id", namespaceId);
			BindIdTable("@object_ids", objectIds.Select((Guid id) => id.ToString()));
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			while (databaseDataReader.Read())
			{
				list.Add(databaseDataReader.GetString(0));
			}
			return list;
		});
	}

	public void RemoveUser(string userSid)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_remove_identity");
			BindSid("@user_sid", userSid);
			ExecuteNonQuery();
		});
	}

	private static string ConvertActionsToXml(List<ActionDefinition> actions)
	{
		StringBuilder stringBuilder = new StringBuilder();
		using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder))
		{
			xmlWriter.WriteStartElement("actions");
			for (int i = 0; i < actions.Count; i++)
			{
				xmlWriter.WriteStartElement("action");
				xmlWriter.WriteAttributeString("name", actions[i].Name);
				xmlWriter.WriteAttributeString("display_name", actions[i].DisplayName);
				xmlWriter.WriteAttributeString("visible", actions[i].Visible ? "1" : "0");
				xmlWriter.WriteEndElement();
			}
			xmlWriter.WriteEndElement();
		}
		return stringBuilder.ToString();
	}

	public bool TimeBasedSecurityIsPermitted(Guid namespaceId, string objectId, string actionId, AccessControlEntry[] accessControlEntry)
	{
		List<Guid> list = new List<Guid>();
		foreach (AccessControlEntry accessControlEntry2 in accessControlEntry)
		{
			AccessControlTimeProfile accessControlTimeProfile = QueryAccessTimeProfileEntryIsPermitted(namespaceId, objectId, accessControlEntry2.Sid, actionId);
			if (accessControlTimeProfile == null)
			{
				return true;
			}
			if (accessControlTimeProfile.IsDefaultTimeProfile)
			{
				if (!Guid.TryParse(accessControlEntry2.Sid, out var _))
				{
					continue;
				}
				DefaultTimeProfile defaultTimeProfile = QueryDefaultTimeProfile(new Guid(accessControlEntry2.Sid));
				if (defaultTimeProfile != null)
				{
					if (defaultTimeProfile.TimeProfileId == null)
					{
						return true;
					}
					list.Add(new Guid(defaultTimeProfile.TimeProfileId));
				}
			}
			else
			{
				if (accessControlTimeProfile.TimeProfileId == null)
				{
					return true;
				}
				list.Add(new Guid(accessControlTimeProfile.TimeProfileId));
			}
		}
		if (list.Count == 0)
		{
			return false;
		}
		List<Guid> list2 = new List<Guid>();
		DateTime utcNow = DateTime.UtcNow;
		foreach (Guid item in list)
		{
			if (list2.Contains(item))
			{
				continue;
			}
			VmoTimeProfile vmoTimeProfile = QueryTimeProfile(item);
			if (vmoTimeProfile != null)
			{
				if (TimeProfileManager.EvaluateTimeProfile(vmoTimeProfile.Definition, vmoTimeProfile.TimeProfileType, utcNow))
				{
					return true;
				}
				list2.Add(item);
			}
		}
		return false;
	}

	public AccessControlTimeProfile QueryAccessTimeProfileEntryIsPermitted(Guid namespaceId, string objectId, string userSid, string actionId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_query_access_time_profile_entry");
			BindUniqueIdentifier("@namespace_id", namespaceId);
			BindString("@object_id", objectId, 300, allowNull: false, SqlDbType.NVarChar);
			BindString("@action_name", actionId, 260, allowNull: false, SqlDbType.NVarChar);
			BindSid("@user_sid", userSid);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			AccessControlTimeProfile accessControlTimeProfile = new AccessControlTimeProfile();
			if (!databaseDataReader.Read())
			{
				return (AccessControlTimeProfile)null;
			}
			accessControlTimeProfile = AccessControlTimeProfileColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return accessControlTimeProfile;
		});
	}

	public bool TimeBasedSecurityIsPermitted(Guid namespaceId, string objectId, string roleId, string actionId, DateTime evaluationTime, out string timeProfileId)
	{
		timeProfileId = null;
		if (GetLegacyApplicationTimeProfile(roleId, actionId, out timeProfileId))
		{
			if (timeProfileId == null)
			{
				return true;
			}
		}
		else
		{
			AccessControlTimeProfile accessControlTimeProfile = QueryAccessTimeProfileEntryIsPermitted(namespaceId, objectId, roleId, actionId);
			if (accessControlTimeProfile == null)
			{
				return false;
			}
			if (accessControlTimeProfile.IsDefaultTimeProfile)
			{
				if (!Guid.TryParse(roleId, out var result))
				{
					return false;
				}
				DefaultTimeProfile defaultTimeProfile = QueryDefaultTimeProfile(result);
				if (defaultTimeProfile == null)
				{
					return false;
				}
				if (defaultTimeProfile.TimeProfileId == null)
				{
					return true;
				}
				timeProfileId = defaultTimeProfile.TimeProfileId;
			}
			else
			{
				if (accessControlTimeProfile.TimeProfileId == null)
				{
					return true;
				}
				timeProfileId = accessControlTimeProfile.TimeProfileId;
			}
		}
		VmoTimeProfile vmoTimeProfile = QueryTimeProfile(new Guid(timeProfileId));
		if (vmoTimeProfile == null)
		{
			return false;
		}
		if (TimeProfileManager.EvaluateTimeProfile(vmoTimeProfile.Definition, vmoTimeProfile.TimeProfileType, evaluationTime))
		{
			return true;
		}
		return false;
	}

	private bool GetLegacyApplicationTimeProfile(string roleId, string attributeName, out string timeProfileId)
	{
		bool result;
		(result, timeProfileId) = base.Connection.ExecuteRetryable(delegate
		{
			string item = null;
			PrepareStoredProcedure("security_legacy_GetApplicationTimeProfile");
			BindGuidType("@IDRole", roleId);
			BindNameType("@IDSecurityAttribute", attributeName);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (false, timeProfileIdResult: item);
			}
			if (databaseDataReader.IsDBNull(0))
			{
				return (true, timeProfileIdResult: item);
			}
			item = new DatabaseColumnBinder("IDTimeProfile").GetString(databaseDataReader);
			if (databaseDataReader.Read())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return (true, timeProfileIdResult: item);
		});
		return result;
	}

	public AccessControlTimeProfileEntry QueryAccessTimeProfileEntry(Guid namespaceId, string objectId, string identity, string actionId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_query_access_time_profile_entry");
			BindUniqueIdentifier("@namespace_id", namespaceId);
			BindString("@object_id", objectId, 300, allowNull: false, SqlDbType.NVarChar);
			BindString("@action_name", actionId, 260, allowNull: false, SqlDbType.NVarChar);
			BindSid("@user_sid", identity);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			AccessControlTimeProfileEntry result = new AccessControlTimeProfileEntry();
			if (databaseDataReader.Read())
			{
				result = AccessControlTimeProfileEntryColumns.Bind(databaseDataReader);
			}
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public void CreateUpdateAccessTimeProfileEntry(Guid namespaceId, string objectId, string objectName, string identitySid, string actionId, string timeProfileId, bool isDefaultTimeProfile, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_create_update_access_time_profile_entry");
			BindUniqueIdentifier("@namespace_id", namespaceId);
			BindString("@object_id", objectId, 300, allowNull: false, SqlDbType.NVarChar);
			BindNameType("@object_name", objectName, allowNull: false);
			BindString("@action_name", actionId, 260, allowNull: false, SqlDbType.NVarChar);
			BindSid("@user_sid", identitySid);
			BindGuidType("@timeprofile_id", timeProfileId, allowNullOrEmpty: true);
			BindBoolean("@is_default", isDefaultTimeProfile);
			BindGuidType("@session_id", sessionId.ToString());
			ExecuteNonQuery();
		});
	}

	public void DeleteAccessTimeProfileEntry(Guid namespaceId, string objectId, string objectName, string identitySid, string actionId, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_delete_access_time_profile_entry");
			BindUniqueIdentifier("@namespace_id", namespaceId);
			BindString("@object_id", objectId, 300, allowNull: false, SqlDbType.NVarChar);
			BindNameType("@object_name", objectName, allowNull: false);
			BindString("@action_name", actionId, 260, allowNull: false, SqlDbType.NVarChar);
			BindSid("@user_sid", identitySid);
			BindGuidType("@session_id", sessionId.ToString());
			ExecuteNonQuery();
		});
	}

	public VmoTimeProfile QueryTimeProfile(Guid timeProfileId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("time_based_security_query_time_profile");
			BindObjectId("@pkid", timeProfileId.ToString());
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (VmoTimeProfile)null;
			}
			VmoTimeProfile result = VmoTimeProfileColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	private DefaultTimeProfile QueryDefaultTimeProfile(Guid roleId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_default_time_profile_for_role");
			BindString("@IDRole", roleId.ToString(), 36, allowNull: false, SqlDbType.Char);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return new DefaultTimeProfile(null);
			}
			DefaultTimeProfile result = DefaultTimeProfileColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return result;
		});
	}
}
