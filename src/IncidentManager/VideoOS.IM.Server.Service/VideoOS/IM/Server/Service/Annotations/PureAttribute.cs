using System;

namespace VideoOS.IM.Server.Service.Annotations;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public sealed class PureAttribute : Attribute
{
}
