using System;

using LibGit2Sharp;

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
        public string Get_Url(Remote remote)
        {
            var url = remote.Url;
            return url;
        }
    }
}
