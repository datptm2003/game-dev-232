cmake_minimum_required(VERSION 3.0.0)
project(BTL2 VERSION 0.1.0 LANGUAGES C CXX)
add_executable(BTL2 src/main.cpp src/game.cpp src/BaseObject.cpp src/GameMap.cpp src/Character.cpp)
target_link_libraries(BTL2 SDL2main SDL2 GL SDL2_image SDL2_mixer SDL2_ttf)