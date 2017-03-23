using System.Data;

namespace Stove.Orm
{
    public interface IActiveTransactionOrConnectionProvider
    {
        /// <summary>
        ///     Gets the active transaction.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        IDbTransaction GetActiveTransaction(ActiveTransactionOrConnectionProviderArgs args);

        /// <summary>
        ///     Gets the active connection.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        IDbConnection GetActiveConnection(ActiveTransactionOrConnectionProviderArgs args);
    }
}
