using System.Collections.Generic;


namespace Team14.Data
{
    public class BasicDataLeaf
    {
        // is ^[A-Za-z0-9 .\-_]{2,20}$ on Elements
        public string Name { get; init; } = "";  //done while deseriallising

        public bool HasDouble { get; set; } // set while cleaning

        public override string ToString() => $"{Name}- node";
    }
}
