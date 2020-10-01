using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameSimulator
{
    private static GameState gameState;
    //Resets the simulation in order to start a new one.
    public static void ResetSimulation(GameState newState)
    {
        gameState = newState;
    }
    //Returns true if one of the player won (or maybe after x simulations)
    public static bool IsSimulationFinished()
    {
        return false;
    }

    //Will return the next actions the AI can do (for example if he's in mid air he can't jump)
    public static List<CharacterActionType> GetNextPossibleActions(MCTSTree node)
    {
        return new List<CharacterActionType>();
    }

    //Returns a random Action
    public static CharacterActionType GetRandomAction(List<CharacterActionType> actions)
    {
        int possibleActions = actions.Count;
        return actions[Random.Range(0, possibleActions)];
    }

    //Updates the current simulation's state by laying the action required by the AI.
    public static void PlayAction(CharacterActionType action)
    {
        
    }

    //Return if the player won or not by giving it's id to identify if the result is on the point of view of the player 1 or player 2
   public static int GetResult(int id)
    {
        return 0;
    }
}