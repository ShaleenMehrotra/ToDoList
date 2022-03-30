using DataProvider;
using Microsoft.Data.Sqlite;
using System;
using Xunit;

namespace IntegrationTests.DataProvider
{
    public class DbProviderTests
    {
        private readonly DbProvider _dbProvider;

        public DbProviderTests()
        {
            _dbProvider = new DbProvider();
        }

        //[Fact]
        public void ShouldRetrieveTaskDetails()
        {
            // Arrange

            // Act
            var response = _dbProvider.RetrieveTaskDetails();

            // Assert
            Assert.NotNull(response);
        }
    }
}
