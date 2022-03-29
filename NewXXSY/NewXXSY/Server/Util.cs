using System;
using System.Collections.Generic;
using System.Text;

namespace NewXXSY.Server
{
    internal static class Util
    {

        public static string IDtoURL(string id)
        {
            StringBuilder url = new StringBuilder("000/00/00/00");
            int t = 0;
            int start = 11;
            for (int i = id.Length - 1; i >= 0; i--)
            {
                url.Remove(start, 1);
                url.Insert(start, id[i]);
                t++;
                start--;
                if (t == 2)
                {
                    start--;
                    t = 0;
                }
            }
            return url.ToString();
        }
    }
}
