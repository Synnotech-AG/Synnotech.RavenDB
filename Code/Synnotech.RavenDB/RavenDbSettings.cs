﻿using System;
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
        /// The default RavenDB server URL that is assigned to <see cref="ServerUrl"/>.
        /// </summary>
        public const string DefaultServerUrl = "http://localhost:10001";

        /// <summary>
        /// Gets or sets the URL to the target RavenDB server.
        /// </summary>
        public string ServerUrl { get; set; } = DefaultServerUrl;

        /// <summary>
        /// Gets or sets the name of the target database.
        /// </summary>
        public string DatabaseName { get; set; } = string.Empty;

        /// <summary>
        /// Loads the <see cref="RavenDbSettings"/> from configuration or provides the default instance.
        /// </summary>
        /// <param name="configuration">The configuration instance where the settings are loaded from.</param>
        /// <param name="sectionName">The name of the section that represents the RavenDB settings.</param>
        /// <returns>The loaded settings, or the default instance when the section cannot be loaded.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="configuration"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="sectionName"/> is an empty string or contains only whitespace.</exception>
        public static RavenDbSettings FromConfiguration(IConfiguration configuration, string sectionName = DefaultSectionName) =>
            FromConfiguration<RavenDbSettings>(configuration, sectionName);

        /// <summary>
        /// Loads the RavenDB settings from configuration or provides the default instance.
        /// </summary>
        /// <typeparam name="T">The subtype of RavenDB settings that will be used to load the settings.</typeparam>
        /// <param name="configuration">The configuration instance where the settings are loaded from.</param>
        /// <param name="sectionName">The name of the section that represents the RavenDB settings.</param>
        /// <returns>The loaded settings, or the default instance when the section cannot be loaded.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="configuration"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="sectionName"/> is an empty string or contains only whitespace.</exception>
        public static T FromConfiguration<T>(IConfiguration configuration, string sectionName = DefaultSectionName)
        {
            configuration.MustNotBeNull(nameof(configuration));
            sectionName.MustNotBeNullOrWhiteSpace(nameof(sectionName));
            return configuration.GetSection(sectionName)
                                .Get<T?>() ?? throw new InvalidConfigurationException($"RavenDB settings could not be retrieved from configuration section \"{sectionName}\".");
        }
    }
}