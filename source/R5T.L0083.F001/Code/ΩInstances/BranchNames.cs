using System;


namespace R5T.L0083.F001
{
    public class BranchNames : IBranchNames
    {
        #region Infrastructure

        public static IBranchNames Instance { get; } = new BranchNames();


        private BranchNames()
        {
        }

        #endregion
    }
}
