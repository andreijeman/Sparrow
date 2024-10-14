﻿using ConsoleUI;

namespace Client.Pages
{
    public class MenuPage : BasePage
    {
        private List<BaseInput> _inputs; 
        private int _inputIndex;

        public MenuPage()
        {
            _inputs = new List<BaseInput>();

            //Chats
            _inputs.Add(new ControllInput());
            //Settings
            _inputs.Add(new ControllInput());

            _inputIndex = 0;
            _inputs[_inputIndex].Active = true;

        }

        public override void Render()
        {

            
        }


    }
}
