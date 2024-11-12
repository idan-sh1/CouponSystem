# CouponSystem
A web-based application designed to manage coupons, with a user authentication and authorization system for secure access.

## Features

- **User Management**: Admin users can register new users, manage user roles, and disable/enable them.
- **Coupon Management**: Admin users can add, edit, delete, and show coupons data. Normal signed in users can also add coupons.
- **Role-Based Access**: Different access levels for users and admins.
- **JWT Authentication**: Secure API endpoints with JSON Web Tokens (JWT).
- **Use Coupons**: Everyone can use coupon (if they have the code) and see the new price after discount.

## Technologies Used

- **ASP.NET Core Web API** (.NET 8)
- **Entity Framework Core** for data access
- **SQL Server** for the database
- **JWT** for secure token-based authentication
- **Swagger** for API documentation

## Getting Started

### Prerequisites

- [.NET 8](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

### Setup

1. **Clone the repository**:

    ```bash
    git clone https://github.com/your-username/your-repo-name.git
    cd your-repo-name
    ```

2. **Configure Database**:
    - Update the `appsettings.json` file with your SQL Server connection string.

3. **Run Migrations**:
    - Apply migrations to your database:

    ```bash
    dotnet ef database update
    ```

4. **Seed Initial Data**:
    - Seed the database with roles and an admin user to get started.

5. **Run the Application**:
    - Start the application:

    ```bash
    dotnet run
    ```

6. **Access API Documentation**:
    - Explore the API at [http://localhost:5000/swagger](http://localhost:5000/swagger).

## Usage

### Admin Features
- **Register Users**: Only admins can register new users.
- **Manage Coupons**: Add, edit, delete, and show coupons data.
- **Manage Roles**: Admins can manage user roles and permissions.

### Signed-in User Features
- **Add Coupons**: Users can add coupons.

### Anonymous User Features
- **Apply Coupons**: Everyone can apply coupons they are eligible for.

## License

This project is licensed under the MIT License.
