#include "../Header/game.h"

bool Game::initial()
{
    bool success = true;
    int ret = SDL_Init(SDL_INIT_VIDEO);

    if (ret < 0)
        return false;

    SDL_SetHint(SDL_HINT_RENDER_SCALE_QUALITY, "1");

    // Create window
    gWindow = SDL_CreateWindow("Game SDL2.0", SDL_WINDOWPOS_UNDEFINED,
                               SDL_WINDOWPOS_UNDEFINED, SCREEN_WIDTH,
                               SCREEN_HEIGHT, SDL_WINDOW_SHOWN);

    if (gWindow == NULL)
        success = false;
    else
    {
        gScreen = SDL_CreateRenderer(gWindow, -1, SDL_RENDERER_ACCELERATED);
        if (gScreen == NULL)
            success = false;
        else
        {
            SDL_SetRenderDrawColor(gScreen, RENDER_DRAW_COLOR, RENDER_DRAW_COLOR, RENDER_DRAW_COLOR, RENDER_DRAW_COLOR);
            int imgFlags = IMG_INIT_PNG;
            if (!(IMG_Init(imgFlags) && imgFlags))
                success = false;
        }
    }

    return success;
}

bool Game::loadBackground()
{

    bool ret = gBackground.LoadImg("../assets/images/background.png", gScreen);
    return ret;
}

void Game::close()
{
    gBackground.Free();
    SDL_DestroyRenderer(gScreen);
    gScreen = NULL;
    SDL_DestroyWindow(gWindow);
    gWindow = NULL;

    IMG_Quit();
    SDL_Quit();
}

void Game::start()
{
    if (!initial())
        return;

    if (!loadBackground())
        return;

    GameMap game_map;
    game_map.LoadMap("../assets/map.txt");
    game_map.LoadTiles(gScreen);

    Character p_player;
    p_player.LoadImg("../assets/images/characters/player_left.png", gScreen);
    p_player.setClips();

    bool isQuit = false;
    while (!isQuit)
    {
        while (SDL_PollEvent(&gEvent) != 0)
        {
            if (gEvent.type == SDL_QUIT)
            {
                isQuit = true;
            }
            p_player.handleInputAction(gEvent, gScreen);
        }
        SDL_SetRenderDrawColor(gScreen, RENDER_DRAW_COLOR, RENDER_DRAW_COLOR, RENDER_DRAW_COLOR, RENDER_DRAW_COLOR);
        SDL_RenderClear(gScreen);

        gBackground.Render(gScreen, NULL);
        // game_map.DrawMap(gScreen);

        Map map_data = game_map.getMap();

        game_map.setMap(map_data);
        game_map.DrawMap(gScreen);

        p_player.doPlayer(map_data);
        p_player.show(gScreen);
        SDL_RenderPresent(gScreen);
    }
}