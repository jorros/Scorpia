//
//  soloud_scorpia.hpp
//  soloud-scorpia
//
//  Created by Jörg Rosenauer on 07.10.22.
//

#include "include/soloud_file.h"

#ifndef soloud_scorpia_
#define soloud_scorpia_

/* The classes below are exported */
#pragma GCC visibility push(default)

class ScorpiaFile : SoLoud::File
{
    public:
    void HelloWorld(const char *);
};

#pragma GCC visibility pop
#endif
