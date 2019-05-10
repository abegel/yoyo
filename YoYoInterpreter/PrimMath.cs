using System;
namespace YoYo
{
    public class PrimMath : YoYoMethods
    {
        public static Random rand = new Random();

        public PrimMath()
        {
        }

        public string[] InfixList()
        {
            String[] output =
            {
                "+", "PrimSum", "-3",
                "*", "PrimMul", "-4",
                "-", "PrimSub", "-3",
                "/", "PrimDiv", "-4",
                "<=", "PrimLessEqual", "-2",
                ">=", "PrimGreaterEqual", "-2",
                ">", "PrimGreater", "-2",
                "<", "PrimLess", "-2",
                "^", "PrimPower", "-5",
                "mod", "PrimMod", "-4",
                "bitand", "PrimBitAnd", "-6",
                "bitor", "PrimBitOr", "-6",
                "bitxor", "PrimBitXor", "-6",
                "=", "PrimEqual", "-2",
                "!=", "PrimNotEqual", "-2",
                "and", "PrimMathAnd", "-1",
                "or", "PrimMathOr", "-1",
                };
            return output;
        }

        public string[] MethodList()
        {
            String[] output =
            {
                "sum", "PrimSum",
                "rem", "PrimRem",
                "difference", "PrimSub",
                "product", "PrimMul",
                "quotient", "PrimDiv",
                "power", "PrimPower",
                "greater?", "PrimGreater",
                "less?", "PrimLess",
                "minus", "PrimSub",
                "rsh", "PrimRsh",
                "lsh", "PrimLsh",
                "bitneg", "PrimBitNeg",
                "min", "PrimMin",
                "max", "PrimMax",
                "random", "PrimRandom",
                "sin", "PrimSin",
                "cos", "PrimCos",
                "tan", "PrimTan",
                "atan", "PrimAtan",
                "acos", "PrimAcos",
                "asin", "PrimAsin",
                "exp", "PrimExp",
                "ln", "PrimLn",
                "int", "PrimInt",
                "round", "PrimRound",
                "abs", "PrimAbs",
                "number?", "PrimNumberp",
                "sqrt", "PrimSqrt",
                "string-to-number", "PrimSToN",
                "not", "PrimNot",
                "string-is-number?", "PrimStNump",
            };
            return output;
        }

        public void PostPrimCall(Primitive p, Context c)
        {
        }

        public void PrePrimCall(Primitive p, object[] arglist, Context c)
        {

        }

        public static Object PrimSum(Object v0, Object v1, Context c)
        {
            return YoYo.aDouble(c, v0) + YoYo.aDouble(c, v1);
        }

        public static Object PrimSub(Object v0, Object v1, Context c)
        {
            return YoYo.aDouble(c, v0) - YoYo.aDouble(c, v1);
        }

        public static Object PrimMul(Object v0, Object v1, Context c)
        {
            return YoYo.aDouble(c, v0) * YoYo.aDouble(c, v1);
        }

        public static Object PrimDiv(Object v0, Object v1, Context c)
        {
            return YoYo.aDouble(c, v0) / YoYo.aDouble(c, v1);
        }

        public static Object PrimMod(Object v0, Object v1, Context c)
        {
            double divisor = YoYo.aDouble(c, v1);
            double answer = YoYo.aDouble(c, v0) % divisor;
            if (answer < 0) return answer + divisor;
            return answer;
        }

        public static Object PrimPower(Object v0, Object v1, Context c)
        {
            return Math.Pow(YoYo.aDouble(c, v0), YoYo.aDouble(c, v1));
        }

        public static Object PrimLess(Object v0, Object v1, Context c)
        {
            if (YoYo.aDouble(c, v0) < YoYo.aDouble(c, v1))
            { return YoYo.symtrue; }
            else { return YoYo.symfalse; }
        }

        public static Object PrimGreater(Object v0, Object v1, Context c)
        {
            if (YoYo.aDouble(c, v0) > YoYo.aDouble(c, v1))
            { return YoYo.symtrue; }
            else { return YoYo.symfalse; }
        }

        public static Object PrimLessEqual(Object v0, Object v1, Context c)
        {
            if (YoYo.aDouble(c, v0) <= YoYo.aDouble(c, v1))
            { return YoYo.symtrue; }
            else { return YoYo.symfalse; }
        }

        public static Object PrimGreaterEqual(Object v0, Object v1, Context c)
        {
            if (YoYo.aDouble(c, v0) >= YoYo.aDouble(c, v1))
            { return YoYo.symtrue; }
            else { return YoYo.symfalse; }
        }

        public static Object PrimRsh(Object v0, Object v1, Context c)
        {
            return (double)(YoYo.aInteger(c, v0) >> (int)YoYo.aInteger(c, v1));
        }

        public static Object PrimLsh(Object v0, Object v1, Context c)
        {
            return (double)(YoYo.aInteger(c, v0) << (int)YoYo.aInteger(c, v1));
        }

        public static Object PrimBitand(Object v0, Object v1, Context c)
        {
            return (double)(YoYo.aInteger(c, v0) & YoYo.aInteger(c, v1));
        }

        public static Object PrimBitor(Object v0, Object v1, Context c)
        {
            return (double)(YoYo.aInteger(c, v0) | YoYo.aInteger(c, v1));
        }

