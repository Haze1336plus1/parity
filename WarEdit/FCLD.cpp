#include "StdInc.h"

FCLD::FCLD(const char* fileName)
{
	this->fileName = fileName;
	this->ProcessFile();
}
FCLD::~FCLD()
{
	delete[] this->Entries;
	delete this->Header;
}

void FCLD::ProcessFile()
{
	char* buffer;
	bool fileRead;
	FILE* fcldFile = fopen(this->fileName, "rb");
	if(fcldFile)
	{
		//Get file length
		fseek(fcldFile, 0, SEEK_END);
		long fileLen = ftell(fcldFile);
		fseek(fcldFile, 0, SEEK_SET);

		//Allocate memory
		buffer = (char*)malloc(fileLen + 1);
		fread(buffer, fileLen, sizeof(char), fcldFile);
		fclose(fcldFile);

		fileRead = true;
	}
	if(fileRead)
	{
		this->Header = new FCLDHeader();
		memcpy(this->Header, buffer, sizeof(FCLDHeader));
		this->Entries = new FCLDEntry*[this->Header->EntryCount];
		int memIndex = sizeof(FCLDHeader);
		for(int i = 0; i < this->Header->EntryCount; i++)
		{
			FCLDEntry* entry = new FCLDEntry(buffer + memIndex);
			this->Entries[i] = entry;
			memIndex += entry->Size();
		}
		free(buffer);
	}
}