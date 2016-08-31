//******************************************************************
// * Name: EventMessages.mc
// *
// * Description: NT event log categories and events are defined in 
// *			   this file.
// *
// * (C) 2006 - Idera, a division of BBS Technologies, Inc.
// *******************************************************************/
// ----------------------------------------------------------------------------------------
// *********************************Category Definitions***********************************
// ----------------------------------------------------------------------------------------
// These must start at 1 and be consecutive numbers, the max
// category number is 999.   Events start after the max category number.
//
// If categories are added and deleted, be sure to update
// the EventMessageInstaller CATEGORY_COUNT.
//
// Update the SQLsecureCat and SQLsecureEvent enums to
// contain the correct categories and events.
//
//MessageIdTypedef=WORD
// ----------------------------------------------------------------------------------------
// Data Loader Categories
// The range for these categories is 1-99
// ----------------------------------------------------------------------------------------
//
//  Values are 32 bit values layed out as follows:
//
//   3 3 2 2 2 2 2 2 2 2 2 2 1 1 1 1 1 1 1 1 1 1
//   1 0 9 8 7 6 5 4 3 2 1 0 9 8 7 6 5 4 3 2 1 0 9 8 7 6 5 4 3 2 1 0
//  +---+-+-+-----------------------+-------------------------------+
//  |Sev|C|R|     Facility          |               Code            |
//  +---+-+-+-----------------------+-------------------------------+
//
//  where
//
//      Sev - is the severity code
//
//          00 - Success
//          01 - Informational
//          10 - Warning
//          11 - Error
//
//      C - is the Customer code flag
//
//      R - is a reserved bit
//
//      Facility - is the facility code
//
//      Code - is the facility's status code
//
//
// Define the facility codes
//


//
// Define the severity codes
//


//
// MessageId: 0x00000001L (No symbolic name defined)
//
// MessageText:
//
//  Start
//


//
// MessageId: 0x00000002L (No symbolic name defined)
//
// MessageText:
//
//  End
//


//
// MessageId: 0x00000003L (No symbolic name defined)
//
// MessageText:
//
//  Validation
//


//
// MessageId: 0x00000004L (No symbolic name defined)
//
// MessageText:
//
//  FilterLoad
//


//
// MessageId: 0x00000005L (No symbolic name defined)
//
// MessageText:
//
//  DataLoad
//


// ----------------------------------------------------------------------------------------
// **********************************Event Definitions*************************************
// ----------------------------------------------------------------------------------------
//MessageIdTypedef=DWORD
// ----------------------------------------------------------------------------------------
// Data Loader Event Messages
// The range for these event messages is 5000-5999
// ----------------------------------------------------------------------------------------
//
// MessageId: 0x00001388L (No symbolic name defined)
//
// MessageText:
//
//  Idera SQLsecure Data Loader started at %1, the parameters are: %2.
//


//
// MessageId: 0x00001389L (No symbolic name defined)
//
// MessageText:
//
//  Idera SQLsecure Data Loader stopped at %1.
//


//
// MessageId: 0x0000138AL (No symbolic name defined)
//
// MessageText:
//
//  The SQL Server version of the Repository on %1 is %2.   This is not supported by the Data Loader.  
//


//
// MessageId: 0x0000138BL (No symbolic name defined)
//
// MessageText:
//
//  The SQLsecure database not found on the Repository SQL Server instance %1.
//


//
// MessageId: 0x0000138CL (No symbolic name defined)
//
// MessageText:
//
//  The SQLsecure Repository DAL and Schema versions are not compatible.   The Repository versions are DAL: %1 and Schema: %2,
//  the Data Loader is expecting %3 and %4 respectively.
//


//
// MessageId: 0x0000138DL (No symbolic name defined)
//
// MessageText:
//
//  The SQLsecure Collector does not have permissions to update the SQLsecure Repository.
//


//
// MessageId: 0x0000138EL (No symbolic name defined)
//
// MessageText:
//
//  The SQLsecure Collector was unable to open a connection to the Repository instance %1.   Error: %2.
//


//
// MessageId: 0x0000138FL (No symbolic name defined)
//
// MessageText:
//
//  The SQLsecure Collector was unable to aquire a valid license.
//


//
// MessageId: 0x0000139CL (No symbolic name defined)
//
// MessageText:
//
//  The monitored SQL Server instance %1 is not registered with SQLsecure.
//


//
// MessageId: 0x0000139DL (No symbolic name defined)
//
// MessageText:
//
//  The SQL Server version of the monitored SQL Server instance %1 is %2.   This is not supported by the Data Loader.  
//


//
// MessageId: 0x0000139EL (No symbolic name defined)
//
// MessageText:
//
//  The SQLsecure Collector was unable to open a connection to the monitored SQL Server instance %1.   Error: %2.
//


//
// MessageId: 0x0000139FL (No symbolic name defined)
//
// MessageText:
//
//  The SQLsecure Collector does not have permissions to read SQL Server permissions from monitored SQL Server instance %1.
//


//
// MessageId: 0x000013A0L (No symbolic name defined)
//
// MessageText:
//
//  The SQLsecure Collector encountered a problem collecting data: %1.
//


// ----------------------------------------------------------------------------------------
// Exception Event Messages
// The range for these event messages is 5900-5999
// ----------------------------------------------------------------------------------------
//
// MessageId: 0x0000170CL (No symbolic name defined)
//
// MessageText:
//
//  Exception was encountered - %1.  Error: %2
//


