# MAUI Products Manager

A modern, cross-platform mobile application built with .NET MAUI for managing products with full CRUD operations and CSV export functionality.

## 🚀 Features

- **Product Management**
  - Add new products with name, description, price, and category
  - Edit existing product information
  - Delete products with confirmation
  - View product details

- **Modern UI**
  - Swipe gestures for quick actions (Edit/Delete)
  - Context menu for additional options
  - Clean and intuitive interface
  - Responsive design for all screen sizes

- **Data Export**
  - Export products to CSV format
  - Share functionality for easy file saving
  - Proper CSV formatting with field escaping

- **Cross-Platform**
  - Works on Android, iOS, Windows, and macOS
  - Native performance on each platform

## 🛠️ Technology Stack

- **.NET MAUI** - Cross-platform framework
- **C#** - Programming language
- **XAML** - UI markup
- **MVVM Pattern** - Architecture
- **ObservableCollection** - Data binding

## 📋 Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) with MAUI workload
- Or [Visual Studio Code](https://code.visualstudio.com/) with C# extensions

## 🏃‍♂️ Getting Started

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/maui-products-manager.git
   cd maui-products-manager
Restore dependencies

bash
dotnet restore
Build the project

bash
dotnet build
Run the application

bash
# For Android
dotnet run --framework net8.0-android

# For iOS (requires Mac)
dotnet run --framework net8.0-ios

# For Windows
dotnet run --framework net8.0-windows10.0.19041.0
📱 Platform-Specific Setup
Android
Enable Developer Options on your device

Enable USB Debugging

Connect device via USB or use Android Emulator

iOS
Requires Mac with Xcode for development

For testing on device: Free Apple Developer account or AltStore

Windows
No additional setup required

Works on Windows 10/11

🎯 Usage
Adding a Product
Tap the "Add Product" button

Fill in product details (Name, Description, Price, Category)

Tap "Save" to add the product

Editing a Product
Swipe left on any product and tap "Edit"

Or tap the menu button (⋮) and select "Edit"

Modify the product information and save changes

Deleting a Product
Swipe left and tap "Delete"

Or use the menu button (⋮) and select "Delete"

Confirm deletion in the dialog

Exporting Data
Tap the "Export CSV" button

Choose save location through share dialog

CSV file will be generated with all products
