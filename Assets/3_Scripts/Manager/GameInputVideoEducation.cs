using UnityEngine;

public class GameInputVideoEducation : MonoBehaviour {

    private InputActions inputActions;
    private void Awake() {
        inputActions = new InputActions();
        inputActions.Player.Enable();
    }

    public Vector2 GetPlayerMovement() {
        Vector2 moveInputVector = inputActions.Player.Move.ReadValue<Vector2>();
        return moveInputVector;
    }

    public bool PlayerSalam() {
        return inputActions.Player.Salam.IsPressed();
    }

    public bool PlayerPembukaan() {
        return inputActions.Player.Pembukaan.IsPressed();
    }

        public bool PlayerPunch() {
        return inputActions.Player.Punch.IsPressed();
    }
    
    public bool PlayerKick() {
        return inputActions.Player.Kick.IsPressed();
    }
    
    public bool PlayerBlock() {
        return inputActions.Player.Block.IsPressed();
    }

    public bool PlayerDash() {
        return inputActions.Player.Dash.IsPressed();
    }
}
