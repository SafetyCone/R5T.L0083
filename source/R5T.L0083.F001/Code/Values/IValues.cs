using System;

using R5T.T0131;


namespace R5T.L0083.F001
{
    [ValuesMarker]
    public partial interface IValues : IValuesMarker
    {
        /// <summary>
        /// <para><value>&lt;null&gt;</value></para>
        /// The value returned by <see cref="LibGit2Sharp.Repository.Discover(string)"/> if no Git repository is found for the given path.
        /// </summary>
        public string RepositoryNotFoundDirectoryPath => null;
    }
}
