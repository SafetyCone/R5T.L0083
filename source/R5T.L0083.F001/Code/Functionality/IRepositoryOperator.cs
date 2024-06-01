using System;
using System.Collections.Generic;
using System.Linq;

using LibGit2Sharp;

using R5T.T0132;

using Glossary = R5T.Y0004.Glossary;


namespace R5T.L0083.F001
{
    [FunctionalityMarker]
    public partial interface IRepositoryOperator : IFunctionalityMarker
    {
        public string Clone_NonIdempotent(
            string sourceUrl,
            string repositoryDirectoryPath,
            string username,
            string password)
        {
            var options = Instances.CloneOptionsOperator.Get_CloneOptions(
                username,
                password);

            // LibGit2Sharp will create the local repository directory if it exists.
            // LibGit2Sharp will also perform the safety check that local repository directory path is empty (if it exists).
            // LibGit2Sharp.NameConflictException, '{directory path}' exists and is not an empty directory
            var repositoryGitDirectoryPath = Repository.Clone(sourceUrl, repositoryDirectoryPath, options);
            return repositoryGitDirectoryPath;
        }

        public void Commit(
            Repository repository,
            string commitMessage,
            string authorName,
            string authorEmailAddress)
        {
            var hasAnyToCommit = this.Has_AnyToCommit(repository);
            if(hasAnyToCommit)
            {
                var authorSignature = Instances.SignatureOperator.Get_Signature(
                authorName,
                authorEmailAddress);

                var committerSignature = authorSignature;

                repository.Commit(
                    commitMessage,
                    authorSignature,
                    committerSignature);
            }
        }

        public void Commit(
            string repositoryDirectoryPath,
            string commitMessage,
            string authorName,
            string authorEmailAddress)
        {
            using var repository = this.Get_Repository(repositoryDirectoryPath);

            this.Commit(
                repository,
                commitMessage,
                authorName,
                authorEmailAddress);
        }

        // Prior work in R5T.L0001.Extensions.
        public string Discover_RepositoryDirectoryPath(string path)
        {
            var wasFound = this.Try_Discover_RepositoryDirectoryPath(
                path,
                out var repositoryPath);

            if(!wasFound)
            {
                throw new InvalidOperationException($"No Git repository found for '{path}'.");
            }

            return repositoryPath;
        }

        public void Fetch(
            Repository repository,
            Remote remote,
            string username,
            string password)
        {
            var fetchOptions = Instances.FetchOptionsOperator.Get_FetchOptions(
                username,
                password);

            var refSpecs = remote.FetchRefSpecs.Select(x => x.Specification);

            var logMessage = String.Empty;

            Commands.Fetch(repository, remote.Name, refSpecs, fetchOptions, logMessage);
        }

        // Adapted from here: https://github.com/libgit2/libgit2sharp/wiki/git-fetch
        public void Fetch(
            string repositoryDirectoryPath,
            string username,
            string password)
        {
            using var repository = Instances.RepositoryOperator.Get_Repository(repositoryDirectoryPath);

            var remote = Instances.RepositoryOperator.Get_Remote_Origin(repository);

            Instances.RepositoryOperator.Fetch(
                repository,
                remote,
                username,
                password);
        }

        /// <summary>
        /// Returns null if not found.
        /// </summary>
        public Branch Get_Branch_Main(Repository repository)
        {
            var mainBranch = repository.Branches[Instances.BranchNames.Main];

            // If the main branch does no exist, try the old name for the main branch.
            mainBranch ??= repository.Branches[Instances.BranchNames.Master];

            return mainBranch;
        }

        public string Get_LatesRevision_ForMainBranch(Repository repository)
        {
            var masterBranch = this.Get_Branch_Main(repository);

            var tipCommit = masterBranch.Tip;

            var revisionIdentity = tipCommit.Sha;
            return revisionIdentity;
        }

        public string Get_DirectoryPath(Repository repository)
        {
            var gitDirectoryPath = this.Get_GitDirectoryPath(repository);

            var output = Instances.PathOperator.Get_ParentDirectoryPath(
                gitDirectoryPath);

            return output;
        }

