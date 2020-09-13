using System;

namespace ExaminationLib.Helpers
{
    public class Message : MessageBase<string>
    {
        public Message() { }
        public Message(bool success, string contentMessage = null, Exception ex = null)
        {
            this.Success = success;
            this.Content = (success == true && contentMessage == null) ? "OK" : contentMessage;
            this.Exception = ex;
        }

    }
}
