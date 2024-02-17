#ifndef PAUSESCREEN_H
#define PAUSESCREEN_H

#include "../components/TextBox.h"
#include "State.h"

class PauseScreen : public State {
private:
	int pos = 0;
	int n = 3;

	TextBox title;
	TextBox textBoxes[3];

public:
	PauseScreen(SDL_Window* window, SDL_Renderer * renderer) : State(window, renderer) {
		title.renderer = renderer;
        title.size = 60;
        title.color = {87, 196, 229};
        title.x = SCREEN_WIDTH / 2;
        title.y = 200;
        title.message = "PAUSE";

		for (int i = 0; i < n; i++) {
			textBoxes[i].renderer = renderer;
			textBoxes[i].x = SCREEN_WIDTH / 2;
			textBoxes[i].y = 150 + (SCREEN_HEIGHT-((n-1) * 100)) / 2 + i * 75;  
			textBoxes[0].message = "Continue";
			textBoxes[1].message = "Back to title";
			textBoxes[2].message = "Quit";
		}
	}
	~PauseScreen() {}

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
							quit = false;
							back = true;
							n_back = 1;
							break;
						} else if (pos == 1) { 
							quit = false;
							back = true;
							n_back = 2;
							break;
				
						} else {
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
			textBoxes[i].color = { 237, 242, 239 };
			textBoxes[i].size = 24;
		}

		textBoxes[pos].color = { 209, 214, 70 }; //put yellow for the one selected
		textBoxes[pos].size = 42;
    }
	void render() {
        SDL_SetRenderDrawColor(renderer, 33, 39, 56, 255);
		SDL_RenderClear(renderer);

		title.render();
		for (int i = 0; i < n; i++) {
			textBoxes[i].render();
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