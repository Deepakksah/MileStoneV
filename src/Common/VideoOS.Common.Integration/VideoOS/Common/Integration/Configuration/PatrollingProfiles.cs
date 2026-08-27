using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using VideoOS.Management.Common;

namespace VideoOS.Common.Integration.Configuration;

public class PatrollingProfiles : CollectionBase
{
	public new PatrollingProfile this[Guid id] => base[id] as PatrollingProfile;

	public PatrollingProfiles(ConfigurationManager manager)
		: base(manager, manager.Dataset.PatrollingProfiles)
	{
	}

	public PatrollingProfile Add(Guid patrollingProfileId, Guid deviceId, string name, string description, string profileXml)
	{
		return Add(_configurationManager.Dataset.PatrollingProfiles.AddPatrollingProfilesRow(DateTime.UtcNow, patrollingProfileId.ToString(), deviceId.ToString(), name, description, profileXml)) as PatrollingProfile;
	}

	protected override IConfigurationItem GetInstance(DataRow row)
	{
		return new PatrollingProfile(_configurationManager, row as ConfigurationDataSet.PatrollingProfilesRow);
	}

	public void RemoveProfilesForDevice(Guid deviceID)
	{
		try
		{
			ArrayList arrayList = new ArrayList();
			foreach (PatrollingProfile value in base.Values)
			{
				if (value.IDDevice == deviceID)
				{
					arrayList.Add(value.ID);
				}
			}
			foreach (Guid item in arrayList)
			{
				Remove(item);
			}
		}
		catch (RowNotInTableException)
		{
		}
	}

	public List<string> ContainsPreset(Guid deviceId, string presetKey)
	{
		List<string> list = new List<string>();
		foreach (PatrollingProfile value in base.Values)
		{
			try
			{
				if (value.IDDevice != deviceId)
				{
					continue;
				}
			}
			catch (RowNotInTableException)
			{
				continue;
			}
			PatrollingSchema patrollingSchema = new PatrollingSchema(Guid.NewGuid());
			patrollingSchema.LoadXml(value.DefinitionAsXml);
			if (patrollingSchema.ContainsPreset(presetKey))
			{
				list.Add(value.Name);
			}
		}
		return list;
	}

	public void RemovePreset(Guid deviceId, string presetKey)
	{
		foreach (PatrollingProfile value in base.Values)
		{
			try
			{
				if (value.IDDevice != deviceId)
				{
					continue;
				}
			}
			catch (RowNotInTableException)
			{
				continue;
			}
			PatrollingSchema patrollingSchema = new PatrollingSchema(Guid.NewGuid());
			patrollingSchema.LoadXml(value.DefinitionAsXml);
			patrollingSchema.RemovePreset(presetKey);
			value.DefinitionAsXml = patrollingSchema.ToXml();
		}
	}

	public void ChangePresetKey(Guid deviceId, string oldPresetKey, string newPresetKey)
	{
		foreach (PatrollingProfile value in base.Values)
		{
			try
			{
				if (value.IDDevice != deviceId)
				{
					continue;
				}
			}
			catch (RowNotInTableException)
			{
				continue;
			}
			PatrollingSchema patrollingSchema = new PatrollingSchema(Guid.NewGuid());
			patrollingSchema.LoadXml(value.DefinitionAsXml);
			if (patrollingSchema.ContainsPreset(oldPresetKey))
			{
				patrollingSchema.ChangePresetKey(oldPresetKey, newPresetKey);
			}
			value.DefinitionAsXml = patrollingSchema.ToXml();
		}
	}
}
