# User Management API Documentation

This document provides information on how to use the User Management API for the frontend application.

## Overview

The User Management API is a SOAP-based web service that provides functionality for managing users, roles, client memberships, and department access. It allows the frontend application to perform operations such as creating users, assigning roles, managing client access, and controlling department permissions.

## Authentication

All API calls require authentication using a SOAP header. The header should contain the following information:

```xml
<CredentialsHeader xmlns="http://msiwebtrax.com/">
  <UserName>admin_username</UserName>
  <PWD>admin_password</PWD>
</CredentialsHeader>
```

The user making the API calls must have appropriate permissions (typically Administrator role) to perform certain operations.

## API Endpoints

### User Operations

#### GetAllUsers

Retrieves all users in the system.

**Request:**

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tem="http://msiwebtrax.com/">
  <soap:Header>
    <CredentialsHeader xmlns="http://msiwebtrax.com/">
      <UserName>admin_username</UserName>
      <PWD>admin_password</PWD>
    </CredentialsHeader>
  </soap:Header>
  <soap:Body>
    <tem:GetAllUsers />
  </soap:Body>
</soap:Envelope>
```

**Response:**

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <soap:Body>
    <GetAllUsersResponse xmlns="http://msiwebtrax.com/">
      <GetAllUsersResult>
        <UserInfo>
          <UserId>guid</UserId>
          <UserName>string</UserName>
          <Email>string</Email>
          <IsApproved>boolean</IsApproved>
          <IsLockedOut>boolean</IsLockedOut>
          <CreateDate>dateTime</CreateDate>
          <LastLoginDate>dateTime</LastLoginDate>
          <LastActivityDate>dateTime</LastActivityDate>
        </UserInfo>
        <!-- Additional UserInfo elements -->
      </GetAllUsersResult>
    </GetAllUsersResponse>
  </soap:Body>
</soap:Envelope>
```

#### GetUserByName

Retrieves a specific user by username.

**Request:**

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tem="http://msiwebtrax.com/">
  <soap:Header>
    <CredentialsHeader xmlns="http://msiwebtrax.com/">
      <UserName>admin_username</UserName>
      <PWD>admin_password</PWD>
    </CredentialsHeader>
  </soap:Header>
  <soap:Body>
    <tem:GetUserByName>
      <tem:username>target_username</tem:username>
    </tem:GetUserByName>
  </soap:Body>
</soap:Envelope>
```

**Response:**

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <soap:Body>
    <GetUserByNameResponse xmlns="http://msiwebtrax.com/">
      <GetUserByNameResult>
        <UserId>guid</UserId>
        <UserName>string</UserName>
        <Email>string</Email>
        <IsApproved>boolean</IsApproved>
        <IsLockedOut>boolean</IsLockedOut>
        <CreateDate>dateTime</CreateDate>
        <LastLoginDate>dateTime</LastLoginDate>
        <LastActivityDate>dateTime</LastActivityDate>
      </GetUserByNameResult>
    </GetUserByNameResponse>
  </soap:Body>
</soap:Envelope>
```

#### GetUserByEmail

Retrieves a specific user by email.

**Request:**

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tem="http://msiwebtrax.com/">
  <soap:Header>
    <CredentialsHeader xmlns="http://msiwebtrax.com/">
      <UserName>admin_username</UserName>
      <PWD>admin_password</PWD>
    </CredentialsHeader>
  </soap:Header>
  <soap:Body>
    <tem:GetUserByEmail>
      <tem:email>user@example.com</tem:email>
    </tem:GetUserByEmail>
  </soap:Body>
</soap:Envelope>
```

**Response:**

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <soap:Body>
    <GetUserByEmailResponse xmlns="http://msiwebtrax.com/">
      <GetUserByEmailResult>
        <UserId>guid</UserId>
        <UserName>string</UserName>
        <Email>string</Email>
        <IsApproved>boolean</IsApproved>
        <IsLockedOut>boolean</IsLockedOut>
        <CreateDate>dateTime</CreateDate>
        <LastLoginDate>dateTime</LastLoginDate>
        <LastActivityDate>dateTime</LastActivityDate>
      </GetUserByEmailResult>
    </GetUserByEmailResponse>
  </soap:Body>
</soap:Envelope>
```

#### CreateUser

Creates a new user.

**Request:**

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tem="http://msiwebtrax.com/">
  <soap:Header>
    <CredentialsHeader xmlns="http://msiwebtrax.com/">
      <UserName>admin_username</UserName>
      <PWD>admin_password</PWD>
    </CredentialsHeader>
  </soap:Header>
  <soap:Body>
    <tem:CreateUser>
      <tem:username>new_username</tem:username>
      <tem:password>new_password</tem:password>
      <tem:email>new_user@example.com</tem:email>
      <tem:isApproved>true</tem:isApproved>
    </tem:CreateUser>
  </soap:Body>
