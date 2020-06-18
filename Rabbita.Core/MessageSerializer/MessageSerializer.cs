using System;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Rabbita.Core.MessageSerializer
{
    public sealed class MessageSerializer : IMessageSerializer
    {
        public String Serialize(in IMessage message)
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

        public IMessage? Deserialize(in String @object)
        {
            return JsonConvert.DeserializeObject<IMessage>(@object, new JsonSerializerSettings
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