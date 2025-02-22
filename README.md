To run this project, you have to complete the following steps:

- Create a .env file on the frontend with the following items:
  -  VITE_API_BASE_URL to your backend url;
  -   DEV_SERVER_PORT with your frontend port;
  -   VITE_API_VERSION with the version of your API.

- Create the database following these steps:
  - Delete Migrations folder
  - On the terminal, "Add-Migration migration-name"
  - On the terminal, "Update-Database"
- You need to be with ToDo.Server as the StartUp Project
- Add a appsettings.json on your ToDo.Server with the following structure:
```json
{
  "ConnectionStrings": {
    "ToDoDb": "your connection string"
  },
  "Jwt": {
    "Issuer": "your issuer",
    "Audience": "your audience",
    "Key": "your key"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```
- Then if you´re using VS2022, you can create a profile that start the ToDo.Server and the todo.client at the same time in the "Configure Startup Projects"

With this steps, you can have the project on your computer without any problem. Have a great day 😄

Dashboard Page (No Authentication):
![DashboardPageNoAuth](https://github.com/user-attachments/assets/342c67ef-adfa-48ff-8240-46d782e59e10)

Dashboard Page (Authentication):
![DashboardPageWithAuth](https://github.com/user-attachments/assets/e7391c81-3881-452d-a22d-26ab5b4623b2)

Signup Page:
![SignupPage](https://github.com/user-attachments/assets/8a94d59f-8b00-4ebb-8728-e20ec6f5af8a)

Signin Page:
![SigninPage](https://github.com/user-attachments/assets/0410f736-3b2b-48dc-9ffa-6d600a67dfb7)

Add Task Page:
![AddTaskPage](https://github.com/user-attachments/assets/37dfee7b-652e-42ff-b0f9-878670585377)

Tasks Page:
![TasksPage](https://github.com/user-attachments/assets/07e87260-5651-4bfa-a09c-d90c29fb9383)

User Profile Page:
![ProfilePage](https://github.com/user-attachments/assets/fff38e2a-ad5f-4f07-b1dc-d432f33a0d7f)















