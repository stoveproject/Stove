using System.Data.Common;
using System.Data.Entity;

using Stove.EntityFramework.EntityFramework;
using Stove.Tests.SampleApplication.Domain.Entities;

namespace Stove.Tests.SampleApplication.Domain
{
    public class SampleApplicationDbContext : StoveDbContext
    {
        public SampleApplicationDbContext()
        {
        }

        public SampleApplicationDbContext(DbConnection connection)
            : base(connection, false)
        {
        }

        public virtual IDbSet<Product> Products { get; set; }

        public virtual IDbSet<User> Users { get; set; }

        public virtual IDbSet<Order> Orders { get; set; }

        public virtual IDbSet<OrderDetail> OrderDetails { get; set; }

        public virtual IDbSet<ProductDetail> ProductDetails { get; set; }
    }
}
