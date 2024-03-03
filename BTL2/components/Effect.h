#ifndef EFFECT_H
#define EFFECT_H

#include <iostream>
#include <cstdlib>
#include <math.h>
#include <string.h>

#include "../include/SDL2/SDL.h"
#include "../include/SDL2/SDL_ttf.h"
#include "../include/SDL2/SDL_image.h"

// #include "../config.cpp"

class Effect {
public:
	int x, y, d;
    int type;
    SDL_Renderer * renderer;
	SDL_Rect rect;
    std::string typeList[4] = {"fireball","ice","reversion","snail"};

public:
	Effect() {
        srand(time(NULL));
        d = 45;
        rect.w = d;
        rect.h = d;
        x = rand() % 440 + 20;
        y = rand() % 200 + 20;

        type = -1;

        rect.x = x - rect.w / 2;
        rect.y = y - rect.h / 2;
    }

	~Effect() {}

	void update() {
        rect.w = d;
        rect.h = d;
        rect.x = x - rect.w / 2;
	    rect.y = y - rect.h / 2;
    }

    // bool handleCollision() {
    //     if (rect.x - d / 2 <= 0 || rect.x + d / 2 >= SCREEN_WIDTH) {
    //         dir[0] = -dir[0];
    //     }
    //     if (rect.y - d / 2 <= 0 || rect.y + d / 2 >= SCREEN_HEIGHT) {
    //         dir[1] = -dir[1];
    //     }

    //     return (rect.x - d / 2 <= 0 || rect.x - d / 2 >= SCREEN_WIDTH || rect.y - d / 2 <= 0 || rect.y - d / 2 >= SCREEN_HEIGHT);
    // }



	void render() {
        IMG_Init(IMG_INIT_PNG);
        // Sửa thành đường dẫn tới file trên máy của chị nhe, do chỗ này để đường dẫn tương đối nó ko nhận
        std::string sprite = "F:/STUDY MATERIAL/HK232/LAP TRINH GAME/temp/game-dev-232/BTL2/assets/sprite/effect_" + typeList[type] + ".png";
        SDL_Surface* effectSurface = IMG_Load(sprite.c_str());
        // std::cout << "X\n";
        IMG_Quit();

        SDL_Texture* effectTexture = SDL_CreateTextureFromSurface(renderer, effectSurface);

        SDL_Rect effectRect;                      //create a rect
        effectRect.w = d;                         // controls the width of the rect
        effectRect.h = d;                         // controls the height of the rect
        effectRect.x = rect.x - (effectRect.w / 2);      // controls the rect's x coordinate 
        effectRect.y = rect.y - (effectRect.h / 2);      // controls the rect's y coordinte
        
        // SDL_SetRenderTarget(renderer, messageTexture);
        SDL_RenderCopy(renderer, effectTexture, NULL, &effectRect);
        // std::cout << effectRect.x << ", " << effectRect.y << "\n";
        // SDL_RenderPresent(renderer);
        // SDL_SetRenderTarget(renderer, NULL);
        SDL_FreeSurface(effectSurface);
        SDL_DestroyTexture(effectTexture);
    }



};

#endif