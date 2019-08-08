using System;

namespace MyHotel.AutoMapper
{
    public interface IPropertyPathResolver
    {
        string Resolve(Type sourceType, string sourcepropertyPath, Type destinationType);
    }
}