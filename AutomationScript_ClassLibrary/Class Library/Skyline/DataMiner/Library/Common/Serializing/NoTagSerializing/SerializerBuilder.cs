namespace Skyline.DataMiner.Library.Common.Serializing.NoTagSerializing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class SerializerBuilder
    {
        private readonly List<Type> overrides = new List<Type>();
        private Type baseType;

        public ISerializer Build()
        {
            return overrides.Any() ? BuildWithOverrides() : BuildWithoutOverrides();
        }

        public SerializerBuilder WithBaseType(Type t)
        {
            baseType = t;
            return this;
        }

        public SerializerBuilder WithPossibleTypes(params Type[] possibleTypes)
        {
            overrides.AddRange(possibleTypes);
            return this;
        }

        public SerializerBuilder WithSerializer(XmlSerializerType type)
        {
            return this;
        }

        private ISerializer BuildWithoutOverrides()
        {
            return baseType != null ? new UsingJsonNewtonSoft.Serializer(baseType) : new UsingJsonNewtonSoft.Serializer();
        }

        private ISerializer BuildWithOverrides()
        {
            return baseType != null ? new UsingJsonNewtonSoft.Serializer(baseType, overrides) : new UsingJsonNewtonSoft.Serializer(overrides);
        }
    }
}