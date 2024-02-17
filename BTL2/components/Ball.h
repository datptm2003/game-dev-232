#ifndef BALL_H
#define BALL_H

#include <iostream>
#include "../include/SDL2/SDL.h"
#include "../include/SDL2/SDL_ttf.h"
#include "../include/SDL2/SDL_image.h"

class Ball {
public:
	int x, y, r;
    int speed;
    SDL_Color color;
    SDL_Renderer * renderer;
	SDL_Rect rect;

public:
	Ball() {
        r = 16;
        rect.w = r;
        rect.h = r;
        x = 200;
        y = 320;
        speed = 10;
        rect.x = x - rect.w / 2;
        rect.y = y - rect.h / 2;
    }

	~Ball() {}

	void update() {
        rect.w = r;
        rect.h = r;
        rect.x = x - rect.w / 2;
	    rect.y = y - rect.h / 2;
    }
	void render() {
        IMG_Init(IMG_INIT_PNG);
        // Sửa thành đường dẫn tới file trên máy của chị nhe, do chỗ này để đường dẫn tương đối nó ko nhận
        std::string sprite = "F:/STUDY MATERIAL/HK232/LAP TRINH GAME/game-dev-232/BTL2/assets/ball.png";
        SDL_Surface* ballSurface = IMG_Load(sprite.c_str());
        IMG_Quit();

        SDL_Texture* ballTexture = SDL_CreateTextureFromSurface(renderer, ballSurface);

        SDL_Rect ballRect;                      //create a rect
        ballRect.w = r;                         // controls the width of the rect
        ballRect.h = r;                         // controls the height of the rect
        ballRect.x = rect.x - (ballRect.w / 2);      // controls the rect's x coordinate 
        ballRect.y = rect.y - (ballRect.h / 2);      // controls the rect's y coordinte

        // SDL_SetRenderTarget(renderer, messageTexture);
        SDL_RenderCopy(renderer, ballTexture, NULL, &ballRect);
        // SDL_RenderPresent(renderer);
        // SDL_SetRenderTarget(renderer, NULL);
        SDL_FreeSurface(ballSurface);
        SDL_DestroyTexture(ballTexture);
    }



};

#endif