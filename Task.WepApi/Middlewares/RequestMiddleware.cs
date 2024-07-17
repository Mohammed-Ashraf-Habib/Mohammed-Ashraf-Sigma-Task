using Microsoft.AspNetCore.Http;
using System.Linq;
using System;
using System.Threading.Tasks;
using Serilog.Events;
using System.Buffers;
using System.IO;

using System.IO.Pipelines;
using Task.Business.Logger;

namespace Task.WepApi.Middlewares
{
    public class RequestMiddleware
    {
        #region Data Memners
        private readonly RequestDelegate _next;
        private readonly ILoggerService _logger;
        private readonly string _ExtraOutService = "RequestViewModel Id : {@requestId}\nRequest Method : {@requestType}\n URI : {@requestSigniture}\n request Query Parameters : {@requestQueryParameters}\n request Form Parameters : {@requestFormParametersString}\n Body : {@Body}\n Requester Ip Address : {@RequesterIpAddress}\n Response : {@Response}\n Authorization : {@Authorization}\n Response TimeStamp : {@ResponseTimeStamp}\n RequestViewModel TimeStamp : {@RequestTimeStamp}\n RequestViewModel Duration : {@RequestDuration:000} ms";
        #endregion

        #region Constructors
        public RequestMiddleware(RequestDelegate next, ILoggerService logger)
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
        public  async Task<string> GetResponseBody(MemoryStream memStream)
        {
            memStream.Position = 0;
            string responseBody = new StreamReader(memStream).ReadToEnd();

            memStream.Position = 0;
    

            return responseBody;
        }
        public async System.Threading.Tasks.Task InvokeAsync(HttpContext httpContext)
        {
            var originalBody = httpContext.Response.Body;

            MemoryStream memStream = new MemoryStream();
            httpContext.Response.Body = memStream;
            var watch = new System.Diagnostics.Stopwatch();

            var RequestTimeStamp = DateTime.Now;
            var ResponseTimeStamp = DateTime.Now;
            long RequestDuration = 0;
            try
            {
                watch.Start();

                await _next(httpContext);
                ResponseTimeStamp = DateTime.Now;
            }
            catch (Exception ex)
            {
                watch.Stop();

                ResponseTimeStamp = DateTime.Now;
                RequestDuration = watch.ElapsedMilliseconds;
                string requestSigniture = httpContext.Request.Path.ToString();
                var requestType = httpContext.Request.Method;
                var requestQueryParameters = httpContext.Request.QueryString.Value;
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
                requestQueryParameters = string.IsNullOrEmpty(requestQueryParameters) ? "N/A" : requestQueryParameters;
                var requestId = httpContext.TraceIdentifier;
                var RequesterIpAddress = httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

                var Body = await GetRequestBody(httpContext);
                Body = string.IsNullOrEmpty(Body) ? "N/A" : Body;
                var Authorization = httpContext.Request.Headers["Authorization"].FirstOrDefault();
                Authorization = string.IsNullOrEmpty(Authorization) ? "N/A" : Authorization;

                var Response = await GetResponseBody(memStream);
                Response = string.IsNullOrEmpty(Response) ? "N/A" : Response;
                memStream.Position = 0;

                await memStream.CopyToAsync(originalBody);

                httpContext.Response.Body = originalBody;
               
                _logger.LogInfo(_ExtraOutService, requestId,requestType, requestSigniture, requestQueryParameters, requestFormParametersString, Body, RequesterIpAddress, Response, Authorization, ResponseTimeStamp, RequestTimeStamp, RequestDuration);
                throw;
            }
            finally
            {
                watch.Stop();

                RequestDuration = watch.ElapsedMilliseconds;

                string requestSigniture = httpContext.Request.Path.ToString();
                var requestType = httpContext.Request.Method;
                var requestQueryParameters = httpContext.Request.QueryString.Value;
                var contantType =string.IsNullOrEmpty( httpContext.Request.ContentType)? false: httpContext.Request.ContentType.ToLower().Contains("form");
                var requestFormParameters = contantType? httpContext.Request.Form:null;
                var requestFormParametersString = string.Empty;
                if (contantType)
                {
                    
                foreach (var item in requestFormParameters)
                {
                    requestFormParametersString += item.Key + " = " + item.Value + " & ";
                }
                }
                requestQueryParameters = string.IsNullOrEmpty(requestQueryParameters) ? "N/A" : requestQueryParameters;
                var RequesterIpAddress = httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                var requestId = httpContext.TraceIdentifier;

                var Body = await GetRequestBody(httpContext);
                Body = string.IsNullOrEmpty(Body) ? "N/A" : Body;
                var Authorization = httpContext.Request.Headers["Authorization"].FirstOrDefault();
                var Response = await GetResponseBody(memStream);
                Response = string.IsNullOrEmpty(Response) ? "N/A" : Response;
                memStream.Position = 0;

                await memStream.CopyToAsync(originalBody);

                httpContext.Response.Body = originalBody;
                _logger.LogInfo(_ExtraOutService, requestId, requestType, requestSigniture, requestQueryParameters, requestFormParametersString, Body, RequesterIpAddress, Response, Authorization, ResponseTimeStamp, RequestTimeStamp, RequestDuration);

            }
           
           
        }
    }
}