</soap:Envelope>
```

**Response:**

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <soap:Body>
    <CreateUserResponse xmlns="http://msiwebtrax.com/">
      <CreateUserResult>
        <Success>boolean</Success>
        <Message>string</Message>
        <ErrorCode>int</ErrorCode>
      </CreateUserResult>
    </CreateUserResponse>
  </soap:Body>
</soap:Envelope>
```

#### UpdateUser

Updates an existing user.

**Request:**

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tem="http://msiwebtrax.com/">
  <soap:Header>
    <CredentialsHeader xmlns="http://msiwebtrax.com/">
      <UserName>admin_username</UserName>
      <PWD>admin_password</PWD>
    </CredentialsHeader>
  </soap:Header>
  <soap:Body>
    <tem:UpdateUser>
      <tem:username>target_username</tem:username>
      <tem:email>updated_email@example.com</tem:email>
      <tem:isApproved>true</tem:isApproved>
    </tem:UpdateUser>
  </soap:Body>
</soap:Envelope>
```

**Response:**

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <soap:Body>
    <UpdateUserResponse xmlns="http://msiwebtrax.com/">
      <UpdateUserResult>
        <Success>boolean</Success>
        <Message>string</Message>
        <ErrorCode>int</ErrorCode>
      </UpdateUserResult>
    </UpdateUserResponse>
  </soap:Body>
</soap:Envelope>
```

#### DeleteUser

Deletes a user.

**Request:**

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tem="http://msiwebtrax.com/">
  <soap:Header>
    <CredentialsHeader xmlns="http://msiwebtrax.com/">
      <UserName>admin_username</UserName>
      <PWD>admin_password</PWD>
    </CredentialsHeader>
  </soap:Header>
  <soap:Body>
    <tem:DeleteUser>
      <tem:username>target_username</tem:username>
    </tem:DeleteUser>
  </soap:Body>
</soap:Envelope>
```

**Response:**

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <soap:Body>
    <DeleteUserResponse xmlns="http://msiwebtrax.com/">
      <DeleteUserResult>
        <Success>boolean</Success>
        <Message>string</Message>
        <ErrorCode>int</ErrorCode>
      </DeleteUserResult>
    </DeleteUserResponse>
  </soap:Body>
</soap:Envelope>
```

#### ResetUserPassword

Resets a user's password.

**Request:**

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tem="http://msiwebtrax.com/">
  <soap:Header>
    <CredentialsHeader xmlns="http://msiwebtrax.com/">
      <UserName>admin_username</UserName>
      <PWD>admin_password</PWD>
    </CredentialsHeader>
  </soap:Header>
  <soap:Body>
    <tem:ResetUserPassword>
      <tem:username>target_username</tem:username>
      <tem:newPassword>new_password</tem:newPassword>
    </tem:ResetUserPassword>
  </soap:Body>
</soap:Envelope>
```

**Response:**

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <soap:Body>
    <ResetUserPasswordResponse xmlns="http://msiwebtrax.com/">
      <ResetUserPasswordResult>
        <Success>boolean</Success>
        <Message>string</Message>
        <ErrorCode>int</ErrorCode>
      </ResetUserPasswordResult>
    </ResetUserPasswordResponse>
  </soap:Body>
</soap:Envelope>
```

### Role Operations

#### GetAllRoles

Retrieves all roles in the system.

**Request:**

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tem="http://msiwebtrax.com/">
  <soap:Header>
    <CredentialsHeader xmlns="http://msiwebtrax.com/">
      <UserName>admin_username</UserName>
      <PWD>admin_password</PWD>
    </CredentialsHeader>
  </soap:Header>
  <soap:Body>
    <tem:GetAllRoles />
  </soap:Body>
</soap:Envelope>
```

**Response:**

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <soap:Body>
    <GetAllRolesResponse xmlns="http://msiwebtrax.com/">
      <GetAllRolesResult>
        <string>Role1</string>
        <string>Role2</string>
        <!-- Additional role strings -->
      </GetAllRolesResult>
    </GetAllRolesResponse>
  </soap:Body>
</soap:Envelope>
```

#### GetRolesForUser

Retrieves all roles assigned to a specific user.

**Request:**

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tem="http://msiwebtrax.com/">
  <soap:Header>
    <CredentialsHeader xmlns="http://msiwebtrax.com/">
      <UserName>admin_username</UserName>
      <PWD>admin_password</PWD>
    </CredentialsHeader>
  </soap:Header>
  <soap:Body>
    <tem:GetRolesForUser>
      <tem:username>target_username</tem:username>
    </tem:GetRolesForUser>
  </soap:Body>
