using System;

using LibGit2Sharp;

using R5T.T0132;


namespace R5T.L0083.F001
{
    [FunctionalityMarker]
    public partial interface ISignatureOperator : IFunctionalityMarker
    {
        public Signature Get_Signature(
            string name,
            string emailAddress)
        {
            // Use local time (since it will be implicitly converted to the offeet).
            var when = Instances.DateTimeOperator.Get_Now_Local();

            var output = new Signature(
                name,
                emailAddress,
                when);

            return output;
        }
    }
}
