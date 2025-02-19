using System.Diagnostics;
using System.Net.Mime;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Velentr.INDIVIDUAL_SUPPORT.Shared.TestApp;

public class TestGame : PerformanceMonitoredGame
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public TestGame() : base(
        title: "Velentr.INDIVIDUAL_SUPPORT",
        version: FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion,
#if FNA
        framework: "FNA",
#elif MONOGAME
        framework: "Monogame",
#else
        framework: "Unknown",
#endif
        font: "font",
        fontColor: Color.Black,
        metricsPosition: new Vector2(5, 5)
        )
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    private void UpdateResolution(int width, int height, bool fullscreen, GraphicsDeviceManager graphics)
    {
        graphics.IsFullScreen = fullscreen;
        graphics.PreferredBackBufferWidth = width;
        graphics.PreferredBackBufferHeight = height;
        graphics.ApplyChanges();
    }

    protected override void Initialize()
    {
        UpdateResolution(1280, 768, false, _graphics);
        
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        base.LoadContent();
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();
        RenderMetrics(gameTime, this._spriteBatch);

        // TODO: Add your drawing code here

        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
