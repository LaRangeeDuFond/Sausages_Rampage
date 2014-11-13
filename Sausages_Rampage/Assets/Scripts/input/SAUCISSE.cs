using UnityEngine;
using System.Collections;

public class SAUCISSE : MonoBehaviour
{

		public KeyCode jump;
		public KeyCode Up;
		public float GroundDetectionLength = 0.2f;
		public float JumpForceClamp = 2000f;
		public float JumpForceClampWhenUp = 500f;
		public float JumpForceIncrementation = 2000f;
		public float RotationJump = 1200f;
		public float RotationSpeedUp = 180f;
		public float RotationSpeedDown = 180f;
		public GameObject Root;
		float DirX, DirZ;
		Vector3 JumpDirection;
		float JumpForce = 0f;
		bool isLanding;
		bool isGrounded;
		bool isUp;
		float ZRot = 0f;

		void Update ()
		{

				//Savoir si la saucisse repose sur le sol



				if (Physics.Raycast (transform.position, Vector3.down, GroundDetectionLength)) {
						isGrounded = true;
				} else {
						isGrounded = false;
				}

				// Pour faire Bouger		
				DirX = Input.GetAxis ("Horizontal");
				DirZ = Input.GetAxis ("Vertical");

				JumpDirection = new Vector3 (DirX, 1f, DirZ);

				if (networkView.isMine) {

						if (Input.GetKey (jump) && isGrounded) {
								if (isUp) {
										if (JumpForce <= JumpForceClampWhenUp) {
												JumpForce += JumpForceIncrementation * Time.deltaTime;
										}
								} else {
										if (JumpForce <= JumpForceClamp) {
												JumpForce += JumpForceIncrementation * Time.deltaTime;
										}
								}


						}

						if (Input.GetKeyUp (jump)) {
								rigidbody.AddForce (JumpDirection * JumpForce);
								JumpForce = 0f;
						}

						//METTRE LA SAUCISSE DEBOUT

						//Savoir quand elle atterit
						if (Input.GetKeyUp (Up)) {
								isLanding = true;
						} else {
								if (isGrounded) {
										isLanding = false;
								}
						}

						//Pour qu'elle bondisse lorsqu'elle se redresse
						if (Input.GetKeyDown (Up) && isGrounded) {

								rigidbody .AddForce (Vector3.up * RotationJump);
				
						}

						//Le mouvement de Rotation


						if (Input.GetKey (Up) && !isLanding) {

								if (ZRot < 90f) {
										ZRot += RotationSpeedUp * Time.deltaTime;
								} else {
										ZRot = 90f;
										isUp = true;
								}
						} else {
								if (ZRot > 0f) {
										ZRot -= RotationSpeedDown * Time.deltaTime;
								} else {
										ZRot = 0f;
										isUp = false;
								}
						}

						transform.eulerAngles = new Vector3 (transform.eulerAngles.x, transform.eulerAngles.y, ZRot);

		
				}





		}

		void OnSerializeNetworkView (BitStream stream, NetworkMessageInfo info)
		{
				Vector3 syncPosition = Vector3.zero;
				if (stream.isWriting) {
						syncPosition = rigidbody.position;
						stream.Serialize (ref syncPosition);
				} else {
						stream.Serialize (ref syncPosition);
						rigidbody.position = syncPosition;
				}
		}
}
