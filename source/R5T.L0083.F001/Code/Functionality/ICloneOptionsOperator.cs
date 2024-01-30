using System;

using LibGit2Sharp;

using R5T.T0132;


namespace R5T.L0083.F001
{
    [FunctionalityMarker]
    public partial interface ICloneOptionsOperator : IFunctionalityMarker
    {
        public CloneOptions Get_CloneOptions(
            string username,
            string password)
        {
            var cloneOptions = new CloneOptions();

            Instances.FetchOptionsOperator.Set_CredentialsProvider(
                cloneOptions.FetchOptions,
                username,
                password);

            return cloneOptions;
        }
    }
}
