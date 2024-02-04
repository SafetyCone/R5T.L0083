using System;

using R5T.T0132;

using Glossary = R5T.Y0004.Glossary;


namespace R5T.L0083.F001
{
    /// <summary>
    /// A <see cref="ILibGit2SharpOperator"/>-based Git operator.
    /// </summary>
    [FunctionalityMarker]
    public partial interface IGitOperator : IFunctionalityMarker
    {
        /// <inheritdoc cref="ILibGit2SharpOperator.Clone_NonIdempotent(string, string, string, string)" path="/summary"/>
        /// <inheritdoc cref="ILibGit2SharpOperator.Clone_NonIdempotent(string, string, string, string)" path="/returns"/>
        /// <remarks>
        /// Quality-of-life forward for <see cref="ILibGit2SharpOperator.Clone_NonIdempotent(string, string, string, string)"/>.
        /// </remarks>
        public string Clone_NonIdempotent(
            string sourceUrl,
            string repositoryDirectoryPath,
            string username,
            string password)
        {
            var output = Instances.LibGit2SharpOperator.Clone_NonIdempotent(
                sourceUrl,
                repositoryDirectoryPath,
                username,
                password);

            return output;
        }

        /// <inheritdoc cref="ILibGit2SharpOperator.Commit(string, string, string, string)" path="/summary"/>
        /// <remarks>
        /// Quality-of-life forward for <see cref="ILibGit2SharpOperator.Commit(string, string, string, string)"/>.
        /// </remarks>
        public void Commit(
            string repositoryDirectoryPath,
            string commitMessage,
            string authorName,
            string authorEmailAddress)
        {
            Instances.LibGit2SharpOperator.Commit(
                repositoryDirectoryPath,
                commitMessage,
                authorName,
                authorEmailAddress);
        }

        public string Get_RepositoryDirectoryPath(string path)
        {
            var wasFound = this.Has_Repository(
                path,
                out var repositoryDirectoryPath);

            if (!wasFound)
            {
                throw new Exception($"Not git repository was found for path:\n{path}");
            }

            return repositoryDirectoryPath;
        }

        public string Get_RepositoryRemoteUrl(string pathInRepositoryDirectory)
        {
            var output = Instances.LibGit2SharpOperator.Get_RepositoryRemoteUrl(pathInRepositoryDirectory);
            return output;
        }

        /// <summary>
        /// Returns the <inheritdoc cref="Glossary.ForDirectories.RepositoryGitDirectory" path="/name"/> path.
        /// </summary>
        public bool Has_Repository_GitDirectory(
            string path,
            out string gitDirectoryPath_OrNotFound)
        {
            var repositoryGitDirectoryPathWasFound = Instances.RepositoryOperator.Try_Discover_RepositoryDirectoryPath(
                path,
                out gitDirectoryPath_OrNotFound);

            return repositoryGitDirectoryPathWasFound;
        }

        /// <summary>
        /// Returns the <inheritdoc cref="Glossary.ForDirectories.RepositoryDirectory" path="/name"/> path given a file or directory path from within the repository.
        /// </summary>
        public bool Has_Repository(
            string path,
            out string repositoryDirectoryPath_OrNotFound)
        {
            var wasFound_GitDirectory = this.Has_Repository_GitDirectory(
                path,
                out var gitDirectoryPath_OrNotFound);

            repositoryDirectoryPath_OrNotFound = wasFound_GitDirectory
                ? Instances.PathOperator.Get_ParentDirectoryPath_ForDirectory(gitDirectoryPath_OrNotFound)
                : Instances.Values.RepositoryNotFoundDirectoryPath
                ;

            return wasFound_GitDirectory;
        }

        public bool Has_UnpushedChanges(string repositoryDirectoryPath)
        {
            var output = Instances.LibGit2SharpOperator.Has_UnpushedChanges(repositoryDirectoryPath);
            return output;
        }

        public bool Push(
            string repositoryDirectoryPath,
            string username,
            string password)
        {
            var output = Instances.LibGit2SharpOperator.Push(
                repositoryDirectoryPath,
                username,
                password);

            return output;
        }

        public int Stage_UnstagedPaths(string repositoryDirectoryPath)
        {
            var output = Instances.LibGit2SharpOperator.Stage_UnstagedPaths(repositoryDirectoryPath);
            return output;
        }
    }
}