        public static Object PrimBitxor(Object v0, Object v1, Context c)
        {
            return (double)(YoYo.aInteger(c, v0) ^ YoYo.aInteger(c, v1));
        }

        public static Object PrimBitneg(Object v0, Context c)
        {
            return (double)~YoYo.aInteger(c, v0);
        }

        public static Object PrimMin(Object v0, Object v1, Context c)
        {
            if (YoYo.aDouble(c, v0) < YoYo.aDouble(c, v1))
                return v0;
            else return v1;
        }

        public static Object PrimMax(Object v0, Object v1, Context c)
        {
            if (YoYo.aDouble(c, v0) > YoYo.aDouble(c, v1))
                return v0;
            else return v1;
        }

        public static Object PrimRandom(Object v0, Context c)
        {
            return Math.Floor(rand.NextDouble() * YoYo.aInteger(c, v0));
        }

        public static double deg2Rad(double deg)
        {
            return ((deg % 360) / 180.0) * Math.PI;
        }

        public static double rad2Deg(double rad)
        {
            return (rad * 180.0) / Math.PI;
        }

        public static Object PrimSin(Object v0, Context c)
        {
            return Math.Sin(deg2Rad(YoYo.aDouble(c, v0)));
        }

        public static Object PrimCos(Object v0, Context c)
        {
            return Math.Cos(deg2Rad(YoYo.aDouble(c, v0)));
        }

        public static Object PrimTan(Object v0, Context c)
        {
            return Math.Tan(deg2Rad(YoYo.aDouble(c, v0)));
        }

        public static Object PrimAtan(Object v0, Object v1, Context c)
        {
            return rad2Deg(Math.Atan(YoYo.aDouble(c, v0) / YoYo.aDouble(c, v1)));
        }

        public static Object PrimAsin(Object v0, Context c)
        {
            return rad2Deg(Math.Asin(YoYo.aDouble(c, v0)));
        }

        public static Object PrimAcos(Object v0, Context c)
        {
            return rad2Deg(Math.Acos(YoYo.aDouble(c, v0)));
        }

        public static Object PrimExp(Object v0, Context c)
        {
            return Math.Exp(YoYo.aDouble(c, v0));
        }

        public static Object PrimLn(Object v0, Context c)
        {
            return Math.Log(YoYo.aDouble(c, v0));
        }

        public static Object PrimInt(Object v0, Context c)
        {
            return (double)YoYo.aInteger(c, v0);
        }

        public static Object PrimRound(Object v0, Context c)
        {
            return Math.Round(YoYo.aDouble(c, v0));
        }

        public static Object PrimAbs(Object v0, Context c)
        {
            double foo = YoYo.aDouble(c, v0);
            if (foo < 0) return -foo; else return v0;
        }

        public static bool IsNumber(object value)
        {
            return value is sbyte
                    || value is byte
                    || value is short
                    || value is ushort
                    || value is int
                    || value is uint
                    || value is long
                    || value is ulong
                    || value is float
                    || value is double
                    || value is decimal;
        }

        public static Object PrimNumberp(Object v0, Context c)
        {
            if (IsNumber(v0)) 
            return YoYo.symtrue;
            else return YoYo.symfalse;
        }

        public static Object PrimEqual(Object v0, Object v1, Context c)
        {
            return YoYo.equal(v0, v1);
        }

        public static Object PrimNotEqual(Object v0, Object v1, Context c)
        {
            return (YoYo.equal(v0, v1) == YoYo.symtrue)
                ? YoYo.symfalse : YoYo.symtrue;
        }

        public static Symbol truefalse(Boolean tf)
        {
            return (tf) ? YoYo.symtrue : YoYo.symfalse;
        }

        public static Object PrimAnd(Object v0, Object v1, Context c)
        {
            //System.out.println("and: " + YoYo.printToString(v0) + " " 
            //         + YoYo.printToString(v1));
            if (YoYo.aBoolean(c, v0) && YoYo.aBoolean(c, v1))
                return YoYo.symtrue;
            else return YoYo.symfalse;
        }

        public static Object PrimOr(Object v0, Object v1, Context c)
        {
            if (YoYo.aBoolean(c, v0) || YoYo.aBoolean(c, v1))
                return YoYo.symtrue;
            else return YoYo.symfalse;
        }

        public static Object PrimXor(Object v0, Object v1, Context c)
        {
            if (YoYo.aBoolean(c, v0) ^ YoYo.aBoolean(c, v1))
                return YoYo.symtrue;
            else return YoYo.symfalse;
        }


        public static Object PrimSqrt(Object v0, Context c)
        {
            return Math.Sqrt(YoYo.aDouble(c, v0));
        }

        public static Object PrimSToN(Object v0, Context c)
        {
            String foo = YoYo.aString(c, v0);
            Object[] bar = Reader.Read(foo);
            if (bar.Length > 0 && IsNumber(bar[0])) return bar[0];
            LogoError.Error(foo + " is not a number", c);
            return null;
        }

        public static Object PrimStNump(Object v0, Context c)
        {
            String foo = YoYo.aString(c, v0);
            Object[] bar = Reader.Read(foo);
            if (bar.Length > 0 && IsNumber(bar[0])) return YoYo.symtrue;
            return YoYo.symfalse;
        }


    }
}
