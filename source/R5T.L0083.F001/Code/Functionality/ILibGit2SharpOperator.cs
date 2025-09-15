using System;

using LibGit2Sharp;

using F10Y.T0011;

using R5T.T0132;


namespace R5T.L0083.F001
{
    /// <summary>
    /// A LibGit2Sharp-based Git operator.
    /// </summary>
    /// <remarks>
    /// Prior work in:
    /// <list type="bullet">
    /// <item>R5T.D0038.L0001</item>
    /// <item>R5T.F0019</item>
    /// </list>
    /// </remarks>
    [FunctionalityMarker]
    public partial interface ILibGit2SharpOperator : IFunctionalityMarker
    {
        /// <summary>
        /// Non-idempotently clones a remote Git repository to a local directory path.
        /// An error will be thrown if the local directory is not empty, if it exists.
        /// </summary>
        /// <returns>
        /// The local repository .git directory path.
        /// </returns>
        [InstanceIdentity("D8F64EE6-D3B7-45BD-BAD8-8DAB98420E7B")]
        public string Clone_NonIdempotent(
            string sourceUrl,
            string repositoryDirectoryPath,
            string username,
            string password)
        {
            return Instances.RepositoryOperator.Clone_NonIdempotent(
                sourceUrl,
                repositoryDirectoryPath,
                username,
                password);
        }

        public void Commit(
            string repositoryDirectoryPath,
            string commitMessage,
            string authorName,
            string authorEmailAddress)
        {
            Instances.RepositoryOperator.Commit(
                repositoryDirectoryPath,
                commitMessage,
                authorName,
                authorEmailAddress);
        }

        public void Fetch(
            string repositoryDirectoryPath,
            string username,
            string password)
        {
            Instances.RepositoryOperator.Fetch_Origin(
                repositoryDirectoryPath,
                username,
                password);
        }

        public DateTimeOffset Get_LatestCommit_Timestamp(Repository repository)
        {
            // Get the HEAD's latest commit
            var latestCommit = repository.Head.Tip;

            // Get the commit date (use Author.When for authorship date or Committer.When for commit date)
            var commitDate = latestCommit.Committer.When;
            return commitDate;
        }

        public DateTimeOffset Get_LatestCommit_Timestamp(string repositoryDirectoryPath)
        {
            using var repository = Instances.RepositoryOperator.From(repositoryDirectoryPath);

            var output = this.Get_LatestCommit_Timestamp(repository);
            return output;
        }

        /// <summary>
        /// Quality-of-life overload for <see cref="Get_Remote_Origin_Url(string)"/>.
        /// </summary>
        [InstanceIdentity("BD5F04FB-5290-4A73-8DF7-F72EAC4FC223")]
        public string Get_RepositoryRemoteUrl(string pathInRepositoryDirectory)
        {
            var output = this.Get_Remote_Origin_Url(pathInRepositoryDirectory);
            return output;
        }

        public string Get_Remote_Origin_Url(string pathInRepositoryDirectory)
        {
            var repositoryDirectoryPath = Instances.RepositoryOperator.Discover_RepositoryDirectoryPath(pathInRepositoryDirectory);

            using var repository = Instances.RepositoryOperator.Get_Repository(repositoryDirectoryPath);

            var originRemoteUrl = Instances.RepositoryOperator.Get_Remote_Origin_Url(repository);
            return originRemoteUrl;
        }

        [InstanceIdentity("0FACB49E-7A27-42B7-B678-473F16355789")]
        public bool Has_UnpushedChanges(string repositoryDirectoryPath)
            => Instances.RepositoryOperator.Has_UnpushedChanges(repositoryDirectoryPath);

        public bool Is_GitRepository(string directoryPath)
            => Instances.RepositoryOperator.Is_Repository(directoryPath);

        public bool Push_WithStageAndCommit(
            string repositoryDirectoryPath,
            string commitMessage,
            string authorName,
            string authorEmailAddress,
            string username,
            string password)
        {
            using var repository = Instances.RepositoryOperator.Get_Repository(repositoryDirectoryPath);

            var hasUnpushedChanges = Instances.RepositoryOperator.Has_UnpushedChanges(repository);
            if(hasUnpushedChanges)
            {
                Instances.RepositoryOperator.Stage_UnstagedPaths(repository);

                Instances.RepositoryOperator.Commit(
                    repository,
                    commitMessage,
                    authorName,
                    authorEmailAddress);

                Instances.RepositoryOperator.Push(
                    repository,
                    username,
                    password);
            }

            return hasUnpushedChanges;
        }

        public bool Push(
            string repositoryDirectoryPath,
            string username,
            string password)
        {
            using var repository = Instances.RepositoryOperator.Get_Repository(repositoryDirectoryPath);

            var hasUnpushedChanges = Instances.RepositoryOperator.Has_UnpushedChanges(repository);
            if (hasUnpushedChanges)
            {
                Instances.RepositoryOperator.Push(
                    repository,
                    username,
                    password);
            }

            return hasUnpushedChanges;
        }

        public int Stage_UnstagedPaths(string repositoryDirectoryPath)
        {
            var output = Instances.RepositoryOperator.Stage_UnstagedPaths(repositoryDirectoryPath);
            return output;
        }
    }
}
