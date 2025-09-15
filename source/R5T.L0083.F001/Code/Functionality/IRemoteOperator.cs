using System;

using LibGit2Sharp;

using F10Y.T0011;

using R5T.T0132;


namespace R5T.L0083.F001
{
    /// <summary>
	/// Operator for <see cref="Remote"/>-related functionality.
	/// </summary>
    /// <remarks>
    /// See prior work in: R5T.F0019. 
    /// </remarks>
    [FunctionalityMarker]
    public partial interface IRemoteOperator : IFunctionalityMarker
    {
        [InstanceIdentity("BB977E2B-9745-4010-8E86-B02D19E7B588")]
        public string Get_Url(Remote remote)
        {
            var url = remote.Url;
            return url;
        }
    }
}
