using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiCarro : MonoBehaviour
{
    public float speed = 0f;
    public float maxSpeed = 40f;
    public float angle = 40f;
    public AudioSource carEngine;

    public float deathHeight;
    bool finished = false;
    bool lapIncrease = true;
    int lapNum = 1;

    float horiz;
    float vert;

    bool lightsOn =  true;
    bool highBeams = false;
    bool highSpeed = false;
    bool blinking = false;
    bool cruiseControl = false;
    float delay = 0.5f;

    public GameObject[] lights;
    public GameObject[] beams;
    public GameObject[] rBlinkers;
    public GameObject[] lBlinkers;

    // Update is called once per frame
    void Update()
    {
        horiz = Input.GetAxisRaw("Horizontal");
        vert = Input.GetAxisRaw("Vertical");

        if (finished)
        {
            if (speed >= maxSpeed * -1)
            {
                speed -= 0.2f;
            }
        }

        else
        {
            if (!cruiseControl)
            {
                // Critical to maintain car driving integrity. Without this line of code, the entire car system breaks down and has problems such as drifting backwards.
                if (vert == 0 && speed < 0.2 && speed > -0.2)
                {
                    speed = 0;
                }
                else if (vert > 0 && speed <= maxSpeed)
                {
                    speed += 0.2f;
                }
                else if (vert < 0 && speed >= maxSpeed * -1)
                {
                    speed -= 0.2f;
                }

                // Slows down after a boost
                if (speed < maxSpeed * -1)
                {
                    speed += 0.2f;
                }
                else if (speed > maxSpeed)
                {
                    speed -= 0.2f;
                }

                // ABS system.
                if (vert < 0 && speed > 0)
                {
                    speed -= 0.4f;
                }
                else if (vert > 0 && speed < 0)
                {
                    speed += 0.4f;
                }

                // Gradual deceleration due to friction
                if (vert == 0 && speed > 0)
                {
                    speed -= 0.2f;
                }
                else if (speed < 0 && vert == 0)
                {
                    speed += 0.2f;
                }

                //if (horiz > 0 && speed == 0)
                //{
                //    speed = 1;
                //}
                //else if (horiz < 0 && speed == 0)
                //{
                //    speed = 1;
                //}
                //if (horiz == 0 && speed <= 0.2 && speed >= -0.2)
                //{
                //    speed = 0;
                //}
            }

            else
            {
                // Critical to maintain car driving integrity. Without this line of code, the entire car system breaks down and has problems such as drifting backwards.
                if (vert == 0 && speed < 0.2 && speed > -0.2)
                {
                    speed = 0;
                }
                else if (vert > 0 && speed <= maxSpeed)
                {
                    speed += 0.2f;
                }
                else if (vert < 0 && speed >= maxSpeed * -1)
                {
                    speed -= 0.2f;
                }
                // ABS system. As braking is a critical feature, we must make sure the player can brake whenever they want.
                if (vert < 0 && speed > 0)
                {
                    speed -= 0.4f;
                }
                else if (vert > 0 && speed < 0)
                {
                    speed += 0.4f;
                }
            }



            // When the player falls off of the track, we must kill them at a certain height.
            if (transform.position.y < deathHeight)
            {
                transform.position = new Vector3(0f, 12f, -33f);
                transform.eulerAngles = new Vector3(0, 0, 0);
            }

            // Turn off player lights if they want to deactivate them
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (lightsOn == true)
                {
                    foreach (GameObject light in lights)
                    {
                        light.SetActive(false);
                    }
                    lightsOn = false;
                }
                else
                {
                    foreach (GameObject light in lights)
                    {
                        light.SetActive(true);
                    }
                    lightsOn = true;
                }
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (highBeams == true)
                {
                    foreach (GameObject beam in beams)
                    {
                        beam.GetComponent<Light>().intensity = 15;
                        beam.GetComponent<Light>().range = 60;
                    }
                    highBeams = false;
                }
                else
                {
                    foreach (GameObject beam in beams)
                    {
                        beam.GetComponent<Light>().intensity = 30;
                        beam.GetComponent<Light>().range = 120;
                    }
                    highBeams = true;
                }
            }

            if (Input.GetKeyDown(KeyCode.Mouse2))
            {
                //if (highSpeed == true)
                //{
                //    maxSpeed = 40f;
                //    highSpeed = false;
                //}
                //else
                //{
                //    maxSpeed = 75f;
                //    highSpeed = true;
                //}
                cruiseControl = !cruiseControl;
            }

            // regulate the blinkers

            if (horiz < 0 && blinking == false)
            {
                StartCoroutine("BlinkLeft");
                blinking = true;
            }

            else if (horiz > 0 && blinking == false)
            {
                StartCoroutine("BlinkRight");
                blinking = true;
            }

            else if (horiz == 0 && blinking == true)
            {
                blinking = false;
                StopCoroutine("BlinkLeft");
                StopCoroutine("BlinkRight");
                foreach (GameObject blinker in lBlinkers)
                {
                    blinker.SetActive(false);
                }
                foreach (GameObject blinker in rBlinkers)
                {
                    blinker.SetActive(false);
                }
            }
        }
        //Debug.Log(transform.rotation.z + " z");

        if (transform.rotation.z >= -0.30 && transform.rotation.z <= 0.30) { }
        else
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
            //transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, 0 - transform.rotation.z, transform.rotation.w);
        }

        carEngine.pitch = Mathf.Abs(speed / 45f) + 1;
        Vector3 move = new Vector3(0f, 0f, speed);
        transform.Translate(Time.deltaTime * move);
        transform.Rotate(Vector3.up, horiz * Time.deltaTime * angle);
        
    }

    //void OnCollisionEnter(Collision collision)
    //{
    //    speed -= 5;
    //}
    //private void OnCollisionStay(Collision collision)
    //{
    //    while (!(speed < 0))
    //    {
    //        speed -= 5;
    //    }
    //}

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger Exit");
        if (other.gameObject.tag == "End")
        {
            Debug.Log("Entered End");
            if (lapNum > 3)
            {
                finished = true;
            }
            else if (lapIncrease == false)
            {
                lapNum++;
                lapIncrease = true;
            }
        }
        
        if (other.gameObject.tag == "Reactivate")
        {
            Debug.Log("Entered Reactivate Zone");
            if (lapIncrease)
            {
                lapIncrease = false;
            }
        }
    }

    public float GetSpeed()
    {
        return speed;
    }

    public float GetHoriz()
    {
        return horiz;
    }

    public int GetLaps()
    {
        return lapNum;
    }

    private IEnumerator BlinkLeft()
    {
        //Debug.Log("Left");
        while (horiz < 0)
        {
            foreach (GameObject blinker in lBlinkers)
            {
                blinker.SetActive(true);
            }
            //Debug.Log("Left On");

            yield return new WaitForSeconds(delay);

            foreach (GameObject blinker in lBlinkers)
            {
                blinker.SetActive(false);
            }
            //Debug.Log("Left Off");

            yield return new WaitForSeconds(delay);
        }
        yield return null;
    }

    private IEnumerator BlinkRight()
    {
        //Debug.Log("Right");
        while (horiz > 0)
        {
            foreach (GameObject blinker in rBlinkers)
            {
                blinker.SetActive(true);
            }
            //Debug.Log("Right On");

            yield return new WaitForSeconds(delay);

            foreach (GameObject blinker in rBlinkers)
            {
                blinker.SetActive(false);
            }
            //Debug.Log("Right Off");

            yield return new WaitForSeconds(delay);
        }
        yield return null;
    }
}
