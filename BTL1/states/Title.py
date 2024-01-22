import pygame, os
from states.State import State
from states.OptionMenu import OptionMenu
from states.Shop import Shop

class Title(State):
    def __init__(self, game):
        State.__init__(self, game)
        # self.play_box = pygame.Rect(450, 305, 380, 110)
        self.start_box = pygame.Rect(50, 400, 500, 100)
        self.shop_box = pygame.Rect(50, 500, 500, 100)
        self.img_background = None

    def update(self, actions, screen):
        if actions["start"] or actions["left"] and self.start_box.collidepoint(pygame.mouse.get_pos()):
            new_state = OptionMenu(self.game)
            new_state.enter_state()
        elif actions["start"] or actions["left"] and self.shop_box.collidepoint(pygame.mouse.get_pos()):
            new_state = Shop(self.game)
            new_state.enter_state()
        elif self.start_box.collidepoint(pygame.mouse.get_pos()):
            pygame.mouse.set_cursor(pygame.cursors.broken_x)
            self.img_background = pygame.image.load(os.path.join(self.game.background_dir, "BG2.png"))
        elif self.shop_box.collidepoint(pygame.mouse.get_pos()):
            pygame.mouse.set_cursor(pygame.cursors.broken_x)
            self.img_background = pygame.image.load(os.path.join(self.game.background_dir, "BG3.png"))
        elif not self.start_box.collidepoint(pygame.mouse.get_pos()) and not self.shop_box.collidepoint(pygame.mouse.get_pos()):
            pygame.mouse.set_cursor(pygame.cursors.broken_x)
            self.img_background = pygame.image.load(os.path.join(self.game.background_dir, "BG1.png"))
        # if actions["start"] or actions["left"] and self.play_box.collidepoint(mouse_pos):
        #     new_state = OptionMenu(self.game)
        #     new_state.enter_state()
        # elif self.play_box.collidepoint(pygame.mouse.get_pos()):
        #     pygame.mouse.set_cursor(pygame.cursors.broken_x)
        #     self.img_background = pygame.image.load(os.path.join(self.game.background_dir, "backgroundInit2.png"))
        # elif not self.play_box.collidepoint(pygame.mouse.get_pos()):
        #     pygame.mouse.set_cursor(pygame.cursors.arrow)
        #     self.img_background = pygame.image.load(os.path.join(self.game.background_dir, "backgroundInit1.png"))
        self.game.reset_keys()
        
    def render(self, display):
        if self.img_background is None:
            self.img_background = pygame.image.load(os.path.join(self.game.background_dir, "BG1.png"))
        display.blit(self.img_background, (0,0))
