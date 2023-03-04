using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChannelsDisplay : MonoBehaviour{

    public SMIS smis;

    // Update is called once per frame
    void Update(){
        for (int i = 0; i < 2; i++) {
            if (smis.channelFeedbacks[i] != null) {
                transform.GetChild(i).GetComponent<Image>().color = new Color(1, 0, 0, smis.channels[i].masterVolume);
                transform.GetChild(i).GetComponentInChildren<Text>().text = smis.channels[i].mainFrequency + "Hz";
            } else {
                transform.GetChild(i).GetComponent<Image>().color = new Color(1, 1, 1, 0.25f);
                transform.GetChild(i).GetComponentInChildren<Text>().text = "";
            }
        }
    }
}
