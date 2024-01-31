
#ifndef GAME_H_
#define GAME_H_

#include "CommonFunc.h"
#include "BaseObject.h"
#include "GameMap.h"
#include "Character.h"

class Game
{
private:
    BaseObject gBackground;

public:
    Game() { ; }
    ~Game() { ; }

    bool initial();
    bool loadBackground();
    void close();
    void start();
};

#endif