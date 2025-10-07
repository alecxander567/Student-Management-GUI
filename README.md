# Student Management System GUI

A comprehensive C# .NET WinForms application for managing students, instructors, classes, and assignments. This desktop application provides a modern, user-friendly interface with role-based access control and seamless integration with a RESTful API backend.

![.NET](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)
![Windows Forms](https://img.shields.io/badge/Windows%20Forms-0078D6?style=for-the-badge&logo=windows&logoColor=white)

## Features

### 🔐 Authentication & Authorization
- User registration with role selection (Student/Instructor)
- Secure login system with session management
- JWT or cookie-based authentication support
- Role-based UI and access control

### 👨‍🏫 Instructor Features
- **Class Management Dashboard**
  - View all assigned classes
  - Create, edit, and delete classes
  - Monitor class statistics (student count, assignments, schedules)
  
- **Assignment Management**
  - Create assignments with titles, instructions, and due dates
  - Edit existing assignments
  - Delete assignments with confirmation
  - View all assignments by class
  - Real-time assignment tracking

- **Student Management**
  - Add new students to classes
  - Edit student information (name, department, year level)
  - Remove students from classes
  - View comprehensive student lists with DataGridView
  - Track student demographics and academic details

- **Classroom Dashboard**
  - Overview cards showing:
    - Number of students enrolled
    - Total assignments for the class
    - Total classes managed
    - Class schedule (days and time)
  - Quick navigation between different views
  - Responsive layout with modern design

### 👨‍🎓 Student Features
- View enrolled classes
- Access class materials and assignments
- Track assignment due dates
- View class schedules

### 🎨 User Interface
- Modern, clean design using Siticone UI components
- Responsive layouts with dynamic sizing
- Color-coded elements for better UX:
  - MediumSeaGreen for primary actions
  - IndianRed for delete/warning actions
  - DodgerBlue for refresh actions
  - Goldenrod for edit actions
- Smooth navigation with sidebar menu
- Card-based information display
- Shadow effects and rounded corners for modern aesthetics

## Tech Stack

**Frontend:**
- C# .NET Framework/Core
- Windows Forms (WinForms)
- Siticone UI Controls
- Newtonsoft.Json for JSON serialization

**Backend Integration:**
- RESTful API communication via HttpClient
- Asynchronous operations for better performance
- CORS support for cross-origin requests

**Recommended Backend:**
- Django REST Framework
- .NET Core Web API
- Node.js/Express

## API Endpoints

### Authentication
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/signup/` | Register a new user |
| POST | `/api/login/` | User authentication |
| POST | `/api/logout/` | User logout |
| DELETE | `/api/delete_account/` | Delete user account |

### Classes
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/classes/` | Retrieve all classes |
| POST | `/api/add_class/` | Create a new class |
| POST | `/api/classes/edit/<id>/` | Edit class details |
| DELETE | `/api/classes/<id>/delete/` | Delete a class |
| GET | `/api/dashboard/` | Get dashboard statistics |

### Students
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/students/?class_id={id}` | Get students by class |
| POST | `/api/add_student/` | Add a new student |
| POST | `/api/edit_student/{id}/` | Update student information |
| POST | `/api/delete_student/{id}/` | Delete a student |

### Assignments
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/get_assignments/?class_id={id}` | Get assignments by class |
| POST | `/api/assignments/` | Create a new assignment |
| PUT | `/api/update_assignment/{id}/` | Update assignment details |
| DELETE | `/api/delete_assignment/{id}/` | Delete an assignment |

## Installation

### Prerequisites
- Visual Studio 2019 or later
- .NET Framework 4.7.2+ or .NET 6.0+
- Running backend API server (default: `http://localhost:8000`)

### Steps
1. Clone the repository:
```bash
git clone https://github.com/alecxander567/Student-Management-GUI.git
cd Student-Management-GUI
```

2. Open the solution in Visual Studio:
```bash
Student-Management-System.sln
```

3. Restore NuGet packages:
   - Newtonsoft.Json
   - Siticone.Desktop.UI

4. Update the API base URL in the code if needed (default: `http://localhost:8000`)

5. Build and run the application

## Configuration

### API Base URL
The application is configured to connect to `http://localhost:8000` by default. To change this:
1. Search for `http://localhost:8000` in the codebase
2. Replace with your backend API URL
3. Ensure CORS is properly configured on your backend

### Date Format
The application uses ISO 8601 format for date serialization:
```csharp
DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
```

## Project Structure

```
Student-Management-System/
├── Forms/
│   ├── frmLogin.cs              # Login form
│   ├── frmSignup.cs             # Registration form
│   ├── frmInstructorClasses.cs  # Instructor's class list
│   ├── frmClass.cs              # Individual class management
│   ├── frmAssignment.cs         # Assignment creation/editing
│   └── frmStudent.cs            # Student information form
├── Models/                      # Data models (if applicable)
├── Services/                    # API service classes
└── Resources/                   # Images, icons, etc.
```

## Key Features Implementation

### Dynamic Dashboard
The classroom dashboard dynamically loads:
- Real-time student count
- Assignment statistics
- Class schedule information
- Responsive box layout that adapts to window size

### CRUD Operations
All CRUD operations are implemented with:
- Async/await pattern for non-blocking UI
- Error handling and user feedback
- Confirmation dialogs for destructive actions
- Automatic view refresh after operations

### Form Validation
Input validation on all forms to ensure:
- Required fields are filled
- Valid date selections
- Proper data types

## Usage

### For Instructors
1. Login with instructor credentials
2. View all assigned classes on the dashboard
3. Click on a class to access:
   - **Dashboard**: Overview and assignment management
   - **Student List**: Add, edit, or remove students
   - **Settings**: Account management options
4. Create assignments with deadlines
5. Manage student enrollments

### For Students
1. Login with student credentials
2. View enrolled classes
3. Access assignments and due dates
4. Check class schedules

## Screenshots

*Add screenshots of your application here*

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the project
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## Future Enhancements

- [ ] Grade management system
- [ ] File upload for assignments
- [ ] Email notifications for due dates
- [ ] Export reports to PDF/Excel
- [ ] Dark mode support
- [ ] Multi-language support
- [ ] Calendar view for assignments
- [ ] Student performance analytics
- [ ] Discussion forums per class

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Author

**Alexander**
- GitHub: [@alecxander567](https://github.com/alecxander567)

## Acknowledgments

- Siticone UI for the modern WinForms controls
- Newtonsoft.Json for JSON handling
- The .NET community for excellent documentation

---

**Note:** Make sure your backend API is running before starting the application. Default backend URL is `http://localhost:8000`
