import pygame, os
from states.State import State
import json

class Shop(State):
    def __init__(self, game):
        State.__init__(self, game)
        self.SCREEN_WIDTH = 1280
        self.SCREEN_HEIGHT = 720
        self.COLORWHITE = (255, 255, 255)
        self.img_background = pygame.image.load(os.path.join(self.game.background_dir, "Shop1_2.png"))
        
        self.back_box = pygame.Rect(125, 20, 256, 100)
        self.wood_box = pygame.Rect(125, 150, 125, 150)
        self.steel_box = pygame.Rect(125, 327, 125, 150)
        self.thunder_box = pygame.Rect(125, 504, 125, 150)
        self.buy_box = pygame.Rect(900, 506, 200, 80)
        self.backConfirm_box = pygame.Rect(525, 375, 230, 60)
        
        self.weapons = self.loadScore("weapon.json")
        self.scores = self.loadScore("score.json")
        self.weapon = 1
        self.backConfirm_bg = False
        
    def saveScore(self, fileName, data):
        with open(os.path.join(self.game.assets_dir, fileName), 'w') as file:
            json.dump(data, file)

    def loadScore(self, fileName):
        try:
            with open(os.path.join(self.game.assets_dir, fileName), 'r') as file:
                score = json.load(file)
        except FileNotFoundError:
            return 0
        return score

    def update(self, actions, screen):
        if actions["start"] or actions["left"] and self.back_box.collidepoint(pygame.mouse.get_pos()):
            self.exit_state()
        elif actions["start"] or actions["left"] and self.wood_box.collidepoint(pygame.mouse.get_pos()):
            self.img_background = pygame.image.load(os.path.join(self.game.background_dir, "Shop1_2.png"))
            self.weapon = "wooden"
        elif actions["start"] or actions["left"] and self.steel_box.collidepoint(pygame.mouse.get_pos()):
            imageName = "Shop2_" + str(self.weapons["steel"]) + ".png"
            self.img_background = pygame.image.load(os.path.join(self.game.background_dir, imageName))
            self.weapon = "steel"
        elif actions["start"] or actions["left"] and self.thunder_box.collidepoint(pygame.mouse.get_pos()):
            imageName = "Shop3_" + str(self.weapons["thunder"]) + ".png"
            self.img_background = pygame.image.load(os.path.join(self.game.background_dir, imageName))
            self.weapon = "thunder"
        elif actions["start"] or actions["left"] and self.buy_box.collidepoint(pygame.mouse.get_pos()):
            if self.weapon == "steel":
                if self.scores["totalScore"] < 30000:
                    self.backConfirm_bg = True
                else:
                    self.scores["totalScore"] -= 30000
                    self.saveScore("score.json", self.scores)
                    self.weapons["steel"] = 2
                    imageName = "Shop2_" + str(self.weapons["steel"]) + ".png"
                    self.img_background = pygame.image.load(os.path.join(self.game.background_dir, imageName))
                    self.saveScore("weapon.json", self.weapons)
            elif self.weapon == "thunder":
                if self.scores["totalScore"] < 100000:
                    self.backConfirm_bg = True
                else:
                    self.scores["totalScore"] -= 100000
                    self.weapons["thunder"] = 2
                    imageName = "Shop3_" + str(self.weapons["thunder"]) + ".png"
                    self.img_background = pygame.image.load(os.path.join(self.game.background_dir, imageName))
                    print(self.weapons["thunder"])
                    self.saveScore("score.json", self.scores)
                    self.saveScore("weapon.json", self.weapons)
        elif actions["start"] or actions["left"] and self.backConfirm_box.collidepoint(pygame.mouse.get_pos()):
            self.backConfirm_bg = False

        self.game.reset_keys()

    def render(self, display):
        display.blit(self.img_background, (0,0))

        # self.score = self.loadScore("score.json")["totalScore"]
        text = self.game.medium_rare_font.render(str(self.scores["totalScore"]), True, (255,255,255))
        display.blit(text, (985, 76))

        if self.backConfirm_bg:
            backConfirm_background = pygame.image.load(os.path.join(self.game.background_dir, "NotEnoughScore.png"))
            display.blit(backConfirm_background, (440, 260))
