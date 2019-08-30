using System;
using System.Collections.Generic;
using System.Linq;

namespace Telegram.Utils
{
    public class CallbackData: ICallbackData
    {
        public CallbackData(string prefix,  params string[] keys)
            : this(':', prefix, keys)
        {
        }
        
        public CallbackData(char separator, string prefix, params string[] keys)
        {
            if (string.IsNullOrEmpty(prefix))
                throw new ArgumentNullException(nameof(prefix));
            if (keys == null)
                throw new ArgumentNullException(nameof(keys));
            if (keys.Any(string.IsNullOrEmpty)) 
                throw new ArgumentException(nameof(keys) + " mustn't contain null or empty strings", nameof(keys));
            if (prefix.Contains(separator))
                throw new ArgumentException("Prefix can't contain the separator");
            if (keys.Any(key => key.Contains(separator)))
                throw new ArgumentException("Values can't contain the separator");

            _prefix = prefix;
            _keys = keys;
            _separator = separator;
        }

        private readonly string _prefix;
        private readonly string[] _keys;
        private readonly char _separator;

        public string New(object callbackDataObj)
        {
            if (callbackDataObj == null) {
                throw new ArgumentNullException(nameof(callbackDataObj));
            }
            
            var objProperties = callbackDataObj.GetType().GetProperties();
            if (objProperties.Length != _keys.Length)
                throw new ArgumentException("Values count doesn't match keys one");

            var callbackData = new List<string>
            {
                _prefix
            };
            
            foreach (var (propName, propValueObject) in callbackDataObj.GetType().GetProperties()
                .Select(prop => (prop.Name, prop.GetValue(callbackDataObj))))
            {
                if (!(propValueObject is string propValue))
                    throw new ArgumentException("Value's type must be string", nameof(callbackDataObj));
                if (!_keys.Contains(propName))
                    throw new ArgumentException($"Key {propName} doesn't exist");
                if (string.IsNullOrEmpty(propValue))
                    throw new ArgumentException("Value can't be empty");

                callbackData.Add((string) propValueObject);
            }

            return string.Join(_separator, callbackData);
        }

        public Dictionary<string, string> Parse(string callbackDataString)
        {
            if (string.IsNullOrEmpty(callbackDataString))
                throw new ArgumentNullException(nameof(callbackDataString));
            
            var values = callbackDataString.Split(_separator);
            var prefix = values[0];

            if (prefix != _prefix)
                throw new ArgumentException("Incorrect prefix");
            if (values.Length - 1 != _keys.Length)
                throw new ArgumentException("Invalid values count");

            var parsedCallbackData = new Dictionary<string, string>();
            for (var i = 0; i < _keys.Length; i++)
                parsedCallbackData[_keys[i]] = values[i + 1];

            return parsedCallbackData;
        }
    }
}