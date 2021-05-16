namespace JK.DomainDrivenDesign.Framework
{
    using System.Collections.Generic;
    using Xunit;

    public class IEntityTests
    {
        [Fact]
        public void IsSameAs_NullInstance_ReturnsFalse()
        {
            // Arrange
            var entity1 = (IEntity<string>)new SimpleEntityRepresentation1("test-id");
            // Act
            var result = entity1.IsSameAs(null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsSameAs_SameInstance_ReturnsTrue()
        {
            // Arrange
            var entity1 = (IEntity<string>)new SimpleEntityRepresentation1("test-id");
            var entity2 = entity1;

            // Act
            var result = entity1.IsSameAs(entity2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsSameAs_DifferentInstanceWithSameIdValue_ReturnsTrue()
        {
            // Arrange
            var entity1 = (IEntity<string>)new SimpleEntityRepresentation1("test-id");
            var entity2 = new SimpleEntityRepresentation1("test-id");

            // Act
            var result = entity1.IsSameAs(entity2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsSameAs_DifferentInstanceWithDifferentIdValue_ReturnsFalse()
        {
            // Arrange
            var entity1 = (IEntity<string>)new SimpleEntityRepresentation1("test-id");
            var entity2 = new SimpleEntityRepresentation1("test-id2");

            // Act
            var result = entity1.IsSameAs(entity2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsSameAs_DifferentInstanceTypeWithSameIdValue_ReturnsTrue()
        {
            // Arrange
            var entity1 = (IEntity<string>)new SimpleEntityRepresentation1("test-id");
            var entity2 = new SimpleEntityRepresentation2("test-id");

            // Act
            var result = entity1.IsSameAs(entity2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsSameAs_CustomIdentityComparerAndEqualIdentityValue_ReturnsTrue()
        {
            // Arrange
            var entity1 = (IEntity<ComplexIdentity>)new ComplexEntityRepresentation1(new ComplexIdentity("test-id"));
            var entity2 = new ComplexEntityRepresentation1(new ComplexIdentity("test-id"));
            var customComparer = new ComplexIdentityComparer();
            IEntity<ComplexIdentity>.SetIdentityComparer(customComparer);

            // Act
            var result = entity1.IsSameAs(entity2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsSameAs_CustomIdentityComparerAndDistinctIdentityValue_ReturnsFalse()
        {
            // Arrange
            var entity1 = (IEntity<ComplexIdentity>)new ComplexEntityRepresentation1(new ComplexIdentity("test-id"));
            var entity2 = new ComplexEntityRepresentation1(new ComplexIdentity("test-id2"));
            var customComparer = new ComplexIdentityComparer();
            IEntity<ComplexIdentity>.SetIdentityComparer(customComparer);

            // Act
            var result = entity1.IsSameAs(entity2);

            // Assert
            Assert.False(result);
        }

        private class SimpleEntityRepresentation1 : IEntity<string>
        {
            public SimpleEntityRepresentation1(string id)
            {
                this.Id = id;   
            }

            public string Id { get; }
        }

        private class SimpleEntityRepresentation2 : IEntity<string>
        {
            public SimpleEntityRepresentation2(string id)
            {
                this.Id = id;   
            }

            public string Id { get; }
        }

        private class ComplexIdentity
        {
            public ComplexIdentity(string id)
            {
                this.Id = id;
            }

            public string Id { get; }
        }

        private class ComplexIdentityComparer : IEqualityComparer<ComplexIdentity>
        {
            public bool Equals(ComplexIdentity x, ComplexIdentity y)
            {
                return EqualityComparer<string>.Default.Equals(x.Id, y.Id);
            }

            public int GetHashCode(ComplexIdentity obj)
            {
                return EqualityComparer<string>.Default.GetHashCode(obj.Id);
            }
        }

        private class ComplexEntityRepresentation1 : IEntity<ComplexIdentity>
        {
            public ComplexEntityRepresentation1(ComplexIdentity id)
            {
                this.Id = id;   
            }

            public ComplexIdentity Id { get; }
        }

        private class ComplexEntityRepresentation2 : IEntity<ComplexIdentity>
        {
            public ComplexEntityRepresentation2(ComplexIdentity id)
            {
                this.Id = id;   
            }

            public ComplexIdentity Id { get; }
        }
    }
}
