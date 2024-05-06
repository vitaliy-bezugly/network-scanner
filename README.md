# IP Scanner

IP Scanner is a scheduled network scanning program for local network analysis. The goal is to provide functionality to display all devices of a certain network (by the specified range of ip addresses, local or global network), to provide access to shared folders, to offer remote control of computers (via RDP). The program is intended for user convenience and is designed to work as a portable version.

## How to Launch

To get started with the Network Scanner application, follow these steps:

1. **Clone the Repository**: Open your Git terminal and clone the repository by running:
```
git clone https://github.com/vitaliy-bezugly/ip-scanner.git
```

2. **Open the Project**: Open the cloned repository with Visual Studio or another IDE that supports UWP development.

3. **Set Process Type**: Make sure to set the process type to either <code>**x64 or x86**</code>: 

![image](https://github.com/vitaliy-bezugly/ip-scanner/assets/87979065/5e34c57b-1bcf-4af1-a2c8-ecf7966def73)

4. **Build and Run**: Build the solution and then run the application. You should see the Ip Scanner's main window, and you can begin exploring its features.

## Project Structure

The repository is organized into two main directories: 
- `src`: Contains the source code of the application.
- `tests`: Houses the unit tests.

### Source Code (`src`)

The source code resides in a solution named `IpScanner`, comprising the following projects:

- **IpScanner.Ui**: This is the UWP (Universal Windows Platform) desktop application. Built using the MVVM (Model-View-ViewModel) design pattern. It serves as the primary interface between the user and the application.
- **IpScanner.Infrastructure**: This project houses the infrastructure code, facilitating implementation of external dependencies like HTTP, TCP, DNS, UDP, etc.
- **IpScanner.Domain**: This project contains the core logic of the network scanning functionality.
- **IpScanner.Models**: Contains all the data models used across the application. These models serve as the data structure for storing and transmitting information within the application.
- **IpScanner.ViewModels**: Houses the view models that serve as a bridge between the UI and the business logic, following the MVVM design pattern.
- **IpScanner.Services**: This project contains additional business logic encapsulated into various services. This allows for greater modularity and easier testing.
- **IpScanner.Helpers**: This project contains various utility classes and common functionalities that can be used throughout the application.

### Tests (`tests`)

The unit tests are organized as follows:

- **IpScanner.Domain.UnitTests**: Contains unit tests focused on the core business logic in the `IpScanner.Domain` assembly.
- **IpScanner.Infrastructure.UnitTests**: Includes unit tests focused on testing the implementation of external dependencies in the `IpScanner.Infrastructure` assembly.
- **IpScanner.Models.UnitTests**: These tests are designed to validate the integrity and behavior of the data models used across the application. They ensure that the models function as expected under various scenarios and data conditions.

All projects follow the naming convention: `IpScanner.[AssemblyName]`, making it easier for identification and management.

## Known Issues and Behavior in Debugging Mode

### Performance Issues in Debugging Mode

When you run the application in debug mode, you may experience performance issues such as freezing and frame rate (FPS) drops during the process of <code>canceling a scan operation</code>.

#### Workaround
To circumvent this issue, run the application without debugging. In the non-debugging mode, the application functions as expected without any performance degradation.

It's important to note that this is not a bug in the application itself but rather an issue related to Visual Studio's debugging mode.

The video covers various features and functionalities, offering a comprehensive overview of what the application can do.

## Pictures 
![image](https://github.com/vitaliy-bezugly/ip-scanner/assets/87979065/9af33989-a4ac-4f33-9ab5-429f745cd18d)
![image](https://github.com/vitaliy-bezugly/ip-scanner/assets/87979065/4af44251-79c3-4bf5-b19d-76288aff95c4)
![image](https://github.com/vitaliy-bezugly/ip-scanner/assets/87979065/ed8b5f83-6287-4e1b-b313-806f4697eee7)
![image](https://github.com/vitaliy-bezugly/ip-scanner/assets/87979065/1c7ccef0-0f55-4660-b0b1-ab660a59ce31)
![image](https://github.com/vitaliy-bezugly/ip-scanner/assets/87979065/d6a1d3fb-1a8b-4822-9ef7-3e2b0776038f)
![image](https://github.com/vitaliy-bezugly/ip-scanner/assets/87979065/5ac20a91-6101-4a03-88d4-44c97ceed14b)
![image](https://github.com/vitaliy-bezugly/ip-scanner/assets/87979065/b8c206f5-13ea-4c19-a106-f8277cd857b3)
![image](https://github.com/vitaliy-bezugly/ip-scanner/assets/87979065/d9a49d59-f75b-4713-aef9-e0670bca53b2)
