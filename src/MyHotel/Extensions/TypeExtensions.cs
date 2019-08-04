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

        public static bool TryGetObjectGraphType(this Type type, out Type objectGraphType)
        {
            objectGraphType = null;

            if (type.BaseType != null && type.BaseType.Name == "ObjectGraphType`1")
            {
                objectGraphType = type;
                return true;
            }

            return false;
        }

        public static Type ModelType(this Type type)
        {
            return type?.GraphType().BaseType?.GetGenericArguments().First();
        }
    }
}