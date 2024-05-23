 config
            .EnableSwagger(c =>
            {
                c.SingleApiVersion("v1", "Your API");
                c.OAuth2("oauth2")
                    .Description("OAuth2 Implicit Grant")
                    .Flow("implicit")
                    .AuthorizationUrl("/token")
                    .TokenUrl("/token")
                    .Scopes(scopes =>
                    {
                        scopes.Add("read", "Read access to protected resources");
                        scopes.Add("write", "Write access to protected resources");
                    });
                c.OperationFilter<AssignOAuth2SecurityRequirements>();
            })
            .EnableSwaggerUi(c =>
            {
                c.EnableOAuth2Support(
                    clientId: "your-client-id",
                    clientSecret: null,
                    realm: "your-realm",
                    appName: "Swagger UI",
                    additionalQueryStringParams: new Dictionary<string, string>() { { "response_type", "token" } });
            });

        app.UseWebApi(config);
    }
}

public class AssignOAuth2SecurityRequirements : IOperationFilter
{
    public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
    {
        if (operation.security == null)
            operation.security = new List<IDictionary<string, IEnumerable<string>>>();

        var oAuthRequirements = new Dictionary<string, IEnumerable<string>>
        {
            { "oauth2", new[] { "read", "write" } }
        };

        operation.security.Add(oAuthRequirements);
    }
}























