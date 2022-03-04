using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FinClever
{
    public class NonLazyloaderContractResolver : DefaultContractResolver
    {
        public new static readonly NonLazyloaderContractResolver Instance = new NonLazyloaderContractResolver();

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            if (property.PropertyName == "LazyLoader")
            {
                property.ShouldSerialize = i => false;
            }

            return property;
        }
    }
}
