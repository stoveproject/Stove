using System;
using System.Data;

namespace Stove.Orm
{
    public interface IActiveTransactionProvider
    {
        /// <summary>
        ///     Gets the active transaction.
        /// </summary>
        /// <param name="dbContextType">Type of the database context.</param>
        /// <returns></returns>
        IDbTransaction GetActiveTransaction(Type dbContextType = null);

        /// <summary>
        ///     Gets the active connection.
        /// </summary>
        /// <param name="dbContextType">Type of the database context.</param>
        /// <returns></returns>
        IDbConnection GetActiveConnection(Type dbContextType = null);
    }
}
