#Slides

![imag0303](https://cloud.githubusercontent.com/assets/9950081/12404336/597dc056-bdef-11e5-96c6-db9d736dede6.jpg)


## 1. Intro
[Sergey] 

Introduction slide
- What is Application Insights
- Auto collection of the Data
- Application Insights is a platform - supprot your technology

## 2. Architecture
[Alex]

## 3. Journey
[Alex]

## 4. Middleware
[Alex]

## 5. Context Semantics
[Sergey] 

Describe every context propery and how they being collected.

UI: on *performance blade* - group by and filter by property

## 6. Correlation
[Sergey] 

Show how to correlate by user, session and by ID

UI: on *details blade* - same user, same session and telemetry for this request

## 7. Dependencies
[Alex]

## 8. Adaptive sampling
[Sergey] 

Talk about big customers and how we will minimize amount of collected data without loosing semantics

## 9. Demo - live data

## 10. GitHub + community
[Sergey] 

Community page? https://azure.microsoft.com/en-us/documentation/articles/app-insights-platforms/



1. Middleware - see https://gist.github.com/s093294/d4b8abdaf4000b6c7f80

2. Exception tracking - see https://github.com/advancedrei/ApplicationInsights.Helpers/blob/master/src/WebApi2/InsightsExceptionLogger.cs

3. Semantics of fields - see [below](#context-properties)
	
4. Channel & adaptive sampling - see http://apmtips.com/blog/2015/09/03/more-telemetry-channels/

5. Enable dependency tracking and Performance counters tracking - see https://github.com/Microsoft/ApplicationInsights-aspnet5/issues/65

6. Telemetry Initializer

7. Self-diagnostics http://apmtips.com/blog/2015/11/07/application-insights-self-diagnostic/

#Context properties

Whole set of context properties that are currently accepted by DC is defined by bond schema file: 
https://mseng.visualstudio.com/DefaultCollection/AppInsights/ChuckNorris/_git/DataCollectionSchemas#path=%2Fv2%2FBond%2FContextTagKeys.bond&version=GBmaster&_a=contents 

Here are the properties that are currently used for Server-side and JavaScript SDK for diagnostics and troubleshooting scenarios:

Cloud identity
Cloud identity properties defines where the server application is running. It is required for instance-based metrics aggregations to identify machines in bad state. Cloud identity context properties has low cardinality. These properties should be associated with every telemetry item we send. The only property SDK collects by default is roleInstance. RoleName will be collected for Azure cloud services. 

"ai.device.roleInstance"

Use as a dimension: Yes 
Cardinality: low
Data format compression: impractical

"ai.device.roleName"

Use as a dimension: No
Cardinality: low
Data format compression: impractical


Application Identity
The only application identity property we are using today is application version. Application identity context properties has low cardinality. These properties should be associated with every telemetry item we send. Application version is collected automatically if BuildInfo.config file is available for classic ASP.NET applications. Application version property is important for the future back to code scenarios as it tights together build number and exception stack or performance problem.

"ai.application.ver"

Use as a dimension: No
Cardinality: low
Data format compression: impractical

User Identity Properties
User and session identities are critical for impact analysis as well as telemetry correlation to record history of user actions just before the error happened. Common scenarios – “how many users affected by specific database outage”, “show me what happened in the session before the certain exception”. These properties are also used for sampling score calculation.
"ai.user.id"
"ai.session.id"
"ai.user.authId"
"ai.user.accountId"

Use as a dimension: No, but used for cardinality query with late compute calculations.
Cardinality: very high
               Decrease cardinality: 
1.	Do not use these fields to correlate with GSM tests
2.	Allow the tracking cookies to operate across subdomains https://github.com/Microsoft/ApplicationInsights-JS/issues/64 

Data format compression: make identifier shorter and use base64 encoding

Current Operation Properties
Operation properties enables correlation of telemetry items. Our long term plan is to use correlation vector for correlation. However there are many systems that are still using correlation based on id, parent and root. When possible we will only use correlation vector.

Right now operation name represent “root operation”  - name of the very first operation initiated omitting of this telemetry item. Operation name is used for grouping telemetry items by logical operations. For example, scenarios like “what pages are affected by this exception” or “show all types of dependencies this page every executes”.
"ai.operation.id"
"ai.operation.parentId"
"ai.operation.rootId"
"ai.operation.correlationVector"

"ai.operation.name"

Use as a dimension: No 
Cardinality: very high
Data format compression: make identifier shorter and use base64 encoding

Browser
Browser and browser version are used troubleshoot JavaScript errors and performance problems. This field will be calculated based on User-Agent header received by DC from browser. We may add this property for server side SDK instead of UserAgent context in future.
NOT EXIST IN DC SCHEMA, ONLY ES: "ai.device.browser"

Use as a dimension: Yes
Cardinality: low for main browsers with the long tale.
Data format compression: impractical

Location
Location IP is used for latency investigations for page view performance in JavaScript SDK. It is extracted from http packet IP client address when browser sends data to the DC. This context should not be collected by default by server side SDK. However customer may write custom code to enable data collection for it. Location IP will be masked and geo information extracted from it:
               "ai.location.ip"

NOT EXIST IN DC SCHEMA, ONLY ES: “ai.locaiton.Country”
NOT EXIST IN DC SCHEMA, ONLY ES: “ai.locaiton.Province”

Use as a dimension: Yes for Country and Province
Cardinality: medium
Data format compression: may use codes for Country and Province in future

Synthetic source
Synthetic source is currently used to mark telemetry as being initiated by different synthetic sources like GSM.

NOT EXIST IN DC SCHEMA, ONLY ES: “ai.operation.isSynthetic”

Use as a dimension: Yes 
Cardinality: low

"ai.operation.syntheticSource"

Use as a dimension: No
Cardinality: medium
Data format compression: Yes, have shorter name for “Application Insights Availability Monitoring”


Telemetry
SDK version field is used to specify the type and version of SDK collected this telemetry item. It is used for telemetry, troubleshooting and potentially for special processing of telemetry items on DC.
"ai.internal.sdkVersion"

Request Telemetry item
Request telemetry item represent incoming http request. 

               "requestData.name"
"requestData.success"
"requestData.responseCode"
               NOT EXIST IN DC SCHEMA, ONLY ES: “requestData.performanceBucket”

Use as a dimension: Yes
Cardinality: can be high
               Decrease cardinality:
1.	Support attribute-based MVC Web API routing
2.	Apply regex to remove “id” and “giud” parts of URL
3.	Allow customers easily override

"requestData.httpMethod"
"requestData.url"

"requestData.duration"

Use as a dimension: No
Cardinality: high

Dependency Telemetry item
Dependency telemetry item represent outgoing dependency call that is made by application. It may be http, sql or any customer defined call. 
Base Data

“dependencyData.name”
               “dependencyData.success”
“dependencyData.dependencyTypeName”
               NOT EXIST IN DC SCHEMA, ONLY ES: “dependencyData.performanceBucket”

Use as a dimension: Yes
Cardinality: can be high
               Decrease cardinality:
                                TBD

               “dependencyData.commandName”
               “dependencyData.duration”

Use as a dimension: No
Cardinality: high

Trace Telemetry item
Trace telemetry item represent trace message. Expected to be high volume. 
               “messageData.message”
               “messageData.severityLevel”

Use as a dimension: No
Cardinality: high

Event Telemetry item
Event telemetry is tracked by user to indicate an point in time event. Typically for the telemetry purposes. It is expected that event name cardinality will be low. 

“eventData.name”

Use as a dimension: Yes
Cardinality: expected to be low

PageView
Page view telemetry item represent page visit with the loading duration. Page view described by page name and page url. DC will run the logic to extract urlBase from url to allow to group by URL as it is quite common pattern for people to group by it to analyze performance problems.

“pageViewData.name”
NOT EXIST IN DC SCHEMA, ONLY ES: “pageViewData.urlBase”
               NOT EXIST IN DC SCHEMA, ONLY ES: “pageViewData.durationPerformanceBucket”

Use as a dimension: Yes
Cardinality: expected to be low

“pageViewData.url”
“pageViewData.duration”

Use as a dimension: No
Cardinality: high

PageViewPerfromance
Same as page view, but represents all the network timing information. We are thinking of merging these two together.

“PageviewPerformanceData.name”
NOT EXIST IN DC SCHEMA, ONLY ES: “pageViewData.urlBase”
               NOT EXIST IN DC SCHEMA, ONLY ES: “pageViewData.durationPerformanceBucket”

Use as a dimension: Yes
Cardinality: expected to be low

“PageviewPerformanceData.url”
“PageviewPerformanceData.duration”
“PageviewPerformanceData.perfTotal”
“PageviewPerformanceData.networkConnect”
“PageviewPerformanceData.sentRequest”
“PageviewPerformanceData.receivedResponse”
“PageviewPerformanceData.domProcessing”

Use as a dimension: No
Cardinality: high

Exception Telemetry Item
Exception telemetry item is complex and heavy object. We will need a separate effort to minimize the size of every single exception object. Problem ID for exception is calculated on DC and typically it is a customer’s function name and exception type. Problem ID is the only dimension that we will aggregate by.

CALCULATED IN DC: “ExceptionData.problemId”

Use as a dimension: Yes
Cardinality: medium

“ExceptionData.handledAt”
“ExceptionData.exceptions”
“ExceptionData.severityLevel”
“ExceptionData.crashThreadId”

Use as a dimension: No
Cardinality: high

Session State
Session state event is sent every time session starts and stops. This event is used for counting user and sessions counts. We should discuss whether we can get this information from actual events like page views and requests.

                “sessionState.state”

Use as a dimension: No

Summary

Dimensions:

Server SDK:
"ai.device.roleInstance"
“ai.operation.isSynthetic”

"requestData.name"
"requestData.success"

"requestData.responseCode"
“requestData.performanceBucket”

“dependencyData.name”
“dependencyData.success”
“dependencyData.dependencyTypeName”
“dependencyData.performanceBucket”

“eventData.name”

“ExceptionData.problemId”

JavaScript SDK:
"ai.device.browser"
“ai.locaiton.Country”
“ai.locaiton.Province”
“ai.operation.isSynthetic”

“pageViewData.name”
“pageViewData.urlBase”
“pageViewData.durationPerformanceBucket”

“ExceptionData.problemId”


Impact analysis queries by these fields:

"ai.user.id"
"ai.session.id"
"ai.user.authId"
"ai.user.accountId"
