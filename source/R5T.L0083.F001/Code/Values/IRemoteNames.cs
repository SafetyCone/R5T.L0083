using System;

using R5T.T0131;


namespace R5T.L0083.F001
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// See prior work in: R5T.F0019.IRemoteRepositoryNames, and R5T.T0008.GitHelper.
    /// </remarks>
    [ValuesMarker]
    public partial interface IRemoteNames : IValuesMarker
    {
        /// <summary>
        /// <para><value>origin</value></para>
        /// </summary>
        public string Origin => "origin";
    }
}
