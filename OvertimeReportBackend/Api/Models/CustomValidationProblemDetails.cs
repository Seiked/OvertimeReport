using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Core.Models.Response;

namespace Api.Models
{
    public class CustomValidationProblemDetails : Response<HttpContext>
    {
    }
}