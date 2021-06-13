

namespace Team14.Data
{
    public class BasicDataLeaf
    {
        // Service checkes for not loger than 45
        public string Name { get; init; } = ""; //set while deseriallising

        public bool HasDouble { get; set; } // set while CheckFunction in Service

        public override string ToString() => $"{Name}";
    }
}
