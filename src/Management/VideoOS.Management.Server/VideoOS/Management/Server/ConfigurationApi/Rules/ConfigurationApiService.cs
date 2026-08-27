using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Server.ConfigurationApi.MethodHandling;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Builders;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;
using VideoOS.Management.Server.ConfigurationApi.Rules.Builders;
using VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;
using VideoOS.Management.Server.ConfigurationApi.Validators;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi.Rules;

public class ConfigurationApiService : ConfigurationApiBase, IConfigurationApiSupport
{
	private readonly IConfigurationItemBuilder _itemBuilder;

	private readonly IRuleModelBuilder _ruleModelBuilder;

	private readonly IMethodProvider _methodProvider;

	private readonly ICommonValidator _commonValidator;

	private readonly IRuleProxy _ruleProxy;

	private readonly IRuleModelXmlBuilder _modelToXmlBuilder;

	private readonly IXmlRuleModelBuilder _modelFromXmlBuilder;

	private readonly IValidator<RuleModel> _ruleModelValidator;

	public ConfigurationApiService(IConfigurationItemBuilder itemBuilder, IRuleModelBuilder ruleModelBuilder, IMethodProvider methodProvider, ICommonValidator commonValidator, IRuleProxy ruleProxy, IRuleModelXmlBuilder modelToXmlBuilder, IXmlRuleModelBuilder modelFromXmlBuilder, IValidator<RuleModel> ruleModelValidator)
	{
		_itemBuilder = itemBuilder;
		_ruleModelBuilder = ruleModelBuilder;
		_methodProvider = methodProvider;
		_commonValidator = commonValidator;
		_ruleProxy = ruleProxy;
		_modelToXmlBuilder = modelToXmlBuilder;
		_modelFromXmlBuilder = modelFromXmlBuilder;
		_ruleModelValidator = ruleModelValidator;
	}

