# Example file showing a basic pygame "game loop"
import pygame
# import pygame_menu as pm
import random

class Background:
    def __init__(self):
        self.img_background = pygame.image.load('./BTL1/assets/backgroundInit1.png')

    def start(self):
        self.img_background = pygame.image.load('./BTL1/assets/backgroundStartGame.png')

class Character:
    def __init__(self):
        self.img_character_1 = pygame.image.load('./BTL1/assets/mole_11.png')
        self.img_character_2 = pygame.image.load('./BTL1/assets/mole_12.png')
        self.img_character_3 = pygame.image.load('./BTL1/assets/mole_1.png')
        self.img_character_4 = pygame.image.load('./BTL1/assets/mole_2.png')
        self.img_character_5 = pygame.image.load('./BTL1/assets/mole_21.png')
        self.img_character_6 = pygame.image.load('./BTL1/assets/mole_22.png')
        self.data = []
        self.data.append(self.img_character_1.subsurface(0, 0, 80, 90))
        self.data.append(self.img_character_2.subsurface(0, 0, 80, 90))
        self.data.append(self.img_character_3.subsurface(0, 0, 80, 90))
        self.data.append(self.img_character_4.subsurface(0, 0, 80, 90))
        self.data.append(self.img_character_5.subsurface(0, 0, 80, 90))
        self.data.append(self.img_character_6.subsurface(0, 0, 80, 90))

