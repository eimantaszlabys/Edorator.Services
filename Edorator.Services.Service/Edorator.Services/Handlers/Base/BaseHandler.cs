using System;
using System.Threading.Tasks;
using Edorator.Services.Contracts;
using Microsoft.Extensions.Logging;

namespace Edorator.Services.Handlers.Base
{
    public abstract class BaseHandler<TRequest, TResponse> : IHandler<TRequest, TResponse> 
        where TRequest : BaseRequest 
        where TResponse : BaseResponse
    {
        private readonly ILogger _logger;

        protected BaseHandler(ILoggerFactory logger)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            _logger = logger.CreateLogger("BaseHandler");
        }

        public Task<TResponse> Handle(TRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                return HandleCore(request);
            }
            catch (Exception e)
            {
                _logger.LogError("Error on handling.", e);
                Console.WriteLine(e);
                throw;
            }
        }

        protected abstract Task<TResponse> HandleCore(TRequest request);
    }
}