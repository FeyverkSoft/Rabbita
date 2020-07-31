using System;

namespace Rabbita.Core.Message
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class MessageNameAttribute : Attribute
    {
        public String MessageName { get; private set; }

        public MessageNameAttribute(String messageName)
        {
            MessageName = messageName;
        }
    }
}