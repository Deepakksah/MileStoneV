using System.Collections.Generic;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.LPR.Common.WCF.DataContracts;
using VideoOS.Management.Server.ConfigurationApi.BusinessObjects.LprMatchList.Validators;
using VideoOS.Management.Server.ConfigurationApi.Converters;
using VideoOS.Management.Server.Properties;
using VideoOS.Platform;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi.BusinessObjects.LprMatchList.Builders;

public class ConfigurationItemBuilder : IConfigurationItemBuilder
{
	private readonly IItemValidator _validator;

	private readonly IFQIDConverter _fqidConverter;

	private readonly IStringCollectionConverter _stringCollectionConverter;

	public ConfigurationItemBuilder(IItemValidator validator, IFQIDConverter fqidConverter, IStringCollectionConverter stringCollectionConverter)
	{
		validator.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\LPRMatchList\\Builders\\ConfigurationItemBuilder.cs", 23);
		fqidConverter.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\LPRMatchList\\Builders\\ConfigurationItemBuilder.cs", 24);
		_validator = validator;
		_fqidConverter = fqidConverter;
		_stringCollectionConverter = stringCollectionConverter;
	}

	public ConfigurationItem Build(ConfigurationItemPath path, ConfigurationItemPath parentPath, LicensePlateList list, LicensePlateInfo[] licensePlates, string[] methodIds)
	{
		Property[] properties = BuildItemProperties(list, licensePlates);
		return new ConfigurationItem
		{
			ItemType = "LprMatchList",
			UIHint = "DirectionHorizontal",
			ItemCategory = ItemCategories.Item,
			DisplayName = list.Name,
			ParentPath = parentPath.Text,
			Path = path.Text,
			Properties = properties,
			MethodIds = methodIds
		};
	}

	private Property[] BuildItemProperties(LicensePlateList list, LicensePlateInfo[] licensePlates)
	{
		return new Property[5]
		{
			GeneralPropertyDefinitions.GetIdPropertyGuid(list.Id),
			GeneralPropertyDefinitions.GetNameProperty(list.Name, isSettable: true),
			CreateEventListProperty(list.TriggerEvents),
			CreateCustomFieldsProperty(list.PropertyDefinitions),
			CreateRegistrationNumbersProperty(licensePlates)
		};
	}

	private Property CreateEventListProperty(IEnumerable<FQID> ids)
	{
		return new Property("TriggerEventList", _fqidConverter.ConvertFQIDsToPaths(ids))
		{
			ValueType = "PathList",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyTriggerEventList,
			ItemReference = true,
			ServerValidation = false,
			TranslationId = "PropertyTriggerEventList",
			ToolTipTranslationId = "PropertyMatchListTriggerEventListTip",
			UIImportance = UIImportance.Summary,
			SortKey = 100,
			ValueTypeInfos = _validator.ValidEventItemTypes.Select((string s) => new ValueTypeInfo
			{
				Name = "PathItemType",
				Value = s
			}).ToArray()
		};
	}

	private Property CreateCustomFieldsProperty(LicensePlatePropertyDefinition[] customFields)
	{
		string[] enumerable = (from s in customFields
			orderby s.ColumnIndex
			select s.Name).ToArray();
		return GeneralPropertyDefinitions.GetListProperty(_stringCollectionConverter.EnumerableToString(enumerable), "CustomFields", ConfigurationApiResources.PropertyMatchListCustomFieldsList, "PropertyMatchListCustomFieldsList", isSettable: true);
	}

	private Property CreateRegistrationNumbersProperty(LicensePlateInfo[] licensePlates)
	{
		string[] enumerable = licensePlates.Select((LicensePlateInfo s) => s.LicensePlate.RegistrationNumber).ToArray();
		return GeneralPropertyDefinitions.GetListProperty(_stringCollectionConverter.EnumerableToString(enumerable), "RegistrationNumbers", ConfigurationApiResources.PropertyMatchListRegistrationNumbersList, "PropertyMatchListRegistrationNumbersList", isSettable: false);
	}
}
