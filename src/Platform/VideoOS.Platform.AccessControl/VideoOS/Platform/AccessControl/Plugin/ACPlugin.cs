using System;
using System.Collections.Generic;
using System.Drawing;

namespace VideoOS.Platform.AccessControl.Plugin;

public abstract class ACPlugin
{
	public abstract Guid Id { get; }

	public abstract string Name { get; }

	public abstract Image Icon { get; }

	public abstract Version Version { get; }

	public abstract string VersionText { get; }

	public abstract void Init();

	public abstract void Close();

	public abstract IEnumerable<ACIconInfo> GetIcons();

	public abstract IEnumerable<ACCategoryInfo> GetCategories();

	public abstract IEnumerable<ACPropertyDefinition> GetPropertyDefinitions();

	public abstract ACSystem GenerateAccessControlSystem();

	public abstract ACPropertyValidationResult ValidateProperties(IEnumerable<ACProperty> properties);
}
