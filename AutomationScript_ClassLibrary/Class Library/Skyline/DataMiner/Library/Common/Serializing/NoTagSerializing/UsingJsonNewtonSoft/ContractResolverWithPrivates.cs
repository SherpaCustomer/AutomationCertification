namespace Skyline.DataMiner.Library.Common.Serializing.NoTagSerializing.UsingJsonNewtonSoft
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    using Skyline.DataMiner.Library.Common.Attributes;

    using System;
    using System.Linq;

    [DllImport("Newtonsoft.Json.dll")]
    internal class ContractResolverWithPrivates : CamelCasePropertyNamesContractResolver
    {
        protected override JsonDictionaryContract CreateDictionaryContract(Type objectType)
        {
            JsonDictionaryContract contract = base.CreateDictionaryContract(objectType);

            contract.DictionaryKeyResolver = key => key;

            return contract;
        }

		protected override System.Collections.Generic.IList<JsonProperty> CreateProperties(System.Type type, MemberSerialization memberSerialization)
        {
            return base.CreateProperties(type, memberSerialization).OrderBy(p => p.PropertyName).ToList();
        }

        protected override Newtonsoft.Json.Serialization.JsonProperty CreateProperty(System.Reflection.MemberInfo member, MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);

            if (!prop.Writable)
            {
                var property = member as System.Reflection.PropertyInfo;
                if (property != null)
                {
                    var hasPrivateSetter = property.GetSetMethod(true) != null;
                    prop.Writable = hasPrivateSetter;
                }
            }

            return prop;
        }
    }
}