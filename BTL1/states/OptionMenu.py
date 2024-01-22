import pygame, os
from states.State import State
from states.Playground import Playground
from RWFile import HandleFile

class OptionMenu(State):
    def __init__(self, game):
        State.__init__(self, game)
        # self.confirm_box = pygame.Rect(450, 305, 380, 110)
        self.img_background = None
        self.easy_cur_color = "Green"
        self.medium_cur_color = "White"
        self.hard_cur_color = "White"
        self.wood_cur_color = "Yellow"
        self.steel_cur_color = "White"
        self.thunder_cur_color = "White"
        
        self.level_easy_box = pygame.Rect(100, 150, 350, 100)
        self.level_medium_box = pygame.Rect(100, 250, 350, 100)
        self.level_hard_box = pygame.Rect(100, 350, 350, 100)
        # self.level
        
        self.weapon_wood_box = pygame.Rect(780, 150, 400, 100)
        self.weapon_steel_box = pygame.Rect(780, 250, 400, 100)
        self.weapon_thunder_box = pygame.Rect(780, 350, 400, 100)
        
        self.start_game_box = pygame.Rect(390, 550, 500, 100)
        
        self.back_box = pygame.Rect(125, 45, 200, 75)
        
        self.level_bg = 1
        self.level_bg_hover = None
        self.weapon_bg = 1
        self.weapon_bg_hover = None
        self.start_game_bg = False
        self.back_bg = 1
        
        self.weapons = HandleFile.loadScore(self.game.assets_dir, "weapon.json")

    def update(self, actions, screen):
        if actions["start"] or actions["left"] and self.level_easy_box.collidepoint(pygame.mouse.get_pos()):
            self.level_bg = 1
        elif actions["start"] or actions["left"] and self.level_medium_box.collidepoint(pygame.mouse.get_pos()):
            self.level_bg = 2
        elif actions["start"] or actions["left"] and self.level_hard_box.collidepoint(pygame.mouse.get_pos()):
            self.level_bg = 3
        elif self.level_easy_box.collidepoint(pygame.mouse.get_pos()):
            pygame.mouse.set_cursor(pygame.cursors.broken_x)
            self.level_bg_hover = 1
        elif self.level_medium_box.collidepoint(pygame.mouse.get_pos()):
            pygame.mouse.set_cursor(pygame.cursors.broken_x)
            self.level_bg_hover = 2
        elif self.level_hard_box.collidepoint(pygame.mouse.get_pos()):
            pygame.mouse.set_cursor(pygame.cursors.broken_x)
            self.level_bg_hover = 3
        else:
            self.level_bg_hover = None

        if actions["start"] or actions["left"] and self.weapon_wood_box.collidepoint(pygame.mouse.get_pos()) and self.weapons["wooden"] == 2:
            self.weapon_bg = 1
        elif actions["start"] or actions["left"] and self.weapon_steel_box.collidepoint(pygame.mouse.get_pos()) and self.weapons["steel"] == 2:
            self.weapon_bg = 2
        elif actions["start"] or actions["left"] and self.weapon_thunder_box.collidepoint(pygame.mouse.get_pos()) and self.weapons["thunder"] == 2:
            self.weapon_bg = 3
        elif self.weapon_wood_box.collidepoint(pygame.mouse.get_pos()) and self.weapons["wooden"] == 2:
            pygame.mouse.set_cursor(pygame.cursors.broken_x)
            self.weapon_bg_hover = 1
        elif self.weapon_steel_box.collidepoint(pygame.mouse.get_pos()) and self.weapons["steel"] == 2:
            pygame.mouse.set_cursor(pygame.cursors.broken_x)
            self.weapon_bg_hover = 2
        elif self.weapon_thunder_box.collidepoint(pygame.mouse.get_pos()) and self.weapons["thunder"] == 2:
            pygame.mouse.set_cursor(pygame.cursors.broken_x)
            self.weapon_bg_hover = 3
        else:
            self.weapon_bg_hover = None
        
        if actions["start"] or actions["left"] and self.start_game_box.collidepoint(pygame.mouse.get_pos()):
            newState = Playground(self.game, self.level_bg, self.weapon_bg)
            newState.enter_state()
        elif self.start_game_box.collidepoint(pygame.mouse.get_pos()):
            pygame.mouse.set_cursor(pygame.cursors.broken_x)
            self.start_game_bg = True
        else:
            self.start_game_bg = False
            
        if actions["start"] or actions["left"] and self.back_box.collidepoint(pygame.mouse.get_pos()):
            self.exit_state()
        elif self.back_box.collidepoint(pygame.mouse.get_pos()):
            pygame.mouse.set_cursor(pygame.cursors.broken_x)
            self.back_bg = 2
        else:
            self.back_bg = 1
        self.game.reset_keys()

    def render(self, display):
        if self.img_background is None:
            self.img_background = pygame.image.load(os.path.join(self.game.background_dir, "Menu.png"))

        display.blit(self.img_background, (0,0))
        if self.level_bg_hover is not None:
            imageName = "Level_" + str(self.level_bg_hover) + "_2.png"
            imagePos = (100, int(50 + self.level_bg_hover * 100))
            level_image = pygame.image.load(os.path.join(self.game.background_dir, imageName))
            display.blit(level_image, imagePos)
        
        if self.level_bg is not None:
            imageName = "Level_" + str(self.level_bg) + "_2.png"
            imagePos = (100, int(50 + self.level_bg * 100))
            level_image = pygame.image.load(os.path.join(self.game.background_dir, imageName))
            display.blit(level_image, imagePos)
            
        if self.weapon_bg_hover is not None:
            imageName = "Weapon_" + str(self.weapon_bg_hover) + "_2.png"
            imagePos = (780, int(50 + self.weapon_bg_hover * 100))
            level_image = pygame.image.load(os.path.join(self.game.background_dir, imageName))
            display.blit(level_image, imagePos)
        
        if self.weapon_bg is not None:
            imageName = "Weapon_" + str(self.weapon_bg) + "_2.png"
            imagePos = (780, int(50 + self.weapon_bg * 100))
            level_image = pygame.image.load(os.path.join(self.game.background_dir, imageName))
            display.blit(level_image, imagePos)
            
        if self.start_game_bg:
            display.blit(pygame.image.load(os.path.join(self.game.background_dir, "StartGame2.png")), (390, 550))
        
        imageName = "Back_" + str(self.back_bg) + ".png"
        display.blit(pygame.image.load(os.path.join(self.game.background_dir, imageName)), (125, 45))

