#include "StdInc.h"
#include "../extdll/Native.h"

LONG WINAPI CustomExceptionFilter(_EXCEPTION_POINTERS *ExceptionInfo)
{
	if(ExceptionInfo->ExceptionRecord->ExceptionCode == 0xCEAD0001)
	{
		NativeExInfo* exInfo = (NativeExInfo*)ExceptionInfo->ContextRecord->Edi;

		extdll::INative* nativeInstance = (extdll::INative*)exInfo->instance;
		nativeInstance->TestFunc();
		nativeInstance->Log("memory-loaded module, got instance thru exception handler, called a logging function thru interface. genius");

		// patch it to skip bad code, in case someone killing our exception

		DWORD dwPatchAddress = exInfo->patchAddress;
		DWORD dwReturnNormalAddress = exInfo->returnAddress;

		DWORD flOldProtect;
		VirtualProtect((LPVOID)dwPatchAddress, 5, PAGE_EXECUTE_READWRITE, &flOldProtect);

		*(BYTE*)dwPatchAddress = 0xE9;
		*(DWORD*)(dwPatchAddress + 1) = dwReturnNormalAddress - dwPatchAddress - 5;

		VirtualProtect((LPVOID)dwPatchAddress, 5, flOldProtect, &flOldProtect);

		return EXCEPTION_CONTINUE_EXECUTION;
	}
	return EXCEPTION_CONTINUE_SEARCH;
}

bool __stdcall DllMain(HMODULE hModule, DWORD dwReason, LPVOID lpReserved)
{
	if(dwReason == DLL_PROCESS_ATTACH)
	{
		AddVectoredExceptionHandler(0, CustomExceptionFilter);
	}
	return true;
}