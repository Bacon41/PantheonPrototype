using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PantheonPrototype
{
    /// <summary>
    /// The Entity defines an object within the game. It has physical presence
    /// in the game as a Sprite and can collide with other Entities.
    /// </summary>
    public class Entity
    {
        /// <summary>
        /// This is the point around which the sprite is drawn. It functions
        /// as the center of the entity and is automatically set to the center
        /// of the bounding box.
        /// </summary>
        protected Vector2 actionPoint;

        public Vector2 Location
        {
            get { return actionPoint; }
            set { actionPoint = value; }
        }

        /// <summary>
        /// Gets and sets the action point relative to the top left corner.
        /// </summary>
        public Vector2 ActionPoint
        {
            get
            {
                return new Vector2(
                    -drawingBox.X,
                    -drawingBox.Y);
            }
            set
            {
                drawingBox.X = -(int)value.X;
                drawingBox.Y = -(int)value.Y;
            }
        }

        protected Vector2 prevLocation;

        public Vector2 PrevLocation
        {
            get { return prevLocation; }
            set { prevLocation = value; }
        }

        /// <summary>
        /// The box to which the sprite will be drawn.
        /// </summary>
        protected Rectangle drawingBox;

        public Rectangle DrawingBox
        {
            get
            {
                return new Rectangle(
                    (int)(Location.X + drawingBox.X),
                    (int)(Location.Y + drawingBox.Y),
                    drawingBox.Width,
                    drawingBox.Height
                    );
            }
            set { drawingBox = value; }
        }

        /// <summary>
        /// Puppies!!! No puppies, actually fruit.
        /// 
        /// Also, this is the bounding box.
        /// </summary>
        protected Rectangle boundingBox;

        public Rectangle BoundingBox
        {
            get {
                return new Rectangle(
                    (int)(DrawingBox.X + boundingBox.X),
                    (int)(DrawingBox.Y + boundingBox.Y),
                    boundingBox.Width,
                    boundingBox.Height
                    );
            }
            set { boundingBox = value; }
        }

        /// <summary>
        /// The name the entity has within level.
        /// </summary>
        protected string name;

        public string Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// A string representing the current state.
        /// </summary>
        protected string currentState;

        public string CurrentState {
            get
            {
                return currentState;
            }
            set
            {
                //Change the sprite state
                sprite.changeState(value);

                //Change the current state
                currentState = value;
            }
        }

        /// <summary>
        /// A list of characteristics which determine what an entity does on collision.
        /// 
        /// Basically, this is a list that determines which collision types to check.
        /// </summary>
        protected List<string> characteristics;

        public List<string> Characteristics
        {
            get { return characteristics; }
        }

        /// <summary>
        /// This tells the level that it is dead, and needs to be removed.
        /// </summary>
        protected bool toDestroy;

        public bool ToDestroy
        {
            get { return toDestroy; }
        }

        /// <summary>
        /// The visual representation of the entity.
        /// </summary>
        protected Sprite sprite;

        public Sprite Sprite
        {
            get { return sprite; }
            set { sprite = value; }
        }

        /// <summary>
        /// A no parameter entity for conveniences sake.
        /// </summary>
        public Entity()
            : this(0,0,new Rectangle(-20, -20, 40, 40), new Rectangle(15, 15, 10,10), "George I") // Please note that George the First should not be noted.
        { }

        /// <summary>
        /// Constructs a basic entity. This is the technical constructor. Unless
        /// you understand exactly how the entity is stored, I recommend using the
        /// other constructor.
        /// </summary>
        /// <param name="x">The x coordinate of the action point.</param>
        /// <param name="y">The y coordinate of the action point.</param>
        /// <param name="drawBox">The draw box defines the area to which the sprite
        /// is drawn relative to the upper left hand corner..</param>
        /// <param name="boundingBox">The bounding box relative to the action point.</param>
        public Entity(float x, float y, Rectangle drawBox, Rectangle boundingBox, string name)
        {
            this.sprite = new Sprite();
            this.drawingBox = drawBox;
            this.boundingBox = boundingBox;
            this.actionPoint = new Vector2(x, y);
            this.prevLocation = actionPoint;
            this.toDestroy = false;

            currentState ="Default";

            this.characteristics = new List<string>();
        }

        /// <summary>
        /// Constructs a basic entity. This is a more accessible constructor.
        /// In general, use this constructor when making new entities.
        /// </summary>
        /// <param name="location">The location of the entity relative to global space. Note that the reference point of the entity is the center of the bounding box.</param>
        /// <param name="drawBox">The box to which the sprite will be drawn. Only the width and height will be used.</param>
        /// <param name="boundingBox">The bounding box of the entity relative to the upper right hand corner of the entity.</param>
        public Entity(Vector2 location, Rectangle drawBox, Rectangle boundingBox, string name)
            : this(
                location.X,
                location.Y,
                new Rectangle(
                    -(boundingBox.X + boundingBox.Width/2),
                    -(boundingBox.Y + boundingBox.Height/2),
                    drawBox.Width,
                    drawBox.Height),
                new Rectangle(
                    boundingBox.X,
                    boundingBox.Y,
                    boundingBox.Width,
                    boundingBox.Height),
                name
                )
        { }

        /// <summary>
        /// Loads any assets this particular entity needs.
        /// </summary>
        public virtual void Load(ContentManager contentManager)
        {
        }

        /// <summary>
        /// Updates the entity... yeah.
        /// </summary>
        /// <param name="gameTime">Even has the option for frame rate, pretty cool eh?</param>
        /// <param name="gameReference">The ugly global game reference of doom.</param>
        public virtual void Update(GameTime gameTime, Pantheon gameReference)
        {
            this.sprite.Update(gameTime);
        }

        /// <summary>
        /// Detects collisions between another entity.
        /// 
        /// Uses the rectangle defining sprite to detect collisions.
        /// </summary>
        /// <param name="other">The entity to detect collision with.</param>
        /// <returns>True if the bounding rectangles overlap.</returns>
        public bool collidesWith(Entity other)
        {
            return this.boundingBox.Intersects(other.boundingBox);
        }

        /// <summary>
        /// Draws the entity to its current location.
        /// </summary>
        /// <param name="canvas">An initialized sprite batch to draw the sprite upon.</param>
        public virtual void Draw(SpriteBatch canvas)
        {
            this.sprite.Draw(canvas, new Rectangle((int)actionPoint.X, (int)actionPoint.Y, drawingBox.Width, drawingBox.Height), new Vector2(-drawingBox.X, -drawingBox.Y));
        }
    }
}
