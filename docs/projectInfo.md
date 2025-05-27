# CredWise Customer Management System

## Project Overview
CredWise Customer Management System is a .NET 8.0 based application that provides comprehensive user management functionality with secure authentication and authorization features.

## Project Structure
The solution is organized into multiple projects following Clean Architecture principles:

- **CredWiseCustomer.Api**: Web API project containing controllers and API endpoints
- **CredWiseCustomer.Application**: Application layer containing DTOs, interfaces, and business logic
- **CredWiseCustomer.Core**: Core domain entities and interfaces
- **CredWiseCustomer.Infrastructure**: Data access and external service implementations
- **CredWise.Utills**: Utility classes and helper functions
- **Authentication.Service**: Provides JWT authentication and policy-based authorization services
- **JwtSampleGenerator**: Console app to generate valid JWT tokens for testing the API

## Authentication & Authorization

### JWT Authentication
- The API uses JWT Bearer authentication.
- Tokens are generated using the `Authentication.Service` project, specifically the `JwtAuthenticationService` class.
- The JWT configuration (secret, issuer, audience) is set in `appsettings.json` and must match the values used in token generation.

#### Example JWT Configuration (appsettings.json):
```json
"Jwt": {
  "SecretKey": "A7d!kL9zQ2x#Vb6pT4w@eR8uY1s$Jm5n",
  "Issuer": "your-issuer",
  "Audience": "your-audience",
  "TokenExpirationInHours": 1
}
```

#### Claims Required for Admin Access
- `role`: Must be `"Admin"` (case-sensitive)
- `unique_name`: User's email
- `nameid`: User's ID

### Authorization
- Admin endpoints require the `role` claim to be `"Admin"`.
- Policies are defined in `Program.cs` using `RequireClaim("role", "Admin")`.
- The `[Authorize(Policy = "Admin")]` attribute is used on protected endpoints.

## How to Generate a JWT for Testing

1. **Build the Solution**
   - Ensure all projects build successfully. If you see errors about missing types, make sure `JwtSampleGenerator` references `Authentication.Service` and has the correct `using` statements.

2. **Run JwtSampleGenerator**
   - This console app will print a valid JWT token for an admin user.
   - Example usage:
     ```sh
     dotnet run --project JwtSampleGenerator
     ```
   - Output:
     ```
     Generated JWT token:
     <token>
     
     Use this token in your API requests as follows:
     Authorization: Bearer <token>
     ```

3. **Use the Token in API Requests**
   - Add the following header to your requests:
     ```
     Authorization: Bearer <token>
     ```
   - This is required for all protected endpoints.

## Troubleshooting Authentication & Authorization

### Common Issues
- **401 Unauthorized**: Token is missing, expired, invalid, or the `Bearer` prefix is missing in the header.
- **403 Forbidden**: Token is valid, but the user does not have the required role/claim for the endpoint.

### Debugging Steps
1. **Check the Authorization Header**
   - Must be exactly: `Authorization: Bearer <token>`
2. **Check JWT Claims**
   - Decode your JWT at [jwt.io](https://jwt.io/) and verify the `role` claim is exactly `"Admin"`.
3. **Check Policy and Controller Attributes**
   - Policy in `Program.cs` should be:
     ```csharp
     options.AddPolicy("Admin", policy => policy.RequireClaim("role", "Admin"));
     ```
   - Controller endpoints should use `[Authorize(Policy = "Admin")]`.
4. **Check Database User Role**
   - The `Role` column for your admin user in the database must be exactly `Admin` (case-sensitive, no spaces, not encoded).
   - Example SQL to fix:
     ```sql
     UPDATE Users SET Role = 'Admin' WHERE Email = 'admin@credwise.com';
     ```
5. **Check API Console Output**
   - Look for debug lines about claims and role value during authorization.
6. **Regenerate and Use a New Token**
   - After fixing the DB, log in again as admin to get a new token.

### Example: Generating and Using a Token

1. **Generate Token**
   - Run:
     ```sh
     dotnet run --project JwtSampleGenerator
     ```
   - Copy the output token.

2. **Use in API Request**
   - Example with curl:
     ```sh
     curl -H "Authorization: Bearer <token>" https://localhost:7037/api/User
     ```

## Common Mistakes
- Using a Base64-encoded or lowercase role value in the token or database (must be `Admin`).
- Not updating the token after fixing the database or code.
- Not using the `Bearer` prefix in the Authorization header.
- Policy and claim mismatch (policy expects `Admin`, token has something else).

## API Endpoints and Usage

### User Management
1. **Create User**
   ```http
   POST /api/User
   Content-Type: application/json
   {
     "email": "user@credwise.com",
     "password": "StrongP@ss123",
     "firstName": "John",
     "lastName": "Doe",
     "phoneNumber": "9876543210"
   }
   ```
2. **Get User by ID**
   ```http
   GET /api/User/{id}
   ```
3. **Get User by Email**
   ```http
   GET /api/User/email/{email}
   ```
4. **Get All Users**
   ```http
   GET /api/User
   ```
5. **Update User**
   ```http
   PUT /api/User/{id}
   Content-Type: application/json
   {
     "firstName": "Updated",
     "lastName": "Name",
     "phoneNumber": "9876543210",
     "isActive": true
   }
   ```
6. **Soft Delete User**
   ```http
   DELETE /api/User/{id}
   ```
7. **Restore User**
   ```http
   POST /api/User/{id}/restore
   ```

## Development Environment
- .NET 8.0
- Development server: http://localhost:5279
- Environment: Development

## Dependencies
- FluentValidation.AspNetCore (11.3.0)
- Swashbuckle.AspNetCore (6.6.2)
- AutoMapper
- Entity Framework Core
- Microsoft.IdentityModel.Tokens (7.3.1)
- System.IdentityModel.Tokens.Jwt (7.3.1)

## Future Improvements
1. **Global Exception Handling**
   - Add global exception handler middleware
   - Implement comprehensive logging
2. **Security Enhancements**
   - Implement rate limiting
   - Add request validation for SQL injection prevention
   - Configure CORS policies
3. **Documentation**
   - Add XML documentation for API endpoints
   - Enhance Swagger documentation
   - Add API versioning
4. **Testing**
   - Add unit tests
   - Add integration tests
   - Add API tests

## Getting Started
1. Clone the repository
2. Restore NuGet packages
3. Update database connection string in appsettings.json
4. Run database migrations
5. Start the application using:
   ```bash
   dotnet run --project CredWiseCustomer.Api/CredWiseCustomer.Api.csproj
   ```
6. Access the API at http://localhost:5279
7. Access Swagger documentation at http://localhost:5279/swagger
