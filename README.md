# Domain Driven Design

A lot has been said about the book `Domain-Driven Design - Tackling Complexity In The Heart Of Software` by Eric Evans.

Every person that talks about domain driven design appears to have their own interpretation of the ideas in the book, and they use those interpretations to push for or against various things like programming paradigms, arquitectural patterns, etc.

Instead of relying on indirect accounts, I took it upon myself to actually read the book, and try to interpret the ideas in their original form. This page gathers my notes on the subject.

**Note 1:** these notes are the result of my interpretation, and I don't claim them to be completely faithful to their intended meanings.

**Note 2:** this is not a comprehensive gathering of all the useful things in the book. These seemed to me like the most important parts to remember, but for a complete view please read the book.

## Overview

Domain driven design revolves around techniques to design what the book calls as the **domain**.

The domain is where all the **business logic** would lie, and would be at the center of all applications, **when it makes sense**. This book is clear on the fact that not every application needs or benefits from having an elaborate domain.

The result of that design is what is called as the **model**. The model is a representation of the main **concepts** in the domain, their **relationships**, and the **business rules** that apply to them. However, the model should not try to represent everything there is to know about the domain, to the point of exhaustion. The model should **focus on the aspects that really matter** to the purpose of the application. This model **should be clear in the actual implementation**, i.e., the code should represent the domain model as faithfully as any other form of representation.

One of the most important concepts in the book is the **ubiquitous language**. This is the **common language** shared among the developers, the business experts, the customers, and any other stakeholder of the system. That language should appear in every aspect of the design, development and operation of the system. It drives the discussions between the business experts, developers and customers. It drives the concepts in the domain model. The main goal of this language is to bring everyone to the same page, and make sure that all stakeholders understand what is being discussed in any interaction. If we take nothing else from the book, a common language between all stakeholders should always be the goal for any project team.

According to the book, in terms of the actual code, all the business logic lives in the domain. One recommendation is for the domain to be an independent layer in the architecture. There isn't much said about what other layers can or should exist, as long as the domain is clearly separated and all business logic resides in it.

**One very important note:** nothing that is described in this process should be considered as static. Not the model, not the code, and certainly not the ubuquitous language that supports all of it. Domain driven design is not supposed to be something that is done in the beginning of a project and considered as the absolute truth. It is supposed to drive the project, and everything from the model to the code should evolve, and so should the ubiquitous language embedded in all of it. The true value of domain driven design is in bringing domain knowledge into the software and the development teams, and letting them evolve the software as their knowledge of the domain also evolves. It is with that evolution that the model will truly become more aligned with the domain, and will more easily be able to deal with new requirements that appear along the way.

## Building blocks

These are the most basic concepts on which the domain model should be built. All of these are described in the book, and the notes in this section are my attempt to single out the most important parts about each of them.

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
- They represent elements that we only care about **"what" they are**, not "who" or "which" they are.
- Recommended for when we only care about an element's attributes.
- Value objects **should be immutable**, i.e. changes should result in new instances with the new values.
- A value object can group attributes into a "conceptual whole", i.e. a group of attributes that belong together in the domain scenario in question.
- Can be used to **group related attributes** of an entity (e.g.: a `Person` entity can have an `Address` property as a value object with `City`, `Street` and `PostalCode` attributes).
- **Avoid bidirectional associations** between two value objects. It usually doesn't make sense, since there is no identity to which to point, there are only values that are recreated when changed. If a need for bidirectional associations between value objects exists, then maybe one or both of the value objects are missing an identity, in which case we're dealing with an entity and not a value object.

*Personal note:* in .NET, C# 9 record types might make for good value objects. They have natural support for immutability and value based equality. They also provide an easy mechanism to copy an instance while changing values (see the [`with` expression](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/with-expression)).

### Services

- ***Sometimes, it just isn't a thing.***
- *A ***good service has three characteristics***:*
  1. *The operation relates to a domain concept that is not a natural part of an entity or value object.*
  2. *The interface is defined in terms of other elements of the domain model.*
  3. *The operation is stateless.*
- Services tend to be **named using verbs** rather than nouns.

