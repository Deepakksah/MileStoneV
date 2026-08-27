using System;
using System.Collections;

namespace VideoOS.Common.Integration.Configuration;

public class NotificationTypes : DictionaryBase
{
	public static class WellKnownIds
	{
		public static readonly Guid Email = new Guid("74E649D7-D6F6-47DB-8828-9A6CCFE95204");
	}

	private ConfigurationManager _configurationManager;

	private ConfigurationDataSet.NotificationTypesDataTable _table;

	public ICollection Keys => base.Dictionary.Keys;

	public ICollection Values => base.Dictionary.Values;

	public NotificationType this[Guid notificationTypeId] => (NotificationType)base.Dictionary[notificationTypeId];

	public NotificationTypes(ConfigurationManager manager)
	{
		if (manager == null)
		{
			throw new ArgumentNullException("manager");
		}
		_configurationManager = manager;
		_table = _configurationManager.Dataset.NotificationTypes;
		_configurationManager.EnsureTableDataLoaded(_table);
		foreach (ConfigurationDataSet.NotificationTypesRow item in _table)
		{
			Add(item);
		}
	}

	public NotificationType Add(ConfigurationDataSet.NotificationTypesRow row)
	{
		NotificationType notificationType = new NotificationType(_configurationManager, row);
		base.Dictionary.Add(notificationType.IDNotificationType, notificationType);
		return notificationType;
	}

	public NotificationType Add(Guid notificationTypeId, string name, string pageName)
	{
		NotificationType notificationType = new NotificationType(_configurationManager, _table.AddNotificationTypesRow(DateTime.UtcNow, notificationTypeId.ToString(), name, pageName));
		base.Dictionary.Add(notificationType.IDNotificationType, notificationType);
		return notificationType;
	}
}
