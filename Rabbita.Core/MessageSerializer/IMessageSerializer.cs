using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Rabbita.Core.MessageSerializer
{
    public interface IMessageSerializer
    {
        String Serialize(in IMessage message);

        IMessage? Deserialize(in String @object);
    }
}
