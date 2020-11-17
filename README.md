# ProjectTracker
ASP.NET MVC 5 web app for tracking projects and creating reports

## Project setup

### Build project to install packages

### To create logging DB, run from Package Manager Console:
```
PM> update-database -ConfigurationTypeName ProjectTracker.Migrations.Audit.Configuration
```

### To seed and create admin user account, run from Package Manager Console:
```
PM> update-database -ConfigurationTypeName ProjectTracker.Migrations.ProjectTracker.Configuration
```
### Log in with:

```
Username: admin
Password: admin
```

### Create additional accounts from Users tab and at least one account with Scripter role.
