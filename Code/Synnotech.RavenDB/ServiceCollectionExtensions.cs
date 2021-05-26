using System;
using Light.GuardClauses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using Raven.Client.Documents.Session;

namespace Synnotech.RavenDB
{
    /// <summary>
    /// Provides extension methods to register RavenDB services with the DI container.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Gets the default Identity Parts Separator which is '-'.
        /// </summary>
        public const char DefaultIdentityPartsSeparator = '-';

        /// <summary>
        /// Registers <see cref="IDocumentStore" /> as a singleton and <see cref="IAsyncDocumentSession" /> as a transient
        /// service. The document store is configured via <see cref="RavenDbSettings" /> that
        /// are retrieved from the <see cref="IConfiguration" /> instance (which should already be registered with the DI container).
        /// The document conventions of the store are adjusted so that the
        /// <see cref="DocumentConventions.IdentityPartsSeparator" /> is set to '-' (to prevent issues with URLs).
        /// </summary>
        /// <param name="services">The service collection used to register types with the DI container.</param>
        /// <param name="configurationSectionName">
        /// The name of the configuration section that holds the settings values for <see cref="RavenDbSettings" /> (optional).
        /// The default value is "ravenDb".
        /// </param>
        /// <param name="identityPartsSeparator">
        /// The character that is used as the Identity Parts Separator for document IDs. The default separator is '/'
        /// which might cause issues when these IDs are used in URLs. We therefore recommend to use '-' by default.
        /// </param>
        /// <param name="sessionLifetime">
        /// The lifetime that is used to register RavenDB's <see cref="IAsyncDocumentSession"/> with the DI container (optional).
        /// The default value is <see cref="ServiceLifetime.Transient"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="services" /> is null.</exception>
        public static IServiceCollection AddRavenDb(this IServiceCollection services,
                                                    string configurationSectionName = RavenDbSettings.DefaultSectionName,
                                                    char identityPartsSeparator = DefaultIdentityPartsSeparator,
                                                    ServiceLifetime sessionLifetime = ServiceLifetime.Transient)
        {
            services.MustNotBeNull(nameof(services));

            services.AddSingleton(container => InitializeDocumentStoreFromConfiguration(container.GetRequiredService<IConfiguration>(), configurationSectionName, identityPartsSeparator))
                    .Add(new ServiceDescriptor(
                             typeof(IAsyncDocumentSession),
                             container => container.GetRequiredService<IDocumentStore>().OpenAsyncSession(),
                             sessionLifetime
                         ));
            return services;
        }

        /// <summary>
        /// Initializes the RavenDB document store. The settings for the store will be retrieved from the <paramref name="configuration" />
        /// instance using the default <see cref="RavenDbSettings" />. The document conventions of the store are adjusted so that the
        /// <see cref="DocumentConventions.IdentityPartsSeparator" /> is set to '-' (to prevent issues with URLs).
        /// </summary>
        /// <param name="configuration">The configuration where the RavenDB settings are loaded from.</param>
        /// <param name="configurationSectionName">
        /// The name of the configuration section that holds the settings values for <see cref="RavenDbSettings" /> (optional).
        /// The default value is "ravenDb".
        /// </param>
        /// <param name="identityPartsSeparator">
        /// The character that is used as the Identity Parts Separator for document IDs. The default separator is '/'
        /// which might cause issues when these IDs are used in URLs. We therefore recommend to use '-' by default.
        /// </param>
        /// <returns>The initialized RavenDB document store</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="configuration" /> is null.</exception>
        public static IDocumentStore InitializeDocumentStoreFromConfiguration(IConfiguration configuration,
                                                                              string configurationSectionName = RavenDbSettings.DefaultSectionName,
                                                                              char identityPartsSeparator = DefaultIdentityPartsSeparator)
        {
            var ravenDbSettings = RavenDbSettings.FromConfiguration(configuration, configurationSectionName);
            return new DocumentStore
            {
                Urls = ravenDbSettings.ServerUrls.ToArray(),
                Database = ravenDbSettings.DatabaseName,
                Conventions = new DocumentConventions().SetIdentityPartsSeparator(identityPartsSeparator)
            }.Initialize();
        }

        /// <summary>
        /// Sets the <paramref name="identityPartsSeparator" /> on the specified document conventions. We recommend you use '-'
        /// as the separator, as the default separator '/' might collide with URLs in web apps.
        /// </summary>
        /// <param name="documentConventions">The document conventions that will be manipulated.</param>
        /// <param name="identityPartsSeparator">The character that should be used to separate the different parts of a RavenDB ID (optional). The default value is '-'.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="documentConventions" /> is null.</exception>
        public static DocumentConventions SetIdentityPartsSeparator(this DocumentConventions documentConventions, char identityPartsSeparator = DefaultIdentityPartsSeparator)
        {
            documentConventions.MustNotBeNull(nameof(documentConventions));
            documentConventions.IdentityPartsSeparator = identityPartsSeparator;
            return documentConventions;
        }
    }
}