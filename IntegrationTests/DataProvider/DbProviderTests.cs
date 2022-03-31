using DataProvider;
using Models.Request;
using System;
using System.Linq;
using Xunit;

namespace IntegrationTests.DataProvider
{
    public class DbProviderTests
    {
        private readonly DbProvider _dbProvider;

        public DbProviderTests()
        {
            // Setting the env variable for the integration tests. 
            // Created a separate database to not interfere with the main database.
            Environment.SetEnvironmentVariable("CONNECTION_STRING", "./TestDatabase.db");
            _dbProvider = new DbProvider();
        }

        [Fact]
        public void ShouldRetrieveTaskDetails()
        {
            // Act
            var response = _dbProvider.RetrieveTaskDetails();

            // Assert
            Assert.NotEmpty(response);
            Assert.NotEqual(0, response.First().Id);
            Assert.NotNull(response.First().Description);
        }

        [Fact]
        public void ShouldStoreTaskDetails()
        {
            // Arrange
            Task task = new Task
            {
                Id = 1,
                Description = "Test Description",
                LastUpdatedDate = DateTime.Now
            };

            // Act
            var response = _dbProvider.StoreTaskDetails(task);

            // Assert
            Assert.NotEqual(0, response);
        }

        [Fact]
        public void ShouldDeleteTaskDetails()
        {
            // Arrange
            int id = 5;

            // Act
            var response = _dbProvider.DeleteTaskDetails(id);

            // Assert
            Assert.True(response >= 0);
        }
    }
}
