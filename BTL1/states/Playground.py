import pygame, os
from states.State import State
from states.Result import Result
from RWFile import HandleFile
import random
import json
# from states.PauseMenu import PauseMenu

class Playground(State):
    def __init__(self, game, level, weapon):
        State.__init__(self,game)
        self.SCREEN_WIDTH = 1280
        self.SCREEN_HEIGHT = 720
        self.COLORWHITE = (255, 255, 255)
        
        self.img_background = pygame.image.load(os.path.join(self.game.background_dir, "backgroundStartGame.png"))
        self.actions = []
        self.startGame = False
        self.startTime = pygame.time.get_ticks()
        self.countdown = 6
        self.countdownTime = self.countdown
        self.weapon = weapon
        self.level = level
        
                
    def saveScore(self):
        with open('score.json', 'w') as file:
            json.dump(self.score, file)

    def loadScore(self):
        try:
            with open('score.json', 'r') as file:
                score = json.load(file)
        except FileNotFoundError:
            return 0
        return score
    
    def update(self, actions, mouse_pos):
        # Check if the game was paused 
        # if actions["pause"]:
        #     new_state = PauseMenu(self.game)
        #     new_state.enter_state()
        
        if self.startGame:
            self.mapHoles.update(actions, mouse_pos)
            # pygame.mouse.set_visible(False)
            # self.animate()
            
        self.game.reset_keys()
        pass

    def displayLaze(self, display):
        for index in range(len(Playground.animationOfThunder)):
            currentTime = float((pygame.time.get_ticks() - Playground.animationOfThunder[index][2]) / 1000)
            if (currentTime > 0 and currentTime < 0.5):
                display.blit(self.lazeImage, (Playground.animationOfThunder[index][0], Playground.animationOfThunder[index][1]))
    
    def animate(self):
        currentTime = float((pygame.time.get_ticks() - self.startTimeToClick) / 1000)
        if currentTime > 0 and currentTime < 0.3:
            weaponImageName = "weapon" + str(self.weapon) + "_2.png"
            self.weaponImage = pygame.image.load(os.path.join(self.game.sprite_dir, weaponImageName))
            self.weaponImage_rect = self.weaponImage.get_rect()
            
        else:
            weaponImageName = "weapon" + str(self.weapon) + "_1.png"
            self.weaponImage = pygame.image.load(os.path.join(self.game.sprite_dir, weaponImageName))
            self.weaponImage_rect = self.weaponImage.get_rect()
    
    def render(self, display):
        display.blit(self.img_background, (0,0))
        if self.startGame:
            self.mapHoles.render(display)
        else: 
            self.huge_font = pygame.font.SysFont('comicsansms', 72) 
            self.countdownTime = int(self.countdown - (pygame.time.get_ticks() - self.startTime) / 1000)
            if self.countdownTime > 2:
                countdownText = self.huge_font.render(str(self.countdownTime - 2), True, self.COLORWHITE)
                countdownPosition = countdownText.get_rect()
                countdownPosition.center = (self.SCREEN_WIDTH / 2, self.SCREEN_HEIGHT / 2)
                display.blit(countdownText, countdownPosition)
            elif self.countdownTime == 2:
                countdownText = self.huge_font.render("Start", True, self.COLORWHITE)
                countdownPosition = countdownText.get_rect()
                countdownPosition.center = (self.SCREEN_WIDTH / 2, self.SCREEN_HEIGHT / 2)
                display.blit(countdownText, countdownPosition)
            else:
                self.mapHoles = MapHoles(self.game, self.level, self.weapon)
                self.startGame = True
        # Handle to display weapon
        

