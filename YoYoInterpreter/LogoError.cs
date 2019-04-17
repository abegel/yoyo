using System;
using System.Text;

namespace YoYo
{
    public class LogoError : Exception
    {
        public LogoError()
        {
        }

        public LogoError(String s) : base(s)
        {
        }

        public static string ErrorToString(Context c)
        {
            StringBuilder output = new StringBuilder();
            if (c.currentFun != null)
            {
                output.Append(" in ");
                output.Append(c.currentFun.ToString());
            } 
            if (c.ufun != null)
            {
                output.Append(" in ");
                output.Append(c.ufun.ToString());
            }
            return output.ToString();
        }

        public static void Error(String s, Context c)
        {
            if (c == null) throw new LogoError(s);
            throw new LogoError(s + ErrorToString(c));
        }

        public static void Error(String s)
        {
            throw new LogoError(s);
        }

        public override string ToString()
        {
            string msg = Message;
            if (msg == null) return "LogoError";
            return Message;
        }
    }
}
