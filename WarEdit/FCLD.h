struct FCLDHeader
{
	char Format[4];
	unsigned int FFTag;
	float Version;
	int EntryCount;
};
struct FCLDEntry
{
	unsigned int FilenameLength;
	char* Filename;
	long FileModifyTime;
	unsigned int FileSize;
	unsigned int Checksum;

	FCLDEntry(char* buffer)
		: FilenameLength(*(DWORD*)buffer),
		FileModifyTime(*(long*)(buffer + FilenameLength + 5)),
		FileSize(*(unsigned int*)(buffer + FilenameLength + 13)),
		Checksum(*(unsigned int*)(buffer + FilenameLength + 17))
	{
		Filename = new char[FilenameLength + 1];
		memcpy(Filename, buffer + 4, FilenameLength);
		Filename[FilenameLength] = 0;
	}
	~FCLDEntry()
	{
		free(this->Filename);
	}

public:
	int Size()
	{
		return FilenameLength + 21;
	}
};

class FCLD
{
private:
	const char* fileName;
	void ProcessFile();
public:
	FCLDHeader* Header;
	FCLDEntry** Entries;
	FCLD(const char* fileName);
	~FCLD();
};