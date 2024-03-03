#ifndef PLAYGROUND_H
#define PLAYGROUND_H

#include <iostream>
#include <ctime>
#include "State.h"
#include "PauseScreen.h"
#include "../components/Player.h"
#include "../components/Ball.h"
#include "../components/Effect.h"

enum CollisionDirection {
    NO_COLLISION,
    FROM_TOP,
    FROM_BOTTOM,
    FROM_LEFT,
    FROM_RIGHT
};

struct CollisionInfo {
    CollisionDirection direction;
    Player pushingPlayer;
    Player pushedPlayer;
};

CollisionInfo checkPaddleCollision(Player p1, Player p2) {
    int leftA, leftB;
    int rightA, rightB;
    int topA, topB;
    int bottomA, bottomB;

    leftA = p1.rect.x;
    rightA = leftA + p1.rect.w;
    topA = p1.rect.y;
    bottomA = topA + p1.rect.h;

    leftB = p2.rect.x;
    rightB = leftB + p2.rect.w;
    topB = p2.rect.y;
    bottomB = topB + p2.rect.h;

    if (bottomA <= topB || topA >= bottomB || rightA <= leftB || leftA >= rightB) {
        return { NO_COLLISION, p1, p2 };
    }

    int overlapX = std::min(rightA, rightB) - std::max(leftA, leftB);
    int overlapY = std::min(bottomA, bottomB) - std::max(topA, topB);

    CollisionDirection direction;
    if (overlapX < overlapY) {
        direction = (topA < topB) ? FROM_BOTTOM : FROM_TOP;
    } else {
        direction = (leftA < leftB) ? FROM_RIGHT : FROM_LEFT;
    }

    return { direction, (direction == FROM_BOTTOM || direction == FROM_RIGHT) ? p1 : p2, (direction == FROM_BOTTOM || direction == FROM_RIGHT) ? p2 : p1 };
}

class Playground : public State {
private:
    Player p1, p2;
    Ball ball;
    Effect effect[2];
    // int effect[2];
    // int effectPos[2][2];
    const Uint8* keyStates = SDL_GetKeyboardState(NULL);
    int intro = 3;
    clock_t start_intro;
    clock_t lastEffect;
    clock_t startFreezing;

    TextBox countdown;
    bool first_time;
    bool isPlayer1Turn;
    bool startCountDown;
    bool gapEffect;
    bool collideVerticalWall;
    bool collideHorizontalWall;
    bool stopMovement;

public:
	Playground(SDL_Window* window, SDL_Renderer * renderer) : State(window, renderer) {
        srand(time(NULL));

        countdown.renderer = renderer;
        countdown.size = 60;
        countdown.color = {0, 0, 0};
        countdown.x = SCREEN_WIDTH / 2;
        countdown.y = 300;
        countdown.message = std::to_string(intro);

        p1.renderer = renderer;
        p2.renderer = renderer;
        ball.renderer = renderer;
        effect[0].renderer = renderer;
        effect[1].renderer = renderer;

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
        isPlayer1Turn = true;
        startCountDown = false;
        gapEffect = false;
        stopMovement = false;

        collideVerticalWall = false;
        collideHorizontalWall = false;
    }
	~Playground() {}

	int handleEvents(bool &quit, bool &back) {
        SDL_Event event; //Event handling
        clock_t now = clock();

        if (stopMovement && now - startFreezing >= 3000) {
            stopMovement = false;
            std::cout << "AA\n";
        }
	
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
        // std::cout << !stopMovement << ", " << !isPlayer1Turn << "\n";
        // std::cout << "****" << keyStates[SDL_SCANCODE_S] << "\n";
        if (keyStates[SDL_SCANCODE_S]) {
            if (!isPlayer1Turn || !stopMovement) {
                p1.y += p1.speed;
            }
        }
        if (keyStates[SDL_SCANCODE_W]) {
            if (!isPlayer1Turn || !stopMovement) {
                p1.y -= p1.speed;
            }
        }
        if (keyStates[SDL_SCANCODE_A]) {
            if (!isPlayer1Turn || !stopMovement) {
                p1.x -= p1.speed;
            }
        }
        if (keyStates[SDL_SCANCODE_D]) {
            if (!isPlayer1Turn || !stopMovement) {
                p1.x += p1.speed;
            }
        }
        if (keyStates[SDL_SCANCODE_V]) {
            if (!isPlayer1Turn || !stopMovement) p1.angle -= p1.rot_speed;
        }
        if (keyStates[SDL_SCANCODE_B]) {
            if (!isPlayer1Turn || !stopMovement) p1.angle += p1.rot_speed;
        }
        
        
        if (keyStates[SDL_SCANCODE_DOWN]) {
            if (isPlayer1Turn || !stopMovement) {
                p2.y += p2.speed;
            }
        }
        if (keyStates[SDL_SCANCODE_UP]) {
            if (isPlayer1Turn || !stopMovement) p2.y -= p2.speed;
        }
        if (keyStates[SDL_SCANCODE_LEFT]) {
            if (isPlayer1Turn || !stopMovement) p2.x -= p2.speed;
        }
        if (keyStates[SDL_SCANCODE_RIGHT]) {
            if (isPlayer1Turn || !stopMovement) p2.x += p2.speed;
        }
        if (keyStates[SDL_SCANCODE_K]) {
            if (isPlayer1Turn || !stopMovement) p2.angle -= p2.rot_speed;
        }
        if (keyStates[SDL_SCANCODE_L]) {
            if (isPlayer1Turn || !stopMovement) p2.angle += p2.rot_speed;
        }
        
        

        return n_back;
    }
	void update() {
        if (intro == 0) {
            first_time = false;

            p1.update();
            p2.update();
            handleCollision();
            ball.x += ball.speed*ball.dir[0];
            ball.y += ball.speed*ball.dir[1];

            ball.update();
        }
    }
    float degToRad(float degrees) {
        return degrees * (M_PI / 180.0f);
    }

