using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CustomerBase.Adapter.WebAPI.Filters
{
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Verifique se há atributos de autenticação na operação
            var hasAuthorizeAttributes = context.MethodInfo.GetCustomAttributes(true)
                .OfType<AuthorizeAttribute>()
                .Any();

            if (hasAuthorizeAttributes)
            {
                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

                operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    // Adicione o nome do esquema de segurança, "Bearer", e uma lista vazia de escopos
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
                }
            };
            }
        }
    }

}
