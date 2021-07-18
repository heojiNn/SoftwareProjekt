using System.Collections.Generic;


namespace XCV.Data
{
    /// <summary>
    ///         the Class/Struct that represents the datenbasis.json
    /// </summary>
    struct dataSet
    {
        public IEnumerable<string> fields;  //nur elements
        public dataSetLanguages languages;  //elements und wages
        public dataSetSkills skills;  //elements und wages
        public IEnumerable<dataSetrole> roles;  //elements und wages
    }

    /// <summary>
    ///         a internal strutcure of the datenbasis.json
    /// </summary>
    struct dataSetrole
    {
        public string name;
        public dataSetWage[] wages;
    }
    struct dataSetWage
    {
        public int RCL;
        public float PerHour;
    }
    /// <summary>
    ///         a internal strutcure of the datenbasis.json
    /// </summary>
    struct dataSetLanguages
    {
        public string[] levels;
        public IEnumerable<string> elements;
    }

    /// <summary>
    ///         a internal strutcure of the datenbasis.json
    /// </summary>
    struct dataSetSkills
    {
        public string[] hardSkillLevels;
        public SkillCategory HardSkills;
        public string[] SoftSkills;
    }
}
