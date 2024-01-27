import os, time, pygame
# Load our scenes
from states.Title import Title

class Game():
    def __init__(self):
        pygame.init()
        pygame.mixer.init()

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

        self.game_canvas = pygame.Surface((self.SCREEN_WIDTH,self.SCREEN_HEIGHT))
        self.screen = pygame.display.set_mode((self.SCREEN_WIDTH,self.SCREEN_HEIGHT))

        self.running, self.playing = True, True
        self.actions = {"left": False, "right": False, "pause" : False, "start" : False}
        self.mouse_pos = (0,0)
        self.dt, self.prev_time = 0, 0
        self.state_stack = []
        self.load_assets()
        self.load_states()

    def game_loop(self):
        self.load_sounds()
        while self.playing:
            self.get_dt()
            self.get_events()
            self.update()
            self.render()

    def get_events(self):
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                self.playing = False
                self.running = False
            if event.type == pygame.MOUSEBUTTONDOWN:
                mouse_click = pygame.mouse.get_pressed()
                if mouse_click[0]:
                    self.actions['left'] = True
                    if self.play_pickup_sound:
                        self.pickup_sound.play()
                if mouse_click[2]:
                    self.actions['right'] = True
                    if self.play_pickup_sound:
                        self.pickup_sound.play()  # Play click sound
                self.mouse_pos = pygame.mouse.get_pos()
            if event.type == pygame.KEYDOWN:
                if event.key == pygame.K_ESCAPE:
                    self.actions['pause'] = True
                if event.key == pygame.K_RETURN:
                    self.actions['start'] = True
                self.mouse_pos = (0,0)

            if event.type == pygame.MOUSEBUTTONUP:
                mouse_click = pygame.mouse.get_pressed()
                if mouse_click[0]:
                    self.actions['left'] = False
                if mouse_click[2]:
                    self.actions['right'] = False
                self.mouse_pos = pygame.mouse.get_pos()
            if event.type == pygame.KEYUP:
                if event.key == pygame.K_ESCAPE:
                    self.actions['pause'] = False   
                if event.key == pygame.K_RETURN:
                    self.actions['start'] = False
                self.mouse_pos = (0,0)

    def update(self):
        self.state_stack[-1].update(self.actions, self.screen)

    def render(self):
        self.state_stack[-1].render(self.game_canvas)
        # Render current state to the screen
        self.screen.blit(pygame.transform.scale(self.game_canvas,(self.SCREEN_WIDTH, self.SCREEN_HEIGHT)), (0,0))
        pygame.display.flip()

    def get_dt(self):
        now = time.time()
        self.dt = now - self.prev_time
        self.prev_time = now

    def draw_text(self, surface, text, color, x, y):
        text_surface = self.font.render(text, True, color)
        #text_surface.set_colorkey((0,0,0))
        text_rect = text_surface.get_rect()
        text_rect.center = (x, y)
        surface.blit(text_surface, text_rect)

    def load_assets(self):
        # Create pointers to directories 
        self.assets_dir = os.path.join("./BTL1/assets")
        self.background_dir = os.path.join(self.assets_dir, "background")
        self.sprite_dir = os.path.join(self.assets_dir, "sprites")
        self.font_dir = os.path.join(self.assets_dir, "fonts")
        self.sound_dir = os.path.join(self.assets_dir, "sounds")
        self.huge_font = pygame.font.Font(os.path.join(self.font_dir, "font.ttf"), 70)
        self.large_font = pygame.font.Font(os.path.join(self.font_dir, "font.ttf"), 50)
        self.medium_font = pygame.font.Font(os.path.join(self.font_dir, "font.ttf"), 30)
        self.medium_rare_font = pygame.font.Font(os.path.join(self.font_dir, "font.ttf"), 25)
        self.small_font = pygame.font.Font(os.path.join(self.font_dir, "font.ttf"), 15)

    def load_states(self):
        self.title_screen = Title(self)
        self.state_stack.append(self.title_screen)

    def reset_keys(self):
        for action in self.actions:
            self.actions[action] = False

    def load_sounds(self):
        self.background_sound = pygame.mixer.Sound(os.path.join(self.sound_dir, "background_sound.mp3"))

        self.play_pickup_sound = True
        self.pickup_sound = pygame.mixer.Sound(os.path.join(self.sound_dir, "pickup.wav"))
        
        self.punch_sound = pygame.mixer.Sound(os.path.join(self.sound_dir, "punch.wav"))
        self.explosion_sound = pygame.mixer.Sound(os.path.join(self.sound_dir, "explosion.wav"))
        self.wrong_sound = pygame.mixer.Sound(os.path.join(self.sound_dir, "wrong.mp3"))

        self.background_sound.play(loops=-1)


if __name__ == "__main__":
    g = Game()
    while g.running:
        g.game_loop()