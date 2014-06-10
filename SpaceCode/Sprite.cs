using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;

namespace SpaceCode
{
    public partial class Sprite
    {
        public Vector2F Velocity;
        public Vector2F Position;

        public float RotationalVelocityInDegrees = 0;
        public float RotationInDegrees = 0;

        public SpaceControl _Space;

        private Sprite _Parent;
        protected Matrix TransformLocalToParent;
        protected Image _Image;
        protected Bitmap _Bitmap;
        protected string _ImageFileName = "";

        public Sprite()
        {
        }


        public float RotationInRadians
        {
            get { return (float)(RotationInDegrees * Math.PI / 180); }
        }

        public SpaceControl Space
        {
            get { return _Space; }
            set
            {
                if (_Space != value)
                {
                    if (null != _Space)
                    {
                        _Space.RemoveSprite(this);
                    }

                    _Space = value;
                    if (null != _Space)
                    {
                        Space.AddSprite(this);
                    }
                }
            }
        }

        public string ImageFileName
        {
            get { return _ImageFileName; }
            set
            {
                if (_ImageFileName != value)
                {
                    if (null == value || "" == value)
                    {
                        _ImageFileName = null;
                        _Image = null;
                        _Bitmap = null;
                        return;
                    }

                    _ImageFileName = value;
                    string cd = Directory.GetCurrentDirectory();
                    if (File.Exists(value))
                    {
                        _Image = Image.FromFile(value);
                        _Bitmap = new Bitmap(value);
                    }
                }
            }
        }

        public virtual void OnTick(TimeSpan delta)
        {
            if (null == Parent)
            {
                Position.X += Velocity.X * (float)delta.TotalSeconds;
                Position.Y += Velocity.Y * (float)delta.TotalSeconds;
                RotationInDegrees += RotationalVelocityInDegrees * (float)delta.TotalSeconds;
            }
            else
            {
                Matrix transform = TransformLocalToWorld;
                Position.X = transform.OffsetX;
                Position.Y = transform.OffsetY;
            }
            if (Position.X > Space.Width)
                Position.X -= Space.Width;
            else if (Position.X < 0)
                Position.X += Space.Width;

            if (Position.Y > Space.Height)
                Position.Y -= Space.Height;
            else if (Position.Y < 0)
                Position.Y += Space.Height;
        }

        public void Paint(Graphics g)
        {
            if (null != _Image)
            {
                g.Transform = TransformLocalToWorld;

                // Add the image.
                Vector2F draw_location = new Vector2F(0, 0);
                g.DrawImage(_Image, draw_location);
            }
        }

        public Matrix TransformLocalToWorld
        {
            get
            {
                if (null == Parent)
                {
                    Vector2F translation = Position;
                    if (null != _Image)
                        translation -= new Vector2F(_Image.Size) / 2;

                    Matrix m = new Matrix();
                    m.RotateAt(RotationInDegrees, Position);
                    m.Translate((float)translation.X, (float)translation.Y);
                    return m;
                }
                else
                {
                    Matrix parent_to_world = Parent.TransformLocalToWorld.Clone();
                    Matrix local_to_parent = this.TransformLocalToParent.Clone();
                    parent_to_world.Multiply(local_to_parent);
                    return parent_to_world;
                }

            }
        }

        internal bool IsCollidingWith(Sprite s2)
        {
            // A Sprite colliding with itself is ignored.
            if (this == s2)
                return false;

            // If a sprite hits something that doesn't exists, ignore.
            if (s2._Image == null || this._Image == null)
                return false;

            
            if (this is Missile)
            {
                // Ignore collisions with ships. We are interested in missiles and planets only.
                if ((s2 is AIShip) || (s2 is Ship)) return false;
            }
            else
                return false;

            Vector2F offset = this.Position - s2.Position;

            double my_radius = Math.Sqrt(_Image.Width * _Image.Width + _Image.Height * _Image.Height)/2;
            double s2_radius = Math.Sqrt(s2._Image.Width * s2._Image.Width + s2._Image.Height * s2._Image.Height)/2;
            if (offset.Length > my_radius + s2_radius)
                return false;

            Bitmap b = new Bitmap(_Image.Width, _Image.Height);
            Graphics g = Graphics.FromImage(b);
            Matrix s2_to_world = s2.TransformLocalToWorld;
            
            Matrix world_to_local = TransformLocalToWorld;
            world_to_local.Invert();

            Matrix s2_to_local = world_to_local;
            s2_to_local.Multiply(s2_to_world);

            g.Transform = s2_to_local;

            Vector2F draw_location = new Vector2F(0,0);
            g.DrawImage(s2._Image, draw_location);

            for (int x = 0; x < b.Width; ++x)
                for (int y = 0; y < b.Height; ++y)
                {
                    Color mine = _Bitmap.GetPixel(x, y);
                    Color theirs = b.GetPixel(x, y);
                    
                    if (mine.A > 0.5 && theirs.A > 0.5)
                        return true;
                }

            return false;
        }

        virtual internal void Hit(Sprite s2)
        {
        }

        public Sprite Parent
        {
            get { return _Parent; }
            set 
            {
                if (!CanBeParent(value))
                    return;

                _Parent = null;
                Matrix local_to_world_transform = TransformLocalToWorld.Clone();

                _Parent = value;

                if (null != _Parent)
                {
                    Matrix m = Parent.TransformLocalToWorld.Clone();
                    m.Invert();
                    Matrix world_to_parent = m;

                    TransformLocalToParent = world_to_parent;
                    TransformLocalToParent.Multiply(local_to_world_transform);

                    Velocity = new Vector2F(0, 0);
                }
            }
        }

        protected bool CanBeParent(Sprite value)
        {
            List<Sprite> ancestors = new List<Sprite>();
            Sprite parent = value;
            while (null != parent)
            {
                if (this == parent)
                    return false;
                parent = parent.Parent;
            }

            return true;
        }
    }
}
    