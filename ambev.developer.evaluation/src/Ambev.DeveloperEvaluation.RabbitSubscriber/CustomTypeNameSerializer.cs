using EasyNetQ;

namespace Ambev.DeveloperEvaluation.RabbitSubscriber;

public class CustomTypeNameSerializer : ITypeNameSerializer
{
    public string Serialize(Type type)
    {
        return type.Name;
    }

    public Type DeSerialize(string typeName)
    {
        // Procura o tipo em todos os assemblies carregados
        var type = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .FirstOrDefault(t => t.Name == typeName);

        if (type == null)
            throw new Exception($"Tipo '{typeName}' não encontrado.");

        return type;
    }
}