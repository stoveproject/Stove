using Stove.Domain.Entities;

namespace Stove.Domain.Uow
{
    /// <summary>
    ///     Standard filters of Stove.
    /// </summary>
    public static class StoveDataFilters
    {
        /// <summary>
        ///     "SoftDelete".
        ///     Soft delete filter.
        ///     Prevents getting deleted data from database.
        ///     See <see cref="ISoftDelete" /> interface.
        /// </summary>
        public const string SoftDelete = "SoftDelete";
    }
}
