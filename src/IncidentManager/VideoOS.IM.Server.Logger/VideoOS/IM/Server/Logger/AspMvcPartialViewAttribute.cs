using System;

namespace VideoOS.IM.Server.Logger;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter)]
public sealed class AspMvcPartialViewAttribute : PathReferenceAttribute
{
}
