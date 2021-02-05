using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Rabbita.Core.MessageSerializer
{
    public sealed class JsonMessageSerializer : IMessageSerializer
    {
        public String Serialize([NotNull] in IMessage message)
        {
            return JsonConvert.SerializeObject(message, new JsonSerializerSettings
            {
                Converters = new List<JsonConverter>
                {
                    new StringEnumConverter(),
                    new IsoDateTimeConverter(),
                },
                NullValueHandling = NullValueHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Objects
            });
        }

        public T? Deserialize<T>([NotNull] in String @object) where T : IMessage
        {
            return JsonConvert.DeserializeObject<T>(@object, new JsonSerializerSettings
            {
                Converters = new List<JsonConverter>
                {
                    new StringEnumConverter(),
                    new IsoDateTimeConverter(),
                },
                NullValueHandling = NullValueHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Objects
            });
        }
    }
}