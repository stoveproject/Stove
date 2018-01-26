namespace Stove
{
    public static class StoveConsts
    {
        public const string OrmRegistrarContextKey = "OrmRegistrars";

        public static class Orms
        {
            public const string Dapper = "Dapper";
            public const string EntityFramework = "EntityFramework";
            public const string EntityFrameworkCore = "EntityFrameworkCore";
            public const string NHibernate = "NHibernate";
        }

        public static class Events
        {
            public const string CausationId = "CausationId";
            public const string UserId = "UserId";
            public const string CorrelationId = "CorrelationId";
            public const string SourceType = "SourceType";
            public const string QualifiedName = "QualifiedName";
        }
    }
}
