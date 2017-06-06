using System;
using System.Collections.Generic;
using Edorator.Services.Handlers;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Security.Principal;
using System.Threading.Tasks;
using Edorator.Services.Contracts;
using Edorator.Services.Data;
using Edorator.Services.Models;
using Microsoft.Extensions.Options;

namespace Edorator.Services.Tests.Handlers
{
    [TestClass]
    public class AddServiceHandlerTests
    {
        private MockRepository _mockRepository;

        private Mock<ILoggerFactory> _mockLoggerFactory;
        private Mock<IPrincipal> _mockPrincipal;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new MockRepository(MockBehavior.Default);

            _mockLoggerFactory = _mockRepository.Create<ILoggerFactory>();
            _mockPrincipal = _mockRepository.Create<IPrincipal>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this._mockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task AddServiceEmpty()
        {
            AddServiceHandler addServiceHandler = CreateAddServiceHandler();

            AddServiceRequest request = new AddServiceRequest();
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => addServiceHandler.Handle(request));
        }

        private AddServiceHandler CreateAddServiceHandler()
        {
            return new AddServiceHandler(
                _mockLoggerFactory.Object,
                _mockPrincipal.Object, new ServiceRepository(new OptionsManager<Settings>(new List<IConfigureOptions<Settings>>())));
        }
    }
}