using System.Collections.Generic;

using Stove.Json;

namespace Stove
{
    public class Headers : Dictionary<string, object>
    {
        public string GetCorrelationId()
        {
            return ContainsKey(StoveConsts.Events.CorrelationId)
                ? this[StoveConsts.Events.CorrelationId].ToString()
                : string.Empty;
        }

        public string GetCausationId()
        {
            return ContainsKey(StoveConsts.Events.CausationId)
                ? this[StoveConsts.Events.CausationId].ToString()
                : string.Empty;
        }

        public string GetUserId()
        {
            return ContainsKey(StoveConsts.Events.UserId)
                ? this[StoveConsts.Events.UserId].ToString()
                : string.Empty;
        }

        public string GetSourceType()
        {
            return ContainsKey(StoveConsts.Events.SourceType)
                ? this[StoveConsts.Events.SourceType].ToString()
                : string.Empty;
        }

        public string GetQualifiedName()
        {
            return ContainsKey(StoveConsts.Events.QualifiedName)
                ? this[StoveConsts.Events.QualifiedName].ToString()
                : string.Empty;
        }

        public string GetAggregateId()
        {
            return ContainsKey(StoveConsts.Events.AggregateId)
                ? this[StoveConsts.Events.AggregateId].ToString()
                : string.Empty;
        }

        public string Get(string key)
        {
            return ContainsKey(key)
                ? this[key].ToString()
                : string.Empty;
        }

        public override string ToString()
        {
            return this.ToJsonString(true, true);
        }
    }
}
