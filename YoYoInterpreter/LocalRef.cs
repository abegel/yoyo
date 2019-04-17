using System;
namespace YoYo
{
    public class LocalRef : Sym
    {
        public int num
        {
            get;
            set;
        }
        public Symbol name
        {
            get;
            set;
        }

        public LocalRef(int loc, Symbol name)
        {
            this.name = name;
            this.num = loc;
        }
       
        public override string ToString()
        {
            return name.ToString();
        }
    }
}
