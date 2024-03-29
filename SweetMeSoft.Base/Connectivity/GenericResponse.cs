﻿using System.Net;

namespace SweetMeSoft.Base.Connectivity
{
    public class GenericResponse<T>
    {
        public HttpResponseMessage HttpResponse { get; set; }

        public CookieContainer CookieContainer { get; set; }

        public T Object { get; set; }

        public ErrorDetails Error { get; set; }
    }
}
