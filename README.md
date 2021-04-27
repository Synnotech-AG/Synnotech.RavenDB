# Synnotech.RavenDB

*Extensions for RavenDB that make your data access code easier.*

[![Synnotech Logo](synnotech-large-logo.png)](https://www.synnotech.de/)

[![License](https://img.shields.io/badge/License-MIT-green.svg?style=for-the-badge)](https://github.com/Synnotech-AG/Synnotech.RavenDB/blob/main/LICENSE)
[![NuGet](https://img.shields.io/badge/NuGet-1.0.0-blue.svg?style=for-the-badge)](https://www.nuget.org/packages/Synnotech.RavenDB/)

# How to Install

Synnotech.RavenDB is compiled against [.NET Standard 2.0 and 2.1](https://docs.microsoft.com/en-us/dotnet/standard/net-standard) and thus supports all major plattforms like .NET 5, .NET Core, .NET Framework 4.6.1 or newer, Mono, Xamarin, UWP, or Unity.

Synnotech.RavenDB is available as a [NuGet package](https://www.nuget.org/packages/Synnotech.RavenDB/) and can be installed via:

- **Package Reference in csproj**: `<PackageReference Include="Synnotech.RavenDB" Version="1.0.0" />`
- **dotnet CLI**: `dotnet add package Synnotech.RavenDB`
- **Visual Studio Package Manager Console**: `Install-Package Synnotech.RavenDB`

# What does Synnotech.RavenDB offer you?

Synnotech.RavenDB implements the session abstractions of [Synnotech.DatabaseAbstractions](https://github.com/Synnotech-AG/Synnotech.DatabaseAbstractions) for RavenDB 5 or newer. I/O calls should be abstracted so that the calling code can be easily tested without a RavenDB server being necessary. Also, any I/O should be performed asynchronously by default to avoid blocking threads, which is especially important in service apps that might suffer from thread starvation when too many Thread Pool threads are blocked at the same time.

## Sessions that only read from RavenDB

When writing code that performs I/O with RavenDB, you should create a custom interface that represents each I/O operation with a single method. The following code snippets show the example for an ASP.NET Core controller that represents an HTTP GET operation for contacts:

The I/O abstraction:

```csharp
public interface IGetContactsSession : IDisposable
{
    Task<List<Contact>> GetContactsAsync(int skip, int take);
}
```

To implement this interface, you should derive from the `AsyncReadOnlySession` interface of Synnotech.RavenDB:

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

`AsyncReadOnlySession` implements `IDisposable` and `IAsyncDisposable` for you and provides RavenDB's `IAsyncDocumentSession` via the protected `Session` property. This reduces the code that you need to write in your session for your specific use case.

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
        
        using var session = CreateSession();
        var contacts = await session.GetContactsAsync(skip, take);
        return ContactDto.FromContacts(contacts);
    }
}
```

In this example, a `Func<IGetContactsSession` is injected into the controller. This factory delegate is used to instantiate the session once the parameters are validated. We recommend that you do not register your session as "scoped", but rather as transient with your DI container (because it's the controllers responsibility to properly open and close the session). This allows you to test if the session is disposed correctly without setting up the whole ASP.NET Core ecosystem to instantiate the controller.

For this to work, we suggest that you use a DI container like [LightInject](https://github.com/seesharper/LightInject) that automatically provides you with [function factories](https://www.lightinject.net/#function-factories) once you have registered a type.

## Sessions that manipulate data

If your session requires the `SaveChangesAsync` method, you should derive from `Synnotech.DatabaseAbstractions.IAsyncSession`.

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
            
        using var session = CreateSession();
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
        
        using var session = CreateSession();
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
2. Prefer async I/O over sync I/O because threads that wait for a database query to complete can handle other requests in the meantime. This prevents thread starvation.
3. Your custom abstractions can derive from `IDisposable` (when they only read data from RavenDB) or from `IAsyncSession` (when they also manipulate data).
4. In case of web apps, we do not recommend using the DI container to dispose of the session. Instead, it is the controller's responsibility to do that. This way you can easily test the controller without running the whole ASP.NET Core infrastructure in your tests. To make your life easier, use an appropriate DI container like [LightInject](https://github.com/seesharper/LightInject) that provides more functionality like [Function Factories](https://www.lightinject.net/#function-factories) instead of Microsoft.Extensions.DependencyInjection.