        public string Get_GitDirectoryPath(Repository repository)
            => repository.Info.Path;

        public string Get_Remote_Origin_Url(Repository repository)
        {
            var originRemote = this.Get_Remote_Origin(repository);

            var originRemoteUrl = Instances.RemoteOperator.Get_Url(originRemote);
            return originRemoteUrl;
        }

        public Remote Get_Remote_Origin(Repository repository)
        {
            var output = repository.Network.Remotes[Instances.RemoteNames.Origin];
            return output;
        }

        public Repository Get_Repository(string repositoryDirectoryPath)
        {
            var output = new Repository(repositoryDirectoryPath);
            return output;
        }

        /// <summary>
        /// Quality-of-life overload for <see cref="Has_AnyStaged(Repository)"/>.
        /// </summary>
        public bool Has_AnyToCommit(Repository repository)
        {
            var output = this.Has_AnyStaged(repository);
            return output;
        }

        public bool Has_AnyStaged(Repository repository)
        {
            var output = repository.Index
                .Where(x => x.StageLevel == StageLevel.Staged)
                .Any();

            return output;
        }

        // Prior work in R5T.D0038.L0001.
        public bool Has_UnpulledMainBranchChanges(string repositoryDirectoryPath)
        {
            using var repository = this.Get_Repository(repositoryDirectoryPath);

            // Determine if the local master branch is behind the remote master branch.
            var mainBranch = this.Get_Branch_Main(repository);

            var isBehind = mainBranch.TrackingDetails.BehindBy;

            var hasUnpulledMasterBranchChanges = isBehind.HasValue && isBehind.Value > 0;

            return hasUnpulledMasterBranchChanges;
        }

        // Prior work in R5T.D0038.L0001.
        public bool Has_UnpushedChanges(string repositoryDirectoryPath)
        {
            using var repository = this.Get_Repository(repositoryDirectoryPath);

            var output = this.Has_UnpushedChanges(repository);
            return output;
        }

        public bool Has_UnpushedChanges(Repository repository)
        {
            // Are there any differenced or staged files in the working copy?
            var anyDifferencedStagedFiles = this.Enumerate_DifferencedOrStaged_RelativeFilePaths(repository)
                .Any();

            if(anyDifferencedStagedFiles)
            {
                return true;
            }

            // Get the current branch.
            var currentBranch = repository.Head;

            // Is the current branch untracked? This indicates that it has not been pushed to the remote!
            var isUntracked = !currentBranch.IsTracking;
            if(isUntracked)
            {
                return true;
            }

            // Is the current branch ahead its remote tracking branch?
            var currentBranchLocalIsAheadOfRemote = currentBranch.TrackingDetails.AheadBy > 0;
            if(currentBranchLocalIsAheadOfRemote)
            {
                return true;
            }

            // Finally, return false since there are no unpushed changes.
            return false;
        }

        public bool Is_Repository(string directoryPath)
        {
            var output = Repository.IsValid(directoryPath);
            return output;
        }

        public string[] List_UnstagedPaths(string repositoryDirectoryPath)
        {
            using var repository = this.Get_Repository(repositoryDirectoryPath);

            var output = this.List_UnstagedPaths(repository);
            return output;
        }

        public string[] List_UnstagedPaths(Repository repository)
        {
            var unstagedPaths = repository.Diff.Compare<TreeChanges>(
                repository.Head.Tip.Tree,
                DiffTargets.WorkingDirectory)
                .Select(xChange => xChange.Path)
                .ToArray();

            return unstagedPaths;
        }

        public string[] Get_DifferencedOrStaged_FilePaths(string repositoryDirectoryPath)
        {
            using var repository = this.Get_Repository(repositoryDirectoryPath);

            var output = this.Enumerate_DifferencedOrStaged_FilePaths(repository)
                .Now();

            return output;
        }

