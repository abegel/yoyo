using System;
namespace YoYo
{
    public class StringStream
    {

        string input;
        int current = 0;

        public StringStream(String inString)
        {
            input = inString;
            current = 0;
        }

        public void Reset()
        {
            current = 0;
        }

        public Boolean Empty()
        {
            if (current >= input.Length) return true;
            return false;
        }

        public int Peek()
        {
            if (current < input.Length) return input[current];
            return -1;
        }

        public int PeekPeek()
        {
            if (current < input.Length - 1) return input[current+1];
            return -1;
        }

        public int Read()
        {
            if (current < input.Length)
            {
                return input[current++];
            }
            return -1;
        }


    }
}
