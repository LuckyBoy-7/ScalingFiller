using System.Collections.Generic;
using Lucky.Managers;

public class GameManager: Singleton<GameManager>
{
    public List<Square> squares = new();
}