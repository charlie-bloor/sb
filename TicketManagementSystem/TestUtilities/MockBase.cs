using System;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;

namespace TestUtilities
{
    /// <summary>
    /// Base class for tests that adds auto-mocking services.
    /// </summary>
    /// <typeparam name="TSubject">The type of the system under test</typeparam>
    public abstract class MockBase<TSubject> where TSubject : class
    {
        protected AutoMocker Mocker { get; private set; }

        /// <summary>
        /// The automatically generated system under test.
        /// </summary>
        protected TSubject Subject { get; private set; }

        [SetUp]
        public void BaseSetUp()
        {
            Mocker = new AutoMocker();

            SetUp();

            Subject = Mocker.CreateInstance<TSubject>();
        }

        [TearDown]
        public void BaseTearDown()
        {
            if (Subject is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        /// <summary>
        /// Obtain an instance of an automatically generated mock dependency.
        /// Typically, this is for setup/verification purposes.
        /// </summary>
        /// <typeparam name="TMock">The type (typically an Interface) of the dependency being mocked</typeparam>
        protected Mock<TMock> GetMock<TMock>() where TMock : class
        {
            return Mocker.GetMock<TMock>();
        }

        /// <summary>
        /// Register specific instances for the subjects dependencies
        /// before subject itself is AutoMocked
        /// </summary>
        protected virtual void SetUp()
        {
        }
    }
}