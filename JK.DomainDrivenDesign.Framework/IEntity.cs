namespace JK.DomainDrivenDesign.Framework
{
    using System;
    using System.Collections.Generic;

    public interface IEntity<TIdentity>
    {
        private static IEqualityComparer<IEntity<TIdentity>> EntityIdentityComparer = EntityIdentityComparer<TIdentity>.Default;

        TIdentity Id { get; }

        public static void SetIdentityComparer(IEqualityComparer<TIdentity> comparer)
        {
            if (comparer is null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            EntityIdentityComparer = new EntityIdentityComparer<TIdentity>(comparer);
        }

        public bool IsSameAs(IEntity<TIdentity> another)
            => EntityIdentityComparer.Equals(this, another);
    }
}
