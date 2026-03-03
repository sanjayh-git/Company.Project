# Meter Readings API (Interview Exercise)

> Parts of this solution (project scaffolding and some boilerplate code) were generated with the help of an AI coding assistant.  
> I have reviewed, understood and adapted the code to ensure it meets the assignment requirements and my own coding standards.

## Overview

This is a C# ASP.NET Core Web API that allows uploading a CSV file of meter readings and persists valid readings to a SQLite database.

Endpoint:

- `POST /meter-reading-uploads`  
  Accepts a CSV file of meter readings and returns the number of successful and failed rows.

Validation rules:

- A meter reading must be associated with an **existing account**.
- Reading value must match the format `NNNNN` (5 digits).
- The same entry (AccountId + ReadingDateTime) cannot be loaded twice.
- When an account already has readings, a new reading must not be **older** than the latest existing reading for that account (nice-to-have).

## Tech stack

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core + SQLite
- Swagger (OpenAPI)

## Getting started

1. Ensure you have the .NET 10 SDK installed.

2. Put your **Test_Accounts.csv** file in:

   ```text
   MeterReadings.Api/Data/Test_Accounts.csv
   ```

   The expected format is:

   ```csv
   AccountId,FirstName,LastName
   2344,Tommy,Test
   2233,Julio,Test
   ...
   ```

3. Restore packages:

   ```bash
   dotnet restore
   ```

4. Apply migrations (optional – or let EF create the DB on first run if you don't add migrations):

   ```bash
   dotnet ef database update
   ```

5. Run the API:

   ```bash
   dotnet run
   ```

6. Open Swagger UI in the browser (URL will be shown in the console, typically `https://localhost:080/swagger`).

7. Use the `POST /meter-reading-uploads` endpoint:

   - Click **Try it out**
   - Select your `Meter_reading.csv` file as the `file` form-field.
   - Execute and observe the `successCount` and `failureCount` in the JSON response.

## Notes for discussion in the interview

- **Architecture**  
  - Simple layered approach: Controller → Service → DbContext (EF Core).  
  - CSV parsing is abstracted behind `ICsvParser` to keep the service testable.

- **Validation**  
  - Account existence check against seeded accounts.
  - Regex-based validation for meter values to enforce `NNNNN`.
  - Unique index on `(AccountId, ReadingDateTime)` so the DB enforces "no duplicates".
  - Business rule to ignore readings older than the latest existing one.

- **Testing**  
  - `MeterReadingService` can be unit tested with:
    - An in-memory or SQLite test DbContext.
    - A fake `ICsvParser` returning known rows.

- **Extensibility**  
  - Could add authentication, pagination, or a UI client (React/Angular) that POSTs CSV via `multipart/form-data`.


  graph TD
    Api[Company.Project.Api]
    App[Company.Project.Application]
    Infra[Company.Project.Infrastructure]
    Domain[Company.Project.Data]

    Api --> App
    Api --> Infra
    App --> Domain
    Infra --> App
    Infra --> Domain
