using System;


namespace YoYo
{
    public class InstructionList
    {

        public Object[] list;

        public InstructionList(String s)
        {
            list = Reader.Read(s);
        }

        public InstructionList(Object[] list)
        {
            this.list = list;
        }

        public override string ToString()
        {
            return YoYo.ObjectArrayToString(list, '(', ')', false);
        }

    }
}
