using System;

using LibGit2Sharp;

using R5T.T0131;


namespace R5T.L0083.F001
{
    [ValuesMarker]
    public partial interface IValues : IValuesMarker
    {
        /// <summary>
        /// Null
        /// </summary>
        public Branch Branch_NotFound => null;

        /// <summary>
        /// <para><value>&lt;null&gt;</value></para>
        /// The value returned by <see cref="LibGit2Sharp.Repository.Discover(string)"/> if no Git repository is found for the given path.
        /// </summary>
        public const string RepositoryNotFoundDirectoryPath_Constant = null;

        /// <inheritdoc cref="RepositoryNotFoundDirectoryPath_Constant"/>
        public string RepositoryNotFoundDirectoryPath => IValues.RepositoryNotFoundDirectoryPath_Constant;
    }
}
