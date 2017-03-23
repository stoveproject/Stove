using System.Collections.Generic;

namespace Stove.Orm
{
    public class ActiveTransactionOrConnectionProviderArgs : Dictionary<string, object>
    {
        public static ActiveTransactionOrConnectionProviderArgs Empty = new ActiveTransactionOrConnectionProviderArgs();
    }
}
