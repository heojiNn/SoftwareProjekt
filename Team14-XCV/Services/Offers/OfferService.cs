using System.Collections.Generic;


namespace XCV.Data
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
