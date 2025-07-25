using System;
using System.Collections.Generic;
using System.Linq;

using LibGit2Sharp;

using R5T.T0132;

using Glossary = R5T.Y0004.Glossary;


namespace R5T.L0083.F001
{
    /// <summary>
    /// Git repository functions.
    /// </summary>
    /// <remarks>
    /// <inheritdoc cref="Documentation.Project_SelfDescription" path="/summary"/>
    /// </remarks>
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

        public void Fetch_Origin(
            Repository repository,
            string username,
            string password)
        {
            var remote = Instances.RepositoryOperator.Get_Remote_Origin(repository);

            Instances.RepositoryOperator.Fetch(
                repository,
                remote,
                username,
                password);
        }

        // Adapted from here: https://github.com/libgit2/libgit2sharp/wiki/git-fetch
        public void Fetch_Origin(
            string repositoryDirectoryPath,
            string username,
            string password)
        {
            using var repository = Instances.RepositoryOperator.Get_Repository(repositoryDirectoryPath);

            this.Fetch_Origin(
                repository,
                username,
                password);
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

        /// <summary>
        /// Chooses <see cref="Fetch(Repository, string, string)"/> as the default.
        /// </summary>
        public void Fetch(
            Repository repository,
            string username,
            string password)
            => this.Fetch_Origin(
                repository,
                username,
                password);

        /// <summary>
        /// Chooses <see cref="Fetch_Origin(string, string, string)"/> as the default.
        /// </summary>
        public void Fetch(
            string repositoryDirectoryPath,
            string username,
            string password)
            => this.Fetch_Origin(
                repositoryDirectoryPath,
                username,
                password);

        public Repository From(string repositoryDirectoryPath)
        {
            var output = new Repository(repositoryDirectoryPath);
            return output;
        }

        public bool Has_Branch(
            Repository repository,
            string branchName,
            out Branch branch)
        {
            branch = repository.Branches[branchName];

            var output = branch != Instances.Values.Branch_NotFound;
            return output;
        }

        /// <inheritdoc cref="Get_DirectoryPath(Repository)"/>
        public string Get_Path(Repository repository)
            => this.Get_DirectoryPath(repository);

        public Branch Get_Branch(
            Repository repository,
            string branchName)
        {
            var hasBranch = this.Has_Branch(
                repository,
                branchName,
                out var output);

            if(!hasBranch)
            {
                var repositoryPath = this.Get_Path(repository);

                throw new Exception($"'{branchName}': Git repository branch not found in repository:\n{repositoryPath}");
            }

            return output;
        }

        /// <summary>
        /// Returns null if not found.
        /// </summary>
        public Branch Get_Branch_Main(Repository repository)
        {
            var has_Main = this.Has_Branch(
                repository,
                Instances.BranchNames.Main,
                out var mainBranch);

            if(has_Main)
            {
                return mainBranch;
            }
            else
            {
                // If the main branch does no exist, try master, the old, non-DEI name for the main branch.
                var has_Master = this.Has_Branch(
                    repository,
                    Instances.BranchNames.Master,
                    out var masterBranch);

                if(has_Master)
                {
                    return masterBranch;
                }
                else
                {
                    // Throw an exception.
                    var repositoryPath = this.Get_Path(repository);

                    throw new Exception($"Neither '{Instances.BranchNames.Main}' nor '{Instances.BranchNames.Master}' branches were found for repository:\n{repositoryPath}");
                }
            }
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

        /// <summary>
        /// Chooses <see cref="Has_UnpulledChanges_WithFetch(string, string, string)"/> as the default.
        /// </summary>
        public bool Has_UnpulledChanges(
            string repositoryDirectoryPath,
            string username,
            string password)
            => this.Has_UnpulledChanges_WithFetch(
                repositoryDirectoryPath,
                username,
                password);

        public bool Has_UnpulledChanges_WithFetch(
            string repositoryDirectoryPath,
            string username,
            string password)
        {
            using var repository = this.Get_Repository(repositoryDirectoryPath);

            var output = this.Has_UnpulledChanges_WithFetch(
                repository,
                username,
                password);

            return output;
        }

        public bool Has_UnpulledChanges_WithFetch(
            Repository repository,
            string username,
            string password)
        {
            this.Fetch(
                repository,
                username,
                password);

            var output = this.Has_UnpulledChanges_WithoutFetch(repository);
            return output;
        }

        public bool Has_UnpulledChanges_WithoutFetch(string repositoryDirectoryPath)
        {
            using var repository = this.Get_Repository(repositoryDirectoryPath);

            var output = this.Has_UnpulledChanges_WithoutFetch(repository);
            return output;
        }

        public bool Has_UnpulledChanges_WithoutFetch(Repository repository)
        {
            // Determine if the local master branch is behind the remote master branch.
            var mainBranch = this.Get_Branch_Main(repository);

            var isBehind = mainBranch.TrackingDetails.BehindBy;

            var output = isBehind.HasValue && isBehind.Value > 0;
            return output;
        }

        /// <summary>
        /// Note: only works after a fetch.
        /// </summary>
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

        /// <inheritdoc cref="Has_UnpushedChanges(Repository)"/>
        // Prior work in R5T.D0038.L0001.
        public bool Has_UnpushedChanges(string repositoryDirectoryPath)
        {
            using var repository = this.Get_Repository(repositoryDirectoryPath);

            var output = this.Has_UnpushedChanges(repository);
            return output;
        }

        /// <summary>
        /// As opposed to <see cref="Has_OnlyUnpushedChanges(Repository)"/>, this method also looks for differenced or staged files, not just unpushed commits.
        /// </summary>
        public bool Has_UnpushedChanges(Repository repository)
        {
            // Are there any differenced or staged files in the working copy?
            var anyDifferencedOrStagedFiles = this.Enumerate_DifferencedOrStaged_RelativeFilePaths(repository)
                .Any();

            if(anyDifferencedOrStagedFiles)
            {
                return true;
            }

            var anyOnlyUnpushedChanges = this.Has_OnlyUnpushedChanges(repository);
            if(anyOnlyUnpushedChanges)
            {
                return true;
            }

            // Finally, return false since there are no unpushed changes.
            return false;
        }

        /// <inheritdoc cref="Has_OnlyUnpushedChanges(Repository)"/>
        public bool Has_OnlyUnpushedChanges(string repositoryDirectoryPath)
        {
            using var repository = this.Get_Repository(repositoryDirectoryPath);

            var output = this.Has_OnlyUnpushedChanges(repository);
            return output;
        }

        /// <summary>
        /// As opposed to <see cref="Has_UnpushedChanges(Repository)"/>, this does not look for differenced or staged files, just unpushed commits.
        /// </summary>
        public bool Has_OnlyUnpushedChanges(Repository repository)
        {
            // Get the current branch.
            var currentBranch = repository.Head;

            // Is the current branch untracked? This indicates that it has never been pushed to the remote!
            var isUntracked = !currentBranch.IsTracking;
            if (isUntracked)
            {
                return true;
            }

            // Is the current branch ahead its remote tracking branch?
            var currentBranchLocalIsAheadOfRemote = currentBranch.TrackingDetails.AheadBy > 0;
            if (currentBranchLocalIsAheadOfRemote)
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
                .ToArray();

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
                .ToArray();

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
                .ToArray();

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
                .ToArray();

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

        public MergeResult Pull_WithFetch(
            string repositoryDirectoryPath,
            string username,
            string password)
        {
            using var repository = this.Get_Repository(repositoryDirectoryPath);

            var output = this.Pull_WithFetch(
                repository,
                username,
                password);

            return output;
        }

        public MergeResult Pull_WithFetch(
            Repository repository,
            string username,
            string password)
        {
            var pullOptions = new PullOptions
            {
                FetchOptions = Instances.FetchOptionsOperator.Get_FetchOptions(
                    username,
                    password),
                MergeOptions = new MergeOptions
                {
                    FastForwardStrategy = FastForwardStrategy.Default,
                    //OnCheckoutNotify
                }
            };

            var signature = repository.Config.BuildSignature(DateTimeOffset.Now);

            var output = Commands.Pull(
                repository,
                signature,
                pullOptions);

            return output;
        }

        public MergeResult Pull_WithoutFetch_IsMerge(
            string repositoryDirectoryPath,
            string authorName,
            string authorEmail)
        {
            using var repository = this.Get_Repository(repositoryDirectoryPath);

            var output = this.Pull_WithoutFetch_IsMerge(
                repository,
                authorName,
                authorEmail);

            return output;
        }

        public MergeResult Pull_WithoutFetch_IsMerge(
            Repository repository,
            string authorName,
            string authorEmail)
        {
            var mergeOptions = new MergeOptions
            {
                FastForwardStrategy = FastForwardStrategy.Default,
                //OnCheckoutNotify
            };

            var mainBranch = this.Get_Branch_Main(repository);

            //var signature = repository.Config.BuildSignature(DateTimeOffset.Now);
            var signature = new Signature(
                authorName,
                authorEmail,
                DateTimeOffset.Now);

            var output = repository.Merge(mainBranch.TrackedBranch, signature, mergeOptions);
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
        /// Returns the <inheritdoc cref="Glossary.ForDirectories.RepositoryGitDirectory" path="/name"/> path, or null if no repository is found.
        /// </summary>
        /// <param name="repositoryPath"><inheritdoc cref="Repository.Discover(string)" path="/returns"/></param>
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