        public IEnumerable<string> Enumerate_DifferencedOrStaged_FilePaths(Repository repository)
        {
            var repositoryDirectoryPath = this.Get_DirectoryPath(repository);

            var output = this.Enumerate_DifferencedOrStaged_RelativeFilePaths(repository)
                .Select(relativeFilePath => Instances.PathOperator.Get_FilePath(
                    repositoryDirectoryPath,
                    relativeFilePath))
                ;

            return output;
        }

        public IEnumerable<string> Enumerate_DifferencedOrStaged_RelativeFilePaths(Repository repository)
        {
            var differencedFilePaths = this.Enumerate_DifferencedOrStaged_Changes(repository)
                .Select(xChange => xChange.Path)
                ;

            return differencedFilePaths;
        }

        public string[] Get_DifferencedButUnstaged_FilePaths(string repositoryDirectoryPath)
        {
            using var repository = this.Get_Repository(repositoryDirectoryPath);

            var output = this.Enumerate_DifferencedButUnstaged_FilePaths(repository)
                .Now();

            return output;
        }

        public IEnumerable<string> Enumerate_DifferencedButUnstaged_FilePaths(Repository repository)
        {
            var repositoryDirectoryPath = this.Get_DirectoryPath(repository);

            var output = this.Enumerate_DifferencedButUnstaged_RelativeFilePaths(repository)
                .Select(relativeFilePath => Instances.PathOperator.Get_FilePath(
                    repositoryDirectoryPath,
                    relativeFilePath))
                ;

            return output;
        }

        public IEnumerable<string> Enumerate_DifferencedButUnstaged_RelativeFilePaths(Repository repository)
        {
            var unaddedFilePaths = this.Enumerate_DifferencedButUnstaged_Changes(repository)
                .Select(xChange => xChange.Path)
               // For debug.
               //.Select(xChange =>
               //{
               //    Console.WriteLine($"{xChange.Status}: {xChange.Path}");

               //    return xChange.Path;
               //})
               ;

            return unaddedFilePaths;
        }

        public string[] Get_StagedButUncommitted_FilePaths(string repositoryDirectoryPath)
        {
            using var repository = this.Get_Repository(repositoryDirectoryPath);

            var output = this.Enumerate_StagedButUncommitted_FilePaths(repository)
                .Now();

            return output;
        }

        public IEnumerable<string> Enumerate_StagedButUncommitted_FilePaths(Repository repository)
        {
            var repositoryDirectoryPath = this.Get_DirectoryPath(repository);

            var output = this.Enumerate_StagedButUncommitted_RelativeFilePaths(repository)
                .Select(relativeFilePath => Instances.PathOperator.Get_FilePath(
                    repositoryDirectoryPath,
                    relativeFilePath))
                ;

            return output;
        }

        public IEnumerable<string> Enumerate_StagedButUncommitted_RelativeFilePaths(Repository repository)
        {
            var unaddedFilePaths = this.Enumerate_StagedButUncommitted_Changes(repository)
                .Select(xChange => xChange.Path)
                ;

            return unaddedFilePaths;
        }

        public IEnumerable<TreeEntryChanges> Enumerate_DifferencedButUnstaged_Changes(Repository repository)
        {
            var output = repository.Diff.Compare<TreeChanges>(
                repository.Head.Tip.Tree,
                DiffTargets.WorkingDirectory)
                ;

            return output;
        }

        public IEnumerable<TreeEntryChanges> Enumerate_StagedButUncommitted_Changes(Repository repository)
        {
            var output = repository.Diff.Compare<TreeChanges>(
                repository.Head.Tip.Tree,
                DiffTargets.Index)
                ;

            return output;
        }

        public IEnumerable<TreeEntryChanges> Enumerate_DifferencedOrStaged_Changes(Repository repository)
        {
            var output = repository.Diff.Compare<TreeChanges>(
                repository.Head.Tip.Tree,
                DiffTargets.Index | DiffTargets.WorkingDirectory)
                ;

            return output;
        }

        public string[] Get_UnaddedFilePaths(string repositoryDirectoryPath)
        {
            using var repository = this.Get_Repository(repositoryDirectoryPath);

            var output = this.Enumerate_UnaddedFilePaths(repository)
                .Now();

            return output;
        }

