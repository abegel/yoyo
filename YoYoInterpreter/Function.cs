using System;
namespace YoYo
{
    public interface Function
    {
        Symbol GetName();
        int GetNargs();
        object[] GetArgArray(Context c);
        object Run(Object[] arglist, Context c);
        void Trace();
        void Untrace();

    }
}
