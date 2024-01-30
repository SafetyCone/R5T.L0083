using System;


namespace R5T.L0083.F001
{
    public class RemoteNames : IRemoteNames
    {
        #region Infrastructure

        public static IRemoteNames Instance { get; } = new RemoteNames();


        private RemoteNames()
        {
        }

        #endregion
    }
}
