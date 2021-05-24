using System;
namespace Team14.Data
{
    enum category
    {
        Hardskill,
        Softskill
    }

    public struct Skill
    {
        public int ID { get; }
        public string Name { get; }
        public Enum category;

        public Skill(Enum category) : this() => this.category = category;
        public Skill(string name) : this() => Name = name;
        public Skill(int id) : this() => ID = id;
    }
}
