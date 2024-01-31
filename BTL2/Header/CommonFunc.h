
#ifndef COMMON_FUNCTION_H_
#define COMMON_FUNCTION_H_

#include <iostream>
#include <string>
#include <fstream>
#include "SDL2/SDL.h"
#include "SDL2/SDL_image.h"
#include "SDL2/SDL_mixer.h"
#include "SDL2/SDL_ttf.h"

using namespace std;

static SDL_Window *gWindow = NULL;
static SDL_Renderer *gScreen = NULL;
static SDL_Event gEvent;

// Screen: 1550 - 800
const int SCREEN_WIDTH = 1260;
const int SCREEN_HEIGHT = 720;
const int SCREEN_BPP = 32;

const int COLOR_KEY_R = 167;
const int COLOR_KEY_G = 175;
const int COLOR_KEY_B = 180;

const int RENDER_DRAW_COLOR = 255;

// 50 - 31 - 16
#define BLANK_TILE 0
#define TILE_SIZE 30

#define MAX_MAP_X 42
#define MAX_MAP_Y 24

struct Input
{
    int left_;
    int right_;
    int up_;
    int down_;
};

// Đối tượng dữ liệu cho 1 ô
struct Map
{
    int start_x_;
    int start_y_;

    int max_x_;
    int max_y_;

    // Luu toan bo trang thai cua o
    int tile[MAX_MAP_Y][MAX_MAP_X];
    string file_name_;
};

#endif