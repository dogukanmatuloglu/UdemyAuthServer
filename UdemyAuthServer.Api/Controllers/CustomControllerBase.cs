using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Dtos;

namespace UdemyAuthServer.Api.Controllers
{
    public class CustomControllerBase : ControllerBase
    {
        public IActionResult ActionResultInstance<T>(Response<T> response)where T : class
        {
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode,
            };
        }
    }
}
