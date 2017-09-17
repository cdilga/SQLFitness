package solver;

import java.util.ArrayList;
import java.util.Collections;
import java.util.Iterator;
import java.util.Properties;

import multiObjective.GenericMOComparator;

import constraintHandling.GenericComparator;

import problem.Problem;
import utility.Logger;
import utility.OutOfFunctionEvaluationsException;
import utility.SolutionPool;
import utility.StagnationException;
import utility.Utils;
import utility.reporting.Reporting;

public class ConstrainedGA extends Algorithm
{
    private double crossoverRate;
       //private double mutationProb;
    private int noImprovementCounter;
    private double bestFitness;
    private ArrayList<Solution> population;
    private ArrayList<Solution> matingPool;


    @Override
    public void initialise(Properties props, Problem problem, Reporting reporting, SolutionPool p)
    {
	super.initialise(props, problem, reporting, p);
	noImprovementCounter = 0;
	bestFitness = 0;
	crossoverRate = Double.parseDouble(props.getProperty("crossover_prob"));
	population = new ArrayList<Solution>(populationSize);
	matingPool = new ArrayList<Solution>(matingPoolSize);

	for (int i = 0; i < populationSize; i++)
	{
	    Solution sol = pool.get();

	    try
	    {
		problem.evaluateSolution(sol);
	    } catch (OutOfFunctionEvaluationsException e)
	    {
		// TODO Auto-generated catch block
		Logger.getLogger().logLine(Logger.LOGFILE, "Ran out of FE during initialisation");
	    }
	    population.add(sol);
	}
    }
    
    
    private void addToPopulationNoDuplicates(Solution sol)
    {
	for (int i = population.size()-1; i > 0 ; i--)
	{
	    if (population.get(i).currentFitness == sol.currentFitness)
		return;
	}
	population.add(sol);
    }
   
    public void run()
    {
	try
	{
	    while (true)
	    {
		doStochasticUniversalSelection();
		//doBinaryTournamentWithNiching();
		//doBinaryTournament();
		ConstrainedGASolution sol1, sol2;
		int x1, x2;

		while(!matingPool.isEmpty())
		{
		    x1 = Utils.random.nextInt(matingPool.size());
		    sol1 = (ConstrainedGASolution) matingPool.remove(x1);
		    if (matingPool.size()> 1)
		    {
			do{
			    x2 = Utils.random.nextInt(matingPool.size());
			}while (x1 == x2);
			sol2 = (ConstrainedGASolution) matingPool.remove(x2);

			doUniformCrossover(sol1, sol2, crossoverRate);

		    }
		    //doMultipointMutation(sol1, mutationProb);
		    doMutation(sol1);
		}
		doSelection(currentComparator);
		try
		{
		    checkImprovement(reporting);
		} catch (StagnationException e)
		{
		    reinitialisePopulation();
		}
	    }
	} catch (OutOfFunctionEvaluationsException e)
	{
	    finishOff(reporting, pool);
	}

    }

    private void reinitialisePopulation() throws OutOfFunctionEvaluationsException
    {
	for (Iterator<Solution> iterator = population.iterator(); iterator.hasNext();)
	{
	    Solution sol = iterator.next();
	    sol.initialiseRandomly(problem.getCardinality(), problem.getOptions(), pool.getRandom());
	    problem.evaluateSolution(sol);
	}

    }




    private void doStochasticUniversalSelection()
    {
	double max = population.get(0).penaltyFitness;  
	double min = population.get(population.size()-1).penaltyFitness;

	double interval = (max - min)/(matingPoolSize+1);
	//System.out.println("Max "+max+" min "+min+" interval "+interval);
	double currentLocation = population.get(0).penaltyFitness -  Utils.random.nextDouble()*interval;
	int i = 0;
	int j = 0;
	while ( i < matingPoolSize)
	{
	    currentLocation -= interval;
	    //System.out.println("Current location "+currentLocation);
	    j = 0;
	    while (j < population.size()-1)
	    {
		if (currentLocation<population.get(j).penaltyFitness && currentLocation > population.get(j+1).penaltyFitness)
		{
		    if (population.get(j).penaltyFitness - currentLocation < currentLocation - population.get(j+1).penaltyFitness)
			matingPool.add(population.get(j));
		    else
			matingPool.add(population.get(j+1));
		    break;
		}
		else
		    j++;
	    }

	    i++;
	}
    }
    
    private void doUniformCrossover(ConstrainedGASolution sol1, ConstrainedGASolution sol2, double probability) throws OutOfFunctionEvaluationsException
    {
	if (sol1 == sol2)
	    return;
	Solution [] children = sol1.uniformCrossover(sol2, probability, pool);
	problem.evaluateSolution(children[0]);
	problem.evaluateSolution(children[1]);
	/*System.out.println("Parent 1 "+sol1);
	System.out.println("Parent 2 "+sol2);
	System.out.println("Child 1 "+children[0]);
	System.out.println("Child 2 "+children[1]);*/
	this.addToPopulationNoDuplicates(children[0]);
	this.addToPopulationNoDuplicates(children[1]);
    }

    private void doMutation(ConstrainedGASolution sol) throws OutOfFunctionEvaluationsException
    {
	Solution sol2 = sol.mutate(problem.getOptions(), pool);
	problem.evaluateSolution(sol2);
	this.addToPopulationNoDuplicates(sol2);
    }
    
    
    protected void doSelection(GenericComparator fitnessComp)
    {
	fitnessComp.prepare(population);
	Collections.sort(population, fitnessComp);
	fitnessComp.unprepare();
	
	for (int i = population.size()-1; i >= populationSize; i--)
	    pool.put(population.remove(i));
	/*System.out.println("Start");
		 for (int i = 0; i < population.size(); i++)
		{
		    System.out.println(population.get(i).getPenaltyFitness());
		}*/
    }

