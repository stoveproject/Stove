using System;

using Stove.Extensions;
using Stove.Timing;

namespace Stove.Domain.Entities.Auditing
{
	public static class EntityAuditingHelper
	{
		public static void SetCreationAuditProperties(object entityAsObj, long? userId)
		{
		    if (!(entityAsObj is IHasCreationTime entityWithCreationTime))
			{
				return;
			}

			if (entityWithCreationTime.CreationTime == default)
			{
				entityWithCreationTime.CreationTime = Clock.Now;
			}

			if (userId.HasValue && entityAsObj is ICreationAudited)
			{
				var entity = entityAsObj as ICreationAudited;
				if (entity.CreatorUserId == null)
				{
					entity.CreatorUserId = userId;
				}
			}
		}

		public static void SetModificationAuditProperties(object entityAsObj, long? userId)
		{
			if (entityAsObj is IHasModificationTime)
			{
				entityAsObj.As<IHasModificationTime>().LastModificationTime = Clock.Now;
			}

			if (entityAsObj is IModificationAudited)
			{
				var entity = entityAsObj.As<IModificationAudited>();

				if (userId == null)
				{
					entity.LastModifierUserId = null;
					return;
				}

				entity.LastModifierUserId = userId;
			}
		}
	}
}
