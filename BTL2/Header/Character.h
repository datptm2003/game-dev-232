
#ifndef CHARACTER_H_
#define CHARACTER_H_

#include "CommonFunc.h"
#include "BaseObject.h"

class Character : public BaseObject
{
private:
    float x_val_;
    float y_val_;

    float x_pos_;
    float y_pos_;

    // width and height of 1 frame
    int width_frame_;
    int height_frame_;

    // Cac frame
    SDL_Rect frame_clip_[8];

    // Dang trong action nao?
    Input input_type_;

    // Dang o frame nao?
    int frame_;

    // Dang di chuyen qua phai~ hay trai' ??
    int status_;

    // Check dung tren mat dat hay chua
    bool on_ground_;

    // Khi di chuyen, mep cua ban do o vi tri nao?
    int map_x_;
    int map_y_;

public:
    Character();
    ~Character();

    virtual bool LoadImg(string path, SDL_Renderer *screen);
    void Show(SDL_Renderer *des);
    void HandleInputAction(SDL_Event events, SDL_Renderer *screen);
    void setClips();
    void doPlayer(Map &map_data);
    void checkToMap(Map &map_data);
    void setMapXY(const int map_x, const int map_y)
    {
        map_x_ = map_x;
        map_y_ = map_y;
    };
    void centerEntityOnMap(Map &map_data);
};

#endif