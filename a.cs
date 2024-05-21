public interface ILogger
{
    void LogInfo(string message);
    void LogError(string message, Exception ex);
}

public class FileLogger : ILogger
{
    public void LogInfo(string message)
    {
        // Implement logging to a file
        Console.WriteLine($"INFO: {message}");
    }

    public void LogError(string message, Exception ex)
    {
        // Implement logging errors to a file
        Console.WriteLine($"ERROR: {message} - {ex.Message}");
    }
}


public interface IEncryptionStrategy
{
    string Encrypt(string plainText);
    string Decrypt(string cipherText);
}

public class AesEncryptionStrategy : IEncryptionStrategy
{
    public string Encrypt(string plainText)
    {
        // AES encryption logic
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(plainText));
    }

    public string Decrypt(string cipherText)
    {
        // AES decryption logic
        return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(cipherText));
    }
}



public interface IRepository<T>
{
    IEnumerable<T> GetAll();
    T GetById(int id);
    void Add(T entity);
    void Update(T entity);
    void Delete(int id);
}

public class Repository<T> : IRepository<T> where T : class
{
    // Assume we have a DbContext instance injected here for Entity Framework
    private readonly DbContext _context;

    public Repository(DbContext context)
    {
        _context = context;
    }

    public IEnumerable<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }

    public T GetById(int id)
    {
        return _context.Set<T>().Find(id);
    }

    public void Add(T entity)
    {
        _context.Set<T>().Add(entity);
        _context.SaveChanges();
    }

    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var entity = _context.Set<T>().Find(id);
        if (entity != null)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }
    }
}


[ApiController]
[Route("api/[controller]")]
public class MyController : ControllerBase
{
    private readonly ICoreFactory _coreFactory;

    public MyController(ICoreFactory coreFactory)
    {
        _coreFactory = coreFactory;
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        try
        {
            // Use factory to create instances
            var logger = _coreFactory.CreateLogger();
            var repository = _coreFactory.CreateRepository<MyEntity>();
            var encryptionStrategy = _coreFactory.CreateEncryptionStrategy();

            // Perform operations
            var entity = repository.GetById(id);
            if (entity == null)
            {
                logger.LogInfo($"Entity with id {id} not found.");
                return NotFound();
            }

            logger.LogInfo($"Entity with id {id} retrieved successfully.");
            var encryptedData = encryptionStrategy.Encrypt("Sample data");

            return Ok(new { entity, encryptedData });
        }
        catch (Exception ex)
        {
            var logger = _coreFactory.CreateLogger();
            logger.LogError("An error occurred while fetching the entity.", ex);
            return StatusCode(500, "Internal server error");
        }
    }
}



using System.Web.Http;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;

public static class WebApiConfig
{
    public static void Register(HttpConfiguration config)
    {
        // Configure Unity Container
        var container = new UnityContainer();

        // Register core services
        container.RegisterType<ILogger, FileLogger>(new TransientLifetimeManager());
        container.RegisterType<IEncryptionStrategy, AesEncryptionStrategy>(new TransientLifetimeManager());
        container.RegisterType(typeof(IRepository<>), typeof(Repository<>), new TransientLifetimeManager());

        // Register the factory
        container.RegisterSingleton<ICoreFactory, CoreFactory>();

        // Set the dependency resolver
        config.DependencyResolver = new UnityDependencyResolver(container);

        // Web API routes
        config.MapHttpAttributeRoutes();

        config.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional }
        );
    }
}

