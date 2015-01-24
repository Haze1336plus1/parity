using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Handler
{
    public class _Instance
    {

        public readonly Net.Handler.IHandler<Client.Client> @Handler;
        public readonly Net.Handler.Requirements Requirements;

        public _Instance(Type handlerType)
        {
            this.@Handler = (Net.Handler.IHandler<Client.Client>)handlerType.Assembly.CreateInstance(handlerType.FullName);
            this.Requirements =
                handlerType
                .GetCustomAttributes(typeof(Net.Handler.Requirements), false)
                .FirstOrDefault()
                as Net.Handler.Requirements;
        }

    }
}
