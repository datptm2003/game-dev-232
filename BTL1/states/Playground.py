import pygame, os
from states.State import State
# from states.PauseMenu import PauseMenu

class Playground(State):
    def __init__(self, game):
        State.__init__(self,game)
        self.img_background = pygame.image.load(os.path.join(self.game.background_dir, "backgroundStartGame.png"))
        self.mapHoles = MapHoles(self.game)

    def update(self,delta_time, actions, mouse_pos):
        # Check if the game was paused 
        # if actions["pause"]:
        #     new_state = PauseMenu(self.game)
        #     new_state.enter_state()
        self.mapHoles.update(delta_time, actions, mouse_pos)
    
    def render(self, display):
        display.blit(self.img_background, (0,0))
        self.mapHoles.render(display)

class MapHoles:
    def __init__(self, game):
        self.game = game
        self.holePosition = [
            (78, 360),
            (300, 360),
            (522, 360),
            (744, 360),
            (966, 360),
            (165, 515),
            (387, 515),
            (609, 515),
            (831, 515),
            (1053, 515),
        ]

    def render(self, display):
        #--------- TODO ---------#
        # Control the frames of zombies in the holes
        pass
        
    def update(self, delta_time, actions, mouse_pos):
        #--------- TODO ---------#
        # Catch the actions of hit and miss
        pass

    def animate(self, delta_time, hit_pos):
        #--------- TODO ---------#
        # Control the animation when hit or miss
        pass
        
    
class Zombie:
    def __init__(self, game, zombie_type):
        self.game = game
        self.load_sprites()

        self.zombie_type = zombie_type
        if zombie_type == "normal":
            self.num_hit = 1
            self.win_point = 100
            self.lose_point = 10
            self.explosion = False
        elif zombie_type == "explosive":
            self.num_hit = 2
            self.win_point = 250
            self.lose_point = 20
            self.explosion = True
        elif zombie_type == "tough":
            self.num_hit = 3
            self.win_point = 350
            self.lose_point = 70
            self.explosion = False
        elif zombie_type == "human":
            self.num_hit = 1
            self.win_point = -120
            self.lose_point = 0
            self.explosion = False
        else:
            self.num_hit = 0
            self.win_point = 0
            self.lose_point = 0
            self.explosion = False

    def load_sprites(self):
        #--------- TODO ---------#
        pass
    
    def render(self, display):
        #--------- TODO ---------#
        # Control the frames of a single zombie
        pass
