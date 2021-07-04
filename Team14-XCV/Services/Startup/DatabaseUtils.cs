using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace XCV.Data
{
    public class DatabaseUtils
    {
        private readonly IConfiguration config;
        private readonly ILogger<DatabaseUtils> log;
        private readonly string connectionString;
        private readonly string databaseName;


        public DatabaseUtils(IConfiguration IConfiguration, ILogger<DatabaseUtils> logger)
        {
            config = IConfiguration;
            log = logger;
            connectionString = config.GetConnectionString("MS_SQL_Connection");     //from appsettings.json
            databaseName = config.GetConnectionString("DatabaseName");              //from appsettings.json
        }



        public void CreateDatabase()
        {
            using var con = new SqlConnection(config.GetConnectionString("MS_SQL_Connection_Root"));
            try
            {
                con.Open();
                con.Execute(@$"IF NOT EXISTS ( SELECT name  FROM sys.databases  WHERE name = N'{databaseName}' )
                            CREATE DATABASE {databaseName};");

            }
            catch (SqlException e)
            {
                log.LogError($"CreateDatabase() SqlException: \n{e.Message}\n");
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        /// <summary>
        ///     Creates
        ///         [field],  [role],  [language], [language_level],  [skillCategory], [skill], [skill_level]
        ///         [employee],   [project], [activity], [activity_done_by], [project_purpose]
        ///         [employee_acrole], [employee_field], [employee_role], [employee_language], [employee_skill]
        /// </summary>
        public void CreateTables()
        {
            using var con = new SqlConnection(connectionString);
            try
            {
                //-----------lookup-tables-----------------------------------------------------------------------------
                con.Execute("IF OBJECT_ID('field', 'U') IS NULL " +                     // Field
                            "CREATE TABLE [field] ( " +                                 // ref by emp_field, project
                                    "[Name]         VARCHAR(50) PRIMARY KEY " +
                            ");");


                con.Execute("IF OBJECT_ID('role', 'U') IS NULL " +                       // Role
                            "CREATE TABLE [role] ( " +                                  // ref by employee_role
                                    "[Name]         VARCHAR(50), " +
                                    "[Rcl]          INT, " +
                                    "[Wage]         DECIMAL(5, 2) NOT NULL, " +
                                    "PRIMARY KEY (name, rcl) " +
                            ");");


                con.Execute("IF OBJECT_ID('language', 'U') IS NULL " +                   // Language
                            "CREATE TABLE [language] ( " +                              // ref by employee_language
                                    "[Name]         VARCHAR(50) PRIMARY KEY " +
                            ");");
                con.Execute("IF OBJECT_ID('language_level', 'U') IS NULL " +             // Language_Level
                            "CREATE TABLE [language_level] ( " +                        // ref by employee_language
                                    "[Level]        INT UNIQUE, " +
                                    "[Name]         VARCHAR(50) PRIMARY KEY " +
                            ");");                  //Key is Name   cause values can be  inserted,changed and deleted


                con.Execute("IF OBJECT_ID('skillCategory', 'U') IS NULL " +              // SkillCategory
                            "CREATE TABLE [skillCategory] ( " +                         // ref by skill
                                    "[Name]         VARCHAR(50) PRIMARY KEY, " +
                                    "[Parent]       VARCHAR(50) " +
                            ");");
                //          "CONSTRAINT fK_cat_cat FOREIGN KEY (parent) REFERENCES skillCategory(Name)"
                con.Execute("IF OBJECT_ID('skill', 'U') IS NULL " +                      // Skill
                            "CREATE TABLE [skill] ( " +                                 // ref by employee_skill
                                    "[Name]         VARCHAR(50), " +
                                    "[Category]     VARCHAR(50) NOT NULL, " +
                                    "PRIMARY KEY (name, category), " +
                                    "CONSTRAINT fK_s_cat FOREIGN KEY (category) REFERENCES skillCategory(Name) ON DELETE CASCADE" +
                            ");");
                con.Execute("IF OBJECT_ID('skill_level', 'U') IS NULL " +                // Skill_Level
                            "CREATE TABLE [skill_level] ( " +                           // ref by employee_skill
                                    "[Level]        INT PRIMARY KEY, " +
                                    "[Name]         VARCHAR(50) UNIQUE " +
                            ");");                  //Key is Level  cause  number 4 is fixed  ---(no insert or delete)----
                con.Execute(@"IF NOT EXISTS (SELECT * FROM [skill_level] ) " +
                            "Insert Into [skill_level] Values " +
                                "(1, 'h'),  (2, 'p'),  (3, 'rege'),  (4, 'exp') " +
                            ";");



                //-----------Entity------------------------------------------------------------------------------------
                con.Execute("IF OBJECT_ID('employee', 'U') IS NULL " +                   // Employee
                            "CREATE TABLE [employee] ( " +              //ref by employee_accessrole/field/role/laguage/skill
                                    "[PersoNumber]  VARCHAR(20) PRIMARY KEY, " +
                                    "[Password]     VARCHAR(20) NOT NULL, " +
                                    "[FirstName]    VARCHAR(50) NOT NULL, " +
                                    "[LastName]     VARCHAR(50) NOT NULL, " +
                                    "[Description]  VARCHAR(200) NOT NULL, " +
                                    "[Image]        VARCHAR(50) NOT NULL, " +
                                    "[RCL]          INT, " +                        //can be Null
                                    "[Expirience]   DATE, " +                       //can be Null
                                    "[EmployyedSince] DATE NOT NULL, " +
                                    "[MadeFirstChangesOnProfile] BIT NOT NULL " +
                            ");");
                con.Execute("IF OBJECT_ID('project', 'U') IS NULL " +                    // Project
                            "CREATE TABLE [project] ( " +                               // ref by pro_purpose, activitie
                                    "[Id]           INT IDENTITY(1,1) PRIMARY KEY, " +
                                    "[Title]        VARCHAR(50) NOT NULL, " +
                                    "[Description]  VARCHAR(1000) NOT NULL, " +
                                    "[Start]        DATE NOT NULL, " +
                                    "[End]          DATE NOT NULL, " +
                                    "[Field]        VARCHAR(50), " +                //can be Null
                                    "CONSTRAINT fK_pro_field FOREIGN KEY (field) REFERENCES  field(Name) ON DELETE SET NULL " +
                            ");");
                con.Execute("IF OBJECT_ID('offer', 'U') IS NULL " +                     // Offer
                            "CREATE TABLE [offer] ( " +                                 // ref by offer_Employee offer_Field offer_Skill
                                    "[Id]           INT NOT NULL IDENTITY PRIMARY KEY," +
                                    "[Title]        VARCHAR(50) NOT NULL," +
                                    "[Description]  VARCHAR(1000) NOT NULL, " +
                            ");");



                //-----------Weak entity-------------------------------------------------------------------------------
                con.Execute("IF OBJECT_ID('projectHasPurpose', 'U') IS NULL " +            // Project Has Purpose
                            "CREATE TABLE [projectHasPurpose] ( " +
                                    "[Name]         VARCHAR(200), " +
                                    "[Project]      INT, " +
                                    "PRIMARY KEY (name, project), " +
                                    "CONSTRAINT fK_pur_pro FOREIGN KEY (project) REFERENCES  project(Id) ON DELETE CASCADE " +
                            ");");
                con.Execute("IF OBJECT_ID('projectHasActivity', 'U') IS NULL " +            //Pproject Has Activity
                            "CREATE TABLE [projectHasActivity] ( " +                        // ref by activity_Emp
                                    "[Name]         VARCHAR(100), " +
                                    "[Project]      INT, " +
                                    "PRIMARY KEY (name, project), " +
                                    "CONSTRAINT fK_akti_pro FOREIGN KEY (project) REFERENCES  project(Id) ON DELETE CASCADE " +
                            ");");


                //-----------n to m Relation---------------------------------------------------------------------------
                con.Execute("IF OBJECT_ID('employeeHasAcrole', 'U') IS NULL " +            // Employee Has Acrole
                            "CREATE TABLE [employeeHasAcrole] ( " +
                                    "[EnumerationInt] INT, " +
                                    "[Employee]     VARCHAR(20), " +
                                    "PRIMARY KEY (enumerationint, employee), " +
                                    "CONSTRAINT fK_e_a_emp FOREIGN KEY (employee) REFERENCES  employee(PersoNumber) ON DELETE CASCADE " +
                            ");");
                con.Execute("IF OBJECT_ID('employeeHasField', 'U') IS NULL " +             // Employee Has Field
                            "CREATE TABLE [employeeHasField] ( " +
                                    "[Field]        VARCHAR(50), " +
                                    "[Employee]     VARCHAR(20), " +
                                    "PRIMARY KEY (field, employee), " +
                                    "CONSTRAINT fK_e_f_field FOREIGN KEY (field) REFERENCES  field(name) ON DELETE CASCADE, " +
                                    "CONSTRAINT fK_e_f_emp FOREIGN KEY (employee) REFERENCES  employee(PersoNumber) ON DELETE CASCADE " +
                            ");");
                con.Execute("IF OBJECT_ID('employeeHasRole', 'U') IS NULL " +              // Employee Has Role
                            "CREATE TABLE [employeeHasRole] ( " +
                                    "[Role]         VARCHAR(50), " +
                                    "[Role_Rcl]     INT NOT NULL, " +
                                    "[Employee]     VARCHAR(20), " +
                                    "PRIMARY KEY (role, employee), " +
                                    "CONSTRAINT fK_e_r_role FOREIGN KEY (role, role_rcl) REFERENCES role(Name, Rcl) ON DELETE CASCADE, " +
                                    "CONSTRAINT fK_e_r_emp FOREIGN KEY (employee) REFERENCES  employee(PersoNumber) ON DELETE CASCADE " +
                            ");");
                con.Execute("IF OBJECT_ID('employeeHasLanguage', 'U') IS NULL " +          // Employee Has Language
                            "CREATE TABLE [employeeHasLanguage] ( " +
                                    "[Language]       VARCHAR(50), " +
                                    "[Language_Level] VARCHAR(50), " +      //can be Null (only when references is gone)
                                    "[Employee]       VARCHAR(20), " +
                                    "PRIMARY KEY (language, employee), " +
                                    "CONSTRAINT fK_e_l_lang FOREIGN KEY (language) REFERENCES  language(Name) ON DELETE CASCADE, " +
                                    "CONSTRAINT fK_e_l_lvl FOREIGN KEY (language_level) REFERENCES  language_level(Name) ON DELETE SET Null, " +
                                    "CONSTRAINT fK_e_l_emp FOREIGN KEY (employee) REFERENCES  employee(PersoNumber) ON DELETE CASCADE " +
                            ");");
                con.Execute("IF OBJECT_ID('employeeHasSkill', 'U') IS NULL " +             // Employee Has Skill
                            "CREATE TABLE [employeeHasSkill] ( " +
                                    "[Skill]        VARCHAR(50), " +
                                    "[Skill_Cat]    VARCHAR(50), " +
                                    "[Skill_Level]  INT, " +               //can Null  when SoftSkill
                                    "[Employee]     VARCHAR(20), " +
                                    "PRIMARY KEY (skill, skill_cat, employee), " +
                                    "CONSTRAINT fK_e_s_skill FOREIGN KEY (skill, skill_cat) REFERENCES skill(Name, Category) ON DELETE CASCADE, " +
                                    "CONSTRAINT fK_e_s_lvl FOREIGN KEY (skill_level) REFERENCES  skill_level(Level), " +
                                    "CONSTRAINT fK_e_s_emp FOREIGN KEY (employee) REFERENCES  employee(PersoNumber) ON DELETE CASCADE " +
                            ");");


                con.Execute("IF OBJECT_ID('activityHasEmployee', 'U') IS NULL " +          // Activity(in Project) Has Employee
                            "CREATE TABLE [activityHasEmployee] ( " +
                                    "[Activity]     VARCHAR(100), " +
                                    "[Project]      INT,  " +
                                    "[Employee]     VARCHAR(20),  " +
                                    "PRIMARY KEY (activity, project, employee), " +
                                    "CONSTRAINT fK_a_e_akti FOREIGN KEY (activity, project) REFERENCES  ProjectHasActivity(Name, Project) ON DELETE CASCADE, " +
                                    "CONSTRAINT fK_a_e_emp FOREIGN KEY (employee) REFERENCES  employee(PersoNumber) ON DELETE CASCADE " +
                            ");");

                con.Execute("IF OBJECT_ID('offerHasEmployee', 'U') IS NULL " +          // Offer Has Employee
                            "CREATE TABLE [offerHasEmployee] ( " +
                                    "Offer INT, " +
                                    "PersoNumber VARCHAR(20), " +
                                    "Role VARCHAR(50), " + // soll dem Mitarbeiter zugewiesen werden innerhalb des Angebots
                                    "RCL INT," +          // -=-
                                    "PRIMARY KEY (Offer, PersoNumber)," +   //Employee has only one role in an Offer -> unique (if he can have more adapt Primary Key)
                                    "CONSTRAINT fK_ofem FOREIGN KEY (Offer) REFERENCES  offer(Id) ON DELETE CASCADE, " +
                                    "CONSTRAINT fK_pnr_ofem FOREIGN KEY (PersoNumber) REFERENCES  employee(PersoNumber) ON DELETE CASCADE, " +
                                    "CONSTRAINT fK_role_ofem FOREIGN KEY (Role, RCL) REFERENCES role(name, rcl) ON DELETE  CASCADE" +
                            ");");
                con.Execute("IF OBJECT_ID('offerHasField', 'U') IS NULL " +             // Offer Has Field
                            "CREATE TABLE [offerHasField] ( " +
                                    "Offer INT, " +
                                    "Field VARCHAR(50), " +
                                    "PRIMARY KEY (Offer, Field), " +
                                    "CONSTRAINT fK_oid_offld FOREIGN KEY (Offer) REFERENCES offer(Id) ON DELETE CASCADE, " +
                                    "CONSTRAINT fK_field_offld FOREIGN KEY (Field) REFERENCES field(name) ON DELETE CASCADE " +
                            ");");
                con.Execute("IF OBJECT_ID('offerHasSkill', 'U') IS NULL " +             // Offer Has Skill
                            "CREATE TABLE [offerHasSkill] ( " +
                                    "Offer INT, " +
                                    "skill_name VARCHAR(50), " +
                                    "skill_cat VARCHAR(50), " +
                                    "skill_level INT, " +
                                    "PRIMARY KEY (Offer, skill_name, skill_cat, skill_level), " +
                                    "CONSTRAINT fK_oid_ofsk FOREIGN KEY (Offer) REFERENCES  offer(Id) ON DELETE CASCADE, " +
                                    "CONSTRAINT fK_sk_ofsk FOREIGN KEY (skill_name, skill_cat) REFERENCES skill(name, category) ON DELETE CASCADE, " +
                                    "CONSTRAINT fK_sklvl_ofsk FOREIGN KEY (skill_level) REFERENCES  skill_level(level) " +
                            ");");


                //-----------------------------------------------------------------------------------------------------
            }
            catch (SqlException e)
            {
                log.LogError($"CreateTables() SqlException  \n{e.Message}\n");
            }
            finally
            {
                con.Close();
            }
        }



        // public void DropTables()
        // {
        //     using var con = new SqlConnection(connectionString);
        //     try
        //     {
        //         con.Execute("IF OBJECT_ID('employeeHasAcrole', 'U') IS NOT  NULL " +      // Employee__AcRole
        //                     "Drop TABLE   [employeeHasAcrole] ;");
        //         con.Execute("IF OBJECT_ID('employeeHasField', 'U') IS NOT  NULL " +       // Employee__Field
        //                     "Drop TABLE   [employeeHasField] ;");
        //         con.Execute("IF OBJECT_ID('employeeHasRole', 'U') IS NOT  NULL " +        // Employee__Role
        //                     "Drop TABLE   [employeeHasRole] ;");
        //         con.Execute("IF OBJECT_ID('employeeHasLanguage', 'U') IS NOT  NULL " +    // Employee__Language
        //                     "Drop TABLE   [employeeHasLanguage] ;");
        //         con.Execute("IF OBJECT_ID('employeeHasSkill', 'U') IS NOT  NULL " +       // Employee__Skill
        //                     "Drop TABLE   [employeeHasSkill] ;");
        //         con.Execute("IF OBJECT_ID('activityHasEmployee', 'U') IS NOT  NULL " +    // Activity_in_Project__Employee
        //                     "Drop TABLE   [activityHasEmployee] ;");

        //         con.Execute("IF OBJECT_ID('projectHasActivity', 'U') IS NOT  NULL " +
        //                     "Drop TABLE   [projectHasActivity] ;");                              // ref by activity_done_by
        //         con.Execute("IF OBJECT_ID('projectHasPurpose', 'U') IS NOT  NULL " +
        //                     "Drop TABLE   [projectHasPurpose] ;");

        //         con.Execute("IF OBJECT_ID('Employee', 'U') IS NOT  NULL " +
        //                     "Drop TABLE   [Employee] ;");           //ref by employee_accessrole/field/role/laguage/skill
        //         con.Execute("IF OBJECT_ID('Project', 'U') IS NOT  NULL " +
        //                     "Drop TABLE   [Project] ;");
        //         // Drop Offer

        //         con.Execute("IF OBJECT_ID('Field', 'U') IS NOT  NULL " +
        //                     "Drop TABLE   [Field] ;");                                  // ref by emp_field, project
        //         con.Execute("IF OBJECT_ID('Role', 'U') IS NOT  NULL " +
        //                     "Drop TABLE   [Role] ;");                                   // ref by employee_role
        //         con.Execute("IF OBJECT_ID('Language', 'U') IS NOT  NULL " +
        //                     "Drop TABLE   [Language] ;");                               // ref by employee_language
        //         con.Execute("IF OBJECT_ID('Language_Level', 'U') IS NOT  NULL " +
        //                     "Drop TABLE   [Language_Level] ;");                         // ref by employee_language
        //         con.Execute("IF OBJECT_ID('Skill', 'U') IS NOT  NULL " +
        //                     "Drop TABLE   [Skill] ;");                                  // ref by employee_skill
        //         con.Execute("IF OBJECT_ID('SkillCategory', 'U') IS NOT  NULL " +
        //                   "Drop TABLE   [SkillCategory] ;");                            // ref by skill
        //         con.Execute("IF OBJECT_ID('Skill_Level', 'U') IS NOT  NULL " +
        //                     "Drop TABLE   [Skill_Level] ;");                            // ref by employee_skill
        //     }
        //     finally
        //     {
        //         con.Close();
        //     }
        // }


    }
}
