#ifndef PLAYER_H
#define PLAYER_H

#include <iostream>
#include "../include/SDL2/SDL.h"
#include "../include/SDL2/SDL_ttf.h"

class Player {
public:
	int x, y, w, h;
    int speed;
    SDL_Color color;
    SDL_Renderer * renderer;
	SDL_Rect rect;

public:
	Player() {
        rect.w = 64;
        rect.h = 64;
        x = 400;
        y = 320;
        speed = 10;
        rect.x = x - rect.w / 2;
        rect.y = y - rect.h / 2;
    }

	~Player() {}

	void update() {
        rect.x = x - rect.w / 2;
	    rect.y = y - rect.h / 2;
    }
	void render() {
        SDL_SetRenderDrawColor(renderer, color.r, color.g, color.b, color.a);
        SDL_RenderDrawRect(renderer, &rect);
        SDL_RenderFillRect(renderer, &rect);
    }



};

#endif