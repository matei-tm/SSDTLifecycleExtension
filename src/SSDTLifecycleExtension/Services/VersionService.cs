﻿namespace SSDTLifecycleExtension.Services
{
    using System;
    using System.Text;
    using Annotations;
    using Shared.Models;

    [UsedImplicitly]
    public class VersionService : IVersionService
    {
        public string DetermineFinalVersion(Version version,
                                            ConfigurationModel versionConfiguration)
        {
            if (version == null)
                throw new ArgumentNullException(nameof(version));
            if (versionConfiguration == null)
                throw new ArgumentNullException(nameof(versionConfiguration));

            var pattern = versionConfiguration.VersionPattern.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries);
            var final = new StringBuilder();

            for (var i = 0; i < pattern.Length; i++)
            {
                var positionValue = pattern[i];
                switch (positionValue)
                {
                    case ConfigurationModel.MajorVersionSpecialKeyword:
                        final.Append(version.Major);
                        break;
                    case ConfigurationModel.MinorVersionSpecialKeyword:
                        final.Append(version.Minor);
                        break;
                    case ConfigurationModel.BuildVersionSpecialKeyword:
                        final.Append(version.Build);
                        break;
                    case ConfigurationModel.RevisionVersionSpecialKeyword:
                        final.Append(version.Revision);
                        break;
                    default:
                        final.Append(positionValue);
                        break;
                }

                // Add . between each version position
                if (i != pattern.Length - 1)
                    final.Append(".");
            }

            return final.ToString();
        }
    }
}