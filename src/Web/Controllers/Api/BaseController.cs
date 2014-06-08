using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace Solaise.Weather.Web.Controllers.Api
{
    public class BaseController : ApiController
    {
        protected HttpResponseMessage Get<T>(Func<T> action, string errorMessage = null) where T : class
        {
            try
            {
                var result = action();
                if (result == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Resource not found");
                }

                var response = Request.CreateResponse(HttpStatusCode.OK, result);
                response.Headers.CacheControl = new CacheControlHeaderValue { Public = true, MaxAge = TimeSpan.FromDays(1) };
                return response;
            }
            catch (Exception exc)
            {
                throw new ApplicationException(errorMessage ?? exc.Message, exc);
            }
        }

        protected HttpResponseMessage Post(Action action, string errorMessage = null)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, GetErrors(ModelState));
            }
            try
            {
                action();
                return new HttpResponseMessage(HttpStatusCode.Created);
            }
            catch (Exception exc)
            {
                throw new ApplicationException(errorMessage ?? exc.Message, exc);
            }
        }

        protected HttpResponseMessage Delete(Action action, string errorMessage = null)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, GetErrors(ModelState));
            }
            try
            {
                action();
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception exc)
            {
                throw new ApplicationException(errorMessage ?? exc.Message, exc);
            }
        }

        static string GetErrors(IDictionary<string, ModelState> modalState)
        {
            var errorsSb = new StringBuilder();
            var errors = modalState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage)).ToList();
            errors.ForEach(x => errorsSb.AppendLine(x));
            return errorsSb.ToString();
        }
    }
}