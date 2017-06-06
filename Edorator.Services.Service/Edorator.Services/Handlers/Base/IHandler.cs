using System.Threading.Tasks;
using Edorator.Services.Contracts;

namespace Edorator.Services.Handlers.Base
{
    public interface IHandler<TRequest, TResponse>
        where TRequest : BaseRequest
        where TResponse : BaseResponse
    {
         Task<TResponse> Handle(TRequest request);
    }
}