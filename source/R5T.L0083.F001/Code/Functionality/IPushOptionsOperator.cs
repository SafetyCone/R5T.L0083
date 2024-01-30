using System;

using LibGit2Sharp;

using R5T.T0132;


namespace R5T.L0083.F001
{
    [FunctionalityMarker]
    public partial interface IPushOptionsOperator : IFunctionalityMarker
    {
        public PushOptions Get_PushOptions(
            string username,
            string password)
        {
            var pushOptions = new PushOptions
            {
                CredentialsProvider = Instances.CredentialsOperator.Get_CredentialsHandler(
                    username,
                    password)
            };

            return pushOptions;
        }
    }
}
