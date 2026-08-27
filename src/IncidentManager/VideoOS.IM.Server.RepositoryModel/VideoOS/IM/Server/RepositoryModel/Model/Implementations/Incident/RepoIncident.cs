using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using VideoOS.IM.Common.GlobalDefinitions.Configuration;
using VideoOS.IM.Common.GlobalDefinitions.Localization;
using VideoOS.IM.Common.GlobalDefinitions.Utilities;
using VideoOS.IM.Server.RepositoryModel.CommonCode;
using VideoOS.IM.Server.RepositoryModel.Model.Abstractions.Common;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.DynamicalDataTypesAndOptions;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.IncidentActivityLog;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.IncidentCall;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.IncidentComment;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.IncidentSequence;
using VideoOS.IM.Server.RepositoryModel.Model.Interfaces.Common;
using VideoOS.IM.Server.RepositoryModel.Model.Interfaces.Incident;

namespace VideoOS.IM.Server.RepositoryModel.Model.Implementations.Incident;

public class RepoIncident : ARepoBaseEntity, IRepoIncident, IRepoBaseEntity, ILoggingMultilineProvider
{
	public long Reference { get; set; }

	public string ReferenceStr { get; set; }

	public string TypeStr { get; set; }

	public long? TypeId { get; set; }

	public virtual RepoDynamicalDataTypesAndOptions Type { get; set; }

	public string StatusStr { get; set; }

	public long? StatusId { get; set; }

	public virtual RepoDynamicalDataTypesAndOptions Status { get; set; }

	public DateTime? Start { get; set; }

	public string StartStr { get; set; }

	public DateTime? End { get; set; }

	public TimeSpan? Time { get; set; }

	public DateTime? Date { get; set; }

	public string Description { get; set; }

	public long ExtPropLong1 { get; set; }

	public string ExtPropLong1_Title { get; set; }

	public string ExtPropLong1_Desc { get; set; }

	public long ExtPropLong2 { get; set; }

	public string ExtPropLong2_Title { get; set; }

	public string ExtPropLong2_Desc { get; set; }

	public long ExtPropLong3 { get; set; }

	public string ExtPropLong3_Title { get; set; }

	public string ExtPropLong3_Desc { get; set; }

	public string ExtPropString1 { get; set; }

	public string ExtPropString1_Title { get; set; }

	public string ExtPropString1_Desc { get; set; }

	public string ExtPropString2 { get; set; }

	public string ExtPropString2_Title { get; set; }

	public string ExtPropString2_Desc { get; set; }

	public string ExtPropString3 { get; set; }

	public string ExtPropString3_Title { get; set; }

	public string ExtPropString3_Desc { get; set; }

	public bool ExtPropBool1 { get; set; }

	public string ExtPropBool1_Title { get; set; }

	public string ExtPropBool1_Desc { get; set; }

	public bool ExtPropBool2 { get; set; }

	public string ExtPropBool2_Title { get; set; }

	public string ExtPropBool2_Desc { get; set; }

	public bool ExtPropBool3 { get; set; }

	public string ExtPropBool3_Title { get; set; }

	public string ExtPropBool3_Desc { get; set; }

	public decimal ExtPropDecimal1 { get; set; }

	public string ExtPropDecimal1_Title { get; set; }

	public string ExtPropDecimal1_Desc { get; set; }

	public decimal ExtPropDecimal2 { get; set; }

	public string ExtPropDecimal2_Title { get; set; }

	public string ExtPropDecimal2_Desc { get; set; }

	public decimal ExtPropDecimal3 { get; set; }

	public string ExtPropDecimal3_Title { get; set; }

	public string ExtPropDecimal3_Desc { get; set; }

	public DateTime? ExtPropDateTime1 { get; set; }

	public string ExtPropDateTime1_Title { get; set; }

	public string ExtPropDateTime1_Desc { get; set; }

	public string ExtPropDateTime1_Configuration { get; set; }

