# Złote Betterki - Voting Application

A modern ASP.NET Core Razor Pages web application for managing and voting on video clips in multiple categories.

## Overview

Złote Betterki (Golden Betterki) is an interactive voting platform designed for organizing and showcasing content across various categories. Users can browse video clips and images, and cast their votes for their favorite entries.

## Features

- **User Authentication**: Secure login system with password protection
- **Category Management**: Organize content into multiple voting categories
- **Multi-Media Support**: Support for video (MP4) and image (PNG, JPG) files
- **Voting System**: Track user votes and prevent duplicate votes in the same category
- **Session Management**: Persistent user sessions with 30-minute timeout
- **Responsive Design**: Mobile-friendly interface with Bootstrap 5
- **Golden Theme**: Custom styling with elegant gold and dark green color scheme

## Tech Stack

- **Framework**: ASP.NET Core 8.0
- **UI**: Razor Pages with Bootstrap 5
- **Frontend**: jQuery, JavaScript
- **Storage**: File-based (TXT, JSON)
- **Authentication**: Session-based

## Project Structure

```
Zlote-Betterki/
├── Pages/
│   ├── Index.cshtml              # Main voting page
│   ├── Index.cshtml.cs           # Voting logic
│   ├── Admin.cshtml              # Admin panel
│   └── Shared/
│       └── _Layout.cshtml        # Master layout
├── Models/
│   └── KlipModel.cs              # Video clip model
├── wwwroot/
│   ├── css/                       # Stylesheets
│   ├── js/                        # JavaScript files
│   ├── lib/                       # Third-party libraries
│   ├── klipy/                     # Video and image storage
│   └── Zote_Betterki.png         # Logo image
├── Properties/
│   └── launchSettings.json       # Launch configuration
├── Program.cs                     # Application entry point
└── Betterki.csproj               # Project file
```

## Installation

### Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022 or Visual Studio Code

### Setup Steps

1. **Clone the repository**
   ```bash
   git clone https://github.com/Mejsterek/Zlote-Betterki.git
   cd Zlote-Betterki
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Create required files**
   - `funkcjonariusze.txt`: List of authorized users (one per line, format: "First Last")
   - `wwwroot/klipy/`: Create subdirectories for each voting category and add media files

4. **Build the project**
   ```bash
   dotnet build
   ```

5. **Run the application**
   ```bash
   dotnet run
   ```

   The application will be available at `http://localhost:5100`

## Configuration

### User Management

Edit `funkcjonariusze.txt` to add authorized voters:
```
John Smith
Jane Doe
Bob Johnson
```

### Category Setup

Create subdirectories in `wwwroot/klipy/` for each voting category:
```
wwwroot/klipy/
├── Best Moment/
│   ├── clip1.mp4
│   └── image1.png
├── Best Pilot/
│   ├── clip2.mp4
│   └── image2.jpg
└── Best Chase/
	└── clip3.mp4
```

### Login

- **Default Password**: K@chamyPani04
- Users must be listed in `funkcjonariusze.txt` to access voting

## Security Notes

⚠️ **Important**: This is a demonstration application. For production use:
- Replace hardcoded password with environment variables
- Implement proper database authentication
- Use HTTPS only
- Add CORS configuration if needed
- Implement proper authorization middleware

## Data Storage

The application stores data in plain text files:

- `funkcjonariusze.txt`: Authorized users list
- `baza.txt`: Vote counts per clip (JSON format)
- `glosy.txt`: Individual vote records (format: `username::category::clip`)
- `keys/`: Data protection keys

## Development

### Building

```bash
dotnet build
```

### Running in Debug Mode

```bash
dotnet run
```

Press `Ctrl+C` to stop the application.

### Running Tests

```bash
dotnet test
```

### Publishing

```bash
dotnet publish -c Release
```

## Known Issues

- Nullable warning messages in build (non-breaking)
- File-based storage not suitable for high-concurrency scenarios
- No built-in admin user management interface

## Future Enhancements

- [ ] Database integration (SQL Server/PostgreSQL)
- [ ] Modern admin dashboard
- [ ] Real-time vote updates
- [ ] User registration system
- [ ] Vote analytics and reports
- [ ] Multi-language support
- [ ] API endpoints for programmatic access

## Contributing

Contributions are welcome! Please feel free to submit pull requests or open issues for bugs and feature requests.

## Author

Created by Dawid "Mejster" Senouci

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

For issues and questions, please open an issue on GitHub:
[GitHub Issues](https://github.com/Mejsterek/Zlote-Betterki/issues)

## Changelog

### Version 1.0.0 (2026)

**Fixed**
- Static assets (CSS, JavaScript, images) loading correctly
- Logo sizing and positioning
- Asset path linking using relative URLs
- Web root path configuration for multiple execution contexts

**Added**
- Professional login form styling
- Responsive design for mobile devices
- Copyright year update

**Improved**
- Code cleanup and removed Polish comments
- CSS organization and consistency
- Documentation and README

---

**Last Updated**: 2026
**Status**: Active Development
