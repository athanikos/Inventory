namespace Rooms.API
{
    public static  class ApiEndpoints
    {

        private const string ApiBase = "";

        public static class Inventories
        {
            private const string Base = $"{ApiBase}/inventories";

            public const string Create = Base;
            public const string Get = $"{Base}/{{id}}";
            public const string GetAll = $"{Base}";
            public const string Put = $"{Base}/{{id}}";
        }

        public static class Rooms
        {
            private const string Base = $"{ApiBase}/rooms";
            public const string Create = Base;
            public const string Get = $"{Base}/{{id}}";
            public const string GetAll = $"{Base}";
            public const string Put = $"{Base}/{{id}}";
        }

        public static class Customers
        {
            private const string Base = $"{ApiBase}/customers";

            public const string Create = Base;
            public const string Get = $"{ApiBase}/{{id}}";
            public const string GetAll = $"{ApiBase}";
            public const string Put = $"{ApiBase}/{{id}}";
        }

        public static class Bookings
        {
            private const string Base = $"{ApiBase}/bookings";

            public const string Create = Base;
            public const string Get = $"{ApiBase}/{{id}}";
            public const string GetAll = $"{ApiBase}";
            public const string Put = $"{ApiBase}/{{id}}";
        }



    }
}
