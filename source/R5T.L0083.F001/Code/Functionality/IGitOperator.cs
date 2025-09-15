using System;

using F10Y.T0011;

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
        [InstanceIdentity("E42F0A64-ABC8-4FB8-88CD-C331BA196A9C")]
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

        [InstanceIdentity("5211E6C4-8EF9-4624-9DD2-DFA6DF2C059D")]
        public void Fetch_Remote(
            string repositoryDirectoryPath,
            string username,
            string password)
            => Instances.RepositoryOperator.Fetch_Origin(
                repositoryDirectoryPath,
                username,
                password);

        public DateTimeOffset Get_LatestCommit_Timestamp(string repositoryDirectoryPath)
        {
            var output = Instances.LibGit2SharpOperator.Get_LatestCommit_Timestamp(repositoryDirectoryPath);
            return output;
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

        [InstanceIdentity("DCDCF69B-AADA-49FE-A7B0-FF19D44D6A9C")]
        public string Get_RepositoryRemoteUrl(string pathInRepositoryDirectory)
        {
            var output = Instances.LibGit2SharpOperator.Get_RepositoryRemoteUrl(pathInRepositoryDirectory);
            return output;
        }

        /// <summary>
        /// Returns the <inheritdoc cref="Glossary.ForDirectories.RepositoryGitDirectory" path="/name"/> path, or null if no repository is found, or null if not found.
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
        /// Returns the <inheritdoc cref="Glossary.ForDirectories.RepositoryDirectory" path="/name"/> path given a file or directory path from within the repository,
        /// or null if not found.
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

        /// <summary>
        /// Determines if a local repository has unpulled changes on its main branch relative it origin remote.
        /// </summary>
        public bool Has_UnpulledChanges(
            string repositoryDirectoryPath,
            string username,
            string password)
            => Instances.RepositoryOperator.Has_UnpulledChanges(
                repositoryDirectoryPath,
                username,
                password);

        [InstanceIdentity("580CEF21-1323-4B78-87A3-554C8432F944")]
        public bool Has_UnpulledChanges_WithoutFetch(string repositoryDirectoryPath)
            => Instances.RepositoryOperator.Has_UnpulledChanges_WithoutFetch(repositoryDirectoryPath);

        public bool Has_OnlyUnpushedChanges(string repositoryDirectoryPath)
            => Instances.RepositoryOperator.Has_OnlyUnpushedChanges(repositoryDirectoryPath);

        [InstanceIdentity("74DDE904-5EA5-4B24-A18C-5369CF08123D")]
        public bool Has_UnpushedChanges(string repositoryDirectoryPath)
            => Instances.LibGit2SharpOperator.Has_UnpushedChanges(repositoryDirectoryPath);

        [InstanceIdentity("F79C2884-3BF8-4F73-AB28-B011FD6CF371")]
        public bool Is_GitRepository(string directoryPath)
            => Instances.RepositoryOperator.Is_Repository(directoryPath);

        public string[] Get_DifferencedOrStaged_FilePaths(string repositoryDirectoryPath)
            => Instances.RepositoryOperator.Get_DifferencedOrStaged_FilePaths(repositoryDirectoryPath);

        public string[] Get_DifferencedButUnstaged_FilePaths(string repositoryDirectoryPath)
            => Instances.RepositoryOperator.Get_DifferencedButUnstaged_FilePaths(repositoryDirectoryPath);

        public string[] Get_StagedButUncommitted_FilePaths(string repositoryDirectoryPath)
            => Instances.RepositoryOperator.Get_StagedButUncommitted_FilePaths(repositoryDirectoryPath);

        public string[] Get_UnaddedFilePaths(string repositoryDirectoryPath)
            => Instances.RepositoryOperator.Get_UnaddedFilePaths(repositoryDirectoryPath);

        public string[] List_UnstagedFilePaths(string repositoryDirectoryPath)
            => Instances.RepositoryOperator.List_UnstagedPaths(repositoryDirectoryPath);

        public void Pull_WithFetch(
            string repositoryDirectoryPath,
            string username,
            string password)
            => Instances.RepositoryOperator.Pull_WithFetch(
                repositoryDirectoryPath,
                username,
                password);

        [InstanceIdentity("8E3D6176-CCEA-4537-AE25-A2F2299D2AC9")]
        public void Pull_WithoutFetch(
            string repositoryDirectoryPath,
            string authorName,
            string authorEmail)
            => Instances.RepositoryOperator.Pull_WithoutFetch_IsMerge(
                repositoryDirectoryPath,
                authorName,
                authorEmail);

        public bool Push_WithoutStageAndCommit(
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

        /// <summary>
        /// As opposed to <see cref="Push_WithStageAndCommit(string, string, string, string, string, string)"/>, this method stages, commits, and pushes changes.
        /// </summary>
        public void Push_WithStageAndCommit(
            string repositoryDirectoryPath,
            string commitMessage,
            string authorName,
            string authorEmailAddress,
            string username,
            string password)
        {
            Instances.LibGit2SharpOperator.Push_WithStageAndCommit(
                repositoryDirectoryPath,
                commitMessage,
                authorName,
                authorEmailAddress,
                username,
                password);
        }

        /// <summary>
        /// Chooses <see cref="Push_WithoutStageAndCommit(string, string, string)"/> as the default.
        /// </summary>
        public void Push(
            string repositoryDirectoryPath,
            string commitMessage,
            string authorName,
            string authorEmailAddress,
            string username,
            string password)
            => this.Push_WithStageAndCommit(
                repositoryDirectoryPath,
                commitMessage,
                authorName,
                authorEmailAddress,
                username,
                password);

        public int Stage_UnstagedPaths(string repositoryDirectoryPath)
        {
            var output = Instances.LibGit2SharpOperator.Stage_UnstagedPaths(repositoryDirectoryPath);
            return output;
        }
    }
}
