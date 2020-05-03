# UPSShare

## Share UPS Services among several boxes

Usually a UPS is attached to one box, but sometimes the cost of it makes the user to plug more than one box into it. 
This allows the user to quickly turn off or hibernate those extra boxes attached to the UPS so the work they host is not lost.

UPSShare helps the user to automate that task using a master/slave approach, where the computer with the UPS attached to it takes 
the master role and those other boxes attached to the same UPS but with no communication with it take a slave role.

Master role then publish events on UPS behavior and Slave role subscribes to the master watching for those events and take action before 
power is down.

### Network Schema
![Network Schema](Documents/Network%20Schema.jpg "Network Schema")

### Software Requirements
* Visual Studio 2017/2019
* .NET Desktop development workload
* [.NET Framework 4.6 Developer Pack](https://dotnet.microsoft.com/download/dotnet-framework/thank-you/net46-developer-pack-offline-installer)
* [WIX Toolset](https://wixtoolset.org/releases/)

### Required Extensions for Visual Studio 2017
* [Wix Toolset Visual Studio 2017 Extension](https://marketplace.visualstudio.com/items?itemName=WixToolset.WixToolsetVisualStudio2017Extension)

### Required Extensions for Visual Studio 2019
* [Wix Toolset Visual Studio 2019 Extension](https://marketplace.visualstudio.com/items?itemName=WixToolset.WixToolsetVisualStudio2019Extension)

### Recommended Extensions
* [Markdown Editor](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.MarkdownEditor)
