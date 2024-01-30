using System;


namespace R5T.L0083.F001
{
    public class CloneOptionsOperator : ICloneOptionsOperator
    {
        #region Infrastructure

        public static ICloneOptionsOperator Instance { get; } = new CloneOptionsOperator();


        private CloneOptionsOperator()
        {
        }

        #endregion
    }
}
