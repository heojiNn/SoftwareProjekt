using System;
using System.Collections.Generic;



namespace Team14.Data
{
    public interface IProfileService
    {


        // Summary:
        //     Those (three) might find Employyes
        //     1    everything
        //     2    (firstName == "")only check lastName  and viseversa     otherwise check both  case-insesitive
        //
        // Parameters:
        //   ...:
        //     to limit/reduce the the Result
        //
        // Returns:
        //     A List of Emplyees that migth be empty (but never null)
        //
        // Raises Events with:          in german
        //    EventArgs:NoResult.Message    tells that nothig was found for: <query>
        public IEnumerable<Employee> ShowAllProfiles();
        public IEnumerable<Employee> SearchProfiles(string firstName, string lastName);
        //public IEnumerable<Employee> SearchProfiles(IEnumerable<SearchKey> querry);

        // Summary:
        //     might be null  but only if Interface insn't used properly
        public Employee ShowProfile(string persNum);


        // Summary:
        //     Checkes Updates against Constrains
        //
        // Parameters:
        //   Profile:
        //     that contains  Profile.PersoNumber for reference
        //                    and the  new data for replacement
        //
        // Raises Events with:                  in german
        //    EventArgs:ChangeInfo.ChangeMessage    tells what will be changed
        //    EventArgs:ChangeInfo.ErrorMessage     tells that <this kind> of change is invalid
        public void CheckProfileUpdate(Employee newVersion);
        // public void AddProject(string persNum, Project project);
        // public void AddProject(string persNum, Project project, string activities


        // Raises Events with:                  in german
        //    EventArgs:ChangeInfo.ChangeMessage    tells what will be changed
        //    EventArgs:ChangeInfo.ErrorMessage     tells that this kind of change is invalid
        // Das <Projekt> mit der <Activität> existiert  nicht);

        // Summary:
        //     Commits a valid updates  into the persistence
        public void CommitProfileUpdate(Employee toCommit);

        public event EventHandler<NoResult> SearchEventHandel;
        public event EventHandler<ChangeResult> ChangeEventHandel;

    }
}
