using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public bool UseDestroy;
    public float DoTime;
    private float _timer = 0;

    Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            _timer += Time.deltaTime;
            if (_timer >= DoTime)
            {
                DoDestroy();
            }
        }
        else
        {
            _timer = 0;
            _rb.velocity = Vector3.zero;
        }
    }

    private void DoDestroy()
    {
        if (UseDestroy)
        {
            Destroy(gameObject);
        }
        else
        {
            _timer = 0;
            _rb.velocity = Vector3.zero;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Environment"))
        {
            DoDestroy();
        }
    }
}
