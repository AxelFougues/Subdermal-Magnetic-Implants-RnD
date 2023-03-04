using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchableObject : MonoBehaviour{

    public TouchableMaterialProperties materialProperties;
    public bool noFeedback = false;
    public bool beingTouched = false;

    SMIS smis;
    Dictionary<int, IEnumerator> coroutine = new Dictionary<int, IEnumerator>();

    //Smoothing velocities for tactile sliding
    public List<ContactParameters> contactParameters = new List<ContactParameters>();
    RollingAverageFilter positionFilter = new RollingAverageFilter(5, 2);
    RollingAverageFilter velocityFilter = new RollingAverageFilter(1, 2);

    private void Awake() {
        smis = FindObjectOfType<SMIS>();
        contactParameters.Add(new ContactParameters()); 
        contactParameters.Add(new ContactParameters());
    }


    /*private void OnTriggerEnter(Collider other) {
        for (int channel = 0; channel < smis.virtualFingers.Count; channel++) {
            if (smis.virtualFingers[channel] == other) {
                ReliableOnTriggerExit.NotifyTriggerEnter(other, gameObject, OnTriggerExit);
                smis.startTactileFeedback(channel, this);
                positionFilter.clear(channel);
                contactParameters[channel].previousPosition = Vector3.zero;
                beingTouched = true;
            }
        }        
    }*/

    //Try to fix the "no stimuli if last shape is exited after new one is entered"
    private void OnTriggerStay(Collider other) {
        for (int channel = 0; channel < smis.virtualFingers.Count; channel++) {
            if (smis.virtualFingers[channel] == other && smis.channelFeedbacks[channel] == null) {
                ReliableOnTriggerExit.NotifyTriggerEnter(other, gameObject, OnTriggerExit);
                coroutine.Add(channel, calculatePenetration(other, channel));
                StartCoroutine(coroutine[channel]);
                if(!noFeedback)smis.startTactileFeedback(channel, this);
                positionFilter.clear(channel);
                contactParameters[channel].previousPosition = Vector3.zero;
                beingTouched = true;
            }
        }
    }


    private void OnTriggerExit(Collider other) {
        for (int channel = 0; channel < smis.virtualFingers.Count; channel++) {
            if (smis.virtualFingers[channel] == other && coroutine.ContainsKey(channel)) {
                ReliableOnTriggerExit.NotifyTriggerExit(other, gameObject);
                if (!noFeedback) smis.stopTactileFeedback(channel, this);
                StopCoroutine(coroutine[channel]);
                coroutine.Remove(channel);
                beingTouched = false;
            }
        } 
    }


    IEnumerator calculatePenetration(Collider other, int channel) {
        while (true) {
            Vector3 direction; float distance;
            if (Physics.ComputePenetration(GetComponent<Collider>(), transform.position, transform.rotation, other, other.transform.position, other.transform.rotation, out direction, out distance)) {
                contactParameters[channel].pDirection = direction.normalized;

                Debug.DrawLine(other.transform.position + contactParameters[channel].pDirection * 0.1f, other.transform.position - contactParameters[channel].pDirection * 0.1f, Color.red);

                contactParameters[channel].pDistance = distance * 1000;  // in mm
                Vector3 currentPosition = positionFilter.getValue(other.transform.position * 1000, channel); // in mm
                Vector3 velocity = (currentPosition - contactParameters[channel].previousPosition) / Time.fixedDeltaTime; //in mm/s
                contactParameters[channel].pVelocity = Vector3.Dot(velocity, contactParameters[channel].pDirection); //in mm/s
                contactParameters[channel].pPerpendicularVelocity = velocityFilter.getValue((velocity.magnitude - (contactParameters[channel].pVelocity * contactParameters[channel].pDirection).magnitude), channel); // in mm/s
                contactParameters[channel].previousPosition = currentPosition;
            }
            yield return new WaitForFixedUpdate();
        }
    }

}
