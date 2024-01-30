using System;
using System.Linq;

using LibGit2Sharp;

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
            Instances.RepositoryOperator.Fetch(
                repositoryDirectoryPath,
                username,
                password);
        }

        public string Get_Remote_Origin_Url(string pathInRepositoryDirectory)
        {
            var repositoryDirectoryPath = Instances.RepositoryOperator.Discover_RepositoryDirectoryPath(pathInRepositoryDirectory);

            using var repository = Instances.RepositoryOperator.Get_Repository(repositoryDirectoryPath);

            var originRemoteUrl = Instances.RepositoryOperator.Get_Remote_Origin_Url(repository);
            return originRemoteUrl;
        }

        public bool Push_Changes(
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
                Instances.RepositoryOperator.Stage_Unstaged(repository);

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
    }
}