    bool checkCollision(Player p) {
        float ballRadius = ball.d / 2;
        float playerHalfWidth = p.rect.w / 2;
        float playerHalfHeight = p.rect.h / 2;

        float dx = ball.rect.x - (p.rect.x + playerHalfWidth);
        float dy = ball.rect.y - (p.rect.y + playerHalfHeight);

        float rotatedX = cos(-p.angle) * dx - sin(-p.angle) * dy;
        float rotatedY = sin(-p.angle) * dx + cos(-p.angle) * dy;

        float overlapX = ballRadius + playerHalfWidth - std::abs(rotatedX);
        float overlapY = ballRadius + playerHalfHeight - std::abs(rotatedY);

        bool xOverlap = overlapX > 0;
        bool yOverlap = overlapY > 0;

        if (xOverlap && yOverlap) {
            float collisionX = cos(p.angle) * rotatedX - sin(p.angle) * rotatedY + (p.rect.x + playerHalfWidth);
            float collisionY = sin(p.angle) * rotatedX + cos(p.angle) * rotatedY + (p.rect.y + playerHalfHeight);

            if (overlapX < overlapY) {
                if (rotatedX > 0) {
                    ball.rect.x += overlapX;
                } else {
                    ball.rect.x -= overlapX;
                }
            double temp = ball.dir[0];
            ball.dir[0] = cos(acos(temp) - 2 * degToRad(p.angle));
            ball.dir[1] = -sin(acos(temp) - 2 * degToRad(p.angle));
            ball.rect.x = 2 * collisionX - ball.rect.x;

            } else {
                if (rotatedY > 0) {
                    ball.rect.y += overlapY;
                } else {
                    ball.rect.y -= overlapY;
                }

            double temp = ball.dir[0];
            ball.dir[0] = cos(acos(temp) - 2 * degToRad(p.angle));
            ball.dir[1] = -sin(acos(temp) - 2 * degToRad(p.angle));
            ball.rect.y = 2 * collisionY - ball.rect.y;
            }

            return true;
        }

        return false;
}
    void handleCollision() {
        if (!collideVerticalWall && (ball.rect.x - ball.d / 2 <= 0 || ball.rect.x + ball.d / 2 >= SCREEN_WIDTH)) {
            ball.dir[0] = -ball.dir[0];
            collideVerticalWall = true;
        } else collideVerticalWall = false;
        if (!collideHorizontalWall && (ball.rect.y - ball.d / 2 <= 0)) {
            ball.dir[1] = -ball.dir[1];
            collideHorizontalWall = true;
        } else collideHorizontalWall = false;

        if(checkCollision(p2)) {

            // ball.dir[1] = -ball.dir[1];
            isPlayer1Turn = true;
            // std::cout << isPlayer1Turn << "++\n";
            // if(!isPlayer1Turn) 
            // {
            //     p2.score++;
            //     startCountDown = true;
            // }
        }
        else if(checkCollision(p1))
        {
            // ball.dir[1] = -ball.dir[1];
            isPlayer1Turn = false;
            // std::cout << isPlayer1Turn << "++\n";
            // if (isPlayer1Turn){
            //     p1.score++;
            //     startCountDown = true;
            // }
        }
        if (checkPaddleCollision(p1, p2).direction != NO_COLLISION) {
            CollisionInfo collisionInfo = checkPaddleCollision(p1, p2);
            switch (collisionInfo.direction) {
                case FROM_TOP:
                    break;
                case FROM_BOTTOM:
                    break;
                case FROM_LEFT:
                    if (collisionInfo.pushingPlayer == p2) {
                    p1.x += (p1.rect.x);
                    }
                    p2.x += (p2.rect.x);
                    break;
                case FROM_RIGHT:
                    if (collisionInfo.pushingPlayer == p2) {
                        p1.x -= (p2.rect.x);
                    }
                    p2.x -= p1.rect.x;
                    break;
                default:
                    break;
            }
        }

        for (int i = 0; i < 2; ++i) {
            if (effect[i].type != -1) {
                if (
                    (ball.rect.x < effect[i].rect.x + effect[i].rect.w && ball.rect.x + ball.d/2 > effect[i].rect.x) && 
                    (ball.rect.y < effect[i].rect.y + effect[i].rect.h && ball.rect.y + ball.d/2 > effect[i].rect.y)
                ) {
                    // std::cout << "AAA\n";
                    
                    if (effect[i].type == 0) {
                        ball.speed *= 2;
                        // std::cout << "BBB\n";
                    } else if (effect[i].type == 1) {
                        stopMovement = true;
                        startFreezing = clock();
                    } else if (effect[i].type == 2) {
                        ball.dir[0] = -ball.dir[0];
                    } else {
                        if (isPlayer1Turn) {
                            p1.speed = p1.speed / 2 - 1;
                        } else {
                            p2.speed = p2.speed / 2 - 1;
                        }
                    }
                    effect[i].type = -1;
                    lastEffect = clock();
                }
            }
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

            ball.speed = 8;
            p1.speed = p2.speed = 10;
            start_intro = clock();
        }

        // return (ball.rect.x - ball.d / 2 <= 0 || ball.rect.x - ball.d / 2 >= SCREEN_WIDTH || ball.rect.y - ball.d / 2 <= 0 || ball.rect.y - ball.d / 2 >= SCREEN_HEIGHT);
    }

