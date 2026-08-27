using System;

namespace VideoOS.IM.Server.Registrars.Properties;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Delegate, AllowMultiple = false, Inherited = true)]
public sealed class CanBeNullAttribute : Attribute
{
}
