using System;
using System.Collections.Generic;

using Stove.Domain.Entities;

namespace Stove.EntityFramework.Common
{
	public interface IDbContextEntityFinder
	{
		IEnumerable<EntityTypeInfo> GetEntityTypeInfos(Type dbContextType);
	}
}
