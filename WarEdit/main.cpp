#include "StdInc.h"

int main(int argc, const char* argv[])
{

	System::Threading::Thread::CurrentThread->CurrentCulture = gcnew System::Globalization::CultureInfo("en-US");
	Console::Title = "WarEdit [FCLD]";

	FCLD* globalFcl = new FCLD("E:\\WarRock\\WarRock EU\\data\\Global.FCL");

	Console::WriteLine("Format: {0}, Version: {1}, FFTag: {2}, EntryCount: {3}",
		gcnew String(globalFcl->Header->Format),
		globalFcl->Header->Version.ToString("0.0"),
		globalFcl->Header->FFTag.ToString("X8"),
		globalFcl->Header->EntryCount.ToString());
	Console::ReadLine();
	return 0;
}