//**********************************************************************
//*
//* File: SSPI_Helpers.h
//*
//* Copyright Idera (BBS Technologies) 2005
//*
//**********************************************************************
#pragma once

namespace SSPI_Helpers
{
   BOOL __stdcall 
      LogonUser (
         LPCTSTR szDomain, 
         LPCTSTR szUser, 
         LPCTSTR szPassword
      );
}