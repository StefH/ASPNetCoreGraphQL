using System;

namespace MyHotel.AutoMapper
{
    public interface IAutoMapperPropertyNameResolver
    {
        string Resolve(Type type, string propertyName);
    }
}