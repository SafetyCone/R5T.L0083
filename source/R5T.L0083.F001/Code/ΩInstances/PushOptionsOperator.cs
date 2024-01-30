using System;


namespace R5T.L0083.F001
{
    public class PushOptionsOperator : IPushOptionsOperator
    {
        #region Infrastructure

        public static IPushOptionsOperator Instance { get; } = new PushOptionsOperator();


        private PushOptionsOperator()
        {
        }

        #endregion
    }
}
