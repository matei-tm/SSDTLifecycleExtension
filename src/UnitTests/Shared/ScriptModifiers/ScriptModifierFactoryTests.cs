﻿using NUnit.Framework;

namespace SSDTLifecycleExtension.UnitTests.Shared.ScriptModifiers
{
    using System;
    using SSDTLifecycleExtension.Shared.ScriptModifiers;

    [TestFixture]
    public class ScriptModifierFactoryTests
    {
        [Test]
        [TestCase(ScriptModifier.Undefined)]
        public void CreateScriptModifier_ArgumentOutOfRangeException(ScriptModifier scriptModifier)
        {
            // Arrange
            IScriptModifierFactory f = new ScriptModifierFactory();

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => f.CreateScriptModifier(scriptModifier));
        }

        [Test]
        [TestCase(ScriptModifier.AddCustomHeader, typeof(AddCustomHeaderModifier))]
        [TestCase(ScriptModifier.AddCustomFooter, typeof(AddCustomFooterModifier))]
        [TestCase(ScriptModifier.TrackDacpacVersion, typeof(TrackDacpacVersionModifier))]
        public void CreateScriptModifier_CorrectCreation(ScriptModifier scriptModifier,
                                                         Type implementingType)
        {
            // Arrange
            IScriptModifierFactory f = new ScriptModifierFactory();

            // Act
            var modifier = f.CreateScriptModifier(scriptModifier);

            // Assert
            Assert.IsNotNull(modifier);
            Assert.IsInstanceOf(implementingType, modifier);
        }
    }
}