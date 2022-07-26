# FullDuplex
## Solution Overview



This solution is meant to fullfill the requirements by the client to make use of GRPC developed in .NET Web Application with WPF Client

## Solution Overview
- FullDuplex (Web Application Server using GRPC)
- Shared Project (Contain GRPC auto generated client files for the given protocol - to reuse in WPF Client)
- Task Component (Winforms simple UI)
- WPFClient (Simple Client for sending/receiving requests to FullDuplex GRPC Server/Web Application)
- Client Setup
## Features

- Send and receive a message from client to server using given GRPC protocol. Making use of streams at both ends.
- Launch/QUIT a program at server by sending a request from client (*Read Limitations)
- See execuation logs/erros in simple listview

## Technology

This projects uses the following technologies

- [.NET] - .NET 6
- [Visual Studio] - Visual Studio 2022
- [GRPC] - Google GRPC
- [C#] - C#
- [Extension-Installer] - Microsoft Visual Studio Installer Projects 2022



## Starting Project with Visual Studio

This solution requires visual studio to run.

Open the solution FullDuplex.sln file in visual studio


Wait for the nuget packages to restore and rebuild the solution, if an error occur on ClientProject, please install Visual Studio Installer Project from Extensions menu in visual studio and rebuild project. Once successful, run the project, the solution is already configured to start both server and client starting server first. Make sure the appUrl in WPFClient>StreamingHandler.cs>appUrl and FullDuplex>Properties>launchSettings>ApplicationUrl is pointing to the same localhost address with https. 
Use client by sending a message to server and wait for response from the server for 20 seconds.
TaskComponent exe can be run directly by cmd without arguments (UI-launch), with arguments (can start a process in Main(string args) method)- Refer to TaskComponent Project Main Method.

## Installing project to computer
- Install Client by building the ClientSetup Project. Open setup ClientSetup\Release\Setup.exe
- Rebuild the server project, go to complete publish path FullDuplex\bin\Release\net6.0\publish
- Open IIS Manager, right click sites, click add website, write sitename as FullDuplex, Provide above publish full path to physical path, select binding type as https, IP assigned - all unassigned, port 443. Select SSL certificate as IIS Express Development Certificate
- Open FullDuplex project folder, go to properties, go to Security tab, under Group or user names click Edit button, the click Add button, write IIS_IUSRS and click Check names, the user name will be available followed by your computer name. Click Ok, Ok Ok.
- Once the site is added to IIS, click browse website and see if it running and returning a response. GRPC Server is setup.
- Now you can publish from visual studio with web deploy profile to this website for any changes in the source code.
- Right Click FullDuplex Project and click Publish, In publish profile select WebDeploy profile and click on publish button.
- Open FullDuplex project folder, go to properties, go to Security tab, under Group or user names click Edit button, the click Add button, write IIS_IUSRS and click Check names, the user name will be available followed by your computer name. Click Ok, Ok Ok.
- Note down the URL of the deployed site after you open it in browser, Use the same url in WPFClient>StreamingHandler.cs>AppUrl.

## Limitations

- A .NET web application cannot start a UI process from the server itself, however it can launch TaskComponent in task manager as a process. There are some security hacks through which we can acheive this but that is not recommended.
- However, we can make two UI based clients communicating with the web service using GRPC, and one client can send a request to other client using webservice and it can launch a UI process because the other client will also be UI based.
- A UI client must run as UI program or as a tray program to launch other UI process. A windows service or webservice is preventing it.
- The Task Component is launched with UI when runnnig the project through visual studio, however it only starts a process without UI after deploying the webservice to IIS.


## Challenges
- Compatibility issues with GRPC auto generated files with WPF, had to make SharedProject for GRPC Client files.
- Issues with making isntaller for deploying server to IIS. Had to follow one time configuration with IIS manually and then use WebDeploy to publish with one click to IIS.
- Limiatations of a webservice to launch another UI program itself. Even with the 'make this app to interact with desktop' didnt work.




## For any Questions/Queries, do not forget to write back.







   [GRPC]: <https://grpc.io/docs/languages/csharp/quickstart/>
   [Visual Studio]: <https://visualstudio.microsoft.com/vs/>
   [C#]: <https://docs.microsoft.com/en-us/dotnet/csharp/>
   [.NET]: <https://dotnet.microsoft.com/en-us/download/dotnet/6.0>
   [Extension-Installer]: <https://marketplace.visualstudio.com/items?itemName=VisualStudioClient.MicrosoftVisualStudio2022InstallerProjects>
  
