//**********************************************************************
//*
//* File: stdafx.h
//*
//* Copyright Idera (BBS Technologies) 2005
//*
//**********************************************************************
#pragma once


#define SECURITY_WIN32
// Windows Header Files:
#include <windows.h>
#include <ntsecapi.h>
#include <tchar.h>
#include <atlbase.h> 
#include <string>
#include <memory>
#include <stdio.h>
#include <sspi.h>
#include <aclapi.h>
#include <time.h>
#include <assert.h>
// Older versions of WinError.h does not have SEC_I_COMPLETE_NEEDED #define.
// So, in such SDK environment setup, we will include issperr.h which has the
// definition for SEC_I_COMPLETE_NEEDED. Include issperr.h only if
// SEC_I_COMPLETE_NEEDED is not defined.
#ifndef SEC_I_COMPLETE_NEEDED
#include <issperr.h>
#endif

// TODO: reference additional headers your program requires here
#include "InstallUtilLib.h"
#include "SSPI_Helpers.h"

#ifndef DIM
#define DIM(array) (sizeof (array) / sizeof (array[0]))
#endif
