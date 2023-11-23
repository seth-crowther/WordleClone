using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WordleClone
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch spriteBatch;
        private Page page;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Globals.wordPicker = new WordPicker();
            page = new Page();
        }

        protected override void Initialize()
        {
            //Setting up fullscreen
            _graphics.PreferredBackBufferWidth = (int)Globals.ScreenDims.X;
            _graphics.PreferredBackBufferHeight = (int)Globals.ScreenDims.Y;
            Window.IsBorderless = false;
            _graphics.ApplyChanges();

            Globals.wordPicker.PickWord();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Globals.graphicsDevice = GraphicsDevice;
            Globals.arial = Content.Load<SpriteFont>("Fonts/Arial");
            spriteBatch = new SpriteBatch(Globals.graphicsDevice);
            page.Initialise();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            page.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            page.Draw(spriteBatch);
            base.Draw(gameTime);
            spriteBatch.End();
        }
    }
}
