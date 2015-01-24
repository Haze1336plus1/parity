using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.GameDetails.Item
{
    public class Price : Basic
    {

        public int[] Dinar { get; private set; }
        public int[] Credits { get; private set; }

        public Price(string code, int[] dinar, int[] credits)
            : base(code)
        {

            Func<int[], int[]> fixPricing = new Func<int[], int[]>((int[] priceArray) =>
            {
                Array.Resize<int>(ref priceArray, 5);
                for (int i = 0; i < 5; i++)
                    priceArray[i] = (priceArray[i] <= 0 ? -1 : priceArray[i]);
                return priceArray;
            });

            this.Dinar = fixPricing(dinar);
            this.Credits = fixPricing(credits);

        }

    }
}
