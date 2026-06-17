# Odense Mobil Reparation

A desktop application for managing customers and repair orders in a mobile phone repair shop.

> Currently only Windows is supported.

## Features

### Customer Management

- Create new customers
- Edit existing customer information
- Delete customers
- Sort customer lists by clicking column headers
- Search customers by:
  - Name
  - Phone number
  - Email address
- Optional fuzzy search support
- View all orders belonging to a customer
- Create new orders directly from a customer page

### Order Management

- Create new repair orders
- Edit existing repair orders
- Delete repair orders
- Sort order lists by clicking column headers
- Assign orders to customers
- Track:
  - Device handed in
  - Repair description
  - Hand-in date and time
  - Return date and time
  - Borrowed phone information
  - Completion status
- Search orders by:
  - Device handed in
  - Repair description
  - Customer name
  - Borrowed phone
- Filter orders by:
  - Completion status
  - Hand-in date range
  - Return date range
- Optional fuzzy search support

### PDF Report Generation

- Generate printable PDF reports for repair orders
- Include company logo
- Include company contact information
- Include customer information
- Include repair order details
- Reports are stored automatically in the Application Data folder

### Data Storage

- Local SQLite database
- No internet connection required
- Data persists between application restarts
- Easy migration between devices

### Logging

- Application log files are automatically created
- Logs are stored in the Application Data folder
- Useful for troubleshooting and support

---

## Installation

### Download

Download the latest release from:

<https://github.com/andr9528/Odense-Mobil-Reparation/releases>

### First Launch

1. Download the latest release.
2. Extract the files if required.
3. Run the application.
4. Open the **Application Data** folder using the button in the navigation menu.
5. Open the file:

```text
appsettings.json
```

6. Configure the report settings:

```json
{
  "ReportData": {
    "LogoPath": "dummy-logo.png",
    "CompanyWebsiteUrl": "<https://example.com>",
    "CompanyAddress": "Example Street 1",
    "CompanyPhoneNumber": "+45 12345678",
    "CompanyEmail": "<info@example.com>"
  }
}
```

7. Save the file.
8. Restart the application.

The configured values will be used when generating PDF reports.

---

## Application Data Folder

The application automatically creates an Application Data folder containing:

```text
appsettings.json
repair.db
README.md
Logs/
Reports/
```

### Contents

| Item | Description |
|------|-------------|
| appsettings.json | Application configuration |
| repair.db | SQLite database containing all customers and orders |
| README.md | Local copy of this documentation |
| Logs | Application log files |
| Reports | Generated PDF reports |

---

## Moving Data Between Devices

To move all customer and repair data to another computer:

1. Install the application on the new device.
2. Launch it once so the Application Data folder is created.
3. Close the application.
4. Copy the following file from the old device:

```text
repair.db
```

1. Replace the database file on the new device.
2. Start the application.

All customers, orders and repair history will now be available on the new device.

Optionally copy:

```text
appsettings.json
```

to transfer report configuration as well.

---

## Technology Stack

### Application

- .NET 10
- Uno Platform
- WinUI
- SQLite
- Entity Framework Core
- QuestPDF
- Serilog

### Architecture

- Dependency Injection
- Entity Framework Core
- SQLite persistence
- Service-based architecture
- MVVM-inspired UI structure

---

## Third-Party Dependencies

### Runtime Dependencies

The following packages are required by the application during normal use.

| Package | License |
|----------|----------|
| Uno Platform | Apache License 2.0 |
| Uno Community Toolkit DataGrid | Apache License 2.0 |
| Entity Framework Core | MIT License |
| Microsoft.Extensions.* | MIT License |
| Newtonsoft.Json | MIT License |
| QuestPDF | MIT License |
| Serilog | Apache License 2.0 |
| Serilog.AspNetCore | Apache License 2.0 |
| Serilog.Extensions.Logging | Apache License 2.0 |
| Serilog.Sinks.Console | Apache License 2.0 |
| Serilog.Sinks.File | Apache License 2.0 |

### Development Dependencies

The following package is used during development and Debug builds to generate sample customers and repair orders for testing and demonstrations.

| Package | License |
|----------|----------|
| Bogus | MIT License |

### Test Dependencies

The following packages are used exclusively by the automated test suite and are not required for normal application operation.

| Package | License |
|----------|----------|
| FluentAssertions | Apache License 2.0 |
| Moq | BSD 3-Clause License |
| TUnit | MIT License |

For complete and up-to-date license information, please refer to the respective project repositories.

---

## System Requirements

### Supported Platforms

Currently only Windows is supported.

### Requirements

- Windows 10 or newer
- .NET Desktop Runtime, if not included with the release
- Write access to the user's Application Data folder

---

## Support

Generated reports, application logs and application data are all available through the Application Data folder accessible from within the application.
