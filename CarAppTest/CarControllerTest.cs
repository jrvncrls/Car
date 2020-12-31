using CarApp.Controllers;
using CarApp.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace CarAppTest
{
    [TestClass]
    public class CarControllerTest
    {
        private List<Car> cars;

        [TestInitialize]
        public void Setup()
        {
            cars = new List<Car>()
            {
                new Car
                {
                    Id = 1,
                    Name = "Car1",
                    Color = "Blue",
                    YearMade = new DateTime(2018-01-01)
                },
                new Car
                {
                    Id = 2,
                    Name = "Car2",
                    Color = "Green",
                    YearMade = new DateTime(2018-01-01)
                },
                new Car
                {
                    Id = 3,
                    Name = "Car3",
                    Color = "Red",
                    YearMade = new DateTime(2019-01-01)
                },
            };
        }

        [TestMethod]
        public void GetCarByYear_HasNoMatch_ReturnsEmpty()
        {
            // Arrange
            var mockContext = new Mock<CarAppEntities>();

            // Populate Car "table"
            var carDbSet = cars.GetQueryableMockDbSet();
            foreach (var car in cars)
            {
                carDbSet.Add(car);
            }

            mockContext.Setup(context => context.Cars).Returns(carDbSet);
            var dbContext = mockContext.Object;

            // Action
            var controller = new CarController(dbContext);
            var result = controller.GetCarByYear(2020) as ViewResult;

            var carData = (IEnumerable<Car>)result.Model;

            //Assert
            Assert.IsFalse(carData.Count() > 0);
        }

        [TestMethod]
        public void GetCarByYear_HasMatch_ReturnsAtLeastOne()
        {
            // Arrange
            var mockContext = new Mock<CarAppEntities>();

            // Populate Car "table"
            var carDbSet = cars.GetQueryableMockDbSet();
            foreach (var car in cars)
            {
                carDbSet.Add(car);
            }

            mockContext.Setup(context => context.Cars).Returns(carDbSet);
            var dbContext = mockContext.Object;

            // Action
            var controller = new CarController(dbContext);
            var result = controller.GetCarByYear(2019) as ViewResult;

            var carData = (IEnumerable<Car>)result.Model;

            //Assert
            Assert.IsTrue(carData.Count() > 0);
            Assert.IsTrue(carData.Count() == 1);
        }

        [TestMethod]
        public void GetCarByYear_HasMatchMoreThanOne_ReturnsMoreThanOne()
        {
            // Arrange
            var mockContext = new Mock<CarAppEntities>();

            // Populate Car "table"
            var carDbSet = cars.GetQueryableMockDbSet();
            foreach (var car in cars)
            {
                carDbSet.Add(car);
            }

            mockContext.Setup(context => context.Cars).Returns(carDbSet);
            var dbContext = mockContext.Object;

            // Action
            var controller = new CarController(dbContext);
            var result = controller.GetCarByYear(2018) as ViewResult;

            var carData = (IEnumerable<Car>)result.Model;

            //Assert
            Assert.IsTrue(carData.Count() > 0);
            Assert.IsTrue(carData.Count() > 1);
        }
    }

    public static class DbSetExtensions
    {
        public static DbSet<T> GetQueryableMockDbSet<T>(this IEnumerable<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            return dbSet.Object;
        }
    }
}