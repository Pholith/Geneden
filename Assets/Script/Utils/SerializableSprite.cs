using Newtonsoft.Json;
using System;

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;

[Serializable]
public class SerializableSprite
{
    public static implicit operator SerializableSprite(Sprite p) => null;
    public static implicit operator Sprite(SerializableSprite p) => null;
}