        public IEnumerable<string> Enumerate_UnaddedFilePaths(Repository repository)
        {
            var repositoryDirectoryPath = this.Get_DirectoryPath(repository);

            var output = this.Enumerate_UnaddedRelativeFilePaths(repository)
                .Select(relativeFilePath => Instances.PathOperator.Get_FilePath(
                    repositoryDirectoryPath,
                    relativeFilePath))
                ;

            return output;
        }

        public IEnumerable<string> Enumerate_UnaddedRelativeFilePaths(Repository repository)
        {
            var output = this.Enumerate_DifferencedButUnstaged_Changes(repository)
                // Because these are changes, for untracked files, the change relative to the local repository would be an addition.
                // So the change is "added" not "untraced".
                .Where(change => change.Status == ChangeKind.Added)
                .Select(change => change.Path)
                ;

            return output;
        }

        public void Push_HeadToOrigin(
            string repositoryDirectoryPath,
            string username,
            string password)
        {
            using var repository = new Repository(repositoryDirectoryPath);

            this.Push_HeadToOrigin(
                repository,
                username,
                password);
        }

        public void Push_HeadToOrigin(
            Repository repository,
            string username,
            string password)
        {
            var pushOptions = Instances.PushOptionsOperator.Get_PushOptions(
                username,
                password);

            repository.Network.Push(
                repository.Head,
                pushOptions);
        }

        /// <summary>
        /// Quality-of-life overload for <see cref="Push_HeadToOrigin(Repository, string, string)"/>.
        /// </summary>
        public void Push(
            Repository repository,
            string username,
            string password)
        {
            this.Push_HeadToOrigin(
                repository,
                username,
                password);
        }

        /// <summary>
        /// Quality-of-life overload for <see cref="Push_HeadToOrigin(string, string, string)"/>.
        /// </summary>
        public void Push(
            string repositoryDirectoryPath,
            string username,
            string password)
        {
            this.Push_HeadToOrigin(
                repositoryDirectoryPath,
                username,
                password);
        }

        public int Stage_UnstagedPaths(string repositoryDirectoryPath)
        {
            using var repository = this.Get_Repository(repositoryDirectoryPath);

            var output = this.Stage_UnstagedPaths(repository);
            return output;
        }

        public int Stage_UnstagedPaths(Repository repository)
        {
            var unstagedPaths = this.List_UnstagedPaths(repository);

            this.Stage(
                repository,
                unstagedPaths);

            var output = unstagedPaths.Length;
            return output;
        }

        public void Stage(
            string repositoryDirectoryPath,
            IEnumerable<string> filePaths)
        {
            using var repository = this.Get_Repository(repositoryDirectoryPath);

            this.Stage(
                repository,
                filePaths);
        }

        public void Stage(
            Repository repository,
            IEnumerable<string> filePaths)
        {
            var anyFilePaths = filePaths.Any();
            if (anyFilePaths)
            {
                Commands.Stage(
                    repository,
                    filePaths);
            }
        }

        /// <summary>
        /// Returns the <inheritdoc cref="Glossary.ForDirectories.RepositoryGitDirectory" path="/name"/> path.
        /// </summary>
        // Prior work in R5T.L0001.Extensions.
        public bool Try_Discover_RepositoryDirectoryPath(
            string path,
            out string repositoryPath)
        {
            repositoryPath = Repository.Discover(path);

            var wasFound = this.WasFound_RepositoryDirectory(repositoryPath);
            return wasFound;
        }

        /// <summary>
        /// Evaluates the output of <see cref="Repository.Discover(string)"/> to determine if a repository was discovered.
        /// </summary>
        // Prior work in R5T.L0001.Extensions.
        public bool WasFound_RepositoryDirectory(string repositoryDirectoryDiscoveryResult)
        {
            var wasFound = Instances.NullOperator.Is_NotNull(repositoryDirectoryDiscoveryResult);
            return wasFound;
        }
    }
}
