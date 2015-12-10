namespace FakeItEasy.Tests.Configuration
{
    using System;
    using FakeItEasy.Configuration;
    using FakeItEasy.Core;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class RecordedCallRuleTests
    {
        [Test]
        public void UsePredicateToValidateArguments_should_set_predicate_to_IsApplicableToArguments()
        {
            var rule = this.CreateRule();

            Func<ArgumentCollection, bool> predicate = x => true;

            rule.UsePredicateToValidateArguments(predicate);

            rule.IsApplicableToArguments.Should().BeSameAs(predicate);
        }

        [Test]

        public void DescriptionOfValidCall_should_be_recorded_call()
        {
            // Arrange
            var rule = this.CreateRule();

            // Act

            // Assert
            rule.DescriptionOfValidCall.Should().Be("Recorded call");
        }
        
        private RecordedCallRule CreateRule()
        {
            return new RecordedCallRule(A.Fake<MethodInfoManager>());
        }
    }
}
