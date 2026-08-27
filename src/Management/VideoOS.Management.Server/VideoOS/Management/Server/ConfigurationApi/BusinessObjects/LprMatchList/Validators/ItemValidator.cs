using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.LPR.Common.WCF.DataContracts;
using VideoOS.Management.Server.ConfigurationApi.Validators;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi.BusinessObjects.LprMatchList.Validators;

public class ItemValidator : IItemValidator
{
	private readonly ICommonValidator _commonValidator;

	public string[] ValidEventItemTypes => new string[2] { "Output", "TriggerEvent" };

	public ItemValidator(ICommonValidator commonValidator)
	{
		_commonValidator = commonValidator;
	}

	public ValidateResult Validate(IItemReader item, ConfigurationItemPath path)
	{
		item.NullCheck("", "Validate", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\LPRMatchList\\Validators\\ItemValidator.cs", 25);
		path.NullCheck("", "Validate", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\LPRMatchList\\Validators\\ItemValidator.cs", 26);
		return Validate(item, delegate
		{
			ValidateCommon(item);
			ValidateItemType(item);
			ValidateItemIdAndPath(item, path);
			ValidateItemName(item);
			ValidateTriggeredEvents(item);
			ValidateCustomFields(item);
		});
	}

	public void ValidateUniqueName(Guid itemId, string name, LicensePlateList[] licensePlateLists)
	{
		name.NullCheck("", "ValidateUniqueName", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\LPRMatchList\\Validators\\ItemValidator.cs", 39);
		licensePlateLists.NullCheck("", "ValidateUniqueName", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\LPRMatchList\\Validators\\ItemValidator.cs", 40);
		LicensePlateList licensePlateList = licensePlateLists.FirstOrDefault((LicensePlateList s) => s.Id != itemId && s.Name.Trim().ToUpperInvariant() == name.Trim().ToUpperInvariant());
		if (licensePlateList != null)
		{
			Throw("Item name is duplicated for item with id: {0}; Another item with such name has id: {1}", itemId, licensePlateList.Id);
		}
	}

	public ValidateResult CreateOkResult(ConfigurationItem item)
	{
		item.NullCheck("", "CreateOkResult", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\LPRMatchList\\Validators\\ItemValidator.cs", 51);
		return new ValidateResult
		{
			ValidatedOk = true,
			ResultItem = item
		};
	}

	private ValidateResult Validate(IItemReader item, Action action)
	{
		try
		{
			action();
			return CreateOkResult(item.Item);
		}
		catch (ArgumentException exception)
		{
			ValidateResult validateResult = ErrorHelper.CreateValidateResult(exception, item.Item.Properties);
			if (validateResult != null)
			{
				validateResult.ResultItem = item.Item;
				return validateResult;
			}
			throw;
		}
	}

	private static void ValidateCommon(IItemReader item)
	{
		if (item.LicensePlateLists == null)
		{
			Throw("Failed to obtain license plate match lists");
		}
	}

	private static void ValidateItemType(IItemReader item)
	{
		if (item.Item.ItemType != "LprMatchList")
		{
			Throw("Item type should be {0}", "LprMatchList");
		}
	}

	private static void ValidateItemIdAndPath(IItemReader item, ConfigurationItemPath path)
	{
		if (!item.Id.HasValue)
		{
			ThrowPropertyNotSet(item, "Id");
		}
		if (path.IsFolder)
		{
			Throw("Item path cannot be a folder");
		}
		if (path.Id.ToUpperInvariant() != item.Id.ToString().ToUpperInvariant())
		{
			Throw("Path id does not match item id: {0}", item.Id);
		}
		if (item.ItemLicensePlateList == null)
		{
			Throw("Cannot find a match list with such id: {0}", item.Id);
		}
	}

	private void ValidateItemName(IItemReader item)
	{
		if (string.IsNullOrEmpty(item.Name))
		{
			ThrowPropertyNotSet(item, "Name");
		}
		ValidateUniqueName(item.Id.Value, item.Name, item.LicensePlateLists);
	}

	private void ValidateTriggeredEvents(IItemReader item)
	{
		if (item.TriggeredEvents == null)
		{
			ThrowPropertyNotSet(item, "TriggerEventList");
		}
		string[] triggeredEvents = item.TriggeredEvents;
		foreach (string pathString in triggeredEvents)
		{
			_commonValidator.ValidateItemListPath(pathString, new HashSet<string>(ValidEventItemTypes), "TriggerEventList");
		}
	}

	private static void ValidateCustomFields(IItemReader item)
	{
		if (item.CustomFields == null)
		{
			ThrowPropertyNotSet(item, "CustomFields");
		}
	}

	private static void Throw(string msg, params object[] args)
	{
		throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, msg, args));
	}

	private static void ThrowPropertyNotSet(IItemReader item, string property)
	{
		Throw("Item property {0} is not set for item with id: {1}", property, item.Id?.ToString() ?? "NULL");
	}
}
