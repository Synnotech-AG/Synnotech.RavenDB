using System;
using System.Collections.Generic;
using Light.GuardClauses;
using Light.GuardClauses.Exceptions;
using Microsoft.Extensions.Configuration;

namespace Synnotech.RavenDB
{
    /// <summary>
    /// Represents the default settings that are used to connect to a RavenDB database.
    /// </summary>
    public class RavenDbSettings
    {
        /// <summary>
        /// The default section name within the IConfiguration where the settings are loaded from.
        /// </summary>
        public const string DefaultSectionName = "ravenDb";

        /// <summary>
        /// Gets or sets the URL to the target RavenDB server.
        /// </summary>
        public List<string> ServerUrls { get; set; } = new ();

        /// <summary>
        /// Gets or sets the name of the target database.
        /// </summary>
        public string DatabaseName { get; set; } = string.Empty;

        /// <summary>
        /// Loads the <see cref="RavenDbSettings"/> from configuration.
        /// </summary>
        /// <param name="configuration">The configuration instance where the settings are loaded from.</param>
        /// <param name="sectionName">The name of the section that represents the RavenDB settings.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="configuration"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="sectionName"/> is an empty string or contains only whitespace.</exception>
        /// <exception cref="InvalidConfigurationException">Thrown when the settings could not be loaded (most likely because the section is not present in the configuration).</exception>
        public static RavenDbSettings FromConfiguration(IConfiguration configuration, string sectionName = DefaultSectionName) =>
            FromConfiguration<RavenDbSettings>(configuration, sectionName);

        /// <summary>
        /// Loads the RavenDB settings from configuration.
        /// </summary>
        /// <typeparam name="T">The type of RavenDB settings that will be used to load the settings.</typeparam>
        /// <param name="configuration">The configuration instance where the settings are loaded from.</param>
        /// <param name="sectionName">The name of the section that represents the RavenDB settings.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="configuration"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="sectionName"/> is an empty string or contains only whitespace.</exception>
        /// <exception cref="InvalidConfigurationException">Thrown when the settings could not be loaded (most likely because the section is not present in the configuration).</exception>
        public static T FromConfiguration<T>(IConfiguration configuration, string sectionName = DefaultSectionName)
        {
            configuration.MustNotBeNull(nameof(configuration));
            sectionName.MustNotBeNullOrWhiteSpace(nameof(sectionName));
            return configuration.GetSection(sectionName)
                                .Get<T?>() ?? throw new InvalidConfigurationException($"RavenDB settings could not be retrieved from configuration section \"{sectionName}\".");
        }
    }
}