	public DateTime? ExtPropDateTime2 { get; set; }

	public string ExtPropDateTime2_Title { get; set; }

	public string ExtPropDateTime2_Desc { get; set; }

	public string ExtPropDateTime2_Configuration { get; set; }

	public DateTime? ExtPropDateTime3 { get; set; }

	public string ExtPropDateTime3_Title { get; set; }

	public string ExtPropDateTime3_Desc { get; set; }

	public string ExtPropDateTime3_Configuration { get; set; }

	public string ExtPropJSon1 { get; set; }

	public string ExtPropJSon1_Title { get; set; }

	public string ExtPropJSon1_Desc { get; set; }

	public string ExtPropJSon1_Configuration { get; set; }

	public string ExtPropJSon2 { get; set; }

	public string ExtPropJSon2_Title { get; set; }

	public string ExtPropJSon2_Desc { get; set; }

	public string ExtPropJSon2_Configuration { get; set; }

	public string ExtPropJSon3 { get; set; }

	public string ExtPropJSon3_Title { get; set; }

	public string ExtPropJSon3_Desc { get; set; }

	public string ExtPropJSon3_Configuration { get; set; }

	public string ExtendedPropsGroup1_JSonConfiguration { get; set; }

	public string ExtendedPropsGroup2_JSonConfiguration { get; set; }

	[NotMapped]
	public virtual IncidentProperties IncidentProperties { get; set; }

	public long? Category1Id { get; set; }

	public virtual RepoDynamicalDataTypesAndOptions Category1 { get; set; }

	public long? Category2Id { get; set; }

	public virtual RepoDynamicalDataTypesAndOptions Category2 { get; set; }

	public long? Category3Id { get; set; }

	public virtual RepoDynamicalDataTypesAndOptions Category3 { get; set; }

	public long? Category4Id { get; set; }

	public virtual RepoDynamicalDataTypesAndOptions Category4 { get; set; }

	public long? Category5Id { get; set; }

	public virtual RepoDynamicalDataTypesAndOptions Category5 { get; set; }

	public virtual List<RepoIncidentComment> IncidentCommentsList { get; set; }

	public virtual List<RepoIncidentCall> IncidentCallsList { get; set; }

	public virtual List<RepoIncidentSequence> SequencesList { get; set; }

	public virtual List<RepoIncidentActivityLog> ActivityLogsList { get; set; }

	public IEnumerable<LocalizableDescriptionWrapper> GetChildEntries()
	{
		List<LocalizableDescriptionWrapper> list = new List<LocalizableDescriptionWrapper>();
		list.Add(new LocalizableDescriptionWrapper(Constants.Translations.TranslationPrefix + "NewDescription", new List<LocalizableDescriptionParameter>
		{
			new LocalizableDescriptionParameter("Description", Description.GetType(), Description.Truncate())
		}));
		if (Type != null)
		{
			list.Add(new LocalizableDescriptionWrapper(Constants.Translations.TranslationPrefix + "NewTypeTitle", new List<LocalizableDescriptionParameter>
			{
				new LocalizableDescriptionParameter("Title", Type.Title.GetType(), Type.Title)
			}));
			if (Type.Description != null)
			{
				list.Add(new LocalizableDescriptionWrapper(Constants.Translations.TranslationPrefix + "NewTypeDescription", new List<LocalizableDescriptionParameter>
				{
					new LocalizableDescriptionParameter("Description", Type.Description.GetType(), Type.Description.Truncate())
				}));
			}
		}
		if (Status != null)
		{
			list.Add(new LocalizableDescriptionWrapper(Constants.Translations.TranslationPrefix + "NewStatusTitle", new List<LocalizableDescriptionParameter>
			{
				new LocalizableDescriptionParameter("Title", Status.Title.GetType(), Status.Title)
			}));
			if (Status.Description != null)
			{
				list.Add(new LocalizableDescriptionWrapper(Constants.Translations.TranslationPrefix + "NewStatusDescription", new List<LocalizableDescriptionParameter>
				{
					new LocalizableDescriptionParameter("Description", Status.Description.GetType(), Status.Description.Truncate())
				}));
			}
		}
		return list;
	}

