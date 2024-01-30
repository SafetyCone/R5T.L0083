using System;


namespace R5T.L0083.F001
{
    public class CredentialsOperator : ICredentialsOperator
    {
        #region Infrastructure

        public static ICredentialsOperator Instance { get; } = new CredentialsOperator();


        private CredentialsOperator()
        {
        }

        #endregion
    }
}
