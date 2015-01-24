#include "stdafx.h"
#include "Parity.LowLevel.WRBin.h"

String^ Parity::LowLevel::WRBin::Decrypt(String^ input)
{
	if(input->Length % 2 > 0)
		throw gcnew Exception("input->Length % 2 must be 0!");
	
	int inputLen = input->Length / 2;
	array<Byte>^ outData = gcnew array<Byte>(inputLen);
	
	for(int i = 0; i < inputLen; i++)
	{
		int j = i * 2;
		char keyA = input[j];
		char keyB = input[j + 1];
		outData[i] = (Byte)(((WRBin::hashmapDec[keyA] << 4) | WRBin::hashmapDec[keyB]) ^ WRBin::Key);
	}
	
	return Text::Encoding::UTF8->GetString(outData);
	
}

String^ Parity::LowLevel::WRBin::Encrypt(String^ input)
{
	array<Byte>^ outData = gcnew array<Byte>(input->Length * 2);
	//TODO: Continue on LowLevel library, finish this
	return Text::Encoding::UTF8->GetString(outData);
}