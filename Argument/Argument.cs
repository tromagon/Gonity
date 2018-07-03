using System;
using System.Collections.Generic;

namespace Gonity
{
    public class Argument
    {
        private Dictionary<object, ArgumentValue> _arguments;

        public Argument()
        {
            _arguments = new Dictionary<object, ArgumentValue>();
        }

        public Argument Set<T>(object key, T value)
        {
            _arguments.Add(key, new ArgumentValue<T> { value = value });
            return this;
        }

        public T Get<T>(object key)
        {
            return (_arguments[key] as ArgumentValue<T>).value;
        }

        private abstract class ArgumentValue { }

        private class ArgumentValue<T> : ArgumentValue
        {
            public T value;
        }
    }
}