class MapHoles:
    def __init__(self, game, level, weapon):
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
        
        self.SCREEN_WIDTH = 1280
        self.SCREEN_HEIGHT = 720
        self.FONT_TOP_MARGIN = 50
        self.CHARACTER_WIDTH = 150
        self.CHARACTER_HEIGHT = 150
        self.level = level
        self.weapon = weapon
        
        weaponImageName = "weapon" + str(weapon) + "_1.png"
        self.weaponImage = pygame.image.load(os.path.join(self.game.sprite_dir, weaponImageName))
        self.weaponImage_rect = self.weaponImage.get_rect()
        
        self.lazeImage = pygame.image.load(os.path.join(self.game.sprite_dir, "laze1.png"))
        self.lazeImage_rect = self.lazeImage.get_rect()
        
        self.scores = HandleFile.loadScore(self.game.assets_dir, "score.json")
        self.maxScore = 0
        
        if level == 1:
            self.aliveTime = 3
            self.maxScore = self.scores["easyScore"]
        elif level == 2:
            self.aliveTime = 2
            self.maxScore = self.scores["mediumScore"]
        else:
            self.aliveTime = 1
            self.maxScore = self.scores["hardScore"]
        
        self.score = 0
        self.miss = 0
        self.time = 31
        self.remainingTime = self.time
        self.delayStartTime = pygame.time.get_ticks()
        self.startTimeToClick = 0
        if self.weapon == 2:
            self.delayTime = 1
        elif self.weapon == 3:
            self.delayTime = 0.5
        else:
            self.delayTime = 2
        self.needDelay = False

        self.startTime = pygame.time.get_ticks()
        self.startTimeToAddNewZombie = pygame.time.get_ticks()
        
        self.holes_background = pygame.image.load(os.path.join(self.game.background_dir, "holes.png"))

        # -1: hide, 0-1: time to display, 1-3: time alive, 4: become -1
        self.zombies = [[-1, None, False], [-1, None, False], [-1, None, False], [-1, None, False], [-1, None, False], [-1, None, False], [-1, None, False], [-1, None, False], [-1, None, False], [-1, None, False]]
        
        self.animationOfThunder = []
        self.animationOfSteel = []

    def renderZombie(self, display):
        discTime = 0.3
        if self.level == 2:
            discTime = 0.2
        elif self.level == 3:
            discTime = 0.1
        for index in range(len(self.zombies)):
            additionalHeight = 50
            if (self.zombies[index][0] == -1): 
                continue
            currentTime = float((pygame.time.get_ticks() - self.zombies[index][0]) / 1000)
            if (self.zombies[index][2] == False and self.zombies[index][1] != None):
                # if self.aliveTime == 2:
                #     pass
                # else:
                if self.zombies[index][1].zombie_type == "tough" or self.zombies[index][1].zombie_type == "human":
                    additionalHeight = 35
                
                if currentTime > 0 and currentTime < discTime:
                    display.blit(self.zombies[index][1].images[0], 
                                    (self.holePosition[index][0] + 35, self.holePosition[index][1] + additionalHeight))

                elif currentTime >= discTime and currentTime < (discTime * 2):
                    display.blit(self.zombies[index][1].images[1], 
                                    (self.holePosition[index][0] + 35, self.holePosition[index][1] + additionalHeight))
                
                elif currentTime >= (discTime * 2) and currentTime < float(self.aliveTime + discTime):
                    display.blit(self.zombies[index][1].images[2], 
                                    (self.holePosition[index][0] + 35, self.holePosition[index][1] + additionalHeight))

                elif currentTime >= float(self.aliveTime + discTime) and currentTime < float(self.aliveTime + (discTime * 2)):
                    display.blit(self.zombies[index][1].images[1], 
                                    (self.holePosition[index][0] + 35, self.holePosition[index][1] + additionalHeight))

                elif currentTime >= float(self.aliveTime + (discTime * 2)):
                    self.zombies[index][0] = -1
                    if self.zombies[index][1].zombie_type == "normal":
                        self.score -= self.zombies[index][1].lose_point
                    elif self.zombies[index][1].zombie_type == "explosive":
                        self.score -= self.zombies[index][1].lose_point
                    elif self.zombies[index][1].zombie_type == "tough":
                        self.score -= self.zombies[index][1].lose_point
                        
                    self.miss += 1
                    self.zombies[index][1] = None
                    self.zombies[index][2] = False
            else:
                if self.zombies[index][1].zombie_type == "tough" or self.zombies[index][1].zombie_type == "human":
                    additionalHeight = 35
                if currentTime > 0 and currentTime < discTime:
                    display.blit(self.zombies[index][1].images[3], 
                                    (self.holePosition[index][0] + 35, self.holePosition[index][1] + additionalHeight))
                elif currentTime >= discTime and currentTime < (discTime * 2):
                    display.blit(self.zombies[index][1].images[4], 
                                    (self.holePosition[index][0] + 35, self.holePosition[index][1] + additionalHeight))
                elif currentTime >= (discTime * 2) and currentTime < (discTime * 3):
                    display.blit(self.zombies[index][1].images[5], 
                                    (self.holePosition[index][0] + 35, self.holePosition[index][1] + additionalHeight))
                elif currentTime >= (discTime * 3):
                    self.zombies[index][0] = -1
                    self.zombies[index][1] = None
                    self.zombies[index][2] = False

    def renderTimer(self, display):
        display.blit(pygame.image.load(os.path.join(self.game.background_dir, "timeContainer.png")), (1080, 0))
        # Update time
        self.remainingTime = int(self.time - (pygame.time.get_ticks() - self.startTime) / 1000)
        if (self.remainingTime >= 0):
            currentTimeString = "TIME: " + str(self.remainingTime)
            timeText = self.game.small_font.render(currentTimeString, True, (255,255,255))
            timeTextPosition = timeText.get_rect()
            timeTextPosition = (self.SCREEN_WIDTH - 250, self.FONT_TOP_MARGIN)
            display.blit(timeText, timeTextPosition)
        else:
            currentTimeString = "TIME: 0" 
            timeText = self.game.small_font.render(currentTimeString, True, (255,255,255))
            timeTextPosition = timeText.get_rect()
            timeTextPosition = (self.SCREEN_WIDTH - 250, self.FONT_TOP_MARGIN)
            display.blit(timeText, timeTextPosition)
            pygame.mouse.set_visible(True)
            newState = Result(self.game, self.score, self.miss, self.level)
            newState.enter_state()
            

        # Update the player's score
        currentScoreString = "SCORE: " + str(self.score)
        scoreText = self.game.small_font.render(currentScoreString, True, (255,255,255))
        scoreTextPosition = scoreText.get_rect()
        scoreTextPosition = (
            self.SCREEN_WIDTH - 250, self.FONT_TOP_MARGIN * 1.75)
        display.blit(scoreText, scoreTextPosition)
        
        # Update the player's miss
        currentMissString = "MISS: " + str(self.miss)
        missText = self.game.small_font.render(currentMissString, True, (255,255,255))
        missTextPosition = missText.get_rect()
        missTextPosition = (
            self.SCREEN_WIDTH - 250, self.FONT_TOP_MARGIN * 2.5)
        display.blit(missText, missTextPosition)
        
        # Update the player's miss
        currentMaxScoreString = "MAX-SCORE: " + str(self.maxScore)
        maxScoreText = self.game.small_font.render(currentMaxScoreString, True, (255,255,255))
        maxScoreTextPosition = maxScoreText.get_rect()
        maxScoreTextPosition = (
            self.SCREEN_WIDTH - 250, self.FONT_TOP_MARGIN * 3.25)
        display.blit(maxScoreText, maxScoreTextPosition)

    def displayLaze(self, display):
        for index in range(len(self.animationOfThunder)):
            currentTime = float((pygame.time.get_ticks() - self.animationOfThunder[index][2]) / 1000)
            if (currentTime > 0 and currentTime <= 0.1):
                lazeImage = pygame.image.load(os.path.join(self.game.sprite_dir, "efWeapon3_1.png"))
                display.blit(lazeImage, (self.animationOfThunder[index][0], self.animationOfThunder[index][1]))
            elif (currentTime > 0.1 and currentTime <= 0.2):
                lazeImage = pygame.image.load(os.path.join(self.game.sprite_dir, "efWeapon3_2.png"))
                display.blit(lazeImage, (self.animationOfThunder[index][0], self.animationOfThunder[index][1]))
            elif (currentTime > 0.2 and currentTime <= 0.4):
                lazeImage = pygame.image.load(os.path.join(self.game.sprite_dir, "efWeapon3_3.png"))
                display.blit(lazeImage, (self.animationOfThunder[index][0], self.animationOfThunder[index][1]))
                
    def displaySteelEffect(self, display):
        for index in range(len(self.animationOfSteel)):
            currentTime = float((pygame.time.get_ticks() - self.animationOfSteel[index][2]) / 1000)
            if (currentTime > 0 and currentTime <= 0.1):
                lazeImage = pygame.image.load(os.path.join(self.game.sprite_dir, "efWeapon2_1.png"))
                display.blit(lazeImage, (self.animationOfSteel[index][0], self.animationOfSteel[index][1]))
            elif (currentTime > 0.1 and currentTime <= 0.2):
                lazeImage = pygame.image.load(os.path.join(self.game.sprite_dir, "efWeapon2_2.png"))
                display.blit(lazeImage, (self.animationOfSteel[index][0], self.animationOfSteel[index][1]))
            elif (currentTime > 0.2 and currentTime <= 0.4):
                lazeImage = pygame.image.load(os.path.join(self.game.sprite_dir, "efWeapon2_3.png"))
                display.blit(lazeImage, (self.animationOfSteel[index][0], self.animationOfSteel[index][1]))

    def render(self, display):
        #--------- TODO ---------#
        # Control the frames of zombies in the holes
        display.blit(self.holes_background, (0, 300))
        
        self.renderZombie(display)
        self.renderTimer(display)
        self.displayLaze(display)
        self.displaySteelEffect(display)
        
        mouse_x, mouse_y = pygame.mouse.get_pos()
        self.weaponImage_rect.x = mouse_x - self.weaponImage_rect.width / 2.3
        self.weaponImage_rect.y = mouse_y - self.weaponImage_rect.height / 2
        display.blit(self.weaponImage, self.weaponImage_rect)
        
        
        pass
    
    def randomProb(self,prob):
        r = random.random()
        if r <= prob:
            return True
        return False

    def createNewZombie(self):
        distTime = 1
        if self.level == 2:
            distTime = 0.75
        elif self.level == 3:
            distTime = 0.5
        if float((pygame.time.get_ticks() - self.startTimeToAddNewZombie) / 1000) >= distTime and self.remainingTime > 0:
            newIndex = random.randint(0, 9)
            if self.zombies[newIndex][0] == -1:
                self.startTimeToAddNewZombie = pygame.time.get_ticks()
                self.zombies[newIndex][0] = pygame.time.get_ticks()
                if self.level == 1:
                    zombieType = random.randint(0, 9)
                    if zombieType >= 0 and zombieType <= 4:
                        self.zombies[newIndex][1] = Zombie(self.game, "normal", self.weapon)
                    else:
                        self.zombies[newIndex][1] = Zombie(self.game, "explosive", self.weapon)
                elif self.level == 2:
                    zombieType = random.randint(0, 9)
                    if zombieType >= 0 and zombieType <= 3:
                        self.zombies[newIndex][1] = Zombie(self.game, "normal", self.weapon)
                    elif zombieType >= 4 and zombieType <= 6:
                        self.zombies[newIndex][1] = Zombie(self.game, "explosive", self.weapon)
                    else:
                        self.zombies[newIndex][1] = Zombie(self.game, "tough", self.weapon)
                else:
                    zombieType = random.randint(0, 9)
                    if zombieType >= 0 and zombieType <= 2:
                        self.zombies[newIndex][1] = Zombie(self.game, "normal", self.weapon)
                    elif zombieType >= 3 and zombieType <= 4:
                        self.zombies[newIndex][1] = Zombie(self.game, "explosive", self.weapon)
                    elif zombieType >= 5 and zombieType <= 6:
                        self.zombies[newIndex][1] = Zombie(self.game, "tough", self.weapon)
                    else:
                        self.zombies[newIndex][1] = Zombie(self.game, "human", self.weapon)
    
    def applyEffect(self, index):        
        criticalStrike = False
        if self.weapon == 3:
            criticalStrike = self.randomProb(0.65)
        elif self.weapon == 2:
            criticalStrike = self.randomProb(0.4)
        if criticalStrike:
            curX, curY = pygame.mouse.get_pos()
            lazeRectX = curX - 145 / 1.65
            lazeRectY = curY - 166 
            if (curY > 360) and (curY < 510) and \
                (curX - 222 >= 78) or (curY > 515) and (curY < 665) and curX - 222 >= 165:
                if self.zombies[index-1][1] is not None and self.zombies[index-1][1].zombie_type == "human":
                    self.isHit((curX - 222, curY),False)
                else:
                    self.isHit((curX - 222, curY),True)

                if self.weapon == 3:
                    self.animationOfThunder.append((lazeRectX - 222, lazeRectY, pygame.time.get_ticks()))
                elif self.weapon == 2:
                    self.animationOfSteel.append((lazeRectX - 222, lazeRectY, pygame.time.get_ticks()))
                    
            if ((curY > 360) and (curY < 510) and \
                (curX + 222 <= 966 + self.CHARACTER_WIDTH)) or \
                ((curY > 515) and (curY < 665) and \
                (curX + 222 <= 1053 + self.CHARACTER_WIDTH)):
                if self.zombies[index+1][1] is not None and self.zombies[index+1][1].zombie_type == "human":
                    self.isHit((curX + 222, curY),False)
                else:
                    self.isHit((curX + 222, curY),True)
                
                if self.weapon == 3:
                    self.animationOfThunder.append((lazeRectX + 222, lazeRectY, pygame.time.get_ticks()))
                elif self.weapon == 2:
                    self.animationOfSteel.append((lazeRectX + 222, lazeRectY, pygame.time.get_ticks()))

    def isHit(self, mousePosition, kill):
        if not kill:
            return
        mouseX, mouseY = mousePosition
        currentTime = pygame.time.get_ticks()
        self.game.play_pickup_sound = False
        if self.needDelay and currentTime - self.delayStartTime < self.delayTime * 1000:
            return
        else:
            self.needDelay = False
        for i in range(len(self.zombies)):
            if self.zombies[i][0] != -1 and self.zombies[i][2] == False:
                if mouseY > 360 and mouseY < 510:
                    for j in range(5):
                        if  mouseX > self.holePosition[j][0] and \
                            mouseX < (self.holePosition[j][0] + self.CHARACTER_WIDTH) and i == j:
                            if self.zombies[i][1].num_hit > 1:
                                self.zombies[i][1].num_hit -= 1
                            else:
                                self.score += self.zombies[i][1].win_point
                                self.zombies[i][2] = True
                                self.zombies[i][0] = pygame.time.get_ticks()
                                self.applyEffect(i)
                                self.game.punch_sound.play()
                                if self.zombies[i][1].explosion:
                                    self.game.explosion_sound.play()
                                    self.delayStartTime = pygame.time.get_ticks()
                                    self.needDelay = True
                                elif self.zombies[i][1].human:
                                    self.game.wrong_sound.play()

                elif mouseY > 515 and mouseY < 665:
                    for j in range(5, 10):
                        if  mouseX > self.holePosition[j][0] and \
                            mouseX < (self.holePosition[j][0] + self.CHARACTER_WIDTH) and i == j:
                            if self.zombies[i][1].num_hit > 1:
                                self.zombies[i][1].num_hit -= 1
                            else:
                                self.score += self.zombies[i][1].win_point
                                self.zombies[i][2] = True
                                self.zombies[i][0] = pygame.time.get_ticks()
                                self.applyEffect(i)
                                self.game.punch_sound.play()
                                if self.zombies[i][1].explosion:
                                    self.game.explosion_sound.play()
                                    self.delayStartTime = pygame.time.get_ticks()
                                    self.needDelay = True
                                elif self.zombies[i][1].human:
                                    self.game.wrong_sound.play()


    def animate(self):
        currentTime = float((pygame.time.get_ticks() - self.startTimeToClick) / 1000)
        if currentTime > 0 and currentTime < 0.3 and self.needDelay == False:
            weaponImageName = "weapon" + str(self.weapon) + "_2.png"
            self.weaponImage = pygame.image.load(os.path.join(self.game.sprite_dir, weaponImageName))
            self.weaponImage_rect = self.weaponImage.get_rect()
            
        else:
            weaponImageName = "weapon" + str(self.weapon) + "_1.png"
            self.weaponImage = pygame.image.load(os.path.join(self.game.sprite_dir, weaponImageName))
            self.weaponImage_rect = self.weaponImage.get_rect()

    def update(self, actions, mouse_pos):
        #--------- TODO ---------#
        # handle add zombie
        self.createNewZombie()

        # Catch the actions of hit and miss
        if actions["start"] or actions["left"]:
            self.startTimeToClick = pygame.time.get_ticks()
            
            
            currentTime = pygame.time.get_ticks()
            if self.needDelay and float(currentTime - self.delayStartTime) < float(self.delayTime * 1000):
                return
            
            if self.weapon == 3:
                # Lấy vị trí chuột
                mouse_x, mouse_y = pygame.mouse.get_pos()
                
                # Tính toán vị trí mới cho hình ảnh sao cho nó nằm chính giữa con chuột
                self.lazeImage_rect.x = mouse_x - self.lazeImage_rect.width / 1.65
                self.lazeImage_rect.y = mouse_y - self.lazeImage_rect.height 
                # print(self.lazeImage_rect.width,', ',self.lazeImage_rect.height)
                self.animationOfThunder.append((self.lazeImage_rect.x, self.lazeImage_rect.y, pygame.time.get_ticks()))
            
            if self.weapon == 2:
                # Lấy vị trí chuột
                mouse_x, mouse_y = pygame.mouse.get_pos()
                
                # Tính toán vị trí mới cho hình ảnh sao cho nó nằm chính giữa con chuột
                self.lazeImage_rect.x = mouse_x - self.lazeImage_rect.width / 1.65
                self.lazeImage_rect.y = mouse_y - self.lazeImage_rect.height / 1.35
                # print(self.lazeImage_rect.width,', ',self.lazeImage_rect.height)
                self.animationOfSteel.append((self.lazeImage_rect.x, self.lazeImage_rect.y, pygame.time.get_ticks()))
            
            self.isHit(pygame.mouse.get_pos(), True)

        pygame.mouse.set_visible(False)
        self.animate()
        
        self.game.reset_keys()
        
    
