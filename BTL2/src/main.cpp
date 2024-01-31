#include <iostream>
#include "../Header/game.h"

using namespace std;

int main()
{
    Game *game = new Game();
    game->start();

    cout << "Hello World!" << endl;
    return 0;
}