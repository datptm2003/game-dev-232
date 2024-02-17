#ifndef PLAYGROUND_H
#define PLAYGROUND_H

#include <iostream>
#include "State.h"
#include "PauseScreen.h"
#include "../components/Player.h"
#include "../components/Ball.h"

class Playground : public State {
private:
    Player p1, p2;
    Ball ball;
    const Uint8* keyStates = SDL_GetKeyboardState(NULL);

public:
	Playground(SDL_Window* window, SDL_Renderer * renderer) : State(window, renderer) {
        p1.renderer = renderer;
        p2.renderer = renderer;
        ball.renderer = renderer;

        p1.x = 200;
        p1.y = 100;
        p1.rect.w = 128;
        p1.rect.h = 24;
        p1.color = {249, 112, 104,255};

        p2.x = 300;
        p2.y = 400;
        p2.rect.w = 128;
        p2.rect.h = 24;
        p2.color = {209, 214, 70,255};
    }
	~Playground() {}

	int handleEvents(bool &quit, bool &back) {
        SDL_Event event; //Event handling
	
        SDL_PollEvent(&event);
        int n_back = 0;

        switch (event.type) {
            case SDL_QUIT:
                quit = true;
                exit();
                break;

            case SDL_KEYDOWN:
                if (event.key.keysym.sym == SDLK_ESCAPE) {
                    PauseScreen pause = PauseScreen(window,renderer);
                    n_back = pause.loop(quit, back);
                    // std::cout << "p: " << n_back << "\n";
                    if (back && n_back <= 1) {
                        back = false;
                        if (n_back > 0) --n_back;
                    } else --n_back;
                }
                break;

            default:
                break;
            
        }

        // Keystate handling
        if (keyStates[SDL_SCANCODE_DOWN]) {
            p1.y += p1.speed;
        }
        if (keyStates[SDL_SCANCODE_UP]) {
            p1.y -= p1.speed;
        }
        if (keyStates[SDL_SCANCODE_LEFT]) {
            p1.x -= p1.speed;
        }
        if (keyStates[SDL_SCANCODE_RIGHT]) {
            p1.x += p1.speed;
        }

        if (keyStates[SDL_SCANCODE_S]) {
            p2.y += p2.speed;
        }
        if (keyStates[SDL_SCANCODE_W]) {
            p2.y -= p2.speed;
        }
        if (keyStates[SDL_SCANCODE_A]) {
            p2.x -= p2.speed;
        }
        if (keyStates[SDL_SCANCODE_D]) {
            p2.x += p2.speed;
        }

        return n_back;
    }
	void update() {
        p1.update();
        p2.update();
        ball.update();
    }
	void render() {
        SDL_SetRenderDrawColor(renderer, 237, 242, 239, 255);
        SDL_RenderClear(renderer);

        p1.render();
        p2.render();
        ball.render();
        
        SDL_RenderPresent(renderer);
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