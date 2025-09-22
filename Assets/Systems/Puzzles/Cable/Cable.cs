using System;
using ScriptCable; // Namespace de CableManager
using UnityEngine;

public class Cable : MonoBehaviour
{
    public SpriteRenderer finalCable; // Sprite que se estira
    public GameObject light; // Luz que se activa
    private Vector2 positionOrigin;
    private Vector2 sizeOrigin;
    private bool isDragging = false;

    void Start()
    {
        if (finalCable == null) finalCable = GetComponent<SpriteRenderer>();
        positionOrigin = transform.position;
        sizeOrigin = finalCable.size;
        if (light != null) light.SetActive(false);
        // Debug color inicial
        Debug.Log($"Cable {gameObject.name} color: {finalCable.color}");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isDragging && IsMouseOverThis())
        {
            isDragging = true;
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            TryConnection();
            if (!IsConnected()) Restart(); // Resetea solo si no conectó
        }
    }

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            UpdatePosition();
            UpdateRotation();
            UpdateSize();
        }
    }

    private bool IsMouseOverThis()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return GetComponent<Collider2D>().OverlapPoint(mousePos);
    }

    public void UpdatePosition()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;
    }

    public void UpdateRotation()
    {
        Vector2 actualPosition = transform.position;
        Vector2 originPoint = transform.parent.position;

        Vector2 direction = actualPosition - originPoint;
        float angle = Vector2.SignedAngle(Vector2.right, direction);

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void UpdateSize()
    {
        Vector2 actualPosition = transform.position;
        Vector2 originPoint = transform.parent.position;

        float distance = Vector2.Distance(actualPosition, originPoint);
        finalCable.size = new Vector2(distance, finalCable.size.y);
    }

    public void Restart()
    {
        transform.position = positionOrigin;
        transform.rotation = Quaternion.identity;
        finalCable.size = sizeOrigin;
        if (light != null) light.SetActive(false);
    }

    public void TryConnection()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f); // Aumentado radio
        foreach (Collider2D col in colliders)
        {
            if (col.gameObject != gameObject && col.CompareTag("RightCable")) // Solo targets derechos
            {
                Cable otherCable = col.GetComponent<Cable>();
                if (otherCable != null && ColorsMatch(finalCable.color, otherCable.finalCable.color))
                {
                    Debug.Log($"Conexión detectada entre {gameObject.name} y {col.gameObject.name}");
                    transform.position = col.transform.position; // Snap al target
                    Connection();
                    otherCable.Connection();
                    return; // Conexión única
                }
            }
        }
        Debug.Log($"No se encontró conexión para {gameObject.name}");
    }

    private bool ColorsMatch(Color a, Color b)
    {
        return Mathf.Approximately(a.r, b.r) && Mathf.Approximately(a.g, b.g) && Mathf.Approximately(a.b, b.b);
    }

    public bool IsConnected()
    {
        return light != null && light.activeInHierarchy;
    }

    public void Connection()
    {
        if (light != null) light.SetActive(true);
        enabled = false; // Desactiva en vez de Destroy
        GetComponent<Collider2D>().enabled = false; // Evita más drags
        CableManager.Instance.CableConnected();
    }
}