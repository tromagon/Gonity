using System;

[Flags]
public enum OpenMode 
{
    None = 0,
    Unique = 1 << 1,
    Overlay = 1 << 2
}