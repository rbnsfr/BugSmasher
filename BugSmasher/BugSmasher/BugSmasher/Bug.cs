using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BugSmasher
{
    class Bug : Sprite
    {
        public int mood; // 0 = normal, 1 = angry, 2 = intriguied, etc.
        public Vector2 Target;

        public Bug(
            Vector2 location,
            Texture2D texture,
            Rectangle initialFrame,
            Vector2 velocity) : base (location, texture, initialFrame, velocity)
        {
            Target = Vector2.Zero;
        }

        public override void Update(GameTime gameTime)
        {
            if (Target != Vector2.Zero)
            {
                Vector2 vel = Target - Center;
                vel.Normalize();
                vel *= 100;

                Velocity = vel;
                Rotation = (float)Math.Atan2(vel.Y, vel.X);
            }

            // special functions for updating only this
            base.Update(gameTime);
        }
    }
}
