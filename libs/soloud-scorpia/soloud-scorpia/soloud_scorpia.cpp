//
//  soloud_scorpia.cpp
//  soloud-scorpia
//
//  Created by Jörg Rosenauer on 07.10.22.
//

#include <iostream>
#include "include/soloud_file.h"
#include <archive.h>

class ScorpiaFile : public SoLoud::File
{
public:
    ScorpiaFile(char* file, char* name)
    {
        
    }
    
    int eof()
    {
        return 0;
    }
    
    unsigned int read(unsigned char *aDst, unsigned int aBytes)
    {
        return 0;
    }
    
    unsigned int length()
    {
        return 0;
    }
    
    void seek(int aOffset)
    {
    }
    
    unsigned int pos()
    {
        return 0;
    }
};

void* ScorpiaFile_Open(char* file, char* name)
{
    return new ScorpiaFile(file, name);
}
