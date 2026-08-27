using System;
using System.Collections.Generic;

namespace VideoOS.Common.SharedTimeProfile;

public class TimeProfileManager : ITimeProfileEvaluation
{
	private Dictionary<Guid, TimeProfileEvaluator> timeProfileEvaluators;

	public TimeProfileManager()
		: this(new Dictionary<Guid, TimeProfileEvaluator>())
	{
	}

	public TimeProfileManager(Dictionary<Guid, TimeProfileEvaluator> innerDictionary)
	{
		timeProfileEvaluators = innerDictionary;
	}

	public bool EvaluateTimeProfile(Guid id, DateTime utcTime)
	{
		if (timeProfileEvaluators.TryGetValue(id, out var value))
		{
			return value.EvaluateTime(utcTime);
		}
		return false;
	}

	public TimeSpan GetAppointmentDuration(Guid id, DateTime utcTime, TimeSpan searchInterval)
	{
		if (timeProfileEvaluators.TryGetValue(id, out var value))
		{
			return value.GetAppointmentDuration(utcTime, searchInterval);
		}
		return new TimeSpan(0L);
	}

	public void AddTimeProfile(Guid id, string definitionXml, TimeProfileType timeProfileType)
	{
		TimeProfileEvaluator timeProfileEvaluator = new TimeProfileEvaluator(id, definitionXml, timeProfileType);
		lock (this)
		{
			if (timeProfileEvaluator != null)
			{
				if (timeProfileEvaluators.ContainsKey(id))
				{
					timeProfileEvaluators[id] = timeProfileEvaluator;
				}
				else
				{
					timeProfileEvaluators.Add(id, timeProfileEvaluator);
				}
				return;
			}
			throw new ArgumentException("DefinitionXml can't be parsed to a timeprofile");
		}
	}

	public bool RemoveTimeProfile(Guid id)
	{
		lock (this)
		{
			return timeProfileEvaluators.Remove(id);
		}
	}

	public void Clear()
	{
		lock (this)
		{
			timeProfileEvaluators.Clear();
		}
	}

	public void ClearAppointmentCaches()
	{
		lock (this)
		{
			foreach (TimeProfileEvaluator value in timeProfileEvaluators.Values)
			{
				value.ClearAppointmentCache();
			}
		}
	}

	public static bool EvaluateTimeProfile(string definitionXml, TimeProfileType timeProfileType, DateTime utcTime)
	{
		TimeProfileEvaluator timeProfileEvaluator = null;
		try
		{
			timeProfileEvaluator = new TimeProfileEvaluator(Guid.Empty, definitionXml, timeProfileType);
		}
		catch (ArgumentException)
		{
		}
		return timeProfileEvaluator?.EvaluateTime(utcTime) ?? false;
	}

	public static TimeSpan GetAppointmentDuration(string definitionXml, TimeProfileType timeProfileType, DateTime utcTime, TimeSpan searchInterval)
	{
		TimeProfileEvaluator timeProfileEvaluator = null;
		try
		{
			timeProfileEvaluator = new TimeProfileEvaluator(Guid.Empty, definitionXml, timeProfileType);
		}
		catch (ArgumentException)
		{
		}
		return timeProfileEvaluator?.GetAppointmentDuration(utcTime, searchInterval) ?? new TimeSpan(0L);
	}

	public static bool ConvertTimeProfileType(string timeProfileType, out TimeProfileType outValue)
	{
		if (timeProfileType == null)
		{
			throw new ArgumentNullException("timeProfileType");
		}
		outValue = TimeProfileType.CalendarProfile;
		string text = timeProfileType.Trim(' ');
		if (string.IsNullOrEmpty(text))
		{
			return false;
		}
		Type typeFromHandle = typeof(TimeProfileType);
		string[] names = Enum.GetNames(typeFromHandle);
		Array values = Enum.GetValues(typeFromHandle);
		for (int i = 0; i < names.Length; i++)
		{
			TimeProfileType timeProfileType2 = (TimeProfileType)values.GetValue(i);
			if (string.Equals(text, names[i], StringComparison.OrdinalIgnoreCase))
			{
				outValue = timeProfileType2;
				return true;
			}
		}
		return false;
	}
}
