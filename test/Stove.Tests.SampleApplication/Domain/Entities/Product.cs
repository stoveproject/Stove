using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using JetBrains.Annotations;

using Stove.Domain.Entities;

namespace Stove.Tests.SampleApplication.Domain.Entities
{
	[Table("Product")]
	public class Product : Entity
	{
		private ICollection<ProductCategory> _productCategories;

		protected Product()
		{
		}

		public Product([NotNull] string name) : this()
		{
			Name = name;
		}

		[NotNull]
		public virtual string Name { get; protected set; }

		[ForeignKey("ProductId")]
		public virtual ICollection<ProductCategory> ProductCategories
		{
			get => _productCategories ?? (_productCategories = new List<ProductCategory>());
			set => _productCategories = value;
		}

		public void AddCategory(string categoryName)
		{
			ProductCategories.Add(new ProductCategory(new Category(categoryName), this));
		}
	}
}
