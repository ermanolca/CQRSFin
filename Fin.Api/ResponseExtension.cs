using Fin.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fin.Api
{
    public static class ResponseExtension
    {
        public static IActionResult ToResponse(this FinResponse response)
        {
            if (response.IsUnsuccessful)
                return new ObjectResult(new { response.ErrorMessage }) { StatusCode = response.StatusCode };

            else if (response.HasData)
                return new ObjectResult(new { Data = response.GetData() }) { StatusCode = response.StatusCode };

            else
                return new StatusCodeResult(response.StatusCode);
        }

    }
}