</soap:Envelope>
```

**Response:**

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <soap:Body>
    <GetRolesForUserResponse xmlns="http://msiwebtrax.com/">
      <GetRolesForUserResult>
        <string>Role1</string>
        <string>Role2</string>
        <!-- Additional role strings -->
      </GetRolesForUserResult>
    </GetRolesForUserResponse>
  </soap:Body>
</soap:Envelope>
```

#### AddUserToRole

Assigns a role to a user.

**Request:**

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tem="http://msiwebtrax.com/">
  <soap:Header>
    <CredentialsHeader xmlns="http://msiwebtrax.com/">
      <UserName>admin_username</UserName>
      <PWD>admin_password</PWD>
    </CredentialsHeader>
  </soap:Header>
  <soap:Body>
    <tem:AddUserToRole>
      <tem:username>target_username</tem:username>
      <tem:roleName>role_name</tem:roleName>
    </tem:AddUserToRole>
  </soap:Body>
</soap:Envelope>
```

**Response:**

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <soap:Body>
    <AddUserToRoleResponse xmlns="http://msiwebtrax.com/">
      <AddUserToRoleResult>
        <Success>boolean</Success>
        <Message>string</Message>
        <ErrorCode>int</ErrorCode>
      </AddUserToRoleResult>
    </AddUserToRoleResponse>
  </soap:Body>
</soap:Envelope>
```

#### RemoveUserFromRole

Removes a role from a user.

**Request:**

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tem="http://msiwebtrax.com/">
  <soap:Header>
    <CredentialsHeader xmlns="http://msiwebtrax.com/">
      <UserName>admin_username</UserName>
      <PWD>admin_password</PWD>
    </CredentialsHeader>
  </soap:Header>
  <soap:Body>
    <tem:RemoveUserFromRole>
      <tem:username>target_username</tem:username>
      <tem:roleName>role_name</tem:roleName>
    </tem:RemoveUserFromRole>
  </soap:Body>
</soap:Envelope>
```

**Response:**

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <soap:Body>
    <RemoveUserFromRoleResponse xmlns="http://msiwebtrax.com/">
      <RemoveUserFromRoleResult>
        <Success>boolean</Success>
        <Message>string</Message>
        <ErrorCode>int</ErrorCode>
      </RemoveUserFromRoleResult>
    </RemoveUserFromRoleResponse>
  </soap:Body>
</soap:Envelope>
```

### Client Membership Operations

#### GetClientsByUser

Retrieves all clients a user has access to.

**Request:**

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tem="http://msiwebtrax.com/">
  <soap:Header>
    <CredentialsHeader xmlns="http://msiwebtrax.com/">
      <UserName>admin_username</UserName>
      <PWD>admin_password</PWD>
    </CredentialsHeader>
  </soap:Header>
  <soap:Body>
    <tem:GetClientsByUser>
      <tem:username>target_username</tem:username>
    </tem:GetClientsByUser>
  </soap:Body>
</soap:Envelope>
```

**Response:**

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <soap:Body>
    <GetClientsByUserResponse xmlns="http://msiwebtrax.com/">
      <GetClientsByUserResult>
        <ClientInfo>
          <ClientId>int</ClientId>
          <ClientName>string</ClientName>
          <PreferredClient>boolean</PreferredClient>
          <CanViewVoidedClients>boolean</CanViewVoidedClients>
        </ClientInfo>
        <!-- Additional ClientInfo elements -->
      </GetClientsByUserResult>
    </GetClientsByUserResponse>
  </soap:Body>
</soap:Envelope>
```

#### GetActiveClients

Retrieves all active clients in the system.

**Request:**

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tem="http://msiwebtrax.com/">
  <soap:Header>
    <CredentialsHeader xmlns="http://msiwebtrax.com/">
      <UserName>admin_username</UserName>
      <PWD>admin_password</PWD>
    </CredentialsHeader>
  </soap:Header>
  <soap:Body>
    <tem:GetActiveClients />
  </soap:Body>
</soap:Envelope>
```

**Response:**

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <soap:Body>
    <GetActiveClientsResponse xmlns="http://msiwebtrax.com/">
      <GetActiveClientsResult>
        <ClientInfo>
          <ClientId>int</ClientId>
          <ClientName>string</ClientName>
          <PreferredClient>boolean</PreferredClient>
          <CanViewVoidedClients>boolean</CanViewVoidedClients>
        </ClientInfo>
        <!-- Additional ClientInfo elements -->
      </GetActiveClientsResult>
    </GetActiveClientsResponse>
  </soap:Body>
</soap:Envelope>
```

#### AddUserToClient

Grants a user access to a client.

