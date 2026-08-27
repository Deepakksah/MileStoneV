using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VideoOS.Common.Integration.Definition.Schema;

namespace VideoOS.Common.Integration.Definition;

public class CapabilitiesCollection : KeyedCollection<string, CapabilityDefinition>
{
	private MainDefinition mainDefinition;

	private capabilitiesType capabilities;

	public Hwid Hwid => Hwid.ParseHwid(capabilities.hwid);

	public CapabilitiesCollection(MainDefinition mainDefinition, capabilitiesType capabilities)
	{
		this.mainDefinition = mainDefinition;
		this.capabilities = capabilities;
		if (this.capabilities != null && this.capabilities.capability != null)
		{
			capabilityType[] capability = this.capabilities.capability;
			foreach (capabilityType capability2 in capability)
			{
				Add(new CapabilityDefinition(this.mainDefinition, this.capabilities, capability2));
			}
		}
	}

	protected override string GetKeyForItem(CapabilityDefinition item)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		return item.Name;
	}

	public CapabilityDefinition FindByTypeReferenceId(Guid referenceId)
	{
		if (capabilities == null)
		{
			return null;
		}
		string typeName = mainDefinition.Base.FindTypeNameByReferenceId(referenceId);
		capabilityType capabilityType2 = capabilities.FindCapabilityByTypeName(typeName);
		if (capabilityType2 == null)
		{
			return null;
		}
		try
		{
			return base[capabilityType2.name];
		}
		catch (KeyNotFoundException)
		{
			return null;
		}
	}

	public bool TryGetValue(string name, out CapabilityDefinition capabilityDefinition)
	{
		if (base.Dictionary == null)
		{
			capabilityDefinition = null;
			return false;
		}
		return base.Dictionary.TryGetValue(name, out capabilityDefinition);
	}
}
