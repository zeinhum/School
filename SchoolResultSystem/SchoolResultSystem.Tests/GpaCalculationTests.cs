using Xunit;
using SchoolResultSystem.Web.Areas.Analytics.Models;
using System.Collections.Generic;
using System.Linq;

namespace SchoolResultSystem.Tests
{
    public class GpaCalculationTests
    {
        [Fact]
        public void CalculateAverageGpa_Returns_CorrectValue()
        {
            // Arrange
            var subjects = new List<ScodeGpa>
            {
                new() { gpa = new GPA { GradePoint = 4.0 } },
                new() { gpa = new GPA { GradePoint = 3.0 } },
                new() { gpa = new GPA { GradePoint = 2.0 } }
            };

            // Act
            double average = subjects.Average(s => s.gpa.GradePoint);

            // Assert
            Assert.Equal(3.0, average, 1);
        }

        [Fact]
        public void EmptySubjectList_Returns_ZeroGpa()
        {
            var subjects = new List<ScodeGpa>();
            double average = subjects.Any() ? subjects.Average(s => s.gpa.GradePoint) : 0.0;
            Assert.Equal(0.0, average);
        }
    }
}
