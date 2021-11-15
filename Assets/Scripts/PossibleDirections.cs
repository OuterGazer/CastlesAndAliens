using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//[Flags]
/*public enum PossibleDirections
{
    Right = 0,
    Left = 1,
    Forward = 2,
    Backward = 3,

    RightLeft = Right | Left,
    RightForward = Right | Forward,
    RightBackward = Right | Backward,
    LeftForward = Left | Forward,
    LeftBackward = Left | Backward,
    
    ForwardBackward = Forward | Backward,

    RightLeftForward = RightLeft | Forward,
    RightLeftBackward = RightLeft | Backward,
    LeftForwardBackward = LeftForward | Backward,
    ForwardBackwardRight = ForwardBackward | Right,

    ForwardBackwardRightLeft = RightLeft | ForwardBackward,
}*/

    public enum PossibleDirections
{
    Right,
    Left,
    Forward,
    Backward,

    RightLeft,
    RightForward,
    RightBackward,
    LeftForward,
    LeftBackward,

    ForwardBackward,

    RightLeftForward,
    RightLeftBackward,
    LeftForwardBackward,
    ForwardBackwardRight,

    ForwardBackwardRightLeft,
}
