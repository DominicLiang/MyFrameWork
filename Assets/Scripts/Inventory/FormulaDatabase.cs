using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FormulaDatabase", menuName = "Inventory/FormulaDatabase")]
public class FormulaDatabase : ScriptableObject
{
    List<Formula> formulas;
}