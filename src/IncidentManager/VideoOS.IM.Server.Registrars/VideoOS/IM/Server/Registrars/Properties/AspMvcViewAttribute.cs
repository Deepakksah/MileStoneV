using System;

namespace VideoOS.IM.Server.Registrars.Properties;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter)]
public sealed class AspMvcViewAttribute : PathReferenceAttribute
{
}
