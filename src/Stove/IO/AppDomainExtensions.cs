using System;

namespace Stove.IO
{
    public static class AppDomainExtensions
    {
        public static string GetActualDomainPath(this AppDomain @this)
        {
            return @this.RelativeSearchPath ?? @this.BaseDirectory;
        }
    }
}
