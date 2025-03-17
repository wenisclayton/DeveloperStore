using EasyNetQ;

namespace Ambev.DeveloperEvaluation.Integration.Adapters.Messaging.RabbitMQ;

public class CustomTypeNameSerializer : ITypeNameSerializer
{
    public string Serialize(Type type)
    {
        return type.Name;
    }

    public Type DeSerialize(string typeName)
    {
var type = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .FirstOrDefault(t => t.Name == typeName);

        if (type == null)
            throw new Exception($"Tipo '{typeName}' não encontrado.");

        return type;
    }
}