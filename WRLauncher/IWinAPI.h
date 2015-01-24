#include "stdafx.h"

using namespace System;

namespace WRL
{
	interface class IWinAPI
	{

		bool CreateProcess(String^ lpApplicationName, String^ lpCommandLine, IntPtr lpProcessAttributes, IntPtr lpThreadAttributes, bool bInheritHandles, uint dwCreationFlags, IntPtr lpEnvironment, String^ lpCurrentDirectory, STARTUPINFO* lpStartupInfo, int[] lpProcessInfo);
		IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);
		UInt32 NtUnmapViewOfSection(IntPtr hProcess, IntPtr lpBaseAddress);
		Int32 NtWriteVirtualMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, uint nSize, IntPtr lpNumberOfBytesWritten);
		Int32 NtGetContextThread(IntPtr hThread, IntPtr lpContext);
		Int32 NtSetContextThread(IntPtr hThread, IntPtr lpContext);
		UInt32 NtResumeThread(IntPtr hThread, IntPtr SuspendCount);

	}

}