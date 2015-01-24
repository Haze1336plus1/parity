using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Base.Thread
{
    /// <summary>
    /// Create a .Take function
    /// </summary>
    public class Lockable : IDisposable
    {

        protected object _lockingObject = new Object();
        public bool IsTaken { get; private set; }

        public virtual bool Take()
        {
            if (!this.IsTaken)
            {
                this.IsTaken = true;
                return true;
            }
            return false;
        }
        public void Release()
        {
            this.IsTaken = false;
        }

        void IDisposable.Dispose()
        {
            this.Release();
        }
    }
}
