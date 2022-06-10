using UnityEngine;

public class move : MonoBehaviour
{

	//
	// VARIABLES
	//

	public float turnSpeed = 12.0f;      // Speed of camera turning when mouse moves in along an axis
	public float panSpeed = 12.0f;       // Speed of the camera when being panned
	public float zoomSpeed = 12.0f;      // Speed of the camera going back and forth

	private Vector3 mouseOrigin;    // Position of cursor when mouse dragging starts
	private bool isPanning;     // Is the camera being panned?
	private bool isRotating;    // Is the camera being rotated?
	private bool isZooming;     // Is the camera zooming?

	Camera c;

    private void Start()
    {
		c = GetComponent<Camera>();
    }

    //
    // UPDATE
    //

    void Update()
	{
		// Get the left mouse button
		if (Input.GetMouseButtonDown(0))
		{
			// Get mouse origin
			mouseOrigin = Input.mousePosition;
			isRotating = true;
		}

		// Disable movements on button release
		if (!Input.GetMouseButton(0)) isRotating = false;

		// Rotate camera along X and Y axis
		if (isRotating)
		{
			Vector3 pos = c.ScreenToViewportPoint(Vector3.ClampMagnitude(Input.mousePosition - mouseOrigin, 25f));


			transform.RotateAround(transform.position, transform.right, -pos.y * turnSpeed);
			transform.RotateAround(transform.position, Vector3.up, pos.x * turnSpeed);
		}

		float x = Input.GetAxis("Horizontal") * panSpeed;
		float z = Input.GetAxis("Vertical") * zoomSpeed;
		Vector3 moveDir = new Vector3(x, 0f, z) * Time.deltaTime;
		transform.Translate(moveDir);
	}
}
