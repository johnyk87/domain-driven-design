namespace JK.DomainDrivenDesign.Framework
{
    using System;
    using System.Collections.Generic;

    public class EntityIdentityComparer<TIdentity> : IEqualityComparer<IEntity<TIdentity>>
    {
        public static readonly IEqualityComparer<IEntity<TIdentity>> Default = new EntityIdentityComparer<TIdentity>();

        private readonly IEqualityComparer<TIdentity> identityComparer;

        public EntityIdentityComparer()
            : this(EqualityComparer<TIdentity>.Default)
        {
        }

        public EntityIdentityComparer(IEqualityComparer<TIdentity> identityComparer)
        {
            this.identityComparer = identityComparer ?? throw new ArgumentNullException(nameof(identityComparer));
        }

        public bool Equals(IEntity<TIdentity> x, IEntity<TIdentity> y)
        {
            if (x is null && y is null)
            {
                return true;
            }

            if (x is null || y is null)
            {
                return false;
            }

            return this.identityComparer.Equals(x.Id, y.Id);
        }

        public int GetHashCode(IEntity<TIdentity> obj)
        {
            if (obj is null)
            {
                return 0;
            }

            return this.identityComparer.GetHashCode(obj.Id);
        }
    }
}
