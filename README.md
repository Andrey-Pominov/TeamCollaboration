# Real-Time Team Collaboration Task Board

This Blazor Server project showcases a real-time team collaboration experience similar to a Kanban task board. It highlights how modern web apps can deliver instant updates across multiple clients and serves as a strong addition to a .NET portfolio.

## Highlights
- Real-time task updates powered by SignalR so moving a card from `To Do` to `In Progress` appears instantly for every connected user.
- Interactive drag-and-drop UI built with Blazor components to make task management feel natural and responsive.
- Clean, layered architecture (DDD or Clean Architecture) that separates Presentation (Blazor), Application/Services, Domain, and Infrastructure/Data concerns.
- Background services using the .NET hosting model to handle notifications, housekeeping, or other asynchronous workflows.

## Key Technologies
- **SignalR:** Enables bidirectional, real-time communication between the server and all connected clients.
- **Blazor Server:** Delivers rich, interactive components backed by .NET on the server side.
- **Clean Architecture:** Keeps the codebase maintainable with clear separation between UI, business rules, and persistence.
- **Background Services:** Facilitates scheduled or long-running tasks that support the real-time experience.

## Project Goals
- Demonstrate modern collaboration features that users expect from productivity tools.
- Provide a reference implementation for structuring real-time Blazor applications.
- Encourage experimentation with drag-and-drop interactions in Blazor and SignalR-driven state updates.

## Next Steps
- Build out domain models for teams, boards, columns, and tasks.
- Implement SignalR hubs that broadcast changes and synchronize client state.
- Design background services for notifications or periodic maintenance.
- Expand UI components to support task filtering, assignments, and history tracking.

## Contributing
1. Clone the repository and open it in your preferred IDE (e.g., Rider, Visual Studio).
2. Restore dependencies, review the proposed architecture, and start extending the board features.
3. Share ideas for additional real-time interactions or architectural improvements.

---

This project is a foundation for exploring how Blazor Server and SignalR can create engaging, collaborative experiences in real time.

