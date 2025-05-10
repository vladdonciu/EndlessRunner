using UnityEngine;

public class Button3D : MonoBehaviour
{
    // Culorile pentru diferite stări
    public Color normalColor = Color.white;
    public Color hoverColor = Color.yellow;
    public Color pressedColor = Color.red;

    private Renderer _renderer;

    // Referință la MenuTransition
    public MenuTransition menuTransition;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.material.color = normalColor;
    }

    private void OnMouseEnter()
    {
        _renderer.material.color = hoverColor;
    }

    private void OnMouseExit()
    {
        _renderer.material.color = normalColor;
    }

    private void OnMouseDown()
    {
        _renderer.material.color = pressedColor;
    }

    private void OnMouseUp()
    {
        _renderer.material.color = hoverColor;

        // Apelăm funcția ReturnToMenu din MenuTransition
        if (menuTransition != null)
        {
            menuTransition.ReturnToMenu();
        }
        else
        {
            Debug.LogWarning("MenuTransition nu este setat pe Button3D!");
        }
    }
}