    private void checkImprovement(Reporting reporting) throws StagnationException
    {
	if (population.get(0).currentFitness == bestFitness)
	{
	    noImprovementCounter++;
	    // Logger.getLogger().logLine(Logger.LOGFILE, "No improvement");
	}
	else
	{
	    bestFitness = population.get(0).currentFitness;
	    //  Logger.getLogger().logLine(Logger.LOGFILE,"Best fitness "+bestFitness);
	    noImprovementCounter = 0;
	}
	if (noImprovementCounter == 50)
	{
	    /*  Logger.getLogger().logLine(Logger.LOGFILE,"Stagnation ");
	    Logger.getLogger().logLine(Logger.LOGFILE,"Adding "+population.get(0).currentFitness);
	    Logger.getLogger().logLine(Logger.LOGFILE,"Worst "+population.get(population.size()-1).currentFitness);*/
	    reporting.addToBest(population.get(0).clone(pool));
	    throw new StagnationException();
	}
    }


    private void finishOff(Reporting reporting, SolutionPool pool)
    {
	doSelection(currentComparator);
	reporting.addToBest(population.get(0).clone(pool));
	for (int i = population.size()-1; i >= 0 ; i--)
	{

	   System.out.println(problem.getSolutionInSequence(population.get(i)));
	    System.out.println(population.get(i).currentFitness);
	    System.out.println("Violations "+population.get(i).getConstraintViolation());
	    problem.getAverageSequenceLength(population.get(i));

	}
    }

    //not actually needed
    @Override
    public void setComparator(GenericMOComparator currentComparator)
    {
	// TODO Auto-generated method stub
	
    }
 

 


      


  /*  private void doBinaryTournament()
    {
	Solution sol1, sol2;
	int x1, x2 ;
	BitSet bs = new BitSet();//get rid of duplication
	for (int i = 0; i < matingPoolSize; i++)
	{
	    do{
		x1 = Utils.random.nextInt(populationSize);
		sol1 =  population.get(x1);
		//System.out.println("sol1 "+sol1.currentFitness);
		//System.out.println("sol2 "+sol2.currentFitness);
	    }while (bs.get(x1)) ;
	    do{
		x2 = Utils.random.nextInt(populationSize);
		sol2 = population.get(x2);
		//System.out.println("sol1 "+ x1+ " "+sol1.currentFitness);
		//System.out.println("sol2 "+ x2+ " "+sol2.currentFitness);
	    }while (bs.get(x2) || x1 == x2 );

	    if (sol1.currentFitness > sol2.currentFitness)
	    {
		matingPool.add(sol1);
		bs.set(x1);
	    }
	    else
	    {
		matingPool.add(sol2);
		bs.set(x2);
	    }

	}

    }*/
    /* This niching method is down to Deb (An investigation of niche and species formation in genetic function optimisation, 1989)
     * well, he did it in a continuous environment with the 0.1 factor; The timeout of 0.25*N is authentic*/
 /*   private void doBinaryTournamentWithNiching()
    {
	Solution sol1, sol2 = null;
	int x1, x2 ;
	int timeout = populationSize/4;
	int timeoutCounter = 0;
	double distance = 0;
	for (int i = 0; i < matingPoolSize; i++)
	{
	    timeoutCounter = 0;
	    x1 = Utils.random.nextInt(populationSize);
	    sol1 =  population.get(x1);
	    //System.out.println("sol1 "+sol1.currentFitness);
	    //System.out.println("sol2 "+sol2.currentFitness);

	    do{
		timeoutCounter++;
		if (timeoutCounter > timeout)
		{
		    matingPool.add(sol1);
		    sol1 = null;
		    // System.out.println("Breaking ties ");
		    break;
		}
		x2 = Utils.random.nextInt(populationSize);
		sol2 = population.get(x2);
		distance = Utils.getNormalisedHammingDistance(sol1, sol2, problem.getCardinality());
		//System.out.println("sol1 "+ x1+ " "+sol1.currentFitness);
		//System.out.println("sol2 "+ x2+ " "+sol2.currentFitness);
		//System.out.println("Distance "+distance);
	    }while (distance < 0.3 || x1 == x2 );

	    if (sol1 == null)
		continue;
	    if (sol1.currentFitness > sol2.currentFitness)
	    {
		matingPool.add(sol1);

	    }
	    else
	    {
		matingPool.add(sol2);

	    }

	}

    }*/

 /*   private void doSinglePointCrossover(GASolution sol1, GASolution sol2) throws OutOfFunctionEvaluationsException
    {
	if (sol1 == sol2)
	    return;
	Solution [] children = sol1.singlePointCrossover(sol2);
	problem.evaluateSolution(children[0]);
	problem.evaluateSolution(children[1]);
	System.out.println("Parent 1 "+sol1);
	System.out.println("Parent 2 "+sol2);
	System.out.println("Child 1 "+children[0]);
	System.out.println("Child 2 "+children[1]);
	this.addToPopulationNoDuplicates(children[0]);
	this.addToPopulationNoDuplicates(children[1]);
    }*/

  

 /*   private void doMultipointMutation(GASolution sol, double probability) throws OutOfFunctionEvaluationsException
    {
	Solution sol2 = sol.mutateMultipoint(problem.getOptions(), probability);
	problem.evaluateSolution(sol2);
	this.addToPopulationNoDuplicates(sol2);
    }*/

   





}
