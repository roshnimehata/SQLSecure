;//******************************************************************
;// * Name: EventMessages.mc
;// *
;// * Description: NT event log categories and events are defined in 
;// *			   this file.
;// *
;// * (C) 2006 - Idera, a division of BBS Technologies, Inc.
;// *******************************************************************/

;// ----------------------------------------------------------------------------------------
;// *********************************Category Definitions***********************************
;// ----------------------------------------------------------------------------------------
;// These must start at 1 and be consecutive numbers, the max
;// category number is 999.   Events start after the max category number.
;//
;// If categories are added and deleted, be sure to update
;// the EventMessageInstaller CATEGORY_COUNT.
;//
;// Update the SQLsecureCat and SQLsecureEvent enums to
;// contain the correct categories and events.
;//
;//MessageIdTypedef=WORD

;// ----------------------------------------------------------------------------------------
;// Data Loader Categories
;// The range for these categories is 1-99
;// ----------------------------------------------------------------------------------------
MessageId=1
Language=English
Start
.

MessageId=2
Language=English
End
.

MessageId=3
Language=English
Validation
.

MessageId=4
Language=English
FilterLoad
.

MessageId=5
Language=English
DataLoad
.


;// ----------------------------------------------------------------------------------------
;// **********************************Event Definitions*************************************
;// ----------------------------------------------------------------------------------------
;//MessageIdTypedef=DWORD


;// ----------------------------------------------------------------------------------------
;// Data Loader Event Messages
;// The range for these event messages is 5000-5999
;// ----------------------------------------------------------------------------------------
MessageId=5000
Language=English
Idera SQLsecure Data Loader started at %1, the parameters are: %2.
.

MessageId=5001
Language=English
Idera SQLsecure Data Loader stopped at %1.
.

MessageId=5002
Language=English
The SQL Server version of the Repository on %1 is %2.   This is not supported by the Data Loader.  
.

MessageId=5003
Language=English
The SQLsecure database not found on the Repository SQL Server instance %1.
.

MessageId=5004
Language=English
The SQLsecure Repository DAL and Schema versions are not compatible.   The Repository versions are DAL: %1 and Schema: %2,
the Data Loader is expecting %3 and %4 respectively.
.

MessageId=5005
Language=English
The SQLsecure Collector does not have permissions to update the SQLsecure Repository.
.

MessageId=5006
Language=English
The SQLsecure Collector was unable to open a connection to the Repository instance %1.   Error: %2.
.

MessageId=5007
Language=English
The SQLsecure Collector was unable to aquire a valid license.
.

MessageId=5020
Language=English
The monitored SQL Server instance %1 is not registered with SQLsecure.
.

MessageId=5021
Language=English
The SQL Server version of the monitored SQL Server instance %1 is %2.   This is not supported by the Data Loader.  
.

MessageId=5022
Language=English
The SQLsecure Collector was unable to open a connection to the monitored SQL Server instance %1.   Error: %2.
.

MessageId=5023
Language=English
The SQLsecure Collector does not have permissions to read SQL Server permissions from monitored SQL Server instance %1.
.

MessageId=5024
Language=English
The SQLsecure Collector encountered a problem collecting data: %1.
.


;// ----------------------------------------------------------------------------------------
;// Exception Event Messages
;// The range for these event messages is 5900-5999
;// ----------------------------------------------------------------------------------------
MessageId=5900
Language=English
Exception was encountered - %1.  Error: %2
.
