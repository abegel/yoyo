using System;
namespace YoYo
{
    public interface YoYoMethods
    {
        String[] MethodList();
        String[] InfixList();
        void PrePrimCall(Primitive p, object[] arglist, Context c);
        void PostPrimCall(Primitive p, Context c);

    }
}