### Modules

- Use modules to **group concepts of the domain** into independent pieces.
- Modules should have **low coupling** (few dependencies between them) and **high coesion** (highly related concepts within).
- Like every other domain concept, module **names and responsibilities should be related to domain concepts**.
- **Modules should evolve** just like any other domain concept, and they should not restrict the evolution of the domain concepts contained in them.

### Aggregates

- A ***cluster of associated objects*** treated as ***a unit for the purpose of data changes***.
- An aggregate has a **root and a boundary**:
  - *The boundary defines what is inside the aggregate.*
  - *The root is a single, specific entity contained in the aggregate.*
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
- They should provide operations to obtain domain objects based on certain object attributes.
- They should also provide operations to add and remove objects, which should then be reflected on the backing data store.
- They should **hide the details of the data storage technology**.
- They should **deal with aggregates**, so they should be designed only for aggregate roots.
- Factories make new objects; repositories find existing objects. Repositories can delegate the object reconstitution to a factory.

## Design patterns

Along the book a few design patterns are described. In this section are my notes on the ones that seemed more prominent.

### Explicit constraints

- Important business rules sometimes end up being hidden inside some domain object, like for instance a limit applied to some value of an object.
- That may be an indication that some concept is missing, and it might make sense to make that rule an explicit concept in the model.
- Example: in the cargo shipping business, it is common to overbook shipping voyages to compensate for last-minute cancellations. That overbooking rule can be a good candidate for an explicit constraint, like an `OverbookingPolicy`, applied to the relationship between the shipping voyage and the cargo.

### Strategy

- When there are multiple ways of executing a process, those algorithm variations may be worth a domain object of their own.
- In these cases a **strategy** pattern may be useful.
- This pattern is useful in representing policies that are important in the domain, like for instance multiple algorithm versions that apply different prioritization rules.

### Specifications

- Some processes and algorithms are based on variable **rules or conditions**.
- Those rules might become clearer when explicitly represented in the domain.
- A **specification** is a predicate that determines if an object does or does not satisfy some criteria.
- That specification can then be used by another object to control the conditional parts of an operation.
- A specification can be used for multiple use cases that involve testing a condition based on variable inputs, including selection, filtering and validation.
- It can also be used as the input for building or configuring objects according to some variable criteria.
- The whole point of this pattern is to make rules clearer, and reduce variability in algorithm implementations.

### Intention-revealing interfaces

- Note: the term "interfaces" is used to refer to the public contract of an object, and not necessarily the `interface` constructs known in some programming languages.
- This pattern refers to the recommendation to use names in the interfaces that focus on what is the purpose of an object or operation, and what a consumer should expect from them.
- Names used in those interfaces should not be tied to how they do what they do.
- To put it simply: interfaces should **say what**, **not how**.

### Side-effect-free functions

- *Personal note:* the functional programming folks will love this one.
- It is recommended to place most of the logic in **functions** that return results **without observable side-effects**.
- Basically, try to reduce and segregate operations with side effects from all other logic.
- Whenever it makes sense, **move logic to immutable objects** (e.g.: value objects) and expose the operations that work on them as side effect free functions **returning new instances of the object**.
- The idea is that these types of functions are **safer to use and combine**.

### Assertions

- This just says that we should state **pre and post-conditions** of the operations.
- **Assertions should be included in other model representations**, not just in code.
- Assertions should be **based on the state of the inputs and outputs**, not on the procedure itself.
- Some programming languages may support assertions natively, but others may require unit tests to enforce those assertions (or at least try to check that they hold true).
- The value in assertions comes from the fact that **they are great at detecting awkward semantics**.
  - For instance, an operation that infers moving things from one place to another, but where the source is never actually updated to reflect that the content has moved.
  - These are usually signs that the model is not aligned with the expected behavior of a procedure, and may need some changes.

### Conteptual contours

- Conceptual countours is a name given to the idea that concepts should be **"whole"**.
- In other words:
  - They should be **as small as they need to be** to represent all the **details needed in the domain**.
  - They should be **as big as they need to be** to **avoid** filling the model with **unnecessary detail**.
