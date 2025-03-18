# Customer Management System

## Overview

This system is a Customer Management Application built with Windows Forms (WinForms), utilizing Entity Framework (EF) for database interactions and log4net for logging. It allows managing customer data with features like adding, editing, searching, and exporting customer records.

## Prerequisites

Before running this application, ensure the following dependencies are installed:

### 1. Development Environment

- **.NET Framework**  
  Ensure you have the required .NET version installed, **.NET Framework 4.8**.

- **Visual Studio 2022 (or later)** with the following workloads:
  - .NET desktop development
  - Data storage and processing

- **SQL Server**  
  Ensure you have an instance running, such as SQL Server Express or SQL Server Developer Edition.

### 2. Required NuGet Packages

Install the required NuGet packages in the solution before building the application. Use the **NuGet Package Manager** or run the following commands in the **Package Manager Console**:

```bash
Install-Package EntityFramework
Install-Package log4net
Install-Package Microsoft.EntityFramework.SqlServer
Install-Package System.Data.SqlClient
```

## Configuration

## 1. Database Connection

Modify the `App.config` file to set up the database connection:

```xml
<connectionStrings>
    <add name="CustomerContext"
         connectionString="Server=YOUR_SERVER_NAME;Database=CustomerDB;Trusted_Connection=True;"
         providerName="System.Data.SqlClient" />
</connectionStrings>
```

Replace `YOUR_SERVER_NAME` with your SQL Server instance.

## 2. log4net Configuration

The system uses **log4net** for logging errors and activities. The logging configuration is specified in `App.config`:

```xml
<configSections>
    <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler, log4net"/>
</configSections>

<log4net>
    <appender name="RollingFileAppender" type="log4net.Appender.RollingFileAppender">
        <file value="Logs/CustomerApp.log" />
        <appendToFile value="true" />
        <maximumFileSize value="10MB" />
        <maxSizeRollBackups value="5" />
        <layout type="log4net.Layout.PatternLayout">
            <conversionPattern value="%date [%thread] %-5level %logger - %message%newline" />
        </layout>
    </appender>
    <root>
        <level value="INFO" />
        <appender-ref ref="RollingFileAppender" />
    </root>
</log4net>
```

This ensures logs are stored in Logs/CustomerApp.log, which can be useful for troubleshooting.

# Database Setup

## 1. Create Database and Table

Run the `setup.sql` script inside the `Database` folder to create the required database and table.

## 2. Update Database using Entity Framework (optional)

Since this is using **Entity Framework Code First Migrations**, run the following command in the **Package Manager Console**:

```bash
Update-Database
```

This will apply the latest migrations and create the necessary tables in the database.

# Running the Application

1. **Restore NuGet Packages** (if necessary):

   ```bash
   dotnet restore
   ```

2. **Build the Solution** (Ctrl + Shift + B in Visual Studio).

3. **Run the Application** (F5 in Visual Studio).

---

# Troubleshooting

- If you encounter database connection issues, ensure your SQL Server instance is running and the connection string is correct.

- Check the Logs/CustomerApp.log file for any errors logged by log4net.

- Ensure all necessary NuGet packages are installed before building.

---

# Notes

- This system supports Entity Framework (EF6) as the ORM for database interactions.

- log4net is used for logging errors and activities.

- The App.config file contains all configurable settings required for the application to run successfully.

---



# Author

Glaiza Loren S. Malit
