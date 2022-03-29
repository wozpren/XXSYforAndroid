using System;
using System.Collections.Generic;
using System.Text;

namespace NewXXSY.Models
{
    public readonly struct Message
    {
        public Message(string avatar, string time, string title, string pid, string uid)
        {
            Title = title;
            Time = time;
            Avatar = avatar;
            PID = pid;
            UID = uid;
        }

        public string UID { get;}
        public string PID { get; }
        public string Title { get;}
        public string Time { get;}
        public string Avatar { get; }

    }
}
