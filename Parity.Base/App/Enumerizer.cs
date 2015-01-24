using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Base.App
{
    public class Enumerizer<T> where T : struct
    {

        public int[] Values { get; private set; }
        private Func<T, int> _converter;
        
        public int this[T val]
        {
            get
            {
                return this.Values[this._converter(val)];
            }
        }

        public Enumerizer(int[] values, Func<T, int> converter)
        {
            Array enumValues = System.Enum.GetValues(typeof(T));
            //Array.Resize(ref values, enumValues.Length);
            this.Values = values;
            this._converter = converter;
        }

    }
}
