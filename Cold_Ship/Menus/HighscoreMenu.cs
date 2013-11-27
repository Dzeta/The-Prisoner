//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;

//namespace Cold_Ship
//{
//    public class HighscoreMenu : Menu
//    {
//        private SpriteFont scoreFont;
//        private float timer = 0;
//        private float interval = 80;

//        private int[] scores = new int[10];
//        int nScores = 10;

//        private string filePath;

//        public HighscoreMenu(Texture2D background, Texture2D cursorTexture, SpriteFont font, SpriteFont scoreFont, Cue menuClick)
//            : base(background, cursorTexture, font, 1, menuClick)
//        {
//            this.optionMenu = new String[1] { "Go back to Main Menu" };
//            this.optionPosition = new Vector2[1] { new Vector2(320, 420) };
//            this.scoreFont = scoreFont;

//            try
//            {
//                filePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\highscore.ff";
//                string[] tmpScore = System.IO.File.ReadAllLines(filePath);
//                for (int i = 0; i < nScores; i++)
//                {
//                    scores[i] = Convert.ToInt32(tmpScore[i]);
//                }
//            }
//            catch (Exception exc)
//            {
//                if (exc is FileNotFoundException || exc is DirectoryNotFoundException || exc is UnauthorizedAccessException)
//                {
//                    scores = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
//                }
//                else
//                    throw;                
//            }
//        }

//        public void Save(int score)
//        {
//            int newHighScoreI = nScores;

//            for (int i = nScores-1; i >= 0 ; i--)
//            {
//                if (score > scores[i])
//                    newHighScoreI = i;
//            }

//            if (newHighScoreI < nScores)
//            {
//                int tmp;

//                for (int i = newHighScoreI; i < nScores; i++)
//                {
//                    tmp = scores[i];
//                    scores[i] = score;
//                    score = tmp;
//                }
//            }
//            saveFile();
//        }

//        private void saveFile()
//        {
//            string[] highScore = new string[nScores];
            
//            for (int i = 0; i < nScores; i++)
//            {
//                highScore[i] = Convert.ToString(scores[i]);
//            }

//            System.IO.File.WriteAllLines(@filePath, highScore);
//        }

//        public override void Update(GameTime gameTime, ref Cold_Ship.State currentState)
//        {
//            KeyboardState ks = Keyboard.GetState();

//            if (timer < interval)
//                timer += gameTime.ElapsedGameTime.Milliseconds;
//            else
//            {
//                if (ks.IsKeyDown(Keys.Enter))
//                {
//                    currentState = Cold_Ship.State.MAIN_MENU;
//                    timer = 0;
//                }
//            }
//            base.UpdateFont(3);
//        }

//        public override void Draw(SpriteBatch SpriteBatch)
//        {
//            base.Draw(SpriteBatch);
//            for (int i = 0; i < nScores; i++)
//            {
//                if(i==9)
//                    SpriteBatch.DrawString(scoreFont, (i+1).ToString() + ". " + scores[i].ToString(), new Vector2(330, 250 + 15*i), Color.White);
//                else
//                    SpriteBatch.DrawString(scoreFont, (i + 1).ToString() + ".  " + scores[i].ToString(), new Vector2(330, 250 + 15 * i), Color.White);
//            }
//        }
//    }
//}
