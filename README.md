# Synnotech.RavenDB

*Extensions for RavenDB that make your data access code easier.*

[![Synnotech Logo](synnotech-large-logo.png)](https://www.synnotech.de/)

[![License](https://img.shields.io/badge/License-MIT-green.svg?style=for-the-badge)](https://github.com/Synnotech-AG/Synnotech.RavenDB/blob/main/LICENSE)
[![NuGet](https://img.shields.io/badge/NuGet-3.0.0-blue.svg?style=for-the-badge)](https://www.nuget.org/packages/Synnotech.RavenDB/)

# How to Install

Synnotech.RavenDB is compiled against [.NET Standard 2.0 and 2.1](https://docs.microsoft.com/en-us/dotnet/standard/net-standard) and thus supports all major plattforms like .NET 5, .NET Core, .NET Framework 4.6.1 or newer, Mono, Xamarin, UWP, or Unity.

Synnotech.RavenDB is available as a [NuGet package](https://www.nuget.org/packages/Synnotech.RavenDB/) and can be installed via:

- **Package Reference in csproj**: `<PackageReference Include="Synnotech.RavenDB" Version="3.0.0" />`
- **dotnet CLI**: `dotnet add package Synnotech.RavenDB`
- **Visual Studio Package Manager Console**: `Install-Package Synnotech.RavenDB`

# What does Synnotech.RavenDB offer you?

Synnotech.RavenDB implements the session abstractions of [Synnotech.DatabaseAbstractions](https://github.com/Synnotech-AG/Synnotech.DatabaseAbstractions) for RavenDB 5 or newer. This allows you to simplify your the code in your data access layer. Furthermore, Synnotech.RavenDB allows you to configure your DI container with one call when you want to use the Synnotech default settings for RavenDB.

# Default configuration

Synnotech.RavenDB provides extension methods for `IServiceCollection` to easily get started in e.g. ASP.NET Core apps. Simply call `AddRavenDb` to register an `IDocumentStore` as a singleton and an `IAsyncDocumentSession` as a transient (you can change the lifetime of the session via the optional `sessionLifetime` argument).

In Startup.cs:

```csharp
public void ConfigureService(IServiceCollection services)
{
    // All other services like MVC are left out for brevity's sake 
    services.AddRavenDb();
}
```

The document store is configured using the `IConfiguration` instance that must already be part of the DI container (it is automatically loaded by ASP.NET Core's host builder). Within the configuration, a section called "ravenDb" is searched and deserialized to an instance of `RavenDbSettings` (the section name can be configured with the optional `configurationSectionName` argument of `AddRavenDb`). The settings object can be used to configure the Server URLs and database name.

Thus you can configure the settings to access your Raven database via appsettings.json:

```jsonc
{
    // other configuration sections are left out for brevity's sake
    "ravenDB": {
        "serverUrls": [ "http://localhost:10001" ],
        "databaseName": "MyAwesomeRavenDatabase"
    }
}
```

Additionally, the document conventions of the store are adjusted: the `IdentityPartsSeparator` is set to '-' (default value is '/'). This is done to avoid issues when IDs are part of URLs. You can configure it with the optional `identityPartsSeparator` argument of `AddRavenDb`.

If you don't want to use `AddRavenDb`, you can still use the [RavenDbSettings](https://github.com/Synnotech-AG/Synnotech.RavenDB/blob/main/Code/Synnotech.RavenDB/RavenDbSettings.cs) class and the `SetIdentityPartsSeparator` and `InitializeDocumentStoreFromConfiguration` methods of [ServiceCollectionExtensions](https://github.com/Synnotech-AG/Synnotech.RavenDB/blob/main/Code/Synnotech.RavenDB/ServiceCollectionExtensions.cs) individually in your custom setup.

# Writing custom sessions

When writing code that performs I/O with RavenDB, we usually write custom abstractions, containing a single method for each I/O request. The following sections show you how to write abstractions, implement them, and call them in client code.

## Sessions that only read from RavenDB

The following code snippets show the example for an ASP.NET Core controller that represents an HTTP GET operation for contacts:

Your I/O abstraction should simply derive from Synnotech.DatabaseAbstractions' `IAsyncReadOnlySession` and offer the corresponding I/O call to load contacts:

```csharp
public interface IGetContactsSession : IAsyncReadOnlySession
{
    Task<List<Contact>> GetContactsAsync(int skip, int take);
}
```

To implement this interface, you should derive from the `AsyncReadOnlySession` class of Synnotech.RavenDB:

```csharp
public sealed class RavenGetContactsSession : AsyncReadOnlySession, IGetContactsSession
{
    public RavenGetContactsSession(IAsyncDocumentSession session) : base(session) { }
    
    public Task<List<Contact>> GetContactsAsync(int skip, int take) =>
        Session.Query<Contact>()
               .OrderBy(contact => contact.LastName)
               .Skip(skip)
               .Take(take)
               .ToListAsync();
}
```

`AsyncReadOnlySession` implements `IAsyncReadOnlySession`, `IDisposable` and `IAsyncDisposable` for you and provides RavenDB's `IAsyncDocumentSession` via the protected `Session` property. This reduces the code that you need to write in your session for your specific use case.

You can then consume your session via the abstraction in client code. Check out the following ASP.NET Core controller for example:

```csharp
[ApiController]
[Route("api/contacts")]
public sealed class GetContactsController : ControllerBase
{
    public GetContactsController(Func<IGetContactsSession> createSession) =>
        CreateSession = createSession;
        
    private Func<IGetContractsSession> CreateSession { get; }
    
    [HttpGet]
    public async Task<ActionResult<List<ContactDto>>> GetContacts(int skip, int take)
    {
        if (this.CheckPagingParametersForErrors(skip, take, out var badResult))
            return badResult;
        
        await using var session = CreateSession();
        var contacts = await session.GetContactsAsync(skip, take);
        return ContactDto.FromContacts(contacts);
    }
}
```

In this example, a `Func<IGetContactsSession>` is injected into the controller. This factory delegate is used to instantiate the session once the parameters are validated. We recommend that you do not register your session as "scoped", but rather as transient with your DI container (because it's the controllers responsibility to properly open and close the session). This allows you to test if the session is disposed correctly without setting up the whole ASP.NET Core ecosystem to instantiate the controller. You can inject a `Func<IGetContactsSession>` if you use a proper DI container like [LightInject](https://github.com/seesharper/LightInject) that supports [Function Factories](https://www.lightinject.net/#function-factories), or if you explicitly register it as a singleton:

```csharp
services.AddTransient<IGetContactsSession, RavenGetContactsSession>();
// The next call is not necessary if your DI container can automatically resolve
// Func<T> where T is already registered. LightInject is able to do this.
services.AddSingleton<Func<IGetContactsSession>>(container => container.GetRequiredService<IGetContactsSession>);
```

## Sessions that manipulate data

If your session requires the `SaveChangesAsync` method, you should derive from `Synnotech.DatabaseAbstractions.IAsyncSession`. `AsyncSession` implements this interface for you. `AsyncSession` also supports aborting async opertions via `CancellationToken`.

### Example for updating an existing document

The abstraction might look like this:

```csharp
public interface IUpdateContactSession : IAsyncSession
{
    Task<Contact?> GetContactAsync(string id);
}
```

The class that implements this interface should derive from `AsyncSession`, which provides the same members as `AsyncReadOnlySession` plus the `SaveChangesAsync` method:

```csharp
public sealed class RavenUpdateContactSession : AsyncSession, IUpdateContactSession
{
    public RavenUpdateContactSession(IAsyncDocumentSession session) : base(session) { }
    
    public Task<Contact?> GetContactAsync(string id) => Session.LoadAsync<Contact?>(id);
}
```

Your controller could then look like this:

```csharp
[ApiController]
[Route("api/contacts/update")]
public sealed class UpdateContactController : ControllerBase
{
    public UpdateContactController(Func<IUpdateContactSession> createSession, ContactValidator validator)
    {
        CreateSession = createSession;
        Validator = validator;
    }
    
    private Func<IUpdateContactSession> CreateSession { get; }
    private ContactValidator Validator { get; }
    
    [HttpPut]
    public async Task<IActionResult> UpdateContact(ContactDto contactDto)
    {
        if (this.CheckForErrors(contactDto, Validator, out var badResult))
            return badResult;
            
        await using var session = CreateSession();
        var contact = await session.GetContactAsync(contactDto.Id);
        if (contact == null)
            return NotFound();
        contactDto.UpdateContact(contact); // Or use an object-to-object mapper
        await session.SaveChangesAsync();
        return NoContent();
    }
}
```

In the same manner as in the previous example, the session is injected via a factory delegate. After validation succeeded, the session is instantiated and used to retrieve the requested contact. If it is not available, an HTTP 404 NOT FOUND response is returned. Otherwise, the contact object is updated with the properties and then saved to the database via `SaveChangesAsync`.

### Example for creating a new document

In the same way as in the previous example, the abstraction for your session should derive from `Synnotech.DatabaseAbstractions.IAsyncSession`:

```csharp
public interface ICreateContactSession : IAsyncSession
{
    public Task AddContactAsync(Contact contact);
}
```

Also, the implementation should derive from `AsyncSession` like in the previous example:

```csharp
public sealed class RavenCreateContactSession : AsyncSession, ICreateContactSession
{
    public RavenCreateContactSession(IAsyncDocumentSession session) : base(session) { }
    
    public Task AddContactAsync(Contact contact) => Session.StoreAsync(contact);
}
```

Your controller might then look something like this:

```csharp
[ApiController]
[Route("api/contact/create")]
public sealed class CreateContactController : ControllerBase
{
    public CreateContactController(Func<ICreateContactSession> createSession,
                                   ContactValidator validator)
    {
        CreateSession = createSession;
        Validator = validator;
    }
    
    private Func<ICreateContactSession> CreateSession { get; }
    private ContactValidator Validator { get; }
    
    [HttpPost]
    public async Task<IActionResult> CreateNewContact(NewContactDto newContactDto)
    {
        if (this.CheckForErrors(newContactDto, Validator, out var badResult)
            return badResult;
        
        await using var session = CreateSession();
        var newContact = newContactDto.ConvertToContact(); // Or use an object-to-object mapper
        await session.AddContactAsync(newContact);
        await session.SaveChangesAsync();
        return Created("api/contacts/" + newContact.Id, newContact);
    }
}
```

Again, the access to the session is provided via a factory delegate. Once the parameters are validated, the session is instantiated and used to create a new contact.

# General recommendations

1. All I/O should be abstracted. You should create abstractions that are specific for your use cases.
2. Your custom abstractions should derive from `IAsyncReadOnlySession` (when they only read data from RavenDB) or from `IAsyncSession` (when they also manipulate data). Use `AsyncReadOnlySession` and `AsyncSession` as base classes to implement those abstractions.
3. Prefer async I/O over sync I/O. Threads that wait for a database query to complete can handle other requests in the meantime when the query is performed asynchronously. This prevents thread starvation under high load and allows your web service to scale better. If you really decide to use synchronous access to RavenDB, there are the `ReadOnlySession` and `Session` base classes that you can use to implement abstractions. You must register the store and document session on your own against the DI container in this case.
4. In case of web apps, we do not recommend using the DI container to dispose of the session. Instead, it is the controller's responsibility to do that. This way you can easily test the controller without running the whole ASP.NET Core infrastructure in your tests. To make your life easier, use an appropriate DI container like [LightInject](https://github.com/seesharper/LightInject) instead of Microsoft.Extensions.DependencyInjection. These more sophisticated DI containers provide you with more features, e.g. [Function Factories](https://www.lightinject.net/#function-factories).

# Migration Guides

## From 1.0.0 to 2.0.0

### 1. Breaking change: RavenDbSettings.ServerUrls

The former `string ServerUrl` property is now `List<string> ServerUrls`. This means that you need to adjust your appsettings.json:

```jsonc
{
    "ravenDB": {
        // serverURLs with an 's', you must provide a JSON array now
        "serverUrls": [ "http://localhost:10001" ], 
        "databaseName": "MyAwesomeRavenDatabase"
    }
}
```

Furthermore, the default server URL is no longer localhost:10001, but an empty list of server URLs. This was done to support connections to a cluster of RavenDB servers.

### 2. Breaking changes that require recompilation

The following changes require recompilation. The surface areas of the APIs were either extended or stayed the same.

- `AsyncReadOnlySession` now directly implements `IAsyncReadOnlySession` instead of `IAsyncDisposable` and `IDisposable`
- `AsyncSession` now supports an optional cancellation token on `SaveChangesAsync`.
- more optional parameters in `AddRavenDb` and `InitializeDocumentStoreFromConfiguration`.

## From 2.0.0 to 3.0.0

Synnotech.RavenDB now references Synnotech.DatabaseAbstractions 3.0.0, thus you need to recompile. There are no breaking changes in the API.