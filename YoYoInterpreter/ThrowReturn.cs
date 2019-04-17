using System;
namespace YoYo
{
    public class ThrowReturn : LogoError
    {
        public Object value
        {
            get;
            set;
        }

        public Object errortype
        {
            get;
            set;
        }

        public ThrowReturn(Object errortype, Object value)
        {
            this.errortype = errortype;
            this.value = value;
        }

        public override string ToString()
        {
            if (value != null)
            {
                return value.ToString();
            } else
            {
                return "ThrowReturn";
            }
        }
    }
}
