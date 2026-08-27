using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using VideoOS.Common.Integration.Definition;

namespace VideoOS.Common.Integration.Configuration;

public class IPHardwareMembers : IEnumerable<IPHardware>, IEnumerable
{
	private ConfigurationManager configurationManager;

	private Recorder recorder;

	private RollbackDictionary<Guid, IPHardware> dictionary;

	public IPHardware this[Guid id]
	{
		[DebuggerStepThrough]
		get
		{
			return dictionary[id];
		}
	}

	public int Count => dictionary.Count;

	public IPHardwareMembers(ConfigurationManager configurationManager, Recorder recorder)
	{
		if (configurationManager == null)
		{
			throw new ArgumentNullException("configurationManager");
		}
		if (recorder == null)
		{
			throw new ArgumentNullException("recorder");
		}
		this.configurationManager = configurationManager;
		this.recorder = recorder;
		dictionary = new RollbackDictionary<Guid, IPHardware>(configurationManager, this);
		IPHardware[] hardwareByRecorderId = this.configurationManager.HardwareItems.GetHardwareByRecorderId(recorder.ID);
		foreach (IPHardware iPHardware in hardwareByRecorderId)
		{
			dictionary.Add(iPHardware.ID, iPHardware);
		}
	}

	public IPHardware AddNew(Guid id, string name, string description, bool enabled, Uri uri, string logOnId, string password, Guid idHardwareDriver, string definitionXml, string settings, string eventSettings, string productId, int driverNumber, int? hostedHardwareId, string cameraData, string edgeSettings, Subtypes subtype)
	{
		string text = DefinitionUtility.CreateUniqueHardwareDefinitionKey(definitionXml);
		if (!configurationManager.HardwareDefinitionItems.TryGetHardwareDefinitionByDigest(text, out var definition))
		{
			configurationManager.HardwareDefinitionItems.AddNew(text, definitionXml);
		}
		else if (definition.DataRow.RowState == DataRowState.Unchanged)
		{
			definition.DataRow.SetAdded();
		}
		IPHardware iPHardware = configurationManager.HardwareItems.AddNew(id, name, description, enabled, uri, logOnId, password, recorder, idHardwareDriver, settings, eventSettings, productId, text, driverNumber, hostedHardwareId, cameraData, edgeSettings, subtype);
		dictionary.Add(iPHardware.ID, iPHardware);
		return iPHardware;
	}

	public void Remove(IPHardware hardware)
	{
		if (hardware == null)
		{
			throw new ArgumentNullException("hardware");
		}
		dictionary.Remove(hardware.ID);
		configurationManager.HardwareItems.Remove(hardware.ID);
	}

	public void Clear()
	{
		foreach (IPHardware value in dictionary.Values)
		{
			configurationManager.HardwareItems.Remove(value.ID);
		}
		dictionary.Clear();
	}

	public IEnumerator<IPHardware> GetEnumerator()
	{
		return dictionary.Values.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return dictionary.Values.GetEnumerator();
	}
}
