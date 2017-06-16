using DeepInsights.Components.Account.ViewModels;
using DeepInsights.Services.ForexServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Net;
using System.Net.Http;

namespace DeepInsights.Components.Account.UnitTests
{
    [TestClass]
    public class AccountMainViewModelTest
    {
        [TestMethod]
        public void AccountMainViewModel_ConstructsCorrectly()
        {
            // Arrange
            var forexAccountService = new Mock<IForexAccountService>();

            // Act
            var target = new AccountMainViewModel(forexAccountService.Object);

            // Assert
            Assert.IsNotNull(target);
            Assert.IsNotNull(target.ViewLoadedCommand);
            Assert.IsNotNull(target.AccountKeyValuePairs);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AccountMainViewModel_ThrowExceptionOnNullForexAccountService()
        {
            // Arrange
            IForexAccountService forexAccountService = null;

            // Act
            var target = new AccountMainViewModel(forexAccountService);

            // Assert
            Assert.Fail();
        }

        [TestMethod]
        public void AccountMainViewModel_ViewLoaded_LoadsAccountData()
        {
            // Arrange
            var forexAccountService = new Mock<IForexAccountService>();
            var accountJson = "{\"account\":{\"id\":\"101-003-4355710-001\",\"createdTime\":\"2016-09-25T09:22:18.174902058Z\",\"currency\":\"SGD\",\"createdByUserID\":4355710,\"alias\":\"Primary\",\"marginRate\":\"0.02\",\"hedgingEnabled\":false,\"lastTransactionID\":\"9\",\"balance\":\"100000.0000\",\"openTradeCount\":0,\"openPositionCount\":0,\"pendingOrderCount\":0,\"pl\":\"0.0000\",\"resettablePL\":\"0.0000\",\"financing\":\"0\",\"commission\":\"0\",\"orders\":[],\"positions\":[],\"trades\":[],\"unrealizedPL\":\"0.0000\",\"NAV\":\"100000.0000\",\"marginUsed\":\"0.0000\",\"marginAvailable\":\"100000.0000\",\"positionValue\":\"0.0000\",\"marginCloseoutUnrealizedPL\":\"0.0000\",\"marginCloseoutNAV\":\"100000.0000\",\"marginCloseoutMarginUsed\":\"0.0000\",\"marginCloseoutPositionValue\":\"0.0000\",\"marginCloseoutPercent\":\"0.00000\",\"withdrawalLimit\":\"100000.0000\",\"marginCallMarginUsed\":\"0.0000\",\"marginCallPercent\":\"0.00000\"},\"lastTransactionID\":\"9\"}";
            forexAccountService.Setup(s => s.GetAccountData()).ReturnsAsync(accountJson);
            var target = new AccountMainViewModel(forexAccountService.Object);

            // Act
            target.ViewLoadedCommand.Execute(null);

            // Assert
            forexAccountService.Verify(s => s.GetAccountData(), Times.Once);
            Assert.IsTrue(target.AccountKeyValuePairs.Count > 0);
            Assert.IsTrue(target.ModuleStatus.IsLoaded);
        }

        [TestMethod]
        public void AccountMainViewModel_ViewLoaded_SetModuleStatusErrorIfAccountLoadFails()
        {
            // Arrange
            var forexAccountService = new Mock<IForexAccountService>();
            forexAccountService.Setup(s => s.GetAccountData()).ThrowsAsync(new HttpRequestException("Bad Request"));
            var target = new AccountMainViewModel(forexAccountService.Object);

            // Act
            target.ViewLoadedCommand.Execute(null);

            // Assert
            Assert.IsTrue(target.ModuleStatus.HasErrors);
            forexAccountService.Verify(s => s.GetAccountData(), Times.Once);
            Assert.AreEqual(target.AccountKeyValuePairs.Count, 0);
            Assert.IsFalse(target.ModuleStatus.IsLoaded);
        }
    }
}
