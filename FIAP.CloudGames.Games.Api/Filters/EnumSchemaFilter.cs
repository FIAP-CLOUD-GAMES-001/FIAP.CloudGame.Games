using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FIAP.CloudGames.Api.Filters
{
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                schema.Enum.Clear();
                
                // Adicionar os nomes dos enums como strings (comportamento esperado com JsonStringEnumConverter)
                foreach (var enumName in Enum.GetNames(context.Type))
                {
                    schema.Enum.Add(new OpenApiString(enumName));
                }
                
                // Definir o tipo como string para refletir o comportamento do JsonStringEnumConverter
                schema.Type = "string";
            }
        }
    }
}
