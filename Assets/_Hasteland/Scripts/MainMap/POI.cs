using TMPro;
using System.Collections;
using UnityEngine;

public class POI : MonoBehaviour
{
    public bool isActive;
    private Renderer myRenderer;

    [Header("POI Settings")]
    public string title;
    public int index;
    public float yOverride;
    public TextMeshProUGUI titleText;
    public float titleAnimationSpeed;
    public GameObject nextPOI;
    public Material activeMaterial;
    public Material passiveMaterial;

    [Header("Line Settings")]
    private LineRenderer line;
    public Color activeColour;
    public Color passiveColour;

    [Header("Trail Settings")]
    private GameObject trail;
    public float trailSpeed;


    private void Start()
    {
        titleText = transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        myRenderer = transform.GetChild(1).GetComponent<Renderer>();
        trail = transform.GetChild(0).gameObject;
        line = GetComponent<LineRenderer>();
        RendererMaterial(isActive);

        if (nextPOI != null)
        {
            line.SetPosition(0, transform.position);
            line.SetPosition(1, nextPOI.transform.position);
            LineColour(isActive);
        }
        else
        {
            line.enabled = false;
        }

        StartCoroutine(Arrow());
        StartCoroutine(Title());
    }

    private void RendererMaterial(bool _isActive)
    {
        if (_isActive)
        {
            myRenderer.material = activeMaterial;
        }
        else
        {
            myRenderer.material = passiveMaterial;
        }
    }

    private void LineColour(bool _isActive)
    {
        if (_isActive)
        {
            line.startColor = activeColour;
            line.endColor = activeColour;
        }
        else
        {
            line.startColor = passiveColour;
            line.endColor = passiveColour;
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator Title()
    {
        int _index = 0;
        string _title = "";
        bool _isRunning = true;

        while (_isRunning)
        {
            if (_index<title.Length)
            {
                _title = string.Format("{0}{1}", _title, title[_index]);
                _index++;
                Debug.Log(_title);
                titleText.text = _title;
            }
            else
            {
                _isRunning = false;
            }
            yield return new WaitForSeconds(1f / titleAnimationSpeed);
        }
    }
    private IEnumerator Arrow()
    {
        if (nextPOI != null)
        {
            Transform _trail = trail.transform;
            Transform _nextPOI = nextPOI.transform;
            while (true)
            {
                //Debug.Log("ID:"+transform.name);
                float _distance = Vector3.Distance(_trail.position, _nextPOI.position);
                if (_distance > .1f)
                {
                    //Debug.Log(transform.name + " is moving towards " + _nextPOI.name + " | Distance: " + _distance);
                    _trail.position = Vector3.MoveTowards(_trail.position, _nextPOI.position, trailSpeed * Time.deltaTime);
                    yield return new WaitForEndOfFrame();
                }
                else
                {
                    //Debug.Log("Waiting for restart.");
                    yield return new WaitForSeconds(1f);
                    _trail.position = transform.position;
                }
            }
        }
    }

    private void OnEnable()
    {
        Physics.Raycast(transform.position, -transform.up, out RaycastHit _hit);
        transform.position = new Vector3(transform.position.x, _hit.point.y + yOverride, transform.position.z);
    }
}
