using Microsoft.AspNetCore.Mvc.Filters;

namespace UserManagementCore.Filters
{
    public class MyExceptionFilters : ExceptionFilterAttribute
    {
        private readonly ILogger<MyExceptionFilters> logger;
        public MyExceptionFilters(ILogger<MyExceptionFilters> logger)
        {
            this.logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            logger.LogError(context.Exception, context.Exception.Message);
            base.OnException(context);
        }
    }
}
