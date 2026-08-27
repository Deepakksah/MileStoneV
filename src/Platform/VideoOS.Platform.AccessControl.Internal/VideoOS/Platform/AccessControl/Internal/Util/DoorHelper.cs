using System.Collections.Generic;
using System.Linq;
using VideoOS.Platform.AccessControl.Internal.AccessControlElements;
using VideoOS.Platform.AccessControl.Internal.AccessControlSystem;

namespace VideoOS.Platform.AccessControl.Internal.Util;

public abstract class DoorHelper
{
	public static IEnumerable<AccessControlUnit> GetDoorUnits(AccessControlSystemManager accessControlSystemManager)
	{
		if (accessControlSystemManager == null)
		{
			return new List<AccessControlUnit>();
		}
		return accessControlSystemManager.GetAccessControlElements(CategoryHandler.CategoryUnitTypesDoor, typeof(AccessControlUnit)).Cast<AccessControlUnit>();
	}

	public static IEnumerable<AccessControlUnit> GetDoorUnits(IEnumerable<AccessControlSystemManager> accessControlSystemManagers)
	{
		if (accessControlSystemManagers == null)
		{
			return new List<AccessControlUnit>();
		}
		List<AccessControlUnit> list = new List<AccessControlUnit>();
		foreach (AccessControlSystemManager accessControlSystemManager in accessControlSystemManagers)
		{
			list.AddRange(GetDoorUnits(accessControlSystemManager));
		}
		return list;
	}

	public static bool IsDoorUnit(AccessControlUnit accessControlUnit)
	{
		if (accessControlUnit?.AccessControlType == null)
		{
			return false;
		}
		return accessControlUnit.AccessControlType.Categories.Intersect(CategoryHandler.CategoryUnitTypesDoor).Any();
	}

	public static bool IsAccessPointUnit(AccessControlUnit accessControlUnit)
	{
		if (accessControlUnit?.AccessControlType == null)
		{
			return false;
		}
		return accessControlUnit.AccessControlType.Categories.Intersect(CategoryHandler.CategoryUnitTypesAccessPoint).Any();
	}

	public static bool IsDoorOrDoorDescendantUnit(AccessControlUnit accessControlUnit)
	{
		return GetDoorAncestorUnit(accessControlUnit) != null;
	}

	public static AccessControlUnit GetDoorAncestorUnit(AccessControlUnit accessControlUnit)
	{
		while (accessControlUnit != null)
		{
			if (IsDoorUnit(accessControlUnit))
			{
				return accessControlUnit;
			}
			accessControlUnit = accessControlUnit.Parent;
		}
		return null;
	}

	public static IEnumerable<AccessControlUnit> GetAllDoorAccessPointUnits(AccessControlUnit accessControlUnit)
	{
		List<AccessControlUnit> list = new List<AccessControlUnit>();
		if (IsDoorUnit(accessControlUnit))
		{
			foreach (AccessControlUnit child in accessControlUnit.Children)
			{
				if (IsAccessPointUnit(child))
				{
					list.Add(child);
				}
			}
		}
		return list;
	}

	public static IEnumerable<AccessControlUnit> GetAllDoorDecendantUnits(AccessControlUnit accessControlUnit)
	{
		List<AccessControlUnit> list = new List<AccessControlUnit>();
		if (IsDoorUnit(accessControlUnit))
		{
			AddUnitAndChildsRecursive(accessControlUnit, list);
		}
		return list;
	}

	private static void AddUnitAndChildsRecursive(AccessControlUnit accessControlUnit, List<AccessControlUnit> accessControlUnits)
	{
		accessControlUnits.Add(accessControlUnit);
		if (accessControlUnit.Children == null)
		{
			return;
		}
		foreach (AccessControlUnit child in accessControlUnit.Children)
		{
			AddUnitAndChildsRecursive(child, accessControlUnits);
		}
	}

	public static IEnumerable<AccessControlUnit> GetDescendantDoorUnits(AccessControlUnit accessControlUnit)
	{
		if (IsDoorUnit(accessControlUnit))
		{
			yield return accessControlUnit;
			yield break;
		}
		foreach (AccessControlUnit child in accessControlUnit.Children)
		{
			foreach (AccessControlUnit descendantDoorUnit in GetDescendantDoorUnits(child))
			{
				yield return descendantDoorUnit;
			}
		}
	}
}
