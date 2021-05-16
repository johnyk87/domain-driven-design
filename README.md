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
