using System;

using LibGit2Sharp;
using LibGit2Sharp.Handlers;

using F10Y.T0011;

using R5T.T0132;


namespace R5T.L0083.F001
{
    [FunctionalityMarker]
    public partial interface ICredentialsOperator : IFunctionalityMarker
    {
        [InstanceIdentity("5B84B107-BCD9-4CC6-B2AE-3FB99607FC1B")]
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
