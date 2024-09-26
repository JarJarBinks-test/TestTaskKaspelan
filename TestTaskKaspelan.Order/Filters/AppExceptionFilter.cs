using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace TestTaskKaspelan.Order.Filters
{
    /// <summary>
    /// Application exception filter.
    /// </summary>
    public class AppExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<AppExceptionFilter> _logger;

        /// <summary>
        /// Application exception filter constructor.
        /// </summary>
        /// <param name="logger">Class loger.</param>
        public AppExceptionFilter(ILogger<AppExceptionFilter> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Handle exception method.
        /// </summary>
        /// <param name="context">Exception context.</param>
        public void OnException(ExceptionContext context)
        {
            // TODO: For additional logic.
            context.Result = new BadRequestResult();
        }
    }
}
