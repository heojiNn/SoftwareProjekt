using System;
using System.Collections.Generic;


namespace Team14.Data
{
    public class OfferService
    {
        private readonly List<Offer> theSingelton;
        public OfferService()
        {
            theSingelton = new();
        }

        public List<Offer> GetSingelton()
        {
            return theSingelton;
        }

    }
}
