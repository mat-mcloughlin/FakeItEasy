﻿namespace FakeItEasy.Specs
{
    using System;
    using Core;
    using FluentAssertions;
    using Xbehave;

    public static class FakeScopeSpecs
    {
        [Scenario]
        public static void CallFromConstructor(
            IFakeObjectContainer fakeObjectContainer,
            MakesVirtualCallInConstructor fake,
            string virtualMethodValueInsideOfScope,
            string virtualMethodValueOutsideOfScope)
        {
            "establish"
                .x(() =>
                    {
                        fakeObjectContainer = A.Fake<IFakeObjectContainer>();
                        A.CallTo(() => fakeObjectContainer.ConfigureFake(A<Type>._, A<object>._))
                            .Invokes(
                                (Type t, object options) =>
                                A.CallTo(options).WithReturnType<string>().Returns("configured value in fake scope"));
                    });

            "when configuring a method called by a constructor from within a scope"
                .x(() =>
                    {
                        using (Fake.CreateScope(fakeObjectContainer))
                        {
                            fake = A.Fake<MakesVirtualCallInConstructor>();
                            virtualMethodValueInsideOfScope = fake.VirtualMethod(null);
                        }

                        virtualMethodValueOutsideOfScope = fake.VirtualMethod(null);
                    });

            "it should use the fake object container to configure the fake"
                .x(() => A.CallTo(() => fakeObjectContainer.ConfigureFake(typeof(MakesVirtualCallInConstructor), fake))
                             .MustHaveHappened());

            "it should return the configured value within the scope during the constructor"
                .x(() => fake.VirtualMethodValueDuringConstructorCall.Should().Be("configured value in fake scope"));

            "it should return the configured value within the scope after the constructor"
                .x(() => virtualMethodValueInsideOfScope.Should().Be("configured value in fake scope"));

            "it should return default value outside the scope"
                .x(() => virtualMethodValueOutsideOfScope.Should().Be(string.Empty));
        }
    }
}
