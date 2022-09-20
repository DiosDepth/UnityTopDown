using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDInput : MonoBehaviour
{

    public enum ButtonState { OFF, ButtonDown, ButtonPressed, ButtonUp }



    public class TDButton
    {
        public TDStateMachine<ButtonState> state;
        public string buttonID;

        public delegate void ButtonDownDelegate();
        public delegate void ButtonPressedDelegate();
        public delegate void ButtonUpDelegate();

        public  ButtonDownDelegate ButtonDownMethod;
        public  ButtonPressedDelegate ButtonPressedMethod;
        public  ButtonUpDelegate ButtonUpMethod;

        public TDButton(string btnID, ButtonDownDelegate btnDown = null, ButtonPressedDelegate btnPressed = null, ButtonUpDelegate btnUp = null, GameObject target = null)
        {
            buttonID = btnID;
            ButtonDownMethod = btnDown;
            ButtonPressedMethod = btnPressed;
            ButtonUpMethod = btnUp;
            state = new TDStateMachine<ButtonState>(target);
            state.ChangeState(ButtonState.OFF);
        }

        public void TriggerButtonDown()
        {
            if(ButtonDownMethod == null)
            {
                state.ChangeState(ButtonState.ButtonDown);
            }
            else
            {
                ButtonDownMethod();
            }
        }

        public void TriggerButtonPressed()
        {
            if(ButtonPressedMethod == null)
            {
                state.ChangeState(ButtonState.ButtonPressed);
            }
            else
            {
                ButtonPressedMethod();
            }
        }

        public void TriggerButtonUp()
        {
            if(ButtonUpMethod == null)
            {
                state.ChangeState(ButtonState.ButtonUp);
            }
            else
            {
                ButtonUpMethod();
            }

        }

    }

}
