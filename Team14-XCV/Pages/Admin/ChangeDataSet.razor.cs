using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using XCV.Data;
using XCV.Shared.Misc;

namespace XCV.Pages.Admin
{
    public  partial class ChangeDataSet
    {
        private string newField = "";
        private string newLang = "";
        private string lvlVal = "";
        private int lvlInt;
        private string skillCat = "";
        private string newSkill = "";
        private string skillRemo = "";
        private string[] skillLevel = new string[4];
        private IEnumerable<Role> newRoles;


        protected override void OnInitialized()
        {
            fieldService.ChangeEventHandel += OnChange;
            roleService.ChangeEventHandel += OnChange;
            langService.ChangeEventHandel += OnChange;
            skillService.ChangeEventHandel += OnChange;
            skillLevel = skillService.GetAllLevel();
            newRoles = roleService.GetAllRoles();
        }



        private void CreateField()
        {
            if (newField.Length > 1)
                fieldService.CreateField(new Field() { Name = newField });
        }
        private void RemoveField(Field fi)
        {
            fieldService.RemoveField(fi);
        }

        private void CreateLang()
        {
            if (newLang.Length > 1)
            {
                var oLang = langService.GetAllLanguages().ToList();
                oLang.Add(new Language() { Name = newLang });
                langService.UpdateAllLanguages(oLang);
                changeInfo.SuccesMessage = $"{newLang}: Sprache wurde hinzugefügt";
            }
        }
        private void RemoveLang(Language la)
        {
            var oLang = langService.GetAllLanguages().ToList();
            oLang.Remove(la);
            langService.UpdateAllLanguages(oLang);
        }
        private void IsertLvl()
        {
            if (lvlVal.Length > 1)
            {
                var oLvl = langService.GetAllLevel().ToList();
                oLvl.Insert(lvlInt, lvlVal);
                langService.UpdateAllLevels(oLvl.ToArray());
            }
        }
        private void CreateSkill()
        {
            if (skillCat.Length > 1 && newSkill.Length > 1)
            {
                var cat = new SkillCategory() { Name = skillCat };
                skillService.InsertSkill(new Skill() { Name = newSkill, Category = cat });
            }
        }
        private void RemoveSkill(Skill s)
        {
            skillService.DeleteSkill(s);
        }

        private void UpdateSkillLevels()
        {
            skillService.UpdateAllLevels(skillLevel);
        }

        private void UpdateRoles()
        {
            (_, int c, _) = roleService.UpdateAllRoles(newRoles);
            changeInfo.SuccesMessage = $"{c}: Löhne aktualisiert";
        }






        private ChangeResult changeInfo = new();
        private void OnChange(object sender, ChangeResult e)
        {
            changeInfo = e;
        }
    }
}
