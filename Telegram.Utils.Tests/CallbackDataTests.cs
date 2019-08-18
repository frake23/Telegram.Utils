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
            _callbackData = new CallbackData("user", "id", "lang");
        }
        
        private CallbackData _callbackData;
        
        [Fact]
        public void CallbackData_New_Works()
        {
            Assert.Equal("user:123456789:ru", _callbackData.New(new
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
            }, _callbackData.Parse("user:123456789:ru"));
        }

        [Fact]
        public void CallbackData_Constructor_Exceptions()
        {
            Assert.Throws<StringNullOrEmptyException>(() => new CallbackData("", "id", "lang"));
            Assert.Throws<StringNullOrEmptyException>(() => new CallbackData(null, "id", "lang"));
            Assert.Throws<StringNullOrEmptyException>(() => new CallbackData("user", "", "lang"));
            Assert.Throws<StringNullOrEmptyException>(() => new CallbackData("user", "id", null));
            Assert.Throws<StringNullOrEmptyException>(() => new CallbackData("user", null));
        }

        [Fact]
        public void CallbackData_New_Exceptions()
        {
            Assert.Throws<ArgumentException>(() => _callbackData.New(new
            {
                id = "123"
            }));
            Assert.Throws<StringNullOrEmptyException>(() => _callbackData.New(new
            {
                id = "123",
                lang = ""
            }));
            Assert.Throws<ArgumentException>(() => _callbackData.New(new
            {
                id = "123",
                lang = 123
            }));
            Assert.Throws<ArgumentException>(() => _callbackData.New(new
            {
                id = "123",
                lng = "s"
            }));
            Assert.Throws<ArgumentException>(() => _callbackData.New(new
            {
                id = "123",
                lang = "ru",
                time = "18:00"
            }));
        }
    }
}