using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Common.SharedTimeProfile;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules;

public sealed class TimeProfileConfiguration : ITimeProfileConfiguration, ITimeProfileEvaluation
{
	private object lockObject = new object();

	private TimeProfileManager timeProfileManager;

	public TimeProfileConfiguration()
	{
		timeProfileManager = new TimeProfileManager();
	}

	public void SetConfiguration(IEnumerable<(RuleModification modification, Guid timeProfileId, string timeProfileType, string xmlDefinition)> configuration)
	{
		UpdateConfiguration(configuration, initialConfiguration: true);
	}

	public bool UpdateConfiguration(IEnumerable<(RuleModification modification, Guid timeProfileId, string timeProfileType, string xmlDefinition)> configuration, bool initialConfiguration)
	{
		bool result = false;
		lock (lockObject)
		{
			if (initialConfiguration)
			{
				timeProfileManager = new TimeProfileManager();
			}
			if (configuration != null && configuration.Any())
			{
				result = true;
				foreach (var item in configuration)
				{
					switch (item.modification)
					{
					case RuleModification.Add:
					case RuleModification.Update:
					{
						TimeProfileManager.ConvertTimeProfileType(item.timeProfileType, out var outValue);
						timeProfileManager.AddTimeProfile(item.timeProfileId, item.xmlDefinition, outValue);
						timeProfileManager.ClearAppointmentCaches();
						break;
					}
					case RuleModification.Remove:
						timeProfileManager.RemoveTimeProfile(item.timeProfileId);
						timeProfileManager.ClearAppointmentCaches();
						break;
					}
				}
			}
		}
		return result;
	}

	public bool EvaluateTimeProfile(Guid id, DateTime utcTime)
	{
		return timeProfileManager.EvaluateTimeProfile(id, utcTime);
	}

	public TimeSpan GetAppointmentDuration(Guid id, DateTime utcTime, TimeSpan searchInterval)
	{
		return timeProfileManager.GetAppointmentDuration(id, utcTime, searchInterval);
	}
}
