using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Base.App
{
    public class ArrayHelp
    {

        public static T[] Slice<T>(T[] Arr, int Offset, int Length)
        {
            if (Offset < 0)
                Offset = Arr.Length + Offset;
            T[] newArr = new T[Arr.Length - Length];
            System.Array.Copy(Arr, 0, newArr, 0, Offset);
            System.Array.Copy(Arr, Offset + Length, newArr, Offset, Arr.Length - Offset - Length);
            return newArr;
        }

        public static T[] CutOut<T>(T[] Arr, int Offset, int Length)
        {
            if (Offset < 0)
                Offset = Arr.Length + Offset;
            T[] newArr = new T[Length];
            System.Array.Copy(Arr, Offset, newArr, 0, newArr.Length);
            return newArr;
        }

    }
}