**Request:**

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tem="http://msiwebtrax.com/">
  <soap:Header>
    <CredentialsHeader xmlns="http://msiwebtrax.com/">
      <UserName>admin_username</UserName>
      <PWD>admin_password</PWD>
    </CredentialsHeader>
  </soap:Header>
  <soap:Body>
    <tem:AddUserToClient>
      <tem:username>target_username</tem:username>
      <tem:clientId>123</tem:clientId>
      <tem:preferredClient>true</tem:preferredClient>
      <tem:canViewVoidedClients>false</tem:canViewVoidedClients>
    </tem:AddUserToClient>
  </soap:Body>
</soap:Envelope>
```

**Response:**

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <soap:Body>
    <AddUserToClientResponse xmlns="http://msiwebtrax.com/">
      <AddUserToClientResult>
        <Success>boolean</Success>
        <Message>string</Message>
        <ErrorCode>int</ErrorCode>
      </AddUserToClientResult>
    </AddUserToClientResponse>
  </soap:Body>
</soap:Envelope>
```

### Department Operations

#### GetClientDepartments

Retrieves all departments for a client.

**Request:**

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tem="http://msiwebtrax.com/">
  <soap:Header>
    <CredentialsHeader xmlns="http://msiwebtrax.com/">
      <UserName>admin_username</UserName>
      <PWD>admin_password</PWD>
    </CredentialsHeader>
  </soap:Header>
  <soap:Body>
    <tem:GetClientDepartments>
      <tem:clientId>123</tem:clientId>
    </tem:GetClientDepartments>
  </soap:Body>
</soap:Envelope>
```

**Response:**

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <soap:Body>
    <GetClientDepartmentsResponse xmlns="http://msiwebtrax.com/">
      <GetClientDepartmentsResult>
        <DepartmentInfo>
          <DepartmentId>int</DepartmentId>
          <DepartmentName>string</DepartmentName>
          <ClientId>int</ClientId>
        </DepartmentInfo>
        <!-- Additional DepartmentInfo elements -->
      </GetClientDepartmentsResult>
    </GetClientDepartmentsResponse>
  </soap:Body>
</soap:Envelope>
```

#### AddUserToDepartment

Grants a user access to a department.

**Request:**

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tem="http://msiwebtrax.com/">
  <soap:Header>
    <CredentialsHeader xmlns="http://msiwebtrax.com/">
      <UserName>admin_username</UserName>
      <PWD>admin_password</PWD>
    </CredentialsHeader>
  </soap:Header>
  <soap:Body>
    <tem:AddUserToDepartment>
      <tem:username>target_username</tem:username>
      <tem:clientId>123</tem:clientId>
      <tem:departmentId>456</tem:departmentId>
      <tem:overrideRoleId>00000000-0000-0000-0000-000000000000</tem:overrideRoleId>
    </tem:AddUserToDepartment>
  </soap:Body>
</soap:Envelope>
```

**Response:**

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <soap:Body>
    <AddUserToDepartmentResponse xmlns="http://msiwebtrax.com/">
      <AddUserToDepartmentResult>
        <Success>boolean</Success>
        <Message>string</Message>
        <ErrorCode>int</ErrorCode>
      </AddUserToDepartmentResult>
    </AddUserToDepartmentResponse>
  </soap:Body>
</soap:Envelope>
```

## Error Handling

The API returns an `OperationResult` object for operations that can fail. The `OperationResult` contains the following properties:

- `Success`: A boolean indicating whether the operation was successful.
- `Message`: A string containing a message describing the result of the operation.
- `ErrorCode`: An integer error code. A value of 0 indicates success, while other values indicate specific errors.

Common error codes:

- 401: Unauthorized - The user does not have permission to perform the operation.
- 404: Not Found - The requested resource was not found.
- 500: Internal Server Error - An unexpected error occurred.

## Example Usage (C#)

```csharp
// Create a web service client
var client = new UserManagementService.MSIWebTraxUserManagement();

// Set credentials
client.CredentialsHeaderValue = new UserManagementService.UserCredentials
{
    UserName = "admin_username",
    PWD = "admin_password"
};

// Get all users
var users = client.GetAllUsers();

// Create a new user
var result = client.CreateUser("new_user", "password123", "new_user@example.com", true);
if (result.Success)
{
    Console.WriteLine("User created successfully");
}
else
{
    Console.WriteLine($"Error: {result.Message}");
}

// Add user to role
result = client.AddUserToRole("new_user", "Administrator");
if (result.Success)
{
    Console.WriteLine("User added to role successfully");
}
else
{
    Console.WriteLine($"Error: {result.Message}");
}

// Add user to client
result = client.AddUserToClient("new_user", 123, true, false);
if (result.Success)
{
    Console.WriteLine("User added to client successfully");
}
else
{
    Console.WriteLine($"Error: {result.Message}");
}
```
