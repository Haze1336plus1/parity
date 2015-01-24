#include "stdafx.h"
#include <Windows.h>

using namespace System;

namespace WRL
{
	
	public class WinAPI : IWinAPI
    {

        #region Interface WinAPI

        bool IWinAPI.CreateProcess(string lpApplicationName, string lpCommandLine, IntPtr lpProcessAttributes, IntPtr lpThreadAttributes, bool bInheritHandles, uint dwCreationFlags, IntPtr lpEnvironment, string lpCurrentDirectory, ref STARTUPINFO lpStartupInfo, int[] lpProcessInfo)
        {
            return WinAPI.CreateProcess(lpApplicationName, lpCommandLine, lpProcessAttributes, lpThreadAttributes, bInheritHandles, dwCreationFlags, lpEnvironment, lpCurrentDirectory, ref lpStartupInfo, lpProcessInfo);
        }

        IntPtr IWinAPI.VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect)
        {
            return WinAPI.VirtualAllocEx(hProcess, lpAddress, dwSize, flAllocationType, flProtect);
        }

        uint IWinAPI.NtUnmapViewOfSection(IntPtr hProcess, IntPtr lpBaseAddress)
        {
            return WinAPI.NtUnmapViewOfSection(hProcess, lpBaseAddress);
        }

        int IWinAPI.NtWriteVirtualMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, uint nSize, IntPtr lpNumberOfBytesWritten)
        {
            return WinAPI.NtWriteVirtualMemory(hProcess, lpBaseAddress, lpBuffer, nSize, lpNumberOfBytesWritten);
        }

        int IWinAPI.NtGetContextThread(IntPtr hThread, IntPtr lpContext)
        {
            return WinAPI.NtGetContextThread(hThread, lpContext);
        }

        int IWinAPI.NtSetContextThread(IntPtr hThread, IntPtr lpContext)
        {
            return WinAPI.NtSetContextThread(hThread, lpContext);
        }

        uint IWinAPI.NtResumeThread(IntPtr hThread, IntPtr SuspendCount)
        {
            return WinAPI.NtResumeThread(hThread, SuspendCount);
        }

        #endregion

    }
}