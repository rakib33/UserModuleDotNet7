using Microsoft.AspNetCore.Mvc.Filters;

namespace UserManagementCore.Filters
{
    public class MyActionFilters : IActionFilter
    {
        private readonly ILogger<MyActionFilters> logger;
        public MyActionFilters(ILogger<MyActionFilters> logger)
        {
            this.logger = logger;
        }
        /// <summary>
        /// Before Action
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogInformation("OnActionExecuted");

        }

        /// <summary>
        /// After Action
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            logger.LogInformation("OnActionExecuted");
        }

    }
}
