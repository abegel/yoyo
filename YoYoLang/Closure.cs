using System;
namespace YoYo
{
    public class Closure
    {
        public Object obj;
        public Object last;

        public Closure(Object o, Object s)
        {
            obj = o;
            last = s;
        }
    }
}
