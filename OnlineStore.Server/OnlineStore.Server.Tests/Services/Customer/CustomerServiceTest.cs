using Moq;
using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Customers;
using OnlineStore.Server.Repositories.Customers;
using OnlineStore.Server.Services.Customers;

namespace OnlineStore.Server.Tests.Services.Customer
{
    [Collection("CustomerServiceCollection")]
    public class CustomerServiceTest : IClassFixture<CustomerServiceFixture>
    {
        private readonly CustomerServiceFixture _fixture;
        private readonly Mock<ICustomerRepository> _mockRepository;

        public CustomerServiceTest(CustomerServiceFixture fixture)
        {
            _fixture = fixture;
            _mockRepository = _fixture.CreateMockRepository();
        }

        [Fact]
        public async Task GetOneByCriteria_Success()
        {
            //Arrange
            var service = new CustomerService(_mockRepository.Object);

            //Act
            var test_success_1 = await service.GetOneByCriteria(new() { Id = _fixture.CustomerId_Exists });
            var test_success_2 = await service.GetOneByCriteria(new() { Code = _fixture.CustomerCode_Exists });

            //Assert
            Assert.NotNull(test_success_1);
            Assert.Equal(_fixture.CustomerId_Exists, test_success_1.Id);

            Assert.NotNull(test_success_2);
            Assert.Equal(_fixture.CustomerCode_Exists, test_success_2.Code);
        }

        [Fact]
        public async Task GetOneByCriteria_Fail()
        {
            //Arrange
            var service = new CustomerService(_mockRepository.Object);

            var criteria_fail_1 = new CustomerFilterCriteria { Id = _fixture.Guid_Unexists };
            var criteria_fail_2 = new CustomerFilterCriteria { Id = Guid.Empty };
            var criteria_fail_3 = new CustomerFilterCriteria { Id = null };
            var criteria_fail_4 = new CustomerFilterCriteria { Code = "" };
            var criteria_fail_5 = new CustomerFilterCriteria { Code = "         " };
            var criteria_fail_6 = new CustomerFilterCriteria { Code = "1111-9999" };
            var criteria_fail_7 = new CustomerFilterCriteria { Code = "sdfhsdfhasdfh" };
            var criteria_fail_8 = new CustomerFilterCriteria { Code = "123452000" };

            //Act
            var test_fail_1 = await service.GetOneByCriteria(criteria_fail_1);
            var test_fail_2 = await service.GetOneByCriteria(criteria_fail_2);
            var test_fail_3 = await service.GetOneByCriteria(criteria_fail_3);
            var test_fail_4 = await service.GetOneByCriteria(criteria_fail_4);
            var test_fail_5 = await service.GetOneByCriteria(criteria_fail_5);
            var test_fail_6 = await service.GetOneByCriteria(criteria_fail_6);
            var test_fail_7 = await service.GetOneByCriteria(criteria_fail_7);
            var test_fail_8 = await service.GetOneByCriteria(criteria_fail_8);

            //Assert
            Assert.Null(test_fail_1);
            Assert.Null(test_fail_2);
            Assert.Null(test_fail_3);
            Assert.Null(test_fail_4);
            Assert.Null(test_fail_5);
            Assert.Null(test_fail_6);
            Assert.Null(test_fail_7);
            Assert.Null(test_fail_8);
        }

        [Fact]
        public async Task Update_Success()
        {
            //Arrange
            var service = new CustomerService(_mockRepository.Object);

            //Act
            var success_1 = await service.Update(_fixture.CustomerId_Exists, new CustomerRequest { Name = _fixture.CustomerName_Exists, Code = _fixture.CustomerCode_Exists });

            //Assert
            Assert.True(success_1);
        }

