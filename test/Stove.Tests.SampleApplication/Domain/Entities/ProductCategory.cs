using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Stove.Domain.Entities;

namespace Stove.Tests.SampleApplication.Domain.Entities
{
	[Table("ProductCategory")]
	public class ProductCategory : Entity
	{
		protected ProductCategory()
		{
		}

		public ProductCategory(Category category, Product product) : this()
		{
			Category = category;
			Product = product;
		}

		[Required]
		public virtual Category Category { get; protected set; }

		public virtual int CategoryId { get; protected set; }

		[Required]
		public virtual Product Product { get; protected set; }

		public virtual int ProductId { get; protected set; }
	}
}
