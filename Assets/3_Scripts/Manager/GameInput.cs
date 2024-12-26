using UnityEngine;

public class GameInput : MonoBehaviour {

    private InputActions inputActions;
    private void Awake() {
        inputActions = new InputActions();
        inputActions.Player.Enable();
    }

    public Vector2 GetPlayerMovement() {
        Vector2 moveInputVector = inputActions.Player.Move.ReadValue<Vector2>();
        return moveInputVector;
    }

    public bool PlayerJump() {
        return inputActions.Player.Jump.triggered;
    }
    
    public bool PlayerPunch() {
        return inputActions.Player.Punch.triggered;
    }
    
    public bool PlayerKick() {
        return inputActions.Player.Kick.triggered;
    }
    
    public bool PlayerBlock() {
        return inputActions.Player.Block.IsPressed();
    }

    public bool PlayerDash() {
        return inputActions.Player.Dash.triggered;
    }
}
