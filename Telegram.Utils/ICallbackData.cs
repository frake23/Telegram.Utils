using System.Collections.Generic;

namespace Telegram.Utils
{
    public interface ICallbackData
    {
        string New(object callbackDataObj);

        Dictionary<string, string> Parse(string callbackData);
    }
}