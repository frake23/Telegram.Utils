using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Utils.Exceptions;

namespace Telegram.Utils
{
    public class CallbackData: ICallbackData
    {
        public CallbackData(string prefix, params string[] keys)
        {
            if (prefix == null || prefix.Equals(""))
                throw new StringNullOrEmptyException("Prefix can't be empty or null");
            if (keys == null || keys.Contains(null) || keys.Contains(""))
                throw new StringNullOrEmptyException("Keys can't contain empty string or null");
            
            _prefix = prefix;
            _keys = keys;
        }

        public char Separator
        {
            set => _separator = value;
        }

        private string _prefix;
        private string[] _keys;
        private char _separator = ':';

        public string New(object callbackDataObj)
        {
            var objProperties = callbackDataObj.GetType().GetProperties();
            if (objProperties.Length != _keys.Length)
                throw new ArgumentException("Values count doesn't match keys one");
            var callbackData = new List<string>
            {
                _prefix
            };
            foreach (var (propName, propValue) in callbackDataObj.GetType().GetProperties()
                .Select(prop => (prop.Name, prop.GetValue(callbackDataObj))))
            {
                if (propValue.GetType() != typeof(string))
                    throw new ArgumentException("Values type must be string");
                if (!_keys.Contains(propName))
                    throw new ArgumentException($"Key {propName} doesn't exist");
                if (propValue.Equals(""))
                    throw new StringNullOrEmptyException("Value can't be empty");

                callbackData.Add((string) propValue);
            }

            return string.Join(_separator, callbackData);
        }

        public Dictionary<string, string> Parse(string callbackDataString)
        {
            var values = callbackDataString.Split(_separator);
            var prefix = values[0];
            var parsedCallbackData = new Dictionary<string, string>();
            for (var i = 0; i < _keys.Length; i++)
                parsedCallbackData[_keys[i]] = values[i + 1];
            return parsedCallbackData;
        }
    }
}