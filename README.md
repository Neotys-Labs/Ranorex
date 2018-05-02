<p align="center"><img src="/screenshots/Ranorex-logo.png" width="40%" alt="Ranorex Logo" /></p>

# NeoLoad Integration for Ranorex

## Overview

C# extensions to integrate [Ranorex](https://www.ranorex.com/) with [NeoLoad](https://www.neotys.com/neoload/overview) for Script maintenance and End User Experience measurement.
It allows you to interact with the NeoLoad API: 
* [Design API](https://www.neotys.com/documents/doc/neoload/latest/en/html/#11265.htm) to convert Ranorex script to NeoLoad, update an existing User Path, start/stop recording, specify transaction names, open/close/save/create project, 
* [Runtime API](https://www.neotys.com/documents/doc/neoload/latest/en/html/#18727.htm) to start/stop a test, start/stop users,
* [DataExchange API](https://www.neotys.com/documents/doc/neoload/latest/en/html/#7676.htm) to send End User Experience measurement to NeoLoad during the load test execution.


| Property | Value |
| ----------------    | ----------------   |
| Maturity | Experimental |
| Author | Ranorex |
| License           | [BSD 2-Clause "Simplified"](https://github.com/Neotys-Labs/Ranorex/blob/master/LICENSE) |
| NeoLoad Licensing | License FREE edition, or Enterprise edition, or Professional with Integration & Advanced Usage|
| Ranorex Licensing | [Ranorex license](https://www.ranorex.com/prices/) or a free [30-day trial](https://www.ranorex.com/free-trial/) |
|Supported versions | Tested with Ranorex version [8.1.1](https://info.ranorex.com/e/428272/download-zip-Ranorex-8-1-1/9xvs1r/871610304) and NeoLoad version [6.4.0](https://www.neotys.com/support/download-neoload)|
| Download Binaries | See the [latest release](https://github.com/Neotys-Labs/Ranorex/releases/latest)|

## Setting up the Ranorex - NeoLoad integration

On Ranorex Studio, add the latest version of the Ranorex - NeoLoad integration from NuGet: 
1. Right-click on the **References** node in the Ranorex Projects view.
2. Select **Manage Packages...**:
<p align="center"><img src="/screenshots/references.png" alt="References" /></p>
3. Search for **NeoLoad** in **nuget.org** and add the **Ranorex-NeoLoad integration** package:
<p align="center"><img src="/screenshots/select package.png" alt="Select package" /></p>

This will automatically add the necessary libraries to the Ranorex project. The following modules will now appear in the module browser:
<p align="center"><img src="/screenshots/modulebrowser.png" alt="Module browser" /></p>

## Design API

Interaction with the Design API allows you to: 
* Start a NeoLoad recording, 
* Stop a NeoLoad recording, 
* Start a Transaction, 
* Stop a Transaction,
* Create a NeoLoad project, 
* Open a NeoLoad project,
* Save a NeoLoad project,
* Close a NeoLoad project
   
### Module ConnectToDesignApi

This module establishes a connection to the NeoLoad Design API. 
Parameters: 
* **DesignApiUri**: The Uniform Resource Identifier (URI) of the NeoLoad REST service. 
* **ApiKey**: API Key specified in NeoLoad project when identification is required. If no identification is required, this variable can be left blank.

To access these values, go to the NeoLoad **Preferences**, then the **Project settings** tab, then select the **REST API** category.
<p align="center"><img src="/screenshots/designapi.png" alt="Design API" /></p>

## Runtime API

Interaction with the Runtime API allows you to:
* Start a NeoLoad test
* Stop a NeoLoad test
* Add Virtual Users to a running test
* Stop Virtual Users to a running test 

### Module ConnectToRuntimeApi

This module establishes a connection to the NeoLoad Runtime API. 
Parameters: 
* **RuntimeApiUri**: The Uniform Resource Identifier (URI) of the NeoLoad REST service. 
* **ApiKey**: API Key specified in NeoLoad project when identification is required. If no identification is required, this variable can be left blank.

To access these values, go to the NeoLoad **Preferences**, then the **Project settings** tab, then select the **REST API** category.
<p align="center"><img src="/screenshots/runtimeapi.png" alt="Runtime API" /></p>

### Module StartNeoLoadTest

This module starts a NeoLoad test scenario. You need to define the scenario in NeoLoad before.
Parameters: 
* **Scenario**: The scenario, as defined within the NeoLoad test, that should be started. 
* **Timeout**: The maximum amount of time (in hh:mm:ss) given to Ranorex to start a specific test (recommended value: **00:01:00**).
* **Interval**: The time interval (in hh:mm:ss) after which Ranorex retries to start a specific test (recommended value: **00:00:10**).

### Module StopNeoLoadTest

This module stops the currently running NeoLoad test.
Parameters: 
* **Timeout**: The maximum amount of time (in hh:mm:ss) given to Ranorex to start a specific test (recommended value: **00:01:00**). 
* **Interval**: The time interval (in hh:mm:ss) after which Ranorex retries to start a specific test (recommended value: **00:00:10**).

### Module AddVirtualUsers

This module adds virtual users to a population, defined in a NeoLoad test scenario. This module can only be used when a test is already running.
Parameters: 
* **Population**: The population, as defined in the NeoLoad test scenario, virtual users will be added to. 
* **Amount**: The amount of virtual users that should be added to the given population.

### Module RemoveVirtualUsers

This module stops virtual users from a population, which is defined in a NeoLoad test scenario. This module can only be used when a test is already running.
Parameters: 
* **Population**: The population, as defined in the NeoLoad test, virtual users will be stopped from. 
* **Amount**: The amount of virtual users specified that will be stopped from the given population.

## DataExchange API

### Module ConnectDataExchangeApi

This module establishes a connection to the NeoLoad Data Exchange API. 
Parameters: 
* **DataExchangeApiUri**: The Uniform Resource Identifier (URI) of the NeoLoad REST service. 
* **ApiKey**: API Key specified in NeoLoad project when identification is required. If no identification is required, this variable can be left blank.
To access these values, go to the NeoLoad **Preferences**, then the **Project settings** tab, then select the **REST API** category.
<p align="center"><img src="/screenshots/dataexchangeapi.png" alt="Data Exchange API" /></p>
* **Location**: The location, where the functional test is performed (e.g., Graz, London, Office XYZ...)
* **Hardware**: The hardware used where the functional test is running (e.g., Intel i5-5200u). A string describing the utilized operating system is automatically appended to the string defined.
* **Software**: The software, tested in the functional test. When testing a browser, it is recommended to hand over the browser name. When performing a cross-browser test, it is recommended to bind this variable to the column specifying the browsers.

## ChangeLog

* Version 1.0.0 (September 20, 2017): Initial release.








