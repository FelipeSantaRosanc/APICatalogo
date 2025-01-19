using Microsoft.AspNetCore.Mvc.Filters;

namespace APICatalogo.Extensions.Filters
{
    public class ApiLoggingFilter : IActionFilter
    {
        private readonly ILogger<ApiLoggingFilter> _logger;

        public ApiLoggingFilter(ILogger<ApiLoggingFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Executa antes da Action
            _logger.LogInformation("### Executando -> OnActionExecuted ###");
            _logger.LogInformation("############################################");
            _logger.LogInformation($"{DateTime.Now.ToShortTimeString()}");
            _logger.LogInformation($"ModelState: {context.ModelState.IsValid}");
            _logger.LogInformation("############################################");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Executa depois da Action
            _logger.LogInformation("### Executando -> OnActionExecuted ###");
            _logger.LogInformation("############################################");
            _logger.LogInformation($"{DateTime.Now.ToShortTimeString()}");
            _logger.LogInformation($" Status Code : {context.HttpContext.Response.StatusCode} ");
            _logger.LogInformation("############################################");
        }
    }
}
