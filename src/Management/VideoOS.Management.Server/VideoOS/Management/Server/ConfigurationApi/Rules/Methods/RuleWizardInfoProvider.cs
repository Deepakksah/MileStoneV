using System.Collections.Generic;
using VideoOS.ConfigurationAPI;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Methods;

public class RuleWizardInfoProvider : IRuleWizardInfoProvider
{
	private readonly List<IWizardStep> _steps;

	private IWizardStep _currentStep;

	public RuleWizardInfoProvider(List<IWizardStep> steps)
	{
		_steps = steps;
	}

	public bool IsComplete(IGeminiRequestContext requestContext, ConfigurationItem item)
	{
		foreach (IWizardStep step in _steps)
		{
			if (!step.IsComplete(requestContext, item))
			{
				_currentStep = step;
				return false;
			}
		}
		return true;
	}

	public Property[] GetProperties(IGeminiRequestContext requestContext, ConfigurationItem item)
	{
		List<Property> list = new List<Property>();
		list.AddRange(GetCurrentProperties(requestContext, item, out var lastPropertyKey));
		list.AddRange(GetNextProperties(requestContext, item, lastPropertyKey));
		return list.ToArray();
	}

	private List<Property> GetCurrentProperties(IGeminiRequestContext requestContext, ConfigurationItem item, out string lastPropertyKey)
	{
		lastPropertyKey = null;
		List<Property> list = new List<Property>();
		foreach (IWizardStep step in _steps)
		{
			list.AddRange(step.GetCurrentProperties(requestContext, item, out lastPropertyKey));
			if (_currentStep == step)
			{
				break;
			}
		}
		return list;
	}

	private List<Property> GetNextProperties(IGeminiRequestContext requestContext, ConfigurationItem item, string lastPropertyKey)
	{
		if (_currentStep != null)
		{
			return _currentStep.GetNextProperties(requestContext, item, lastPropertyKey);
		}
		return new List<Property>();
	}
}
