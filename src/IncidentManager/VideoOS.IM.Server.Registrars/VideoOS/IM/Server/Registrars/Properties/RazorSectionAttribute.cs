using System;

namespace VideoOS.IM.Server.Registrars.Properties;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter, Inherited = true)]
public sealed class RazorSectionAttribute : Attribute
{
}
