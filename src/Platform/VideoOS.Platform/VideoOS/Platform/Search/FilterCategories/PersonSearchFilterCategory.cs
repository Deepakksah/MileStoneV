using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using VideoOS.Platform.Resources;
using VideoOS.Platform.Search.FilterConfigurations;
using VideoOS.Platform.Search.FilterValues;
using VideoOS.Platform.Search.FilterValues.Standard;
using VideoOS.Platform.UI.Controls;

namespace VideoOS.Platform.Search.FilterCategories;

public sealed class PersonSearchFilterCategory : SearchFilterCategory
{
	public class PersonAgeSearchFilter : SearchFilter
	{
		private static Guid _id = new Guid("{E8BC15D8-3C68-457D-9DE7-3CAE6BBD7DBB}");

		public static SearchFilter Instance { get; } = new PersonAgeSearchFilter();

		private PersonAgeSearchFilter()
		{
			base.Id = _id;
			base.Name = AgeSearchFilterName;
			base.Description = Strings.PersonAgeSearchFilter_Description;
		}

		public override FilterConfigurationBase GetFilterConfiguration()
		{
			return new RangeSliderFilterConfiguration
			{
				Minimum = 0.0,
				Maximum = 120.0,
				StepSize = 1.0
			};
		}

		public override FilterValueBase CreateValue()
		{
			DoubleRangeFilterValue doubleRangeFilterValue = new DoubleRangeFilterValue();
			ResetValue(doubleRangeFilterValue);
			return doubleRangeFilterValue;
		}

		public override void ResetValue(FilterValueBase value)
		{
			DoubleRangeFilterValue obj = (value as DoubleRangeFilterValue) ?? throw new ArgumentException("Does not match expected type: " + typeof(DoubleRangeFilterValue).Name, "value");
			obj.Low = 0.0;
			obj.High = 120.0;
		}
	}

	public class PersonGenderSearchFilter : SearchFilter
	{
		private static Guid _id;

		public static readonly List<Tuple<Guid, string, PersonGenderFilterValue.Gender>> Genders;

		public static SearchFilter Instance { get; }

		static PersonGenderSearchFilter()
		{
			_id = new Guid("{D646AFA7-9DA6-4815-AE20-7BC710C6D103}");
			Instance = new PersonGenderSearchFilter();
			Genders = new List<Tuple<Guid, string, PersonGenderFilterValue.Gender>>
			{
				new Tuple<Guid, string, PersonGenderFilterValue.Gender>(new Guid("E3AEF7EF-FC6F-4334-A308-C464FCB0A101"), Strings.PersonGenderSearchFilter_GenderFemale, PersonGenderFilterValue.Gender.Female),
				new Tuple<Guid, string, PersonGenderFilterValue.Gender>(new Guid("D831B56E-CAF0-4602-8BE6-884DFA566602"), Strings.PersonGenderSearchFilter_GenderMale, PersonGenderFilterValue.Gender.Male)
			};
			Genders.Sort(SortAlphabetically);
		}

		private PersonGenderSearchFilter()
		{
			base.Id = _id;
			base.Name = GenderSearchFilterName;
			base.Description = Strings.PersonGenderSearchFilter_Description;
		}

		private static int SortAlphabetically(Tuple<Guid, string, PersonGenderFilterValue.Gender> x, Tuple<Guid, string, PersonGenderFilterValue.Gender> y)
		{
			return string.Compare(x.Item2, y.Item2, StringComparison.CurrentCulture);
		}

		public override FilterConfigurationBase GetFilterConfiguration()
		{
			ListSelectionFilterConfiguration listSelectionFilterConfiguration = new ListSelectionFilterConfiguration();
			foreach (Tuple<Guid, string, PersonGenderFilterValue.Gender> gender in Genders)
			{
				listSelectionFilterConfiguration.Items[gender.Item1] = gender.Item2;
			}
			return listSelectionFilterConfiguration;
		}

		public override FilterValueBase CreateValue()
		{
			SelectionFilterValue selectionFilterValue = new SelectionFilterValue();
			ResetValue(selectionFilterValue);
			return selectionFilterValue;
		}

