<p align="center"><img src="/screenshots/Ranorex-logo.png" width="40%" alt="Ranorex Logo" /></p>

# NeoLoad Integration for Ranorex

## Overview

C# extensions to integrate [Ranorex](https://www.ranorex.com/) with [NeoLoad](https://www.neotys.com/neoload/overview) for Script maintenance and End User Experience measurement.
It allows you to interact with the NeoLoad API: 
* [Design API](https://www.neotys.com/documents/doc/neoload/latest/en/html/#11265.htm) to convert Ranorex script to NeoLoad, update an existing User Path, start/stop recording, open/close/save/create NeoLoad project, 
* [Runtime API](https://www.neotys.com/documents/doc/neoload/latest/en/html/#18727.htm) to start/stop a test, start/stop Virtual Users,
* [DataExchange API](https://www.neotys.com/documents/doc/neoload/latest/en/html/#7676.htm) to send End User Experience measurement to NeoLoad during the load test execution.


| Property | Value |
| ----------------    | ----------------   |
| Maturity | Experimental |
| Author | Ranorex |
| License           | [BSD 2-Clause "Simplified"](https://github.com/Neotys-Labs/Ranorex/blob/master/LICENSE) |
| NeoLoad Licensing | License FREE edition, or Enterprise edition, or Professional with Integration & Advanced Usage|
| Ranorex Licensing | [Ranorex license](https://www.ranorex.com/prices/) or a free [30-day trial](https://www.ranorex.com/free-trial/) |
|Supported versions | Tested with Ranorex version [8.1.1](https://info.ranorex.com/e/428272/download-zip-Ranorex-8-1-1/9xvs1r/871610304) and NeoLoad version [6.4.0](https://www.neotys.com/support/download-neoload)|
| Supported Browsers | Internet Explorer, Google Chrome|
| Download Binaries | See the [latest release](https://www.nuget.org/packages/Neoload.Ranorex/)|

## Setting up the NeoLoad Ranorex Plugin

On Ranorex Studio, add the latest version of the [NeoLoad Ranorex Plugin](#https://www.nuget.org/packages/Neoload.Ranorex/0.1.0) from NuGet: 
1. Right-click on the **References** node in the Ranorex Projects view.
2. Select **Manage Packages...**:
<p align="center"><img src="/screenshots/references.png" alt="References" /></p>

3. Search for **NeoLoad** in **nuget.org** and add the **NeoLoad Ranorex Plugin** package:
<p align="center"><img src="/screenshots/selectpluginfromnuget.PNG" alt="SelectPluginFromNuget" /></p>

This will automatically add the necessary libraries to the Ranorex project. The following modules will now appear in the module browser:
<p align="center"><img src="/screenshots/modules.png" alt="Modules" /></p>

## Global Configuration

Four global modes exist: **DESIGN**, **RUNTIME**, **END_USER_EXPERIENCE** and **NO_API**.
The mode can be set as an environment variable (in this case a Ranorex Studio restart is mandatory) or as a global parameter of the test suite. The variable key to set is **nl.ranorex.neoload.mode**.

To set a Ranorex test suite global parameter open Global parameters window:

<p align="center"><img src="/screenshots/testsuiteglobalparam.PNG" alt="TestSuiteGlobalParam" /></p>

Then add the key and the value of the parameter:

<p align="center"><img src="/screenshots/testsuiteglobalparamform.PNG" alt="TestSuiteGlobalParamForm" /></p>

If defined, the Ranorex test suite global parameter will overwrite the System environment variable.
Default mode is **NO_API**.

### No Api Mode

All NeoLoad Modules are disabled. This mode is used to start only functional recording.

### Design Mode 

Used for Neoload recording modules.

### Runtime Mode 

Used for Neoload runtime modules as StartTest and StopTest.

### End User Experience Mode

Used to send measured timer to Data Exchange API.

## NeoLoad module definitions
   
### Module NL_ConnectToDesignAPI

This module establishes a connection to the NeoLoad Design API. 
Parameters: 
* **DesignApiUri**: The Uniform Resource Identifier (URI) of the NeoLoad REST service. 
* **ApiKey**: API Key specified in NeoLoad project when identification is required. If no identification is required, this variable can be left blank.

To access these values, go to the NeoLoad **Preferences**, then the **Project settings** tab, then select the **REST API** category.
<p align="center"><img src="/screenshots/designapi.png" alt="Design API" /></p>

### Module NL_CreateProject

This module creates a new NeoLoad project. Enabled only in **DESIGN** mode.
Parameters: 
* **projectName**: The name of the NeoLoad project.
* **directoryPath**: The location of the project. By default projects are created in NeoLoad projects folder.
* **overwriteExisting**: Default value is false. If set to true, an existing project with the same name and location is deleted.

### Module NL_OpenProject

This module opens a NeoLoad project.
Parameters: 
* **filePath**: The path to the NLP file.

### Module NL_CloseProject

This module closes a NeoLoad project.
Parameters: 
* **saveProject**: Boolean. Default value is **true**. If set to **false**, current project is not saved.
* **forceStop**: Boolean. Default value is **false**. If set to **true**, running tests and recordings are stopped.

### Module NL_SaveProject

This module saves a NeoLoad project. Enabled only in **DESIGN** mode.
Two optional parameters:
* **Timeout**: The maximum amount of time (in hh:mm:ss) given to Ranorex to accomplish the save action (default value: **00:05:00**). 
* **Interval**: The time interval (in hh:mm:ss) after which Ranorex retries to save project (default value: **00:00:20**).

### Module NL_StartRecording

This module starts a recording. Enabled only in **DESIGN** mode.
Parameters: 
* **Timeout**: The maximum amount of time (in hh:mm:ss) given to Ranorex to start the recording (recommended value: **00:01:00**). 
* **Interval**: The time interval (in hh:mm:ss) after which Ranorex retries to start a recording (recommended value: **00:00:10**).
* **UpdateUserPath**: Boolean. Default value is **true**. Update existing user path. If set to **false** the existing user path will not be updated and and error will be thrown.
* **userPath**: The name of the User Path to create (the default value is "UserPath"). If the name is already used and **UpdateUserPath** is true, then it is automatically renamed using a “_X” suffix, where X is an integer. If the name has invalid characters then they will be escaped as an underscore (_) and no error is thrown.

### Module NL_StartRecording_Extended

This module is starts recording with more parameters. Enabled only in **DESIGN** mode.
* **Timeout**: The maximum amount of time (in hh:mm:ss) given to Ranorex to start the recording (recommended value: **00:01:00**). 
* **Interval**: The time interval (in hh:mm:ss) after which Ranorex retries to start a recording (recommended value: **00:00:10**).
* **UpdateUserPath**: Boolean. Default value is **true**. Update existing user path. If set to **false** the existing user path will not be updated and and error will be thrown.
* **userPath**: The name of the User Path to create (the default value is "UserPath"). If the name is already used and **UpdateUserPath** is true, then it is automatically renamed using a “_X” suffix, where X is an integer. If the name has invalid characters then they will be escaped as an underscore (_) and no error is thrown.
* **userAgentString**: Used to specify the user agent.
* **isWebSocketProtocol**: Boolean. Default value is **false**.
* **isHttp2Protocol**: Boolean. Default value is **false**.
* **isAdobeRTMPProtocol**: Boolean. Default value is **false**.
* **addressToExclude**: List of addresses separated by a semicolon. Requests and responses through these addresses will not be taken into account by Neaoload when recording (example : **10.0.0.5:7400;10.3.1.15;localhost:9100**) 

### Module NL_StopRecording

This module stops a recording. Enabled only in **DESIGN** mode.
Parameter: 
* **nl_timeout**: Integer. Default value is **1200** seconds. Set the timeout in seconds to wait for the end of the post record process. 

### Module NL_StopRecording_Extended

This module is stops recording with more paramùeters. Enabled only in **DESIGN** mode.
Parameters: 
* **nl_timeout**: Integer. Default value is **1200** seconds. Set the timeout in seconds to wait for the end of the post record process. 
* **frameworkParameterSearch**: Boolean. Default value is **true**.
* **genericParameterSearch**: Boolean. Default value is **true**.
* **deleteExistingRecording**: Boolean. Default value is **false**.
* **includeVariablesInUserpathMerge**: Boolean. Default value is **true**.
* **updateSharedContainers**: Boolean. Default value is **false**.
* **matchingThreshold**: Integer. Default value is **60**.

### Module NL_ConnectToRuntimeAPI

This module establishes a connection to the NeoLoad Runtime API. Enabled only in **RUNTIME** mode.
Parameters: 
* **RuntimeApiUri**: The Uniform Resource Identifier (URI) of the NeoLoad REST service. 
* **ApiKey**: API Key specified in NeoLoad project when identification is required. If no identification is required, this variable can be left blank.

To access these values, go to the NeoLoad **Preferences**, then the **Project settings** tab, then select the **REST API** category.
<p align="center"><img src="/screenshots/runtimeapi.png" alt="Runtime API" /></p>

### Module NL_StartTest

This module starts a NeoLoad test scenario. You need to define the scenario in NeoLoad before. Enabled only in **RUNTIME** mode.
Parameters: 
* **Scenario**: The scenario, as defined within the NeoLoad test, that should be started. 
* **Timeout**: The maximum amount of time (in hh:mm:ss) given to Ranorex to start a specific test (recommended value: **00:01:00**).
* **Interval**: The time interval (in hh:mm:ss) after which Ranorex retries to start a specific test (recommended value: **00:00:10**).

### Module NL_StopTest

This module stops the currently running NeoLoad test. Enabled only in **RUNTIME** mode.
Parameters: 
* **Timeout**: The maximum amount of time (in hh:mm:ss) given to Ranorex to start a specific test (recommended value: **00:01:00**). 
* **Interval**: The time interval (in hh:mm:ss) after which Ranorex retries to start a specific test (recommended value: **00:00:10**).
* **forceStop**: Boolean. Default value is **false**.

### Module NL_AddVirtualUsers

This module adds virtual users to a population, defined in a NeoLoad test scenario. This module can only be used when a test is already running. Enabled only in **RUNTIME** mode.
Parameters: 
* **Population**: The population, as defined in the NeoLoad test scenario, virtual users will be added to.
* **Amount**: Integer. The amount of virtual users that should be added to the given population.

### Module NL_StopVirtualUsers

This module stops virtual users from a population, which is defined in a NeoLoad test scenario. This module can only be used when a test is already running. Enabled only in **RUNTIME** mode.
Parameters: 
* **Population**: The population, as defined in the NeoLoad test, virtual users will be stopped from. 
* **Amount**: The amount of virtual users specified that will be stopped from the given population.

### Module NL_ConnectToDataExchangeAPI

This module establishes a connection to the NeoLoad Data Exchange API. Enabled in **RUNTIME** and **END_USER_EXPERIENCE** modes.
Parameters: 
* **DataExchangeApiUri**: The Uniform Resource Identifier (URI) of the NeoLoad REST service. 
* **ApiKey**: API Key specified in NeoLoad project when identification is required. If no identification is required, this variable can be left blank.
To access these values, go to the NeoLoad **Preferences**, then the **Project settings** tab, then select the **REST API** category.
<p align="center"><img src="/screenshots/dataexchangeapi.png" alt="Data Exchange API" /></p>

* **Location**: The location, where the functional test is performed (e.g., Graz, London, Office XYZ...)
* **Hardware**: The hardware used where the functional test is running (e.g., Intel i5-5200u). A string describing the utilized operating system is automatically appended to the string defined.
* **Software**: The software, tested in the functional test. When testing a browser, it is recommended to hand over the browser name. When performing a cross-browser test, it is recommended to bind this variable to the column specifying the browsers.

## Tutorials

### How to open/create/save/close NeoLoad project

Interaction with the NeoLoad Design API allows you to open/create/save/close NeoLoad project from Ranorex module.

* Insert the module NL_ConnectToDesignAPI into the test case. 

<p align="center"><img src="/screenshots/connecttodesignapi.png" alt="Connect to DesignAPI" /></p>

* To open an existing NeoLoad project, insert module NL_OpenProject into the test case.

<p align="center"><img src="/screenshots/openproject.png" alt="Open project" /></p>

* To create a new NeoLoad project, insert module NL_CreateProject into the test case.

<p align="center"><img src="/screenshots/createproject.png" alt="Create project" /></p>

* To save a NeoLoad project, insert module NL_SaveProject into the test case.

<p align="center"><img src="/screenshots/saveproject.png" alt="Save project" /></p>

* To close a NeoLoad project, insert module NL_CloseProject into the test case.

<p align="center"><img src="/screenshots/closeproject.png" alt="Close project" /></p>

### How to record/update a Ranorex script into a NeoLoad User Path

Interaction with the NeoLoad Design API allows you to record/update a Ranorex script into NeoLoad.

* Insert the module NL_ConnectToDesignAPI into the test case.
* Insert the module NL_StartRecording into the test case, before the recording.
* Insert the module NL_StopRecording into the test case, after the recording.  

<p align="center"><img src="/screenshots/startstoprecording.png" alt="Start stop recording" /></p>

During the execution of the Ranorex test case, if the NeoLoad User Path does not exist, it will be created. Otherwise, the existing User Path will be updated. 

### How to manage transactions 
Neoload transactions management can be done by inserting **user code** actions in the Ranorex recording.
##### Design Mode
* **StartTransaction**: Starts a new Neoload transaction. A transaction name is mandatory.
* **StopTransaction**: Not used for this mode.
##### End User Experience Mode
A connection to the Data Exchange API is required before invoking these actions.
* **StartTransaction**: Starts the timer and stops the last running one.
* **StopTransaction**: Stops the timer and send it using Data Exchange API.

Using a Ranorex record
<p align="center"><img src="/screenshots/ranorexrecord.png" alt="RanorexRecord" /></p>
Add user code action as steps in the recording
<p align="center"><img src="/screenshots/addusercode.png" alt="AddUserCode" /></p>
<p align="center"><img src="/screenshots/addusercode2.png" alt="AddUserCode2" /></p>
<p align="center"><img src="/screenshots/transactionexample.png" alt="TransactionExample" /></p>


### How to start/stop a NeoLoad test

Interaction with the NeoLoad Runtime API allows you to start/stop a NeoLoad test.

* Insert the module NL_ConnectToRuntimeAPI into the test case.
* Insert the module NL_StartTest into the test case.
* Insert the module NL_StopTest into the test case.  

<p align="center"><img src="/screenshots/startstoptest.png" alt="Start stop test" /></p>

### How to add/stop Virtual Users to a running NeoLoad test

Interaction with the NeoLoad Runtime API allows you to add/stop Virtual Users during a NeoLoad test.

* To add Virtual Users, insert the module NL_AddVirtualUsers into the test case.

<p align="center"><img src="/screenshots/addvirtualusers.png" alt="Add Virtual Users" /></p>

* To stop Virtual Users, insert the module NL_StopVirtualUsers into the test case.

<p align="center"><img src="/screenshots/stopvirtualusers.png" alt="Stop Virtual Users" /></p>

### How to send End User Experience measurement to NeoLoad

Opening a website is related to a certain latency. This latency depends on various factors, such as the network connection or the browser used. It can be measured with the Navigation Timing API, which is offered by all browsers. If you evaluate these timing values, especially when the website is under load, you can localize potential bottlenecks. Eliminating the identified bottlenecks will ultimately improve the end user experience.

The Ranorex **SendTimingValues** User Code allows you yo send End User Experience measurement to NeoLoad.

To use that user code: 

* Make sure the file was recorded with a browser having the Ranorex plugin up and running. 

<p align="center"><img src="/screenshots/browserplugin.png" alt="Browser plugin" /></p>

* Right click on the Ranorex recording file, and select **Add new action**, then **User code**, and then **Select from library**.

<p align="center"><img src="/screenshots/selectfromlibrary.png" alt="Select from library" /></p>

* Then select the user code **SendTimingValues**, and confirm.

<p align="center"><img src="/screenshots/neoloadnodecollection.png" alt="NeoLoad Node Collection" /></p> 
 
* Fill column **domNode** with the DOM node of the element you need a timer.

<p align="center"><img src="/screenshots/domNode.png" alt="domNode.png" /></p>

* Fill column **transactionName**. The timer values will be accessible in the NeoLoad External Data with a category of that transaction name.

<p align="center"><img src="/screenshots/transactionName.png" alt="Transaction name" /></p> 

* Insert the module NL_ConnectToDataExchangeAPI inside the test case. Make sure to insert this module after the test is started, and before executing the **SendTimingValues** code.

<p align="center"><img src="/screenshots/connecttodataexchangeapi.png" alt="Connect to DataExchangeAPI" /></p>



## ChangeLog

* Version 0.1.0 (May 11, 2018): Initial release.








