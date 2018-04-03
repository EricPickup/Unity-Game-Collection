using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour {

	private static Level [] lvls = new Level[3];
	private static int [] eScores = new int[5];
	private static int [] colors = new int[5];

	public void setLevels(Level bronze, Level silver, Level gold){
		lvls[0] = bronze;
		lvls[1] = silver;
		lvls[2] = gold;
	}

	public static Level getLevel(int i){
		return lvls[i];
	}

	public void setScores(int one, int two, int three, int four, int five){
		eScores[0] = one;
		eScores[1] = two;
		eScores[2] = three;
		eScores[3] = four;
		eScores[4] = five;
	}

	public void setColors(int one, int two, int three, int four, int five){
		colors[0] = one;
		colors[1] = two;
		colors[2] = three;
		colors[3] = four;
		colors[4] = five;
	}

	public static int getColor(int i){
		return colors[i];
	}

	public static int getScore(int i){
		return eScores[i];
	}
}