		public override void ResetValue(FilterValueBase value)
		{
			((value as SelectionFilterValue) ?? throw new ArgumentException("Does not match expected type: " + typeof(SelectionFilterValue).Name, "value")).SetSelectedIds(Enumerable.Empty<Guid>());
		}

		public static PersonGenderFilterValue.Gender ConvertToGender(Guid arg)
		{
			return Genders.First((Tuple<Guid, string, PersonGenderFilterValue.Gender> g) => object.Equals(g.Item1, arg)).Item3;
		}

		public override string GetDisplayValue(FilterValueBase value)
		{
			if (!(value is SelectionFilterValue selectionFilterValue))
			{
				throw new ArgumentException("Does not match expected type: " + typeof(SelectionFilterValue).Name, "value");
			}
			if (selectionFilterValue.SelectedIds.Count == 0)
			{
				return Strings.PersonGenderSearchFilter_GenderNotSelected;
			}
			return string.Join(", ", selectionFilterValue.SelectedIds.Select((Guid id) => Genders.First((Tuple<Guid, string, PersonGenderFilterValue.Gender> c) => c.Item1 == id).Item2));
		}
	}

	[Flags]
	public enum StandardSearchFilters
	{
		None = 0,
		Age = 1,
		Gender = 2
	}

	public static string AgeSearchFilterName => Strings.PersonAgeSearchFilter_Name;

	public static string GenderSearchFilterName => Strings.PersonGenderSearchFilter_Name;

	[Obsolete("This constructor is deprecated. Windows Forms controls are no longer supported. Use PersonSearchFilterCategory(string, VideoOSIconSourceBase, StandardSearchFilters, IEnumerable<SearchFilter>) instead.", false)]
	public PersonSearchFilterCategory(string name, Icon icon, StandardSearchFilters standardSearchFilters, IEnumerable<SearchFilter> customSearchFilters)
		: base(name, icon, ResolveSearchFilters(standardSearchFilters, customSearchFilters))
	{
	}

	public PersonSearchFilterCategory(string name, VideoOSIconSourceBase iconSource, StandardSearchFilters standardSearchFilters, IEnumerable<SearchFilter> customSearchFilters)
		: base(name, iconSource, ResolveSearchFilters(standardSearchFilters, customSearchFilters))
	{
	}

	private static IEnumerable<SearchFilter> ResolveSearchFilters(StandardSearchFilters standardSearchFilters, IEnumerable<SearchFilter> customSearchFilters)
	{
		List<SearchFilter> list = new List<SearchFilter>();
		if (standardSearchFilters.HasFlag(StandardSearchFilters.Age))
		{
			list.Add(PersonAgeSearchFilter.Instance);
		}
		if (standardSearchFilters.HasFlag(StandardSearchFilters.Gender))
		{
			list.Add(PersonGenderSearchFilter.Instance);
		}
		if (customSearchFilters != null)
		{
			list.AddRange(customSearchFilters);
		}
		return list;
	}

	public static IReadOnlyCollection<PersonAgeFilterValue> GetAgeFilterValues(SearchCriteria searchCriteria)
	{
		if (searchCriteria == null)
		{
			throw new ArgumentNullException("searchCriteria");
		}
		return (from age in searchCriteria.GetFilterValues(PersonAgeSearchFilter.Instance)
			select new PersonAgeFilterValue
			{
				MinimumAge = (int)((DoubleRangeFilterValue)age).Low,
				MaximumAge = (int)((DoubleRangeFilterValue)age).High
			}).ToList().AsReadOnly();
	}

	public static IReadOnlyCollection<PersonGenderFilterValue> GetGenderFilterValues(SearchCriteria searchCriteria)
	{
		if (searchCriteria == null)
		{
			throw new ArgumentNullException("searchCriteria");
		}
		return (from value in searchCriteria.GetFilterValues(PersonGenderSearchFilter.Instance)
			select new PersonGenderFilterValue
			{
				Genders = ((SelectionFilterValue)value).SelectedIds.Select(PersonGenderSearchFilter.ConvertToGender)
			}).ToList().AsReadOnly();
	}
}
