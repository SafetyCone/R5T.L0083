using System;


namespace R5T.L0083.F001
{
    public class RemoteOperator : IRemoteOperator
    {
        #region Infrastructure

        public static IRemoteOperator Instance { get; } = new RemoteOperator();


        private RemoteOperator()
        {
        }

        #endregion
    }
}
