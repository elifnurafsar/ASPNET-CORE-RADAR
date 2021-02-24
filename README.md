## Description

Basic [ASP.NET](http://asp.net/) Core Model-View-Controller (MVC) app built on C# with Aspect Oriented Programming (AOP).

## Features

- User authentication
- User authorization with predefined roles and user specific permissions
- Session controls using cookies

All of the features above are implemented in AOP without using any third-party APIs or services (such as Identity).

## Pages

- UserLogin works by asking user for mail and password. If an entry is found with the given mail address and the password matches the one in the database, user is redirected to the profile page. Otherwise, a corresponding error is displayed on the page.
- UserRegister asks  for username, mail and password.
- UserProfile is a restricted page. When user clicks on profile menu on the home page, isAuthenticated Aspect checks whether the current user is logged in. If there is an active cookie, user is admitted. Otherwise, an exception is thrown out.
- UserList requires both isAuthenticated and isAuthorization aspects. Former checks whether the user is logged in while the latter requires admin type user rule. If the requirements are fulfilled, name, mail address and session information for each of users are displayed.

## Necessary Programs/Libraries

Program was written in C#, therefore, a special environment for the aforementioned language is needed (Visual Studio 2019 was used for the project). Net Core 5.0 was used for the application and no prior version was tested. Finally, in order to store user data, a local SQL server is needed. SQL Server Management Studio (Version 18.8) was used to initialize the database. For the Aspect side, PostSharp was used. 

## How to Install

1. Download the app from GitHub Repository 
2. Open the app in Visual Studio 
3. Open Microsoft SQL Server Management Studio
4. Connect to your local SQL Server
5. Create a new database CORE
6. Execute the following queries to create necessary tables

    ```java
    CREATE TABLE AccountRoles (
    	Roleid INT IDENTITY PRIMARY KEY,
    	Rolename nvarchar(100) NOT NULL,
    	Roleallow nvarchar(100), 
    	Roledeny nvarchar(100)
    );

    CREATE TABLE AccountInfo(
    	UserID INT IDENTITY PRIMARY KEY,
    	Username nvarchar(100),
    	Usermail nvarchar(100) NOT NULL, 
    	Userpassword nvarchar(100) NOT NULL,
    	Userallow nvarchar(100),
    	Userdeny nvarchar(100)
    );

    CREATE TABLE UserRoles(
    	ID INT IDENTITY PRIMARY KEY,
    	UserID INT FOREIGN KEY REFERENCES AccountInfo(UserID),
    	Roleid INT FOREIGN KEY REFERENCES AccountRoles(Roleid)
    );

    CREATE TABLE AccountSessions (
    	ID INT IDENTITY PRIMARY KEY,
    	Usermail nvarchar(100),
    	LoginDate nvarchar(100),
    	IsLoggedIn INT
    );

    INSERT INTO AccountInfo(Username, Usermail, Userpassword) VALUES ('admin', 'admin@admin.com', 'admin');

    INSERT INTO UserRoles(UserID, Roleid) VALUES (1,1);

    INSERT INTO AccountRoles(Rolename, Roleallow) VALUES ('Admin', '45EADA4A-CFB8-46A9-8DDB-5A1ACCC89D2A');
    INSERT INTO AccountRoles(Rolename, Roledeny) VALUES ('RegularUser', '45EADA4A-CFB8-46A9-8DDB-5A1ACCC89D2A');
    ```
7. Create a new databas RADAR
8. Execute the following queries to create necessary tables
````java 
    CREATE TABLE Antenna (
        antenna_id uniqueidentifier PRIMARY KEY NOT NULL,
        type nvarchar(50) NOT NULL CHECK (type IN('parabolic', 'cassegrain', 'phased array')),
        task nvarchar(50) NOT NULL CHECK (task IN('transmitter', 'receiver','multiuse')),
        horizontal_beamwidth INT,
        vertical_beamwidth INT,
        polarization nvarchar(500) NOT NULL,
        antenna_gain NUMERIC(2,2),
        number_of_feed INT,
        horizontal_dimension NUMERIC(4,2),
        vertical_dimension NUMERIC(4,2)
    );

    CREATE TABLE Receiver(
        receiver_id uniqueidentifier PRIMARY KEY NOT NULL,
        listening_time INT NOT NULL,
        rest_time INT NOT NULL,
        recovery_time INT NOT NULL,
        Antenna_id uniqueidentifier FOREIGN KEY REFERENCES Antenna(antenna_id) ON DELETE CASCADE ON UPDATE CASCADE
    );


    CREATE TABLE Transmitter(
        transmitter_id uniqueidentifier PRIMARY KEY NOT NULL,
        PW numeric(2,2) NOT NULL,
        PRI numeric(2,2) NOT NULL,
        PRF numeric(2,2) NOT NULL,
        power INT NOT NULL,
        Antenna_id uniqueidentifier FOREIGN KEY REFERENCES Antenna(antenna_id) ON DELETE CASCADE ON UPDATE CASCADE
    );

    CREATE TABLE Location(
        location_id uniqueidentifier PRIMARY KEY NOT NULL,
        country nvarchar(500) NOT NULL,
        city nvarchar(500) NOT NULL,
        geographic_latitude nvarchar(500) NOT NULL,
        geographic_longitude nvarchar(500) NOT NULL
    );

    CREATE TABLE Scan(
        scan_id uniqueidentifier PRIMARY KEY NOT NULL,
        type nvarchar(500) NOT NULL
    );

    CREATE TABLE Radar(
        radar_id uniqueidentifier PRIMARY KEY NOT NULL,
        type nvarchar(500) NOT NULL CHECK (type IN('attack warning', 'threat engagement', 'multiple type')),
        configuration nvarchar(500) NOT NULL CHECK (configuration IN('bistatic', 'continious wave', 'doppler', 'fm-cw', 'monopulse', 'passive', 'planar array', 'pulse doppler')),
        receiver uniqueidentifier FOREIGN KEY REFERENCES Receiver(receiver_id) ON DELETE CASCADE ON UPDATE CASCADE,
        transmitter uniqueidentifier FOREIGN KEY REFERENCES Transmitter(transmitter_id) ON DELETE CASCADE ON UPDATE CASCADE,
        location uniqueidentifier FOREIGN KEY REFERENCES Location(location_id) ON DELETE CASCADE ON UPDATE CASCADE
    );

    CREATE TABLE RadarScans(
        rs_id uniqueidentifier PRIMARY KEY NOT NULL,
        radar_id uniqueidentifier FOREIGN KEY REFERENCES Radar(radar_id) ON DELETE CASCADE ON UPDATE CASCADE,
        scan_id uniqueidentifier FOREIGN KEY REFERENCES Scan(scan_id) ON DELETE CASCADE ON UPDATE CASCADE
    );
````
10. Go back to Visual Studio and open Server Explorer (can be found in view)
11. Select Add Connection after right clicking on Data Connections

    Data Source: Microsoft SQL Server (SqlClient)

    Server Name: Your local SQL Server's name 

9. Select the database created in step 5
10. Under Data Connections, select properties after right clicking the server added in the previous step
11. Copy connection String and update the connection variables in UserRegistrationController, UserLoginController and UserProfileControlle
12. Go to Tools → NuGet Package Manager → Package Manager Console
13. Search & Install Postgres (necessary for AOP)
14. Finally, to run the application, click Ctrl+F5 (Start without debugging)
15. Program will be opened in a new tab in your default browser
