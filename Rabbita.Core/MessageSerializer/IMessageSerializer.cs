using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Rabbita.Core.MessageSerializer
{
    /// <summary>
    /// Базовый интерфес сериализаторо и десериалитора сообщений
    /// </summary>
    public interface IMessageSerializer
    {
        String Serialize([NotNull] in IMessage message);

        T? Deserialize<T>([NotNull] in String @object) where T : IMessage;
    }
}
