#ifndef PLAYGROUND_H
#define PLAYGROUND_H

#include <iostream>
#include <ctime>
#include "State.h"
#include "PauseScreen.h"
#include "../components/Player.h"
#include "../components/Ball.h"

class Playground : public State {
private:
    Player p1, p2;
    Ball ball;
    const Uint8* keyStates = SDL_GetKeyboardState(NULL);
    int intro = 3;
    clock_t start_intro;

    TextBox countdown;
    bool first_time;

public:
	Playground(SDL_Window* window, SDL_Renderer * renderer) : State(window, renderer) {
        countdown.renderer = renderer;
        countdown.size = 60;
        countdown.color = {0, 0, 0};
        countdown.x = SCREEN_WIDTH / 2;
        countdown.y = 300;
        countdown.message = std::to_string(intro);

        p1.renderer = renderer;
        p2.renderer = renderer;
        ball.renderer = renderer;

        p1.x = 160;
        p1.y = 400;
        p1.rect.w = 72;
        p1.rect.h = 20;
        p1.color = {249, 112, 104,255};

        p2.x = 320;
        p2.y = 400;
        p2.rect.w = 72;
        p2.rect.h = 20;
        p2.color = {209, 214, 70, 255};

        start_intro = clock();
        first_time = true;
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
        if (intro > 0) return 0;
        if (keyStates[SDL_SCANCODE_S]) {
            p1.y += p1.speed;
        }
        if (keyStates[SDL_SCANCODE_W]) {
            p1.y -= p1.speed;
        }
        if (keyStates[SDL_SCANCODE_A]) {
            p1.x -= p1.speed;
        }
        if (keyStates[SDL_SCANCODE_D]) {
            p1.x += p1.speed;
        }
        if (keyStates[SDL_SCANCODE_V]) {
            p1.angle -= p1.rot_speed;
        }
        if (keyStates[SDL_SCANCODE_B]) {
            p1.angle += p1.rot_speed;
        }

        if (keyStates[SDL_SCANCODE_DOWN]) {
            p2.y += p2.speed;
        }
        if (keyStates[SDL_SCANCODE_UP]) {
            p2.y -= p2.speed;
        }
        if (keyStates[SDL_SCANCODE_LEFT]) {
            p2.x -= p2.speed;
        }
        if (keyStates[SDL_SCANCODE_RIGHT]) {
            p2.x += p2.speed;
        }
        if (keyStates[SDL_SCANCODE_K]) {
            p2.angle -= p2.rot_speed;
        }
        if (keyStates[SDL_SCANCODE_L]) {
            p2.angle += p2.rot_speed;
        }

        return n_back;
    }
	void update() {
        if (intro == 0) {
            first_time = false;

            p1.update();
            p2.update();
            handleCollision();
            // std::cout << ":: " << ball.x << "," << ball.y << "\n";
            ball.x += ball.speed*ball.dir[0];
            ball.y += ball.speed*ball.dir[1];
            
            ball.update();
        }
        
    }

    void handleCollision() {
        if (ball.rect.x - ball.d / 2 <= 0 || ball.rect.x + ball.d / 2 >= SCREEN_WIDTH) {
            ball.dir[0] = -ball.dir[0];
        }
        if (ball.rect.y - ball.d / 2 <= 0) {
            ball.dir[1] = -ball.dir[1];
        }
        if (ball.rect.y + ball.d / 2 >= SCREEN_HEIGHT) {
            intro = 3;
            countdown.message = std::to_string(intro);
            ball.x = 240;
            ball.y = 220;
            // ball.rect.x = ball.x - ball.rect.w / 2;
            // ball.rect.y = ball.y - ball.rect.h / 2;
            ball.dir[0] = -(rand() % 100)*1.0 / 100;
            ball.dir[1] = -sqrt(1 - ball.dir[0]*ball.dir[0]);

            p1.x = 160;
            p1.y = 400;
            p1.rect.x = p1.x - p1.rect.w / 2;
            p1.rect.y = p1.y - p1.rect.h / 2;
            p1.angle = 0;

            p2.x = 320;
            p2.y = 400;
            p2.rect.x = p2.x - p2.rect.w / 2;
            p2.rect.y = p2.y - p2.rect.h / 2;
            p2.angle = 0;

            start_intro = clock();
        }

        // return (ball.rect.x - ball.d / 2 <= 0 || ball.rect.x - ball.d / 2 >= SCREEN_WIDTH || ball.rect.y - ball.d / 2 <= 0 || ball.rect.y - ball.d / 2 >= SCREEN_HEIGHT);
    }

    
	void render() {
        SDL_SetRenderDrawColor(renderer, 237, 242, 239, 255);
        SDL_RenderClear(renderer);

        if (!first_time) {
            p1.render();
            p2.render();
            ball.render();
        }

        if (intro > 0) {
            // SDL_SetRenderDrawColor(renderer, 237, 242, 239, 255);
            // SDL_RenderClear(renderer);

            countdown.render();
            clock_t now = clock();
            if (now - start_intro >= 1000*(4 - intro)) {
                countdown.message = std::to_string(intro - 1);
                intro--;
            }

            // p1.render();
            // p2.render();
            // ball.render();
            // SDL_RenderPresent(renderer);
            // std::cout << intro << "\n";
        }
        
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