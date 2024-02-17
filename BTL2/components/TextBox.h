#ifndef TEXTBOX_H
#define TEXTBOX_H

#include "../include/SDL2/SDL.h"
#include "../include/SDL2/SDL_ttf.h"
#include <string>
#include <iostream>

class TextBox {
public:
    int x, y, size;
    std::string font;
    SDL_Color color;
    std::string message;
    SDL_Renderer * renderer;

public:
    TextBox() {
        // this->renderer = renderer;
        x = 0;
        y = 0;
        font = "assets/font/OpenSans-Regular.ttf";
        size = 24;
        color = {237, 242, 239};
        message = "Type your text";
    }

    TextBox(SDL_Renderer * renderer, int x, int y, int size, SDL_Color color, std::string message) {
        // this->renderer = renderer;
        this->x = x;
        this->y = y;
        this->size = size;
        this->color = color;
        this->message = message;
    }

    void render() {
        TTF_Init();
        TTF_Font* sans = TTF_OpenFont(font.c_str(), size);
        SDL_Surface* messageSurface = TTF_RenderText_Solid(sans, message.c_str(), color);
        TTF_CloseFont(sans);
        TTF_Quit();

        SDL_Texture* messageTexture = SDL_CreateTextureFromSurface(renderer, messageSurface);
        SDL_Rect messageRect; //create a rect
        messageRect.w = messageSurface->w;          // controls the width of the rect
        messageRect.h = messageSurface->h;          // controls the height of the rect
        messageRect.x = x - (messageRect.w / 2);    // controls the rect's x coordinate 
        messageRect.y = y - (messageRect.h / 2);    // controls the rect's y coordinte

        // SDL_SetRenderTarget(renderer, messageTexture);
        SDL_RenderCopy(renderer, messageTexture, NULL, &messageRect);
        // SDL_RenderPresent(renderer);
        // SDL_SetRenderTarget(renderer, NULL);
        SDL_FreeSurface(messageSurface);
        SDL_DestroyTexture(messageTexture);
    }

    ~TextBox() {}
};

#endif