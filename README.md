# Domain Driven Design

## The theory

### Entities

- Entities have **identity**.
- Identity is defined by representing the **same "thing"**, regardless of whether other attributes have changed.
- The model must specify what it **means** to be the same thing (aka, how to compare entity identities).
- Entities should be used only where identity needs to be tracked.
- The **identity** of an entity **should be immutable**, thus preventing that an entity changes its identity over its lifecycle.
- Entities **can be mutable**, i.e. any attribute that is not part of its identity can be changed over time.
- Try to **avoid bidirectional associations** between entities unless strictly necessary. Bidirectional associations **can usually be refactored into unidirectional associations** by introducing some missing concept in how the association takes place.

### Value objects

- Value objects have **no identity**.
- Represent elements that we only care about **"what" they are**, not "who" or "which" they are.
- Recommended for when we only care about an element's attributes.
- Value objects **should be immutable**, i.e. changes should result in new instances with the new values.
- A value object can group attributes into a "conceptual whole", i.e. a group of attributes that belong together in the domain scenario in question.
- Can be used to **group related attributes** of an entity (e.g.: a `Person` entity can have an `Address` property as a value object with `City`, `Street` and `PostalCode` attributes).
- **Avoid bidirectional associations** between two value objects usually doesn't make sense, since there is no identity to which to point, only values that be recreated when changed. If a need for bidirectional associations between value objects exists, then maybe one or both of the value objects are missing an identity, in which case we're dealing with an entity and not a value object.
- C# 9 record types might make for good value objects. They have natural support for immutability and value based equality. They also provide an easy mechanism to copy an instances while changing values (see the [`with` expression](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/with-expression)).

### Services

- **"Sometimes, it just isn't a thing."**
- "A **good service has three characteristics**:
  1. The operation relates to a domain concept that is not a natural part of an entity or value object.
  2. The interface is defined in terms of other elements of the domain model.
  3. The operation is stateless."
- Services tend to be **named using verbs** rather than nouns.

### Modules

- Use modules to **group concepts of the domain** into independent pieces.
- Modules should have **low coupling** (few dependencies between them) and **high coesion** (highly related concepts within).
- Like every other domain concept, module **names and responsibilities should be related to domain concepts**.
- **Modules should evolve** just like any other domain concept, and they should not restrict the evolution of the domain concepts contained in them.

### Aggregates

- A **"cluster of associated objects"** treated as **"a unit for the purpose of data changes"**.
- An aggregate has a **root and a boundary**:
  - "The boundary defines what is inside the aggregate."
  - "The root is a single, specific entity contained in the aggregate."
- Objects outside the aggregate **can only hold references to** the entity which is **the aggregate root**.
- **Root entities have global identity** (i.e. unique across multiple aggregates), while **entities internal to the aggregate have local identity** (i.e. unique only in the context of the aggregate).

### Factories

- A factory **encapsulates** the knowledge needed to create a complex object or aggregate.
- Why factories?
  - Object assembly should not be the assembled object's responsibility.
  - Making consumers assemble the objects complicates the design of the client and may be delegating responsibilities to outside the boundaries of the domain.
  - So, **building a domain object is a domain responsibility**, but it is not the built object's responsibility, hence a factory.
- Factories can be simple factory methods, or follow more complex patterns like abstract factories or builders. The important part of using a factory is the segregation of responsibility of building a complex object, not the actual factory's design.
- There are **two basic requirements for any good factory**:
  - The creation method should be atomic and enforce all the invariants of the object under construction. The returned object must always be in a consistent state.
  - The factory should be abstracted to the desired type, and not necessarily to a concrete object implementation.
- A factory can just be a factory method on another domain object, when the internal rules of that object are central to the new object's creation.
- Don't use factories for every object. **Sometimes a constructor is enough.**
- Factories can also be used to reconstitute objects from another medium, like a data store representation or other network representations.
- Reconstitution should still try to ensure the invariants, but different outcomes may be necessary when the invariants are broken.

### Repositories

- Repositories should be used to **abstract data access for any globally accessible object**.
- Repositories should provide operations to obtain domain objects based on certain object attributes.
- Repositories should also provide operations to add and remove objects, which should then be reflected on the backing data store.
- Repositories should **hide the details of the data storage technology**.
- Repositories should **deal with aggregates**, so they should be designed only for aggregate roots.
- Factories make new objects; repositories find old objects. Repositories can delegate the object reconstitution to a factory.
