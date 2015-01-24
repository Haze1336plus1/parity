using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Collections.Generic
{
    public static class ArrayExtension
    {

        public static T Pop<T>(ref T[] @this, bool begin = false)
        {
            T retVal = default(T);
            if (@this.Length == 0)
                throw new InvalidOperationException("Array has no elements to pop");
            if (begin)
            {
                retVal = @this[0];
                Array.Reverse(@this);
                Array.Resize(ref @this, @this.Length - 1);
                Array.Reverse(@this);
            }
            else
            {
                retVal = @this[@this.Length - 1];
                Array.Resize(ref @this, @this.Length - 1);
            }
            return retVal;
        }

        public static T[] Reverse<T>(ref T[] @this)
        {
            Array.Reverse(@this);
            return @this;
        }
    
    }
}
