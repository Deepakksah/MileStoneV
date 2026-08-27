using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace VideoOS.Common.Integration.Configuration;

public class DeviceLinks : IEnumerable<DeviceLink>, IEnumerable
{
	private class DeviceLinkEntries : LinkedListWithIndexesBase<DeviceLink>
	{
		private LinkedListIndex<string, DeviceLink> indexOnKey;

		private LinkedListIndex<Guid, DeviceLink> indexOnDeviceId;

		private LinkedListIndex<Guid, DeviceLink> indexOnLinkedDeviceId;

		public DeviceLinkEntries()
		{
			indexOnKey = new LinkedListIndex<string, DeviceLink>((DeviceLink d) => d.ID);
			indexOnDeviceId = new LinkedListIndex<Guid, DeviceLink>((DeviceLink d) => d.IDDevice);
			indexOnLinkedDeviceId = new LinkedListIndex<Guid, DeviceLink>((DeviceLink d) => d.IDDeviceLink);
		}

		public DeviceLink QueryByKey(string key)
		{
			return (from e in indexOnKey.GetItemsByKey(key)
				select e.Value).FirstOrDefault();
		}

		public IEnumerable<DeviceLink> QueryByDeviceId(Guid deviceId)
		{
			return from e in indexOnDeviceId.GetItemsByKey(deviceId)
				select e.Value;
		}

		public IEnumerable<DeviceLink> QueryByLinkedDeviceId(Guid linkedDeviceId)
		{
			return from e in indexOnLinkedDeviceId.GetItemsByKey(linkedDeviceId)
				select e.Value;
		}

		protected override LinkedListNode<DeviceLink> FindEntry(DeviceLink item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			return indexOnKey.GetItemsByKey(item.ID).FirstOrDefault();
		}

		protected override void AddEntryToIndexes(LinkedListNode<DeviceLink> entry)
		{
			indexOnKey.AddItem(entry);
			indexOnDeviceId.AddItem(entry);
			indexOnLinkedDeviceId.AddItem(entry);
		}

		protected override void RemoveEntryFromIndexes(LinkedListNode<DeviceLink> entry)
		{
			indexOnKey.RemoveItem(entry);
			indexOnDeviceId.RemoveItem(entry);
			indexOnLinkedDeviceId.RemoveItem(entry);
		}
	}

	private ConfigurationManager configurationManager;

	private DataTable table;

	private bool modified;

	private List<DeviceLink> deletedItems;

	private DeviceLinkEntries items;

	public DeviceLinks(ConfigurationManager manager)
	{
		if (manager == null)
		{
			throw new ArgumentNullException("manager");
		}
		items = new DeviceLinkEntries();
		configurationManager = manager;
		deletedItems = new List<DeviceLink>();
		configurationManager.ChangesAccepted += ConfigurationManager_ChangesAccepted;
		configurationManager.ChangesRejected += ConfigurationManager_ChangesRejected;
		table = manager.Dataset.DeviceLinks;
		configurationManager.EnsureTableDataLoaded(table);
		foreach (ConfigurationDataSet.DeviceLinksRow row in table.Rows)
		{
			Add(row);
		}
		modified = false;
	}

	public DeviceLink Add(Guid deviceId, Guid linkedDeviceId)
	{
		ConfigurationDataSet.DeviceLinksRow row = configurationManager.Dataset.DeviceLinks.AddDeviceLinksRow(DateTime.UtcNow, deviceId.ToString(), linkedDeviceId.ToString(), 0);
		return Add(row);
	}

	private DeviceLink Add(ConfigurationDataSet.DeviceLinksRow row)
	{
		DeviceLink deviceLink = new DeviceLink(configurationManager, row);
		items.Add(deviceLink);
		modified = true;
		return deviceLink;
	}

	public void Remove(Guid deviceId, Guid linkedDeviceId)
	{
		string key = DeviceLink.Key(deviceId, linkedDeviceId);
		DeviceLink deviceLink = items.QueryByKey(key);
		if (deviceLink != null)
		{
			DeleteItem(deviceLink);
		}
	}

	public bool Contains(Guid deviceId, Guid linkedDeviceId)
	{
		string key = DeviceLink.Key(deviceId, linkedDeviceId);
		return items.QueryByKey(key) != null;
	}

	private void DeleteItem(DeviceLink item)
	{
		items.Remove(item);
		item.DataRow.Delete();
		deletedItems.Add(item);
		modified = true;
	}

	public void DeviceDeleted(Guid deviceId)
	{
		IEnumerable<DeviceLink> source = items.QueryByDeviceId(deviceId);
		for (int num = source.Count() - 1; num >= 0; num--)
		{
			DeleteItem(source.ElementAt(num));
		}
		source = items.QueryByLinkedDeviceId(deviceId);
		for (int num2 = source.Count() - 1; num2 >= 0; num2--)
		{
			DeleteItem(source.ElementAt(num2));
		}
	}

	private void ConfigurationManager_ChangesRejected(object sender, EventArgs e)
	{
		if (!modified)
		{
			return;
		}
		List<DeviceLink> list = new List<DeviceLink>();
		foreach (DeviceLink item in items)
		{
			if (item.DataRow.RowState == DataRowState.Detached)
			{
				list.Add(item);
			}
		}
		list.ForEach(delegate(DeviceLink i)
		{
			items.Remove(i);
		});
		deletedItems.ForEach(delegate(DeviceLink i)
		{
			items.Add(i);
		});
		deletedItems.Clear();
		modified = false;
	}

	private void ConfigurationManager_ChangesAccepted(object sender, EventArgs e)
	{
		if (modified)
		{
			deletedItems.Clear();
			modified = false;
		}
	}

	public IEnumerator<DeviceLink> GetEnumerator()
	{
		return items.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return items.GetEnumerator();
	}
}
