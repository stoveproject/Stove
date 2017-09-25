using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Stove.Domain.Entities;

namespace Stove.Tests.SampleApplication.Domain.Entities
{
	[Table("Category")]
	public class Category : Entity
	{
		protected Category()
		{
		}

		public Category(string name) : this()
		{
			Name = name;
		}

		[Required]
		public virtual string Name { get; protected set; }
	}
}
