using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class MCTSTree
{
    public CharacterActionType action;
    public GameState gameState;
    private List<MCTSTree> children;
    private int nbVictories;
    private int nbSimulations;

    public MCTSTree(CharacterActionType action, GameState gameState)
    {
        this.action = action;
        this.gameState = gameState;
        nbVictories = 0;
        nbSimulations = 0;
        children = new List<MCTSTree>();
    }
    public List<MCTSTree> Expand()
    {
        children.Add(new MCTSTree(CharacterActionType.idle, gameState));
        children.Add(new MCTSTree(CharacterActionType.jump, gameState));
        children.Add(new MCTSTree(CharacterActionType.goLeft, gameState));
        children.Add(new MCTSTree(CharacterActionType.goRight, gameState));
        children.Add(new MCTSTree(CharacterActionType.attack, gameState));
        return children;
    }
    public void AddSimulationResult(int nbVictories, int nbSimulations)
    {
        this.nbVictories += nbVictories;
        this.nbSimulations += nbSimulations;
    }

    public int GetSimulationResult()
    {
        return nbVictories;
    }

}