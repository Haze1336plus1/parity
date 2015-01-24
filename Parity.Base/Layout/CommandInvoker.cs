using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Base.Layout
{
    public class CommandInvoker
    {

        public Enum.CommandInvoker Type { get; private set; }
        public object Invoker { get; private set; }

        public CommandInvoker(Enum.CommandInvoker invokerType, object invoker = null)
        {
            this.Type = invokerType;
            this.Invoker = invoker;
        }

    }
}
