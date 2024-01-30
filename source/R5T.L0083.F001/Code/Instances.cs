using System;


namespace R5T.L0083.F001
{
    public static class Instances
    {
        public static IBranchNames BranchNames => F001.BranchNames.Instance;
        public static ICloneOptionsOperator CloneOptionsOperator => F001.CloneOptionsOperator.Instance;
        public static ICredentialsOperator CredentialsOperator => F001.CredentialsOperator.Instance;
        public static L0066.IDateTimeOperator DateTimeOperator => L0066.DateTimeOperator.Instance;
        public static IFetchOptionsOperator FetchOptionsOperator => F001.FetchOptionsOperator.Instance;
        public static L0066.INullOperator NullOperator => L0066.NullOperator.Instance;
        public static IPushOptionsOperator PushOptionsOperator => F001.PushOptionsOperator.Instance;
        public static IRemoteNames RemoteNames => F001.RemoteNames.Instance;
        public static IRemoteOperator RemoteOperator => F001.RemoteOperator.Instance;
        public static IRepositoryOperator RepositoryOperator => F001.RepositoryOperator.Instance;
        public static ISignatureOperator SignatureOperator => F001.SignatureOperator.Instance;
    }
}