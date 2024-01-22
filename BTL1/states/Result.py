import pygame, os
from states.State import State
# from states.OptionMenu import OptionMenu
# from states.Title import Title
from RWFile import HandleFile
import json

class Result(State):
    def __init__(self, game, score, miss):
        State.__init__(self, game)
        self.SCREEN_WIDTH = 1280
        self.SCREEN_HEIGHT = 720
        self.COLORWHITE = (255, 255, 255)
        self.img_background = pygame.image.load(os.path.join(self.game.background_dir, "Result.png"))
        
        self.backToMenu_box = pygame.Rect(240, 400, 350, 100)
        self.backToHome_box = pygame.Rect(390, 550, 500, 100)
        self.tryAgain_box = pygame.Rect(690, 400, 350, 100)
        
        self.backToMenu_bg = pygame.image.load(os.path.join(self.game.background_dir, "backToMenu.png"))
        self.backToHome_bg = pygame.image.load(os.path.join(self.game.background_dir, "backToHome.png"))
        self.tryAgain_bg = pygame.image.load(os.path.join(self.game.background_dir, "playAgain.png"))
        
        self.backToMenu_hover = False
        self.backToHome_hover = False
        self.tryAgain_hover = False
        
        self.weapons = self.loadScore("weapon.json")
        self.weapon = 1
        self.backConfirm_bg = False
        
        self.score = score
        self.miss = miss
        
        newScore = int(HandleFile.loadScore(self.game.assets_dir, "score.json")) + score
        HandleFile.saveScore(self.game.assets_dir, "score.json", newScore)
        
        
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
        if self.backToMenu_box.collidepoint(pygame.mouse.get_pos()):
            self.backToMenu_hover = True
        else:
            self.backToMenu_hover = False
        
        if self.backToHome_box.collidepoint(pygame.mouse.get_pos()):
            self.backToHome_hover = True
        else:
            self.backToHome_hover = False
        
        if actions["start"] or actions["left"] and self.tryAgain_box.collidepoint(pygame.mouse.get_pos()):
            # print(self.game.countdownTime)
            # print(self.countdownTime)
            self.exit_state()
            self.game.state_stack[-1].countdownTime = 6
            self.game.state_stack[-1].remainingTime = 31
            self.game.state_stack[-1].score = 0
            self.game.state_stack[-1].startTime = pygame.time.get_ticks()
            self.game.state_stack[-1].startGame = False
            
        elif actions["start"] or actions["left"] and self.backToHome_box.collidepoint(pygame.mouse.get_pos()):
            # newState = Title(self.game)
            # newState.enter_state()
            self.exit_state()
            self.exit_state()
            self.exit_state()
            
        elif actions["start"] or actions["left"] and self.backToMenu_box.collidepoint(pygame.mouse.get_pos()):
            self.exit_state()
            self.game.state_stack[-1].countdownTime = 6
            self.game.state_stack[-1].remainingTime = 31
            self.game.state_stack[-1].score = 0
            self.game.state_stack[-1].startTime = pygame.time.get_ticks()
            self.game.state_stack[-1].startGame = False
            self.exit_state()
            
        if self.tryAgain_box.collidepoint(pygame.mouse.get_pos()):
            self.tryAgain_hover = True
        else:
            self.tryAgain_hover = False

        self.game.reset_keys()

    def render(self, display):
        display.blit(self.img_background, (0,0))
        if self.backToMenu_hover:
            display.blit(self.backToMenu_bg, (240, 400))
            
        if self.backToHome_hover:
            display.blit(self.backToHome_bg, (390, 550))
            
        if self.tryAgain_hover:
            display.blit(self.tryAgain_bg, (690, 400))
            
        scoreString = "SCORE: " + str(self.score)
        scoreText = self.game.medium_font.render(scoreString, True, (255,255,255))
        display.blit(scoreText, (450, 155))
        
        missString = "MISS: " + str(self.miss)
        missText = self.game.medium_font.render(missString, True, (255,255,255))
        display.blit(missText, (450, 225))
