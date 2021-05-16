namespace JK.DomainDrivenDesign.Framework
{
    using System.Collections.Generic;
    using Xunit;

    public class EntityIdentityComparerTests
    {
        [Fact]
        public void Equals_TwoNullInstances_ReturnsTrue()
        {
            // Arrange
            var comparer = new EntityIdentityComparer<string>();

            // Act
            var result = comparer.Equals(null, null);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Equals_FirstInstanceNull_ReturnsFalse()
        {
            // Arrange
            var entity2 = new SimpleEntityRepresentation1("test-id");
            var comparer = new EntityIdentityComparer<string>();

            // Act
            var result = comparer.Equals(null, entity2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Equals_SecondInstanceNull_ReturnsFalse()
        {
            // Arrange
            var entity1 = new SimpleEntityRepresentation1("test-id");
            var comparer = new EntityIdentityComparer<string>();

            // Act
            var result = comparer.Equals(entity1, null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Equals_SameInstance_ReturnsTrue()
        {
            // Arrange
            var entity1 = new SimpleEntityRepresentation1("test-id");
            var entity2 = entity1;
            var comparer = new EntityIdentityComparer<string>();

            // Act
            var result = comparer.Equals(entity1, entity2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Equals_DifferentInstanceWithSameIdValue_ReturnsTrue()
        {
            // Arrange
            var entity1 = new SimpleEntityRepresentation1("test-id");
            var entity2 = new SimpleEntityRepresentation1("test-id");
            var comparer = new EntityIdentityComparer<string>();

            // Act
            var result = comparer.Equals(entity1, entity2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Equals_DifferentInstanceWithDifferentIdValue_ReturnsFalse()
        {
            // Arrange
            var entity1 = new SimpleEntityRepresentation1("test-id");
            var entity2 = new SimpleEntityRepresentation1("test-id2");
            var comparer = new EntityIdentityComparer<string>();

            // Act
            var result = comparer.Equals(entity1, entity2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Equals_DifferentInstanceTypeWithSameIdValue_ReturnsTrue()
        {
            // Arrange
            var entity1 = new SimpleEntityRepresentation1("test-id");
            var entity2 = new SimpleEntityRepresentation2("test-id");
            var comparer = new EntityIdentityComparer<string>();

            // Act
            var result = comparer.Equals(entity1, entity2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Equals_CustomIdentityComparerAndEqualIdentityValue_ReturnsTrue()
        {
            // Arrange
            var entity1 = new ComplexEntityRepresentation1(new ComplexIdentity("test-id"));
            var entity2 = new ComplexEntityRepresentation1(new ComplexIdentity("test-id"));
            var comparer = new EntityIdentityComparer<ComplexIdentity>(new ComplexIdentityComparer());

            // Act
            var result = comparer.Equals(entity1, entity2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Equals_CustomIdentityComparerAndDistinctIdentityValue_ReturnsFalse()
        {
            // Arrange
            var entity1 = new ComplexEntityRepresentation1(new ComplexIdentity("test-id"));
            var entity2 = new ComplexEntityRepresentation1(new ComplexIdentity("test-id2"));
            var comparer = new EntityIdentityComparer<ComplexIdentity>(new ComplexIdentityComparer());

            // Act
            var result = comparer.Equals(entity1, entity2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetHashCode_NullInstance_ReturnsZero()
        {
            // Arrange
            var comparer = new EntityIdentityComparer<string>();

            // Act
            var result = comparer.GetHashCode(null);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void GetHashCode_SameInstance_ReturnsSameValue()
        {
            // Arrange
            var entity1 = new SimpleEntityRepresentation1("test-id");
            var entity2 = entity1;
            var comparer = new EntityIdentityComparer<string>();

            // Act
            var result1 = comparer.GetHashCode(entity1);
            var result2 = comparer.GetHashCode(entity2);

            // Assert
            Assert.NotEqual(0, result1);
            Assert.Equal(result1, result2);
        }

        [Fact]
        public void GetHashCode_DifferentInstanceWithSameIdValue_ReturnsSameValue()
        {
            // Arrange
            var entity1 = new SimpleEntityRepresentation1("test-id");
            var entity2 = new SimpleEntityRepresentation1("test-id");
            var comparer = new EntityIdentityComparer<string>();

            // Act
            var result1 = comparer.GetHashCode(entity1);
            var result2 = comparer.GetHashCode(entity2);

            // Assert
            Assert.NotEqual(0, result1);
            Assert.Equal(result1, result2);
        }

        [Fact]
        public void GetHashCode_DifferentInstanceWithDifferentIdValue_ReturnsDistinctValue()
        {
            // Arrange
            var entity1 = new SimpleEntityRepresentation1("test-id");
            var entity2 = new SimpleEntityRepresentation1("test-id2");
            var comparer = new EntityIdentityComparer<string>();

            // Act
            var result1 = comparer.GetHashCode(entity1);
            var result2 = comparer.GetHashCode(entity2);

            // Assert
            Assert.NotEqual(0, result1);
            Assert.NotEqual(0, result2);
            Assert.NotEqual(result1, result2);
        }

        [Fact]
        public void GetHashCode_DifferentInstanceTypeWithSameIdValue_ReturnsSameValue()
        {
            // Arrange
            var entity1 = new SimpleEntityRepresentation1("test-id");
            var entity2 = new SimpleEntityRepresentation2("test-id");
            var comparer = new EntityIdentityComparer<string>();

            // Act
            var result1 = comparer.GetHashCode(entity1);
            var result2 = comparer.GetHashCode(entity2);

            // Assert
            Assert.NotEqual(0, result1);
            Assert.Equal(result1, result2);
        }

        [Fact]
        public void GetHashCode_CustomIdentityComparerAndEqualIdentityValue_ReturnsSameValue()
        {
            // Arrange
            var entity1 = new ComplexEntityRepresentation1(new ComplexIdentity("test-id"));
            var entity2 = new ComplexEntityRepresentation1(new ComplexIdentity("test-id"));
            var comparer = new EntityIdentityComparer<ComplexIdentity>(new ComplexIdentityComparer());

            // Act
            var result1 = comparer.GetHashCode(entity1);
            var result2 = comparer.GetHashCode(entity2);

            // Assert
            Assert.NotEqual(0, result1);
            Assert.Equal(result1, result2);
        }

        [Fact]
        public void GetHashCode_CustomIdentityComparerAndDistinctIdentityValue_ReturnsDistinctValue()
        {
            // Arrange
            var entity1 = new ComplexEntityRepresentation1(new ComplexIdentity("test-id"));
            var entity2 = new ComplexEntityRepresentation1(new ComplexIdentity("test-id2"));
            var comparer = new EntityIdentityComparer<ComplexIdentity>(new ComplexIdentityComparer());

            // Act
            var result1 = comparer.GetHashCode(entity1);
            var result2 = comparer.GetHashCode(entity2);

            // Assert
            Assert.NotEqual(0, result1);
            Assert.NotEqual(0, result2);
            Assert.NotEqual(result1, result2);
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
