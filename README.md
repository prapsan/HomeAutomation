# Home Automation API

## Overview

Welcome to the Home Automation API! This API allows you to manage and control various home automation devices.

## Getting Started

To use the API, follow these steps:

### Prerequisites

Make sure you have the following installed:

- [.NET SDK](https://dotnet.microsoft.com/download)

### Clone the Repository

```bash
git clone https://github.com/prapsan/HomeAutomation.git          
```


## API Endpoints

The API will be hosted at http://localhost:5062

### Get Devices

Retrieve a list of all devices.

```bash
GET /api/Devices
```

### Get Device by ID

Retrieve details of a specific device by ID.

```bash
GET /api/Device/{id}
```

### Register a Device

Register a new device.

```bash
POST /api/Devices
{
  "id": "b86f2096-237a-4059-8329-1bbcea72769b",
  "description": "Garage Door",
  "type": 1,
  "address": "192.168.100.1",
  "isOn": false
}
```

### Remove a Device

Remove a device by ID.

```bash
DELETE /api/Device/{id}
```

### Trigger a Device

Trigger a device by ID.

```bash
POST /api/Device/{id}
```

### Undo Last Action

Undo the last action

```bash
POST /api/Devices/undo
```







