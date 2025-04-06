using System.Collections.Generic;
using Game.Interactions;
using UnityEngine;

namespace Game.Puzzles
{
    public class Puzzle_1 : MonoBehaviour
    {
        public List<string> FinalButtonClickList = new();
        public List<string> CurrentButtonClickList = new();
        [SerializeField] private Bridge _bridge;
        public void ClickRed()
        {
            CurrentButtonClickList.Add("red");
            CheckIfCompleted();
        }
        public void ClickBlue()
        {
            CurrentButtonClickList.Add("blue");
            CheckIfCompleted();
        }
        public void ClickYellow()
        {
            CurrentButtonClickList.Add("yellow");
            CheckIfCompleted();
        }

        private void CheckIfCompleted()
        {
            if(CurrentButtonClickList.Count != FinalButtonClickList.Count) return;

            for (int i = 0; i < FinalButtonClickList.Count; i++)
            {
                if (CurrentButtonClickList[i] != FinalButtonClickList[i])
                {
                    ResetPuzzle();
                    return;
                }
            }
            
            _bridge.DrawBrige();
            //puzzle finished
        }

        private void ResetPuzzle()
        {
            CurrentButtonClickList.Clear();
            ButtonInteraction[] buttons = GetComponentsInChildren<ButtonInteraction>();

            foreach (var button in buttons)
            {
                button.ResetButton();
            }
        }
    }
}