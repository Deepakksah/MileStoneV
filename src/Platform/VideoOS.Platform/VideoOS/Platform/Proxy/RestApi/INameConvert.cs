using System.Collections.Generic;

namespace VideoOS.Platform.Proxy.RestApi;

public interface INameConvert
{
	List<string> UnderstoodItemTypes { get; }

	string GetRemoveMethodId(string[] methodIds, string itemType);

	string ConfigApiPathToResource(string path);

	string ConfigApiPathToResource(string configApiPath, bool full);

	ResourceEntry GetResourcePlural(string resourceName);

	string GetDisplayNameKey(string itemType);

	string ChildItemTypeToFieldName(string childItemType);

	string PropertyKeyToField(string itemType, string key, bool noCasingOfMIPItems = false);

	string PropertyKeyToField(string itemType, string key, string methodId);

	string PropertyFieldToKey(string itemType, string field, bool noCasing = false);

	string MethodIdToTask(string methodId);

	string FolderItemTypeToResourcePlural(string folderItemType);

	string ItemTypeToResourceSingular(string itemType);

	string ItemTypeToResourcePlural(string itemType);

	string ResourcePluralToItemType(string resourceName);

	string ResourcePluralToItemType(string resourceName, bool throwForInvalid);

	string FieldToPropertyKeyNameConvert(string itemType, string[] methodIds, string key);

	string ChildFieldNameToItemType(string childFieldName);

	string ResourcePluralToFolderItemType(string resourceName);

	bool SupportRootFolder(string itemType);

	string SupportRootFolderSuggest(string itemType);

	string GetAddMethodId(string[] methodIds, string itemType);

	string TaskToMethodId(string task);

	bool HasMultipleChildItems(string itemType);

	bool OnlyFourthLevelPostSupported(string itemType);
}
