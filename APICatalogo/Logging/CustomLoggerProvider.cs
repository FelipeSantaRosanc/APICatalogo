using System.Collections.Concurrent;

namespace APICatalogo.Logging;

public class CustomLoggerProvider : ILoggerProvider
{
    readonly CustomLoggerProviderConfiguration loggerConfig;

    // Dicionário para armazenar loggers
    readonly ConcurrentDictionary<string, CustomerLogger> loggers =
               new ConcurrentDictionary<string, CustomerLogger>();

    public CustomLoggerProvider(CustomLoggerProviderConfiguration config)
    {
        loggerConfig = config;
    }

    public ILogger CreateLogger(string categoryName)
    {
        // Adiciona ou obtém o logger para a categoria
        return loggers.GetOrAdd(categoryName, name => new CustomerLogger(name, loggerConfig));
    }

    public void Dispose()
    {
        loggers.Clear(); // Limpa os loggers armazenados
    }
}
