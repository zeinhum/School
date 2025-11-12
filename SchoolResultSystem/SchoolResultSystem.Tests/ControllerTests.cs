using Xunit;
using Microsoft.AspNetCore.Mvc;
using SchoolResultSystem.Web.Areas.Analytics.Controllers;
using SchoolResultSystem.Web.Areas.Analytics.Models;
using SchoolResultSystem.Web.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace SchoolResultSystem.Tests
{
    public class ControllerTests
    {
        private readonly SchoolDbContext _context;

        public ControllerTests()
        {
            var options = new DbContextOptionsBuilder<SchoolDbContext>()
                .UseInMemoryDatabase("TestDB")
                .Options;

            _context = new SchoolDbContext(options);
        }

        [Fact]
        public  void Report()
        {
            // Arrange
            var controller = new ClassController(_context);
            var model = new Requet
            {
                NSN = "Class 11A"
            };

            // Act
            var result =  controller.Report(model) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }
    }
}
