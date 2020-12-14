﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour 
{
    [SerializeField] float sensitivity_x = 5;
	[SerializeField] float moveSpeed = 3;
    private float prev_x;
    private float delta_x;
    private float rot_y;

    private Vector3 startPos;
    private bool isMoved;

    void Start()
    {
        prev_x = Input.mousePosition.x;
        delta_x = 0;
        rot_y = transform.rotation.eulerAngles.y + delta_x * Time.deltaTime * sensitivity_x * Player.PlayTimeScale;

        startPos = transform.position;
        isMoved = false;
    }

	void Update () 
    {
        delta_x = Input.mousePosition.x - prev_x;
        prev_x = Input.mousePosition.x;

        rot_y = transform.rotation.eulerAngles.y + delta_x * Time.deltaTime * sensitivity_x * Player.PlayTimeScale;
        rot_y = (rot_y > 180) ? rot_y - 360 : rot_y;
        transform.rotation = Quaternion.Euler(0, Mathf.Clamp(rot_y, -10, 10), 0);

        if (isMoved)// when camera is moved, move back to original position
        {
			transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime * Player.PlayTimeScale);
			if (transform.position.z >= startPos.z)
			{
				transform.position = startPos;
				isMoved = false;
			}
        }
	}

    public void Recoil(float magnitude)
    {
		transform.position = new Vector3(startPos.x, startPos.y, startPos.z - magnitude);
        isMoved = true;
    }

	public static CameraMove GetCam()
	{
		return Camera.main.transform.parent.GetComponent<CameraMove>();
	}
}