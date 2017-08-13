using Stove.Domain.Entities;

namespace Stove.EntityFrameworkCore.Tests.Domain
{
	public class Ticket : Entity, IPassivable
	{
		public Ticket()
		{
			IsActive = true;
		}

		public virtual string EmailAddress { get; set; }

		public virtual string Message { get; set; }

		public virtual bool IsActive { get; set; }
	}
}
