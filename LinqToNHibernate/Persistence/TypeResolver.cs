using System;
using System.Collections.Generic;
using System.Linq;

namespace Persistence
{
    using System.Reflection;

    public class TypeResolver 
    {
        private readonly IEnumerable<Assembly> _sourceAssemblies;
        private readonly Dictionary<string, Type> _types;


        public TypeResolver(IEnumerable<Assembly> sourceAssemblies)
        {
            _sourceAssemblies = sourceAssemblies;
            _types = new Dictionary<string, Type>();
        }


        public Type ResolveType(string className)
        {
            if (!_types.ContainsKey(className))
            {
                Type requestedType = null;

                foreach (var assembly in _sourceAssemblies)
                {
                    requestedType = assembly.GetTypes()
                                            .FirstOrDefault(type => type.FullName == className);

                    if (requestedType != null)
                        break;
                }

                _types[className] = requestedType;
            }

            return _types[className];
        }
    }
}
