#ifndef BALL_H
#define BALL_H

#include <iostream>
#include <cstdlib>
#include <math.h>

#include "../include/SDL2/SDL.h"
#include "../include/SDL2/SDL_ttf.h"
#include "../include/SDL2/SDL_image.h"

class Ball {
public:
	int x, y, d;
    int speed;
    double dir[2];
    SDL_Renderer * renderer;
	SDL_Rect rect;

public:
	Ball() {
        srand(time(NULL));
        d = 16;
        rect.w = d;
        rect.h = d;
        x = 240;
        y = 220;
        speed = 5;
        dir[0] = -(rand() % 100)*1.0 / 100;
        dir[1] = -sqrt(1 - dir[0]*dir[0]);
        std::cout << dir[0] << ", " << dir[1] << "\n";
        rect.x = x - rect.w / 2;
        rect.y = y - rect.h / 2;
    }

	~Ball() {}

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
        std::string sprite = "F:/STUDY MATERIAL/HK232/LAP TRINH GAME/game-dev-232/BTL2/assets/sprite/ball.png";
        SDL_Surface* ballSurface = IMG_Load(sprite.c_str());
        IMG_Quit();

        SDL_Texture* ballTexture = SDL_CreateTextureFromSurface(renderer, ballSurface);

        SDL_Rect ballRect;                      //create a rect
        ballRect.w = d;                         // controls the width of the rect
        ballRect.h = d;                         // controls the height of the rect
        ballRect.x = rect.x - (ballRect.w / 2);      // controls the rect's x coordinate 
        ballRect.y = rect.y - (ballRect.h / 2);      // controls the rect's y coordinte

        ballRect.x += speed*dir[0];
        ballRect.y += speed*dir[1];
        
        // SDL_SetRenderTarget(renderer, messageTexture);
        SDL_RenderCopy(renderer, ballTexture, NULL, &ballRect);
        // SDL_RenderPresent(renderer);
        // SDL_SetRenderTarget(renderer, NULL);
        SDL_FreeSurface(ballSurface);
        SDL_DestroyTexture(ballTexture);
    }



};

#endif