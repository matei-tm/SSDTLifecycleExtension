﻿namespace SSDTLifecycleExtension.Shared.Contracts.DataAccess
{
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    public interface IDacAccess
    {
        Task<(string DeployScriptContent,
            string DeployReportContent,
            string PreDeploymentScript,
            string PostDeploymentScript,
            string[] Errors)> CreateDeployFilesAsync([NotNull] string previousVersionDacpacPath,
                                                     [NotNull] string newVersionDacpacPath,
                                                     [NotNull] string publishProfilePath,
                                                     bool createDeployScript,
                                                     bool createDeployReport);

        Task<(DefaultConstraint[] DefaultConstraints, string[] Errors)> GetDefaultConstraintsAsync([NotNull] string dacpacPath);
    }
}