class Button:
	def __init__(self, image, pos, text_input, font, base_color, hovering_color):
		self.image = image
		self.x_pos = pos[0]
		self.y_pos = pos[1]
		self.font = font
		self.base_color, self.hovering_color = base_color, hovering_color
		self.text_input = text_input
		self.text = self.font.render(self.text_input, True, self.base_color)
		if self.image is None:
			self.image = self.text
		self.rect = self.image.get_rect(center=(self.x_pos, self.y_pos))
		self.text_rect = self.text.get_rect(center=(self.x_pos, self.y_pos))

	def update(self, screen):
		if self.image is not None:
			screen.blit(self.image, self.rect)
		screen.blit(self.text, self.text_rect)

	def checkForInput(self, position):
		if position[0] in range(self.rect.left, self.rect.right) and position[1] in range(self.rect.top, self.rect.bottom):
			return True
		return False
	
	def changeColor(self, position):
		if position[0] in range(self.rect.left, self.rect.right) and position[1] in range(self.rect.top, self.rect.bottom):
			self.text = self.font.render(self.text_input, True, self.hovering_color)
		else:
			self.text = self.font.render(self.text_input, True, self.base_color)
			
	# def changeColorPermanent(self, type):
	# 	if type == "base":
	# 		self.text = self.font.render(self.text_input, True, self.base_color)
	# 	elif type == "hover":
	# 	    self.text = self.font.render(self.text_input, True, self.hovering_color)
    