- Basically, try to find the **right level of detail needed for each concept**.
- Break things apart into more specific concepts when needed, and bundle things into more generic concepts when their specifics are not important.

### Standalone classes

- **Not everything needs to be related to something.**
- Whenever we have a concept that can exist on its own, and can concentrate some important piece of logic, we can attempt to move it to a standalone class.
- **Standalone classes are easier to reason about** without the burden of trying to understand how they relate to other concepts.
- ***The goal is not to eliminate all dependencies, but to eliminate all nonessential ones.***

### Closure of operations

- The name *closure* here comes from mathematics.
  - From [Wikipedia](https://en.wikipedia.org/wiki/Closure_(mathematics)): *In mathematics, a set is closed under an operation if performing that operation on members of the set always produces a member of that set.*
- Similarly, an **operation on a domain object can be considered closed if it accepts and returns the same type of object**.
- In some cases, we can also create value objects with procedures that accept instances of themselves as arguments, and return new instances of that same value object type.
  - This can be very useful in defining domain mathematics.
  - For instance, specifications could be combined to form composed specifications by means of logical operations, like `specificationA.And(specificationB)` would result in a specification instance that represents a combination of both `specificationA` and `specificationB`.
- One more common example of such operations are collection methods for filtering that return new collections.
  - Examples: some collection operations in .NET's Linq and Java's streams.
- This sort of methods allow for **easy combination**, and leveraging that **can lead to domain specific languages** that can make code very expressive.

### Composite

- Intended to deal with *whole-part* situations, i.e., when an **object can be composed in a way that a common set of semantics apply to the parts and to the composition**.
- Leaf objects in that hierarchy return their own values, while composite objects return the result of an aggregation operation over their parts.
- The example of specifications given in the previous pattern can also be considered as a composite. A combination of specifications is itself a specification.
- Not every object that shares semantics is a candidate for this pattern. **The resulting composite must make sense in the domain in question, so don't force it.**

## Stategic design

The last part of the book talks about strategic design, which is a set of recommendations to deal with higher level organization and structure.

### Bounded context

In a large enough application there may be concepts that need a slightly different representation for different purposes, or they may even be similarly named concepts that mean different things.

Sometimes the best way to keep consistency in a model is to create multiple models. The name *bounded context* is the name given to the context in which a given model applies, so that two separate models can have the concepts and representations that make more sense in that particular context.

Different ubiquitous languages may develop in different bounded contexts, or at least different dialects.

Bounded contexts are not the same as modules. Modules are used to organize concepts inside a model, while bounded contexts separate the boundaries of distinct models. Both of them are meant as ways to organize and split the domain, but at various levels of detail.

As usual when dealing with organization techniques, we should strive for model cohesion inside a bounded context, and low model coupling between bounded contexts.

It is common to have distinct bounded contexts and models when there are multiple teams, so that each them works on their own bounded context.

However, distinct models will have to connect at some level. To represent that integration, a context map may be useful to represent the various bounded contexts, and the points where their models connect.

There can be various levels of integration between bounded contexts. The following is a list of the ones proposed in the book:
- **Shared kernel:** a part of the model will be shared and maintained across bounded contexts. Maintaining this shared kernel is the responsibility of all the teams involved. Any concept that applies across bounded contexts should be placed in the shared kernel, with an agreed upon meaning. This model includes the code and any other related artifacts.
- **Customer/supplier development teams:** in this approach, one team acts as a customer to another. The customer sets the requirements it needs, and the supplier is responsible for delivering an interface that meets those requirements. The delivery of changes might be subject to the budget and scheduling constraints of the supplier team.
- **Conformist:** when collaboration is too hard, sometimes adopting another team's model (at least partially) may be the best solution for everyone involved. This reduces the need for more complex integrations and aligments, but it may leave some teams at the mercy of another team. Changes to the source model may ripple through other models. The conformist and shared kernel solutions both share at least part of the model. The biggest difference is in how the decisions are made of what goes into that shared model. While in the shared kernel there is a clear collaboration, in the conformist version the decision is one sided.
- **Anticorruption layer:** this one assumes a one-sided dependency, just like the conformist, but instead of adopting the other model, an extra layer is included to translate the inputs and outputs of that model into a more appropriate representation on the consumer side.
- **Separate ways:** sometimes integration is just too costly. In some situations, creating completely independent models may be the best approach. Of course, if at any point in the future two models really need to integrate, that process will likely be much harder than when using any of the other approaches.
- **Open host service:** this focuses on a model that is consumed by various other systems. In this case, instead of forcing a model upon others, a simpler interface would be provided that exposes the features of the model as a set of services for others to consume. This interface can use a different language than the one used in its underlying model. The goal here is to provide a common language and reduce the amount of translation needed in the consuming models. Ideally, the language used at this interface should be published and shared as a common language between all interested parties. If possible, standard languages and notations should be used, reducing the need for custom interpretations.

The book also explores some guiding principles on when to choose each integration solution, and how to evolve between them, but just read the book for more details. TL;DR: let's just say it involves some politics.

### Core domain

Not everything in a domain model shares the same importance. Some concepts, as necessary as they are, may not be considered as the differentiating factors in an application's success.

In order to focus efforts on the truly important parts of the domain, it is proposed that the model be refined to clearly identify what the core domain is. The core domain should be the part of the application that truly makes it valuable for the company or user that explores it.

So, we should strive to find the part of our model that is really important, and try to move everything else to separate surrounding modules.

Those separate modules can and should be as generic as possible. The goal is not necessarily to make them reusable, but to make them more flexible and able to deal with changes in requirements more easily. The main effort of maintaining a model should be dedicated to the core, not the rest of it. One possibility is using off-the-shelf solutions for the surrounding modules. Other options are described in the book, along with advantages and disadvantages, varying from off-the-shelf solutions to in-house development.

For more about core domains and their surrounding modules, please refer to the book.

### Large-scale structure

One of the final chapters in the book explores ideas behind large-scale structure.

The main goal of this part is to propose some patterns to organize the domain at larger scales, so that it is easier to reason about it as a whole, and to help understand how the multiple parts fit together.

Here are the patterns listed in this section of the book:
- **Evolving order:** don't try to specify too much detail and too many constraints on how each part of the application should be built. The system should be allowed to evolve, and each team should be given enough flexibility to decide on the best solutions for them, even if that requires changing the large-scale structure to better respond to the needs of the system.
- **System metaphor:** this refers to the proposition that we can try to find a metaphor for how the system should be structured. It is supposed to drive people in their decisions regarding the large-scale structure of the system. Although it can help with communication about the system, we should be mindful of how that communication evolves, and be aware of weirdness that may indicate that the metaphor might be getting in the way or even be driving decisions in a wrong direction. When it gets to that, we should not be afraid to drop that metaphor. The example referred to in the book is that of a firewall. In buildings, a firewall is a wall that prevents fires from spreading across it. That metaphor was adopted in software to represent a layer of security in network communications that attempts to protect a given part of the network from the other networks.
- **Responsibility layers:** some aspects of a large domain can be divided into layers. These layers are related to domain responsibilities, they are not intended to represent software related aspects. The general idea is that one part of the domain has a clear set of related functionality, and that functionality depends on other pieces of functionality. In this scenario, it is possible that the domain may be structured into two or more layers to enforce a separation between those distinct purposes, and to clarify their dependencies.
- **Knowledge level:** some parts of a domain may need to be malleable. For instance, there are cases where some rules of the domain may need to be made configurable to users. In that case, two levels may be created: the concrete one, with the basic model that can be configured; and the knowledge level, where we make clear the concepts that are used solely for the purpose of configuring the behavior of the basic model.
- **Pluggable component framework:** basically it proposes the creation of a common model for when multiple applications need to interoperate using the same abstractions. Instead of relying on translation layers between models, a common abstract model can be extracted, and all those that need to interoperate will conform to the abstractions in that common model. It is mentioned that this is a pattern best applied when multiple applications already exist sharing compatible abstractions. This usually requires experience in various applications of the same domain.
