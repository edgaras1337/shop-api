using api.Controllers;
using api.Dtos.PurchaseControllerDtos;
using api.Dtos.UserControllerDtos;
using api.Services;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.Tests
{
    public class PurchaseControllerTests
    {
        private readonly IPurchaseService _purchaseService;
        public PurchaseControllerTests()
        {
            _purchaseService = A.Fake<IPurchaseService>();
        }

        [Fact]
        public async Task CreatePurchase()
        {
            // Arrange
            var purchaseDto = A.Dummy<CreatePurchaseRequest>();
            A.CallTo(() => _purchaseService.CreatePurchaseAsync(purchaseDto))
                .Returns(Task.FromResult(A.Dummy<CreatePurchaseResponse>()));
            var controller = new PurchaseController(_purchaseService);

            // Act
            var actionResult = await controller.CreatePurchase(purchaseDto);

            // Assert
            Assert.True(actionResult.Result is OkObjectResult);
        }

        [Fact]
        public async Task GetCurrentUserHistory()
        {
            // Arrange
            A.CallTo(() => _purchaseService.GetCurrentUserPurchaseHistoryAsync())
                .Returns(Task.FromResult(A.CollectionOfDummy<GetCurrentUserPurchaseHistory>(5).ToList()));
            var controller = new PurchaseController(_purchaseService);

            // Act
            var actionResult = await controller.GetCurrentUserPurchaseHistory();

            // Assert
            Assert.True(actionResult.Result is OkObjectResult);
        }

        [Fact]
        public async Task GetUserByIdHistory()
        {
            // Arrange
            var id = 1;
            A.CallTo(() => _purchaseService.GetUserPurchaseHistoryAsync(id))
                .Returns(Task.FromResult(A.CollectionOfDummy<GetUserPurchaseHistory>(5).ToList()));
            var controller = new PurchaseController(_purchaseService);

            // Act
            var actionResult = await controller.GetUserPurchaseHistory(id);

            // Assert
            Assert.True(actionResult.Result is OkObjectResult);
        }

        [Fact]
        public async Task GetAllHistory()
        {
            // Arrange
            A.CallTo(() => _purchaseService.GetAllPurchaseHistoryAsync())
                .Returns(Task.FromResult(A.CollectionOfDummy<GetAllPurchaseHistoryResponse>(5).ToList()));
            var controller = new PurchaseController(_purchaseService);

            // Act
            var actionResult = await controller.GetAllPurchaseHistory();

            // Assert
            Assert.True(actionResult.Result is OkObjectResult);
        }
    }
}
