using DeepInsights.Components.HistoricalPrices.ViewModels;
using DeepInsights.Services;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Components.HistoricalPrices.UnitTests
{
    [TestClass]
    public class HistoricalPricesMainViewModelTest
    {
        [TestMethod]
        public void HistoricalPricesMainViewModel_ConstructsCorrectly()
        {
            // Arrange
            var forexHistoricalPricesService = new Mock<IForexHistoricalPricesService>();
            var eventAggregator = new Mock<IEventAggregator>();
            //var instrumentChangedEvent = new Mock<InstrumentChangedEvent>();
            //eventAggregator.Setup(ea => ea.GetEvent<InstrumentChangedEvent>()).Returns(instrumentChangedEvent.Object);
            //Action<string> callback = null;
            //instrumentChangedEvent.Setup(p => p.Subscribe(
            //        It.IsAny<Action<string>>(),
            //        It.IsAny<ThreadOption>(),
            //        It.IsAny<bool>(),
            //        It.IsAny<Predicate<string>>()))
            //        .Callback<Action<string>, ThreadOption, bool, Predicate<string>>(
            //        (e, t, b, a) => callback = e);

            var target = new HistoricalPricesMainViewModel(forexHistoricalPricesService.Object, eventAggregator.Object);

            // Assert
            Assert.IsNotNull(target);
            Assert.IsNotNull(target.Candles);
            Assert.IsNotNull(target.YAxisLabel);
            Assert.IsNotNull(target.XAxisLabel);

            //callback.Invoke("USD/GBP");
        }
    }
}
