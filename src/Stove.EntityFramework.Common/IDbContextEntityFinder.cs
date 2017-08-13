using System;
using System.Collections.Generic;

using Stove.Domain.Entities;

namespace Stove.EntityFramework
{
	public interface IDbContextEntityFinder
	{
		IEnumerable<EntityTypeInfo> GetEntityTypeInfos(Type dbContextType);
	}
}
