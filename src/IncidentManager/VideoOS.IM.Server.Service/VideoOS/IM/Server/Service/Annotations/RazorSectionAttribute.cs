using System;

namespace VideoOS.IM.Server.Service.Annotations;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter, Inherited = true)]
public sealed class RazorSectionAttribute : Attribute
{
}
