#ifndef TITLESCREEN_H
#define TITLESCREEN_H

#include <iostream>
#include "../components/TextBox.h"
#include "State.h"
#include "Playground.h"

class TitleScreen : public State {
private:
	int pos = 0;
	int n = 2;
    TextBox title;
    TextBox subtitle;
	TextBox textBoxes[2];

public:
	TitleScreen(SDL_Window* window, SDL_Renderer* renderer) : State(window, renderer) {
        title.renderer = renderer;
        title.size = 48;
        title.color = {87, 196, 229};
        title.x = SCREEN_WIDTH / 2;
        title.y = 160;
        title.message = "PING-PONG";

        subtitle.renderer = renderer;
        subtitle.size = 60;
        subtitle.color = {87, 255, 129};
        subtitle.x = SCREEN_WIDTH / 2;
        subtitle.y = 230;
        subtitle.message = "TENNIS";

        for (int i = 0;i < n; i++) {
            textBoxes[i].renderer = renderer;
            textBoxes[i].x = SCREEN_WIDTH/2;
            textBoxes[i].y = 150 + (SCREEN_HEIGHT-((n-1) * 100)) / 2 + i * 75; 
        }

        textBoxes[0].message = "Play";
        // textBoxes[1].message = "Options";
        textBoxes[1].message = "Quit";
    }
	~TitleScreen() {}

	int handleEvents(bool &quit, bool &back) {
        SDL_Event event;
        SDL_PollEvent(&event);
        int n_back = 0;

        switch (event.type) {
            case SDL_QUIT:
                quit = true;
                exit();
                break;

            case SDL_KEYDOWN:
                switch (event.key.keysym.sym) {
                    case SDLK_UP:
                        pos -= 1;
                        break;
                    case SDLK_DOWN:
                        pos += 1;
                        break;

                    case SDLK_RETURN:
                        if (pos == 0) {
                            Playground playground = Playground(window,renderer);
                            n_back = playground.loop(quit, back);
                            // std::cout << "t: " << n_back << "\n";
                            if (back && n_back <= 1) {
                                back = false;
                                if (n_back > 0) --n_back;
                            } else --n_back;
                        } 
                        else {
                            quit = true;
                            exit();
                        }
                        
                        break;

                    default:
                        break;
                }
                if (pos == n) pos = 0;
                if (pos < 0) pos = n - 1;
            
                break;


            default:
                break;
        }
        return n_back;
    }
	void update() {
        for (int i = 0;i < n;i++) { //reset all to default
            textBoxes[i].color = {237, 242, 239};
            textBoxes[i].size = 24;
        }

        textBoxes[pos].color = { 209, 214, 70 }; //put yellow for the one selected
        textBoxes[pos].size = 42;
    }
	void render() {
        SDL_SetRenderDrawColor(renderer, 33, 39, 56, 255);
        SDL_RenderClear(renderer);

        renderBackground();
        title.render();
        subtitle.render();

        for (int i = 0;i < n;i++) {
            textBoxes[i].render();
        }

        SDL_RenderPresent(renderer);
    }

    void renderBackground() {
        IMG_Init(IMG_INIT_PNG);
        // Sửa thành đường dẫn tới file trên máy của chị nhe, do chỗ này để đường dẫn tương đối nó ko nhận
        std::string background = "F:/STUDY MATERIAL/HK232/LAP TRINH GAME/temp/game-dev-232/BTL2/assets/background/main_background.png";


        SDL_Surface* backgroundSurface = IMG_Load(background.c_str());
        IMG_Quit();

        SDL_Texture* backgroundTexture = SDL_CreateTextureFromSurface(renderer, backgroundSurface);

        SDL_Rect backgroundRect;                      //create a rect
        backgroundRect.w = SCREEN_WIDTH;                         // controls the width of the rect
        backgroundRect.h = SCREEN_HEIGHT;                         // controls the height of the rect
        backgroundRect.x = 0;      // controls the rect's x coordinate 
        backgroundRect.y = 0;      // controls the rect's y coordinte
        
        // SDL_SetRenderTarget(renderer, messageTexture);
        SDL_RenderCopy(renderer, backgroundTexture, NULL, &backgroundRect);
        // SDL_RenderPresent(renderer);
        // SDL_SetRenderTarget(renderer, NULL);
        SDL_FreeSurface(backgroundSurface);
        SDL_DestroyTexture(backgroundTexture);
    }

    int loop(bool &quit, bool &back) {
        int n_back = 0;
        while (!quit && !back) {
            n_back = handleEvents(quit, back);
            update();
            if (n_back <= 0) render();
        }
        return n_back;
    }

};

#endif