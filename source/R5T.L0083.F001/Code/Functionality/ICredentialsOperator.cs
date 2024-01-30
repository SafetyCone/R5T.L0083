using System;

using LibGit2Sharp;
using LibGit2Sharp.Handlers;

using R5T.T0132;


namespace R5T.L0083.F001
{
    [FunctionalityMarker]
    public partial interface ICredentialsOperator : IFunctionalityMarker
    {
        public CredentialsHandler Get_CredentialsHandler(
            string username,
            string password)
        {
            var output = new CredentialsHandler(
                (url, usernameFromUrl, types) =>
                new UsernamePasswordCredentials()
                {
                    Username = username,
                    Password = password,
                }
            );

            return output;
        }
    }
}
