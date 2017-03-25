using System.Collections.Generic;

namespace Stove.Transactions
{
    public class ActiveTransactionProviderArgs : Dictionary<string, object>
    {
        public static ActiveTransactionProviderArgs Empty = new ActiveTransactionProviderArgs();
    }
}