    void renderPlayerScores() {
        int scoreX = SCREEN_WIDTH / 2;
        int scoreY = 20;

        TextBox player1ScoreText;
        player1ScoreText.renderer = renderer;
        player1ScoreText.x = scoreX - 100;
        player1ScoreText.y = scoreY;
        player1ScoreText.size = 24;
        player1ScoreText.color = {0, 0, 0};
        player1ScoreText.message = "Player 1: " + std::to_string(p1.score);
        player1ScoreText.render();

        TextBox player2ScoreText;
        player2ScoreText.renderer = renderer;
        player2ScoreText.x = scoreX + 80;
        player2ScoreText.y = scoreY;
        player2ScoreText.size = 24;
        player2ScoreText.color = {0, 0, 0};
        player2ScoreText.message = "Player 2: " + std::to_string(p2.score);
        player2ScoreText.render();
    }

	void render() {
        SDL_SetRenderDrawColor(renderer, 237, 242, 239, 255);
        SDL_RenderClear(renderer);

        if (!first_time) {
            for (int i = 0; i < 2; ++i) {
                // std::cout << effect[i] << "\n";
                if (effect[i].type != -1) {
                    // std::cout << "---" << "\n";
                    // Effect newEffect;
                    // newEffect.renderer = renderer;
                    // newEffect.type = effect[i];
                    // newEffect.rect.x = effectPos[i][0];
                    // newEffect.rect.y = effectPos[i][1];
                    // std::cout << "BBB\n";
                    effect[i].render();
                    // std::cout << "CCC\n";
                    if (!gapEffect) {
                        lastEffect = clock();
                        gapEffect = true;
                    }
                    // else gapEffect = !gapEffect;
                }
            }
            p1.render();
            p2.render();
            ball.render();
            
            
        }
        if (intro == 0) {
            clock_t now = clock();
            int seed = (rand() % 100);
            // std::cout << "---" << now - lastEffect << "\n";
            if (seed < 10 && now - lastEffect >= 3000) {
                // std::cout << "+++" << seed << "\n";
                for (int i = 0; i < 2; ++i) {
                    if (effect[i].type == -1) {
                        // std::cout << "***\n";
                        // effect[i] = rand() % 4;
                        // effectPos[i][0] = rand() % 440 + 20;
                        // effectPos[i][1] = rand() % 200 + 20;
                        effect[i].type = rand() % 4;
                        effect[i].x = rand() % 380 + 50;
                        effect[i].y = rand() % 120 + 50;
                        // std::cout << effect[i].x << ", " << effect[i].y << "\n";
                        effect[i].update();
                        gapEffect = false;
                        break;
                    }
                    
                }
                
            }
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
            lastEffect = clock();
            gapEffect = false;
            // p1.render();
            // p2.render();
            // ball.render();
            // SDL_RenderPresent(renderer);
        }
        renderPlayerScores();
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