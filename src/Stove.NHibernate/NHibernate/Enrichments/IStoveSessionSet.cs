namespace Stove.NHibernate.Enrichments
{
    public interface IStoveSessionSet<T> where T : class
    {
        /// <summary>
        ///     Not used and not implemented anywhere.
        /// </summary>
        T Self { get; }
    }
}
