using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Microsoft.Extensions.Configuration;
using Synnotech.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Synnotech.RavenDB.Tests
{
    public sealed class RavenDbSettingsTests
    {
        public RavenDbSettingsTests(ITestOutputHelper output)
        {
            Output = output;
        }

        private ITestOutputHelper Output { get; }

        [Theory]
        [InlineData(RavenDbSettings.DefaultSectionName, RavenDbSettings.DefaultServerUrl, "My-Database")]
        [InlineData("someOtherSection", "http://localhost:8000", "TheDatabase")]
        public static void LoadDefaultConfiguration(string sectionName, string serverUrl, string databaseName)
        {
            var configuration = CreateConfiguration(sectionName, serverUrl, databaseName);
            var ravenDbSettings = RavenDbSettings.FromConfiguration(configuration, sectionName);

            var expectedSettings = new RavenDbSettings { ServerUrl = serverUrl, DatabaseName = databaseName };
            ravenDbSettings.Should().BeEquivalentTo(expectedSettings);
        }

        [Theory]
        [InlineData("myRavenSection", "http://localhost:3500", "databaseName", 35)]
        [InlineData("ravenDb", "http://localhost:10001", "My-Database", 104383)]
        public static void LoadCustomSettings(string sectionName, string serverUrl, string databaseName, int otherValue)
        {
            var configuration = CreateConfiguration(sectionName, serverUrl, databaseName, new KeyValuePair<string, string>($"{sectionName}:otherValue", otherValue.ToString()));

            var customSettings = RavenDbSettings.FromConfiguration<CustomRavenDbSettings>(configuration, sectionName);

            var expectedSettings = new CustomRavenDbSettings { ServerUrl = serverUrl, DatabaseName = databaseName, OtherValue = otherValue };
            customSettings.Should().BeEquivalentTo(expectedSettings);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("\t")]
        [InlineData("\r\n")]
        public void InvalidSectionName(string invalidSectionName)
        {
            var configuration = CreateConfiguration();

            Action act = () => RavenDbSettings.FromConfiguration(configuration, invalidSectionName);

            act.Should().Throw<ArgumentException>()
               .Which.ShouldBeWrittenTo(Output);
        }

        [Theory]
        [InlineData("someOtherSection")]
        [InlineData("thisSectionIsNotCalledRavenDb")]
        public void NonExistingSection(string nonExistingSection)
        {
            var configuration = CreateConfiguration();

            Action act = () => RavenDbSettings.FromConfiguration(configuration, nonExistingSection);

            act.Should().Throw<InvalidConfigurationException>()
               .Which.ShouldBeWrittenTo(Output);
        }

        [Fact]
        public void ConfigurationNull()
        {
            Action act = () => RavenDbSettings.FromConfiguration(null!);

            var exception = act.Should().Throw<ArgumentNullException>().Which;
            exception.ParamName.Should().Be("configuration");
            exception.ShouldBeWrittenTo(Output);
        }

        private static IConfiguration CreateConfiguration(string sectionName = RavenDbSettings.DefaultSectionName,
                                                          string serverUrl = RavenDbSettings.DefaultServerUrl,
                                                          string databaseName = "My-Database",
                                                          params KeyValuePair<string, string>[] customSettings)
        {
            return new ConfigurationBuilder()
                  .AddInMemoryCollection(
                       new[]
                       {
                           new KeyValuePair<string, string>($"{sectionName}:serverUrl", serverUrl),
                           new KeyValuePair<string, string>($"{sectionName}:databaseName", databaseName)
                       }.Concat(customSettings)
                   )
                  .Build();
        }

        private sealed class CustomRavenDbSettings : RavenDbSettings
        {
            // ReSharper disable once UnusedAutoPropertyAccessor.Local -- implicitly used by Should().BeEquivalentTo
            public int OtherValue { get; init; } = 42;
        }
    }
}