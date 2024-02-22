#ifndef PLAYER_H
#define PLAYER_H

#include <iostream>
#include "../include/SDL2/SDL.h"
#include "../include/SDL2/SDL_ttf.h"

class Player {
public:
	int x, y;
    int speed;
    double rot_speed;
    double angle;
    SDL_Color color;
    SDL_Renderer * renderer;
	SDL_Rect rect;

public:
	Player() {
        rect.w = 64;
        rect.h = 64;
        x = 240;
        y = 400;
        speed = 10;
        rot_speed = 5;
        angle = 0;
        rect.x = x - rect.w / 2;
        rect.y = y - rect.h / 2;
    }

	~Player() {}

	void update() {
        rect.x = x - rect.w / 2;
	    rect.y = y - rect.h / 2;
    }
	void render() {
        SDL_Surface * playerSurface = SDL_CreateRGBSurface(0, rect.w, rect.h, 32, 0xFF000000, 0x00FF0000, 0x0000FF00, 0x000000FF);
        // std::cout << playerSurface << "\n";
        SDL_FillRect(playerSurface,NULL, (color.r << 24) + (color.g << 16) + (color.b << 8) + (color.a));
        SDL_Texture* playerTexture = SDL_CreateTextureFromSurface(renderer, playerSurface);

        SDL_RenderCopyEx(renderer, playerTexture, NULL, &rect, angle, NULL, SDL_FLIP_NONE);
        SDL_FreeSurface(playerSurface);
        SDL_DestroyTexture(playerTexture);

        // SDL_SetRenderDrawColor(renderer, color.r, color.g, color.b, color.a);
        // SDL_RenderDrawRect(renderer, &rect);
        // SDL_RenderFillRect(renderer, &rect);
    }



};

#endif