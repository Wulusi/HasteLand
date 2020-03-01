using System.Collections;
using UnityEngine;

public class EnemyHealthBarUI : MonoBehaviour
{
    [SerializeField]
    protected Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        //Assuming only one camera exist in game
        mainCamera = FindObjectOfType<Camera>();

        StartCoroutine(StartHealthBars());
    }

    private IEnumerator StartHealthBars()
    {
        while (true)
        {
            FaceCamera();
            yield return null;
        }
    }

    private void FaceCamera()
    {
        var LookBack = transform.position + mainCamera.transform.rotation * Vector3.back;
        var LookUp = mainCamera.transform.rotation * Vector3.up;

        transform.LookAt(LookBack, LookUp);
    }
}
