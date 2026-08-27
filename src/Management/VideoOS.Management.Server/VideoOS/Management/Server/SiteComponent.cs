using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Xml;
using VideoOS.Management.Common;
using VideoOS.Management.VmoClient;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class SiteComponent : GeminiResourceComponent, ISiteComponent, IDisposable
{
	private static class SiteColumns
	{
		private static DatabaseColumnBinder IDSite = new DatabaseColumnBinder("IDSite");

		private static DatabaseColumnBinder IDSiteType = new DatabaseColumnBinder("IDSiteType");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("Description");

		private static DatabaseColumnBinder IDSiteState = new DatabaseColumnBinder("IDSiteState");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		private static DatabaseColumnBinder LastStatusHandshake = new DatabaseColumnBinder("LastStatusHandshake");

		private static DatabaseColumnBinder Addresses = new DatabaseColumnBinder("Addresses");

		private static DatabaseColumnBinder MarkedInconsistent = new DatabaseColumnBinder("MarkInconsistent");

		private static DatabaseColumnBinder IDSiteMaster = new DatabaseColumnBinder("IDSiteMaster");

		private static DatabaseColumnBinder Version = new DatabaseColumnBinder("Version");

		private static DatabaseColumnBinder ServiceAccount = new DatabaseColumnBinder("ServiceAccount");

		private static DatabaseColumnBinder SynchronizationStatus = new DatabaseColumnBinder("SynchronizationStatus");

		private static DatabaseColumnBinder SiteRelations = new DatabaseColumnBinder("SlaveSiteRelations");

		public static Site Bind(DatabaseDataReader reader)
		{
			Site site = new Site();
			site.Id = IDSite.GetString(reader);
			site.SiteType = (SiteType)IDSiteType.GetInt32(reader);
			site.Name = Name.GetString(reader);
			site.Description = Description.GetString(reader);
			site.ConnectionState = (SiteState)IDSiteState.GetInt32(reader);
			site.LastModified = LastModified.GetDateTime(reader);
			site.LastStatusHandshake = LastStatusHandshake.GetDateTime(reader);
			string serverAddressXml = Addresses.GetString(reader);
			site.Addresses = ServerAddresses.GetFromXml(serverAddressXml).ToArray();
			site.MarkedInconsistent = MarkedInconsistent.GetDateTime(reader, allowNulls: true, DateTime.MinValue);
			site.MasterSiteId = IDSiteMaster.GetString(reader, allowNulls: true);
			if (site.SiteType == SiteType.Home)
			{
				site.Version = GeminiApplicationCore.ServerVersion().ToString(4);
			}
			else
			{
				string text = Version.GetString(reader);
				if (string.IsNullOrEmpty(text.Trim()))
				{
					site.Version = new Version(0, 0, 0, 0).ToString(4);
				}
				else
				{
					site.Version = text;
				}
			}
			site.ServiceAccount = ServiceAccount.GetString(reader);
			site.SynchronizationStatus = SynchronizationStatus.GetInt32(reader);
			string text2 = SiteRelations.GetString(reader, allowNulls: true);
			if (!string.IsNullOrEmpty(text2))
			{
				site.RelatedSlaveSiteIds = text2.Split(';');
			}
			else
			{
				site.RelatedSlaveSiteIds = new string[0];
			}
			return site;
		}
	}

	public Site[] QuerySlaveSites()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_query_all_slave_sites");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<Site> list = new List<Site>();
			while (databaseDataReader.Read())
			{
				list.Add(SiteColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public Site[] QuerySlaveSites(string siteId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_query_slave_sites");
			BindGuidType("@IDSite", siteId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<Site> list = new List<Site>();
			while (databaseDataReader.Read())
			{
				list.Add(SiteColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public string[] QuerySlaveSiteIds(string siteId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_query_slave_site_ids");
			BindGuidType("@IDSite", siteId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("IDSite");
			while (databaseDataReader.Read())
			{
				list.Add(databaseColumnBinder.GetString(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public Site[] QuerySlaveSitesByIds(string[] siteIds)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_query_slave_sites_by_ids");
			BindIdTable("@IDSites", siteIds);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<Site> list = new List<Site>();
			while (databaseDataReader.Read())
			{
				list.Add(SiteColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public Site QuerySite(string siteId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_query_site");
			BindGuidType("@IDSite", siteId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new SecurityDatabaseConfigMissingRowException();
			}
			Site result = SiteColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public Site QueryHomeSite()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_query_home_site");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new SecurityDatabaseConfigMissingRowException();
			}
			Site result = SiteColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new SiteHomeNotUniqueException();
			}
			return result;
		});
	}

	public Site QueryMasterSite()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_query_master_site");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (Site)null;
			}
			Site result = SiteColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new SiteHomeNotUniqueException();
			}
			return result;
		});
	}

	public void AttachMasterSite(Site site)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("site_attach_master_site");
			BindGuidType("@IDSite", site.Id);
			BindNameType("@Name", site.Name);
			BindDescription("@Description", site.Description);
			BindXml("@Addresses", ServerAddresses.GetAsXml(site.Addresses));
			BindString("@Version", site.Version, 15, allowNull: false, SqlDbType.NVarChar);
			BindString("@ServiceAccount", site.ServiceAccount, 256, allowNull: false, SqlDbType.NVarChar);
			BindInt("@SynchronizationStatus", site.SynchronizationStatus);
			ExecuteNonQuery();
		});
	}

	public void AttachSlaveSite(Site site)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("site_attach_slave_site");
			BindGuidType("@IDSite", site.Id);
			BindNameType("@Name", site.Name);
			BindDescription("@Description", site.Description);
			BindXml("@Addresses", ServerAddresses.GetAsXml(site.Addresses));
			BindString("@Version", site.Version, 15, allowNull: false, SqlDbType.NVarChar);
			BindString("@ServiceAccount", site.ServiceAccount, 256, allowNull: false, SqlDbType.NVarChar);
			BindInt("@SynchronizationStatus", site.SynchronizationStatus);
			ExecuteNonQuery();
		});
	}

	public void UpdateHomeSite(string name, string description, ServerAddress[] addresses, string version, string serviceAccount, int? synchronizationStatus, CustomProperty[] customProperties)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_update_home_site");
			BindNameType("@Name", name, allowNull: true);
			BindDescription("@Description", description, allowNull: true);
			BindServerAddresses("@Addresses", addresses, allowNull: true);
			BindString("@Version", version, 15, allowNull: true, SqlDbType.NVarChar);
			BindString("@ServiceAccount", serviceAccount, 256, allowNull: true, SqlDbType.NVarChar);
			BindInt("@SynchronizationStatus", synchronizationStatus);
			BindCustomProperties("@custom_properties", customProperties);
			ExecuteNonQuery();
		});
	}

	private DatabaseParameter BindServerAddresses(string parameterName, ServerAddress[] addresses, bool allowNull)
	{
		string parameterValue = null;
		if (addresses != null)
		{
			parameterValue = ServerAddresses.GetAsXml(addresses);
		}
		return BindXml(parameterName, parameterValue, allowNull);
	}

	public void DeleteSite(string siteId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_site");
			BindGuidType("@IDSite", siteId);
			ExecuteNonQuery();
		});
	}

	public void MarkInconsistent(string siteId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_site_mark_inconsistent");
			BindGuidType("@IDSite", siteId);
			ExecuteNonQuery();
		});
	}

	public void AuthorizeMaster()
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_authorize_master");
			ExecuteNonQuery();
		});
	}

	public void UpdateSiteState(string siteId, SiteState siteState)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_update_site_state");
			BindGuidType("@IDSite", siteId);
			BindInt("@IDSiteState", (int)siteState);
			ExecuteNonQuery();
		});
	}

	public void UpdateSiteHandshakeAndSyncStatus(Site site)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_update_site_handshake_and_sync_status");
			BindGuidType("@IDSite", site.Id);
			BindDateTime("@LastStatusHandshake", site.LastStatusHandshake);
			BindInt("@SynchronizationStatus", site.SynchronizationStatus);
			ExecuteNonQuery();
		});
	}

	public void UpdateSiteVersion(Site site)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_update_site_version");
			BindGuidType("@IDSite", site.Id);
			BindString("@Version", site.Version, 15, allowNull: false, SqlDbType.NVarChar);
			ExecuteNonQuery();
		});
	}

	public void UpdateSiteServiceAccount(Site site)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_update_site_service_account");
			BindGuidType("@IDSite", site.Id);
			BindString("@ServiceAccount", site.ServiceAccount, 256, allowNull: false, SqlDbType.NVarChar);
			ExecuteNonQuery();
		});
	}

	public void UpdateSiteSyncStatus(Site site)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_update_site_sync_status");
			BindGuidType("@IDSite", site.Id);
			BindInt("@SynchronizationStatus", site.SynchronizationStatus);
			ExecuteNonQuery();
		});
	}

	public void SyncSite(Site site)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_update_site_transaction");
			BindGuidType("@IDSite", site.Id);
			BindNameType("@Name", site.Name);
			BindDescription("@Description", site.Description);
			BindXml("@Addresses", ServerAddresses.GetAsXml(site.Addresses));
			BindDateTime("@LastModified", site.LastModified);
			BindString("@Version", site.Version, 15, allowNull: false, SqlDbType.NVarChar);
			BindString("@ServiceAccount", site.ServiceAccount, 256, allowNull: false, SqlDbType.NVarChar);
			BindInt("@SynchronizationStatus", site.SynchronizationStatus);
			ExecuteNonQuery();
		});
	}

	public Site[] QuerySlaveSitesComplete(string siteId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_query_slave_sites_complete");
			BindGuidType("@IDSite", siteId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<Site> list = new List<Site>();
			while (databaseDataReader.Read())
			{
				list.Add(SiteColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public void SyncronizeSlaveTree(Site slaveSite, IVmoClient slaveServer)
	{
		try
		{
			XmlDocument sitesXmlDoc = new XmlDocument();
			sitesXmlDoc.LoadXml("<Sites></Sites>");
			bool flag = false;
			List<string> list;
			do
			{
				flag = false;
				slaveServer.ManagementServer.ChildSites.Refresh(refreshChildObjects: false);
				list = new List<string>();
				Site site = SerializeVmoClientSite(slaveServer.ManagementServer, 1, slaveSite.MasterSiteId, slaveSite.LastStatusHandshake, 3, slaveSite.Version, slaveSite.ServiceAccount, slaveSite.SynchronizationStatus);
				CreateSiteXmlNode(sitesXmlDoc, site);
				list.Add(site.Id);
			}
			while (CreateSlaveChildren(slaveServer.ManagementServer.ChildSites, sitesXmlDoc, list));
			base.Connection.ExecuteRetryable(delegate
			{
				PrepareStoredProcedure("site_syncronize_slave_site");
				BindXml("@SiteXmlDefinition", sitesXmlDoc.InnerXml);
				ExecuteNonQuery();
			});
		}
		catch
		{
		}
	}

	private bool CreateSlaveChildren(ChildSiteCollection childSites, XmlDocument sitesXmlDoc, List<string> processedSites)
	{
		if (childSites == null)
		{
			return false;
		}
		ChildSite[] array = childSites.ToArray();
		foreach (ChildSite childSite in array)
		{
			string text = null;
			Site site = SerializeVmoClientSite(masterSiteId: (childSite.ParentSite == null) ? childSite.ManagementServer.Id.ToString().ToUpper(CultureInfo.InvariantCulture) : childSite.ParentSite.Id.ToString().ToUpper(CultureInfo.InvariantCulture), vmoSite: childSite, connectionState: (int)childSite.ConnectionState, lastStatusHandshake: childSite.LastConfirmedHandshake, siteType: 3, version: childSite.Version.ToString(), serviceAccount: childSite.ServiceAccount, synchronizationStatus: childSite.SynchronizationStatus);
			CreateSiteXmlNode(sitesXmlDoc, site);
			if (processedSites.Contains(site.Id))
			{
				return true;
			}
			processedSites.Add(site.Id);
			if (CreateSlaveChildren(childSite.ChildSites, sitesXmlDoc, processedSites))
			{
				return true;
			}
		}
		return false;
	}

	private static void CreateSiteXmlNode(XmlDocument sitesXml, Site slaveSite)
	{
		XmlNode xmlNode = sitesXml.CreateNode(XmlNodeType.Element, "Site", null);
		XmlNode xmlNode2 = sitesXml.CreateNode(XmlNodeType.Element, "SiteId", null);
		xmlNode2.InnerText = slaveSite.Id;
		xmlNode.AppendChild(xmlNode2);
		XmlNode xmlNode3 = sitesXml.CreateNode(XmlNodeType.Element, "MasterSiteId", null);
		xmlNode3.InnerText = slaveSite.MasterSiteId;
		xmlNode.AppendChild(xmlNode3);
		XmlNode xmlNode4 = sitesXml.CreateNode(XmlNodeType.Element, "Name", null);
		xmlNode4.InnerText = slaveSite.Name;
		xmlNode.AppendChild(xmlNode4);
		XmlNode xmlNode5 = sitesXml.CreateNode(XmlNodeType.Element, "Description", null);
		xmlNode5.InnerText = slaveSite.Description;
		xmlNode.AppendChild(xmlNode5);
		XmlNode xmlNode6 = sitesXml.CreateNode(XmlNodeType.Element, "Addresses", null);
		xmlNode6.InnerText = ServerAddresses.GetAsXml(slaveSite.Addresses);
		xmlNode.AppendChild(xmlNode6);
		XmlNode xmlNode7 = sitesXml.CreateNode(XmlNodeType.Element, "LastModified", null);
		xmlNode7.InnerText = slaveSite.LastModified.ToString("s", CultureInfo.InvariantCulture);
		xmlNode.AppendChild(xmlNode7);
		XmlNode xmlNode8 = sitesXml.CreateNode(XmlNodeType.Element, "LastConfirmedHandshake", null);
		xmlNode8.InnerText = slaveSite.LastStatusHandshake.ToString("s", CultureInfo.InvariantCulture);
		xmlNode.AppendChild(xmlNode8);
		XmlNode xmlNode9 = sitesXml.CreateNode(XmlNodeType.Element, "SiteState", null);
		xmlNode9.InnerText = ((int)slaveSite.ConnectionState).ToString(CultureInfo.InvariantCulture);
		xmlNode.AppendChild(xmlNode9);
		XmlNode xmlNode10 = sitesXml.CreateNode(XmlNodeType.Element, "Version", null);
		xmlNode10.InnerText = slaveSite.Version;
		xmlNode.AppendChild(xmlNode10);
		XmlNode xmlNode11 = sitesXml.CreateNode(XmlNodeType.Element, "ServiceAccount", null);
		xmlNode11.InnerText = slaveSite.ServiceAccount;
		xmlNode.AppendChild(xmlNode11);
		XmlNode xmlNode12 = sitesXml.CreateNode(XmlNodeType.Element, "SynchronizationStatus", null);
		xmlNode12.InnerText = slaveSite.SynchronizationStatus.ToString(CultureInfo.InvariantCulture);
		xmlNode.AppendChild(xmlNode12);
		sitesXml.DocumentElement.AppendChild(xmlNode);
	}

	public static Site SerializeVmoClientSite(ISite vmoSite, int connectionState, string masterSiteId, DateTime lastStatusHandshake, int siteType, string version, string serviceAccount, int synchronizationStatus)
	{
		Site site = new Site();
		site.Id = vmoSite.Id.ToString().ToUpper(CultureInfo.InvariantCulture);
		site.Name = vmoSite.Name;
		site.Description = vmoSite.Description;
		List<ServerAddress> list = new List<ServerAddress>();
		foreach (ReadOnlyAddress address in vmoSite.Addresses)
		{
			ServerAddress serverAddress = new ServerAddress();
			serverAddress.URL = address.ServerUrl.OriginalString;
			serverAddress.Type = (ServerAddressType)address.AddressType;
			list.Add(serverAddress);
		}
		site.Addresses = list.ToArray();
		site.LastModified = vmoSite.LastModified;
		site.ConnectionState = (SiteState)connectionState;
		site.MasterSiteId = masterSiteId;
		site.LastStatusHandshake = lastStatusHandshake;
		site.SiteType = (SiteType)siteType;
		site.Version = version;
		site.ServiceAccount = serviceAccount;
		site.SynchronizationStatus = synchronizationStatus;
		return site;
	}
}
