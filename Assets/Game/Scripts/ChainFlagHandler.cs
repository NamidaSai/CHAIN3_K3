using UnityEngine;

namespace Game
{
    /// <summary>
    /// Allows to Create, Delete or Check for a given flag in CHAIN_SharedData
    /// </summary>
    public class ChainFlagHandler : MonoBehaviour
    {
        [SerializeField] private string targetFlag;
        
        public void CreateFlag()
        {
            CHAIN_SharedData.CreateFlag(targetFlag);
        }

        public void DeleteFlag()
        {
            CHAIN_SharedData.DeleteFlag(targetFlag);
        }

        public bool DoesFlagExist()
        {
            return CHAIN_SharedData.DoesFlagExist(targetFlag);
        }
    }
}