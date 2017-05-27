using System.Collections.Generic;

namespace Stove.Data
{
    public class ActiveTransactionProviderArgs : Dictionary<string, object>
    {
        public static ActiveTransactionProviderArgs Empty = new ActiveTransactionProviderArgs();
    }
}
