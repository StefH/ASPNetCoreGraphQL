using GraphQL.Types;
using System;
using System.Linq;

namespace MyHotel.Extensions
{
    public static class TypeExtensions
    {
        public static Type GraphType(this Type type)
        {
            return type.BaseType == typeof(NonNullGraphType) ? type.GetGenericArguments().First() : type;
        }

        public static bool IsObjectGraphType(this Type type)
        {
            return type.BaseType != null && type.BaseType.Name == "ObjectGraphType`1";
        }

        public static Type ModelType(this Type type)
        {
            return type?.GraphType().BaseType?.GetGenericArguments().First();
        }
    }
}