using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject Inca;

    void Update()
    {
        if (Inca != null) // Asegúrate de que el objeto Inca no sea nulo
        {
            Vector3 position = transform.position;
            position.x = Inca.transform.position.x; // Solo cambia el eje X
            transform.position = position;
        }
    }
}
