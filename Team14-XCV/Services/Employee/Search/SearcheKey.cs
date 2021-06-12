


namespace Team14.Data
{
    public struct SearchKey
    {
        public string Category { get; init; }
        public string Name { get; set; }
        public string Level { get; init; }




        public override string ToString()
        {
            var category = Category.Equals("") ? "" : $"{Category}\\";
            var level = Level.Equals("") ? "" : $"-{Level}";
            return  $"{Category}\"{Name}\"{Level}";
        }
    }
}
