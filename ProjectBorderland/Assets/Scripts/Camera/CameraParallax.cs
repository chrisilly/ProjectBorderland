using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraParallax : MonoBehaviour
{
    [SerializeField] Transform _farBackground, _middleBackground, _middleBackground2, _foreGround1, _foreGround2;
    private float _lastPosX;
    private float _lastPosY;
    private float _amountToMoveX;
    private float _amountToMoveY;

    private void Start()
    {
        _lastPosX = transform.position.x;
        _lastPosY = transform.position.y;
    }

    private void FixedUpdate()
    {
        _amountToMoveX = transform.position.x - _lastPosX;
        _amountToMoveY = transform.position.y - _lastPosY;

        HorizontalParallax();
        VerticallParallax();

        _lastPosX = transform.position.x;
        _lastPosY = transform.position.y;
    }

    private void HorizontalParallax()
    {
        _farBackground.position = _farBackground.position + new Vector3(_amountToMoveX, 0f, 0f); // far back ground moves same amout as player moves

        _middleBackground.position += new Vector3(_amountToMoveX * 0.8f, 0f, 0f);  // middlegrounds moves a litte bit slower
        _middleBackground2.position += new Vector3(_amountToMoveX * 0.7f, 0f, 0f);

        _foreGround1.position += new Vector3(_amountToMoveX * 0.5f, 0f, 0f);  // foregound moves slowest
        _foreGround2.position += new Vector3(_amountToMoveX * 0.4f, 0f, 0f);
    }

    private void VerticallParallax()
    {
        //_farBackground.position = _farBackground.position + new Vector3(0f, _amountToMoveY, 0f);
        _farBackground.position += new Vector3(0f, _amountToMoveY * 0.9f, 0f);

        _middleBackground.position += new Vector3(0f, _amountToMoveY * 0.85f, 0f);
        _middleBackground2.position += new Vector3(0f, _amountToMoveY * 0.8f, 0f);

        _foreGround1.position += new Vector3(0f, _amountToMoveY * 0.65f, 0f);
        _foreGround2.position += new Vector3(0f, _amountToMoveY * 0.6f, 0f);
    }
}
