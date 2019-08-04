using System;
using System.Collections.Generic;
using AutoMapper;
using JetBrains.Annotations;

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
            foreach (TypeMap map in allTypeMaps)
            {
                var propertyMaps = map.PropertyMaps;

                foreach (PropertyMap propertyMap in propertyMaps)
                {
                    string destinationName = propertyMap.DestinationName;
                    string modelMemberName = propertyMap.SourceMember?.Name;

                    (Type, string) key = (map.DestinationType, destinationName);
                    _mappings.Add(key, modelMemberName);
                }
            }
        }

        public string Resolve(Type type, string propertyName)
        {
            (Type, string) key = (type, propertyName);

            // Not found in dictionary, just return propertyName or null.
            return _mappings.ContainsKey(key) ? _mappings[key] : propertyName;
        }
    }
}