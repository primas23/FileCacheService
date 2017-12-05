using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using FCS.Contracts;
using FCS.Core;
using FCS.Models;

namespace FCS.Tests
{
    [TestClass]
    public class FileCacheTests
    {
        [TestMethod]
        public void Get_ShouldCallDelete_WhenTheFileIsExpired()
        {
            var dirLocation = Directory.GetCurrentDirectory();
            string filePath = dirLocation + "\\TempData\\delete.me";
            var dateTimeNow = DateTime.Now;

            var fileRead = new Mock<IFileRead>();
            var fileWrite = new Mock<IFileWrite>();
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            var josnService = new Mock<IJosnService>();
            var fileOperationsProvider = new Mock<IFileOperationsProvider>();
            var directoryOperationsProvider = new Mock<IDirectoryOperationsProvider>();

            directoryOperationsProvider
                .Setup(d => d.Exists(It.IsAny<string>()))
                .Returns(true);

            directoryOperationsProvider
               .Setup(d => d.GetCurrentDirectory())
               .Returns(dirLocation);

            fileOperationsProvider
                .Setup(f => f.Exists(It.IsAny<string>()))
                .Returns(true);

            fileOperationsProvider
                .Setup(f => f.GetCreationTime(It.IsAny<string>()))
                .Returns(dateTimeNow.AddSeconds(-10));

            dateTimeProvider
                .Setup(d => d.GetDateTimeNow())
                .Returns(dateTimeNow);

            ICacheService cacheService = new FileCache(fileRead.Object,
                fileWrite.Object,
                dateTimeProvider.Object,
                josnService.Object,
                fileOperationsProvider.Object,
                directoryOperationsProvider.Object);

            IEnumerable<JsonTestModel> posts = cacheService
                .Get(DateTime.Now.ToString("yyyy-dd-M"), () => this.GetMockedDataPosts(), 1);

            fileOperationsProvider.Verify(f => f.DeleteFile(It.IsAny<string>()));
        }

        [TestMethod]
        public void Get_ShouldNotCallRead_WhenTheFileIsNotExpired()
        {
            var dirLocation = Directory.GetCurrentDirectory();
            string filePath = dirLocation + "\\TempData\\delete.me";
            var dateTimeNow = DateTime.Now;

            var fileRead = new Mock<IFileRead>();
            var fileWrite = new Mock<IFileWrite>();
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            var josnService = new Mock<IJosnService>();
            var fileOperationsProvider = new Mock<IFileOperationsProvider>();
            var directoryOperationsProvider = new Mock<IDirectoryOperationsProvider>();

            directoryOperationsProvider
                .Setup(d => d.Exists(It.IsAny<string>()))
                .Returns(true);

            directoryOperationsProvider
               .Setup(d => d.GetCurrentDirectory())
               .Returns(dirLocation);

            fileOperationsProvider
                .Setup(f => f.Exists(It.IsAny<string>()))
                .Returns(true);

            fileOperationsProvider
                .Setup(f => f.GetCreationTime(It.IsAny<string>()))
                .Returns(dateTimeNow.AddSeconds(10));

            dateTimeProvider
                .Setup(d => d.GetDateTimeNow())
                .Returns(dateTimeNow);

            ICacheService cacheService = new FileCache(fileRead.Object,
                fileWrite.Object,
                dateTimeProvider.Object,
                josnService.Object,
                fileOperationsProvider.Object,
                directoryOperationsProvider.Object);

            IEnumerable<JsonTestModel> posts = cacheService
                .Get(DateTime.Now.ToString("yyyy-dd-M"), () => this.GetMockedDataPosts(), 1);

            fileRead.Verify(f => f.ReadFromFile(It.IsAny<string>()));
        }

        [TestMethod]
        public void Get_ShouldCallWriteToFile_WhenTheFileIsExpired()
        {
            var dirLocation = Directory.GetCurrentDirectory();
            string filePath = dirLocation + "\\TempData\\delete.me";
            var dateTimeNow = DateTime.Now;

            var fileRead = new Mock<IFileRead>();
            var fileWrite = new Mock<IFileWrite>();
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            var josnService = new Mock<IJosnService>();
            var fileOperationsProvider = new Mock<IFileOperationsProvider>();
            var directoryOperationsProvider = new Mock<IDirectoryOperationsProvider>();

            directoryOperationsProvider
                .Setup(d => d.Exists(It.IsAny<string>()))
                .Returns(true);

            directoryOperationsProvider
               .Setup(d => d.GetCurrentDirectory())
               .Returns(dirLocation);

            fileOperationsProvider
                .Setup(f => f.Exists(It.IsAny<string>()))
                .Returns(true);

            fileOperationsProvider
                .Setup(f => f.GetCreationTime(It.IsAny<string>()))
                .Returns(dateTimeNow.AddSeconds(-10));

            dateTimeProvider
                .Setup(d => d.GetDateTimeNow())
                .Returns(dateTimeNow);

            ICacheService cacheService = new FileCache(fileRead.Object,
                fileWrite.Object,
                dateTimeProvider.Object,
                josnService.Object,
                fileOperationsProvider.Object,
                directoryOperationsProvider.Object);

            IEnumerable<JsonTestModel> posts = cacheService
                .Get(DateTime.Now.ToString("yyyy-dd-M"), () => this.GetMockedDataPosts(), 1);

            fileWrite.Verify(f => f.WriteToFile(It.IsAny<string>(), It.IsAny<string>()));
        }

