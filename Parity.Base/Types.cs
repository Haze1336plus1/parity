using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Base
{
    public class Types
    {

        private static readonly Type tByte = typeof(byte);
        private static readonly Type tSByte = typeof(sbyte);
        private static readonly Type tShort = typeof(short);
        private static readonly Type tUShort = typeof(ushort);
        private static readonly Type tInt = typeof(int);
        private static readonly Type tUInt = typeof(uint);
        private static readonly Type tLong = typeof(long);
        private static readonly Type tULong = typeof(ulong);
        private static readonly Type tDouble = typeof(double);
        private static readonly Type tFloat = typeof(float);
        private static readonly Type tString = typeof(string);
        private static readonly Type tBoolean = typeof(bool);
		private static readonly Type tIPAddress = typeof(System.Net.IPAddress);

        private delegate Func<string, T> ParseDelegate<T>(string input);

        public static bool ParseEnum<T>(string input, out T output) where T : struct
        {
            return System.Enum.TryParse(input, out output) && System.Enum.IsDefined(typeof(T), output);
        }
        public static bool TryParse<T>(string input, out T output)
        {
            Type gen = typeof(T);
            object oOutput = default(T);
            bool isSuccess = TryParse(gen, input, out oOutput);
            output = (T)oOutput;
            return isSuccess;
        }

        public static bool IsValid<T>(string input)
        {
            T output = default(T);
            return TryParse<T>(input, out output);
        }

        public static bool TryParse(Type genericType, string Input, out object Output)
        {
            bool doRet = false;
            Output = null;
            if (genericType == tString)
            {
                Output = (object)Input;
                return true;
            }
            else if (genericType == tByte)
            {
                byte pValue = default(byte);
                doRet = byte.TryParse(Input, out pValue);
                Output = (object)pValue;
            }
            else if (genericType == tSByte)
            {
                sbyte pValue = default(sbyte);
                doRet = sbyte.TryParse(Input, out pValue);
                Output = (object)pValue;
            }
            else if (genericType == tShort)
            {
                short pValue = default(short);
                doRet = short.TryParse(Input, out pValue);
                Output = (object)pValue;
            }
            else if (genericType == tUShort)
            {
                ushort pValue = default(ushort);
                doRet = ushort.TryParse(Input, out pValue);
                Output = (object)pValue;
            }
            else if (genericType == tInt)
            {
                int pValue = default(int);
                doRet = int.TryParse(Input, out pValue);
                Output = (object)pValue;
            }
            else if (genericType == tUInt)
            {
                uint pValue = default(uint);
                doRet = uint.TryParse(Input, out pValue);
                Output = (object)pValue;
            }
            else if (genericType == tLong)
            {
                long pValue = default(long);
                doRet = long.TryParse(Input, out pValue);
                Output = (object)pValue;
            }
            else if (genericType == tULong)
            {
                ulong pValue = default(ulong);
                doRet = ulong.TryParse(Input, out pValue);
                Output = (object)pValue;
            }
            else if (genericType == tDouble)
            {
                double pValue = default(double);
                doRet = double.TryParse(Input, out pValue);
                Output = (object)pValue;
            }
            else if (genericType == tFloat)
            {
                float pValue = default(float);
                doRet = float.TryParse(Input, out pValue);
                Output = (object)pValue;
            }
            else if (genericType == tBoolean)
            {
                bool pValue = default(bool);
                if (Input == "1")
                {
                    doRet = true;
                    Output = (object)true;
                }
                else if (Input == "0")
                {
                    doRet = true;
                    Output = (object)false;
                }
                else
                {
                    doRet = bool.TryParse(Input, out pValue);
                    Output = (object)pValue;
                }
            }
            else if (genericType == tIPAddress)
            {
                System.Net.IPAddress pValue = null;
                doRet = System.Net.IPAddress.TryParse(Input, out pValue);
                Output = (object)pValue;
            }
            return doRet;
            //}
        }

    }
}
