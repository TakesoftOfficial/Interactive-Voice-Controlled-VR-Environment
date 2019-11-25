using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpeechLib;
using System.IO;

public abstract class Command
{

     ~Command() { }
    void execute() { }
    void undo() { }
    

}
