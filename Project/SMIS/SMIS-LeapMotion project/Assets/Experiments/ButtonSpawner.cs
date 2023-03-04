using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSpawner : Spawner{

    public List<GameObject> buttons;
    [Range(1, 3)]
    public int buttonCount = 2;

    public bool allowDuplicates = false;

    public override void spawn() {
        //clean up
        if (transform.childCount > 0) foreach (Transform child in transform) if (child.tag == "Spawnable") Destroy(child.gameObject);
        //spawn
        switch (buttonCount) {
            case 1: 
                    Instantiate(buttons[Random.Range(0, buttons.Count)], transform.position, Quaternion.LookRotation(-transform.up), transform);
                    break;
            case 2: {
                    List<GameObject> usableButtons = new List<GameObject>(buttons);

                    int index = Random.Range(0, usableButtons.Count);
                    Instantiate(usableButtons[index], transform.position + Vector3.right * 0.2f, Quaternion.LookRotation(-transform.up), transform);
                    if (!allowDuplicates && usableButtons.Count > 1) usableButtons.RemoveAt(index);
                    index = Random.Range(0, usableButtons.Count);
                    Instantiate(usableButtons[index], transform.position - Vector3.right * 0.2f, Quaternion.LookRotation(-transform.up), transform);
                    break;
                }
            case 3: {
                    List<GameObject> usableButtons = new List<GameObject>(buttons);

                    int index = Random.Range(0, usableButtons.Count);
                    Instantiate(usableButtons[index], transform.position, Quaternion.LookRotation(-transform.up), transform);
                    if (!allowDuplicates && usableButtons.Count > 1) usableButtons.RemoveAt(index);
                    index = Random.Range(0, usableButtons.Count);
                    Instantiate(usableButtons[index], transform.position + Vector3.right * 0.2f, Quaternion.LookRotation(-transform.up), transform);
                    if (!allowDuplicates && usableButtons.Count > 1) usableButtons.RemoveAt(index);
                    index = Random.Range(0, usableButtons.Count);
                    Instantiate(usableButtons[index], transform.position - Vector3.right * 0.2f, Quaternion.LookRotation(-transform.up), transform);
                    break;
                }
        }
    }


}
