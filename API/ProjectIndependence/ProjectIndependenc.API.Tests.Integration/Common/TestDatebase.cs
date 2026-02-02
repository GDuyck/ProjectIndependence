using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIndependence.API.Tests.Integration.Common
{
    public static class TestDatebase
    {
        public static string CreateConnectionString(string baseConnectionString)
        {
            var databaseName = $"TestDb_{Guid.NewGuid():N}";
            return $"{baseConnectionString};Database={databaseName}";
        }
    }
}
