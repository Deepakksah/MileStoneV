using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Common.Integration.Configuration;

public class IPHardwareBlockCollection : IEnumerable<IPHardwareBlock>, IEnumerable
{
	private ConfigurationManager configurationManager;

	private RollbackDictionary<int, IPHardwareBlock> dictionary;

	public IPHardwareBlockCollection(ConfigurationManager configurationManager)
	{
		if (configurationManager == null)
		{
			throw new ArgumentNullException("configurationManager");
		}
		this.configurationManager = configurationManager;
		dictionary = new RollbackDictionary<int, IPHardwareBlock>(this.configurationManager, this);
		ConfigurationDataSet.HardwareBlockDataTable hardwareBlock = configurationManager.Dataset.HardwareBlock;
		this.configurationManager.EnsureTableDataLoaded(hardwareBlock);
		foreach (ConfigurationDataSet.HardwareBlockRow item in hardwareBlock)
		{
			IPHardwareBlock value = new IPHardwareBlock(this.configurationManager, item);
			dictionary.Add(item.pkid, value);
		}
	}

	public void RemoveByHardware(IPHardware hardware)
	{
		if (hardware == null)
		{
			throw new ArgumentNullException("hardware");
		}
		IPHardwareBlock[] array = this.Where((IPHardwareBlock b) => b.HardwareId == hardware.ID).ToArray();
		foreach (IPHardwareBlock item in array)
		{
			Remove(item);
		}
	}

	public void Remove(IPHardwareBlock item)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		dictionary.Remove(item.Id);
		item.DataRow.Delete();
	}

	public IEnumerator<IPHardwareBlock> GetEnumerator()
	{
		return dictionary.Values.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return dictionary.Values.GetEnumerator();
	}
}
