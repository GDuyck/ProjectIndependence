using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;

namespace ProjectIndependence.API.Core.Response
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        [JsonPropertyName("error")]
        public object? Error { get; set; }
        public string? Message { get; set; } // Only use for delete

        public static ApiResponse<T> FromSucces(T data)
        {
            return new ApiResponse<T>
            {
                Data = data,
                Success = true
            };
        }

        public static ApiResponse<T> FromError(ProblemDetails details)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Error = details
            };
        }

        public static ApiResponse<T> FromValidationErrors(IDictionary<string, string[]> errors)
        {
            var modelState = new ModelStateDictionary();

            foreach (var pair in errors
                .SelectMany(kvp => (kvp.Value ?? Array.Empty<string>())
                .Select(err => (kvp.Key, Error: err))))
                {
                    modelState.AddModelError(pair.Key, pair.Error);
                }

            var details = new ValidationProblemDetails(modelState)
            {
                Title = "One or more problems have occured",
                Status = StatusCodes.Status400BadRequest
            };

            return new ApiResponse<T>
            {
                Success = false,
                Error = details
            };
        }

        public static ApiResponse<T> FromSuccesfullyDeleted(string message)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message
            };
        }

        public static ApiResponse<T> FromDomainError(string message)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Error = new { Domain = new[] { message} }
            };
        }
    }
}