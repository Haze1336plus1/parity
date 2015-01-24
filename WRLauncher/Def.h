struct STARTUPINFO
{
	int cb;
	char* lpReserved;
	char* lpDesktop;
	char* lpTitle;
	unsigned int dwX;
	unsigned int dwY;
	unsigned int dwXSize;
	unsigned int dwYSize;
	unsigned int dwXCountChars;
	unsigned int dwYCountChars;
	unsigned int dwFillAttribute;
	unsigned int dwFlags;
	unsigned int wShowWindow;
	unsigned short cbReserved2;
	void* lpReserved2;
	void* hStdInput;
	void* hStdOutput;
	void* hStdError;
};