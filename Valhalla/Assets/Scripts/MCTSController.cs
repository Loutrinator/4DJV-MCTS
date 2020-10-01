using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCTSController : AController {
    
    public int simulationAmountPerAction = 1;

    private MCTSTree tree;
    private MCTSTree currentNode;
    public override void ExecuteActions()
    {
        MCTSTree chosenAction = MCTSComputeAction();
    }

    private MCTSTree MCTSComputeAction()
    {
        int max = int.MinValue;
        MCTSTree bestAction = new MCTSTree(CharacterActionType.idle);
        
        foreach(var action in currentNode.Expand()) //Expansion
        {
            int numberVictory = 0;
            for (int i = 0; i < simulationAmountPerAction; ++i)
            {
                numberVictory += Simulate(action);
            }
            action.AddSimulationResult(numberVictory,simulationAmountPerAction); //Retropropagation
            if(max < action.GetSimulationResult()) //SimulationResult > -1
            {
                max = action.GetSimulationResult();
                bestAction = action;
            }
        }

        return bestAction;
    }

    private int Simulate(MCTSTree node)
    {
        GameSimulator.ResetSimulation(node.gameState);
        while (GameSimulator.IsSimulationFinished())
        {
            List<CharacterActionType> actions = GameSimulator.GetNextPossibleActions(node);
            CharacterActionType selectedAction = GameSimulator.GetRandomAction(actions);
            GameSimulator.PlayAction(selectedAction);
        }
        return GameSimulator.GetResult(0);
    }

    
    /*
     Void ComputeMCTS()
    {
        int max = MIN_INT;
        Action bestAction = null;
        foreach( possibleAction in CurrentNode.GetPossibleAction()) //Expansion
        {
            int numberVictory = SimulateResult(possibleAction); //Simulation (a faire plusieurs fois !)
            possibleAction.AddSimulationResult(numberVictory); //Retropropagation
            if(max < possibleAction.GetSimulationResult()) //SimulationResult > -1
            {
                max = possibleAction.GetSimulationResult();
                bestAction = possibleAction
            }
        }
    }
     */
    /*
    Int SimulateResult(Action possibleAction)
    {
        while(!Game.isFinished()) //Attention votre jeu doit être finit !
        {
            List<Action> actions = Game.GetNextPossibleAction(possibleAction);
            Action selectedAction = Game.GetRandomAction(actions);
            Game.PlayAction(selectedAction);
        }
        return Game.Result(); //0 si perdu 1 si win
    }*/
}
