﻿// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.ConfigFiles;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.Configuration
{
    /// <summary>
    /// Tests around merging policy injection configuration sections
    /// from multiple config files.
    /// </summary>
    [TestClass]
    public class PolicyInjectionSettingsMergeFixture
    {
        [TestInitialize]
        public void Setup()
        {
            var fileCreator = new ResourceHelper<ConfigFileLocator>();
            fileCreator.DumpResourceFileToDisk("main.config");
            fileCreator.DumpResourceFileToDisk("subordinate.config");
        }

        [TestMethod]
        public void WhenRetrievingAPiabConfigElementPresentInBothSources()
        {
            var configSource = new FileConfigurationSource("main.config");

            var piab = configSource.GetSection(PolicyInjectionSettings.SectionName) as PolicyInjectionSettings;

            Assert.IsNotNull(piab);

            Assert.AreEqual(3, piab.Policies.Count);

            var policy = piab.Policies.Get("Merged Policy");

            Assert.AreEqual(1, policy.MatchingRules.Count);

            var matchingRule = (MemberNameMatchingRuleData)policy.MatchingRules.Get(0);

            Assert.AreEqual(2, matchingRule.Matches.Count);

            var member = matchingRule.Matches[1];

            Assert.IsFalse(member.IgnoreCase);
        }
    }
}
