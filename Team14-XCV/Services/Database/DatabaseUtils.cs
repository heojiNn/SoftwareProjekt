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
            log = logger;
            config = IConfiguration;
            connectionString = config.GetConnectionString("MyLocalConnection");     //from appsettings.json
            databaseName = config.GetConnectionString("DatabaseName");              //from appsettings.json
        }



        public void CreateDatabase()
        {
            using var con = new SqlConnection(config.GetConnectionString("MyLocalWhithoutDB"));
            try
            {
                con.Open();
                con.Execute($"IF NOT EXISTS ( SELECT name  FROM sys.databases  WHERE name = N'{databaseName}' ) " +
                                                    $"CREATE DATABASE {databaseName};");
            }
            catch (SqlException e)
            {
                log.LogError($"DatabaseUtils.CreateDatabase() Excep:\n{e.Message}\n");
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public void CreateTables()
        {
            using var con = new SqlConnection(connectionString);
            try
            {
                //----------------------mostly lookup-tables set through databasis.json--------------------------------
                int rows = con.Execute("IF OBJECT_ID('field', 'U') IS NULL " +            //ref by employee_field
                                                    "CREATE TABLE field ( " +
                                                        "name VARCHAR(50) PRIMARY KEY " +
                                                    ");");
                rows = con.Execute("IF OBJECT_ID('role', 'U') IS NULL " +             //ref by employee_role
                                                    "CREATE TABLE role ( " +
                                                        "name VARCHAR(50)   , " +
                                                        "rcl INT            , " +
                                                        "wage DECIMAL(5, 2) NOT NULL " +
                                                        "PRIMARY KEY (name, rcl) " +
                                                    ");");
                rows = con.Execute("IF OBJECT_ID('language_name', 'U') IS NULL " +    //ref by employee_language
                                                    "CREATE TABLE language_name ( " +
                                                        "name VARCHAR(50) PRIMARY KEY " +
                                                    ");");
                rows = con.Execute("IF OBJECT_ID('language_level', 'U') IS NULL " +   //ref by employee_language
                                                    "CREATE TABLE language_level ( " +
                                                        "level INT UNIQUE, " +
                                                        "name VARCHAR(50) PRIMARY KEY " +
                                                    ");");

                rows = con.Execute("IF OBJECT_ID('skillcategory', 'U') IS NULL " +        //ref by skill
                                                    "CREATE TABLE skillcategory ( " +
                                                        "name VARCHAR(50)   PRIMARY KEY, " +
                                                        "parent VARCHAR(50) NOT NULL " + // foreinKey would lead to more work in insert
                                                    ");");
                rows = con.Execute("IF OBJECT_ID('skill', 'U') IS NULL " +               //ref by employee_skill
                                                    "CREATE TABLE skill ( " +
                                                        "name VARCHAR(50)    , " +
                                                        "category VARCHAR(50), " +
                                                        "PRIMARY KEY (name, category), " +
                                                        "CONSTRAINT fK_cat FOREIGN KEY (category) REFERENCES SkillCategory(name) ON DELETE CASCADE" +
                                                    ");");
                rows = con.Execute("IF OBJECT_ID('skill_level', 'U') IS NULL " +          //ref by employee_skill
                                                    "CREATE TABLE skill_level ( " +
                                                        "level INT PRIMARY KEY," +
                                                        "name VARCHAR(50) UNIQUE " +
                                                    ");");
                //-----------------------------------------------------------------------------------------------------

                rows = con.Execute("IF OBJECT_ID('employee', 'U') IS NULL " +     //ref by employee_accessrole/field/role/laguageskill
                                                       "CREATE TABLE employee ( " +
                                                           "PersoNumber VARCHAR(20) PRIMARY KEY, " +
                                                           "Password VARCHAR(20) NOT NULL, " +
                                                           "FirstName VARCHAR(50) NOT NULL, " +
                                                           "LastName VARCHAR(50) NOT NULL, " +
                                                           "Description VARCHAR(200) NOT NULL, " +
                                                           "Image VARCHAR(50) NOT NULL, " +
                                                           "RCL INT, " +
                                                           "Expirience DECIMAL(4, 2), " +
                                                           "WorkingSince DATE NOT NULL, " +
                                                           "MadeFirstChangesOnProfile BIT " +
                                                       ");");

                rows = con.Execute("IF OBJECT_ID('project', 'U') IS NULL " +
                                                    "CREATE TABLE project ( " +
                                                        "Id INT IDENTITY(1,1) PRIMARY KEY, " +
                                                        "Title VARCHAR(50)          , " +
                                                        "Description VARCHAR(1000)  , " +
                                                        "Start DATE                 , " +
                                                        "Ende   DATE                 , " +
                                                        "Field VARCHAR(50)      , " +
                                                        "CONSTRAINT fK_p_field FOREIGN KEY (Field) REFERENCES  field(Name) ON DELETE CASCADE " +
                                                    ");");

                //----------------------1 to n relation----------------------------------------------------------------
                rows = con.Execute("IF OBJECT_ID('project_purpose', 'U') IS NULL " +
                                                    "CREATE TABLE project_purpose ( " +
                                                        "Project INT       , " +
                                                        "Purpose VARCHAR(100) , " +
                                                        "PRIMARY KEY (project, purpose), " +
                                                        "CONSTRAINT fK_pro_pur FOREIGN KEY (project) REFERENCES  project(Id) ON DELETE CASCADE " +
                                                    ");");

                rows = con.Execute("IF OBJECT_ID('activitie', 'U') IS NULL " +  //ref by activitie_done_by
                                                    "CREATE TABLE activitie ( " +
                                                        "Project INT       , " +
                                                        "Activitie VARCHAR(100) , " +
                                                        "PRIMARY KEY (project, activitie), " +
                                                        "CONSTRAINT fK_ak_pro FOREIGN KEY (project) REFERENCES  project(Id) ON DELETE CASCADE " +
                                                    ");");
                //-----------------------------------------------------------------------------------------------------


                //----------------------n to m relation----------------------------------------------------------------
                rows = con.Execute("IF OBJECT_ID('employee_accessrole', 'U') IS NULL " +
                                                    "CREATE TABLE employee_accessrole ( " +
                                                        "employee VARCHAR(20), " +
                                                        "acrole INT         , " +
                                                        "PRIMARY KEY (employee, acrole), " +
                                                        "CONSTRAINT fK_a_emp FOREIGN KEY (employee) REFERENCES  employee(persoNumber) ON DELETE CASCADE " +
                                                    ");");
                rows = con.Execute("IF OBJECT_ID('employee_field', 'U') IS NULL " +
                                                    "CREATE TABLE employee_field ( " +
                                                        "employee VARCHAR(20)   , " +
                                                        "field VARCHAR(50)      , " +
                                                        "PRIMARY KEY (employee, field), " +
                                                        "CONSTRAINT fK_f_emp FOREIGN KEY (employee) REFERENCES  employee(persoNumber) ON DELETE CASCADE, " +
                                                        "CONSTRAINT fK_field FOREIGN KEY (field) REFERENCES  field(name) ON DELETE CASCADE " +
                                                    ");");
                rows = con.Execute("IF OBJECT_ID('employee_role', 'U') IS NULL " +
                                                    "CREATE TABLE employee_role ( " +
                                                        "employee VARCHAR(20), " +
                                                        "role VARCHAR(50)    , " +
                                                        "role_rcl INT        , " +
                                                        "PRIMARY KEY (employee, role), " +
                                                        "CONSTRAINT fK_r_emp FOREIGN KEY (employee) REFERENCES  employee(persoNumber) ON DELETE CASCADE, " +
                                                        "CONSTRAINT fK_role FOREIGN KEY (role, role_rcl) REFERENCES role(name, rcl) ON DELETE CASCADE" +
                                                    ");");
                rows = con.Execute("IF OBJECT_ID('employee_language', 'U') IS NULL " +
                                                    "CREATE TABLE employee_language ( " +
                                                        "employee VARCHAR(20)    , " +
                                                        "language VARCHAR(50)  , " +
                                                        "language_level VARCHAR(50) , " +
                                                        "PRIMARY KEY (employee, language, language_level), " +
                                                        "CONSTRAINT fK_l_emp FOREIGN KEY (employee) REFERENCES  employee(persoNumber) ON DELETE CASCADE, " +
                                                        "CONSTRAINT fK_lang FOREIGN KEY (language) REFERENCES  language_name(name) ON DELETE CASCADE, " +
                                                        "CONSTRAINT fK_lvl FOREIGN KEY (language_level) REFERENCES  language_level(name) ON DELETE CASCADE " +
                                                    ");");
                rows = con.Execute("IF OBJECT_ID('employee_skill', 'U') IS NULL " +
                                                    "CREATE TABLE employee_skill ( " +
                                                        "employee VARCHAR(20), " +
                                                        "skill_name VARCHAR(50) , " +
                                                        "skill_cat VARCHAR(50)  , " +
                                                        "skill_level INT, " +
                                                        "PRIMARY KEY (employee, skill_name, skill_cat, skill_level), " +
                                                        "CONSTRAINT fK_s_emp FOREIGN KEY (employee) REFERENCES  employee(persoNumber) ON DELETE CASCADE, " +
                                                        "CONSTRAINT fK_skill FOREIGN KEY (skill_name, skill_cat) REFERENCES skill(name, category) ON DELETE CASCADE, " +
                                                        "CONSTRAINT fK_lcl FOREIGN KEY (skill_level) REFERENCES  skill_level(level) " +
                                                    ");");


                rows = con.Execute("IF OBJECT_ID('activitie_done_by', 'U') IS NULL " +
                                                    "CREATE TABLE activitie_done_by ( " +
                                                        "Project INT       , " +
                                                        "Activitie VARCHAR(100) , " +
                                                        "Employee VARCHAR(20) , " +
                                                        "PRIMARY KEY (project, activitie, employee), " +
                                                        "CONSTRAINT fK_do_pro FOREIGN KEY (project, activitie) REFERENCES  activitie(project, activitie) ON DELETE CASCADE, " +
                                                        "CONSTRAINT fK_do_emp FOREIGN KEY (employee) REFERENCES  employee(PersoNumber) ON DELETE CASCADE " +
                                                    ");");
                //-----------------------------------------------------------------------------------------------------
            }
            catch (SqlException e)
            {
                log.LogError($"DatabaseUtils.CreateTables() Excep:\n{e.Message}\n");
            }
            finally
            {
                con.Close();
            }
        }

        public void Populate()
        {
            using var con = new SqlConnection(connectionString);
            try
            {
                // int rows = con.Execute("IF NOT EXISTS (  SELECT * " +
                //                                         "FROM field  WHERE name = '' ) " +
                //                             "Insert Into field Values ('');");
                int rows = con.Execute("IF NOT EXISTS (  SELECT * " +
                                                    "FROM Employee  WHERE persoNumber = '000' ) " +
                                            "Insert Into Employee Values " +
                                            "( '000', '000', 'admin', 'admin',      '', '',           null, 0, '2020-01-01', 0)," +  // 000 admin
                                            "( '001', '001', 'arnold', 'schwarzen', '', 'musterPic.png', 1, 3, '2020-06-20', 0)," +// 001 arnold
                                            "( '002', '002', 'brad', 'pitt',        '', 'musterPic.png', 2, 0, '2020-06-21', 0)," +// 002 brad
                                            "( '003', '003', 'charlie', 'chaplin',  '', 'musterPic.png', 3, 0, '2020-06-22', 0);");// 003 charlie
                rows = con.Execute("IF NOT EXISTS (  SELECT * " +
                                                    "FROM employee_AccessRole  WHERE employee = '000' ) " +
                                            "Insert Into employee_AccessRole Values " +             // admin=a  arnold=s  brad=s,a
                                            "( '000', 1), ( '001', 0), ( '002', 0), ( '002', 1);");
                rows = con.Execute("IF NOT EXISTS (  SELECT * " +
                                                        "FROM skill_level  WHERE level = 0 ) " +
                                            "Insert Into skill_level Values (0, ''), " +            //Required for SofSkills (0, '')
                                            " (1, 'hobby'), (2, 'produc'), (3, 'rege'), (4, 'exp') ;");

                rows = con.Execute("IF NOT EXISTS (  SELECT * " +
                                                        "FROM skillcategory  WHERE name = 'HardSkills' ) " +
                                            "Insert Into skillcategory Values " +
                                                "('HardSkills', ''), " +
                                                "('Sprachen und Frameworks', 'HardSkills'), " +
                                                "('Bibliotheken', 'Sprachen und Frameworks'), " +
                                                "('Sprachen', 'Sprachen und Frameworks'), " +
                                                "('SoftSkills', '') ;");
                rows = con.Execute("IF NOT EXISTS (  SELECT * " +
                                                        "FROM skill  WHERE name = 'C' ) " +
                                            "Insert Into skill Values " +
                                                "('Akquisitionsstärke', 'SoftSkills'), " +
                                                "('Automapper', 'Bibliotheken'), " +
                                                "('C', 'Sprachen'), " +
                                                "('CSS', 'Sprachen') ;");

                rows = con.Execute("IF NOT EXISTS (  SELECT * " +
                                                        "FROM field  WHERE name = '' ) " +
                                            "Insert Into field Values ('') ;");//Required for projects
                rows = con.Execute("IF NOT EXISTS (  SELECT * " +
                                                        "FROM field  WHERE name = 'Beratung' ) " +
                                            "Insert Into field Values ('Beratung'), ('Automobil') ;");
                rows = con.Execute("IF NOT EXISTS (  SELECT * " +
                                                        "FROM project  WHERE Title = 'Brille' ) " +
                                            "Insert Into project Values " +
                                                "('Brille', '', '2020-05-01', '2021-12-30', 'Beratung') ;");
                rows = con.Execute("IF EXISTS (  SELECT Id " +
                                                        "FROM project  WHERE Title = 'Brille' ) " +
                                            "Insert Into activitie Values " +
                                                "((  SELECT Id " +
                                                        "FROM project  WHERE Title = 'Brille' ), ''), " +
                                                "((  SELECT Id " +
                                                        "FROM project  WHERE Title = 'Brille' ), 'FrontEnd'), " +
                                                "((  SELECT Id " +
                                                        "FROM project  WHERE Title = 'Brille' ), 'BackEnd') ;");
            }
            catch (SqlException e)
            {
                log.LogError($"DatabaseUtils.Populate() Excep:\n{e.Message}\n");
            }
            finally
            {
                con.Close();
            }
        }

        public void DropTables()
        {
            using var con = new SqlConnection(connectionString);
            try
            {

                int rows = con.Execute("IF OBJECT_ID('employee_accessrole', 'U') IS NOT  NULL " +
                                                    "Drop TABLE employee_accessrole ;");
                rows = con.Execute("IF OBJECT_ID('employee_field', 'U') IS NOT  NULL " +
                                                    "Drop TABLE employee_field ;");
                rows = con.Execute("IF OBJECT_ID('employee_role', 'U') IS NOT  NULL " +
                                                    "Drop TABLE employee_role ;");
                rows = con.Execute("IF OBJECT_ID('employee_language', 'U') IS NOT  NULL " +
                                                    "Drop TABLE employee_language ;");
                rows = con.Execute("IF OBJECT_ID('employee_skill', 'U') IS NOT  NULL " +
                                                    "Drop TABLE employee_skill ;");
                rows = con.Execute("IF OBJECT_ID('activitie_done_by', 'U') IS NOT  NULL " +
                                                    "Drop TABLE activitie_done_by ;");
                rows = con.Execute("IF OBJECT_ID('activitie', 'U') IS NOT  NULL " +
                                                    "Drop TABLE activitie ;");
                rows = con.Execute("IF OBJECT_ID('project_purpose', 'U') IS NOT  NULL " +
                                                    "Drop TABLE project_purpose ;");
                //----------------------
                rows = con.Execute("IF OBJECT_ID('employee', 'U') IS NOT  NULL " +
                                                    "Drop TABLE employee ;");
                rows = con.Execute("IF OBJECT_ID('project', 'U') IS NOT  NULL " +
                                                    "Drop TABLE project ;");
                //----------------------
                rows = con.Execute("IF OBJECT_ID('field', 'U') IS NOT  NULL " +
                                                   "Drop TABLE field ;");
                rows = con.Execute("IF OBJECT_ID('role', 'U') IS NOT  NULL " +
                                                    "Drop TABLE role ;");
                rows = con.Execute("IF OBJECT_ID('language_name', 'U') IS NOT  NULL " +
                                                    "Drop TABLE language_name ;");
                rows = con.Execute("IF OBJECT_ID('language_level', 'U') IS NOT  NULL " +
                                                    "Drop TABLE language_level ;");
                rows = con.Execute("IF OBJECT_ID('skill', 'U') IS NOT  NULL " +
                                                    "Drop TABLE skill ;");
                rows = con.Execute("IF OBJECT_ID('skillcategory', 'U') IS NOT  NULL " +
                                                    "Drop TABLE skillcategory ;");
                rows = con.Execute("IF OBJECT_ID('skill_level', 'U') IS NOT  NULL " +
                                                    "Drop TABLE skill_level ;");
            }
            finally
            {
                con.Close();
            }
        }


        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
    }
}
