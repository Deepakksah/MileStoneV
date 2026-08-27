using System;

namespace VideoOS.UI.Common.WPF.Annotations;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter, Inherited = true)]
public sealed class RazorSectionAttribute : Attribute
{
}
