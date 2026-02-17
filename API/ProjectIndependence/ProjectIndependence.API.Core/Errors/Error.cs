using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectIndependence.API.Core.Errors
{
    public sealed record Error(string Code, string Message);
}
