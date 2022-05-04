namespace Skyline.DataMiner.Library.Common.Serializing.NoTagSerializing.UsingJsonNewtonSoft
{
    using Newtonsoft.Json;

    using Skyline.DataMiner.Library.Common.Attributes;

    using System;
    using System.Collections.Generic;

    [DllImport("Newtonsoft.Json.dll")]
    internal class Serializer : ISerializer
    {
        public Serializer()
        {
            KnownTypes = new KnownTypesBinder();
            ApplySettings();
        }

        public Serializer(List<Type> knownTypes)
        {
            KnownTypes = new KnownTypesBinder(knownTypes);
            ApplySettings();
        }

        public Serializer(Type rootType, List<Type> knownTypes = null)
        {
            RootType = rootType;
            KnownTypes = knownTypes != null ? new KnownTypesBinder(knownTypes) : new KnownTypesBinder();

			ApplySettings();
        }

        public KnownTypesBinder KnownTypes { get; private set; }

        public Type RootType { get; private set; }

        public JsonSerializerSettings Settings { get; private set; }

        public T DeserializeFromString<T>(string input)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(input, Settings);
            }
            catch (Exception e)
            {
                throw new ArgumentException("Cannot Json Deserialize: " + input, "input", e);
            }
        }

        public string SerializeToString(object input)
        {
            try
            {
				return RootType != null ? JsonConvert.SerializeObject(input, RootType, Settings) : JsonConvert.SerializeObject(input, Settings);
			}
            catch (Exception e)
            {
                throw new ArgumentException("Cannot Json Serialize the provided object", "input", e);
            }
        }

        private void ApplySettings()
        {
            Settings = new JsonSerializerSettings
            {
                SerializationBinder = KnownTypes,
                TypeNameHandling = TypeNameHandling.Auto,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full,
                MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead,
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                ContractResolver = new ContractResolverWithPrivates(),
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };
        }
    }
}