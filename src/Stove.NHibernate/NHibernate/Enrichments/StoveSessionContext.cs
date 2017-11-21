using Autofac.Extras.IocManager;

using Stove.Domain.Entities;

namespace Stove.NHibernate.Enrichments
{
    /// <summary>
    ///     This class will be used for seperating the Databases.
    ///     We can assume that just like as EF's DbContext. Put your entities in here which are inherited from
    ///     <see cref="IEntity{TPrimaryKey}" /> with <see cref="IStoveSessionSet{T}" />.
    ///     Repository generation/registration will be completed automatically.
    /// </summary>
    public abstract class StoveSessionContext : ITransientDependency
    {
    }
}
