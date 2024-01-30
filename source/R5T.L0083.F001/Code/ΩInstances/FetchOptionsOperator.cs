using System;


namespace R5T.L0083.F001
{
    public class FetchOptionsOperator : IFetchOptionsOperator
    {
        #region Infrastructure

        public static IFetchOptionsOperator Instance { get; } = new FetchOptionsOperator();


        private FetchOptionsOperator()
        {
        }

        #endregion
    }
}
