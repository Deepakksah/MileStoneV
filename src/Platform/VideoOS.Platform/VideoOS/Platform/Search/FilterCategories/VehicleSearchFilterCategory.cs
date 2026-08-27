using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Media;
using VideoOS.Platform.Resources;
using VideoOS.Platform.Search.FilterConfigurations;
using VideoOS.Platform.Search.FilterValues;
using VideoOS.Platform.Search.FilterValues.Standard;
using VideoOS.Platform.Search.SearchFilters;
using VideoOS.Platform.UI.Controls;

namespace VideoOS.Platform.Search.FilterCategories;

public sealed class VehicleSearchFilterCategory : SearchFilterCategory
{
	public class VehicleLicensePlateSearchFilter : SearchFilter
	{
		private static Guid _id = new Guid("{B088FCA5-DA9C-4064-9649-64C15E30A91B}");

		public static SearchFilter Instance { get; } = new VehicleLicensePlateSearchFilter();

		private VehicleLicensePlateSearchFilter()
		{
			base.Id = _id;
			base.Name = LicensePlateSearchFilterName;
			base.Description = Strings.VehicleLicensePlateSearchFilter_Description;
		}

		public override FilterConfigurationBase GetFilterConfiguration()
		{
			return new TextBoxFilterConfiguration
			{
				Watermark = Strings.VehicleLicensePlateSearchFilter_Watermark,
				MaxLength = 1000
			};
		}

		public override FilterValueBase CreateValue()
		{
			StringFilterValue stringFilterValue = new StringFilterValue();
			ResetValue(stringFilterValue);
			return stringFilterValue;
		}

		public override void ResetValue(FilterValueBase value)
		{
			((value as StringFilterValue) ?? throw new ArgumentException("Does not match expected type: " + typeof(StringFilterValue).Name, "value")).Text = string.Empty;
		}
	}

	[Flags]
	public enum StandardSearchFilters
	{
		None = 0,
		Color = 1,
		LicensePlate = 2
	}

	private static Guid VehicleColorSearchFilterId = new Guid("{8FC6626A-47E6-41D8-954A-ED50900F5312}");

	public static IDictionary<Guid, (string DisplayName, System.Windows.Media.Color Value)> VehicleColorSearchFilterColors = new Dictionary<Guid, (string, System.Windows.Media.Color)>
	{
		{
			new Guid("B200144B-9774-4661-B89E-3FC52BB99501"),
			(Strings.VehicleColorSearchFilter_ColorWhite, Colors.White)
		},
		{
			new Guid("B200144B-9774-4661-B89E-3FC52BB99502"),
			(Strings.VehicleColorSearchFilter_ColorBeige, Colors.Beige)
		},
		{
			new Guid("B200144B-9774-4661-B89E-3FC52BB99503"),
			(Strings.VehicleColorSearchFilter_ColorYellow, Colors.Yellow)
		},
		{
			new Guid("B200144B-9774-4661-B89E-3FC52BB99504"),
			(Strings.VehicleColorSearchFilter_ColorRed, Colors.Red)
		},
		{
			new Guid("B200144B-9774-4661-B89E-3FC52BB99505"),
			(Strings.VehicleColorSearchFilter_ColorBlue, Colors.Blue)
		},
		{
			new Guid("B200144B-9774-4661-B89E-3FC52BB99506"),
			(Strings.VehicleColorSearchFilter_ColorGreen, Colors.Green)
		},
		{
			new Guid("B200144B-9774-4661-B89E-3FC52BB99507"),
			(Strings.VehicleColorSearchFilter_ColorBlack, Colors.Black)
		},
		{
			new Guid("B200144B-9774-4661-B89E-3FC52BB99508"),
			(Strings.VehicleColorSearchFilter_ColorGrey, Colors.Gray)
		}
	};

	private static ColorSearchFilter VehicleColorSearchFilter;

	public static string ColorSearchFilterName => Strings.VehicleColorSearchFilter_Name;

	public static string LicensePlateSearchFilterName => Strings.VehicleLicensePlateSearchFilter_Name;

	[Obsolete("This constructor is deprecated. Windows Forms controls are no longer supported. Use VehicleSearchFilterCategory(string, VideoOSIconSourceBase, StandardSearchFilters, IEnumerable<SearchFilter>) instead.", false)]
	public VehicleSearchFilterCategory(string name, Icon icon, StandardSearchFilters standardSearchFilters, IEnumerable<SearchFilter> customSearchFilters)
		: base(name, icon, ResolveSearchFilters(standardSearchFilters, customSearchFilters))
	{
	}

	public VehicleSearchFilterCategory(string name, VideoOSIconSourceBase iconSource, StandardSearchFilters standardSearchFilters, IEnumerable<SearchFilter> customSearchFilters)
		: base(name, iconSource, ResolveSearchFilters(standardSearchFilters, customSearchFilters))
	{
	}

	private static IEnumerable<SearchFilter> ResolveSearchFilters(StandardSearchFilters standardSearchFilters, IEnumerable<SearchFilter> customSearchFilters)
	{
		List<SearchFilter> list = new List<SearchFilter>();
		if (standardSearchFilters.HasFlag(StandardSearchFilters.Color))
		{
			if (VehicleColorSearchFilter == null)
			{
				VehicleColorSearchFilter = new ColorSearchFilter(VehicleColorSearchFilterId, ColorSearchFilterName, VehicleColorSearchFilterColors);
			}
			list.Add(VehicleColorSearchFilter);
		}
		if (standardSearchFilters.HasFlag(StandardSearchFilters.LicensePlate))
		{
			list.Add(VehicleLicensePlateSearchFilter.Instance);
		}
		if (customSearchFilters != null)
		{
			list.AddRange(customSearchFilters);
		}
		return list;
	}

	public static IReadOnlyCollection<VehicleColorFilterValue> GetColorFilterValues(SearchCriteria searchCriteria)
	{
		if (searchCriteria == null)
		{
			throw new ArgumentNullException("searchCriteria");
		}
		return (from value in searchCriteria.GetFilterValues(VehicleColorSearchFilter)
			select new VehicleColorFilterValue
			{
				Colors = ((SelectionFilterValue)value).SelectedIds.Select(VehicleColorSearchFilter.GetItemValue)
			}).ToList().AsReadOnly();
	}

	public static IReadOnlyCollection<VehicleLicensePlateFilterValue> GetLicensePlateFilterValue(SearchCriteria searchCriteria)
	{
		if (searchCriteria == null)
		{
			throw new ArgumentNullException("searchCriteria");
		}
		return (from value in searchCriteria.GetFilterValues(VehicleLicensePlateSearchFilter.Instance)
			select new VehicleLicensePlateFilterValue
			{
				LicensePlate = ((StringFilterValue)value).Text
			}).ToList().AsReadOnly();
	}
}