        [TestMethod]
        public void Get_ShouldCallWriteToFile_WhenTheFileDoesNotExist()
        {
            var dirLocation = Directory.GetCurrentDirectory();
            string filePath = dirLocation + "\\TempData\\delete.me";
            var dateTimeNow = DateTime.Now;

            var fileRead = new Mock<IFileRead>();
            var fileWrite = new Mock<IFileWrite>();
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            var josnService = new Mock<IJosnService>();
            var fileOperationsProvider = new Mock<IFileOperationsProvider>();
            var directoryOperationsProvider = new Mock<IDirectoryOperationsProvider>();

            directoryOperationsProvider
                .Setup(d => d.Exists(It.IsAny<string>()))
                .Returns(true);

            directoryOperationsProvider
               .Setup(d => d.GetCurrentDirectory())
               .Returns(dirLocation);

            fileOperationsProvider
                .Setup(f => f.Exists(It.IsAny<string>()))
                .Returns(false);

            ICacheService cacheService = new FileCache(fileRead.Object,
                fileWrite.Object,
                dateTimeProvider.Object,
                josnService.Object,
                fileOperationsProvider.Object,
                directoryOperationsProvider.Object);

            IEnumerable<JsonTestModel> posts = cacheService
                .Get(DateTime.Now.ToString("yyyy-dd-M"), () => this.GetMockedDataPosts(), 1);

            fileWrite.Verify(f => f.WriteToFile(It.IsAny<string>(), It.IsAny<string>()));
        }

        [TestMethod]
        public void Get_ShouldCallCreateDirectory_WhenTheDirectoryDoesNotExist()
        {
            var dirLocation = Directory.GetCurrentDirectory();
            string filePath = dirLocation + "\\TempData\\delete.me";
            var dateTimeNow = DateTime.Now;

            var fileRead = new Mock<IFileRead>();
            var fileWrite = new Mock<IFileWrite>();
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            var josnService = new Mock<IJosnService>();
            var fileOperationsProvider = new Mock<IFileOperationsProvider>();
            var directoryOperationsProvider = new Mock<IDirectoryOperationsProvider>();

            directoryOperationsProvider
                .Setup(d => d.Exists(It.IsAny<string>()))
                .Returns(false);

            directoryOperationsProvider
               .Setup(d => d.GetCurrentDirectory())
               .Returns(dirLocation);
            
            ICacheService cacheService = new FileCache(fileRead.Object,
                fileWrite.Object,
                dateTimeProvider.Object,
                josnService.Object,
                fileOperationsProvider.Object,
                directoryOperationsProvider.Object);

            IEnumerable<JsonTestModel> posts = cacheService
                .Get(DateTime.Now.ToString("yyyy-dd-M"), () => this.GetMockedDataPosts(), 1);

            directoryOperationsProvider.Verify(f => f.CreateDirectory(It.IsAny<string>()));
        }

        private IEnumerable<JsonTestModel> GetMockedDataPosts()
        {
            IEnumerable<JsonTestModel> mockedData = new List<JsonTestModel>()
            {
                new JsonTestModel
                {
                    Id = 1,
                    UserId = 1,
                    Title = "sunt aut facere repellat provident occaecati excepturi optio reprehenderit",
                    Body = "quia et suscipit\nsuscipit recusandae consequuntur expedita et cum\nreprehenderit molestiae ut ut quas totam\nnostrum rerum est autem sunt rem eveniet architecto"
                },
                new JsonTestModel
                {
                    Id = 2,
                    UserId = 2,
                    Title = "qui est esse",
                    Body = "est rerum tempore vitae\nsequi sint nihil reprehenderit dolor beatae ea dolores neque\nfugiat blanditiis voluptate porro vel nihil molestiae ut reiciendis\nqui aperiam non debitis possimus qui neque nisi nulla"
                }
            };

            return mockedData;
        }
    }
}
