import pygame, os
from states.State import State
from states.OptionMenu import OptionMenu

class Title(State):
    def __init__(self, game):
        State.__init__(self, game)
        self.play_box = pygame.Rect(450, 305, 380, 110)
        self.img_background = None

    def update(self, delta_time, actions, mouse_pos):
        if actions["start"] or actions["left"] and self.play_box.collidepoint(mouse_pos):
            new_state = OptionMenu(self.game)
            new_state.enter_state()
        elif self.play_box.collidepoint(pygame.mouse.get_pos()):
            pygame.mouse.set_cursor(pygame.cursors.broken_x)
            self.img_background = pygame.image.load(os.path.join(self.game.background_dir, "backgroundInit2.png"))
        elif not self.play_box.collidepoint(pygame.mouse.get_pos()):
            pygame.mouse.set_cursor(pygame.cursors.arrow)
            self.img_background = pygame.image.load(os.path.join(self.game.background_dir, "backgroundInit1.png"))
        self.game.reset_keys()
        
    def render(self, display):
        if self.img_background is None:
            self.img_background = pygame.image.load(os.path.join(self.game.background_dir, "backgroundInit1.png"))
        display.blit(self.img_background, (0,0))
