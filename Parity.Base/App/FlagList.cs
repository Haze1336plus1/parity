using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Base.App
{
    public class FlagList<T>
    {

        protected readonly bool[] FlagOptions;
        public readonly List<T> Flags;
        public T[] GetFlags()
        {
            return this.Flags.Where((T flag) => { return this.FlagOptions[this.Flags.IndexOf(flag)] == true; }).ToArray();
        }

        public void Set(T flag) { this.FlagOptions[this.Flags.IndexOf(flag)] = true; }
        public void Unset(T flag) { this.FlagOptions[this.Flags.IndexOf(flag)] = false; }
        public void Change(T flag, bool value) { this.FlagOptions[this.Flags.IndexOf(flag)] = value; }

        public FlagList(params T[] flags)
        {
            this.Flags = flags.ToList();
            this.FlagOptions = Enumerable.Repeat<bool>(true, flags.Length).ToArray();
        }

    }
}
