# Situations

Scenario-driven test composition for .NET

---

## Overview

Situations is a lightweight testing utility that allows you to define reusable **testing scenarios** ("situations") which configure mocks, dependencies, and assertions in a clean and composable way.

Instead of repeating mock setup and verification logic across tests, Situations lets you define those behaviors once and reuse them across your test suite.

This improves:

- Test readability
- Test maintainability
- Reuse of setup and assertion logic
- Behavioral clarity

---

## Motivation

Unit tests often suffer from large and repetitive arrange blocks.

Example of traditional unit testing:

```csharp
mockPosition.Setup(x => x.IsManager(managerId)).Returns(true);
mockEmployee.Setup(x => x.EmployeeExist(employeeId)).Returns(false);

service.AddEmployee(managerId, employee);

mockEmployee.Verify(x => x.AddEmployee(employee), Times.Once);
```

As tests grow, this setup becomes duplicated and difficult to maintain.

Situations solves this by allowing tests to be written using reusable behavioral building blocks.

---

## What is a Situation?

A Situation is a named behavior that can:

- Configure a mock
- Provide dependency behavior
- Verify expected interactions

Situations are defined using enums and registered in a builder configuration.

---

## Project Structure

```
Situations.Core
    Core container and execution engine

Situations.Moq
    Moq integration support

Situations.NSubstitute
    NSubstitute integration support

Situations.Sample.Tests
    Example implementation and reference usage
```

---

## Quick Start

### Step 1 — Define Situations Enum

Situations are defined as enum values representing business or behavioral states.

```csharp
public enum EmployeeCreationSituations
{
    RequestorIsManager,
    RequesterIsNotManager,
    EmployeeTryingToAddedDoesNotExist,
    EmployeeTryingToBeAddedExist,
    EmployeeWasAdded,
    EmployeeWasNotAdded,
    ManagerWasNotified,
    ManagerWasNotNotified,
    ManagerOfEmployeeIsFound
}
```

---

### Step 2 — Configure Situations

Create a configuration class that registers services, mocks, and situation behaviors.

```csharp
using Moq;
using Situations.Core;
using Situations.Moq;

public static class SampleMoqSituationConfiguration
{
    public static SituationsContainer<EmployeeCreationSituations> GetSituationsContainer()
    {
        var builder = new MoqSituationsBuilder<EmployeeCreationSituations>();

        // Register services that are undergoing tests
        builder.RegisterService<EmployeeCreationService>();

        // For services that need no setup, you can register them as mocks.
        builder.AddMock<ILoggingService>();

        // This is how you map a Moq setup to a to an enum value used to invoke that set up later during the test run.
        builder
            .RegisterSituation<IPositionRepository>(EmployeeCreationSituations.RequestorIsManager)
            .OnInvocation(mock => mock.Setup(x => x.IsManager(TestingConstants.ManagerId)).Returns(true));

        // More registration
        ...

        return builder.Build();
    }
}
```

---

### Step 3 — Write Tests

Tests become clean and expressive.

```csharp
[TestClass]
public class SampleMoqSituationTest
{
    private readonly SituationsContainer<EmployeeCreationSituations> _container =
        SampleMoqSituationConfiguration.GetSituationsContainer();

    [TestMethod]
    public void AddEmployee_GivenManagerAndEmployeeDoesNotExist_EmployeeWasAdded()
    {
        using var service = _container.GetConfiguredService<EmployeeCreationService>();

        service.InvokeSituation(EmployeeCreationSituations.RequestorIsManager);
        service.InvokeSituation(EmployeeCreationSituations.EmployeeTryingToAddedDoesNotExist);

        service.Instance.AddEmployee(TestingConstants.ManagerId, TestingConstants.Employee);

        service.InvokeSituation(EmployeeCreationSituations.EmployeeWasAdded);
    }

    [TestMethod]
    public void AddEmployee_GivenRequesterIsNotManager_ManagerWasNotified()
    {
        using var service = _container.GetConfiguredService<EmployeeCreationService>();

        service.InvokeSituation(EmployeeCreationSituations.RequesterIsNotManager);
        service.InvokeSituation(EmployeeCreationSituations.ManagerOfEmployeeIsFound);

        service.Instance.AddEmployee(TestingConstants.EmployeeId, TestingConstants.Employee);

        service.InvokeSituation(EmployeeCreationSituations.ManagerWasNotified);
    }
}
```

---

## How Situations Works

### SituationsContainer

Stores all registered situations and builds configured service instances.

### MoqSituationsBuilder

Registers:

- Services under test
- Mock dependencies
- Situation setup logic
- Situation verification logic

### InvokeSituation()

Invoking a situation executes the registered logic for that scenario. This may configure mocks or validate expectations.

---

## Benefits

- Removes repeated mock setup code
- Improves readability of tests
- Centralizes configuration
- Encourages behavior-driven testing
- Simplifies maintenance when dependencies change

---

## Recommended Usage

Situations is most useful when:

- Services contain multiple behavioral branches
- Mock configuration is repeated across tests
- Tests are written to express business rules
- Teams want clearer specification-style tests

---

## Philosophy

Situations shifts tests from implementation detail to behavior.

Instead of writing:

> Setup repository to return true

You write:

> Given RequestorIsManager

Instead of writing:

> Verify AddEmployee was called once

You write:

> Then EmployeeWasAdded

---
