﻿using NUnit.Framework;

namespace SSDTLifecycleExtension.UnitTests.Extension.ViewModels
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Moq;
    using SSDTLifecycleExtension.Shared.Contracts;
    using SSDTLifecycleExtension.Shared.Contracts.DataAccess;
    using SSDTLifecycleExtension.Shared.Contracts.Services;
    using SSDTLifecycleExtension.Shared.Events;
    using SSDTLifecycleExtension.Shared.Models;
    using SSDTLifecycleExtension.ViewModels;

    [TestFixture]
    public class ScriptCreationViewModelTests
    {
        [Test]
        public void Constructor_ArgumentNullException_Project()
        {
            // Act & Assert
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new ScriptCreationViewModel(null, null, null, null, null, null));
            // ReSharper restore AssignNullToNotNullAttribute
        }

        [Test]
        public void Constructor_ArgumentNullException_ConfigurationService()
        {
            // Arrange
            var project = new SqlProject("a", "b", "c");

            // Act & Assert
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new ScriptCreationViewModel(project, null, null, null, null, null));
            // ReSharper restore AssignNullToNotNullAttribute
        }

        [Test]
        public void Constructor_ArgumentNullException_ScaffoldingService()
        {
            // Arrange
            var project = new SqlProject("a", "b", "c");
            var csMock = Mock.Of<IConfigurationService>();

            // Act & Assert
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new ScriptCreationViewModel(project, csMock, null, null, null, null));
            // ReSharper restore AssignNullToNotNullAttribute
        }

        [Test]
        public void Constructor_ArgumentNullException_ScriptCreationService()
        {
            // Arrange
            var project = new SqlProject("a", "b", "c");
            var csMock = Mock.Of<IConfigurationService>();
            var ssMock = Mock.Of<IScaffoldingService>();

            // Act & Assert
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new ScriptCreationViewModel(project, csMock, ssMock, null, null, null));
            // ReSharper restore AssignNullToNotNullAttribute
        }

        [Test]
        public void Constructor_ArgumentNullException_ArtifactsService()
        {
            // Arrange
            var project = new SqlProject("a", "b", "c");
            var csMock = Mock.Of<IConfigurationService>();
            var ssMock = Mock.Of<IScaffoldingService>();
            var scsMock = Mock.Of<IScriptCreationService>();

            // Act & Assert
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new ScriptCreationViewModel(project, csMock, ssMock, scsMock, null, null));
            // ReSharper restore AssignNullToNotNullAttribute
        }

        [Test]
        public void Constructor_ArgumentNullException_Logger()
        {
            // Arrange
            var project = new SqlProject("a", "b", "c");
            var csMock = Mock.Of<IConfigurationService>();
            var ssMock = Mock.Of<IScaffoldingService>();
            var scsMock = Mock.Of<IScriptCreationService>();
            var asMock = Mock.Of<IArtifactsService>();

            // Act & Assert
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new ScriptCreationViewModel(project, csMock, ssMock, scsMock, asMock, null));
            // ReSharper restore AssignNullToNotNullAttribute
        }

        [Test]
        public void Constructor_CorrectInitialization()
        {
            // Arrange
            var project = new SqlProject("a", "b", "c");
            var csMock = Mock.Of<IConfigurationService>();
            var ssMock = Mock.Of<IScaffoldingService>();
            var scsMock = Mock.Of<IScriptCreationService>();
            var asMock = Mock.Of<IArtifactsService>();
            var loggerMock = Mock.Of<ILogger>();

            // Act
            var vm = new ScriptCreationViewModel(project, csMock, ssMock, scsMock, asMock, loggerMock);

            // Assert
            Assert.IsNotNull(vm.ExistingVersions);
            Assert.AreEqual(0, vm.ExistingVersions.Count);
            Assert.IsNotNull(vm.ScaffoldDevelopmentVersionCommand);
            Assert.IsNotNull(vm.ScaffoldCurrentProductionVersionCommand);
            Assert.IsNotNull(vm.StartLatestCreationCommand);
            Assert.IsNotNull(vm.StartVersionedCreationCommand);
        }

        [Test]
        public void SelectedBaseVersion_NoPropertyChangedForSameInstance()
        {
            // Arrange
            var project = new SqlProject("a", "b", "c");
            var csMock = Mock.Of<IConfigurationService>();
            var ssMock = Mock.Of<IScaffoldingService>();
            var scsMock = Mock.Of<IScriptCreationService>();
            var asMock = Mock.Of<IArtifactsService>();
            var loggerMock = Mock.Of<ILogger>();
            var version = new VersionModel();
            var vm = new ScriptCreationViewModel(project, csMock, ssMock, scsMock, asMock, loggerMock)
            {
                SelectedBaseVersion = version
            };
            object invokedSender = null;
            string invokedProperty = null;
            vm.PropertyChanged += (sender,
                                   args) =>
            {
                invokedSender = sender;
                invokedProperty = args?.PropertyName;
            };

            // Act
            vm.SelectedBaseVersion = version;

            // Assert
            Assert.IsNull(invokedSender);
            Assert.IsNull(invokedProperty);
        }

        [Test]
        public void SelectedBaseVersion_Get_Set_PropertyChanged()
        {
            // Arrange
            var project = new SqlProject("a", "b", "c");
            var csMock = Mock.Of<IConfigurationService>();
            var ssMock = Mock.Of<IScaffoldingService>();
            var scsMock = Mock.Of<IScriptCreationService>();
            var asMock = Mock.Of<IArtifactsService>();
            var loggerMock = Mock.Of<ILogger>();
            var version = new VersionModel();
            var vm = new ScriptCreationViewModel(project, csMock, ssMock, scsMock, asMock, loggerMock);
            object invokedSender = null;
            string invokedProperty = null;
            vm.PropertyChanged += (sender,
                                   args) =>
            {
                invokedSender = sender;
                invokedProperty = args?.PropertyName;
            };

            // Act
            vm.SelectedBaseVersion = version;
            var setVersion = vm.SelectedBaseVersion;

            // Assert
            Assert.AreSame(vm, invokedSender);
            Assert.AreSame(version, setVersion);
            Assert.AreEqual(nameof(ScriptCreationViewModel.SelectedBaseVersion), invokedProperty);
        }

        [Test]
        public void ArtifactCommands_CanExecute_NotWhenNotInitialized()
        {
            // Arrange
            var project = new SqlProject("a", "b", "c");
            var csMock = Mock.Of<IConfigurationService>();
            var ssMock = Mock.Of<IScaffoldingService>();
            var scsMock = Mock.Of<IScriptCreationService>();
            var asMock = Mock.Of<IArtifactsService>();
            var loggerMock = Mock.Of<ILogger>();
            var vm = new ScriptCreationViewModel(project, csMock, ssMock, scsMock, asMock, loggerMock);

            // Act
            var canExecuteList = new[]
            {
                vm.ScaffoldDevelopmentVersionCommand.CanExecute(),
                vm.ScaffoldCurrentProductionVersionCommand.CanExecute(),
                vm.StartLatestCreationCommand.CanExecute(),
                vm.StartVersionedCreationCommand.CanExecute()
            };

            // Assert
            Assert.IsTrue(canExecuteList.All(m => m == false));
        }

        [Test]
        public async Task ArtifactCommands_CanExecute_NotWhenConfigurationHasErrors_Async()
        {
            // Arrange
            var config = new ConfigurationModel
            {
                ArtifactsPath = "_Deployment",
                PublishProfilePath = "TestPath2",
                ReplaceUnnamedDefaultConstraintDrops = true,
                VersionPattern = "TestPattern",
                CommentOutUnnamedDefaultConstraintDrops = true,
                CreateDocumentationWithScriptCreation = false,
                CustomHeader = "TestHeader",
                CustomFooter = "TestFooter",
                BuildBeforeScriptCreation = true,
                TrackDacpacVersion = true,
                CommentOutReferencedProjectRefactorings = true
            };
            var project = new SqlProject("a", @"C:\TestProject\TestProject.sqlproj", "c");
            var csMock = new Mock<IConfigurationService>();
            csMock.Setup(m => m.GetConfigurationOrDefaultAsync(project))
                  .ReturnsAsync(config);
            var ssMock = Mock.Of<IScaffoldingService>();
            var scsMock = Mock.Of<IScriptCreationService>();
            var asMock = Mock.Of<IArtifactsService>();
            var loggerMock = Mock.Of<ILogger>();
            var vm = new ScriptCreationViewModel(project, csMock.Object, ssMock, scsMock, asMock, loggerMock);
            await vm.InitializeAsync();

            // Act
            var canExecuteList = new[]
            {
                vm.ScaffoldDevelopmentVersionCommand.CanExecute(),
                vm.ScaffoldCurrentProductionVersionCommand.CanExecute(),
                vm.StartLatestCreationCommand.CanExecute(),
                vm.StartVersionedCreationCommand.CanExecute()
            };

            // Assert
            Assert.IsTrue(canExecuteList.All(m => m == false));
        }

        [Test]
        public async Task ArtifactCommands_CanExecute_NotWhenScaffoldingIsInProgress_Async()
        {
            // Arrange
            var config = new ConfigurationModel
            {
                ArtifactsPath = "_Deployment",
                PublishProfilePath = "TestProfile.publish.xml",
                ReplaceUnnamedDefaultConstraintDrops = false,
                VersionPattern = "1.2.3.4",
                CommentOutUnnamedDefaultConstraintDrops = true,
                CreateDocumentationWithScriptCreation = false,
                CustomHeader = "TestHeader",
                CustomFooter = "TestFooter",
                BuildBeforeScriptCreation = true,
                TrackDacpacVersion = true,
                CommentOutReferencedProjectRefactorings = true
            };
            var project = new SqlProject("a", @"C:\TestProject\TestProject.sqlproj", "c");
            var csMock = new Mock<IConfigurationService>();
            csMock.Setup(m => m.GetConfigurationOrDefaultAsync(project))
                  .ReturnsAsync(config);
            var ssMock = new Mock<IScaffoldingService>();
            ssMock.SetupGet(m => m.IsScaffolding)
                  .Returns(true);
            var scsMock = Mock.Of<IScriptCreationService>();
            var asMock = Mock.Of<IArtifactsService>();
            var loggerMock = Mock.Of<ILogger>();
            var vm = new ScriptCreationViewModel(project, csMock.Object, ssMock.Object, scsMock, asMock, loggerMock);
            await vm.InitializeAsync();

            // Act
            var canExecuteList = new[]
            {
                vm.ScaffoldDevelopmentVersionCommand.CanExecute(),
                vm.ScaffoldCurrentProductionVersionCommand.CanExecute(),
                vm.StartLatestCreationCommand.CanExecute(),
                vm.StartVersionedCreationCommand.CanExecute()
            };

            // Assert
            Assert.IsTrue(canExecuteList.All(m => m == false));
        }

        [Test]
        public async Task ArtifactCommands_CanExecute_AfterScaffoldingCompleted_Async()
        {
            // Arrange
            var config = new ConfigurationModel
            {
                ArtifactsPath = "_Deployment",
                PublishProfilePath = "TestProfile.publish.xml",
                ReplaceUnnamedDefaultConstraintDrops = false,
                VersionPattern = "1.2.3.4",
                CommentOutUnnamedDefaultConstraintDrops = true,
                CreateDocumentationWithScriptCreation = false,
                CustomHeader = "TestHeader",
                CustomFooter = "TestFooter",
                BuildBeforeScriptCreation = true,
                TrackDacpacVersion = true,
                CommentOutReferencedProjectRefactorings = true
            };
            var project = new SqlProject("a", @"C:\TestProject\TestProject.sqlproj", "c");
            var csMock = new Mock<IConfigurationService>();
            csMock.Setup(m => m.GetConfigurationOrDefaultAsync(project))
                  .ReturnsAsync(config);
            var ssMock = new Mock<IScaffoldingService>();
            ssMock.SetupGet(m => m.IsScaffolding)
                  .Returns(true);
            var scsMock = Mock.Of<IScriptCreationService>();
            var asMock = Mock.Of<IArtifactsService>();
            var loggerMock = Mock.Of<ILogger>();
            var vm = new ScriptCreationViewModel(project, csMock.Object, ssMock.Object, scsMock, asMock, loggerMock);
            await vm.InitializeAsync();
            var scaffoldDevelopmentVersionCommandCanExecuteChanged = false;
            vm.ScaffoldDevelopmentVersionCommand.CanExecuteChanged += (sender,
                                                                       args) => scaffoldDevelopmentVersionCommandCanExecuteChanged = true;
            var scaffoldCurrentProductionVersionCommandCanExecuteChanged = false;
            vm.ScaffoldCurrentProductionVersionCommand.CanExecuteChanged += (sender,
                                                                             args) => scaffoldCurrentProductionVersionCommandCanExecuteChanged = true;
            var startLatestCreationCommandCanExecuteChanged = false;
            vm.StartLatestCreationCommand.CanExecuteChanged += (sender,
                                                                args) => startLatestCreationCommandCanExecuteChanged = true;
            var startVersionedCreationCommandCanExecuteChanged = false;
            vm.StartVersionedCreationCommand.CanExecuteChanged += (sender,
                                                                   args) => startVersionedCreationCommandCanExecuteChanged = true;

            // Act
            var canExecuteList = new[]
            {
                vm.ScaffoldDevelopmentVersionCommand.CanExecute(),
                vm.ScaffoldCurrentProductionVersionCommand.CanExecute(),
                vm.StartLatestCreationCommand.CanExecute(),
                vm.StartVersionedCreationCommand.CanExecute()
            };
            ssMock.SetupGet(m => m.IsScaffolding)
                  .Returns(false);
            ssMock.Raise(service => service.IsScaffoldingChanged += null, EventArgs.Empty);
            var canExecuteListAfterCompletion = new[]
            {
                vm.ScaffoldDevelopmentVersionCommand.CanExecute(),
                vm.ScaffoldCurrentProductionVersionCommand.CanExecute(),
                vm.StartLatestCreationCommand.CanExecute(),
                vm.StartVersionedCreationCommand.CanExecute()
            };

            // Assert
            Assert.IsTrue(canExecuteList.All(m => m == false));
            Assert.IsTrue(canExecuteListAfterCompletion.All(m => m));
            Assert.IsTrue(scaffoldDevelopmentVersionCommandCanExecuteChanged);
            Assert.IsTrue(scaffoldCurrentProductionVersionCommandCanExecuteChanged);
            Assert.IsTrue(startLatestCreationCommandCanExecuteChanged);
            Assert.IsTrue(startVersionedCreationCommandCanExecuteChanged);
        }

        [Test]
        public async Task ArtifactCommands_CanExecute_NotWhenScriptCreationIsInProgress_Async()
        {
            // Arrange
            var config = new ConfigurationModel
            {
                ArtifactsPath = "_Deployment",
                PublishProfilePath = "TestProfile.publish.xml",
                ReplaceUnnamedDefaultConstraintDrops = false,
                VersionPattern = "1.2.3.4",
                CommentOutUnnamedDefaultConstraintDrops = true,
                CreateDocumentationWithScriptCreation = false,
                CustomHeader = "TestHeader",
                CustomFooter = "TestFooter",
                BuildBeforeScriptCreation = true,
                TrackDacpacVersion = true,
                CommentOutReferencedProjectRefactorings = true
            };
            var project = new SqlProject("a", @"C:\TestProject\TestProject.sqlproj", "c");
            var csMock = new Mock<IConfigurationService>();
            csMock.Setup(m => m.GetConfigurationOrDefaultAsync(project))
                  .ReturnsAsync(config);
            var ssMock = Mock.Of<IScaffoldingService>();
            var scsMock = new Mock<IScriptCreationService>();
            scsMock.SetupGet(m => m.IsCreating)
                   .Returns(true);
            var asMock = Mock.Of<IArtifactsService>();
            var loggerMock = Mock.Of<ILogger>();
            var vm = new ScriptCreationViewModel(project, csMock.Object, ssMock, scsMock.Object, asMock, loggerMock);
            await vm.InitializeAsync();

            // Act
            var canExecuteList = new[]
            {
                vm.ScaffoldDevelopmentVersionCommand.CanExecute(),
                vm.ScaffoldCurrentProductionVersionCommand.CanExecute(),
                vm.StartLatestCreationCommand.CanExecute(),
                vm.StartVersionedCreationCommand.CanExecute()
            };

            // Assert
            Assert.IsTrue(canExecuteList.All(m => m == false));
        }

        [Test]
        public async Task ArtifactCommands_CanExecute_AfterScriptCreationCompleted_Async()
        {
            // Arrange
            var config = new ConfigurationModel
            {
                ArtifactsPath = "_Deployment",
                PublishProfilePath = "TestProfile.publish.xml",
                ReplaceUnnamedDefaultConstraintDrops = false,
                VersionPattern = "1.2.3.4",
                CommentOutUnnamedDefaultConstraintDrops = true,
                CreateDocumentationWithScriptCreation = false,
                CustomHeader = "TestHeader",
                CustomFooter = "TestFooter",
                BuildBeforeScriptCreation = true,
                TrackDacpacVersion = true,
                CommentOutReferencedProjectRefactorings = true
            };
            var project = new SqlProject("a", @"C:\TestProject\TestProject.sqlproj", "c");
            var csMock = new Mock<IConfigurationService>();
            csMock.Setup(m => m.GetConfigurationOrDefaultAsync(project))
                  .ReturnsAsync(config);
            var ssMock = Mock.Of<IScaffoldingService>();
            var scsMock = new Mock<IScriptCreationService>();
            scsMock.SetupGet(m => m.IsCreating)
                   .Returns(true);
            var asMock = Mock.Of<IArtifactsService>();
            var loggerMock = Mock.Of<ILogger>();
            var vm = new ScriptCreationViewModel(project, csMock.Object, ssMock, scsMock.Object, asMock, loggerMock);
            await vm.InitializeAsync();
            var scaffoldDevelopmentVersionCommandCanExecuteChanged = false;
            vm.ScaffoldDevelopmentVersionCommand.CanExecuteChanged += (sender,
                                                                       args) => scaffoldDevelopmentVersionCommandCanExecuteChanged = true;
            var scaffoldCurrentProductionVersionCommandCanExecuteChanged = false;
            vm.ScaffoldCurrentProductionVersionCommand.CanExecuteChanged += (sender,
                                                                             args) => scaffoldCurrentProductionVersionCommandCanExecuteChanged = true;
            var startLatestCreationCommandCanExecuteChanged = false;
            vm.StartLatestCreationCommand.CanExecuteChanged += (sender,
                                                                args) => startLatestCreationCommandCanExecuteChanged = true;
            var startVersionedCreationCommandCanExecuteChanged = false;
            vm.StartVersionedCreationCommand.CanExecuteChanged += (sender,
                                                                   args) => startVersionedCreationCommandCanExecuteChanged = true;

            // Act
            var canExecuteList = new[]
            {
                vm.ScaffoldDevelopmentVersionCommand.CanExecute(),
                vm.ScaffoldCurrentProductionVersionCommand.CanExecute(),
                vm.StartLatestCreationCommand.CanExecute(),
                vm.StartVersionedCreationCommand.CanExecute()
            };
            scsMock.SetupGet(m => m.IsCreating)
                   .Returns(false);
            scsMock.Raise(service => service.IsCreatingChanged += null, EventArgs.Empty);
            var canExecuteListAfterCompletion = new[]
            {
                vm.ScaffoldDevelopmentVersionCommand.CanExecute(),
                vm.ScaffoldCurrentProductionVersionCommand.CanExecute(),
                vm.StartLatestCreationCommand.CanExecute(),
                vm.StartVersionedCreationCommand.CanExecute()
            };

            // Assert
            Assert.IsTrue(canExecuteList.All(m => m == false));
            Assert.IsTrue(canExecuteListAfterCompletion.All(m => m));
            Assert.IsTrue(scaffoldDevelopmentVersionCommandCanExecuteChanged);
            Assert.IsTrue(scaffoldCurrentProductionVersionCommandCanExecuteChanged);
            Assert.IsTrue(startLatestCreationCommandCanExecuteChanged);
            Assert.IsTrue(startVersionedCreationCommandCanExecuteChanged);
        }

        [Test]
        public async Task InitializeAsync_NoExistingVersions_Async()
        {
            var config = new ConfigurationModel
            {
                ArtifactsPath = "_Deployment",
                PublishProfilePath = "TestProfile.publish.xml",
                ReplaceUnnamedDefaultConstraintDrops = false,
                VersionPattern = "1.2.3.4",
                CommentOutUnnamedDefaultConstraintDrops = true,
                CreateDocumentationWithScriptCreation = false,
                CustomHeader = "TestHeader",
                CustomFooter = "TestFooter",
                BuildBeforeScriptCreation = true,
                TrackDacpacVersion = true,
                CommentOutReferencedProjectRefactorings = true
            };
            var project = new SqlProject("a", @"C:\TestProject\TestProject.sqlproj", "c");
            var csMock = new Mock<IConfigurationService>();
            csMock.Setup(m => m.GetConfigurationOrDefaultAsync(project))
                  .ReturnsAsync(config);
            var ssMock = Mock.Of<IScaffoldingService>();
            var scsMock = new Mock<IScriptCreationService>();
            scsMock.SetupGet(m => m.IsCreating)
                   .Returns(true);
            var asMock = new Mock<IArtifactsService>();
            asMock.Setup(m => m.GetExistingArtifactVersions(project, config))
                  .Returns(new VersionModel[0]);
            var loggerMock = Mock.Of<ILogger>();
            var vm = new ScriptCreationViewModel(project, csMock.Object, ssMock, scsMock.Object, asMock.Object, loggerMock);

            // Act
            var initialized = await vm.InitializeAsync();

            // Assert
            Assert.IsTrue(initialized);
            Assert.AreEqual(0, vm.ExistingVersions.Count);
            Assert.IsNull(vm.SelectedBaseVersion);
        }

        [Test]
        public async Task InitializeAsync_ValidDirectories_SelectHighest_Async()
        {
            var config = new ConfigurationModel
            {
                ArtifactsPath = "_Deployment",
                PublishProfilePath = "TestProfile.publish.xml",
                ReplaceUnnamedDefaultConstraintDrops = false,
                VersionPattern = "1.2.3.4",
                CommentOutUnnamedDefaultConstraintDrops = true,
                CreateDocumentationWithScriptCreation = false,
                CustomHeader = "TestHeader",
                CustomFooter = "TestFooter",
                BuildBeforeScriptCreation = true,
                TrackDacpacVersion = true,
                CommentOutReferencedProjectRefactorings = true
            };
            var project = new SqlProject("a", @"C:\TestProject\TestProject.sqlproj", "c");
            var csMock = new Mock<IConfigurationService>();
            csMock.Setup(m => m.GetConfigurationOrDefaultAsync(project))
                  .ReturnsAsync(config);
            var ssMock = Mock.Of<IScaffoldingService>();
            var scsMock = new Mock<IScriptCreationService>();
            scsMock.SetupGet(m => m.IsCreating)
                   .Returns(true);
            var asMock = new Mock<IArtifactsService>();
            asMock.Setup(m => m.GetExistingArtifactVersions(project, config))
                  .Returns(new[]
                   {
                      new VersionModel
                      {
                          IsNewestVersion = true,
                          UnderlyingVersion = new Version(5, 0)
                      },
                      new VersionModel
                      {
                          UnderlyingVersion = new Version(4, 0, 0)
                      }
                   });
            var loggerMock = Mock.Of<ILogger>();
            var vm = new ScriptCreationViewModel(project, csMock.Object, ssMock, scsMock.Object, asMock.Object, loggerMock);

            // Act
            var initialized = await vm.InitializeAsync();

            // Assert
            Assert.IsTrue(initialized);
            Assert.AreEqual(2, vm.ExistingVersions.Count);
            Assert.IsNotNull(vm.SelectedBaseVersion);
            Assert.AreSame(vm.SelectedBaseVersion, vm.ExistingVersions[0]);
            Assert.IsTrue(vm.ExistingVersions[0].IsNewestVersion);
            Assert.AreEqual(new Version(5, 0), vm.ExistingVersions[0].UnderlyingVersion);
            Assert.IsFalse(vm.ExistingVersions[1].IsNewestVersion);
            Assert.AreEqual(new Version(4, 0, 0), vm.ExistingVersions[1].UnderlyingVersion);
        }

        [Test]
        public async Task ConfigurationService_ConfigurationChanged_SameProject_Async()
        {
            var config = new ConfigurationModel
            {
                ArtifactsPath = "_Deployment",
                PublishProfilePath = "TestProfile.publish.xml",
                ReplaceUnnamedDefaultConstraintDrops = false,
                VersionPattern = "1.2.3.4",
                CommentOutUnnamedDefaultConstraintDrops = true,
                CreateDocumentationWithScriptCreation = false,
                CustomHeader = "TestHeader",
                CustomFooter = "TestFooter",
                BuildBeforeScriptCreation = true,
                TrackDacpacVersion = true,
                CommentOutReferencedProjectRefactorings = true
            };
            var project = new SqlProject("a", @"C:\TestProject\TestProject.sqlproj", "c");
            var csMock = new Mock<IConfigurationService>();
            csMock.Setup(m => m.GetConfigurationOrDefaultAsync(project))
                  .ReturnsAsync(config);
            var ssMock = Mock.Of<IScaffoldingService>();
            var scsMock = new Mock<IScriptCreationService>();
            scsMock.SetupGet(m => m.IsCreating)
                   .Returns(true);
            var asMock = new Mock<IArtifactsService>();
            asMock.Setup(m => m.GetExistingArtifactVersions(project, config))
                  .Returns(new VersionModel[0]);
            var loggerMock = Mock.Of<ILogger>();
            var vm = new ScriptCreationViewModel(project, csMock.Object, ssMock, scsMock.Object, asMock.Object, loggerMock);
            await vm.InitializeAsync();
            var scaffoldDevelopmentVersionCommandCanExecuteChanged = false;
            vm.ScaffoldDevelopmentVersionCommand.CanExecuteChanged += (sender,
                                                                       args) => scaffoldDevelopmentVersionCommandCanExecuteChanged = true;
            var scaffoldCurrentProductionVersionCommandCanExecuteChanged = false;
            vm.ScaffoldCurrentProductionVersionCommand.CanExecuteChanged += (sender,
                                                                             args) => scaffoldCurrentProductionVersionCommandCanExecuteChanged = true;
            var startLatestCreationCommandCanExecuteChanged = false;
            vm.StartLatestCreationCommand.CanExecuteChanged += (sender,
                                                                args) => startLatestCreationCommandCanExecuteChanged = true;
            var startVersionedCreationCommandCanExecuteChanged = false;
            vm.StartVersionedCreationCommand.CanExecuteChanged += (sender,
                                                                   args) => startVersionedCreationCommandCanExecuteChanged = true;

            // Act
            csMock.Raise(m => m.ConfigurationChanged += null, new ProjectConfigurationChangedEventArgs(project));

            // Assert
            csMock.Verify(m => m.GetConfigurationOrDefaultAsync(project), Times.Exactly(2));
            Assert.IsTrue(scaffoldDevelopmentVersionCommandCanExecuteChanged);
            Assert.IsTrue(scaffoldCurrentProductionVersionCommandCanExecuteChanged);
            Assert.IsTrue(startLatestCreationCommandCanExecuteChanged);
            Assert.IsTrue(startVersionedCreationCommandCanExecuteChanged);
        }

        [Test]
        public async Task ConfigurationService_ConfigurationChanged_DifferentProject_Async()
        {
            var config = new ConfigurationModel
            {
                ArtifactsPath = "_Deployment",
                PublishProfilePath = "TestProfile.publish.xml",
                ReplaceUnnamedDefaultConstraintDrops = false,
                VersionPattern = "1.2.3.4",
                CommentOutUnnamedDefaultConstraintDrops = true,
                CreateDocumentationWithScriptCreation = false,
                CustomHeader = "TestHeader",
                CustomFooter = "TestFooter",
                BuildBeforeScriptCreation = true,
                TrackDacpacVersion = true,
                CommentOutReferencedProjectRefactorings = true
            };
            var project = new SqlProject("a", @"C:\TestProject\TestProject.sqlproj", "c");
            var differentProject = new SqlProject("a", @"C:\TestProject\TestProject.sqlproj", "d");
            var csMock = new Mock<IConfigurationService>();
            csMock.Setup(m => m.GetConfigurationOrDefaultAsync(project))
                  .ReturnsAsync(config);
            var ssMock = Mock.Of<IScaffoldingService>();
            var scsMock = new Mock<IScriptCreationService>();
            scsMock.SetupGet(m => m.IsCreating)
                   .Returns(true);
            var asMock = new Mock<IArtifactsService>();
            asMock.Setup(m => m.GetExistingArtifactVersions(project, config))
                  .Returns(new VersionModel[0]);
            var loggerMock = Mock.Of<ILogger>();
            var vm = new ScriptCreationViewModel(project, csMock.Object, ssMock, scsMock.Object, asMock.Object, loggerMock);
            await vm.InitializeAsync();
            var scaffoldDevelopmentVersionCommandCanExecuteChanged = false;
            vm.ScaffoldDevelopmentVersionCommand.CanExecuteChanged += (sender,
                                                                       args) => scaffoldDevelopmentVersionCommandCanExecuteChanged = true;
            var scaffoldCurrentProductionVersionCommandCanExecuteChanged = false;
            vm.ScaffoldCurrentProductionVersionCommand.CanExecuteChanged += (sender,
                                                                             args) => scaffoldCurrentProductionVersionCommandCanExecuteChanged = true;
            var startLatestCreationCommandCanExecuteChanged = false;
            vm.StartLatestCreationCommand.CanExecuteChanged += (sender,
                                                                args) => startLatestCreationCommandCanExecuteChanged = true;
            var startVersionedCreationCommandCanExecuteChanged = false;
            vm.StartVersionedCreationCommand.CanExecuteChanged += (sender,
                                                                   args) => startVersionedCreationCommandCanExecuteChanged = true;

            // Act
            csMock.Raise(m => m.ConfigurationChanged += null, new ProjectConfigurationChangedEventArgs(differentProject));

            // Assert
            csMock.Verify(m => m.GetConfigurationOrDefaultAsync(project), Times.Exactly(1));
            Assert.IsFalse(scaffoldDevelopmentVersionCommandCanExecuteChanged);
            Assert.IsFalse(scaffoldCurrentProductionVersionCommandCanExecuteChanged);
            Assert.IsFalse(startLatestCreationCommandCanExecuteChanged);
            Assert.IsFalse(startVersionedCreationCommandCanExecuteChanged);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task ScaffoldDevelopmentVersionCommand_ExecutedAsync_Async(bool success)
        {
            var config = new ConfigurationModel
            {
                ArtifactsPath = "_Deployment",
                PublishProfilePath = "TestProfile.publish.xml",
                ReplaceUnnamedDefaultConstraintDrops = false,
                VersionPattern = "1.2.3.4",
                CommentOutUnnamedDefaultConstraintDrops = true,
                CreateDocumentationWithScriptCreation = false,
                CustomHeader = "TestHeader",
                CustomFooter = "TestFooter",
                BuildBeforeScriptCreation = true,
                TrackDacpacVersion = true,
                CommentOutReferencedProjectRefactorings = true
            };
            var project = new SqlProject("a", @"C:\TestProject\TestProject.sqlproj", "c");
            var csMock = new Mock<IConfigurationService>();
            csMock.Setup(m => m.GetConfigurationOrDefaultAsync(project))
                  .ReturnsAsync(config);
            var ssMock = new Mock<IScaffoldingService>();
            ssMock.Setup(m => m.ScaffoldAsync(project, config, new Version(0, 0, 0, 0), CancellationToken.None))
                  .ReturnsAsync(success);
            var scsMock = new Mock<IScriptCreationService>();
            var asMock = new Mock<IArtifactsService>();
            asMock.Setup(m => m.GetExistingArtifactVersions(project, config))
                  .Returns(new []
                   {
                       new VersionModel
                       {
                           IsNewestVersion = true,
                           UnderlyingVersion = new Version(4, 0)
                       }
                   });
            var loggerMock = Mock.Of<ILogger>();
            var vm = new ScriptCreationViewModel(project, csMock.Object, ssMock.Object, scsMock.Object, asMock.Object, loggerMock);
            await vm.InitializeAsync();
            var scaffoldDevelopmentVersionCommandCanExecuteChanged = false;
            vm.ScaffoldDevelopmentVersionCommand.CanExecuteChanged += (sender,
                                                                       args) => scaffoldDevelopmentVersionCommandCanExecuteChanged = true;
            var scaffoldCurrentProductionVersionCommandCanExecuteChanged = false;
            vm.ScaffoldCurrentProductionVersionCommand.CanExecuteChanged += (sender,
                                                                             args) => scaffoldCurrentProductionVersionCommandCanExecuteChanged = true;
            var startLatestCreationCommandCanExecuteChanged = false;
            vm.StartLatestCreationCommand.CanExecuteChanged += (sender,
                                                                args) => startLatestCreationCommandCanExecuteChanged = true;
            var startVersionedCreationCommandCanExecuteChanged = false;
            vm.StartVersionedCreationCommand.CanExecuteChanged += (sender,
                                                                   args) => startVersionedCreationCommandCanExecuteChanged = true;

            // Act
            await vm.ScaffoldDevelopmentVersionCommand.ExecuteAsync();

            // Assert
            var expectedConfigurationLoads = success ? 2 : 1;
            csMock.Verify(m => m.GetConfigurationOrDefaultAsync(project), Times.Exactly(expectedConfigurationLoads));
            ssMock.Verify(m => m.ScaffoldAsync(project, config, new Version(0, 0, 0, 0), CancellationToken.None), Times.Once);
            Assert.IsTrue(scaffoldDevelopmentVersionCommandCanExecuteChanged);
            Assert.IsTrue(scaffoldCurrentProductionVersionCommandCanExecuteChanged);
            Assert.IsTrue(startLatestCreationCommandCanExecuteChanged);
            Assert.IsTrue(startVersionedCreationCommandCanExecuteChanged);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task ScaffoldCurrentProductionVersionCommand_ExecutedAsync_Async(bool success)
        {
            var config = new ConfigurationModel
            {
                ArtifactsPath = "_Deployment",
                PublishProfilePath = "TestProfile.publish.xml",
                ReplaceUnnamedDefaultConstraintDrops = false,
                VersionPattern = "1.2.3.4",
                CommentOutUnnamedDefaultConstraintDrops = true,
                CreateDocumentationWithScriptCreation = false,
                CustomHeader = "TestHeader",
                CustomFooter = "TestFooter",
                BuildBeforeScriptCreation = true,
                TrackDacpacVersion = true,
                CommentOutReferencedProjectRefactorings = true
            };
            var project = new SqlProject("a", @"C:\TestProject\TestProject.sqlproj", "c");
            var csMock = new Mock<IConfigurationService>();
            csMock.Setup(m => m.GetConfigurationOrDefaultAsync(project))
                  .ReturnsAsync(config);
            var ssMock = new Mock<IScaffoldingService>();
            ssMock.Setup(m => m.ScaffoldAsync(project, config, new Version(1, 0, 0, 0), CancellationToken.None))
                  .ReturnsAsync(success);
            var scsMock = new Mock<IScriptCreationService>();
            var asMock = new Mock<IArtifactsService>();
            asMock.Setup(m => m.GetExistingArtifactVersions(project, config))
                  .Returns(new[]
                   {
                       new VersionModel
                       {
                           IsNewestVersion = true,
                           UnderlyingVersion = new Version(4, 0)
                       }
                   });
            var loggerMock = Mock.Of<ILogger>();
            var vm = new ScriptCreationViewModel(project, csMock.Object, ssMock.Object, scsMock.Object, asMock.Object, loggerMock);
            await vm.InitializeAsync();
            var scaffoldDevelopmentVersionCommandCanExecuteChanged = false;
            vm.ScaffoldDevelopmentVersionCommand.CanExecuteChanged += (sender,
                                                                       args) => scaffoldDevelopmentVersionCommandCanExecuteChanged = true;
            var scaffoldCurrentProductionVersionCommandCanExecuteChanged = false;
            vm.ScaffoldCurrentProductionVersionCommand.CanExecuteChanged += (sender,
                                                                             args) => scaffoldCurrentProductionVersionCommandCanExecuteChanged = true;
            var startLatestCreationCommandCanExecuteChanged = false;
            vm.StartLatestCreationCommand.CanExecuteChanged += (sender,
                                                                args) => startLatestCreationCommandCanExecuteChanged = true;
            var startVersionedCreationCommandCanExecuteChanged = false;
            vm.StartVersionedCreationCommand.CanExecuteChanged += (sender,
                                                                   args) => startVersionedCreationCommandCanExecuteChanged = true;

            // Act
            await vm.ScaffoldCurrentProductionVersionCommand.ExecuteAsync();

            // Assert
            var expectedConfigurationLoads = success ? 2 : 1;
            csMock.Verify(m => m.GetConfigurationOrDefaultAsync(project), Times.Exactly(expectedConfigurationLoads));
            ssMock.Verify(m => m.ScaffoldAsync(project, config, new Version(1, 0, 0, 0), CancellationToken.None), Times.Once);
            Assert.IsTrue(scaffoldDevelopmentVersionCommandCanExecuteChanged);
            Assert.IsTrue(scaffoldCurrentProductionVersionCommandCanExecuteChanged);
            Assert.IsTrue(startLatestCreationCommandCanExecuteChanged);
            Assert.IsTrue(startVersionedCreationCommandCanExecuteChanged);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task StartLatestCreationCommand_ExecutedAsync_Async(bool success)
        {
            var config = new ConfigurationModel
            {
                ArtifactsPath = "_Deployment",
                PublishProfilePath = "TestProfile.publish.xml",
                ReplaceUnnamedDefaultConstraintDrops = false,
                VersionPattern = "1.2.3.4",
                CommentOutUnnamedDefaultConstraintDrops = true,
                CreateDocumentationWithScriptCreation = false,
                CustomHeader = "TestHeader",
                CustomFooter = "TestFooter",
                BuildBeforeScriptCreation = true,
                TrackDacpacVersion = true,
                CommentOutReferencedProjectRefactorings = true
            };
            var project = new SqlProject("a", @"C:\TestProject\TestProject.sqlproj", "c");
            var csMock = new Mock<IConfigurationService>();
            csMock.Setup(m => m.GetConfigurationOrDefaultAsync(project))
                  .ReturnsAsync(config);
            var ssMock = new Mock<IScaffoldingService>();
            var scsMock = new Mock<IScriptCreationService>();
            var asMock = new Mock<IArtifactsService>();
            asMock.Setup(m => m.GetExistingArtifactVersions(project, config))
                  .Returns(new[]
                   {
                       new VersionModel
                       {
                           IsNewestVersion = true,
                           UnderlyingVersion = new Version(4, 0)
                       }
                   });
            var loggerMock = Mock.Of<ILogger>();
            var vm = new ScriptCreationViewModel(project, csMock.Object, ssMock.Object, scsMock.Object, asMock.Object, loggerMock);
            bool? isCreatingScriptDuringCall = null;
            scsMock.Setup(m => m.CreateAsync(project, config, new Version(4, 0), true, CancellationToken.None))
                   .Callback(() => isCreatingScriptDuringCall = true)
                   .ReturnsAsync(success);
            var invokedIsCreatingScriptCount = 0;
            vm.PropertyChanged += (sender,
                                   args) =>
            {
                if (ReferenceEquals(sender, vm) && args?.PropertyName == nameof(ScriptCreationViewModel.IsCreatingScript))
                    invokedIsCreatingScriptCount++;
            };
            await vm.InitializeAsync();
            var scaffoldDevelopmentVersionCommandCanExecuteChanged = false;
            vm.ScaffoldDevelopmentVersionCommand.CanExecuteChanged += (sender,
                                                                       args) => scaffoldDevelopmentVersionCommandCanExecuteChanged = true;
            var scaffoldCurrentProductionVersionCommandCanExecuteChanged = false;
            vm.ScaffoldCurrentProductionVersionCommand.CanExecuteChanged += (sender,
                                                                             args) => scaffoldCurrentProductionVersionCommandCanExecuteChanged = true;
            var startLatestCreationCommandCanExecuteChanged = false;
            vm.StartLatestCreationCommand.CanExecuteChanged += (sender,
                                                                args) => startLatestCreationCommandCanExecuteChanged = true;
            var startVersionedCreationCommandCanExecuteChanged = false;
            vm.StartVersionedCreationCommand.CanExecuteChanged += (sender,
                                                                   args) => startVersionedCreationCommandCanExecuteChanged = true;

            // Act
            await vm.StartLatestCreationCommand.ExecuteAsync();
            var isCreatingScriptAfterCall = vm.IsCreatingScript;

            // Assert
            var expectedConfigurationLoads = success ? 2 : 1;
            csMock.Verify(m => m.GetConfigurationOrDefaultAsync(project), Times.Exactly(expectedConfigurationLoads));
            scsMock.Verify(m => m.CreateAsync(project, config, new Version(4, 0), true, CancellationToken.None), Times.Once);
            Assert.IsTrue(scaffoldDevelopmentVersionCommandCanExecuteChanged);
            Assert.IsTrue(scaffoldCurrentProductionVersionCommandCanExecuteChanged);
            Assert.IsTrue(startLatestCreationCommandCanExecuteChanged);
            Assert.IsTrue(startVersionedCreationCommandCanExecuteChanged);
            Assert.IsTrue(isCreatingScriptDuringCall);
            Assert.IsFalse(isCreatingScriptAfterCall);
            Assert.AreEqual(2, invokedIsCreatingScriptCount);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task StartVersionedCreationCommand_ExecutedAsync_Async(bool success)
        {
            var config = new ConfigurationModel
            {
                ArtifactsPath = "_Deployment",
                PublishProfilePath = "TestProfile.publish.xml",
                ReplaceUnnamedDefaultConstraintDrops = false,
                VersionPattern = "1.2.3.4",
                CommentOutUnnamedDefaultConstraintDrops = true,
                CreateDocumentationWithScriptCreation = false,
                CustomHeader = "TestHeader",
                CustomFooter = "TestFooter",
                BuildBeforeScriptCreation = true,
                TrackDacpacVersion = true,
                CommentOutReferencedProjectRefactorings = true
            };
            var project = new SqlProject("a", @"C:\TestProject\TestProject.sqlproj", "c");
            var csMock = new Mock<IConfigurationService>();
            csMock.Setup(m => m.GetConfigurationOrDefaultAsync(project))
                  .ReturnsAsync(config);
            var ssMock = new Mock<IScaffoldingService>();
            var scsMock = new Mock<IScriptCreationService>();
            var asMock = new Mock<IArtifactsService>();
            asMock.Setup(m => m.GetExistingArtifactVersions(project, config))
                  .Returns(new[]
                   {
                       new VersionModel
                       {
                           IsNewestVersion = true,
                           UnderlyingVersion = new Version(4, 0)
                       }
                   });
            var loggerMock = Mock.Of<ILogger>();
            var vm = new ScriptCreationViewModel(project, csMock.Object, ssMock.Object, scsMock.Object, asMock.Object, loggerMock);
            bool? isCreatingScriptDuringCall = null;
            scsMock.Setup(m => m.CreateAsync(project, config, new Version(4, 0), false, CancellationToken.None))
                   .Callback(() => isCreatingScriptDuringCall = true)
                   .ReturnsAsync(success);
            var invokedIsCreatingScriptCount = 0;
            vm.PropertyChanged += (sender,
                                   args) =>
            {
                if (ReferenceEquals(sender, vm) && args?.PropertyName == nameof(ScriptCreationViewModel.IsCreatingScript))
                    invokedIsCreatingScriptCount++;
            };
            await vm.InitializeAsync();
            var scaffoldDevelopmentVersionCommandCanExecuteChanged = false;
            vm.ScaffoldDevelopmentVersionCommand.CanExecuteChanged += (sender,
                                                                       args) => scaffoldDevelopmentVersionCommandCanExecuteChanged = true;
            var scaffoldCurrentProductionVersionCommandCanExecuteChanged = false;
            vm.ScaffoldCurrentProductionVersionCommand.CanExecuteChanged += (sender,
                                                                             args) => scaffoldCurrentProductionVersionCommandCanExecuteChanged = true;
            var startLatestCreationCommandCanExecuteChanged = false;
            vm.StartLatestCreationCommand.CanExecuteChanged += (sender,
                                                                args) => startLatestCreationCommandCanExecuteChanged = true;
            var startVersionedCreationCommandCanExecuteChanged = false;
            vm.StartVersionedCreationCommand.CanExecuteChanged += (sender,
                                                                   args) => startVersionedCreationCommandCanExecuteChanged = true;

            // Act
            await vm.StartVersionedCreationCommand.ExecuteAsync();
            var isCreatingScriptAfterCall = vm.IsCreatingScript;

            // Assert
            var expectedConfigurationLoads = success ? 2 : 1;
            csMock.Verify(m => m.GetConfigurationOrDefaultAsync(project), Times.Exactly(expectedConfigurationLoads));
            scsMock.Verify(m => m.CreateAsync(project, config, new Version(4, 0), false, CancellationToken.None), Times.Once);
            Assert.IsTrue(scaffoldDevelopmentVersionCommandCanExecuteChanged);
            Assert.IsTrue(scaffoldCurrentProductionVersionCommandCanExecuteChanged);
            Assert.IsTrue(startLatestCreationCommandCanExecuteChanged);
            Assert.IsTrue(startVersionedCreationCommandCanExecuteChanged);
            Assert.IsTrue(isCreatingScriptDuringCall);
            Assert.IsFalse(isCreatingScriptAfterCall);
            Assert.AreEqual(2, invokedIsCreatingScriptCount);
        }
    }
}