	public IEnumerable<LocalizableDescriptionWrapper> GetChildEntries(IRepoBaseEntity previousValues)
	{
		if (!(previousValues is RepoIncident repoIncident))
		{
			return GetChildEntries();
		}
		List<LocalizableDescriptionWrapper> list = new List<LocalizableDescriptionWrapper>();
		if (!object.Equals(repoIncident.Description, Description))
		{
			list.Add(new LocalizableDescriptionWrapper(Constants.Translations.TranslationPrefix + "NewDescription", new List<LocalizableDescriptionParameter>
			{
				new LocalizableDescriptionParameter("Description", Description.GetType(), Description.Truncate())
			}));
		}
		if (Type != null && !object.Equals(repoIncident.TypeId, TypeId))
		{
			list.Add(new LocalizableDescriptionWrapper(Constants.Translations.TranslationPrefix + "NewTypeTitle", new List<LocalizableDescriptionParameter>
			{
				new LocalizableDescriptionParameter("Title", Type.Title.GetType(), Type.Title)
			}));
			if (Type.Description != null)
			{
				list.Add(new LocalizableDescriptionWrapper(Constants.Translations.TranslationPrefix + "NewTypeDescription", new List<LocalizableDescriptionParameter>
				{
					new LocalizableDescriptionParameter("Description", Type.Description.GetType(), Type.Description.Truncate())
				}));
			}
		}
		if (Status != null && !object.Equals(repoIncident.StatusId, StatusId))
		{
			list.Add(new LocalizableDescriptionWrapper(Constants.Translations.TranslationPrefix + "NewStatusTitle", new List<LocalizableDescriptionParameter>
			{
				new LocalizableDescriptionParameter("Title", Status.Title.GetType(), Status.Title)
			}));
			if (Status.Description != null)
			{
				list.Add(new LocalizableDescriptionWrapper(Constants.Translations.TranslationPrefix + "NewStatusDescription", new List<LocalizableDescriptionParameter>
				{
					new LocalizableDescriptionParameter("Description", Status.Description.GetType(), Status.Description.Truncate())
				}));
			}
		}
		if (IncidentProperties == null)
		{
			return list;
		}
		GetEntryFromDynamicProperty(list, IncidentProperties.Prop_Long1, repoIncident.ExtPropLong1, ExtPropLong1, ExtPropLong1.ToString(CultureInfo.InvariantCulture));
		GetEntryFromDynamicProperty(list, IncidentProperties.Prop_Long2, repoIncident.ExtPropLong2, ExtPropLong2, ExtPropLong2.ToString(CultureInfo.InvariantCulture));
		GetEntryFromDynamicProperty(list, IncidentProperties.Prop_Long3, repoIncident.ExtPropLong3, ExtPropLong3, ExtPropLong3.ToString(CultureInfo.InvariantCulture));
		GetEntryFromDynamicProperty(list, IncidentProperties.Prop_String1, repoIncident.ExtPropString1, ExtPropString1, ExtPropString1);
		GetEntryFromDynamicProperty(list, IncidentProperties.Prop_String2, repoIncident.ExtPropString2, ExtPropString2, ExtPropString2);
		GetEntryFromDynamicProperty(list, IncidentProperties.Prop_String3, repoIncident.ExtPropString3, ExtPropString3, ExtPropString3);
		GetEntryFromDynamicProperty(list, IncidentProperties.Prop_Bool1, repoIncident.ExtPropBool1, ExtPropBool1, ExtPropBool1.ToString(CultureInfo.InvariantCulture));
		GetEntryFromDynamicProperty(list, IncidentProperties.Prop_Bool2, repoIncident.ExtPropBool2, ExtPropBool2, ExtPropBool2.ToString(CultureInfo.InvariantCulture));
		GetEntryFromDynamicProperty(list, IncidentProperties.Prop_Bool3, repoIncident.ExtPropBool3, ExtPropBool3, ExtPropBool3.ToString(CultureInfo.InvariantCulture));
		GetEntryFromDynamicProperty(list, IncidentProperties.Prop_Decimal1, repoIncident.ExtPropDecimal1, ExtPropDecimal1, ExtPropDecimal1.ToString(CultureInfo.InvariantCulture));
		GetEntryFromDynamicProperty(list, IncidentProperties.Prop_Decimal2, repoIncident.ExtPropDecimal2, ExtPropDecimal2, ExtPropDecimal2.ToString(CultureInfo.InvariantCulture));
		GetEntryFromDynamicProperty(list, IncidentProperties.Prop_Decimal3, repoIncident.ExtPropDecimal3, ExtPropDecimal3, ExtPropDecimal3.ToString(CultureInfo.InvariantCulture));
		GetEntryFromDynamicProperty(list, IncidentProperties.Prop_DateTime1, repoIncident.ExtPropDateTime1, ExtPropDateTime1, ExtPropDateTime1?.ToString(CultureInfo.InvariantCulture) ?? string.Empty);
		GetEntryFromDynamicProperty(list, IncidentProperties.Prop_DateTime2, repoIncident.ExtPropDateTime2, ExtPropDateTime2, ExtPropDateTime2?.ToString(CultureInfo.InvariantCulture) ?? string.Empty);
		GetEntryFromDynamicProperty(list, IncidentProperties.Prop_DateTime3, repoIncident.ExtPropDateTime3, ExtPropDateTime3, ExtPropDateTime3?.ToString(CultureInfo.InvariantCulture) ?? string.Empty);
		GetEntryFromDynamicProperty<long?, string>(list, IncidentProperties.DropDownProp1, repoIncident.Category1Id, Category1Id, Category1?.Title ?? string.Empty);
		GetEntryFromDynamicProperty<long?, string>(list, IncidentProperties.DropDownProp2, repoIncident.Category2Id, Category2Id, Category2?.Title ?? string.Empty);
		GetEntryFromDynamicProperty<long?, string>(list, IncidentProperties.DropDownProp3, repoIncident.Category3Id, Category3Id, Category3?.Title ?? string.Empty);
		GetEntryFromDynamicProperty<long?, string>(list, IncidentProperties.DropDownProp4, repoIncident.Category4Id, Category4Id, Category4?.Title ?? string.Empty);
		GetEntryFromDynamicProperty<long?, string>(list, IncidentProperties.DropDownProp5, repoIncident.Category5Id, Category5Id, Category5?.Title ?? string.Empty);
		return list;
	}

	private void GetEntryFromDynamicProperty<T>(List<LocalizableDescriptionWrapper> entries, IncidentProperties.IncidentProperty incidentProperty, T oldValue, T newValue, string newValueString)
	{
		GetEntryFromDynamicProperty<T, T>(entries, incidentProperty, oldValue, newValue, newValueString);
	}

	private void GetEntryFromDynamicProperty<TValues, TCasting>(List<LocalizableDescriptionWrapper> entries, IncidentProperties.IncidentProperty incidentProperty, TValues oldValue, TValues newValue, string newValueString)
	{
		if (incidentProperty != null && incidentProperty.IsOn && !object.Equals(oldValue, newValue))
		{
			entries.Add(new LocalizableDescriptionWrapper(Constants.Translations.TranslationPrefix + "NewDynamicData", new List<LocalizableDescriptionParameter>
			{
				new LocalizableDescriptionParameter("Key", incidentProperty.Title.GetType(), incidentProperty.Title),
				new LocalizableDescriptionParameter("Value", typeof(TCasting), newValueString)
			}));
		}
	}
}
