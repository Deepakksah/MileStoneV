using System;

namespace VideoOS.IM.Server.Logger;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public sealed class PureAttribute : Attribute
{
}
