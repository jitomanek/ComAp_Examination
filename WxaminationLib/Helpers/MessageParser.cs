using System.Collections.Generic;
using System.Linq;

namespace ExaminationLib.Helpers
{
    public class MessageParser : Message
    {
        public List<string> ParserErrors { get; private set; }

        public MessageParser() { }
        public MessageParser(bool success, string[] parserErrors = null) : base(success)
        {
            this.ParserErrors = parserErrors==null ? new List<string>() : parserErrors.ToList();
        }

        public void AddParseError(string parseError)
        {
            ParserErrors.Add(parseError);
        }

        public void AddErrorData(string errorData)
        {
            this.Content = errorData;
        }
    }
}
