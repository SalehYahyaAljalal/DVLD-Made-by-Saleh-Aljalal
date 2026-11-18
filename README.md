DVLD is a desktop application designed to simulate and manage the core operations of a Driving & Vehicle License Department.

The system provides an integrated environment for managing people, users, drivers, driving license applications, tests, local and international licenses, detained licenses, and other related workflows.

The project was built with a strong focus on:

Clean separation of responsibilities
Layered application architecture
Reusable business logic
Database-driven operations
Role-based access control
Maintainable and scalable code structure
Real-world business workflows

Project Goals

The main goal of this project is to build a complete desktop management system that demonstrates how a real-world business application can be structured using C# and SQL Server.

The project focuses not only on implementing CRUD operations, but also on modeling relationships between different business entities and handling complete workflows such as:

Person → Application → Tests → License → License History

Features

People Management

Manage the people registered within the system.

Add new people
Update existing people
Delete people
Search and filter people
View detailed person information
Store personal information and profile images
Reusable person information controls

User Management

Manage system users and their access.

User authentication
Add new users
Update users
Delete users
View user information
Change password
Sign out
Permission-based menu visibility

The application dynamically controls access to different modules based on the current user's permissions.

Driver Management

Manage drivers registered in the system.

View drivers
Search drivers
Display driver information
View driver's license history
Connect drivers with their related applications and licenses

Applications Management

The system supports multiple types of license-related applications.

Local Driving License Applications

Create new applications
View existing applications
Manage application status
Schedule required tests
Retake failed tests
Complete the licensing workflow

International License Applications

Create international license applications
View international applications
Manage international license information
Connect international licenses with existing driver records

Other Applications

The system also supports workflows for:

License renewal
Lost license replacement
Damaged license replacement
Detained license release

Tests Management

The application manages the testing process required for driving licenses.

Supported test types include:

Vision Test
Written Test
Practical Driving Test

The system allows:

Scheduling test appointments
Taking tests
Recording test results
Retaking failed tests
Managing test types

License Management

Manage different types of driving licenses.

Local Licenses

Issue a license for the first time
Display license information
View license history
Detain licenses
Release detained licenses

International Licenses

Issue international licenses
View international license information
Manage international license applications

Authentication 

The application includes an authentication and authorization mechanism.

Authentication

Users must log in before accessing the system.

The currently authenticated user is maintained throughout the application session.

