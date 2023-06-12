using ERP.Core.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Core.Controllers
{
    public abstract class MasterController : ControllerBase
    {
        protected IActionResult ParseException(Exception ex, string message = "")
        {
            if (string.IsNullOrEmpty(message))
                message = $"Error performing operation. Report ErrorCode to support";

            var errorObject = ReturnExceptionModel.Get(message);

            LogHelper.RiseError(ex, errorObject.ErrorCode);

            return StatusCode(StatusCodes.Status500InternalServerError, errorObject);
        }

        protected IActionResult ParseBadRequest(string message)
        {
            var errorObject = ReturnExceptionModel.Get(message);

            errorObject.ErrorCode = StatusCodes.Status400BadRequest.ToString();

            return StatusCode(StatusCodes.Status400BadRequest, errorObject);
        }
    }

    public class ReturnExceptionModel
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime DateTime { get; set; }

        private ReturnExceptionModel()
        {
            Guid guid = Guid.NewGuid();
            ErrorCode = guid.ToString();
            ErrorMessage = "";
        }

        public static ReturnExceptionModel Get(string errorMessage)
        {
            return new ReturnExceptionModel
            {
                DateTime = DateTime.UtcNow,
                ErrorMessage = errorMessage,
            };
        }
    }
}