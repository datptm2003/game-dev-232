
#ifndef BASE_OBJECT_H_
#define BASE_OBJECT_H_

#include "CommonFunc.h"

class BaseObject
{
public:
    BaseObject();
    ~BaseObject();
    void setRect(const int &x, const int &y) { rect_.x = x, rect_.y = y; }

    SDL_Rect getRect() const { return this->rect_; };
    SDL_Texture *getObject() const { return this->p_object_; }

    bool LoadImg(string path, SDL_Renderer *screen);
    void Render(SDL_Renderer *des, const SDL_Rect *clip = NULL);
    void Free();

protected:
    SDL_Texture *p_object_;
    SDL_Rect rect_;
};

#endif