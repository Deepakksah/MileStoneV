using System.Collections.Generic;
using System.Linq;
using VideoOS.LPR.Common.WCF.DataContracts;

namespace VideoOS.LPR.Common.MatchLists;

public static class CustomFieldsSorter
{
	public static void SortMatchListItemsCustomFieldsByDisplay(LicensePlateInfo[] matchListItems, IList<LicensePlatePropertyDefinition> matchListCustomFieldsOrderReference)
	{
		LicensePlatePropertyDefinition[] orderedProperties = matchListCustomFieldsOrderReference.OrderBy((LicensePlatePropertyDefinition y) => y.DisplayOrder).ToArray();
		for (int num = 0; num < matchListItems.Length; num++)
		{
			matchListItems[num].Properties = OrderCustomFields(matchListItems[num].Properties, orderedProperties);
		}
	}

	private static LicensePlateProperty[] OrderCustomFields(LicensePlateProperty[] propertiesToOrder, LicensePlatePropertyDefinition[] orderedProperties)
	{
		LicensePlateProperty[] array = new LicensePlateProperty[orderedProperties.Length];
		int j;
		for (j = 0; j < orderedProperties.Length; j++)
		{
			array[j] = propertiesToOrder.Single((LicensePlateProperty index) => index.ColumnIndex == orderedProperties[j].ColumnIndex);
		}
		return array;
	}
}
