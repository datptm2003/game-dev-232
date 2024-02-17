#ifndef STATE_H
#define STATE_H

#include <iostream>
#include "../include/SDL2/SDL.h"
#include "../include/SDL2/SDL_ttf.h"
#include "../config.cpp"

class State {
public:
    SDL_Window* window;
	SDL_Renderer * renderer;

public:
	State(SDL_Window* window, SDL_Renderer * renderer) {
        this->window = window;
        this->renderer = renderer;
    }
	~State() {}

	int handleEvents(bool &quit, bool &back) {
        return 0;
    }
	void update() {

    }
	void render() {
        
    }
    int loop(bool &quit, bool &back) {
        return 0;
    }

    void exit() {
        SDL_DestroyWindow(window);
        SDL_DestroyRenderer(renderer);
        SDL_Quit();
    }

};

#endif