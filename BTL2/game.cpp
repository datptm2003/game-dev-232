#include <iostream>
#include "include/SDL2/SDL.h"
#include "config.cpp"
using namespace std;

// #undef main

int main(int argc, char* args[]) {
    SDL_Window* window = NULL;
    
    //The surface contained by the window
    SDL_Surface* screenSurface = NULL;

    //Initialize SDL
    if (SDL_Init(SDL_INIT_VIDEO) < 0) {
        cout <<  "SDL could not initialize! SDL_Error: " << SDL_GetError() << "\n";
    } else {
        //Create window
        window = SDL_CreateWindow("SDL Tutorial", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, SCREEN_WIDTH, SCREEN_HEIGHT, SDL_WINDOW_SHOWN);
        if (window == NULL) {
            cout << "Window could not be created! SDL_Error: " << SDL_GetError() << "\n";
        }
    }
    return 0;
}
