# 🏢 Employee Management System

A comprehensive web-based Employee Management System built with **ASP.NET Core MVC** that streamlines HR operations, attendance tracking, payroll management, and employee communications.

![.NET Core](https://img.shields.io/badge/.NET-7.0-blue)
![License](https://img.shields.io/badge/license-MIT-green)
![Status](https://img.shields.io/badge/status-active-success)

## 📸 Screenshots

### Admin Dashboard
<img width="1915" height="993" alt="Screenshot 2025-11-09 161323" src="https://github.com/user-attachments/assets/22fbca0b-9df7-4381-bf6c-dedc8b0e9e45" />


<img width="1919" height="992" alt="Screenshot 2025-11-09 161352" src="https://github.com/user-attachments/assets/1738bceb-3767-4dbe-a152-5ff725f75735" />
<img width="1919" height="992" alt="Screenshot 2025-11-09 161413" src="https://github.com/user-attachments/assets/f5b2364f-b46a-4883-be58-455851283b67" />
<img width="1919" height="995" alt="Screenshot 2025-11-09 161434" src="https://github.com/user-attachments/assets/94db4f67-dd70-4c15-9a85-793cc8f0ecfc" />
<img width="1919" height="993" alt="Screenshot 2025-11-09 161454" src="https://github.com/user-attachments/assets/ab397d16-97c6-46a9-b7a5-0b74835f597e" />
<img width="1919" height="993" alt="Screenshot 2025-11-09 161516" src="https://github.com/user-attachments/assets/a495ce8b-8f4f-4859-b11a-e898917fefe2" />
<img width="1919" height="990" alt="Screenshot 2025-11-09 161537" src="https://github.com/user-attachments/assets/e97bd5e1-7cf5-4637-be8d-dfd2e90fef08" />
<img width="1919" height="992" alt="Screenshot 2025-11-09 161554" src="https://github.com/user-attachments/assets/e6ba35f5-aec6-46e0-a8b4-c7e7a8dd7267" />
<img width="1919" height="994" alt="Screenshot 2025-11-09 161609" src="https://github.com/user-attachments/assets/c4617f0c-8890-48a2-99fd-9f17aca340ab" />
<img width="1919" height="993" alt="Screenshot 2025-11-09 161624" src="https://github.com/user-attachments/assets/c2ffd21f-ba94-4e0d-8cc0-65982aa7fe89" />
<img width="1919" height="993" alt="Screenshot 2025-11-09 161645" src="https://github.com/user-attachments/assets/544ae15e-e1c9-4968-b985-ccc992e10ddc" />
<img width="1919" height="993" alt="Screenshot 2025-11-09 161721" src="https://github.com/user-attachments/assets/f232b0e2-e0b4-455b-a48b-bff549214586" />
<img width="1919" height="993" alt="Screenshot 2025-11-09 161743" src="https://github.com/user-attachments/assets/efe7f34b-85a1-4163-98db-e0172e6860e7" />
<img width="1919" height="991" alt="Screenshot 2025-11-09 161806" src="https://github.com/user-attachments/assets/ab185f73-61ea-4c33-843c-1bf5f3ffbed5" />
<img width="1919" height="990" alt="Screenshot 2025-11-09 161824" src="https://github.com/user-attachments/assets/f3131e17-1c81-4010-a2a9-a20fd514ae4c" />


  
---

## 🌟 Features

### 👨‍💼 Admin Features
- **Employee Management**
  - Add, edit, view, and delete employees
  - Comprehensive employee profiles with all details
  - Department-wise employee organization
  
- **Attendance Management**
  - Mark daily attendance (Present/Absent/Leave)
  - View attendance history with filters
  - Generate attendance reports
  
- **Salary & Bonus Management**
  - Manage employee bonuses
  - Track salary components
  - Multiple bonus types support
  
- **Payslip Generation**
  - Auto-calculate net salary
  - Generate monthly payslips
  - Download/print payslips
  
- **Feedback Management**
  - View employee feedbacks
  - Respond to feedback requests
  - Track feedback status
  
- **Reports & Analytics**
  - Employee overview reports
  - Monthly attendance reports
  - Salary distribution reports
  - Dashboard with key statistics

### 👨‍💻 Employee Features
- **Personal Dashboard**
  - View personal statistics
  - Quick access to important information
  
- **Profile Management**
  - View and edit personal profile
  - Change password securely
  
- **Salary Information**
  - View monthly salary details
  - Access salary history
  - Download payslips
  
- **Attendance Records**
  - View personal attendance history
  - Check attendance statistics
  
- **Feedback System**
  - Submit feedback/complaints
  - Track feedback status
  - View admin responses

---

## 🛠️ Technology Stack

| Category | Technology |
|----------|-----------|
| **Framework** | ASP.NET Core MVC 7.0 |
| **Language** | C# 11 |
| **Database** | SQL Server 2019+ |
| **ORM** | Entity Framework Core 7.0 |
| **Frontend** | HTML5, CSS3, JavaScript |
| **CSS Framework** | Bootstrap 5.3 |
| **Icons** | Font Awesome 6.4 |
| **Fonts** | Google Fonts (Poppins) |
| **Alerts** | SweetAlert2 |
| **IDE** | Visual Studio 2022 |

---

## 📋 Prerequisites

Before you begin, ensure you have the following installed:

- [.NET 7.0 SDK](https://dotnet.microsoft.com/download/dotnet/7.0) or later
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (Community Edition or higher)
- [SQL Server 2019](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or later
- [SQL Server Management Studio (SSMS)](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)

---

## 🚀 Installation & Setup

### Step 1: Clone the Repository

```bash
git clone https://github.com/yourusername/employee-management-system.git
cd employee-management-system
```

### Step 2: Configure Database Connection

1. Open the project in Visual Studio 2022
2. Navigate to `appsettings.json`
3. Update the connection string with your SQL Server details:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=EmployeeManagementDB;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;"
  }
}
```

**Common Server Names:**
- `.\SQLEXPRESS` (SQL Server Express)
- `localhost`
- `(localdb)\MSSQLLocalDB` (LocalDB)

### Step 3: Install NuGet Packages

Open **Package Manager Console** (Tools → NuGet Package Manager → Package Manager Console) and run:

```powershell
Install-Package Microsoft.EntityFrameworkCore -Version 7.0.0
Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 7.0.0
Install-Package Microsoft.EntityFrameworkCore.Tools -Version 7.0.0
```

Or restore packages using:
```bash
dotnet restore
```

### Step 4: Create Database

In **Package Manager Console**, run:

```powershell
Add-Migration InitialCreate
Update-Database
```

Or using .NET CLI:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Step 5: Run the Application

Press `F5` in Visual Studio or run:

```bash
dotnet run
```

The application will start at: `https://localhost:7xxx`

---

## 🔐 Default Login Credentials

### Admin Account
- **Email:** admin@company.com
- **Password:** Admin@123

> ⚠️ **Security Note:** Change the default admin password after first login!

---

## 📊 Database Schema

### Tables Created:
1. **Employees** - Stores employee information
2. **Attendances** - Daily attendance records
3. **SalaryBonuses** - Bonus information
4. **Payslips** - Generated salary slips
5. **Feedbacks** - Employee feedback submissions

### Entity Relationships:
```
Employees (1) ──→ (Many) Attendances
Employees (1) ──→ (Many) SalaryBonuses
Employees (1) ──→ (Many) Payslips
Employees (1) ──→ (Many) Feedbacks
```

---

## 📁 Project Structure

```
EmployeeManagementSystem/
│
├── Controllers/              # Backend Logic
│   ├── AccountController.cs
│   ├── AdminController.cs
│   ├── AttendanceController.cs
│   ├── SalaryBonusController.cs
│   ├── PayslipController.cs
│   ├── FeedbackController.cs
│   ├── EmployeeController.cs
│   └── ReportController.cs
│
├── Models/                   # Data Models
│   ├── Employee.cs
│   ├── Attendance.cs
│   ├── SalaryBonus.cs
│   ├── Payslip.cs
│   └── Feedback.cs
│
├── Views/                    # UI Pages
│   ├── Account/
│   ├── Admin/
│   ├── Attendance/
│   ├── SalaryBonus/
│   ├── Payslip/
│   ├── Feedback/
│   ├── Employee/
│   ├── Report/
│   └── Shared/
│
├── Data/
│   └── ApplicationDbContext.cs
│
├── Helpers/
│   └── SessionHelper.cs
│
├── wwwroot/
│   ├── css/
│   └── js/
│
├── Program.cs
└── appsettings.json
```

---

## 🎨 UI Features

- **Modern Gradient Design** - Beautiful purple/blue color scheme
- **Responsive Layout** - Works on desktop, tablet, and mobile
- **Smooth Animations** - Enhanced user experience with transitions
- **Icon Integration** - Font Awesome icons throughout
- **Alert Notifications** - SweetAlert2 for user feedback
- **Form Validation** - Client and server-side validation

---

## 🔧 Configuration

### Session Management
Sessions are configured with 30-minute timeout:
```csharp
options.IdleTimeout = TimeSpan.FromMinutes(30);
```

### Database Connection
The system uses SQL Server with Entity Framework Core:
- **Connection pooling** enabled
- **Cascade delete** configured for related entities
- **Decimal precision** set to (18,2) for currency fields

---

## 🐛 Troubleshooting

### Issue: Database Connection Failed
**Solution:** 
1. Verify SQL Server is running
2. Check server name in connection string
3. Ensure SQL Server accepts TCP/IP connections

### Issue: Migration Errors
**Solution:**
```powershell
Remove-Migration
Add-Migration InitialCreate
Update-Database
```

### Issue: Views Not Found
**Solution:**
1. Ensure Views folder is included in project
2. Check view file names match action names
3. Rebuild solution

---

## 📝 Usage Guide

### For Administrators

1. **Login** with admin credentials
2. **Dashboard** shows overview statistics
3. **Manage Employees**
   - Click "Employees" in navbar
   - Use "Add Employee" button to create new employee
   - Edit/Delete using action buttons
4. **Mark Attendance**
   - Navigate to Attendance section
   - Select date and employee
   - Mark status (Present/Absent/Leave)
5. **Process Salary**
   - Add bonuses in Salary Bonus section
   - Generate payslips from Payslip section
6. **View Reports**
   - Access Reports section from navbar
   - Select report type
   - Apply filters and generate

### For Employees

1. **Login** with provided credentials
2. **Dashboard** shows personal overview
3. **View Profile** - Check personal information
4. **Check Salary** - View monthly salary details
5. **View Attendance** - Check attendance history
6. **Submit Feedback** - Communicate with management

---

## 🔒 Security Features

- ✅ Password hashing (implement bcrypt for production)
- ✅ Session-based authentication
- ✅ Anti-forgery tokens on forms
- ✅ SQL injection prevention via EF Core
- ✅ Role-based access control
- ✅ XSS protection

> ⚠️ **Note:** For production deployment, implement additional security measures like:
> - Password hashing with BCrypt
> - HTTPS enforcement
> - Rate limiting
> - Email verification
> - Two-factor authentication

---

## 🚀 Future Enhancements

- [ ] Email notifications for payslips
- [ ] Leave management system
- [ ] Performance appraisal module
- [ ] Export reports to PDF/Excel
- [ ] Employee self-service portal
- [ ] Mobile application
- [ ] Biometric attendance integration
- [ ] Document management
- [ ] Training management
- [ ] Asset management

---

## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 👨‍💻 Author

Farhan Sargath
- GitHub: [@Hyphenat]
- LinkedIn: [FARHAN SARGATH](https://www.linkedin.com/in/farhan-sargath-646b06292/)
- Email: fsargath@gmail.com

---


## 📞 Support

If you encounter any issues or have questions:

1. Check the [Troubleshooting](#-troubleshooting) section
2. Review [closed issues](https://github.com/yourusername/employee-management-system/issues?q=is%3Aissue+is%3Aclosed)
3. Open a [new issue](https://github.com/yourusername/employee-management-system/issues/new)

---

## ⭐ Show Your Support

If you find this project helpful, please give it a ⭐ on GitHub!

---

**Made with ❤️ using ASP.NET Core MVC**
