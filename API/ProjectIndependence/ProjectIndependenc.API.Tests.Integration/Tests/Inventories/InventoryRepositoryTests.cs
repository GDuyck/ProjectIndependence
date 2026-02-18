using ProjectIndependence.API.Tests.Integration.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ProjectIndependence.API.Tests.Integration.Tests.Inventories
{
    [Collection("IntegrationTests")]
    public class InventoryRepositoryTests(SqlServerContainerFixture sqlServerContainerFixture) : IntegrationTestBase(sqlServerContainerFixture)
    {
    }
}
