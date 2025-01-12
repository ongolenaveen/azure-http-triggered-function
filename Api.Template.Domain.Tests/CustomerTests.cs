using Api.Template.Domain.ReadModels;
using Api.Template.Shared.Utilities;
using AutoFixture;
using FluentAssertions;

namespace Api.Template.Domain.Tests
{
    public class CustomerTests
    {
        private readonly Fixture _fixture = new();

        [Fact]
        public void Get_Set_Customer_Should_Be_Successful()
        {
            var customer = _fixture.Create<Customer>();
            var sut = new Customer
            {
                Name = customer.Name,
                Type = customer.Type,
                CreatedOn = customer.CreatedOn,
                UpdatedOn = customer.UpdatedOn
            };

            sut.Should().BeEquivalentTo(customer);
        }

        [Fact]
        public void Validate_Customer_Instance_Should_Be_Successful()
        {
            var customer = _fixture.Create<Customer>();
            var results = ModelValidator.Validate(customer);

            results.IsValid.Should().BeTrue();
            results.Results.Should().BeEmpty();
        }
    }
}
