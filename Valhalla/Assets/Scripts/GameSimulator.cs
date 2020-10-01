using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameSimulator
{
    private static GameState _gameState;
    private static int _currentID; // ID of the current simulated player
    //Resets the simulation in order to start a new one.
    public static void ResetSimulation(GameState newState, int id)
    {
        _gameState = newState;
        _currentID = id;
    }
    //Returns true if one of the player won (or maybe after x simulations)
    public static bool IsSimulationFinished()
    {
        return _gameState.gameWon;
    }

    //Will return the next actions the AI can do (for example if he's in mid air he can't jump)
    public static List<CharacterActionType> GetNextPossibleActions(MCTSTree node)
    {
        List<CharacterActionType> possibleActions = new List<CharacterActionType>();
        possibleActions.Add(CharacterActionType.idle);
        possibleActions.Add(CharacterActionType.jump);
        possibleActions.Add(CharacterActionType.goLeft);
        possibleActions.Add(CharacterActionType.goRight);
        possibleActions.Add(CharacterActionType.attack);
        return possibleActions;
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
        switch (action)
        {
            case CharacterActionType.idle: GameSimulator.SimulateIdle();break;
            case CharacterActionType.goLeft: GameSimulator.SimulateGoLeft();break;
            case CharacterActionType.goRight: GameSimulator.SimulateGoRight();break;
            case CharacterActionType.jump: GameSimulator.SimulateJump();break;
            case CharacterActionType.attack: GameSimulator.SimulateAttack();break;
            default: GameSimulator.SimulateIdle();break;
        }

        checkWin();
    }

    //Return if the player won or not by giving it's id to identify if the result is on the point of view of the player 1 or player 2
   public static int GetResult()
   {
       if (_gameState.advantage == 1 && _gameState.gameWon && _currentID == 2)
       {
           return 1;
       }
       else if (_gameState.advantage == -1 && _gameState.gameWon && _currentID == 1)
       {
           return 1;
       }
       return 0;
    }

   private static void SimulateIdle()
   {
       _gameState.players[_currentID-1].velocity = Vector3.zero;
       
   }

   private static void SimulateGoLeft()
   {
       
   }
   private static void SimulateGoRight()
   {
       
   }
   private static void SimulateJump()
   {
   }

   private static void SimulateAttack()
   {
       
   }

   private static void checkWin()
   {
       Bounds currentAgentBounds = new Bounds(_gameState.players[_currentID-1].position, GameManager.Instance.characterColliderSize );
       Bounds winZoneBoudBounds = GameManager.Instance.WinZoneBounds[_currentID-1];
       bool intersect = ((Mathf.Abs(winZoneBoudBounds.min.x - currentAgentBounds.min.x) * 2) <(winZoneBoudBounds.size.x + currentAgentBounds.size.x))
                        && ((Mathf.Abs(winZoneBoudBounds.min.y - currentAgentBounds.min.y) * 2) <(winZoneBoudBounds.size.y + currentAgentBounds.size.y));
       _gameState.gameWon =  intersect;
   }

   
   
}