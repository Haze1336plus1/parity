#include <Windows.h>
#include <TlHelp32.h>
#include <stdio.h>
#include <string>
#include "MinHook.h"

#if defined _M_X64
//#pragma comment(lib, "libMinHook.x64.lib")
#elif defined _M_IX86
#pragma comment(lib, "libMinHook.x86.lib")
#endif

typedef struct _CLIENT_ID
{
     PVOID UniqueProcess;
     PVOID UniqueThread;
} CLIENT_ID, *PCLIENT_ID;

typedef struct _UNICODE_STRING {
    USHORT Length;
    USHORT MaximumLength;
#ifdef MIDL_PASS
    [size_is(MaximumLength / 2), length_is((Length) / 2) ] USHORT * Buffer;
#else // MIDL_PASS
    _Field_size_bytes_part_(MaximumLength, Length) PWCH   Buffer;
#endif // MIDL_PASS
} UNICODE_STRING;
typedef UNICODE_STRING *PUNICODE_STRING;
typedef const UNICODE_STRING *PCUNICODE_STRING;

typedef struct _OBJECT_ATTRIBUTES {
    ULONG Length;
    HANDLE RootDirectory;
    PUNICODE_STRING ObjectName;
    ULONG Attributes;
    PVOID SecurityDescriptor;        // Points to type SECURITY_DESCRIPTOR
    PVOID SecurityQualityOfService;  // Points to type SECURITY_QUALITY_OF_SERVICE
} OBJECT_ATTRIBUTES;
typedef OBJECT_ATTRIBUTES *POBJECT_ATTRIBUTES;

typedef NTSTATUS (WINAPI *tNtOpenProcess)(OUT PHANDLE processHandle, IN ACCESS_MASK accessMask, IN POBJECT_ATTRIBUTES objectAttributes, IN PCLIENT_ID clientId);


tNtOpenProcess oNtOpenProcess;

DWORD FindProcessId(const std::string& processName)
{
	PROCESSENTRY32 processInfo;
	processInfo.dwSize = sizeof(processInfo);

	HANDLE processesSnapshot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, NULL);
	if ( processesSnapshot == INVALID_HANDLE_VALUE )
		return 0;

	Process32First(processesSnapshot, &processInfo);
	if ( !processName.compare(processInfo.szExeFile) )
	{
		CloseHandle(processesSnapshot);
		return processInfo.th32ProcessID;
	}

	while ( Process32Next(processesSnapshot, &processInfo) )
	{
		if ( !processName.compare(processInfo.szExeFile) )
		{
			CloseHandle(processesSnapshot);
			return processInfo.th32ProcessID;
		}
	}
	
	CloseHandle(processesSnapshot);
	return 0;
}

DWORD wrpid = -1;
void updateHandle()
{
	while(true)
	{
		wrpid = FindProcessId("WarRock.exe");
		Sleep(50);
	}
}


NTSTATUS WINAPI nNtOpenProcess(OUT PHANDLE processHandle, IN ACCESS_MASK accessMask, IN POBJECT_ATTRIBUTES objectAttributes, IN PCLIENT_ID clientId)
{
	if(wrpid == -1 || (DWORD)clientId->UniqueProcess == wrpid)
		return 0xC0000022;
	else
	{
		NTSTATUS retVal = oNtOpenProcess(processHandle, accessMask, objectAttributes, clientId);
		return retVal;
	}
	/*if(accessMask & PROCESS_CREATE_THREAD ||
		accessMask & PROCESS_SET_INFORMATION || 
		accessMask & PROCESS_VM_WRITE || 
		accessMask & PROCESS_VM_OPERATION)
	{
		if(objectAttributes != NULL)
		{
			int nameLength = 0;
			char* objectName = ConvertToUTF8(objectAttributes->ObjectName, nameLength);
			MessageBoxA(NULL, objectName, "MEH", 0);
			processHandle = NULL;
			retVal = 0xC0000022;
		}
	}
	return retVal;*/
}

//DWORD desiredAccess

BOOL WINAPI DllMain(HINSTANCE hinstDLL, DWORD fdwReason, LPVOID lpvReserved)
{
	if(fdwReason == DLL_PROCESS_ATTACH)
	{
		//anticheatAppInitHook
		CreateThread(NULL, NULL, (LPTHREAD_START_ROUTINE)updateHandle, NULL, NULL, NULL);
		if(MH_CreateHook((void*)GetProcAddress(GetModuleHandleA("ntdll"), "NtOpenProcess"), &nNtOpenProcess, reinterpret_cast<void**>(&oNtOpenProcess)) != MH_OK)
			MessageBoxA(NULL, "acAIH failed to load", "acAIH", 0);
		return true;
	}
	return false;
}