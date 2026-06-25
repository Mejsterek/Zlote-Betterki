# Złote Betterki

Złote Betterki is a Razor Pages voting application for browsing media entries and casting votes across multiple categories.

## Overview

The app provides session-based login, category browsing, per-category voting, and an admin view for reviewing vote totals.

## Features

- Session-based voter login
- Admin panel for inspecting vote totals
- Support for video and image entries
- Per-category vote tracking
- 30-minute session timeout
- Responsive UI built with Bootstrap 5

## Tech Stack

- ASP.NET Core 10.0
- Razor Pages
- Bootstrap 5
- JavaScript
- File-based storage for votes and access lists

## Project Structure

```text
Zlote-Betterki/
|-- Pages/
|   |-- Index.cshtml
|   |-- Index.cshtml.cs
|   |-- Admin.cshtml
|   |-- Admin.cshtml.cs
|   `-- Shared/
|       `-- _Layout.cshtml
|-- Models/
|   `-- KlipModel.cs
|-- wwwroot/
|   |-- css/
|   |-- js/
|   |-- lib/
|   |-- klipy/
|   `-- Zote_Betterki.png
|-- Program.cs
|-- Betterki.csproj
`-- README.md
```

## Requirements

- .NET 10.0 SDK

## Run Locally

```bash
dotnet restore
dotnet build
dotnet run
```

## Data Files

- `funkcjonariusze.txt` - authorized voters, one per line
- `baza.txt` - vote counts stored as JSON
- `glosy.txt` - individual vote records
- `keys/` - ASP.NET Core Data Protection keys

## Local Configuration

Place media files under `wwwroot/klipy/` in category folders. The app scans those folders at runtime and lists every supported `.mp4`, `.png`, `.jpg`, and `.jpeg` file it finds.

## Notes

- The project is stored in UTF-8, so Polish characters are preserved correctly.
- Build output, IDE metadata, and generated key material are ignored by git.
