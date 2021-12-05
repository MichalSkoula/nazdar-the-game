using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;

class Mouse
{
    static MouseState currentMouseState;
    static MouseState previousMouseState;

    public static MouseState GetState()
    {
        previousMouseState = currentMouseState;
        currentMouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();
        return currentMouseState;
    }

    public static bool IsPressed(bool left)
    {
        if (left)
        {
            return currentMouseState.LeftButton == ButtonState.Pressed;
        } 
        else
        {
            return currentMouseState.RightButton == ButtonState.Pressed;
        }
    }

    public static bool HasBeenPressed(bool left)
    {
        if (left)
        {
            return currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton != ButtonState.Pressed;
        }
        else
        {
            return currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton != ButtonState.Pressed;
        }
    }
}