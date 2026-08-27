using System;

namespace VideoOS.IM.Common.Service.Properties;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Delegate, AllowMultiple = false, Inherited = true)]
public sealed class NotNullAttribute : Attribute
{
}
