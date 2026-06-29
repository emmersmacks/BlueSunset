# Gameplay Overview

The project is structured into four layers with strict responsibility boundaries and top-down dependency flow.

---

## Presentation Layer (Presenter)

- Unity components (Transform, Animator, VFX, AudioSource)
- visual state representation
- animations and effects playback

Rules:
- contains no gameplay logic
- makes no decisions
- has no knowledge of other entities or systems

---

## Game Logic Layer (Controller / System)

### Controller (Entity Logic)

- per-entity logic
- internal state management
- command execution (Move, Act, Interact)

Rules:
- does not coordinate multiple entities
- does not implement global rules

---

### System (Orchestration Layer)

- manages multiple controllers
- gameplay rules and scenarios
- high-level decision making
- entity coordination

Rules:
- does not depend on presentation layer
- does not implement low-level entity logic

---

## Scene Infrastructure Layer (Services)

- time and timers
- randomness
- navigation and graph data
- audio and scene utilities
- shared scene tools and event systems

Rules:
- contains no gameplay logic
- does not control entities

---

## Core Rules

- System = group coordination
- Controller = single entity logic
- Presenter = visual representation
- Services = infrastructure and utilities