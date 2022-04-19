using Xunit;
using Moq;
using System;
using Hyka.Data;
using Hyka.Service.Controllers;
using Microsoft.AspNetCore.Mvc;
using Hyka.Models;
using Microsoft.EntityFrameworkCore;

namespace xHykaTest;

public class PersonControllerTest
{
    public PersonControllerTest()
    {

    }
    [Fact]
    public void GetPersons_WithExisting_ReturnsOk()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDB")
            .Options;
        var _dbContext = new ApplicationDbContext(options);
        var controller = new PersonController(_dbContext);

        //Act
        var result = controller.Get();

        //Assert
        Assert.IsType<OkObjectResult>(result);

    }
}