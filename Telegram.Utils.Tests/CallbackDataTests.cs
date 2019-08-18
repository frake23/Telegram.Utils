using System;
using System.Collections.Generic;
using Telegram.Utils.Exceptions;
using Xunit;

namespace Telegram.Utils.Tests
{
    public class CallbackDataTests
    {
        public CallbackDataTests()
        {
            _defaultCallbackData = new CallbackData("user", "id", "lang");
        }
        
        private readonly CallbackData _defaultCallbackData;

        [Fact]
        public void CallbackData_New_Works()
        {
            Assert.Equal("user:123456789:ru", _defaultCallbackData.New(new
            {
                id = "123456789",
                lang = "ru"
            }));
            
            var callbackData = new CallbackData('.', "user", "id", "lang");
            Assert.Equal("user.123456789.ru", callbackData.New(new
            {
                id = "123456789",
                lang = "ru"
            }));
        }

        [Fact]
        public void CallbackData_Parse_Works()
        {
            Assert.Equal(new Dictionary<string, string>
            {
                {"id", "123456789"},
                {"lang", "ru"}
            }, _defaultCallbackData.Parse("user:123456789:ru"));
        }

        [Fact]
        public void CallbackData_Constructor_Exceptions()
        {
            Assert.Throws<StringNullOrEmptyException>(() => new CallbackData("", "id", "lang"));
            Assert.Throws<StringNullOrEmptyException>(() => new CallbackData(null, "id", "lang"));
            Assert.Throws<StringNullOrEmptyException>(() => new CallbackData("user", "", "lang"));
            Assert.Throws<StringNullOrEmptyException>(() => new CallbackData("user", "id", null));
            Assert.Throws<StringNullOrEmptyException>(() => new CallbackData("user", null));
            Assert.Throws<ArgumentException>(() => new CallbackData("use:r", "id", "lang"));
            Assert.Throws<ArgumentException>(() => new CallbackData('.', "use.r", "id", "lang"));
            Assert.Throws<ArgumentException>(() => new CallbackData("user", "i:d", "lang"));
            Assert.Throws<ArgumentException>(() => new CallbackData('.', "user", "id", "l.ang"));
        }

        [Fact]
        public void CallbackData_New_Exceptions()
        {
            Assert.Throws<ArgumentException>(() => _defaultCallbackData.New(new
            {
                id = "123"
            }));
            Assert.Throws<StringNullOrEmptyException>(() => _defaultCallbackData.New(new
            {
                id = "123",
                lang = ""
            }));
            Assert.Throws<ArgumentException>(() => _defaultCallbackData.New(new
            {
                id = "123",
                lang = 123
            }));
            Assert.Throws<ArgumentException>(() => _defaultCallbackData.New(new
            {
                id = "123",
                lng = "s"
            }));
            Assert.Throws<ArgumentException>(() => _defaultCallbackData.New(new
            {
                id = "123",
                lang = "ru",
                time = "18:00"
            }));
        }

        [Fact]
        public void CallbackData_Parse_Exceptions()
        {
            Assert.Throws<ArgumentException>(() => _defaultCallbackData.Parse("User:123456789:ru"));
            Assert.Throws<ArgumentException>(() => _defaultCallbackData.Parse("user:123456789:ru:18-00"));
            Assert.Throws<ArgumentException>(() => _defaultCallbackData.Parse("user:123456789"));
        }
    }
}