	public ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		PrepareRequest(requestContext, path);
		if (!path.IsFolder)
		{
			return new ConfigurationItem[0];
		}
		string[] ids = _ruleProxy.QueryRuleIds(requestContext);
		Rule[] array = _ruleProxy.QueryRulesByIds(requestContext, ids);
		if (array == null || !array.Any())
		{
			return new ConfigurationItem[0];
		}
		IEnumerable<RuleModel> enumerable = from x in array
			select RuleModelFromXml(requestContext, x) into s
			where s != null
			select s;
		Dictionary<RuleModel, ConfigurationItem> dictionary = new Dictionary<RuleModel, ConfigurationItem>();
		foreach (RuleModel item in enumerable)
		{
			ConfigurationItem configurationItem = ItemFromRuleModel(requestContext, item);
			if (configurationItem != null)
			{
				dictionary.Add(item, configurationItem);
			}
		}
		List<ConfigurationItem> list = new List<ConfigurationItem>();
		foreach (RuleModel key in dictionary.Keys)
		{
			ValidationError[] array2 = _ruleModelValidator.Validate(requestContext, key);
			if (array2.Any())
			{
				list.Add(UpdateItemFromError(dictionary[key], array2));
			}
			else
			{
				list.Add(dictionary[key]);
			}
		}
		return list.ToArray();
	}

	public ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		PrepareRequest(requestContext, path);
		if (path.IsFolder)
		{
			return GroupConfigurationItemFactory.CreateFolderItem("RuleFolder", ConfigurationItemPath.CreateSystemPath());
		}
		Rule rule = _ruleProxy.QueryRuleById(requestContext, path.Id);
		if (rule == null)
		{
			throw new PathNotFoundException(path.Text);
		}
		RuleModel ruleModel = RuleModelFromXml(requestContext, rule);
		if (ruleModel == null)
		{
			throw new NotSupportedException();
		}
		ConfigurationItem configurationItem = ItemFromRuleModel(requestContext, ruleModel);
		if (configurationItem == null)
		{
			throw new NotSupportedException();
		}
		ValidationError[] array = _ruleModelValidator.Validate(requestContext, ruleModel);
		if (array.Length != 0)
		{
			configurationItem = UpdateItemFromError(configurationItem, array);
		}
		return configurationItem;
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string method)
	{
		item.NullCheck("", "InvokeMethod", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\ConfigurationApiService.cs", 132);
		PrepareRequest(requestContext, path);
		return _methodProvider.Provide(item, method).Invoke(requestContext, item, path, parentPath);
	}

	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		item.NullCheck("", "SetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\ConfigurationApiService.cs", 139);
		PrepareRequest(requestContext, path);
		RuleModel ruleModel = _ruleModelBuilder.Build(requestContext, item);
		ValidateResult validateResult = ValidateInternal(requestContext, ruleModel, path);
		if (validateResult.ValidatedOk)
		{
			string xml = _modelToXmlBuilder.GetXml(ruleModel);
			Rule rule = _ruleProxy.QueryRuleById(requestContext, path.Id);
			Rule rule2 = ((rule != null) ? _ruleProxy.UpdateRule(requestContext, rule.Id, ruleModel.Name, ruleModel.Description, xml, ruleModel.Enabled) : _ruleProxy.CreateRule(requestContext, path.Id, ruleModel.Name, ruleModel.Description, xml, ruleModel.Enabled));
			RuleModel model = RuleModelFromXml(requestContext, rule2);
			validateResult.ResultItem = ItemFromRuleModel(requestContext, model);
		}
		return validateResult;
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		item.NullCheck("", "ValidateItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\ConfigurationApiService.cs", 162);
		_commonValidator.ValidatePath(parentPath);
		PrepareRequest(requestContext, path);
		RuleModel model = _ruleModelBuilder.Build(requestContext, item);
		return ValidateInternal(requestContext, model, path);
	}

	private static ValidationError[] ValidatePathId(RuleModel model, ConfigurationItemPath path)
	{
		if (model.Id != new Guid(path.Id))
		{
			return new ValidationError[1]
			{
				new ValidationError("Id:{0} in item differs from Id:{1} in path", model.Id, path.Id)
				{
					PropertyName = "Id"
				}
			};
		}
		return new ValidationError[0];
	}

	private ValidateResult ValidateInternal(IGeminiRequestContext requestContext, RuleModel model, ConfigurationItemPath path)
	{
		List<ValidationError> list = _ruleModelValidator.Validate(requestContext, model).ToList();
		list.AddRange(ValidatePathId(model, path));
		ConfigurationItem item = ItemFromRuleModel(requestContext, model);
		if (list.Any())
		{
			ErrorResult[] errorResults = list.Select((ValidationError x) => new ErrorResult
			{
				ErrorText = x.Message,
				ErrorProperty = x.PropertyName
			}).ToArray();
			return new ValidateResult
			{
				ResultItem = UpdateItemFromError(item, list),
				ValidatedOk = false,
				ErrorResults = errorResults
			};
		}
		return CreateOkResult(item);
	}

	private static ConfigurationItem UpdateItemFromError(ConfigurationItem item, IEnumerable<ValidationError> validationErrors)
	{
		if (item?.Properties == null)
		{
			return item;
		}
		List<Property> list = new List<Property>(item.Properties);
		foreach (ValidationError error in validationErrors)
		{
			if (error.ValidProperty == null)
			{
				continue;
			}
			if (list.Any((Property x) => x.Key == error.PropertyName))
			{
				Property item2 = list.First((Property x) => x.Key == error.PropertyName);
				int index = list.IndexOf(item2);
				list.RemoveAt(index);
				list.Insert(index, error.ValidProperty);
			}
			else
			{
				list.Add(error.ValidProperty);
			}
		}
		item.Properties = list.ToArray();
		return item;
	}

	private static ValidateResult CreateOkResult(ConfigurationItem item)
	{
		return new ValidateResult
		{
			ValidatedOk = true,
			ResultItem = item
		};
	}

	private void PrepareRequest(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		requestContext.NullCheck("", "PrepareRequest", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\ConfigurationApiService.cs", 248);
		_commonValidator.ValidatePath(path);
	}

	private RuleModel RuleModelFromXml(IGeminiRequestContext requestContext, Rule rule)
	{
		try
		{
			return _modelFromXmlBuilder.Build(new Guid(rule.Id), rule.Name, rule.Description, rule.Definition, rule.Enabled, requestContext);
		}
		catch (Exception ex)
		{
			GeminiTrace.Error(TraceType.General, string.Format(CultureInfo.InvariantCulture, "{0} : {1}", ex.StackTrace, ex.Message));
			return null;
		}
	}

	private ConfigurationItem ItemFromRuleModel(IGeminiRequestContext requestContext, RuleModel model)
	{
		try
		{
			return _itemBuilder.Build(requestContext, model);
		}
		catch (NotSupportedException ex)
		{
			GeminiTrace.Error(TraceType.General, string.Format(CultureInfo.InvariantCulture, "Unsupported rule item: " + ex.Message));
			return null;
		}
		catch (Exception ex2)
		{
			GeminiTrace.Error(TraceType.General, string.Format(CultureInfo.InvariantCulture, "{0} : {1}", ex2.StackTrace, ex2.Message));
			return null;
		}
	}
}
