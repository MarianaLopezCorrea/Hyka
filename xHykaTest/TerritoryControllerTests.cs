using Xunit;
using Moq;
using System;
using Hyka.Data;
using Hyka.Service.Controllers;
using Microsoft.AspNetCore.Mvc;
using Hyka.Models;
using Microsoft.EntityFrameworkCore;

namespace xHykaTest;

public class TerritoryControllerTests
{
    public TerritoryControllerTests()
    {

    }
    [Fact]
    public void GetTerritories_WithUnexistingTerritories_ReturnsNotFound()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDB")
            .Options;
        var _dbContext = new ApplicationDbContext(options);
        var controller = new TerritoryController(_dbContext);

        //Act
        var result = controller.GetById(Guid.NewGuid().ToString());

        //Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void GetTerritories_WithExistingTerritories_ReturnsOk()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDB")
            .Options;
        var _dbContext = new ApplicationDbContext(options);
        var controller = new TerritoryController(_dbContext);

        //Act
        var result = controller.Get();

        //Assert
        Assert.IsType<OkObjectResult>(result);

    }
}