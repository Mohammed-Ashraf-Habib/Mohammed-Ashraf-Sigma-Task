
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Task.Business.Logger;
using Task.WepApi.Models;


namespace Task.WepApi.Middlewares
{
	public class ExceptionMiddleware
	{
		#region Data Memners
		private readonly RequestDelegate _next;
		private readonly ILoggerService _logger;
        private readonly string _ExtraOutService = "RequestViewModel Id : {@requestId}\nRequest Method : {@requestType}\n URI : {@errorSigniture}\n request Query Parameters : {@requestQueryParameters}\n request Form Parameters : {@requestFormParametersString}\n Body : {@Body}\n Requester Ip Address : {@RequesterIpAddress}\n Authorization : {@Authorization}";
        #endregion

        #region Constructors
        public ExceptionMiddleware(RequestDelegate next, ILoggerService logger)
		{
			_logger = logger;
			_next = next;
		}
        #endregion
        public static async Task<string> GetRequestBody(HttpContext httpContext)
        {

            var reader = new StreamReader(httpContext.Request.Body);
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            var rawMessage = await reader.ReadToEndAsync();
            reader.BaseStream.Seek(0, SeekOrigin.Begin);

            return rawMessage;
        }
        public async System.Threading.Tasks.Task InvokeAsync(HttpContext httpContext)
		{
            httpContext.Request.EnableBuffering();

            try
            {
				await _next(httpContext);
			}
			catch (Exception ex)
			{
                string errorSigniture = httpContext.Request.Path.ToString();
                var requestType = httpContext.Request.Method;
                var requestQueryParameters = httpContext.Request.QueryString.Value;
                var RequesterIpAddress = httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                var requestId = httpContext.TraceIdentifier;
                var contantType = string.IsNullOrEmpty(httpContext.Request.ContentType) ? false : httpContext.Request.ContentType.ToLower().Contains("form");
                var requestFormParameters = contantType ? httpContext.Request.Form : null;
                var requestFormParametersString = string.Empty;
                if (contantType)
                {

                    foreach (var item in requestFormParameters)
                    {
                        requestFormParametersString += item.Key + " = " + item.Value + " & ";
                    }
                }
                var Body = await GetRequestBody(httpContext);
                Body = string.IsNullOrEmpty(Body) ? "N/A" : Body;
                var Authorization = httpContext.Request.Headers["Authorization"].FirstOrDefault();
                
                _logger.LogError(_ExtraOutService, ex, requestId,requestType, errorSigniture, requestQueryParameters, requestFormParametersString, Body, RequesterIpAddress, Authorization);

                await HandleExceptionAsync(httpContext, ex);
			}
		}

		private System.Threading.Tasks.Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;



            return context.Response.WriteAsync(new ExceptionModel()
				{
					StatusCode = context.Response.StatusCode,
					Message = "Internal Server Error from the custom middleware.",
                    ErrorCode = 500,
                    RequestTime = DateTime.Now,

				}.ToString());
			

		}

	}
}
