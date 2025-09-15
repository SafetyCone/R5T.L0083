using System;

using LibGit2Sharp;

using F10Y.T0011;

using R5T.T0132;


namespace R5T.L0083.F001
{
    [FunctionalityMarker]
    public partial interface ICloneOptionsOperator : IFunctionalityMarker
    {
        [InstanceIdentity("D30392D3-5EE6-47D7-B4D7-217ABAF2CCF3")]
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
