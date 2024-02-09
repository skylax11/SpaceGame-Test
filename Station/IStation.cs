using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStation       
{
    public void Execute(Controller player);
    public string Description { get; set; }
}