class GameContainer:
    def __init__(self):
        self.SCREEN_WIDTH = 1280
        self.SCREEN_HEIGHT = 720
        self.CHARACTER_WIDTH = 80
        self.CHARACTER_HEIGHT = 90
        self.GAME_TITLE = 'Zombie Game'
        self.FPS = 120
        self.CHARACTER_WIDTH = 150
        self.CHARACTER_HEIGHT = 150
        self.FONT_TOP_MARGIN = 30
        self.LEVEL_SCORE_GAP = 4
        
        # Color:
        self.COLORWHITE = (255, 255, 255)
        
        self.startGame = False
        self.inGame = False
        self.gameOver = False

        # Font object for displaying text
        self.font_obj = pygame.font.SysFont('comicsansms', 35)
        self.font_obj_finish = pygame.font.SysFont('comicsansms', 44)
        self.font_coor = pygame.font.SysFont('comicsansms', 14)
        
        # Change mouse
        self.mouseImage = pygame.image.load("./BTL1/assets/sprites/mouse1.png")
        self.mouseImage_rect = self.mouseImage.get_rect()
        
        # Add effectively when hitting
        self.lazeImage = pygame.image.load("./BTL1/assets/sprites/laze1.png")
        self.lazeImage_rect = self.lazeImage.get_rect()
        
        self.speed = 1
        
        # Variables of game
        self.countdown = 6
        self.countdownTime = self.countdown
        self.score = 0
        self.miss = 0
        self.timing = 31
        self.remainingTime = self.timing
        self.countupTime = 0
        self.startTime = 0
        self.startTimeToAddNewZombie = 0
        
        # -1: hide, 0-1: time to display, 1-3: time alive, 4: become -1
        self.zombieTimes = [[-1, False], [-1, False], [-1, False], [-1, False], [-1, False], [-1, False], [-1, False], [-1, False], [-1, False], [-1, False]]
        
        # Hole position:
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
        
        # Initialize screen
        self.screen = pygame.display.set_mode((self.SCREEN_WIDTH, self.SCREEN_HEIGHT))
        pygame.display.set_caption(self.GAME_TITLE)
        
        # Background
        self.bg = Background()
        self.screen.blit(self.bg.img_background, (0, 0))
        
        # Character
        self.character = Character()
        
    def isHit(self, mousePosition):
        mouseX, mouseY = mousePosition

        for i in range(len(self.zombieTimes)):
            if self.zombieTimes[i][0] != -1 and self.zombieTimes[i][1] == False:
                if mouseY > 360 and mouseY < 510:
                    for j in range(5):
                        if  mouseX > self.holePosition[j][0] and \
                            mouseX < (self.holePosition[j][0] + self.CHARACTER_WIDTH) and i == j:
                            self.zombieTimes[i][1] = True
                            self.zombieTimes[i][0] = pygame.time.get_ticks()
                            self.score += 1
                            self.update()
                elif mouseY > 515 and mouseY < 665:
                    for j in range(5, 10):
                        if  mouseX > self.holePosition[j][0] and \
                            mouseX < (self.holePosition[j][0] + self.CHARACTER_WIDTH) and i == j:
                            self.zombieTimes[i][1] = True
                            self.zombieTimes[i][0] = pygame.time.get_ticks()
                            self.score += 1
                            self.update()

    def addNewZombie(self, timeSleep):
        if int((pygame.time.get_ticks() - self.startTimeToAddNewZombie) / 1000) >= timeSleep and self.countdownTime < 2 and self.remainingTime > 0:
            self.startTimeToAddNewZombie = pygame.time.get_ticks()
            zombieRandom = random.randint(0, 9)
            if (self.zombieTimes[zombieRandom][0] == -1):
                self.zombieTimes[zombieRandom][0] = pygame.time.get_ticks()
                
    def updateZombie(self):
        self.screen.blit(pygame.image.load('./BTL1/assets/holes.png'), (0, 300))
        for index in range(len(self.zombieTimes)):
            if (self.zombieTimes[index][0] == -1): 
                continue
            currentTime = float((pygame.time.get_ticks() - self.zombieTimes[index][0]) / 1000)
            if (self.zombieTimes[index][1] == False):
                if currentTime >= 1 and currentTime < 3:
                    self.screen.blit(self.character.data[2], 
                                    (self.holePosition[index][0] + 35, self.holePosition[index][1] + 50))
                elif currentTime > 0 and currentTime < 0.35:
                    self.screen.blit(self.character.data[0], 
                                    (self.holePosition[index][0] + 35, self.holePosition[index][1] + 50))
                elif currentTime >= 0.35 and currentTime < 0.7:
                    self.screen.blit(self.character.data[1], 
                                    (self.holePosition[index][0] + 35, self.holePosition[index][1] + 50))
                elif currentTime > 0.7 and currentTime < 1:
                    self.screen.blit(self.character.data[2], 
                                    (self.holePosition[index][0] + 35, self.holePosition[index][1] + 50))
                elif currentTime > 3 and currentTime < 3.35:
                    self.screen.blit(self.character.data[2], 
                                    (self.holePosition[index][0] + 35, self.holePosition[index][1] + 50))
                elif currentTime >= 3.35 and currentTime < 3.7:
                    self.screen.blit(self.character.data[1], 
                                    (self.holePosition[index][0] + 35, self.holePosition[index][1] + 50))
                elif currentTime >= 3.7:
                    self.zombieTimes[index][0] = -1
                    self.zombieTimes[index][1] = False
                    self.miss += 1
            else:
                if currentTime > 0 and currentTime < 0.15:
                    self.screen.blit(self.character.data[3], 
                                    (self.holePosition[index][0] + 35, self.holePosition[index][1] + 50))
                elif currentTime >= 0.15 and currentTime < 0.4:
                    self.screen.blit(self.character.data[4], 
                                    (self.holePosition[index][0] + 35, self.holePosition[index][1] + 50))
                elif currentTime >= 0.4 and currentTime < 0.7:
                    self.screen.blit(self.character.data[5], 
                                    (self.holePosition[index][0] + 35, self.holePosition[index][1] + 50))
                elif currentTime >= 0.7:
                    self.zombieTimes[index][0] = -1
                    self.zombieTimes[index][1] = False
                
    def displayLaze(self, lazeIndexes):
        for index in range(len(lazeIndexes)):
            currentTime = float((pygame.time.get_ticks() - lazeIndexes[index][2]) / 1000)
            if (currentTime > 0 and currentTime < 1):
                self.screen.blit(self.lazeImage, (lazeIndexes[index][0], lazeIndexes[index][1]))

    def update(self):
        self.screen.blit(pygame.image.load('./BTL1/assets/timeContainer.png'), (1080, 0))
        # Update time
        self.remainingTime = int(self.timing - (pygame.time.get_ticks() - self.startTime) / 1000)
        if (self.remainingTime >= 0):
            currentTimeString = "TIME: " + str(self.remainingTime)
            timeText = self.font_obj.render(currentTimeString, True, self.COLORWHITE)
            timeTextPosition = timeText.get_rect()
            timeTextPosition.center = (self.SCREEN_WIDTH - 80, self.FONT_TOP_MARGIN)
            self.screen.blit(timeText, timeTextPosition)
        else:
            currentTimeString = "TIME: 0" 
            timeText = self.font_obj.render(currentTimeString, True, self.COLORWHITE)
            timeTextPosition = timeText.get_rect()
            timeTextPosition.center = (self.SCREEN_WIDTH - 80, self.FONT_TOP_MARGIN)
            self.screen.blit(timeText, timeTextPosition)

        # Update the player's score
        currentScoreString = "SCORE: " + str(self.score)
        scoreText = self.font_obj.render(currentScoreString, True, self.COLORWHITE)
        scoreTextPosition = scoreText.get_rect()
        scoreTextPosition.center = (
            self.SCREEN_WIDTH - 80, self.FONT_TOP_MARGIN * 3)
        self.screen.blit(scoreText, scoreTextPosition)
        
        # Update the player's miss
        currentMissString = "MISS: " + str(self.miss)
        missText = self.font_obj.render(currentMissString, True, self.COLORWHITE)
        missTextPosition = missText.get_rect()
        missTextPosition.center = (
            self.SCREEN_WIDTH - 80, self.FONT_TOP_MARGIN * 5)
        self.screen.blit(missText, missTextPosition)
    
    def start(self):
        clock = pygame.time.Clock()
        
        # To make the function called just once
        hover = 0
        finish = 0
        finish_hover = 0
        
        # Hole position display
        holeIndex = 0
        
        # remaining time of the zombie
        remainingTime = -1
        
        # laze: (x, y, t)
        lazeIndexes = []
        # thread for random zombie
        play_box = pygame.Rect(450, 305, 380, 110)

        running = True
        
        # randomZombieThread = threading.Thread(target=self.randomZombie, args=(1,))
        # randomZombieThread.start()

        while running:
            # poll for events
            # pygame.QUIT event means the user clicked X to close your window
            for event in pygame.event.get():
                if event.type == pygame.QUIT:
                    running = False
                    # randomZombieThread.join()
                if event.type == pygame.MOUSEBUTTONDOWN:
                    if self.startGame == False and event.button == 1:
                        if play_box.collidepoint(pygame.mouse.get_pos()):
                            self.startGame = True
                            
                            # Start time
                            self.startTime = pygame.time.get_ticks()
                    elif self.inGame == True and self.gameOver == False and event.button > 0:
                        self.isHit(pygame.mouse.get_pos())
                        
                        # Lấy vị trí chuột
                        mouse_x, mouse_y = pygame.mouse.get_pos()
                        
                        # Tính toán vị trí mới cho hình ảnh sao cho nó nằm chính giữa con chuột
                        self.lazeImage_rect.x = mouse_x - self.lazeImage_rect.width / 1.9
                        self.lazeImage_rect.y = mouse_y - self.lazeImage_rect.height / 1.1
                        
                        lazeIndexes.append((self.lazeImage_rect.x, self.lazeImage_rect.y, pygame.time.get_ticks()))

            if self.startGame == False:
                if play_box.collidepoint(pygame.mouse.get_pos()):
                    if hover == 0:
                        pygame.mouse.set_cursor(pygame.cursors.broken_x)
                        self.bg.img_background = pygame.image.load(
                            './BTL1/assets/backgroundInit2.png')
                        self.screen.blit(self.bg.img_background, (0, 0))
                        hover = 1
                else:
                    if hover == 1:
                        pygame.mouse.set_cursor(pygame.cursors.arrow)
                        self.bg.img_background = pygame.image.load(
                            './BTL1/assets/backgroundInit1.png')
                        self.screen.blit(self.bg.img_background, (0, 0))
                        hover = 0
            elif self.inGame == False:
                self.bg.start()
                self.screen.blit(self.bg.img_background, (0, 0))

                fontCountdown = pygame.font.SysFont('comicsansms', 72)
                
                self.countdownTime = int(self.countdown - (pygame.time.get_ticks() - self.startTime) / 1000)
                if self.countdownTime > 2:
                    countdownText = fontCountdown.render(str(self.countdownTime - 2), True, self.COLORWHITE)
                    countdownPosition = countdownText.get_rect()
                    countdownPosition.center = (self.SCREEN_WIDTH / 2, self.SCREEN_HEIGHT / 2)
                    self.screen.blit(countdownText, countdownPosition)
                elif self.countdownTime == 2:
                    countdownText = fontCountdown.render("Start", True, self.COLORWHITE)
                    countdownPosition = countdownText.get_rect()
                    countdownPosition.center = (self.SCREEN_WIDTH / 2, self.SCREEN_HEIGHT / 2)
                    self.screen.blit(countdownText, countdownPosition)
                else:
                    self.inGame = True
                    self.startTime = pygame.time.get_ticks()
                    self.startTimeToAddNewZombie = pygame.time.get_ticks()

            else:
                # Update
                self.screen.blit(self.bg.img_background, (0, 0))
                pygame.mouse.set_visible(False)

                # Lấy vị trí chuột
                mouse_x, mouse_y = pygame.mouse.get_pos()
                # Tính toán vị trí mới cho hình ảnh sao cho nó nằm chính giữa con chuột
                self.mouseImage_rect.x = mouse_x - self.mouseImage_rect.width / 2.8
                self.mouseImage_rect.y = mouse_y - self.mouseImage_rect.height / 2

                self.addNewZombie(1)

                self.updateZombie()
                
                self.update()

                self.screen.blit(self.mouseImage, self.mouseImage_rect)
                self.displayLaze(lazeIndexes)

            # flip() the display to put your work on screen
            pygame.display.flip()

            clock.tick(60)  # limits FPS to 60


# pygame setup
pygame.init()
zombieGame = GameContainer()
zombieGame.start()

pygame.quit()