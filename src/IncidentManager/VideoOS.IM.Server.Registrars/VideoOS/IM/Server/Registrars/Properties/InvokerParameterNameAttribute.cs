using System;

namespace VideoOS.IM.Server.Registrars.Properties;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
public sealed class InvokerParameterNameAttribute : Attribute
{
}