        [Fact]
        public async Task Update_Fail()
        {
            //Arrange
            var service = new CustomerService(_mockRepository.Object);

            var request_fail_1 = new CustomerRequest { Name = "", Code = _fixture.CustomerCode_Exists };
            var request_fail_2 = new CustomerRequest { Name = "    x  ", Code = _fixture.CustomerCode_Exists };
            var request_fail_3 = new CustomerRequest { Name = string.Empty, Code = _fixture.CustomerCode_Exists };
            var request_fail_4 = new CustomerRequest { Name = _fixture.CustomerName_Exists, Code = "1111-9999" };
            var request_fail_5 = new CustomerRequest { Name = _fixture.CustomerName_Exists, Code = "1111-0000" };
            var request_fail_6 = new CustomerRequest { Name = _fixture.CustomerName_Exists, Code = "1911-200" };
            var request_fail_7 = new CustomerRequest { Name = _fixture.CustomerName_Exists, Code = "123402000" };
            var request_fail_8 = new CustomerRequest { Name = _fixture.CustomerName_Exists, Code = "    -    " };
            var request_fail_9 = new CustomerRequest { Name = _fixture.CustomerCode_Exists, Code = _fixture.CustomerName_Exists };
            var request_fail_10 = new CustomerRequest { Name = _fixture.CustomerName_Exists, Code = _fixture.CustomerCode_Exists, Address = "lalala", Discount = -1 };
            var request_fail_11 = new CustomerRequest { Name = _fixture.CustomerName_Exists, Code = _fixture.CustomerCode_Exists, Address = null, Discount = 1000 };
            var request_fake_success = new CustomerRequest { Name = _fixture.CustomerName_Exists, Code = _fixture.CustomerCode_Exists };

            //Act
            var test_fail_1 = await service.Update(_fixture.CustomerId_Exists, request_fail_1);
            var test_fail_2 = await service.Update(_fixture.CustomerId_Exists, request_fail_2);
            var test_fail_3 = await service.Update(_fixture.CustomerId_Exists, request_fail_3);
            var test_fail_4 = await service.Update(_fixture.CustomerId_Exists, request_fail_4);
            var test_fail_5 = await service.Update(_fixture.CustomerId_Exists, request_fail_5);
            var test_fail_6 = await service.Update(_fixture.CustomerId_Exists, request_fail_6);
            var test_fail_7 = await service.Update(_fixture.CustomerId_Exists, request_fail_7);
            var test_fail_8 = await service.Update(_fixture.CustomerId_Exists, request_fail_8);
            var test_fail_9 = await service.Update(_fixture.CustomerId_Exists, request_fail_9);
            var test_fail_10 = await service.Update(_fixture.CustomerId_Exists, request_fail_10);
            var test_fail_11 = await service.Update(_fixture.CustomerId_Exists, request_fail_11);
            var test_fail_12 = await service.Update(_fixture.Guid_Unexists, request_fake_success);

            //Assert
            Assert.False(test_fail_1);
            Assert.False(test_fail_2);
            Assert.False(test_fail_3);
            Assert.False(test_fail_4);
            Assert.False(test_fail_5);
            Assert.False(test_fail_6);
            Assert.False(test_fail_7);
            Assert.False(test_fail_8);
            Assert.False(test_fail_9);
            Assert.False(test_fail_10);
            Assert.False(test_fail_11);
            Assert.False(test_fail_12);
        }

        [Fact]
        public async Task GetPage_Success()
        {
            //Arrange
            var service = new CustomerService(_mockRepository.Object);
            
            var pages1 = new PageInfo(1, 12);
            var pages2 = new PageInfo(2, 3);
            var pages3 = new PageInfo(1, 20);

            //Act
            var success_1 = await service.GetPage(pages1); //yes, yes
            var success_2 = await service.GetPage(pages2); //yes, yes
            var success_3 = await service.GetPage(pages3); //yes, yes

            //Assert
            Assert.NotNull(success_1);
            Assert.NotNull(success_2);
            Assert.NotNull(success_3);

            Assert.Equal(20, success_1.TotalCount);
            Assert.Equal(20, success_2.TotalCount);
            Assert.Equal(20, success_3.TotalCount);

            Assert.Equal(12, success_1.Responses.Count());
            Assert.Equal(3, success_2.Responses.Count());
            Assert.Equal(20, success_3.Responses.Count());
        }

        [Fact]
        public async Task GetPage_Fail()
        {
            //Arrange
            var service = new CustomerService(_mockRepository.Object);

            var pages1 = new PageInfo(0, 0);
            var pages2 = new PageInfo(-1, 1);
            var pages3 = new PageInfo(1, -1);
            var pages4 = new PageInfo(1000, 1000);
            var pages5 = new PageInfo(-1000, -1000);
            var pages6 = new PageInfo(1, 1000);

            //Act
            var fail_1 = await service.GetPage(pages1); //no, no
            var fail_2 = await service.GetPage(pages2); //no, yes
            var fail_3 = await service.GetPage(pages3);  //yes, no
            var fail_4 = await service.GetPage(pages4); //yes, no
            var fail_5 = await service.GetPage(pages5); //no, no
            var fail_6 = await service.GetPage(pages6); //yes, no

            //Assert
            Assert.NotNull(fail_1);
            Assert.NotNull(fail_2);
            Assert.NotNull(fail_3);
            Assert.NotNull(fail_4);
            Assert.NotNull(fail_5);
            Assert.NotNull(fail_6);

            Assert.Equal(0, fail_1.TotalCount);
            Assert.Equal(0, fail_2.TotalCount);
            Assert.Equal(0, fail_3.TotalCount);
            Assert.Equal(0, fail_4.TotalCount);
            Assert.Equal(0, fail_5.TotalCount);
            Assert.Equal(0, fail_6.TotalCount);

            Assert.Empty(fail_1.Responses);
            Assert.Empty(fail_2.Responses);
            Assert.Empty(fail_3.Responses);
            Assert.Empty(fail_4.Responses);
            Assert.Empty(fail_5.Responses);
            Assert.Empty(fail_6.Responses);
        }
    }
}
