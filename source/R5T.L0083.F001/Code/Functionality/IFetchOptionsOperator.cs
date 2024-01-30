using System;

using LibGit2Sharp;
using LibGit2Sharp.Handlers;

using R5T.T0132;


namespace R5T.L0083.F001
{
    [FunctionalityMarker]
    public partial interface IFetchOptionsOperator : IFunctionalityMarker
    {
        public FetchOptions Get_FetchOptions(
            string username,
            string password)
        {
            var fetchOptions = new FetchOptions();

            this.Set_CredentialsProvider(
                fetchOptions,
                username,
                password);

            return fetchOptions;
        }

        public void Set_CredentialsProvider(
            FetchOptions fetchOptions,
            string username,
            string password)
        {
            fetchOptions.CredentialsProvider = Instances.CredentialsOperator.Get_CredentialsHandler(
                username,
                password);
        }
    }
}
