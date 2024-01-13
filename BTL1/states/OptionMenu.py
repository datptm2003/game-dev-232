import pygame, os
from states.State import State
from states.Playground import Playground

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
        
        self.level = 1
        self.hammer = 1

    def update(self, delta_time, actions, mouse_pos):
        if actions["left"] and self.confirm_button.checkForInput(mouse_pos):
            new_state = Playground(self.game)
            new_state.enter_state()
        elif actions["left"] and self.easy_level_button.checkForInput(mouse_pos):
            self.easy_cur_color = self.easy_level_button.hovering_color
            self.medium_cur_color = "white"
            self.hard_cur_color = "White"
            self.level = 1
        elif actions["left"] and self.medium_level_button.checkForInput(mouse_pos):
            self.easy_cur_color = "White"
            self.medium_cur_color = self.medium_level_button.hovering_color
            self.hard_cur_color = "White"
            self.level = 2
        elif actions["left"] and self.hard_level_button.checkForInput(mouse_pos):
            self.easy_cur_color = "White"
            self.medium_cur_color = "White"
            self.hard_cur_color = self.hard_level_button.hovering_color
            self.level = 3

        if actions["left"] and self.wood_hammer_button.checkForInput(mouse_pos):
            self.wood_cur_color = self.wood_hammer_button.hovering_color
            self.steel_cur_color = "white"
            self.thunder_cur_color = "White"
            self.hammer = 1
        elif actions["left"] and self.steel_hammer_button.checkForInput(mouse_pos):
            self.wood_cur_color = "White"
            self.steel_cur_color = self.steel_hammer_button.hovering_color
            self.thunder_cur_color = "White"
            self.hammer = 2
        elif actions["left"] and self.thunder_hammer_button.checkForInput(mouse_pos):
            self.wood_cur_color = "White"
            self.steel_cur_color = "white"
            self.thunder_cur_color = self.thunder_hammer_button.hovering_color
            self.hammer = 3
        self.game.reset_keys()
        
    def render(self, display):
        if self.img_background is None:
            self.img_background = pygame.image.load(os.path.join(self.game.background_dir, "backgroundStartGame.png"))
        display.blit(self.img_background, (0,0))
		
        self.confirm_button = Button(image=None, pos=(650, 550), text_input="Confirm", font=self.game.medium_font, base_color="White", hovering_color="Orange")
        self.confirm_button.changeColor(pygame.mouse.get_pos())
        self.confirm_button.update(display)
        
        self.easy_level_button = Button(image=None, pos=(350, 270), text_input="Easy", font=self.game.medium_font, base_color=self.easy_cur_color, hovering_color="Green")
        self.easy_level_button.changeColor(pygame.mouse.get_pos())
        self.easy_level_button.update(display)
		
        self.medium_level_button = Button(image=None, pos=(350, 320), text_input="Medium", font=self.game.medium_font, base_color=self.medium_cur_color, hovering_color="Green")
        self.medium_level_button.changeColor(pygame.mouse.get_pos())
        self.medium_level_button.update(display)
		
        self.hard_level_button = Button(image=None, pos=(350, 370), text_input="Hard", font=self.game.medium_font, base_color=self.hard_cur_color, hovering_color="Green")
        self.hard_level_button.changeColor(pygame.mouse.get_pos())
        self.hard_level_button.update(display)
		
        self.wood_hammer_button = Button(image=None, pos=(950, 270), text_input="Wood", font=self.game.medium_font, base_color=self.wood_cur_color, hovering_color="Yellow")
        self.wood_hammer_button.changeColor(pygame.mouse.get_pos())
        self.wood_hammer_button.update(display)
		
        self.steel_hammer_button = Button(image=None, pos=(950, 320), text_input="Steel", font=self.game.medium_font, base_color=self.steel_cur_color, hovering_color="Blue")
        self.steel_hammer_button.changeColor(pygame.mouse.get_pos())
        self.steel_hammer_button.update(display)
		
        self.thunder_hammer_button = Button(image=None, pos=(950, 370), text_input="Thunder", font=self.game.medium_font, base_color=self.thunder_cur_color, hovering_color="Red")
        self.thunder_hammer_button.changeColor(pygame.mouse.get_pos())
        self.thunder_hammer_button.update(display)
    


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
    