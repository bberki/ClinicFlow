# Database Schema

This document outlines the database structures used by ClinicFlow. It describes tables, indexes, and stored procedures present in the initial migration.

## Tables

### Doctors
- **Id** (`int`, identity, primary key)
- **FirstName** (`nvarchar(max)`)
- **LastName** (`nvarchar(max)`)
- **Specialty** (`nvarchar(max)`)

### Patients
- **Id** (`int`, identity, primary key)
- **FirstName** (`nvarchar(max)`)
- **LastName** (`nvarchar(max)`)
- **DateOfBirth** (`datetime2`)

### Users
- **Id** (`int`, identity, primary key)
- **Username** (`nvarchar(450)`)
- **PasswordHash** (`nvarchar(max)`)

### Appointments
- **Id** (`int`, identity, primary key)
- **ScheduledAt** (`datetime2`)
- **UserId** (`int`, foreign key -> Users.Id)
- **PatientId** (`int`, foreign key -> Patients.Id)
- **DoctorId** (`int`, foreign key -> Doctors.Id)

## Indexes

- `IX_Appointments_DoctorId` on **Appointments(DoctorId)**
- `IX_Appointments_PatientId` on **Appointments(PatientId)**
- `IX_Appointments_UserId` on **Appointments(UserId)**
- `IX_Appointments_ScheduledAt` on **Appointments(ScheduledAt)** (non-clustered)
- `IX_Users_Username` on **Users(Username)** (unique)

## Stored Procedures

### `GetAppointmentsForDoctor`
Retrieves all appointments for a given doctor ID.

```sql
CREATE PROCEDURE GetAppointmentsForDoctor @DoctorId INT AS
BEGIN
    SELECT * FROM Appointments WHERE DoctorId = @DoctorId;
END
```

Future updates to the database schema should follow this format.
