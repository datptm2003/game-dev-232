
#include "../Header/GameMap.h"

void GameMap::LoadMap(string fileName)
{
    ifstream file;
    file.open(fileName);

    if (file.eof())
        return;

    game_map_.max_x_ = 0;
    game_map_.max_y_ = 0;

    string line;
    for (int i = 0; i < MAX_MAP_Y; i++)
    {
        getline(file, line);
        int j = 0;
        for (int z = 0; z < line.length(); z++)
        {
            if (line[z] == 32)
                continue;
            string num = "";
            num += line[z];
            // cout << num << " ";
            int c = stoi(num);
            game_map_.tile[i][j] = c;
            int val = line[z];
            if (val > 0)
            {
                if (j > game_map_.max_x_)
                    game_map_.max_x_ = j;

                if (i > game_map_.max_y_)
                    game_map_.max_y_ = i;
            }
            j++;
        }
    }

    game_map_.max_x_ = (game_map_.max_x_ + 1) * TILE_SIZE;
    game_map_.max_y_ = (game_map_.max_y_ + 1) * TILE_SIZE;

    game_map_.start_x_ = 0;
    game_map_.start_y_ = 0;

    game_map_.file_name_ = fileName;

    file.close();
}

void GameMap::LoadTiles(SDL_Renderer *screen)
{
    ifstream file;
    for (int i = 0; i < MAX_TILES; i++)
    {
        string fileName = "../assets/images/map/" + to_string(i) + ".png";
        file.open(fileName);

        if (!file.is_open())
            continue;

        // cout << fileName << endl;
        file.close();

        tile_mat[i].LoadImg(fileName, screen);
    }
}

void GameMap::DrawMap(SDL_Renderer *screen)
{
    int x1 = 0, x2 = 0, y1 = 0, y2 = 0;
    int map_x = 0, map_y = 0;

    map_x = game_map_.start_x_ / TILE_SIZE;
    x1 = (game_map_.start_x_ % TILE_SIZE) * (-1);
    x2 = x1 + SCREEN_WIDTH + (x1 == 0 ? 0 : TILE_SIZE);

    map_y = game_map_.start_y_ / TILE_SIZE;
    y1 = (game_map_.start_y_ % TILE_SIZE) * (-1);
    y2 = y1 + SCREEN_HEIGHT + (y1 == 0 ? 0 : TILE_SIZE);

    for (int i = y1; i < y2; i += TILE_SIZE)
    {
        map_x = game_map_.start_x_ / TILE_SIZE;
        for (int j = x1; j < x2; j += TILE_SIZE)
        {
            int val = game_map_.tile[map_y][map_x];
            if (val > 0)
            {
                tile_mat[val].setRect(j, i);
                tile_mat[val].Render(screen, NULL);
            }
            map_x++;
        }
        map_y++;
    }
}