class Zombie:
    def __init__(self, game, zombie_type, weapon):
        self.game = game
        self.load_sprites()

        self.zombie_type = zombie_type
        if zombie_type == "normal":
            self.num_hit = 1
            self.win_point = 100
            self.lose_point = 10
            self.explosion = False
            self.human = False
            self.images = [
                pygame.image.load(os.path.join(self.game.sprite_dir, "normzum_11.png")),
                pygame.image.load(os.path.join(self.game.sprite_dir, "normzum_12.png")),
                pygame.image.load(os.path.join(self.game.sprite_dir, "normzum_1.png")),
                pygame.image.load(os.path.join(self.game.sprite_dir, "normzum_2.png")),
                pygame.image.load(os.path.join(self.game.sprite_dir, "normzum_21.png")),
                pygame.image.load(os.path.join(self.game.sprite_dir, "normzum_22.png")),
            ]
        elif zombie_type == "explosive":
            self.num_hit = 2
            if weapon == 3:
                self.num_hit = 1
            self.win_point = 250
            self.lose_point = 20
            self.explosion = True
            self.human = False
            self.images = [
                pygame.image.load(os.path.join(self.game.sprite_dir, "exzom_1.png")),
                pygame.image.load(os.path.join(self.game.sprite_dir, "exzom_2.png")),
                pygame.image.load(os.path.join(self.game.sprite_dir, "exzom_3.png")),
                pygame.image.load(os.path.join(self.game.sprite_dir, "exzom_4.png")),
                pygame.image.load(os.path.join(self.game.sprite_dir, "exzom_5.png")),
                pygame.image.load(os.path.join(self.game.sprite_dir, "exzom_6.png")),
            ]
        elif zombie_type == "tough":
            self.num_hit = 3
            if weapon == 3:
                self.num_hit = 2
            self.win_point = 350
            self.lose_point = 70
            self.explosion = False
            self.human = False
            self.images = [
                pygame.image.load(os.path.join(self.game.sprite_dir, "masterzum_11.png")),
                pygame.image.load(os.path.join(self.game.sprite_dir, "masterzum_12.png")),
                pygame.image.load(os.path.join(self.game.sprite_dir, "masterzum_1.png")),
                pygame.image.load(os.path.join(self.game.sprite_dir, "masterzum_2.png")),
                pygame.image.load(os.path.join(self.game.sprite_dir, "masterzum_21.png")),
                pygame.image.load(os.path.join(self.game.sprite_dir, "masterzum_22.png")),
            ]
        elif zombie_type == "human":
            self.num_hit = 1
            self.win_point = -120
            self.lose_point = 0
            self.explosion = False
            self.human = True
            self.images = [
                pygame.image.load(os.path.join(self.game.sprite_dir, "human_11.png")),
                pygame.image.load(os.path.join(self.game.sprite_dir, "human_12.png")),
                pygame.image.load(os.path.join(self.game.sprite_dir, "human_1.png")),
                pygame.image.load(os.path.join(self.game.sprite_dir, "human_2.png")),
                pygame.image.load(os.path.join(self.game.sprite_dir, "human_22.png")),
                pygame.image.load(os.path.join(self.game.sprite_dir, "human_21.png")),
            ]
        else:
            self.num_hit = 0
            self.win_point = 0
            self.lose_point = 0
            self.explosion = False
            self.human = False
    
    def getImages(self):
        return self.images

    def load_sprites(self):
        #--------- TODO ---------#
        pass
    
    def render(self, display):
        #--------- TODO ---------#
        # Control the frames of a single zombie
        pass
