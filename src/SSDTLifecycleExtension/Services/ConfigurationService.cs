﻿namespace SSDTLifecycleExtension.Services
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Annotations;
    using DataAccess;
    using Events;
    using Microsoft.VisualStudio.Shell;
    using Newtonsoft.Json;
    using Shared.Contracts;
    using Shared.Models;
    using Task = System.Threading.Tasks.Task;

    [UsedImplicitly]
    public class ConfigurationService : IConfigurationService
    {
        private readonly IFileSystemAccess _fileSystemAccess;
        private readonly IVisualStudioAccess _visualStudioAccess;

        public ConfigurationService(IFileSystemAccess fileSystemAccess,
                                    IVisualStudioAccess visualStudioAccess)
        {
            _fileSystemAccess = fileSystemAccess;
            _visualStudioAccess = visualStudioAccess;
        }

        private static string GetConfigurationPath(SqlProject project)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var directory = Path.GetDirectoryName(project.FullName);
            return Path.Combine(directory ?? throw new InvalidOperationException("Cannot find configuration file. Directory is <null>."), "Properties", "ssdtlifecycle.json");
        }

        public event EventHandler<ProjectConfigurationChangedEventArgs> ConfigurationChanged;

        async Task<ConfigurationModel> IConfigurationService.GetConfigurationOrDefaultAsync(SqlProject project)
        {
            if (project == null)
                throw new ArgumentNullException(nameof(project));

            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var sourcePath = GetConfigurationPath(project);
            var serialized = await _fileSystemAccess.ReadFileAsync(sourcePath);
            var deserialized = serialized == null
                                   ? ConfigurationModel.GetDefault()
                                   : JsonConvert.DeserializeObject<ConfigurationModel>(serialized);
            deserialized.ValidateAll();
            return deserialized;
        }

        async Task IConfigurationService.SaveConfigurationAsync(SqlProject project,
                                                                ConfigurationModel model)
        {
            if (project == null)
                throw new ArgumentNullException(nameof(project));
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var targetPath = GetConfigurationPath(project);
            var serialized = JsonConvert.SerializeObject(model, Formatting.Indented);

            // Save configuration physically.
            await _fileSystemAccess.WriteFileAsync(targetPath, serialized);

            // Notify about changes
            ConfigurationChanged?.Invoke(this, new ProjectConfigurationChangedEventArgs(project));

            // Add configuration to the project, if it hasn't been added before.
            _visualStudioAccess.AddItemToProjectProperties(project, targetPath);
        }
    }
}