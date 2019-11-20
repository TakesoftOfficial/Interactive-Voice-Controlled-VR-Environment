using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command
{

     ~Command() { }
    void execute() { }
    void undo() { }
    

}
