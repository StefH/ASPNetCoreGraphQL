using AutoMapper;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyHotel.AutoMapper
{
    public class AutoMapperPropertyNameResolver : IAutoMapperPropertyNameResolver
    {
        private readonly IDictionary<(Type dtoType, string dtoProperty), string> _mappings = new Dictionary<(Type dtoType, string dtoProperty), string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoMapperPropertyNameResolver"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        public AutoMapperPropertyNameResolver([NotNull] IMapper mapper)
        {
            // Guard.NotNull(mapper, nameof(mapper));

            Init(mapper);
        }

        private void Init(IMapper mapper)
        {
            var allTypeMaps = mapper.ConfigurationProvider.GetAllTypeMaps();
            foreach (var typeMap in allTypeMaps)
            {
                foreach (var propertyMap in typeMap.PropertyMaps.Where(pm => pm.CustomMapExpression == null))
                {
                    string destinationPropertyName = propertyMap.DestinationName;
                    string sourcePath = propertyMap.SourceMember?.Name;

                    _mappings.Add((typeMap.DestinationType, destinationPropertyName), sourcePath);
                }

                foreach (var propertyMap in typeMap.PropertyMaps.Where(pm => pm.CustomMapExpression != null))
                {
                    string body = propertyMap.CustomMapExpression.Body.ToString();
                    string tag = propertyMap.CustomMapExpression.Parameters[0].Name;

                    string destinationPropertyName = propertyMap.DestinationName;
                    string sourcePath = body.Replace($"{tag}.", string.Empty);

                    _mappings.Add((typeMap.DestinationType, destinationPropertyName), sourcePath);
                }
            }
        }

        public string Resolve(Type type, string propertyName)
        {
            (Type, string) key = (type, propertyName);

            // Not found in dictionary, just return propertyName or null.
            return _mappings.ContainsKey((type, propertyName)) ? _mappings[key] : propertyName;
        }
    }
}