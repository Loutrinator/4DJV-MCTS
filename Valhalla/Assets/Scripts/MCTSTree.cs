using System.Collections.Generic;

public class MCTSTree
{
    public CharacterActionType action;
    public GameState gameState;
    private List<MCTSTree> children;
    private int nbVictories;
    private int nbSimulations;

    public MCTSTree(CharacterActionType action)
    {
        this.action = action;
        nbVictories = 0;
        nbSimulations = 0;
        children = new List<MCTSTree>();
    }
    public List<MCTSTree> Expand()
    {
        children.Add(new MCTSTree(CharacterActionType.idle));
        children.Add(new MCTSTree(CharacterActionType.jump));
        children.Add(new MCTSTree(CharacterActionType.goLeft));
        children.Add(new MCTSTree(CharacterActionType.goRight));
        children.Add(new MCTSTree(CharacterActionType.attack));
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